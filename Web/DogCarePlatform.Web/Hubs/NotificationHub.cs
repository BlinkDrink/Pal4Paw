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

        public override Task OnConnectedAsync()
        {
            base.OnConnectedAsync();
            var user = this.Context.User.Identity.Name;
            // Groups.AddAsync(Context.ConnectionId, user);

            return Task.CompletedTask;
        }
    }
}
