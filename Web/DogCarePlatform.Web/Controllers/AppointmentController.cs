namespace DogCarePlatform.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
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
            var notification = this.appointmentsService.GetAppointmentFromNotification(id);
            var startTime = notification.StartTime.Hour.ToString() + ":" + notification.StartTime.Minute.ToString();
            var endTime = notification.EndTime.Hour.ToString() + ":" + notification.EndTime.Minute.ToString();

            var viewModel = new AppointmentControlViewModel
            {
                Date = notification.Date.ToString("dd/mm/yyyy"),
                StartTime = startTime,
                EndTime = endTime,
                Dogsitter = notification.Dogsitter,
                Owner = notification.Owner,
            };

            return this.View(viewModel);
        }
    }
}