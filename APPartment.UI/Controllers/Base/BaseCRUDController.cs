using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net.Http;
using System.Threading.Tasks;
using APPartment.Common;
using APPartment.UI.Constants.Breadcrumbs;
using APPartment.UI.Html;
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
        public BaseCRUDController(IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
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
        [HttpGet]
        public virtual async Task<IActionResult> Index()
        {
            List<T> models = new List<T>();

            using (var httpClient = new HttpClient())
            {
                var requestUri = $"{Configuration.DefaultAPI}/home/{CurrentHomeID}/{CurrentAreaName}/{CurrentControllerName}";
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("CurrentUserID", CurrentUserID.ToString());

                using (var response = await httpClient.GetAsync(requestUri))
                {
                    var content = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                        models = JsonConvert.DeserializeObject<List<T>>(content);
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
        [HttpGet]
        public async Task<IActionResult> Details(long? ID)
        {
            if (ID == null)
                return new Error404NotFoundViewResult();

            var model = new U();

            using (var httpClient = new HttpClient())
            {
                var requestUri = $"{Configuration.DefaultAPI}/home/{CurrentHomeID}/{CurrentAreaName}/{(long)ID}";
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("CurrentUserID", CurrentUserID.ToString());

                using (var response = await httpClient.GetAsync(requestUri))
                {
                    string content = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                        model = JsonConvert.DeserializeObject<U>(content);
                }
            }

            if (model.ID > 0)
            {
                model = await GetClingons(model);
                ViewData["CanManage"] = CanManage;

                return View("_Details", model);
            }
            else
                return new Error404NotFoundViewResult();
        }


        [Breadcrumb("<i class='fas fa-plus'></i> Create")]
        [HttpGet]
        public IActionResult Create()
        {
            var newModel = new U();
            return View("_Edit", newModel);
        }

        [Breadcrumb("<i class='fas fa-plus'></i> Create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(U model)
        {
            if (ModelState.IsValid)
            {
                using (var httpClient = new HttpClient())
                {
                    var requestUri = $"{Configuration.DefaultAPI}/home/{CurrentHomeID}/{CurrentAreaName}/createedit";
                    httpClient.DefaultRequestHeaders.Add("CurrentUserID", CurrentUserID.ToString());
                    httpClient.DefaultRequestHeaders.Add("CurrentHomeID", CurrentHomeID.ToString());

                    using (var response = await httpClient.PostAsJsonAsync(requestUri, model))
                    {
                        var content = await response.Content.ReadAsStringAsync();

                        if (response.IsSuccessStatusCode)
                            return RedirectToAction(nameof(Index));
                        else
                            return View("_Edit", model);
                    }
                }
            }

            return View("_Edit", model);
        }

        [Breadcrumb(BaseCRUDBreadcrumbs.Edit_Breadcrumb)]
        [HttpGet]
        public async Task<IActionResult> Edit(long? ID)
        {
            if (ID == null)
                return new Error404NotFoundViewResult();

            var model = new U();

            using (var httpClient = new HttpClient())
            {
                var requestUri = $"{Configuration.DefaultAPI}/home/{CurrentHomeID}/{CurrentAreaName}/{(long)ID}";
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("CurrentUserID", CurrentUserID.ToString());

                using (var response = await httpClient.GetAsync(requestUri))
                {
                    string content = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                        model = JsonConvert.DeserializeObject<U>(content);
                }
            }

            if (model.ID > 0)
            {
                model = await GetClingons(model);
                return View("_Edit", model);
            }
            else
                return new Error404NotFoundViewResult();
        }

        [Breadcrumb(BaseCRUDBreadcrumbs.Edit_Breadcrumb)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long ID, U model)
        {
            if (ID != model.ID)
                return new Error404NotFoundViewResult();

            if (ModelState.IsValid)
            {
                using (var httpClient = new HttpClient())
                {
                    var requestUri = $"{Configuration.DefaultAPI}/home/{CurrentHomeID}/{CurrentAreaName}/createedit";
                    httpClient.DefaultRequestHeaders.Add("CurrentUserID", CurrentUserID.ToString());
                    httpClient.DefaultRequestHeaders.Add("CurrentHomeID", CurrentHomeID.ToString());

                    using (var response = await httpClient.PostAsJsonAsync(requestUri, model))
                    {
                        var content = await response.Content.ReadAsStringAsync();

                        if (response.IsSuccessStatusCode)
                            return RedirectToAction(nameof(Index));
                        else
                            return View("_Edit", model);
                    }
                }
            }

            return View("_Edit", model);
        }

        public async Task<IActionResult> Delete(long? ID)
        {
            if (ID == null)
                return new Error404NotFoundViewResult();

            var model = new U();

            using (var httpClient = new HttpClient())
            {
                var requestUri = $"{Configuration.DefaultAPI}/home/{CurrentHomeID}/{CurrentAreaName}/{(long)ID}";
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("CurrentUserID", CurrentUserID.ToString());

                using (var response = await httpClient.GetAsync(requestUri))
                {
                    string content = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                        model = JsonConvert.DeserializeObject<U>(content);
                }
            }

            if (model.ID > 0)
            {
                using (var httpClient = new HttpClient())
                {
                    var requestUri = $"{Configuration.DefaultAPI}/home/{CurrentHomeID}/{CurrentAreaName}/delete/{(long)ID}";
                    httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("CurrentUserID", CurrentUserID.ToString());

                    using (var response = await httpClient.GetAsync(requestUri))
                    {
                        string content = await response.Content.ReadAsStringAsync();

                        if (response.IsSuccessStatusCode)
                            return RedirectToAction(nameof(Index));
                        else
                            return new Error404NotFoundViewResult();
                    }
                }
            }
            else
                return new Error404NotFoundViewResult();
        }

        public async Task<JsonResult> GetCount()
        {
            int count = 0;

            using (var httpClient = new HttpClient())
            {
                var requestUri = $"{Configuration.DefaultAPI}/home/{CurrentHomeID}/{CurrentAreaName}/{CurrentControllerName}/count";
                httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("CurrentUserID", CurrentUserID.ToString());

                using (var response = await httpClient.GetAsync(requestUri))
                {
                    string content = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                        count = JsonConvert.DeserializeObject<int>(content);
                }
            }

            return Json(count);
        }

        #region Clingons
        protected async Task<U> GetClingons(U model)
        {
            model.Comments = await GetComments(model.ObjectID);
            model.Images = await GetImages(model.ObjectID);

            return model;
        }

        #region Comments
        private async Task<List<string>> GetComments(long targetObjectID)
        {
            var result = new List<string>();

            using (var httpClient = new HttpClient())
            {
                var requestUri = $"{Configuration.DefaultAPI}/comments/{targetObjectID}";
                httpClient.DefaultRequestHeaders.Add("CurrentUserID", CurrentUserID.ToString());
                httpClient.DefaultRequestHeaders.Add("CurrentHomeID", CurrentHomeID.ToString());

                using (var response = await httpClient.GetAsync(requestUri))
                {
                    string content = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                        result = JsonConvert.DeserializeObject<List<string>>(content);
                }
            }

            return result;
        }

        [HttpPost]
        public async Task<IActionResult> PostComment(long targetID, string commentText)
        {
            var result = string.Empty;
            var comment = new CommentPostViewModel()
            {
                Name = "none",
                Details = commentText,
                TargetObjectID = targetID
            };

            ModelState.Clear();

            using (var httpClient = new HttpClient())
            {
                var requestUri = $"{Configuration.DefaultAPI}/comments/post";
                httpClient.DefaultRequestHeaders.Add("CurrentUserID", CurrentUserID.ToString());
                httpClient.DefaultRequestHeaders.Add("CurrentHomeID", CurrentHomeID.ToString());

                using (var response = await httpClient.PostAsJsonAsync(requestUri, comment))
                {
                    string content = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                        result = content;
                }
            }

            return Json(result);
        }
        #endregion Comments

        #region Images
        [HttpPost]
        public ActionResult UploadImages(string targetObjectIDString)
        {
            // TODO: This here needs to send request to API
            // Also, make sure we store images outside of wwwroot
            //var targetObjectID = long.Parse(targetObjectIDString);

            //bool isSavedSuccessfully = true;
            //string fName = "";
            //try
            //{
            //    foreach (string fileName in HttpContext.Request.Form.Files.Select(x => x.FileName))
            //    {
            //        IFormFile file = HttpContext.Request.Form.Files.Where(x => x.FileName == fileName).FirstOrDefault();

            //        fName = file.FileName;
            //        if (file != null && file.Length > 0)
            //            fileUploadService.UploadImage(file, targetObjectID, (long)CurrentUserID);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    isSavedSuccessfully = false;
            //}

            //if (isSavedSuccessfully)
            //    return Json(new { Message = fName });
            //else
                return Json(new { Message = "Error in saving file" });
        }

        public async Task<ActionResult> DeleteImage(long ID)
        {
            var image = new ImagePostViewModel();

            using (var httpClient = new HttpClient())
            {
                var requestUri = $"{Configuration.DefaultAPI}/images/image/{ID}";
                httpClient.DefaultRequestHeaders.Add("CurrentUserID", CurrentUserID.ToString());
                httpClient.DefaultRequestHeaders.Add("CurrentHomeID", CurrentHomeID.ToString());

                using (var response = await httpClient.GetAsync(requestUri))
                {
                    string content = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                        image = JsonConvert.DeserializeObject<ImagePostViewModel>(content);
                }
            }

            if (image == null)
                return Json(new { success = false, message = "404: Image does not exist." });
            else
            {
                return Json(new { success = false, message = "404: Image does not exist." });

                // TODO: This here needs to send request to API
                // Also, make sure directory path is outside of wwwroot
                //if (Directory.Exists(ImagesPath))
                //{
                //    var di = new DirectoryInfo(ImagesPath);
                //    foreach (var file in di.GetFiles())
                //    {
                //        if (file.Name == image.Name)
                //        {
                //            file.Delete();
                //            break;
                //        }
                //    }

                //    BaseWebService.Delete(image);

                //    return Json(new { success = true, message = "Image deleted successfully." });
                //}
                //else
                //    return Json(new { success = false, message = "404: Images path does not exist." });
            }
        }

        private async Task<List<ImagePostViewModel>> GetImages(long targetObjectID)
        {
            var result = new List<ImagePostViewModel>();

            using (var httpClient = new HttpClient())
            {
                var requestUri = $"{Configuration.DefaultAPI}/images/{targetObjectID}";
                httpClient.DefaultRequestHeaders.Add("CurrentUserID", CurrentUserID.ToString());
                httpClient.DefaultRequestHeaders.Add("CurrentHomeID", CurrentHomeID.ToString());

                using (var response = await httpClient.GetAsync(requestUri))
                {
                    string content = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                        result = JsonConvert.DeserializeObject<List<ImagePostViewModel>>(content);
                }
            }

            return result;
        }
        #endregion Images
        #endregion Clingons

        protected virtual void PopulateViewData()
        { }
    }
}