namespace DogCarePlatform.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using DogCarePlatform.Data.Common.Repositories;
    using DogCarePlatform.Data.Models;
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
               }).OrderByDescending(a => a.AppointmentStatus == AppointmentStatus.Happening).ThenByDescending(a => a.AppointmentStatus == AppointmentStatus.Unprocessed).ThenByDescending(a => a.AppointmentStatus == AppointmentStatus.Processed).ThenByDescending(a => a.Date).ToList();

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
                }).OrderByDescending(a => a.AppointmentStatus == AppointmentStatus.Happening).ThenByDescending(a => a.AppointmentStatus == AppointmentStatus.Unprocessed).ThenByDescending(a => a.AppointmentStatus == AppointmentStatus.Processed).ThenByDescending(a => a.Date).ToList();

            return appointments;
        }

        public async Task StartAppointment(string id)
        {
            var appointment = this.appointmentsRepository.All().FirstOrDefault(a => a.Id == id);

            appointment.Status = AppointmentStatus.Happening;
            await this.appointmentsRepository.SaveChangesAsync();
        }

        public async Task EndAppointment(string id)
        {
            var appointment = this.appointmentsRepository.All().FirstOrDefault(a => a.Id == id);

            appointment.Status = AppointmentStatus.Processed;
            appointment.Timer = Math.Abs(DateTime.UtcNow.Hour - appointment.StartTime.Hour);
            appointment.TaxSoFar = appointment.Timer * appointment.Dogsitter.WageRate;

            await this.appointmentsRepository.SaveChangesAsync();
        }

        public Appointment GetAppointment(string id)
        {
            return this.appointmentsRepository.All().FirstOrDefault(a => a.Id == id);
        }
    }
}
