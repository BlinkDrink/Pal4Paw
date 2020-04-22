namespace DogCarePlatform.Web.Controllers
{
    using System;
    using System.Threading.Tasks;

    using DogCarePlatform.Data.Models;
    using DogCarePlatform.Services.Data;
    using DogCarePlatform.Web.Hubs;
    using DogCarePlatform.Web.ViewModels.Comment;
    using DogCarePlatform.Web.ViewModels.Notification;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.SignalR;

    [Authorize]
    public class NotificationController : Controller
    {
        private readonly IDogsittersService dogsittersService;
        private readonly INotificationsService notificationsService;
        private readonly ICommentsService commentsService;
        private readonly IHubContext<NotificationHub> hubContext;

        public NotificationController(IDogsittersService dogsittersService, INotificationsService notificationsService, ICommentsService commentsService, IHubContext<NotificationHub> hubContext)
        {
            this.dogsittersService = dogsittersService;
            this.notificationsService = notificationsService;
            this.commentsService = commentsService;
            this.hubContext = hubContext;
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
        [Authorize(Roles = "Dogsitter")]
        public IActionResult DogsitterSubmitFeedback(string id)
        {
            var notification = this.notificationsService.GetNotificationById(id);

            if (notification == null)
            {
                return this.RedirectToAction("Index", "Home");
            }

            this.ViewData["DogsitterId"] = notification.DogsitterId;
            this.ViewData["OwnerId"] = notification.OwnerId;
            this.ViewData["SentBy"] = notification.SentBy;
            this.ViewData["notificationId"] = id;

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
            if (!this.ModelState.IsValid)
            {
                this.TempData["wrongFeedback"] = "Коментарът трябва да съдържа поне 50 символа и не повече от 500.";
                return this.RedirectToAction("DogsitterSubmitFeedback", new { id = inputModel.NotificationId });
            }

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
                SentBy = "Dogsitter",
                RatingScore = inputModel.Score,
            };

            var notification = new Notification
            {
                Content = "Имате нов отзив.",
                OwnerId = inputModel.OwnerId,
                DogsitterId = inputModel.DogsitterId,
                SentBy = "Dogsitter",
                ReceivedOn = DateTime.Now,
            };

            // The dogsitter submits his/her feedback to the Owner after end of Appointment
            await this.commentsService.SubmitFeedback(comment, rating);

            // A notification is send to the Owner containing a link to his/her Comments
            await this.notificationsService.SendNotification(notification);

            await this.hubContext.Clients.User(notification.Owner.User.UserName).SendAsync("RefreshDocument", "Имате известие.");

            await this.hubContext.Clients.User(notification.Owner.User.UserName).SendAsync("ReceiveToast", "Имате известие.");

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
            await this.notificationsService.RemoveCommentNotification(id);

            return this.RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// This action shows the page where the Owner has to leave feedback for the Dogsitter.
        /// Then deletes the reviewed notification. Also blocks the Backward page button to retain
        /// the user on this page until he/she finishes giving feedback.
        /// </summary>
        /// <param name="id">Notification Id.</param>
        /// <returns>Returns the view with the according name.</returns>
        [Authorize(Roles = "Owner")]
        public async Task<IActionResult> OwnerSubmitFeedback(string id)
        {
            var notification = this.notificationsService.GetNotificationById(id);

            if (notification == null)
            {
                return RedirectToAction("Index", "Home");
            }

            this.ViewData["DogsitterId"] = notification.DogsitterId;
            this.ViewData["OwnerId"] = notification.OwnerId;
            this.ViewData["SentBy"] = "Owner";
            this.ViewData["notificationId"] = id;

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
        [Authorize(Roles = "Owner")]
        [HttpPost]
        public async Task<IActionResult> SubmitFeedbackToDogsitter(RateAndCommentInputModel inputModel)
        {
            if (!this.ModelState.IsValid)
            {
                this.TempData["wrongFeedback"] = "Коментарът трябва да съдържа поне 50 символа и не повече от 500.";
                return this.RedirectToAction("OwnerSubmitFeedback", new { id = inputModel.NotificationId });
            }

            var rating = new Rating
            {
                Score = inputModel.Score,
                DogsitterId = inputModel.DogsitterId,
                OwnerId = inputModel.OwnerId,
                SentBy = "Owner",
            };

            var comment = new Comment
            {
                Content = inputModel.Content,
                DogsitterId = inputModel.DogsitterId,
                OwnerId = inputModel.OwnerId,
                SentBy = "Owner",
                RatingScore = inputModel.Score,
            };

            var notification = new Notification
            {
                Content = "Имате нов отзив.",
                OwnerId = inputModel.OwnerId,
                DogsitterId = inputModel.DogsitterId,
                SentBy = "Owner",
                ReceivedOn = DateTime.Now,
            };

            await this.commentsService.SubmitFeedback(comment, rating);
            await this.notificationsService.SendNotification(notification);

            var dogsitter = this.dogsittersService.GetDogsitterByDogsitterId(rating.DogsitterId);
            await this.notificationsService.RemoveCommentNotification(inputModel.NotificationId);

            await this.hubContext.Clients.User(dogsitter.User.UserName).SendAsync("RefreshDocument", "Имате известие");

            return this.RedirectToAction("Index", "Home");
        }
    }
}