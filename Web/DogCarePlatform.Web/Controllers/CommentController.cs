namespace DogCarePlatform.Web.Controllers
{
    using DogCarePlatform.Services.Data;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class CommentController : Controller
    {
        private readonly ICommentsService commentsService;
        private readonly INotificationsService notificationsService;

        public CommentController(ICommentsService commentsService, INotificationsService notificationsService)
        {
            this.commentsService = commentsService;
            this.notificationsService = notificationsService;
        }

        /// <summary>
        /// This action is used to list all the comments(feedbacks) for the current dogsitter.
        /// </summary>
        /// <param name="id">ApplicationUser Id.</param>
        /// <returns>Returns View with a list of comments for the current Dogsitter.</returns>
        [Authorize(Roles = "Dogsitter")]
        public IActionResult ListDogsitterComments(string id, string notifId)
        {
            var viewModel = this.commentsService.DogsitterComments(id);

            return this.View(viewModel);
        }

        /// <summary>
        /// This action is used to list all the comments(feedbacks) for the current owner.
        /// </summary>
        /// <param name="id">ApplicationUser Id.</param>
        /// <returns>Returns View with a list of comments for the current Owner.</returns>
        [Authorize(Roles = "Owner")]
        public IActionResult ListOwnerComments(string id)
        {
            var viewModel = this.commentsService.OwnerComments(id);

            return this.View(viewModel);
        }
    }
}
