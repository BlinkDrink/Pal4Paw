namespace DogCarePlatform.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using DogCarePlatform.Services.Data;
    using DogCarePlatform.Web.ViewModels.Comment;
    using Microsoft.AspNetCore.Mvc;

    public class CommentController : Controller
    {
        private readonly ICommentsService commentsService;

        public CommentController(ICommentsService commentsService)
        {
            this.commentsService = commentsService;
        }

        public IActionResult ListDogsitterComments(string id)
        {
            var viewModel = this.commentsService.DogsitterComments(id);

            return this.View(viewModel);
        }

        public IActionResult ListOwnerComments(string id)
        {
            var viewModel = this.commentsService.OwnerComments(id);

            return this.View(viewModel);
        }
    }
}
