namespace DogCarePlatform.Web.Hubs
{
    using System;
    using System.Threading.Tasks;
    using DogCarePlatform.Services.Data;
    using Microsoft.AspNetCore.SignalR;

    public class NotificationHub : Hub
    {
        private readonly IUserConnectionManager _userConnectionManager;

        public NotificationHub(IUserConnectionManager userConnectionManager)
        {
            this._userConnectionManager = userConnectionManager;
        }

        public string GetConnectionId()
        {
            var httpContext = this.Context.GetHttpContext();
            var userId = httpContext.Request.Query["Id"];
            this._userConnectionManager.KeepUserConnection(userId, this.Context.ConnectionId);

            return this.Context.ConnectionId;
        }

        // Called when a connection with the hub is terminated.
        public async override Task OnDisconnectedAsync(Exception exception)
        {
            // get the connectionId
            var connectionId = this.Context.ConnectionId;
            this._userConnectionManager.RemoveUserConnection(connectionId);
            var value = await Task.FromResult(0);
        }
    }
}

