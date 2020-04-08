namespace DogCarePlatform.Services.Data
{
    using Microsoft.AspNetCore.SignalR;
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class MyUserIdProvider : IUserIdProvider
    {
        public string GetUserId(HubConnectionContext connection)
        {
            return connection.User.Identity.Name;
        }
    }
}
