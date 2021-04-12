using APPartment.Common;
using APPartment.Infrastructure.UI.Common.ViewModels;
using APPartment.Infrastructure.UI.Common.ViewModels.Base;
using APPartment.Infrastructure.UI.Common.ViewModels.Clingons.Comment;
using APPartment.Infrastructure.UI.Common.ViewModels.Clingons.Event;
using APPartment.Infrastructure.UI.Common.ViewModels.Clingons.Image;
using APPartment.Infrastructure.UI.Common.ViewModels.Clingons.Link;
using APPartment.Infrastructure.UI.Common.ViewModels.User;
using APPartment.Infrastructure.UI.Web.Constants.Breadcrumbs;
using APPartment.Infrastructure.UI.Web.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using SmartBreadcrumbs.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace APPartment.Infrastructure.UI.Web.Controllers.Base
{
    public abstract class BaseCRUDController<T, U> : BaseAuthorizeController
        where T : GridItemViewModel, new()
        where U : PostViewModel, new()
    {
        public BaseCRUDController(IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
        }

        public virtual async Task SetGridItemActions(T model)
        {
            model.ActionsHtml.Add(GridItemActionBuilder.BuildDetailsAction(CurrentAreaName, CurrentControllerName, model.ID));

            if (CanManage)
            {
                model.ActionsHtml.Add(GridItemActionBuilder.BuildEditAction(CurrentAreaName, CurrentControllerName, model.ID));
                model.ActionsHtml.Add(GridItemActionBuilder.BuildDeleteAction(CurrentAreaName, CurrentControllerName, model.ID));
            }
        }

        public virtual async Task SetObjectActions(U model)
        {
            model.ActionsHtml.Add(ObjectActionBuilder.BuildDetailsAction(CurrentAreaName, CurrentControllerName, model.ID));

            if (CanManage)
            {
                model.ActionsHtml.Add(ObjectActionBuilder.BuildEditAction(CurrentAreaName, CurrentControllerName, model.ID));
                model.ActionsHtml.Add(ObjectActionBuilder.BuildDeleteAction(CurrentAreaName, CurrentControllerName, model.ID));
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
                await SetGridItemActions(model);
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
                ViewData["CanManage"] = CanManage;

                await GetClingons(model);
                await SetObjectActions(model);
                await PopulateViewData(model);

                return View("_Details", model);
            }
            else
                return new Error404NotFoundViewResult();
        }


        [Breadcrumb(BaseCRUDBreadcrumbs.Create_Breadcrumb)]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var newModel = new U();

            await PopulateViewData(newModel);
            return View("_Edit", newModel);
        }

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
                await GetClingons(model);
                await PopulateViewData(model);
                await SetObjectActions(model);

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
        protected async Task GetClingons(U model)
        {
            model.CommentsHtml = await GetComments(model.ObjectID);
            model.Images = await GetImages(model.ObjectID);
            model.EventsHtml = await GetEvents(model.ObjectID);
            model.ObjectLinksHtml = await GetLinks(model.ObjectID);
            model.Participants = await GetParticipants(model.ObjectID);
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
        public async Task<IActionResult> PostComment(long targetObjectID, string comment)
        {
            var result = string.Empty;
            var theComment = new CommentPostViewModel()
            {
                Details = comment,
                TargetObjectID = targetObjectID
            };

            using (var httpClient = new HttpClient())
            {
                var requestUri = $"{Configuration.DefaultAPI}/comments/post";
                httpClient.DefaultRequestHeaders.Add("CurrentUserID", CurrentUserID.ToString());
                httpClient.DefaultRequestHeaders.Add("CurrentHomeID", CurrentHomeID.ToString());

                using (var response = await httpClient.PostAsJsonAsync(requestUri, theComment))
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

        #region Events
        private async Task<List<string>> GetEvents(long targetObjectID)
        {
            var events = new List<EventPostViewModel>();

            using (var httpClient = new HttpClient())
            {
                var requestUri = $"{Configuration.DefaultAPI}/events/{targetObjectID}";
                httpClient.DefaultRequestHeaders.Add("CurrentUserID", CurrentUserID.ToString());
                httpClient.DefaultRequestHeaders.Add("CurrentHomeID", CurrentHomeID.ToString());

                using (var response = await httpClient.GetAsync(requestUri))
                {
                    string content = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                        events = JsonConvert.DeserializeObject<List<EventPostViewModel>>(content);
                }
            }

            var result = EventsRenderer.BuildEvents(events);

            return result;
        }

        [HttpPost]
        public async Task<IActionResult> PostEvent(string targetObjectID, string name, string description, string startdate, string enddate, string[] participants)
        {
            var @event = new EventPostViewModel()
            {
                HomeID = (long)CurrentHomeID,
                Name = name,
                Details = description,
                StartDate = DateTime.Parse(startdate),
                EndDate = DateTime.Parse(enddate),
                ParticipantUserIDs = participants,
                TargetObjectID = long.Parse(targetObjectID)
            };

            using (var httpClient = new HttpClient())
            {
                var requestUri = $"{Configuration.DefaultAPI}/events/post";
                httpClient.DefaultRequestHeaders.Add("CurrentUserID", CurrentUserID.ToString());
                httpClient.DefaultRequestHeaders.Add("CurrentHomeID", CurrentHomeID.ToString());

                using (var response = await httpClient.PostAsJsonAsync(requestUri, @event))
                {
                    string content = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                        @event = JsonConvert.DeserializeObject<EventPostViewModel>(content);
                }
            }

            return Json(EventsRenderer.BuildPostEvent(@event));
        }
        #endregion

        #region Links
        private async Task<List<string>> GetLinks(long targetObjectID)
        {
            var links = new List<ObjectLinkPostViewModel>();

            using (var httpClient = new HttpClient())
            {
                var requestUri = $"{Configuration.DefaultAPI}/objectlinks/{targetObjectID}";
                httpClient.DefaultRequestHeaders.Add("CurrentUserID", CurrentUserID.ToString());
                httpClient.DefaultRequestHeaders.Add("CurrentHomeID", CurrentHomeID.ToString());

                using (var response = await httpClient.GetAsync(requestUri))
                {
                    string content = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                        links = JsonConvert.DeserializeObject<List<ObjectLinkPostViewModel>>(content);
                }
            }

            var result = ObjectLinksRenderer.BuildLinks(links);

            return result;
        }

        [HttpPost]
        public async Task<IActionResult> PostLink(string targetObjectID, string[] objectBIDs, string[] objectLinkTypes)
        {
            var link = new ObjectLinkPostViewModel()
            {
                ObjectBID = long.Parse(objectBIDs[0]),
                ObjectLinkType = objectLinkTypes[0],
                TargetObjectID = long.Parse(targetObjectID)
            };

            using (var httpClient = new HttpClient())
            {
                var requestUri = $"{Configuration.DefaultAPI}/objectlinks/post";
                httpClient.DefaultRequestHeaders.Add("CurrentUserID", CurrentUserID.ToString());
                httpClient.DefaultRequestHeaders.Add("CurrentHomeID", CurrentHomeID.ToString());

                using (var response = await httpClient.PostAsJsonAsync(requestUri, link))
                {
                    string content = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                        link = JsonConvert.DeserializeObject<ObjectLinkPostViewModel>(content);
                }
            }

            return Json(ObjectLinksRenderer.BuildPostLink(link));
        }
        #endregion

        #region Participants
        private async Task<List<ObjectParticipantPostViewModel>> GetParticipants(long targetObjectID)
        {
            var result = new List<ObjectParticipantPostViewModel>();

            using (var httpClient = new HttpClient())
            {
                var requestUri = $"{Configuration.DefaultAPI}/participants/{targetObjectID}";
                httpClient.DefaultRequestHeaders.Add("CurrentUserID", CurrentUserID.ToString());
                httpClient.DefaultRequestHeaders.Add("CurrentHomeID", CurrentHomeID.ToString());

                using (var response = await httpClient.GetAsync(requestUri))
                {
                    string content = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                        result = JsonConvert.DeserializeObject<List<ObjectParticipantPostViewModel>>(content);
                }
            }

            return result;
        }
        #endregion
        #endregion Clingons

        protected virtual async Task PopulateViewData(U model)
        {
            var users = await GetUsersInCurrentHome();
            var usersSelectList = users.Select(x => new SelectListItem() { Text = x.Name, Value = x.ID.ToString() }).ToList();
            ViewData["UsersSelectList"] = usersSelectList;

            // TODO: Brainstorm on more object link types...
            var objectLinkTypesSelectList = new List<SelectListItem>()
            {
                new SelectListItem() { Text = "relates to", Value = "relates to", Selected = true }
            };
            ViewData["ObjectLinkTypeSelectList"] = objectLinkTypesSelectList;

            var objects = new List<BusinessObjectDisplayViewModel>();

            using (var httpClient = new HttpClient())
            {
                var requestUri = $"{Configuration.DefaultAPI}/home/objects/{model.ObjectID}";
                httpClient.DefaultRequestHeaders.Add("CurrentUserID", CurrentUserID.ToString());
                httpClient.DefaultRequestHeaders.Add("CurrentHomeID", CurrentHomeID.ToString());

                using (var response = await httpClient.GetAsync(requestUri))
                {
                    string content = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                        objects = JsonConvert.DeserializeObject<List<BusinessObjectDisplayViewModel>>(content);
                }
            }

            var objectsSelectList = objects.Select(x => new SelectListItem() { Text = x.Name, Value = x.ObjectID.ToString() }).ToList();
            ViewData["ObjectBIDSelectList"] = objectsSelectList;
        }

        protected async Task<List<UserPostViewModel>> GetUsersInCurrentHome()
        {
            var users = new List<UserPostViewModel>();

            using (var httpClient = new HttpClient())
            {
                var requestUri = $"{Configuration.DefaultAPI}/users/home/{CurrentHomeID}";

                using (var response = await httpClient.GetAsync(requestUri))
                {
                    string content = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                        users = JsonConvert.DeserializeObject<List<UserPostViewModel>>(content);
                }
            }

            return users;
        }
    }
}