namespace DogCarePlatform.Services.Data
{
    using System;
    using System.Threading.Tasks;
    using DogCarePlatform.Data.Common.Repositories;
    using DogCarePlatform.Data.Models;

    public class CommentsService : ICommentsService
    {
        private readonly IDeletableEntityRepository<Comment> commentsRepository;
        private readonly IDeletableEntityRepository<Rating> ratingsRepository;

        public CommentsService(IDeletableEntityRepository<Comment> commentsRepository, IDeletableEntityRepository<Rating> ratingsRepository)
        {
            this.commentsRepository = commentsRepository;
            this.ratingsRepository = ratingsRepository;
        }

        public async Task SubmitFeedbackByDogsitter(Comment comment, Rating rating)
        {
            await this.commentsRepository.AddAsync(comment);
            await this.ratingsRepository.AddAsync(rating);

            await this.commentsRepository.SaveChangesAsync();
            await this.ratingsRepository.SaveChangesAsync();
        }
    }
}
