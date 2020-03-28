namespace DogCarePlatform.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using DogCarePlatform.Common;
    using DogCarePlatform.Data.Common.Repositories;
    using DogCarePlatform.Data.Models;
    using DogCarePlatform.Services.Data;
    using DogCarePlatform.Web.ViewModels.Owner;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class OwnerController : Controller
    {
        private readonly IUsersService usersService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IOwnersService ownerService;


        public OwnerController(UserManager<ApplicationUser> userManager, IOwnersService ownerService, IUsersService usersService)
        {
            this.userManager = userManager;
            this.ownerService = ownerService;
            this.usersService = usersService;
        }

        public IActionResult AddInfo()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> AddInfo(AddInfoInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }


            var user = await this.userManager.GetUserAsync(this.User);
            await this.ownerService.AddPersonalInfoAsync(input.Address, input.FirstName, input.MiddleName, input.LastName, input.Gender,  input.ImageUrl, input.PhoneNumber, user.Id);

            this.usersService.AddUserToRole(user.UserName, GlobalConstants.OwnerRoleName);

            return Redirect("/");
        }
    }
}