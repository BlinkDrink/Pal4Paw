namespace DogCarePlatform.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;
    using DogCarePlatform.Data.Models;
    using DogCarePlatform.Services.Data;
    using DogCarePlatform.Web.ViewModels.Dogsitter;
    using Microsoft.AspNetCore.Mvc;

    public class AppointmentController : Controller
    {
        private readonly IAppointmentsService appointmentsService;

        public AppointmentController(IAppointmentsService appointmentsService)
        {
            this.appointmentsService = appointmentsService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetAppointmentFromNotification(string id)
        {
            var notification = this.appointmentsService.GetAppointmentFromNotificationById(id);
            var startTimeMinutes = notification.StartTime.Minute == 0 ? "00" : notification.StartTime.Minute.ToString();
            var endTimeMinutes = notification.EndTime.Minute == 0 ? "00" : notification.EndTime.Minute.ToString();

            var startTime = notification.StartTime.Hour.ToString() + ":" + startTimeMinutes;
            var endTime = notification.EndTime.Hour.ToString() + ":" + endTimeMinutes;

            var viewModel = new AppointmentControlViewModel
            {
                Date = notification.Date.ToString("dddd, dd MMMM yyyy", new CultureInfo("bg-BG")),
                StartTime = startTime,
                EndTime = endTime,
                Dogsitter = notification.Dogsitter,
                Owner = notification.Owner,
            };

            return this.View(viewModel);
        }

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

            await this.appointmentsService.CreateNewAppointment(appointment);

            return this.RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> RejectAppointment(string id)
        {
            throw new NotImplementedException();
        }
    }
}