using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using APPartment.Data.Core;
using APPartment.Data.Server.Models.Core;
using APPartment.Data.Server.Declarations;
using APPartment.Data.Server.Models.MetaObjects;
using APPartment.Data.Server.Models.Objects;
using APPartment.ORM.Framework.Core;
using APPartment.UI.Enums;
using APPartment.UI.Utilities;
using APPartment.UI.Utilities.Constants.Breadcrumbs;
using APPartment.UI.ViewModels;
using APPartment.Web.Services.MetaObjects;
using APPartment.Web.Services.Objects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartBreadcrumbs.Attributes;

namespace APPartment.UI.Controllers.Base
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
        protected BaseService<T> baseService;
        #endregion Context, Services and Utilities

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

            model = GetClingons(model);
            ViewData["Statuses"] = baseService.GetStatuses(typeof(T));

            return View("_Details", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(T model)
        {
            if (ModelState.IsValid)
            {
                model.HomeId = CurrentHomeId;
                await dataContext.SaveAsync(model, CurrentUserId, CurrentHomeId, null);
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

            model = GetClingons(model);
            ViewData["Statuses"] = baseService.GetStatuses(typeof(T));

            return View("_Edit", model);
        }

        [Breadcrumb(BaseCRUDBreadcrumbs.Edit_Breadcrumb)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, T model)
        {
            if (id != model.Id)
            {
                return new Error404NotFoundViewResult();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await dataContext.UpdateAsync(model, CurrentUserId, CurrentHomeId, null);
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

            await dataContext.UpdateAsync(model, CurrentUserId, CurrentHomeId, null);

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

            await dataContext.UpdateAsync(model, CurrentUserId, CurrentHomeId, null);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Assign(string username, long choreId)
        {
            var model = await _context.Set<Chore>().FirstOrDefaultAsync(x => x.Id == choreId);

            if (model == null)
            {
                return new Error404NotFoundViewResult();
            }

            var userToAssign = await _context.Set<User>().FirstOrDefaultAsync(x => x.Username == username);
            var userToAssignUserId = userToAssign.UserId;

            model.AssignedToId = userToAssignUserId;
            await choreDataContext.UpdateAsync(model, CurrentUserId, CurrentHomeId, null);

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

            await dataContext.DeleteAsync(model, CurrentUserId, CurrentHomeId, null);

            return RedirectToAction(nameof(Index));
        }
        #endregion Actions

        #region Clingons
        protected T GetClingons(T model)
        {
            model.Comments = GetComments(model.ObjectId);
            model.Images = GetImages(model.ObjectId);
            model.History = GetHistory(model.ObjectId);

            return model;
        }

        #region Comments
        private List<string> GetComments(long targetId)
        {
            var comments = htmlRenderHelper.BuildComments(_context.Comments.ToList(), targetId);

            return comments;
        }

        [HttpPost]
        public async Task<IActionResult> PostComment(long targetId, string commentText)
        {
            var comment = new Comment()
            {
                Text = commentText,
                TargetId = targetId,
                Username = CurrentUserName
            };

            await commentDataContext.SaveAsync(comment, CurrentUserId, CurrentHomeId, targetId);

            var result = htmlRenderHelper.BuildPostComment(comment);
            return Json(result);
        }
        #endregion Comments

        #region Images
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

                    fName = file.FileName;
                    if (file != null && file.Length > 0)
                    {
                        fileUploadService.UploadImage(file, targetId, CurrentUserId, CurrentHomeId);
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

        public ActionResult DeleteImage(long id)
        {
            var image = _context.Images.Find(id);

            if (image == null)
            {
                return Json(new { success = false, message = "404: Image does not exist." });
            }
            else
            {
                if (Directory.Exists(ImagesPath))
                {
                    var di = new DirectoryInfo(ImagesPath);
                    foreach (var file in di.GetFiles())
                    {
                        if (file.Name == image.Name)
                        {
                            file.Delete();
                            break;
                        }
                    }

                    _context.Images.Remove(image);
                    _context.SaveChanges();

                    return Json(new { success = true, message = "Image deleted successfully." });
                }
                else
                {
                    return Json(new { success = false, message = "404: Images path does not exist." });
                }   
            }
        }

        private List<Image> GetImages(long targetId)
        {
            var images = _context.Images.Where(x => x.TargetId == targetId).ToList();

            return images;
        }
        #endregion Images

        #region History
        private List<string> GetHistory(long targetId)
        {
            var history = _context.Audits.Where(x => x.ObjectId == targetId || x.TargetObjectId == targetId).ToList();

            var objectHistoryDisplayList = historyHtmlBuilder.BuildBaseObjectHistory(history);

            return objectHistoryDisplayList;
        }
        #endregion History
        #endregion Clingons

        protected virtual void PopulateViewData()
        { }
    }
}