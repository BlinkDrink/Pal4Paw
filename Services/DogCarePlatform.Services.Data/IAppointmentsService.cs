﻿namespace DogCarePlatform.Services.Data
{
    using DogCarePlatform.Data.Models;
    using DogCarePlatform.Web.ViewModels.Appointment;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IAppointmentsService
    {
        Notification GetAppointmentFromNotificationById(string id);

        System.Threading.Tasks.Task CreateNewAppointment(Appointment appointment);

        System.Threading.Tasks.Task RemoveNotification(Notification notification);

        System.Threading.Tasks.Task SendNotificationForAcceptedAppointment(Notification notification);

        ICollection<AppointmentViewModel> GetDogsitterAppointmentsToList(string id);

        ICollection<AppointmentViewModel> GetOwnerAppointmentsToList(string id);

    }
}
