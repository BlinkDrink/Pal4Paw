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
    using DogCarePlatform.Web.Utilities;
    using DogCarePlatform.Web.ViewModels.Notification;
    using DogCarePlatform.Web.ViewModels.Owner;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    [Authorize(Roles = "Owner")]
    public class OwnerController : Controller
    {
        private readonly IUsersService usersService;
        private readonly IDogsittersService dogsitterService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IOwnersService ownerService;

        public OwnerController(UserManager<ApplicationUser> userManager, IOwnersService ownerService, IUsersService usersService, IDogsittersService dogsitterService)
        {
            this.userManager = userManager;
            this.ownerService = ownerService;
            this.usersService = usersService;
            this.dogsitterService = dogsitterService;
        }

        public async Task<IActionResult> FindDogsitter()
        {
            var dogsitters = await this.userManager.GetUsersInRoleAsync(GlobalConstants.DogsitterRoleName);
            dogsitters.OrderBy(a => a.Dogsitters);

            var viewModel = new ListDogsittersViewModel
            {
                Dogsitters = this.ownerService.GetDogsittersAsync(dogsitters),
            };

            return this.View(viewModel);
        }

        public IActionResult SendRequestToDogsitter(string id)
        {
            this.TempData["DogsitterId"] = id;
            return this.View();
        }

        [HttpPost]
        public IActionResult SendRequestToDogsitter(SendNotificationInputModel inputModel)
        {
            return this.View();
        }

        public IActionResult DogsitterDetails(string id)
        {
            var dogsitterViewModel = this.ownerService.DogsitterDetailsById<DogsitterInfoViewModel>(id);

            return this.View(dogsitterViewModel);
        }
    }
}