using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Threading.Tasks;
using APPartment.ORM.Framework;
using APPartment.UI.Services;
using APPartment.UI.Utilities;
using APPartment.UI.Utilities.Constants.Breadcrumbs;
using APPartment.UI.ViewModels;
using APPartment.UI.ViewModels.Base;
using APPartment.UI.ViewModels.Clingons.Comment;
using APPartment.UI.ViewModels.Clingons.Image;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
            htmlRenderHelper = new HtmlRenderHelper(CurrentUserID);
            fileUploadService = new FileUploadService(CurrentUserID);
        }

        public abstract Expression<Func<T, bool>> FilterExpression { get; }

        public virtual void SetGridItemActions(T model)
        {
            model.ActionsHtml.Add(GridItemActionBuilder.BuildDetailsAction(CurrentAreaName, CurrentControllerName, model.ID));

            if (CanManage)
            {
                model.ActionsHtml.Add(GridItemActionBuilder.BuildEditAction(CurrentAreaName, CurrentControllerName, model.ID));
                model.ActionsHtml.Add(GridItemActionBuilder.BuildDeleteAction(CurrentAreaName, CurrentControllerName, model.ID));
            }
        }

        public abstract bool CanManage { get; }

        [Breadcrumb("Base")]
        public virtual async Task<IActionResult> Index()
        {
            List<T> models = new List<T>();

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("CurrentUserID", CurrentUserID.ToString());

                using (var response = await httpClient.GetAsync($"{Configuration.DefaultAPI}/home/{CurrentHomeID}/{CurrentAreaName}/{CurrentControllerName}"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();

                    if (!apiResponse.Contains("\"title\":\"Not Found\",\"status\":404"))
                        models = JsonConvert.DeserializeObject<List<T>>(apiResponse);
                }
            }

            ViewData["CanManage"] = CanManage;

            foreach (var model in models)
            {
                SetGridItemActions(model);
            }

            return View("_Grid", models);
        }

        [Breadcrumb(BaseCRUDBreadcrumbs.Details_Breadcrumb)]
        public async Task<IActionResult> Details(long? ID)
        {
            if (ID == null)
                return new Error404NotFoundViewResult();

            var model = new U();

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"{Configuration.DefaultAPI}/home/{CurrentHomeID}/{CurrentAreaName}/{(long)ID}"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();

                    if (!apiResponse.Contains("\"title\":\"Not Found\",\"status\":404"))
                        model = JsonConvert.DeserializeObject<U>(apiResponse);
                }
            }

            if (model.ID > 0)
            {
                model = GetClingons(model);
                ViewData["CanManage"] = CanManage;

                return View("_Details", model);
            }
            else
                return new Error404NotFoundViewResult();
        }


        [Breadcrumb("<i class='fas fa-plus'></i> Create")]
        public IActionResult Create()
        {
            var newModel = new U();
            return View("_Edit", newModel);
        }

        [Breadcrumb("<i class='fas fa-plus'></i> Create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(U model)
        {
            if (ModelState.IsValid)
            {
                model.HomeID = (long)CurrentHomeID;
                BaseWebService.Save(model);

                return RedirectToAction(nameof(Index));
            }

            return View("_Edit", model);
        }

        [Breadcrumb(BaseCRUDBreadcrumbs.Edit_Breadcrumb)]
        public async Task<IActionResult> Edit(long? ID)
        {
            if (ID == null)
                return new Error404NotFoundViewResult();

            var model = new U();

            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync($"{Configuration.DefaultAPI}/home/{CurrentHomeID}/{CurrentAreaName}/{(long)ID}"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();

                    if (!apiResponse.Contains("\"title\":\"Not Found\",\"status\":404"))
                        model = JsonConvert.DeserializeObject<U>(apiResponse);
                }
            }

            if (model.ID > 0)
            {
                model = GetClingons(model);
                return View("_Edit", model);
            }
            else
                return new Error404NotFoundViewResult();
        }

        [Breadcrumb(BaseCRUDBreadcrumbs.Edit_Breadcrumb)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(long ID, U model)
        {
            if (ID != model.ID)
                return new Error404NotFoundViewResult();

            if (ModelState.IsValid)
            {
                BaseWebService.Save(model);
                return RedirectToAction(nameof(Index));
            }

            return View("_Edit", model);
        }

        public IActionResult Delete(long? ID)
        {
            if (ID == null)
                return new Error404NotFoundViewResult();

            var model = BaseWebService.GetEntity<U>((long)ID);

            if (model == null)
                return new Error404NotFoundViewResult();

            BaseWebService.Delete(model);

            return RedirectToAction(nameof(Index));
        }

        public async Task<JsonResult> GetCount()
        {
            int count = 0;

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("CurrentUserID", CurrentUserID.ToString());

                using (var response = await httpClient.GetAsync($"{Configuration.DefaultAPI}/home/{CurrentHomeID}/{CurrentAreaName}/{CurrentControllerName}/count"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();

                    if (!apiResponse.Contains("\"title\":\"Not Found\",\"status\":404"))
                        count = JsonConvert.DeserializeObject<int>(apiResponse);
                }
            }

            return Json(count);
        }

        #region Clingons
        protected U GetClingons(U model)
        {
            model.Comments = GetComments(model.ObjectID);
            model.Images = GetImages(model.ObjectID);

            return model;
        }

        #region Comments
        private List<string> GetComments(long targetObjectID)
        {
            // TODO: x.CreatedById != 0 should be handled as case when user is deleted
            var comment = BaseWebService.GetCollection<CommentPostViewModel>(x => x.TargetObjectID == targetObjectID && x.CreatedByID != 0);
            var commentsResult = htmlRenderHelper.BuildComments(comment);

            return commentsResult;
        }

        [HttpPost]
        public IActionResult PostComment(long targetID, string commentText)
        {
            var comment = new CommentPostViewModel()
            {
                Details = commentText,
                TargetObjectID = targetID,
            };

            BaseWebService.Save(comment);

            var result = htmlRenderHelper.BuildPostComment(comment);
            return Json(result);
        }
        #endregion Comments

        #region Images
        [HttpPost]
        public ActionResult UploadImages(string targetObjectIDString)
        {
            var targetObjectID = long.Parse(targetObjectIDString);

            bool isSavedSuccessfully = true;
            string fName = "";
            try
            {
                foreach (string fileName in HttpContext.Request.Form.Files.Select(x => x.FileName))
                {
                    IFormFile file = HttpContext.Request.Form.Files.Where(x => x.FileName == fileName).FirstOrDefault();

                    fName = file.FileName;
                    if (file != null && file.Length > 0)
                        fileUploadService.UploadImage(file, targetObjectID, (long)CurrentUserID);
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

        public ActionResult DeleteImage(long ID)
        {
            var image = BaseWebService.GetEntity<ImagePostViewModel>(ID);

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

        private List<ImagePostViewModel> GetImages(long targetObjectID)
        {
            var images = BaseWebService.GetCollection<ImagePostViewModel>(x => x.TargetObjectID == targetObjectID);
            return images;
        }
        #endregion Images
        #endregion Clingons

        protected virtual void PopulateViewData()
        { }
    }
}