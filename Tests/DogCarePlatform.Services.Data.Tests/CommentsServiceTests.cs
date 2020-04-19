namespace DogCarePlatform.Services.Data.Tests
{
    using System;
    using System.Linq;

    using DogCarePlatform.Data;
    using DogCarePlatform.Data.Models;
    using DogCarePlatform.Data.Repositories;
    using Microsoft.EntityFrameworkCore;
    using Xunit;

    public class CommentsServiceTests
    {
        private const string User = "user@user.com";

        private const string User2 = "user2@user.com";

        private const string SentByOwner = "Owner";

        private const string SentByDogsitter = "Dogsitter";

        private const string CommentContent = "Страхотно преживяване беше.";

        private const string FillInfo = "Информация";

        [Fact]
        public async void SubmitFeedbackShouldAddCommentAndRatingToTheDatabase()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var commentsRepository = new EfDeletableEntityRepository<Comment>(new ApplicationDbContext(options.Options));
            var ratingsRepository = new EfDeletableEntityRepository<Rating>(new ApplicationDbContext(options.Options));
            var ownersRepository = new EfDeletableEntityRepository<Owner>(new ApplicationDbContext(options.Options));
            var dogsittersRepository = new EfDeletableEntityRepository<Dogsitter>(new ApplicationDbContext(options.Options));
            var usersRepository = new EfDeletableEntityRepository<ApplicationUser>(new ApplicationDbContext(options.Options));

            var commentsService = new CommentsService(commentsRepository, ratingsRepository, ownersRepository);
            var ownersService = new OwnersService(usersRepository, ownersRepository, dogsittersRepository);

            var dogsitter = new Dogsitter();

            var user = new ApplicationUser
            {
                Dogsitter = dogsitter,
                UserName = User,
                Email = User,
            };

            var user2 = new ApplicationUser
            {
                UserName = User2,
                Email = User2,
            };

            dogsitter.UserId = user.Id;

            var rating = new Rating
            {
                Score = 5,
                Dogsitter = dogsitter,
                DogsitterId = dogsitter.Id,
                SentBy = SentByOwner,
            };

            var comment = new Comment
            {
                Content = CommentContent,
                RatingScore = rating.Score,
                Dogsitter = dogsitter,
                DogsitterId = dogsitter.Id,
                SentBy = SentByOwner,
            };

            await ownersService.CreateOwnerAsync(user2, FillInfo, FillInfo, FillInfo, FillInfo, Gender.Female, FillInfo, FillInfo, user2.Id, FillInfo);

            var owner = ownersRepository.All().FirstOrDefault();
            rating.Owner = owner;
            rating.OwnerId = owner.Id;
            comment.Owner = owner;
            comment.OwnerId = owner.Id;
            user2.Owner = owner;

            await commentsService.SubmitFeedback(comment, rating);

            Assert.Equal(1, commentsRepository.All().Count());
        }

        [Fact]
        public async void OwnerCommentsShouldReturnProperCount()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var commentsRepository = new EfDeletableEntityRepository<Comment>(new ApplicationDbContext(options.Options));
            var ratingsRepository = new EfDeletableEntityRepository<Rating>(new ApplicationDbContext(options.Options));
            var ownersRepository = new EfDeletableEntityRepository<Owner>(new ApplicationDbContext(options.Options));
            var dogsittersRepository = new EfDeletableEntityRepository<Dogsitter>(new ApplicationDbContext(options.Options));
            var usersRepository = new EfDeletableEntityRepository<ApplicationUser>(new ApplicationDbContext(options.Options));

            var commentsService = new CommentsService(commentsRepository, ratingsRepository, ownersRepository);
            var ownersService = new OwnersService(usersRepository, ownersRepository, dogsittersRepository);

            var dogsitter = new Dogsitter();

            var user = new ApplicationUser
            {
                Dogsitter = dogsitter,
                UserName = User,
                Email = User,
            };

            var user2 = new ApplicationUser
            {
                UserName = User2,
                Email = User2,
            };

            dogsitter.UserId = user.Id;

            var rating = new Rating
            {
                Score = 5,
                Dogsitter = dogsitter,
                DogsitterId = dogsitter.Id,
                SentBy = SentByDogsitter,
            };

            var comment = new Comment
            {
                Content = CommentContent,
                RatingScore = rating.Score,
                Dogsitter = dogsitter,
                DogsitterId = dogsitter.Id,
                SentBy = SentByDogsitter,
            };

            await ownersService.CreateOwnerAsync(user2, FillInfo, FillInfo, FillInfo, FillInfo, Gender.Female, FillInfo, FillInfo, user2.Id, FillInfo);

            var owner = ownersRepository.All().FirstOrDefault();
            rating.Owner = owner;
            rating.OwnerId = owner.Id;
            comment.Owner = owner;
            comment.OwnerId = owner.Id;
            user2.Owner = owner;

            await commentsService.SubmitFeedback(comment, rating);
            var ownerComments = commentsService.OwnerComments(user2.Id);

            Assert.Single(ownerComments);
        }

        [Fact]
        public async void DogsitterCommentsShouldReturnProperCount()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());
            var commentsRepository = new EfDeletableEntityRepository<Comment>(new ApplicationDbContext(options.Options));
            var ratingsRepository = new EfDeletableEntityRepository<Rating>(new ApplicationDbContext(options.Options));
            var ownersRepository = new EfDeletableEntityRepository<Owner>(new ApplicationDbContext(options.Options));

            var commentsService = new CommentsService(commentsRepository, ratingsRepository, ownersRepository);

            var dogsitter = new Dogsitter
            {
                UserId = Guid.NewGuid().ToString(),
            };

            var owner = new Owner
            {
                UserId = Guid.NewGuid().ToString(),
            };

            var rating = new Rating
            {
                Score = 5,
                Dogsitter = dogsitter,
                DogsitterId = dogsitter.Id,
                SentBy = SentByOwner,
                Owner = owner,
                OwnerId = owner.Id,
            };

            var comment = new Comment
            {
                Content = CommentContent,
                RatingScore = rating.Score,
                Dogsitter = dogsitter,
                DogsitterId = dogsitter.Id,
                SentBy = SentByOwner,
            };

            await commentsService.SubmitFeedback(comment, rating);
            var ownerComments = commentsService.OwnerComments(dogsitter.UserId);

            Assert.Single(ownerComments);
        }
    }
}
