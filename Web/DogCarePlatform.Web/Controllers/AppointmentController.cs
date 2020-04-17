namespace DogCarePlatform.Web.Controllers
{
    using System;
    using System.Globalization;
    using System.Threading.Tasks;

    using DogCarePlatform.Common;
    using DogCarePlatform.Data.Models;
    using DogCarePlatform.Services.Data;
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

        /// <summary>
        /// This action is used by Owners once they receive information about their request.
        /// The request for appointment can be rejected or accepted by the Dogsitter.
        /// Once the notification is clicked it will be dismissed.
        /// </summary>
        /// <param name="id">Notification Id.</param>
        /// <returns>Redirects the owner to his/her appointment page.</returns>
        [Authorize(Roles = "Owner")]
        public async Task<IActionResult> DismissNotificationThenRouteToAppointments(string id)
        {
            var notification = this.notificationsService.GetNotificationById(id);

            await this.notificationsService.RemoveNotification(notification);

            return this.RedirectToAction("OwnerAppointments", "Appointment", new { id = notification.Owner.UserId });
        }

        /// <summary>
        /// This action lists the appointments of the Dogsitter.
        /// </summary>
        /// <param name="id">ApplicationUser Id.</param>
        /// <returns>Returns the View with the according viewModel containing the dogsitter's appointments.</returns>
        [Authorize(Roles = "Dogsitter")]
        public IActionResult DogsitterAppointments(string id)
        {
            var viewModel = this.appointmentsService.GetDogsitterAppointmentsToList(id);

            return this.View(viewModel);
        }

        /// <summary>
        /// This action lists the appointments of the Owner.
        /// </summary>
        /// <param name="id">ApplicationUser Id.</param>
        /// <returns>Returns the View with the according viewModel containing the owner's appointments.</returns>
        [Authorize(Roles="Owner")]
        public IActionResult OwnerAppointments(string id)
        {
            var viewModel = this.appointmentsService.GetOwnerAppointmentsToList(id);

            return this.View(viewModel);
        }

        /// <summary>
        /// This action displays the information about the requested appointment by an Owner.
        /// It contains information about the Date, StartTime and EndTime of an appointment.
        /// Dogsitters can review the appointment and decide whether to accept or reject it.
        /// </summary>
        /// <param name="id">Notification Id.</param>
        /// <returns>Returns a View with viewModel for the appointment's Date, StartTime and EndTime.</returns>
        [Authorize(Roles = "Dogsitter")]
        public IActionResult AppointmentRequest(string id)
        {
            var notification = this.notificationsService.GetAppointmentFromNotificationById(id);
            var startTimeMinutes = notification.StartTime.Minute == 0 ? "00" : notification.StartTime.ToString("mm");
            var endTimeMinutes = notification.EndTime.Minute == 0 ? "00" : notification.EndTime.ToString("mm");

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

        /// <summary>
        /// This is a POST method. Dogsitters accept an appointment, thus saving it to their list
        /// and to the Database. Owners can also see the reflected changes in their Appointments list and
        /// also get a notification with information about the dogsitters decision(either rejected or accepted).
        /// </summary>
        /// <param name="id">Notification Id.</param>
        /// <returns>Redirects the dogsitter to the Home page.</returns>
        [Authorize(Roles = "Dogsitter")]
        [HttpPost]
        public async Task<IActionResult> AcceptAppointment(string id)
        {
            var requestedAppointment = this.notificationsService.GetAppointmentFromNotificationById(id);

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
                Content = GlobalConstants.AcceptedAppointment + requestedAppointment.Dogsitter.FirstName,
                OwnerId = requestedAppointment.OwnerId,
                DogsitterId = requestedAppointment.DogsitterId,
                SentBy = "Dogsitter",
            };

            await this.appointmentsService.CreateNewAppointment(appointment);
            await this.notificationsService.RemoveNotification(requestedAppointment);
            await this.notificationsService.SendNotificationForAcceptedAppointment(notificationToOwner);

            return this.RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// This is a POST method. Dogsitters reject an appointment. Owners can see the reflected changes
        /// in a notification with information about the dogsitters decision(either rejected or accepted).
        /// </summary>
        /// <param name="id">Notification Id.</param>
        /// <returns>Redirects the dogsitter to the Home page.</returns>
        [Authorize(Roles = "Dogsitter")]
        [HttpPost]
        public async Task<IActionResult> RejectAppointment(string id)
        {
            var requestedAppointment = this.notificationsService.GetAppointmentFromNotificationById(id);

            var notification = new Notification
            {
                ReceivedOn = DateTime.UtcNow,
                Content = GlobalConstants.RejectedAppointment + requestedAppointment.Dogsitter.FirstName,
                OwnerId = requestedAppointment.OwnerId,
                DogsitterId = requestedAppointment.DogsitterId,
                SentBy = "Dogsitter",
            };

            await this.notificationsService.RemoveNotification(requestedAppointment);
            await this.notificationsService.SendNotificationForAcceptedAppointment(notification);

            return this.RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// This action manages the begining of an appointment. Dogsitters are the initiators of an appointment.
        /// The start action is done by clicking a button. After that the appointment's status is changed to
        /// Happening.
        /// </summary>
        /// <param name="id">Appointment Id.</param>
        /// <returns>Redirects the dogsitter to the page with a list of his appointments.</returns>
        [Authorize(Roles="Dogsitter")]
        public async Task<IActionResult> StartAppointment(string id)
        {
            await this.appointmentsService.StartAppointment(id);

            var appointment = this.appointmentsService.GetAppointment(id);

            var notification = new Notification
            {
                Content = GlobalConstants.StartedAppointment + appointment.Dogsitter.FirstName,
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

        /// <summary>
        /// With this action the dogsitter can stop the Happening appointment. Thus making it's status to
        /// Processed and changing it's opacity to make it inactive.
        /// </summary>
        /// <param name="id">Appointment Id.</param>
        /// <returns>Redirects the dogsitter to his appointments list page.</returns>
        [Authorize(Roles = "Dogsitter")]
        public async Task<IActionResult> EndAppointment(string id)
        {
            await this.appointmentsService.EndAppointment(id);

            var appointment = this.appointmentsService.GetAppointment(id);

            var notification = new Notification
            {
                Content = GlobalConstants.EndedAppointment + appointment.Dogsitter.FirstName,
                DogsitterId = appointment.DogsitterId,
                OwnerId = appointment.OwnerId,
                Date = DateTime.UtcNow,
                ReceivedOn = DateTime.UtcNow,
                StartTime = appointment.StartTime,
                EndTime = appointment.EndTime,
                SentBy = "Dogsitter",
            };

            await this.notificationsService.SendNotification(notification);

            return this.RedirectToAction("DogsitterSubmitFeedback", "Notification", new { id = notification.Id });
        }
    }
}