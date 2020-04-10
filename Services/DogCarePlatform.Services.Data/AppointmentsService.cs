namespace DogCarePlatform.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using DogCarePlatform.Data.Common.Repositories;
    using DogCarePlatform.Data.Models;
    using DogCarePlatform.Services.Mapping;
    using DogCarePlatform.Web.ViewModels.Appointment;

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

        public ICollection<AppointmentViewModel> GetDogsitterAppointmentsToList(string id)
        {
            var appointments = this.appointmentsRepository.All().Where(a => a.Dogsitter.UserId == id)
               .Select(a => new AppointmentViewModel
               {
                   Id = a.Id,
                   AppointmentStatus = a.Status,
                   Date = a.Date,
                   Dogsitter = a.Dogsitter,
                   Owner = a.Owner,
                   StartTime = a.StartTime,
                   EndTime = a.EndTime,
               }).ToList();

            return appointments;
        }

        public ICollection<AppointmentViewModel> GetOwnerAppointmentsToList(string id)
        {
            var appointments = this.appointmentsRepository.All().Where(a => a.Owner.UserId == id)
                .Select(a => new AppointmentViewModel
                {
                    Id = a.Id,
                    AppointmentStatus = a.Status,
                    Date = a.Date,
                    Dogsitter = a.Dogsitter,
                    Owner = a.Owner,
                    StartTime = a.StartTime,
                    EndTime = a.EndTime,
                }).ToList();

            return appointments;
        }

        public async Task RemoveNotification(Notification notification)
        {
            this.notificationsRepository.Delete(notification);
            await this.notificationsRepository.SaveChangesAsync();
        }

        public async Task SendNotificationForAcceptedAppointment(Notification notification)
        {
            await this.notificationsRepository.AddAsync(notification);
            await this.notificationsRepository.SaveChangesAsync();
        }
    }
}
