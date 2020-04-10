namespace DogCarePlatform.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using DogCarePlatform.Services.Data;
    using DogCarePlatform.Web.ViewModels.Notification;
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

        public IActionResult GoToNotification(string id)
        {
            var viewModel = this.notificationsService.GetNotificationById<RejectedNotificationViewModel>(id);

            return this.View(viewModel);
        }
    }
}