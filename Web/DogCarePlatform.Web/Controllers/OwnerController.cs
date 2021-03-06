﻿namespace DogCarePlatform.Web.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using DogCarePlatform.Common;
    using DogCarePlatform.Data.Models;
    using DogCarePlatform.Services.Data;
    using DogCarePlatform.Web.Hubs;
    using DogCarePlatform.Web.ViewModels.Notification;
    using DogCarePlatform.Web.ViewModels.Owner;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.SignalR;

    [Authorize(Roles = "Owner")]
    public class OwnerController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IOwnersService ownerService;
        private readonly IHubContext<NotificationHub> hubContext;
        private readonly IDogsittersService dogsittersService;

        public OwnerController(UserManager<ApplicationUser> userManager, IOwnersService ownerService, IHubContext<NotificationHub> hubContext, IDogsittersService dogsittersService)
        {
            this.userManager = userManager;
            this.ownerService = ownerService;
            this.hubContext = hubContext;
            this.dogsittersService = dogsittersService;
        }

        /// <summary>
        /// This action displays the list of all Available and Approved dogsitters,
        /// who have filled their profile information.
        /// </summary>
        /// <returns>Returns View with the given ViewModel(List of dogsitters).</returns>
        [Authorize(Roles = "Owner")]
        public async Task<IActionResult> FindDogsitter()
        {
            var dogsitters = await this.userManager.GetUsersInRoleAsync(GlobalConstants.DogsitterRoleName);
            dogsitters.OrderBy(a => a.Dogsitter.Rating.Where(r => r.SentBy == "Owner").Average(r => r.Score));

            var viewModel = new ListDogsittersViewModel
            {
                Dogsitters = this.ownerService.GetDogsittersAsync(dogsitters),
            };

            if (this.TempData["isNotificationSent"] != null)
            {
                if (this.TempData["isNotificationSent"].ToString() == "sent")
                {
                    await this.hubContext.Clients.User(this.TempData["dogsitterUserName"].ToString()).SendAsync("ReceiveToast", $"Получихте заявка от {this.User.Identity.Name}");
                }
            }

            return this.View(viewModel);
        }

        /// <summary>
        /// This action shows the profile of the chosen Dogsitter.
        /// </summary>
        /// <param name="id">Notification Id.</param>
        /// <returns></returns>
        [Authorize(Roles = "Owner")]
        public IActionResult SendRequestToDogsitter(string id)
        {
            this.ViewData["dogsitterId"] = id;

            return this.View();
        }

        /// <summary>
        /// This action sends request to the chosen dogsitter.
        /// It appears as a notification for the dogsitter.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="inputModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Owner")]
        public async Task<IActionResult> SendRequestToDogsitter(SendRequestInputModel inputModel)
        {
            var user = await this.userManager.GetUserAsync(this.User);
            var dogsitter = this.dogsittersService.GetDogsitterByDogsitterId(inputModel.Id);
            var owner = user.Owner;

            if (!this.ModelState.IsValid)
            {
                if (inputModel.Date == DateTime.MinValue)
                {
                    this.TempData["errorEndTime"] = "Невалидна дата";
                }
                else if (inputModel.StartTime == DateTime.MinValue)
                {
                    this.TempData["errorEndTime"] = "Невалидно начално време";
                }
                else if (inputModel.EndTime == DateTime.MinValue)
                {
                    this.TempData["errorEndTime"] = "Невалидно крайно време";
                }
                else if (inputModel.EndTime <= inputModel.StartTime)
                {
                    this.TempData["errorEndTime"] = "Крайното време трябва да е след началното";
                }

                return this.RedirectToAction("SendRequestToDogsitter", "Owner", new { id = inputModel.Id });
            }

            await this.ownerService.SendNotification(inputModel.Id, owner, inputModel.Date, inputModel.StartTime, inputModel.EndTime);

            await this.hubContext.Clients.User(dogsitter.User.UserName).SendAsync("RefreshDocument", $"Получихте заявка от {owner.FirstName}");

            await this.hubContext.Clients.User(dogsitter.User.UserName).SendAsync("ReceiveToast", $"Получихте заявка от {this.User.Identity.Name}");

            this.TempData["isNotificationSent"] = "sent";
            this.TempData["dogsitterUserName"] = dogsitter.User.UserName;

            return this.RedirectToAction("FindDogsitter");
        }

        /// <summary>
        /// This action displays the page with the information about the selected Dogsitter.
        /// </summary>
        /// <param name="id">Dogsitter Id.</param>
        /// <returns>Returns the view with viewModel containing information about the dogsitter.</returns>
        [Authorize(Roles = "Owner")]
        public IActionResult DogsitterDetails(string id)
        {
            var dogsitterViewModel = this.ownerService.DogsitterDetailsById<DogsitterInfoViewModel>(id);

            return this.View(dogsitterViewModel);
        }
    }
}