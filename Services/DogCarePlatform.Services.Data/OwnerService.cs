namespace DogCarePlatform.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    using DogCarePlatform.Data.Common.Repositories;
    using DogCarePlatform.Data.Models;
    using DogCarePlatform.Web.ViewModels.Person;

    public class OwnerService : IOwnerService
    {
        private readonly IUsersService usersService;
        private readonly IDeletableEntityRepository<ApplicationUser> usersRepository;

        public Task AddPersonalInfoAsync()
        {
            throw new NotImplementedException();
        }
    }
}
