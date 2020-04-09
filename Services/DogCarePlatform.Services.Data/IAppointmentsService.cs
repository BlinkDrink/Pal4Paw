namespace DogCarePlatform.Services.Data
{
    using DogCarePlatform.Data.Models;
    using System.Threading.Tasks;

    public interface IAppointmentsService
    {
        Notification GetAppointmentFromNotificationById(string id);

        Task CreateNewAppointment(Appointment appointment);

        Task RemoveNotification(Notification notification);

        Task SendNotificationForAcceptedAppointment(Notification notification);
    }
}
