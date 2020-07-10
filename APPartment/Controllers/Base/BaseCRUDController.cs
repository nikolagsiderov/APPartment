using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using APPartment.Core;
using APPartment.Data;
using APPartment.Enums;
using APPartment.Models;
using APPartment.Models.Declaration;
using APPartment.Services;
using APPartment.Utilities;
using APPartment.Utilities.Constants.Breadcrumbs;
using APPartment.ViewResults;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartBreadcrumbs.Attributes;

namespace APPartment.Controllers.Base
{
    public abstract class BaseCRUDController<T> : BaseAuthorizeController
        where T : class, IBaseObject
    {
        #region Context, Services and Utilities
        private readonly DataAccessContext _context;
        private HtmlRenderHelper htmlRenderHelper;
        private FileUploadService fileUploadService;
        private DataContext<T> dataContext;
        private DataContext<Chore> choreDataContext;
        private DataContext<Comment> commentDataContext;
        private DataContext<Image> imageDataContext;
        private HistoryHtmlBuilder historyHtmlBuilder;
        public BaseService<T> baseService;
        private IHttpContextAccessor contextAccessor;
        #endregion

        public BaseCRUDController(IHttpContextAccessor contextAccessor, DataAccessContext context) : base(contextAccessor, context)
        {
            _context = context;
            htmlRenderHelper = new HtmlRenderHelper(_context);
            imageDataContext = new DataContext<Image>(_context);
            fileUploadService = new FileUploadService(_context, imageDataContext);
            dataContext = new DataContext<T>(_context);
            choreDataContext = new DataContext<Chore>(_context);
            commentDataContext = new DataContext<Comment>(_context);
            historyHtmlBuilder = new HistoryHtmlBuilder(_context);
            baseService = new BaseService<T>(_context);
        }

        public abstract Expression<Func<T, bool>> FilterExpression { get; set; }
        public abstract Expression<Func<T, bool>> FuncToExpression(Func<T, bool> f);

        #region Actions
        [Breadcrumb("Base")]
        public virtual async Task<IActionResult> Index()
        {
            var predicate = FilterExpression.Compile();

            var modelObjects = _context.Set<T>().ToList().Where(predicate);

            ViewData["Statuses"] = baseService.GetStatuses(typeof(T));

            return View("_Grid", modelObjects);
        }

        [Breadcrumb(BaseCRUDBreadcrumbs.Details_Breadcrumb)]
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return new Error404NotFoundViewResult();
            }

            var model = await _context.Set<T>()
                .FirstOrDefaultAsync(m => m.Id == id);

            if (model == null)
            {
                return new Error404NotFoundViewResult();
            }

            model.Comments = GetComments(model.ObjectId);
            model.Images = GetImages(model.ObjectId);
            model.History = GetHistory(model.ObjectId);

            ViewData["Statuses"] = baseService.GetStatuses(typeof(T));

            return View("_Details", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Details,Status,IsCompleted,HouseId,ObjectId")] T model)
        {
            if (ModelState.IsValid)
            {
                model.HouseId = CurrentHouseId;
                await dataContext.SaveAsync(model, CurrentUserId, CurrentHouseId, null);
            }

            return RedirectToAction(nameof(Index));
        }

        [Breadcrumb(BaseCRUDBreadcrumbs.Edit_Breadcrumb)]
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return new Error404NotFoundViewResult();
            }

            var model = await _context.Set<T>().FindAsync(id);

            if (model == null)
            {
                return new Error404NotFoundViewResult();
            }

            model.Comments = GetComments(model.ObjectId);
            model.Images = GetImages(model.ObjectId);
            model.History = GetHistory(model.ObjectId);

            ViewData["Statuses"] = baseService.GetStatuses(typeof(T));

            return View("_Edit", model);
        }

        [Breadcrumb(BaseCRUDBreadcrumbs.Edit_Breadcrumb)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Name,Details,Status,IsCompleted,HouseId,ObjectId")] T model)
        {
            if (id != model.Id)
            {
                return new Error404NotFoundViewResult();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await dataContext.UpdateAsync(model, CurrentUserId, CurrentHouseId, null);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!baseService.ObjectExists(model.Id))
                    {
                        return new Error404NotFoundViewResult();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            model.Comments = GetComments(model.ObjectId);
            model.Images = GetImages(model.ObjectId);
            model.History = GetHistory(model.ObjectId);

            return View("_Edit", model);
        }

        public async Task<IActionResult> SetLowStatus(long? id)
        {
            if (id == null)
            {
                return new Error404NotFoundViewResult();
            }

            var model = await _context.Set<T>()
                .FirstOrDefaultAsync(m => m.Id == id);

            if (model == null)
            {
                return new Error404NotFoundViewResult();
            }

            model.Status = (int)ObjectStatus.Trivial;

            await dataContext.UpdateAsync(model, CurrentUserId, CurrentHouseId, null);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> SetHighStatus(long? id)
        {
            if (id == null)
            {
                return new Error404NotFoundViewResult();
            }

            var model = await _context.Set<T>()
                .FirstOrDefaultAsync(m => m.Id == id);

            if (model == null)
            {
                return new Error404NotFoundViewResult();
            }

            model.Status = (int)ObjectStatus.High;

            await dataContext.UpdateAsync(model, CurrentUserId, CurrentHouseId, null);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Assign(string username, long choreId)
        {
            if (choreId == null)
            {
                return new Error404NotFoundViewResult();
            }

            var model = await _context.Set<Chore>().FirstOrDefaultAsync(x => x.Id == choreId);

            if (model == null)
            {
                return new Error404NotFoundViewResult();
            }

            var userToAssign = await _context.Set<User>().FirstOrDefaultAsync(x => x.Username == username);
            var userToAssignUserId = userToAssign.UserId;

            model.AssignedToId = userToAssignUserId;
            await choreDataContext.UpdateAsync(model, CurrentUserId, CurrentHouseId, null);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return new Error404NotFoundViewResult();
            }

            var model = await _context.Set<T>()
                .FirstOrDefaultAsync(m => m.Id == id);

            if (model == null)
            {
                return new Error404NotFoundViewResult();
            }

            await dataContext.DeleteAsync(model, CurrentUserId, CurrentHouseId, null);

            return RedirectToAction(nameof(Index));
        }



        public ActionResult DeleteImage(long id)
        {
            if (id == null)
            {
                return new Error404NotFoundViewResult();
            }


            var image = _context.Images.Find(id);

            if (image == null)
            {
                return new Error404NotFoundViewResult();
            }


            string currentImage = image.Id.ToString();

            _context.Entry(image).State = EntityState.Deleted;



            if (_context.SaveChanges() > 0)
            {
                if (System.IO.File.Exists(currentImage))
                {
                    System.IO.File.Delete(currentImage);
                }
                TempData["msg"] = "Image Deleted";
            }

            return RedirectToAction(nameof(Index));
        }





        #endregion

        #region Metadata
        public List<string> GetComments(long targetId)
        {
            var comments = htmlRenderHelper.BuildComments(_context.Comments.ToList(), targetId);

            return comments;
        }

        [HttpPost]
        public async Task<IActionResult> PostComment(long targetId, string commentText)
        {
            var username = HttpContext.Session.GetString("Username");

            var comment = new Comment()
            {
                Text = commentText,
                TargetId = targetId,
                Username = username
            };

            await commentDataContext.SaveAsync(comment, CurrentUserId, CurrentHouseId, targetId);

            var result = htmlRenderHelper.BuildPostComment(comment);

            return Json(result);
        }

        [HttpPost]
        public ActionResult UploadImages(string targetIdString)
        {
            var targetId = long.Parse(targetIdString);

            bool isSavedSuccessfully = true;
            string fName = "";
            try
            {
                foreach (string fileName in HttpContext.Request.Form.Files.Select(x => x.FileName))
                {
                    IFormFile file = HttpContext.Request.Form.Files.Where(x => x.FileName == fileName).FirstOrDefault();

                    //Save file content goes here
                    fName = file.FileName;
                    if (file != null && file.Length > 0)
                    {
                        fileUploadService.UploadImage(file, targetId, CurrentUserId, CurrentHouseId);
                    }
                }
            }
            catch (Exception ex)
            {
                isSavedSuccessfully = false;
            }

            if (isSavedSuccessfully)
            {
                return Json(new { Message = fName });
            }
            else
            {
                return Json(new { Message = "Error in saving file" });
            }
        }

        public List<Image> GetImages(long targetId)
        {
            var images = _context.Images.Where(x => x.TargetId == targetId).ToList();

            return images;
        }

        public List<string> GetHistory(long targetId)
        {
            var history = _context.Audits.Where(x => x.ObjectId == targetId || x.TargetObjectId == targetId).ToList();

            var objectHistoryDisplayList = historyHtmlBuilder.BuildBaseObjectHistory(history);

            return objectHistoryDisplayList;
        }
        #endregion

        public virtual void PopulateViewData()
        { }
    }
}