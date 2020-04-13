namespace DogCarePlatform.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using DogCarePlatform.Data.Models;
    using DogCarePlatform.Services.Data;
    using DogCarePlatform.Web.ViewModels.Comment;
    using DogCarePlatform.Web.ViewModels.Notification;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class NotificationController : Controller
    {
        private readonly INotificationsService notificationsService;

        public NotificationController(INotificationsService notificationsService)
        {
            this.notificationsService = notificationsService;
        }

        public IActionResult Index()
        {
            return this.View();
        }

        [Authorize(Roles="Owner")]
        public IActionResult GoToNotification(string id)
        {
            var viewModel = this.notificationsService.GetNotificationById<NotificationViewModel>(id);

            return this.View(viewModel);
        }

        public IActionResult NotificationAfterEndOfAppointment(string id)
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult SubmitFeedbackAfterAppointment()
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [Authorize(Roles = "Owner")]
        public async Task<IActionResult> ReviewedNotification(string id)
        {
            var notification = this.notificationsService.GetNotificationById(id);

            await this.notificationsService.RemoveReviewedNotification(notification);

            return this.RedirectToAction("Index", "Home");
        }
    }
}