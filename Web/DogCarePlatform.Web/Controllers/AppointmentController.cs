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
        private readonly INotificationsService notificationsService;

        public AppointmentController(IAppointmentsService appointmentsService, INotificationsService notificationsService)
        {
            this.appointmentsService = appointmentsService;
            this.notificationsService = notificationsService;
        }

        public IActionResult Index()
        {
            return this.View();
        }

        public async Task<IActionResult> DismissNotificationThenRouteToAppointments(string id)
        {
            var notification = this.notificationsService.GetNotificationById(id);

            await this.appointmentsService.RemoveNotification(notification);

            return this.RedirectToAction("OwnerAppointments", notification.Owner.UserId);
        }

        [Authorize(Roles = "Dogsitter")]
        public IActionResult DogsitterAppointments(string id)
        {
            var viewModel = this.appointmentsService.GetDogsitterAppointmentsToList(id);

            return this.View(viewModel);
        }

        [Authorize(Roles="Owner")]
        public IActionResult OwnerAppointments(string id)
        {
            var viewModel = this.appointmentsService.GetOwnerAppointmentsToList(id);

            return this.View(viewModel);
        }

        [Authorize(Roles = "Dogsitter")]
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
                Content = $"Вашата заявка беше одобрена от {requestedAppointment.Dogsitter.FirstName}",
                OwnerId = requestedAppointment.OwnerId,
                DogsitterId = requestedAppointment.DogsitterId,
                SentBy = "Dogsitter",
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

            var notification = new Notification
            {
                ReceivedOn = DateTime.UtcNow,
                Content = $"Вашата заявка до {requestedAppointment.Dogsitter.FirstName} беше отхвърлена.",
                OwnerId = requestedAppointment.OwnerId,
                DogsitterId = requestedAppointment.DogsitterId,
                SentBy = "Dogsitter",
            };

            await this.appointmentsService.RemoveNotification(requestedAppointment);
            await this.appointmentsService.SendNotificationForAcceptedAppointment(notification);

            return this.RedirectToAction("Index", "Home");
        }

        [Authorize(Roles="Dogsitter")]
        public async Task<IActionResult> StartAppointment(string id)
        {
            await this.appointmentsService.StartAppointment(id);

            var appointment = this.appointmentsService.GetAppointment(id);

            var notification = new Notification
            {
                Content = $"Вашата уговорка с {appointment.Dogsitter.FirstName} започна.",
                DogsitterId = appointment.DogsitterId,
                OwnerId = appointment.OwnerId,
                Date = DateTime.UtcNow,
                ReceivedOn = DateTime.UtcNow,
                StartTime = appointment.StartTime,
                EndTime = appointment.EndTime,
                SentBy = "Dogsitter",
            };

            await this.notificationsService.SendNotification(notification);

            return this.RedirectToAction("DogsitterAppointments", new { id = appointment.Dogsitter.UserId });
        }

        [Authorize(Roles = "Dogsitter")]
        public async Task<IActionResult> EndAppointment(string id)
        {
            await this.appointmentsService.EndAppointment(id);

            var appointment = this.appointmentsService.GetAppointment(id);

            var notification = new Notification
            {
                Content = $"Вашата уговорка с {appointment.Dogsitter.FirstName} приключи.",
                DogsitterId = appointment.DogsitterId,
                OwnerId = appointment.OwnerId,
                Date = DateTime.UtcNow,
                ReceivedOn = DateTime.UtcNow,
                StartTime = appointment.StartTime,
                EndTime = appointment.EndTime,
                SentBy = "Dogsitter",
            };

            await this.notificationsService.SendNotification(notification);

            return this.RedirectToAction("NotificationAfterEndOfAppointment", "Notification", new { id = notification.Id });
        }
    }
}