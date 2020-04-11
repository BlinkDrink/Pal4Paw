namespace DogCarePlatform.Services.Data
{
    using Microsoft.AspNetCore.SignalR;

    public class MyUserIdProvider : IUserIdProvider
    {
        public string GetUserId(HubConnectionContext connection)
        {
            return connection.User.Identity.Name;
        }
    }
}
