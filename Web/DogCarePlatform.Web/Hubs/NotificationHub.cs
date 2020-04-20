namespace DogCarePlatform.Web.Hubs
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.SignalR;

    [Authorize]
    public class NotificationHub : Hub
    {
        public async Task SendMessage(string message)
        {
            await this.Clients.Caller.SendAsync("ReceiveToast", message);
        }

        public async Task RefreshNotificationsAsync(string message)
        {
            await this.Clients.Caller.SendAsync("RefreshDocument", message);
        }
    }
}
