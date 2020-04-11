namespace DogCarePlatform.Web.Controllers
{
    using System;
    using System.Globalization;
    using System.Threading.Tasks;

    using DogCarePlatform.Data.Models;
    using DogCarePlatform.Services.Data;
    using DogCarePlatform.Web.ViewModels.Appointment;
    using DogCarePlatform.Web.ViewModels.Dogsitter;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class AppointmentController : Controller
    {
        private readonly IAppointmentsService appointmentsService;

        public AppointmentController(IAppointmentsService appointmentsService)
        {
            this.appointmentsService = appointmentsService;
        }

        public IActionResult Index()
        {
            return this.View();
        }

        [Authorize(Roles = "Dogsitter")]
        public IActionResult DogsitterAppointments(string id)
        {
            var viewModel = this.appointmentsService.GetDogsitterAppointmentsToList(id);

            return this.View(viewModel);
        }

        [Authorize(Roles ="Owner")]
        public IActionResult OwnerAppointments(string id)
        {
            var viewModel = this.appointmentsService.GetOwnerAppointmentsToList(id);

            return this.View(viewModel);
        }

        [Authorize(Roles="Dogsitter")]
        public IActionResult GetAppointmentFromNotification(string id)
        {
            var notification = this.appointmentsService.GetAppointmentFromNotificationById(id);
            var startTimeMinutes = notification.StartTime.Minute == 0 ? "00" : notification.StartTime.Minute.ToString();
            var endTimeMinutes = notification.EndTime.Minute == 0 ? "00" : notification.EndTime.Minute.ToString();

            var startTime = notification.StartTime.Hour.ToString() + ":" + startTimeMinutes;
            var endTime = notification.EndTime.Hour.ToString() + ":" + endTimeMinutes;

            var viewModel = new AppointmentControlViewModel
            {
                Id = notification.Id,
                Date = notification.Date.ToString("dddd, dd MMMM yyyy", new CultureInfo("bg-BG")),
                StartTime = startTime,
                EndTime = endTime,
                Dogsitter = notification.Dogsitter,
                Owner = notification.Owner,
            };

            return this.View(viewModel);
        }

        [Authorize(Roles = "Dogsitter")]
        [HttpPost]
        public async Task<IActionResult> AcceptAppointment(string id)
        {
            var requestedAppointment = this.appointmentsService.GetAppointmentFromNotificationById(id);

            var appointment = new Appointment
            {
                Status = AppointmentStatus.Unprocessed,
                Timer = 0,
                Date = requestedAppointment.Date,
                StartTime = requestedAppointment.StartTime,
                EndTime = requestedAppointment.EndTime,
                OwnerId = requestedAppointment.OwnerId,
                DogsitterId = requestedAppointment.DogsitterId,
            };

            var notificationToOwner = new Notification
            {
                ReceivedOn = DateTime.UtcNow,
                Content = $"Вашата заявка беше одобрена от <p class=\"text-amber\">{requestedAppointment.Dogsitter.FirstName}</p>",
                OwnerId = requestedAppointment.OwnerId,
                DogsitterId = requestedAppointment.DogsitterId,
            };

            await this.appointmentsService.CreateNewAppointment(appointment);
            await this.appointmentsService.RemoveNotification(requestedAppointment);
            await this.appointmentsService.SendNotificationForAcceptedAppointment(notificationToOwner);

            return this.RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "Dogsitter")]
        [HttpPost]
        public async Task<IActionResult> RejectAppointment(string id)
        {
            var requestedAppointment = this.appointmentsService.GetAppointmentFromNotificationById(id);

            var notificationToOwner = new Notification
            {
                ReceivedOn = DateTime.UtcNow,
                Content = $"Вашата заявка до <p class=\"text-amber\">{requestedAppointment.Dogsitter.FirstName} беше <b class=\"red-text\">отхвърлена</b></p>",
                OwnerId = requestedAppointment.OwnerId,
                DogsitterId = requestedAppointment.DogsitterId,
            };

            await this.appointmentsService.RemoveNotification(requestedAppointment);
            await this.appointmentsService.SendNotificationForAcceptedAppointment(notificationToOwner);

            return this.RedirectToAction("Index", "Home");
        }
    }
}