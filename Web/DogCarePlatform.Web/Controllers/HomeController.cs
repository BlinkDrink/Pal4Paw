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
