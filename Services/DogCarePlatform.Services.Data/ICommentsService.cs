namespace DogCarePlatform.Services.Data
{
    using System.Threading.Tasks;

    using DogCarePlatform.Data.Models;

    public interface ICommentsService
    {
        Task SubmitFeedbackByDogsitter(Comment comment, Rating rating);
    }
}
