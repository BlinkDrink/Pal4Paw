namespace DogCarePlatform.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using DogCarePlatform.Data.Common.Repositories;
    using DogCarePlatform.Data.Models;
    using DogCarePlatform.Services.Mapping;
    using DogCarePlatform.Web.ViewModels.Comment;

    public class CommentsService : ICommentsService
    {
        private readonly IDeletableEntityRepository<Comment> commentsRepository;
        private readonly IDeletableEntityRepository<Rating> ratingsRepository;

        public CommentsService(IDeletableEntityRepository<Comment> commentsRepository, IDeletableEntityRepository<Rating> ratingsRepository)
        {
            this.commentsRepository = commentsRepository;
            this.ratingsRepository = ratingsRepository;
        }

        public List<OwnerCommentsViewModel> OwnerComments(string id)
        {
            var comments = this.commentsRepository.All().Where(c => c.Owner.UserId == id);

            return comments.Select(c => new OwnerCommentsViewModel
            {
                Content = c.Content,
                Dogsitter = c.Dogsitter,
                Owner = c.Owner,
                SentBy = c.SentBy,
                CreatedOn = c.CreatedOn,
                RatingScore = c.RatingScore,
            })
                .ToList();
        }

        public List<DogsitterCommentsViewModel> DogsitterComments(string id)
        {
            var comments = this.commentsRepository.All().Where(c => c.Dogsitter.UserId == id);

            return comments.Select(c => new DogsitterCommentsViewModel
            {
                Content = c.Content,
                Dogsitter = c.Dogsitter,
                Owner = c.Owner,
                SentBy = c.SentBy,
                CreatedOn = c.CreatedOn,
                RatingScore = c.RatingScore,
            })
                .ToList();
        }

        public async Task SubmitFeedback(Comment comment, Rating rating)
        {
            await this.commentsRepository.AddAsync(comment);
            await this.ratingsRepository.AddAsync(rating);

            await this.commentsRepository.SaveChangesAsync();
            await this.ratingsRepository.SaveChangesAsync();
        }
    }
}
