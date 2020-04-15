namespace DogCarePlatform.Web.Controllers
{
    using System.Diagnostics;
    using System.Threading.Tasks;
    using DogCarePlatform.Services.Data;
    using DogCarePlatform.Web.Hubs;
    using DogCarePlatform.Web.ViewModels;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.SignalR;

    public class HomeController : BaseController
    {
        private readonly IHubContext<NotificationHub> notificationHubContext;
        private readonly IUserConnectionManager userConnectionManager;

        public HomeController(IHubContext<NotificationHub> notificationHubContext, IUserConnectionManager userConnectionManager)
        {
            this.notificationHubContext = notificationHubContext;
            this.userConnectionManager = userConnectionManager;
        }

        public async Task<IActionResult> Index()
        {
            //var connections = this.userConnectionManager.GetUserConnections("85dd4a50-8be9-44de-a36b-7d9e9526713d");
            //if (connections != null && connections.Count > 0)
            //{
            //    foreach (var connectionId in connections)
            //    {
            //        await this.notificationHubContext.Clients.Client(connectionId).SendAsync("sendToUser", "Hello", "How are ya doin");
            //    }
            //}

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
