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
    using DogCarePlatform.Web.Hubs;
    using DogCarePlatform.Web.Utilities;
    using DogCarePlatform.Web.ViewModels.Notification;
    using DogCarePlatform.Web.ViewModels.Owner;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.SignalR;

    [Authorize(Roles = "Owner")]
    public class OwnerController : Controller
    {
        private readonly IUsersService usersService;
        private readonly IDogsittersService dogsitterService;
        private readonly IHubContext<NotificationHub> notificationHubContext;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IOwnersService ownerService;

        public OwnerController(UserManager<ApplicationUser> userManager, IOwnersService ownerService, IUsersService usersService, IDogsittersService dogsitterService, IHubContext<NotificationHub> notificationHubContext)
        {
            this.userManager = userManager;
            this.ownerService = ownerService;
            this.usersService = usersService;
            this.dogsitterService = dogsitterService;
            this.notificationHubContext = notificationHubContext;
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
            var notification = new SendNotificationInputModel();
            notification.Id = id;
            return this.View(notification);
        }

        [HttpPost]
        public async Task<IActionResult> SendRequestToDogsitter([FromForm]string id, SendNotificationInputModel inputModel)
        {
            var user = await this.userManager.GetUserAsync(this.User);
            var owner = user.Owners.FirstOrDefault();
            var dogsitter = this.dogsitterService.GetDogsitterById(id);

            await this.ownerService.SendNotification(id, owner, inputModel.Date, inputModel.StartTime, inputModel.EndTime);

            // Refresh the page to reflect changes.
            await this.notificationHubContext.Clients.User(user.UserName).SendAsync("refreshUI");

            // Notify the user who is receiving the notification. (if connected)
            await this.notificationHubContext.Clients.User(dogsitter.User.UserName).SendAsync("sendNotification", owner.User.UserName);

            return this.RedirectToAction("FindDogsitter");
        }

        public IActionResult DogsitterDetails(string id)
        {
            var dogsitterViewModel = this.ownerService.DogsitterDetailsById<DogsitterInfoViewModel>(id);

            return this.View(dogsitterViewModel);
        }
    }
}