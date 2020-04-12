namespace DogCarePlatform.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using DogCarePlatform.Data;
    using DogCarePlatform.Data.Models;
    using DogCarePlatform.Web.ViewModels.Scheduler;
    using Microsoft.AspNetCore.Mvc;

    public class SchedulerController : Controller
    {
        private readonly ApplicationDbContext applicationDbContext;

        public SchedulerController(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }

        public IActionResult Index(string id)
        {
            var viewModel = new DogsitterIdViewModel { DogsitterId = id, };

            return this.View(viewModel);
        }

        public IActionResult GetEvents(string id)
        {
            var events = this.applicationDbContext.Appointments.Where(a => a.DogsitterId == id).ToList();

            return this.Ok(events);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteEvent(string id)
        {
            var status = false;

            var v = this.applicationDbContext.Appointments.Where(a => a.Id == id).FirstOrDefault();

            if (v != null)
            {
                this.applicationDbContext.Appointments.Remove(v);
                await this.applicationDbContext.SaveChangesAsync();

                status = true;
            }

            return this.Ok(status);
        }

        [HttpPost]
        public async Task<IActionResult> SaveEvent(Appointment e)
        {
            var status = false;

            if (e != null)
            {
                //Update the event
                var v = this.applicationDbContext.Appointments.Where(a => a.Id == e.Id).FirstOrDefault();

                if (v != null)
                {
                    v.Id = e.Id;
                    v.StartTime = e.StartTime;
                    v.EndTime = e.EndTime;
                    v.DogsitterId = e.DogsitterId;
                    v.OwnerId = e.OwnerId;
                    v.Status = e.Status;
                    v.Date = e.Date;
                    v.TaxSoFar = e.TaxSoFar;
                    v.Timer = e.Timer;
                }
            }
            else
            {
                return this.NotFound(e);
            }

            await this.applicationDbContext.SaveChangesAsync();
            status = true;

            return Ok(status);
        }
    }
}