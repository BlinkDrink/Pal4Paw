namespace DogCarePlatform.Services.Data
{
    using DogCarePlatform.Data.Common.Repositories;
    using DogCarePlatform.Data.Models;
    using DogCarePlatform.Web.ViewModels.Dogsitter;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class AppointmentsService : IAppointmentsService
    {
        private readonly IDeletableEntityRepository<Notification> notificationsRepository;

        public AppointmentsService(IDeletableEntityRepository<Notification> notificationsRepository)
        {
            this.notificationsRepository = notificationsRepository;
        }

        public Notification GetAppointmentFromNotification(string id)
        {
            return this.notificationsRepository.All().FirstOrDefault(n => n.Id == id);
        }
    }
}
