﻿namespace DogCarePlatform.Web.Controllers
{
    using System;
    using System.Threading.Tasks;

    using DogCarePlatform.Data.Models;
    using DogCarePlatform.Services.Data;
    using DogCarePlatform.Web.ViewModels.Comment;
    using DogCarePlatform.Web.ViewModels.Notification;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class NotificationController : Controller
    {
        private readonly INotificationsService notificationsService;
        private readonly ICommentsService commentsService;

        public NotificationController(INotificationsService notificationsService, ICommentsService commentsService)
        {
            this.notificationsService = notificationsService;
            this.commentsService = commentsService;
        }

        /// <summary>
        ///  This action is used to display to the Owner whether 
        ///  the sent request for appointment is accepted/rejected.
        /// </summary>
        /// <param name="id">The Id of the notification.</param>
        /// <returns>Returns viewModel with the Notification details.</returns>
        [Authorize(Roles = "Owner")]
        public IActionResult GoToNotification(string id)
        {
            var viewModel = this.notificationsService.GetNotificationById<NotificationViewModel>(id);

            return this.View(viewModel);
        }

        /// <summary>
        /// This action is used to display to the Dogsitter
        /// that the appointment has ended. Afterwards he/she
        /// needs to leave a feedback to the Owner.
        /// </summary>
        /// <param name="id">The id of the notification.</param>
        /// <returns>Returns viewModel with a message that the appointment has ended and feedback is required.</returns>
        [Authorize(Roles="Dogsitter")]
        public IActionResult DogsitterSubmitFeedback(string id)
        {
            var notification = this.notificationsService.GetNotificationById(id);

            this.ViewData["DogsitterId"] = notification.DogsitterId;
            this.ViewData["OwnerId"] = notification.OwnerId;
            this.ViewData["SentBy"] = notification.SentBy;

            return this.View();
        }

        /// <summary>
        /// This action is used to submit the given feedback to the said Owner.
        /// </summary>
        /// <param name="dogsitterId">Dogsitter Id.</param>
        /// <param name="ownerId">Owner Id.</param>
        /// <param name="sentBy">A parameter which denotes the sender (i.e "Dogsitter").</param>
        /// <param name="content">The content of the feedback(in my project the feedback equals comment).</param>
        /// <param name="stars">The score(star rating) from 1 - 5 included in the feedback.</param>
        /// <returns>Submits feedback. Sends notification. Redirects to Home page.</returns>
        [Authorize(Roles = "Dogsitter")]
        [HttpPost]
        public async Task<IActionResult> SubmitFeedbackToOwner(RateAndCommentInputModel inputModel)
        {
            var rating = new Rating
            {
                Score = inputModel.Score,
                DogsitterId = inputModel.DogsitterId,
                OwnerId = inputModel.OwnerId,
                SentBy = "Dogsitter",
            };

            var comment = new Comment
            {
                Content = inputModel.Content,
                DogsitterId = inputModel.DogsitterId,
                OwnerId = inputModel.OwnerId,
                SentBy = inputModel.SentBy,
                RatingScore = inputModel.Score,
            };

            var notification = new Notification
            {
                Content = "Имате нов отзив.",
                OwnerId = inputModel.OwnerId,
                DogsitterId = inputModel.DogsitterId,
                SentBy = inputModel.SentBy,
                ReceivedOn = DateTime.Now,
            };

            // The dogsitter submits his/her feedback to the Owner after end of Appointment
            await this.commentsService.SubmitFeedback(comment, rating);

            await this.notificationsService.RemoveCommentNotification(inputModel.DogsitterId, inputModel.OwnerId, inputModel.Content, inputModel.SentBy);

            // A notification is send to the Owner containing a link to his/her Comments
            await this.notificationsService.SendNotification(notification);

            return this.RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// This action basically denotes that the current
        /// notification(which contains information about
        /// whether the request to a dogsitter has been rejected or accepted.)
        /// has been reviewed and is therefore deleted.
        /// It refers only to the Owner's notification.
        /// </summary>
        /// <param name="id">Notification Id.</param>
        /// <returns>Gets the notification by the given Id.
        /// Deletes the notification afterwards. Redirects to Home page.</returns>
        [Authorize(Roles = "Owner")]
        [HttpPost]
        public async Task<IActionResult> ReviewedNotification(string id)
        {
            var notification = this.notificationsService.GetNotificationById(id);

            await this.notificationsService.RemoveReviewedNotification(notification);

            return this.RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// This action shows the page where the Owner has to leave feedback for the Dogsitter.
        /// </summary>
        /// <param name="id">Notification Id.</param>
        /// <returns>Returns the view with the according name.</returns>
        [Authorize(Roles = "Owner")]
        public IActionResult OwnerSubmitFeedback(string id)
        {
            var notification = this.notificationsService.GetNotificationById(id);

            this.ViewData["DogsitterId"] = notification.DogsitterId;
            this.ViewData["OwnerId"] = notification.OwnerId;
            this.ViewData["SentBy"] = "Owner";

            return this.View();
        }

        /// <summary>
        /// This action submits the feedback for the Dogsitter.
        /// </summary>
        /// <param name="dogsitterId">Dogsitter Id.</param>
        /// <param name="ownerId">Owner Id.</param>
        /// <param name="sentBy">A parameter which denotes the sender (i.e "Owner").</param>
        /// <param name="content">The content of the feedback(or comment in this case).</param>
        /// <param name="stars">The score(star rating) from 1 - 5 included in the feedback.</param>
        /// <returns></returns>
        [Authorize(Roles ="Owner")]
        [HttpPost]
        public async Task<IActionResult> SubmitFeedbackToDogsitter(RateAndCommentInputModel inputModel)
        {
            var rating = new Rating
            {
                Score = inputModel.Score,
                DogsitterId = inputModel.DogsitterId,
                OwnerId = inputModel.OwnerId,
                SentBy = inputModel.SentBy,
            };

            var comment = new Comment
            {
                Content = inputModel.Content,
                DogsitterId = inputModel.DogsitterId,
                OwnerId = inputModel.OwnerId,
                SentBy = inputModel.SentBy,
                RatingScore = inputModel.Score,
            };

            var notification = new Notification
            {
                Content = "Имате нов отзив.",
                OwnerId = inputModel.OwnerId,
                DogsitterId = inputModel.DogsitterId,
                SentBy = inputModel.SentBy,
                ReceivedOn = DateTime.Now,
            };

            await this.notificationsService.RemoveCommentNotification(inputModel.DogsitterId, inputModel.OwnerId, inputModel.Content, inputModel.SentBy);
            await this.commentsService.SubmitFeedback(comment, rating);
            await this.notificationsService.SendNotification(notification);

            return this.RedirectToAction("Index", "Home");
        }
    }
}