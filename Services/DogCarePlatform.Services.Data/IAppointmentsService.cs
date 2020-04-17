namespace DogCarePlatform.Services.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using DogCarePlatform.Data.Models;
    using DogCarePlatform.Web.ViewModels.Appointment;

    public interface IAppointmentsService
    {
        Task CreateNewAppointment(Appointment appointment);

        ICollection<AppointmentViewModel> GetDogsitterAppointmentsToList(string id);

        ICollection<AppointmentViewModel> GetOwnerAppointmentsToList(string id);

        Task StartAppointment(string id);

        Task EndAppointment(string id);

        Appointment GetAppointment(string id);
    }
}
