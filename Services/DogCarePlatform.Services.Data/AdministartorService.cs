namespace DogCarePlatform.Services.Data
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using DogCarePlatform.Data.Common.Repositories;
    using DogCarePlatform.Data.Models;
    using DogCarePlatform.Services.Mapping;

    public class AdministartorService : IAdministartorService
    {
        private readonly IDeletableEntityRepository<ApplicationUser> usersRepository;
        private readonly IDeletableEntityRepository<QuestionAnswer> questionsRepository;

        public AdministartorService(IDeletableEntityRepository<ApplicationUser> usersRepository, IDeletableEntityRepository<QuestionAnswer> questionsRepository)
        {
            this.usersRepository = usersRepository;
            this.questionsRepository = questionsRepository;
        }

        public async Task AddDogsitterAsync(string id)
        {
            var user = this.usersRepository.All().FirstOrDefault(u => u.Id == id);

            var dogsitter = new Dogsitter();
            user.Dogsitter = dogsitter;
            user.Dogsitter.PhoneNumber = user.PhoneNumber;
            user.Dogsitter.DateOfBirth = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day);

            await this.usersRepository.SaveChangesAsync();
        }

        public T ApplicantDetailsById<T>(string id)
        {
            var user = this.usersRepository.All()
                .Where(u => u.Id == id)
                .To<T>().FirstOrDefault();

            return user;
        }

        public async Task RemoveQuestionsAnswersFromUserAsync(string userId)
        {
            var questionAnswers = this.questionsRepository.All().Where(qa => qa.UserId == userId);

            foreach (var qa in questionAnswers)
            {
                this.questionsRepository.Delete(qa);
            }

            await this.questionsRepository.SaveChangesAsync();
        }
    }
}
