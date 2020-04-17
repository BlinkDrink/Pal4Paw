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
        public async void GetDogsitterAppointmentsToListShouldReturnProperCount()
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

            var appointment = new Appointment
            {
                Status = AppointmentStatus.Unprocessed,
                Date = DateTime.UtcNow,
                StartTime = DateTime.UtcNow,
                EndTime = DateTime.UtcNow.AddMinutes(5),
                DogsitterId = dogsitters.First().Id,
                OwnerId = owners.First().Id,
            };

            await appointmentsService.CreateNewAppointment(appointment);
            var appointments = appointmentsService.GetDogsitterAppointmentsToList(appointment.Dogsitter.Id);

            Assert.Equal(1, appointments.Count);
        }

        [Fact]
        public async void GetDogsitterAppointmentsToListShouldReturnProperValues()
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

            var appointment = new Appointment
            {
                Status = AppointmentStatus.Unprocessed,
                Date = DateTime.UtcNow,
                StartTime = DateTime.UtcNow,
                EndTime = DateTime.UtcNow.AddMinutes(5),
                DogsitterId = dogsitters.First().Id,
                OwnerId = owners.First().Id,
            };

            await appointmentsService.CreateNewAppointment(appointment);
            var appointments = appointmentsService.GetDogsitterAppointmentsToList(dogsitters.First().UserId);
            var comparedAppointments = appointment.Id.CompareTo(appointments.First().Id);

            Assert.Equal(0, comparedAppointments);
        }

        [Fact]
        public async void GetOwnerAppointmentsToListShouldReturnProperCount()
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

            var appointment = new Appointment
            {
                Status = AppointmentStatus.Unprocessed,
                Date = DateTime.UtcNow,
                StartTime = DateTime.UtcNow,
                EndTime = DateTime.UtcNow.AddMinutes(5),
                DogsitterId = dogsitters.First().Id,
                OwnerId = owners.First().Id,
            };

            await appointmentsService.CreateNewAppointment(appointment);
            var appointments = appointmentsService.GetDogsitterAppointmentsToList(owners.First().UserId);

            Assert.Equal(1, appointments.Count);
        }

        [Fact]
        public async void GetOwnerAppointmentsToListShouldReturnProperValues()
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

            var appointment = new Appointment
            {
                Status = AppointmentStatus.Unprocessed,
                Date = DateTime.UtcNow,
                StartTime = DateTime.UtcNow,
                EndTime = DateTime.UtcNow.AddMinutes(5),
                DogsitterId = dogsitters.First().Id,
                OwnerId = owners.First().Id,
            };

            await appointmentsService.CreateNewAppointment(appointment);

            var appointments = appointmentsService.GetOwnerAppointmentsToList(owners.First().UserId);
            var comparedAppointments = appointment.Id.CompareTo(appointments.First().Id);

            Assert.Equal(0, comparedAppointments);
        }
    }
}
