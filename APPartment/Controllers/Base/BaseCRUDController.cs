using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using APPartment.Core;
using APPartment.Data;
using APPartment.Models;
using APPartment.Models.Declaration;
using APPartment.Services;
using APPartment.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartBreadcrumbs.Attributes;

namespace APPartment.Controllers.Base
{
    public abstract class BaseCRUDController<T> : Controller
        where T : class, IBaseObject
    {
        private const string Details_Breadcrumb = "<i class='fas fa-info-circle'></i> Details";
        private const string Edit_Breadcrumb = "<i class='fas fa-edit'></i> Edit";

        private readonly DataAccessContext _context;
        private HtmlRenderHelper htmlRenderHelper = new HtmlRenderHelper();
        private FileUploadService fileUploadService = new FileUploadService();
        private DataContext<T> dataContext = new DataContext<T>();
        private DataContext<Comment> commentDataContext = new DataContext<Comment>();
        private DataContext<Image> imageDataContext = new DataContext<Image>();
        private HistoryHtmlBuilder historyHtmlBuilder = new HistoryHtmlBuilder();

        public BaseCRUDController(DataAccessContext context)
        {
            _context = context;
        }

        [Breadcrumb("Base")]
        public virtual async Task<IActionResult> Index()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("HouseId")))
            {
                return RedirectToAction("Login", "Account");
            }

            var currentHouseId = long.Parse(HttpContext.Session.GetString("HouseId"));

            var modelObjects = await _context.Set<T>().Where(x => x.HouseId == currentHouseId).ToListAsync();

            ViewData["Manage"] = true;

            return View("_Grid", modelObjects);
        }

        [Breadcrumb("Base")]
        public virtual async Task<IActionResult> IndexCompleted()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("HouseId")))
            {
                return RedirectToAction("Login", "Account");
            }

            var currentHouseId = long.Parse(HttpContext.Session.GetString("HouseId"));

            var modelObjects = await _context.Set<T>().Where(x => x.HouseId == currentHouseId && x.IsCompleted == true).ToListAsync();

            ViewData["Manage"] = false;

            return View("_Grid", modelObjects);
        }

        [Breadcrumb("Base")]
        public virtual async Task<IActionResult> IndexNotCompleted()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("HouseId")))
            {
                return RedirectToAction("Login", "Account");
            }

            var currentHouseId = long.Parse(HttpContext.Session.GetString("HouseId"));

            var modelObjects = await _context.Set<T>().Where(x => x.HouseId == currentHouseId && x.IsCompleted == false).ToListAsync();

            ViewData["Manage"] = false;

            return View("_Grid", modelObjects);
        }

        [Breadcrumb(Details_Breadcrumb)]
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await _context.Set<T>()
                .FirstOrDefaultAsync(m => m.Id == id);

            if (model == null)
            {
                return NotFound();
            }

            model.Comments = GetComments(model.ObjectId);
            model.Images = GetImages(model.ObjectId);
            model.History = GetHistory(model.ObjectId);

            return View("_Details", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Details,Status,IsCompleted,HouseId,ObjectId")] T model)
        {
            if (ModelState.IsValid)
            {
                var currentHouseId = long.Parse(HttpContext.Session.GetString("HouseId"));
                var currentUserId = long.Parse(HttpContext.Session.GetString("UserId"));

                model.HouseId = currentHouseId;

                await dataContext.SaveAsync(model, _context, currentUserId, null, currentHouseId);
            }

            return RedirectToAction(nameof(Index));
        }

        [Breadcrumb(Edit_Breadcrumb)]
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var model = await _context.Set<T>().FindAsync(id);

            if (model == null)
            {
                return NotFound();
            }

            model.Comments = GetComments(model.ObjectId);
            model.Images = GetImages(model.ObjectId);
            model.History = GetHistory(model.ObjectId);

            return View("_Edit", model);
        }

        [Breadcrumb(Edit_Breadcrumb)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Name,Details,Status,IsCompleted,HouseId,ObjectId")] T model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var currentHouseId = long.Parse(HttpContext.Session.GetString("HouseId"));
                var currentUserId = long.Parse(HttpContext.Session.GetString("UserId"));

                try
                {
                    await dataContext.UpdateAsync(model, _context, currentUserId, currentHouseId);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ObjectExists(model.Id))
                    {
                        return NotFound();
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

        public async Task<IActionResult> Complete(long? id)
        {
            var currentUserId = long.Parse(HttpContext.Session.GetString("UserId"));
            var currentHouseId = long.Parse(HttpContext.Session.GetString("HouseId"));

            if (id == null)
            {
                return NotFound();
            }

            var model = await _context.Set<T>()
                .FirstOrDefaultAsync(m => m.Id == id);

            if (model == null)
            {
                return NotFound();
            }

            model.IsCompleted = true;

            await dataContext.UpdateAsync(model, _context, currentUserId, currentHouseId);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> MarkNotCompleted(long? id)
        {
            var currentUserId = long.Parse(HttpContext.Session.GetString("UserId"));
            var currentHouseId = long.Parse(HttpContext.Session.GetString("UserId"));

            if (id == null)
            {
                return NotFound();
            }

            var model = await _context.Set<T>()
                .FirstOrDefaultAsync(m => m.Id == id);

            if (model == null)
            {
                return NotFound();
            }

            model.IsCompleted = false;

            await dataContext.UpdateAsync(model, _context, currentUserId, currentHouseId);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(long? id)
        {
            var currentUserId = long.Parse(HttpContext.Session.GetString("UserId"));
            var currentHouseId = long.Parse(HttpContext.Session.GetString("HouseId"));

            if (id == null)
            {
                return NotFound();
            }

            var model = await _context.Set<T>()
                .FirstOrDefaultAsync(m => m.Id == id);

            if (model == null)
            {
                return NotFound();
            }

            await dataContext.DeleteAsync(model, _context, currentUserId, null, currentHouseId);

            return RedirectToAction(nameof(Index));
        }

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
            var currentUserId = long.Parse(HttpContext.Session.GetString("UserId"));
            var currentHouseId = long.Parse(HttpContext.Session.GetString("HouseId"));

            var comment = new Comment()
            {
                Text = commentText,
                TargetId = targetId,
                Username = username
            };

            await commentDataContext.SaveAsync(comment, _context, currentUserId, targetId, currentHouseId);

            var result = htmlRenderHelper.BuildPostComment(comment);

            return Json(result);
        }

        [HttpPost]
        public ActionResult UploadImages(string targetIdString)
        {
            var targetId = long.Parse(targetIdString);
            var currentUserId = long.Parse(HttpContext.Session.GetString("UserId"));
            var currentHouseId = long.Parse(HttpContext.Session.GetString("HouseId"));

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
                        fileUploadService.UploadImage(file, _context, targetId, imageDataContext, currentUserId, currentHouseId);
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
            var history = _context.Histories.Where(x => x.ObjectId == targetId || x.TargetId == targetId).ToList();

            var objectHistoryDisplayList = historyHtmlBuilder.BuildBaseObjectHistory(history, _context);

            return objectHistoryDisplayList;
        }
        #endregion

        private bool ObjectExists(long id)
        {
            return _context.Set<T>().Any(e => e.Id == id);
        }

        public virtual void PopulateViewData()
        { }
    }
}