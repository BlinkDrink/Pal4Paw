namespace DogCarePlatform.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using DogCarePlatform.Common;
    using DogCarePlatform.Data.Common.Repositories;
    using DogCarePlatform.Data.Models;
    using DogCarePlatform.Services.Mapping;
    using DogCarePlatform.Web.ViewModels.Administration.Dashboard;
    using Microsoft.AspNetCore.Identity;

    public class AdministartorService : IAdministartorService
    {
        private readonly IDeletableEntityRepository<ApplicationUser> usersRepository;
        private readonly IDeletableEntityRepository<QuestionAnswer> questionsRepository;
        private readonly IDeletableEntityRepository<Dogsitter> dogsitterRepository;

        public AdministartorService(IDeletableEntityRepository<ApplicationUser> usersRepository, IDeletableEntityRepository<QuestionAnswer> questionsRepository, IDeletableEntityRepository<Dogsitter> dogsitterRepository)
        {
            this.usersRepository = usersRepository;
            this.questionsRepository = questionsRepository;
            this.dogsitterRepository = dogsitterRepository;
        }

        public async Task AddDogsitterAsync(string id)
        {
            var user = this.usersRepository.All().Where(u => u.Id == id).FirstOrDefault();

            var dogsitter = new Dogsitter();
            user.Dogsitters.Add(dogsitter);
            user.Dogsitters.FirstOrDefault().PhoneNumber = user.PhoneNumber;

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
