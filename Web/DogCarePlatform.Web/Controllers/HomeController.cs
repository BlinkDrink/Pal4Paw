namespace DogCarePlatform.Web.Controllers
{
    using System.Diagnostics;
    using System.Threading.Tasks;
    using DogCarePlatform.Data.Models;
    using DogCarePlatform.Web.Hubs;
    using DogCarePlatform.Web.ViewModels;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.SignalR;

    public class HomeController : BaseController
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IHubContext<NotificationHub> hubContext;

        public HomeController(UserManager<ApplicationUser> userManager, IHubContext<NotificationHub> hubContext)
        {
            this.userManager = userManager;
            this.hubContext = hubContext;
        }

        public async Task<IActionResult> Index()
        {
            if (this.User.IsInRole("Dogsitter"))
            {
                var user = await this.userManager.GetUserAsync(this.User);

                var hasInfo = user.Dogsitter.Address != null ? true :
                    user.Dogsitter.Description != null ? true :
                    user.Dogsitter.FirstName != null ? true :
                    user.Dogsitter.MiddleName != null ? true :
                    user.Dogsitter.LastName != null ? true :
                    user.Dogsitter.WageRate >= 5 ? true :
                    user.Dogsitter.ImageUrl != null ? true : false;

                if (!hasInfo)
                {
                    await this.hubContext.Clients.User(user.UserName).SendAsync("NotFilledInfoPopup", true);
                }
                else
                {
                    await this.hubContext.Clients.User(user.UserName).SendAsync("NotFilledInfoPopup", false);
                }
            }

            return this.View();
        }

        public IActionResult Privacy()
        {
            return this.View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(
                new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }
    }
}
