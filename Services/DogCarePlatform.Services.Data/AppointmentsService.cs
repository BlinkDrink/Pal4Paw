namespace DogCarePlatform.Services.Data
{
    using DogCarePlatform.Data.Common.Repositories;
    using DogCarePlatform.Data.Models;
    using DogCarePlatform.Web.ViewModels.Dogsitter;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class AppointmentsService : IAppointmentsService
    {
        private readonly IDeletableEntityRepository<Notification> notificationsRepository;
        private readonly IDeletableEntityRepository<Appointment> appointmentsRepository;

        public AppointmentsService(IDeletableEntityRepository<Notification> notificationsRepository, IDeletableEntityRepository<Appointment> appointmentsRepository)
        {
            this.notificationsRepository = notificationsRepository;
            this.appointmentsRepository = appointmentsRepository;
        }

        public async Task CreateNewAppointment(Appointment appointment)
        {
            await this.appointmentsRepository.AddAsync(appointment);
            await this.appointmentsRepository.SaveChangesAsync();
        }

        public Notification GetAppointmentFromNotificationById(string id)
        {
            return this.notificationsRepository.All().FirstOrDefault(n => n.Id == id);
        }

        public Task RemoveNotification(string id)
        {
            throw new NotImplementedException();
        }
    }
}
