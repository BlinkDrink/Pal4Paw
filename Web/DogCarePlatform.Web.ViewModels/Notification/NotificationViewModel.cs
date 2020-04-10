namespace DogCarePlatform.Web.ViewModels.Notification
{
    using DogCarePlatform.Data.Models;
    using DogCarePlatform.Services.Mapping;

    public class NotificationViewModel : IMapFrom<Notification>
    {
        public string Id { get; set; }

        public string Content { get; set; }

        public Dogsitter Dogsitter { get; set; }
    }
}
