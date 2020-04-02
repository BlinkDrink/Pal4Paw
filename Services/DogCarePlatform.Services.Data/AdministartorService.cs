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
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IDeletableEntityRepository<ApplicationUser> usersRepository;

        public AdministartorService(UserManager<ApplicationUser> userManager, IDeletableEntityRepository<ApplicationUser> usersRepository)
        {
            this.userManager = userManager;
            this.usersRepository = usersRepository;
        }

        public T ApplicantDetailsById<T>(string id)
        {
            var user = this.usersRepository.All()
                .Where(u => u.Id == id)
                .To<T>().FirstOrDefault();

            return user;
        }

        public Task ApproveApplicant(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<ICollection<ApplicationUser>> GetApplicants()
        {
            ICollection<ApplicationUser> applicants = await this.userManager.GetUsersInRoleAsync(GlobalConstants.UnapprovedUserRoleName);
            applicants.OrderBy(a => a.UserName);

            return applicants;
        }

        public Task RejectApplicant(string id)
        {
            throw new NotImplementedException();
        }
    }
}
