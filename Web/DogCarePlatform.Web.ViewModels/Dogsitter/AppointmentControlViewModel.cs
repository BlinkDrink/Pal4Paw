namespace DogCarePlatform.Web.ViewModels.Dogsitter
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using DogCarePlatform.Data.Models;

    public class AppointmentControlViewModel
    {
        public string Id { get; set; }

        public string StartTime { get; set; }

        public string EndTime { get; set; }

        public string Date { get; set; }

        public Owner Owner { get; set; }

        public Dogsitter Dogsitter { get; set; }
    }
}
