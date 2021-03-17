using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using APPartment.Data.Core;
using APPartment.Data.Server.Models.Base;
using APPartment.Data.Server.Models.MetaObjects;
using APPartment.UI.Utilities;
using APPartment.UI.Utilities.Constants.Breadcrumbs;
using APPartment.UI.ViewModels;
using APPartment.Web.Services.MetaObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartBreadcrumbs.Attributes;

namespace APPartment.UI.Controllers.Base
{
    public abstract class BaseCRUDController<T> : BaseAuthorizeController
        where T : HomeBaseObject, new()
    {
        #region Context, Services and Utilities
        private HtmlRenderHelper htmlRenderHelper;
        private FileUploadService fileUploadService;
        protected BaseFacade baseFacade;
        #endregion Context, Services and Utilities

        public BaseCRUDController(IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
            baseFacade = new BaseFacade();
            htmlRenderHelper = new HtmlRenderHelper();
            fileUploadService = new FileUploadService();
        }

        public abstract Expression<Func<T, bool>> FilterExpression { get; }

        #region Actions
        [Breadcrumb("Base")]
        public virtual IActionResult Index()
        {
            var modelObjects = baseFacade.GetObjects<T>(FilterExpression);
            return View("_Grid", modelObjects);
        }

        [Breadcrumb(BaseCRUDBreadcrumbs.Details_Breadcrumb)]
        public IActionResult Details(long? id)
        {
            if (id == null)
                return new Error404NotFoundViewResult();

            var model = baseFacade.GetObject<T>((long)id);

            if (model == null)
                return new Error404NotFoundViewResult();

            model = GetClingons(model);

            return View("_Details", model);
        }


        [Breadcrumb("<i class='fas fa-plus'></i> Create")]
        public IActionResult Create()
        {
            var newModel = new T();

            return View("_Create", newModel);
        }

        [Breadcrumb("<i class='fas fa-plus'></i> Create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(T model)
        {
            if (ModelState.IsValid)
            {
                model.HomeId = (long)CurrentHomeId;
                baseFacade.Create(model, (long)CurrentUserId);

                return RedirectToAction(nameof(Index));
            }

            return View("_Create", model);
        }

        [Breadcrumb(BaseCRUDBreadcrumbs.Edit_Breadcrumb)]
        public IActionResult Edit(long? id)
        {
            if (id == null)
                return new Error404NotFoundViewResult();

            var model = baseFacade.GetObject<T>((long)id);

            if (model == null)
                return new Error404NotFoundViewResult();

            model = GetClingons(model);

            return View("_Edit", model);
        }

        [Breadcrumb(BaseCRUDBreadcrumbs.Edit_Breadcrumb)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(long id, T model)
        {
            if (id != model.Id)
                return new Error404NotFoundViewResult();

            if (ModelState.IsValid)
            {
                try
                {
                    baseFacade.Update(model, (long)CurrentUserId);
                }
                catch (Exception)
                {
                    return new Error404NotFoundViewResult();
                }

                return RedirectToAction(nameof(Index));
            }

            return View("_Edit", model);
        }

        public IActionResult Delete(long? id)
        {
            if (id == null)
                return new Error404NotFoundViewResult();

            var model = baseFacade.GetObject<T>((long)id);

            if (model == null)
                return new Error404NotFoundViewResult();

            baseFacade.Delete(model);

            return RedirectToAction(nameof(Index));
        }
        #endregion Actions

        #region Clingons
        protected T GetClingons(T model)
        {
            model.Comments = GetComments(model.ObjectId);
            model.Images = GetImages(model.ObjectId);

            return model;
        }

        #region Comments
        private List<string> GetComments(long targetObjectId)
        {
            // TODO: x.CreatedById != 0 should be handled as case when user is deleted
            var comment = baseFacade.GetObjects<Comment>(x => x.TargetObjectId == targetObjectId && x.CreatedById != 0);
            var commentsResult = htmlRenderHelper.BuildComments(comment, targetObjectId);

            return commentsResult;
        }

        [HttpPost]
        public IActionResult PostComment(long targetId, string commentText)
        {
            var comment = new Comment()
            {
                Details = commentText,
                TargetObjectId = targetId,
            };

            baseFacade.Create(comment, (long)CurrentUserId);

            var result = htmlRenderHelper.BuildPostComment(comment);
            return Json(result);
        }
        #endregion Comments

        #region Images
        [HttpPost]
        public ActionResult UploadImages(string targetObjectIdString)
        {
            var targetObjectId = long.Parse(targetObjectIdString);

            bool isSavedSuccessfully = true;
            string fName = "";
            try
            {
                foreach (string fileName in HttpContext.Request.Form.Files.Select(x => x.FileName))
                {
                    IFormFile file = HttpContext.Request.Form.Files.Where(x => x.FileName == fileName).FirstOrDefault();

                    fName = file.FileName;
                    if (file != null && file.Length > 0)
                        fileUploadService.UploadImage(file, targetObjectId, (long)CurrentUserId);
                }
            }
            catch (Exception ex)
            {
                isSavedSuccessfully = false;
            }

            if (isSavedSuccessfully)
                return Json(new { Message = fName });
            else
                return Json(new { Message = "Error in saving file" });
        }

        public ActionResult DeleteImage(long id)
        {
            var image = baseFacade.GetObject<Image>(id);

            if (image == null)
                return Json(new { success = false, message = "404: Image does not exist." });
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

                    baseFacade.Delete(image);

                    return Json(new { success = true, message = "Image deleted successfully." });
                }
                else
                    return Json(new { success = false, message = "404: Images path does not exist." });
            }
        }

        private List<Image> GetImages(long targetObjectId)
        {
            var images = baseFacade.GetObjects<Image>(x => x.TargetObjectId == targetObjectId);
            return images;
        }
        #endregion Images
        #endregion Clingons

        protected virtual void PopulateViewData()
        { }
    }
}