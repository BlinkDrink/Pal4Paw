namespace DogCarePlatform.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using DogCarePlatform.Data.Models;
    using DogCarePlatform.Web.ViewModels.Comment;

    public interface ICommentsService
    {
        Task SubmitFeedbackByDogsitter(Comment comment, Rating rating);

        List<OwnerCommentsViewModel> OwnerComments(string id);

        List<DogsitterCommentsViewModel> DogsitterComments(string id);
    }
}
