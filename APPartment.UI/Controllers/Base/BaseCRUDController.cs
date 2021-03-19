using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using APPartment.UI.Services;
using APPartment.UI.Utilities;
using APPartment.UI.Utilities.Constants.Breadcrumbs;
using APPartment.UI.ViewModels;
using APPartment.UI.ViewModels.Base;
using APPartment.UI.ViewModels.Comment;
using APPartment.UI.ViewModels.Image;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartBreadcrumbs.Attributes;

namespace APPartment.UI.Controllers.Base
{
    public abstract class BaseCRUDController<T, U> : BaseAuthorizeController
        where T : GridItemViewModelWithHome, new()
        where U : PostViewModelWithHome, new()
    {
        #region Services and Utilities
        private HtmlRenderHelper htmlRenderHelper;
        private FileUploadService fileUploadService;
        #endregion Services and Utilities

        public BaseCRUDController(IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
            htmlRenderHelper = new HtmlRenderHelper(CurrentUserId);
            fileUploadService = new FileUploadService(CurrentUserId);
        }

        public abstract Expression<Func<T, bool>> FilterExpression { get; }

        [Breadcrumb("Base")]
        public virtual IActionResult Index()
        {
            var models = BaseWebService.GetCollection<T>(FilterExpression);
            return View("_Grid", models);
        }

        [Breadcrumb(BaseCRUDBreadcrumbs.Details_Breadcrumb)]
        public IActionResult Details(long? id)
        {
            if (id == null)
                return new Error404NotFoundViewResult();

            var model = BaseWebService.GetEntity<U>((long)id);

            if (model == null)
                return new Error404NotFoundViewResult();

            model = GetClingons(model);

            return View("_Details", model);
        }


        [Breadcrumb("<i class='fas fa-plus'></i> Create")]
        public IActionResult Create()
        {
            var newModel = new U();
            return View("_Create", newModel);
        }

        [Breadcrumb("<i class='fas fa-plus'></i> Create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(U model)
        {
            if (ModelState.IsValid)
            {
                model.HomeId = (long)CurrentHomeId;
                BaseWebService.Save(model);

                return RedirectToAction(nameof(Index));
            }

            return View("_Create", model);
        }

        [Breadcrumb(BaseCRUDBreadcrumbs.Edit_Breadcrumb)]
        public IActionResult Edit(long? id)
        {
            if (id == null)
                return new Error404NotFoundViewResult();

            var model = BaseWebService.GetEntity<U>((long)id);

            if (model == null)
                return new Error404NotFoundViewResult();

            model = GetClingons(model);

            return View("_Edit", model);
        }

        [Breadcrumb(BaseCRUDBreadcrumbs.Edit_Breadcrumb)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(long id, U model)
        {
            if (id != model.Id)
                return new Error404NotFoundViewResult();

            if (ModelState.IsValid)
            {
                BaseWebService.Save(model);
                return RedirectToAction(nameof(Index));
            }

            return View("_Edit", model);
        }

        public IActionResult Delete(long? id)
        {
            if (id == null)
                return new Error404NotFoundViewResult();

            var model = BaseWebService.GetEntity<U>((long)id);

            if (model == null)
                return new Error404NotFoundViewResult();

            BaseWebService.Delete(model);

            return RedirectToAction(nameof(Index));
        }

        #region Clingons
        protected U GetClingons(U model)
        {
            model.Comments = GetComments(model.ObjectId);
            model.Images = GetImages(model.ObjectId);

            return model;
        }

        #region Comments
        private List<string> GetComments(long targetObjectId)
        {
            // TODO: x.CreatedById != 0 should be handled as case when user is deleted
            var comment = BaseWebService.GetCollection<CommentPostViewModel>(x => x.TargetObjectId == targetObjectId && x.CreatedById != 0);
            var commentsResult = htmlRenderHelper.BuildComments(comment, targetObjectId);

            return commentsResult;
        }

        [HttpPost]
        public IActionResult PostComment(long targetId, string commentText)
        {
            var comment = new CommentPostViewModel()
            {
                Details = commentText,
                TargetObjectId = targetId,
            };

            BaseWebService.Save(comment);

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
            var image = BaseWebService.GetEntity<ImagePostViewModel>(id);

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

                    BaseWebService.Delete(image);

                    return Json(new { success = true, message = "Image deleted successfully." });
                }
                else
                    return Json(new { success = false, message = "404: Images path does not exist." });
            }
        }

        private List<ImagePostViewModel> GetImages(long targetObjectId)
        {
            var images = BaseWebService.GetCollection<ImagePostViewModel>(x => x.TargetObjectId == targetObjectId);
            return images;
        }
        #endregion Images
        #endregion Clingons

        protected virtual void PopulateViewData()
        { }
    }
}