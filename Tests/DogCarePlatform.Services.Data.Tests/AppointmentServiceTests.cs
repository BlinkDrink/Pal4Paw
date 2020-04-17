namespace DogCarePlatform.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DogCarePlatform.Data;
    using DogCarePlatform.Data.Common.Repositories;
    using DogCarePlatform.Data.Models;
    using DogCarePlatform.Data.Repositories;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using Xunit;

    public class AppointmentServiceTests
    {
        [Fact]
        public async void CreateNewAppointmentShouldCreateApointment()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("Appointment_CreateAppointment_Database");
            var appointmentsRepository = new EfDeletableEntityRepository<Appointment>(new ApplicationDbContext(options.Options));
            var notificationsRepository = new EfDeletableEntityRepository<Notification>(new ApplicationDbContext(options.Options));

            var appointmentsService = new AppointmentsService(notificationsRepository, appointmentsRepository);

            await appointmentsService.CreateNewAppointment(new Appointment
            {
                Status = AppointmentStatus.Unprocessed,
                Date = DateTime.UtcNow,
                StartTime = DateTime.UtcNow,
                EndTime = DateTime.UtcNow.AddMinutes(5),
                DogsitterId = "123",
                OwnerId = "321",
            });

            Assert.Equal(1, appointmentsRepository.All().Count());
        }

        [Fact]
        public async void GetAppointmentShouldReturnCorrectAppointment()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("Appointment_GetAppointment_Database");
            var appointmentsRepository = new EfDeletableEntityRepository<Appointment>(new ApplicationDbContext(options.Options));
            var notificationsRepository = new EfDeletableEntityRepository<Notification>(new ApplicationDbContext(options.Options));

            var appointmentsService = new AppointmentsService(notificationsRepository, appointmentsRepository);

            var appointment = new Appointment
            {
                Status = AppointmentStatus.Unprocessed,
                Date = DateTime.UtcNow,
                StartTime = DateTime.UtcNow,
                EndTime = DateTime.UtcNow.AddMinutes(5),
                DogsitterId = "123",
                OwnerId = "321",
            };

            await appointmentsService.CreateNewAppointment(appointment);

            Assert.Equal(appointment, appointmentsService.GetAppointment(appointment.Id));
        }

        [Fact]
        public async void StartAppointmentShouldChangeTheStatusToHappening()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("Appointment_GetAppointment_Database");
            var appointmentsRepository = new EfDeletableEntityRepository<Appointment>(new ApplicationDbContext(options.Options));
            var notificationsRepository = new EfDeletableEntityRepository<Notification>(new ApplicationDbContext(options.Options));

            var appointmentsService = new AppointmentsService(notificationsRepository, appointmentsRepository);

            var appointment = new Appointment
            {
                Status = AppointmentStatus.Unprocessed,
                Date = DateTime.UtcNow,
                StartTime = DateTime.UtcNow,
                EndTime = DateTime.UtcNow.AddMinutes(5),
                DogsitterId = "123",
                OwnerId = "321",
            };

            await appointmentsService.CreateNewAppointment(appointment);
            await appointmentsService.StartAppointment(appointment.Id);

            Assert.Equal(AppointmentStatus.Happening.ToString(), appointmentsService.GetAppointment(appointment.Id).Status.ToString());
        }

        [Theory]
        [InlineData(AppointmentStatus.Happening)]
        [InlineData(AppointmentStatus.Unprocessed)]
        public async void EndAppointmentShouldChangeTheStatusToProcessed(AppointmentStatus status)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("Appointment_GetAppointment_Database");
            var appointmentsRepository = new EfDeletableEntityRepository<Appointment>(new ApplicationDbContext(options.Options));
            var notificationsRepository = new EfDeletableEntityRepository<Notification>(new ApplicationDbContext(options.Options));

            var appointmentsService = new AppointmentsService(notificationsRepository, appointmentsRepository);

            var appointment = new Appointment
            {
                Status = status,
                Date = DateTime.UtcNow,
                StartTime = DateTime.UtcNow,
                EndTime = DateTime.UtcNow.AddMinutes(5),
                DogsitterId = "123",
                OwnerId = "321",
            };

            await appointmentsService.CreateNewAppointment(appointment);
            await appointmentsService.StartAppointment(appointment.Id);

            Assert.Equal(AppointmentStatus.Happening.ToString(), appointmentsService.GetAppointment(appointment.Id).Status.ToString());
        }

        [Fact]
        public async void GetDogsittersAppointmentsToListShouldReturnProperValues()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("Appointment_GetAppointment_Database");
            var appointmentsRepository = new EfDeletableEntityRepository<Appointment>(new ApplicationDbContext(options.Options));
            var notificationsRepository = new EfDeletableEntityRepository<Notification>(new ApplicationDbContext(options.Options));

            var appointmentsService = new AppointmentsService(notificationsRepository, appointmentsRepository);

            var dogsitters = new List<Dogsitter>
            {
                new Dogsitter(),
            };

            var owners = new List<Owner>
            {
                new Owner(),
            };

            var user = new ApplicationUser
            {
                UserName = "user@user.com",
                Email = "user@user.com",
                Dogsitters = dogsitters,
                Owners = owners,
            };

            var appointment = new Appointment
            {
                Status = AppointmentStatus.Unprocessed,
                Date = DateTime.UtcNow,
                StartTime = DateTime.UtcNow,
                EndTime = DateTime.UtcNow.AddMinutes(5),
                DogsitterId = owners.First().Id,
                OwnerId = dogsitters.First().Id,
            };

            await appointmentsService.CreateNewAppointment(appointment);
            var appointments = appointmentsService.GetDogsitterAppointmentsToList(dogsitters.First().UserId);

            Assert.Equal(1, appointments.Count);
        }
    }
}
