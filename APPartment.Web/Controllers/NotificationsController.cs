﻿using APPartment.UI.Controllers.Base;
using APPartment.UI.Utilities;
using APPartment.UI.ViewModels.Notification;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APPartment.Web.Controllers
{
    public class NotificationsController : BaseController
    {
        private HtmlRenderHelper htmlRenderHelper;

        public NotificationsController(IHttpContextAccessor contextAccessor) : base(contextAccessor)
        {
            htmlRenderHelper = new HtmlRenderHelper(CurrentUserID);
        }

        public JsonResult GetContents()
        {
            var notifications = NotificationService.GetNotifications();
            var result = htmlRenderHelper.BuildNotificationsContent(notifications);
            return Json(result);
        }

        public JsonResult GetCount()
        {
            var count = BaseWebService.Count<NotificationParticipantPostViewModel>(x => x.UserID == (long)CurrentUserID);
            return Json(count);
        }
    }
}