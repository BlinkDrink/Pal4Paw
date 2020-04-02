namespace DogCarePlatform.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using DogCarePlatform.Data.Common.Repositories;
    using DogCarePlatform.Data.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;

    public class UsersService : IUsersService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IDeletableEntityRepository<ApplicationUser> usersRepository;

        public UsersService(
            IDeletableEntityRepository<ApplicationUser> usersRepository,
            UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
            this.usersRepository = usersRepository;
        }

        public ApplicationUser GetUserByUsername(string username)
        {
            return this.userManager.FindByNameAsync(username).GetAwaiter().GetResult();
        }

        public bool AddUserToRole(string username, string role)
        {
            var user = this.GetUserByUsername(username);
            if (user == null)
            {
                return false;
            }

            this.userManager.AddToRoleAsync(user, role).GetAwaiter().GetResult();
            return true;
        }

        public bool RemoveUserFromRole(string username, string role)
        {
            var user = this.GetUserByUsername(username);
            if (user == null)
            {
                return false;
            }

            this.userManager.RemoveFromRoleAsync(user, role).GetAwaiter().GetResult();
            return true;
        }

        public IEnumerable<ApplicationUser> GetUsersByRole(string role)
        {
            var usersOfRole = this.userManager.GetUsersInRoleAsync(role).GetAwaiter().GetResult();

            return this.usersRepository.All().Where(x => usersOfRole.Any(u => u.Id == x.Id))
                                .ToList();
        }

        public Owner GetCurrentSignedInOwner(string username)
        {
            var user = this.userManager.FindByNameAsync(username).GetAwaiter().GetResult();

            var owner = user.Owners.FirstOrDefault(o => o.UserId == user.Id);

            return owner;
        }

        public async Task AddQuestionsAnswersToUser(QuestionAnswer questionAnswer, ApplicationUser user)
        {
            user.QuestionsAnswers.Add(questionAnswer);
            await this.usersRepository.SaveChangesAsync();
        }
    }
}
