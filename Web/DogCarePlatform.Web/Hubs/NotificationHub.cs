namespace DogCarePlatform.Web.Hubs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using DogCarePlatform.Data;
    using Microsoft.AspNetCore.SignalR;

    public class NotificationHub : Hub
    {
        private readonly ApplicationDbContext dbContext;

        public NotificationHub(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        private static IHubContext hubContext = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
        private static List<String> _connectedClients = new List<String>();

        public override Task OnConnected()
        {
            var tmp = Context.User.Identity.Name;
            //HttpContext.Current.Session.Add("connectionId", Context.ConnectionId);
            _connectedClients.Add(Context.ConnectionId);
            return base.OnConnected();
        }

        public void ConnectToMessageHub(string username)
        {
            // MAP LOGICAL USER "LOCATIONID" TO THE "CONNECTIONID" USING GROUPS.
            // THIS ALLOWS US TO SEND TO THE "GROUP" OUTSIDE OF THE HUB, WHERE THERE
            // IS NO CONCEPT OF THE "CONNECTIONID"
            this.Groups.AddToGroupAsync(Context.ConnectionId, username);
            System.Diagnostics.Debug.WriteLine("HUB: CONNECTFORCHECKINS: " + username);
        }

        public static void SendToast(string groupId, string toastType, string content, string title)
        {
            hubContext.Clients.Group(groupId).sendToast(toastType, content, title);
        }

        public override Task OnConnectedAsync()
        {
            base.OnConnectedAsync();
            var user = this.Context.User.Identity.Name;
            // Groups.AddAsync(Context.ConnectionId, user);

            return Task.CompletedTask;
        }
    }
}
