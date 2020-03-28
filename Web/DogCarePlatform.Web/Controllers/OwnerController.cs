namespace DogCarePlatform.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using DogCarePlatform.Data.Common.Repositories;
    using DogCarePlatform.Data.Models;
    using DogCarePlatform.Services.Data;
    using DogCarePlatform.Web.ViewModels.Person;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class OwnerController : Controller
    {
        private readonly IDeletableEntityRepository<ApplicationUser> _userRepository;
        private readonly IUsersService _usersService;

        public OwnerController(IDeletableEntityRepository<ApplicationUser> userRepository, IUsersService usersService)
        {
            this._userRepository = userRepository;
            this._usersService = usersService;
        }

        public IActionResult AddInfo()
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult AddInfo(AddInfoInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }



            var owner = new Owner
            {
                FirstName = input.FirstName,
                MiddleName = input.MiddleName,
                LastName = input.LastName,
                DateOfBirth = input.DateOfBirth,
                Gender = input.Gender,
            };

            throw new NotImplementedException();
        }
    }
}