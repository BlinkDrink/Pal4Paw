namespace DogCarePlatform.Web.Controllers
{
    using System.Diagnostics;
    using System.Threading.Tasks;
    using DogCarePlatform.Web.Hubs;
    using DogCarePlatform.Web.ViewModels;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.SignalR;

    public class HomeController : BaseController
    {
        private readonly IHubContext<NotificationHub> _notificationHubContext;

        public HomeController(IHubContext<NotificationHub> _notificationHubContext)
        {
            this._notificationHubContext = this._notificationHubContext;
        }

        public async Task<IActionResult> Index()
        {
            await this._notificationHubContext.Clients.All.SendAsync("sendToUser", "Hello all", "Happy to meet eacha nd every one of you");
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
