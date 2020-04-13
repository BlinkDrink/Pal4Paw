namespace DogCarePlatform.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using DogCarePlatform.Data.Models;
    using DogCarePlatform.Services.Data;
    using DogCarePlatform.Web.Hubs;
    using DogCarePlatform.Web.ViewModels.Comment;
    using DogCarePlatform.Web.ViewModels.Notification;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.SignalR;

    public class NotificationController : Controller
    {
        private readonly INotificationsService notificationsService;
        private readonly ICommentsService commentsService;
        private readonly IHubContext<NotificationHub> notificationHub;
        private readonly UserManager<ApplicationUser> userManager;

        public NotificationController(INotificationsService notificationsService, ICommentsService commentsService, IHubContext<NotificationHub> notificationHub, UserManager<ApplicationUser> userManager)
        {
            this.notificationsService = notificationsService;
            this.commentsService = commentsService;
            this.notificationHub = notificationHub;
            this.userManager = userManager;
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

            var notification = new Notification
            {
                Content = "Вашата уговорка завърши. Имате нов отзив.",
                OwnerId = ownerId,
                DogsitterId = dogsitterId,
                SentBy = sentBy,
                ReceivedOn = DateTime.Now,
            };

            var owner = await this.notificationsService.GetOwnerApplicationUser(ownerId);

            await this.commentsService.SubmitFeedbackByDogsitter(comment, rating);
            await this.notificationsService.SendNotification(notification);

            // Refresh UI Page using SignalR hubs
            await this.notificationHub.Clients.User(this.User.Identity.Name).SendAsync("refreshUI");

            // Send toasts using SignalR
            await this.notificationHub.Clients.User(owner.UserName).SendAsync("sendNotification", owner.Owners.First());

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