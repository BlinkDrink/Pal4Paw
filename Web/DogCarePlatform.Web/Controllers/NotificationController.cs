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
        private readonly ICommentsService commentsService;

        public NotificationController(INotificationsService notificationsService, ICommentsService commentsService)
        {
            this.notificationsService = notificationsService;
            this.commentsService = commentsService;
        }

        public IActionResult Index()
        {
            return this.View();
        }

        [Authorize(Roles = "Owner")]
        public IActionResult GoToNotification(string id)
        {
            var viewModel = this.notificationsService.GetNotificationById<NotificationViewModel>(id);

            return this.View(viewModel);
        }

        public IActionResult NotificationAfterEndOfAppointment(string id)
        {
            var viewModel = this.notificationsService.GetNotificationById<NotificationAfterAppointmentViewModel>(id);

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> SubmitFeedbackAfterAppointment(string dogsitterId, string ownerId, string sentBy, string content, int stars)
        {
            var comment = new Comment
            {
                Content = content,
                DogsitterId = dogsitterId,
                OwnerId = ownerId,
                SentBy = sentBy,
            };

            var rating = new Rating
            {
                Score = stars,
                DogsitterId = dogsitterId,
                OwnerId = ownerId,
                SentBy = sentBy,
            };

            await this.commentsService.SubmitFeedbackByDogsitter(comment, rating);

            return this.RedirectToAction("Index", "Home");
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