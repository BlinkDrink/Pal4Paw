namespace DogCarePlatform.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using DogCarePlatform.Data.Common.Repositories;
    using DogCarePlatform.Data.Models;
    using DogCarePlatform.Web.ViewModels.Comment;

    public class CommentsService : ICommentsService
    {
        private readonly IDeletableEntityRepository<Comment> commentsRepository;
        private readonly IDeletableEntityRepository<Rating> ratingsRepository;
        private readonly IDeletableEntityRepository<Owner> ownersRepository;

        public CommentsService(IDeletableEntityRepository<Comment> commentsRepository, IDeletableEntityRepository<Rating> ratingsRepository, IDeletableEntityRepository<Owner> ownersRepository)
        {
            this.commentsRepository = commentsRepository;
            this.ratingsRepository = ratingsRepository;
            this.ownersRepository = ownersRepository;
        }

        public List<OwnerCommentsViewModel> OwnerComments(string id)
        {
            //var comments = this.commentsRepository.All().Where(c => c.Owner.UserId == id && c.SentBy == "Dogsitter").ToList();
            var owner = this.ownersRepository.All().FirstOrDefault(o => o.UserId == id);
            var comments = owner.Comments.Where(c => c.SentBy == "Dogsitter").ToList();

            var fiveStarPercentage = this.GetStarPercentage(comments.Count(c => c.RatingScore == 5), comments.Count());
            var fourStarPercentage = this.GetStarPercentage(comments.Count(c => c.RatingScore == 4), comments.Count());
            var threeStarPercentage = this.GetStarPercentage(comments.Count(c => c.RatingScore == 3), comments.Count());
            var twoStarPercentage = this.GetStarPercentage(comments.Count(c => c.RatingScore == 2), comments.Count());
            var oneStarPercentage = this.GetStarPercentage(comments.Count(c => c.RatingScore == 1), comments.Count());

            return comments.Select(c => new OwnerCommentsViewModel
            {
                Content = c.Content,
                Dogsitter = c.Dogsitter,
                Owner = c.Owner,
                SentBy = c.SentBy,
                CreatedOn = c.CreatedOn,
                RatingScore = c.RatingScore,
                FiveStarPercentage = fiveStarPercentage,
                FourStarPercentage = fourStarPercentage,
                ThreeStarPercentage = threeStarPercentage,
                TwoStarPercentage = twoStarPercentage,
                OneStarPercentage = oneStarPercentage,
                Total = comments.Count(c => c.RatingScore != 0),
                FiveStars = comments.Count(c => c.RatingScore == 5),
                FourStars = comments.Count(c => c.RatingScore == 4),
                ThreeStars = comments.Count(c => c.RatingScore == 3),
                TwoStars = comments.Count(c => c.RatingScore == 2),
                OneStar = comments.Count(c => c.RatingScore == 1),
            })
                .ToList();
        }

        public List<DogsitterCommentsViewModel> DogsitterComments(string id)
        {
            var comments = this.commentsRepository.All().Where(c => c.Dogsitter.UserId == id && c.SentBy == "Owner").ToList();

            var fiveStarPercentage = this.GetStarPercentage(comments.Count(c => c.RatingScore == 5), comments.Count());
            var fourStarPercentage = this.GetStarPercentage(comments.Count(c => c.RatingScore == 4), comments.Count());
            var threeStarPercentage = this.GetStarPercentage(comments.Count(c => c.RatingScore == 3), comments.Count());
            var twoStarPercentage = this.GetStarPercentage(comments.Count(c => c.RatingScore == 2), comments.Count());
            var oneStarPercentage = this.GetStarPercentage(comments.Count(c => c.RatingScore == 1), comments.Count());

            return comments.Select(c => new DogsitterCommentsViewModel
            {
                Content = c.Content,
                Dogsitter = c.Dogsitter,
                Owner = c.Owner,
                SentBy = c.SentBy,
                CreatedOn = c.CreatedOn,
                RatingScore = c.RatingScore,
                FiveStarPercentage = fiveStarPercentage,
                FourStarPercentage = fourStarPercentage,
                ThreeStarPercentage = threeStarPercentage,
                TwoStarPercentage = twoStarPercentage,
                OneStarPercentage = oneStarPercentage,
                Total = comments.Count(c => c.RatingScore != 0),
                FiveStars = comments.Count(c => c.RatingScore == 5),
                FourStars = comments.Count(c => c.RatingScore == 4),
                ThreeStars = comments.Count(c => c.RatingScore == 3),
                TwoStars = comments.Count(c => c.RatingScore == 2),
                OneStar = comments.Count(c => c.RatingScore == 1),
            })
                .ToList();
        }

        public async Task SubmitFeedback(Comment comment, Rating rating)
        {
            var owner = this.ownersRepository.All().FirstOrDefault(o => o.Id == comment.OwnerId);

            owner.Comments.Add(comment);
            owner.Rating.Add(rating);

            await this.ownersRepository.SaveChangesAsync();
        }

        private decimal GetStarPercentage(int countPerType, int totalCount)
        {
            if (totalCount == 0)
            {
                return 0;
            }

            var starPortion = (decimal)countPerType / totalCount;
            return starPortion == 0 ? 0 : Math.Round(100 * starPortion, 2);
        }
    }
}
