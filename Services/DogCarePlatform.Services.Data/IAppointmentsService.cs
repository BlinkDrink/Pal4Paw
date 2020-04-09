namespace DogCarePlatform.Services.Data
{
    using DogCarePlatform.Data.Models;

    public interface IAppointmentsService
    {
        Notification GetAppointmentFromNotification(string id);
    }
}
