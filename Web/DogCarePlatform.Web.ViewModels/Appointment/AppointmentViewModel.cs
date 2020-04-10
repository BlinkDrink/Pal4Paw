namespace DogCarePlatform.Web.ViewModels.Appointment
{
    using System;

    using DogCarePlatform.Data.Models;
    using DogCarePlatform.Services.Mapping;

    public class AppointmentViewModel : IMapFrom<Appointment>
    {
        public string Id { get; set; }

        public AppointmentStatus AppointmentStatus { get; set; }

        public DateTime Date { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public Owner Owner { get; set; }

        public Dogsitter Dogsitter { get; set; }
    }
}