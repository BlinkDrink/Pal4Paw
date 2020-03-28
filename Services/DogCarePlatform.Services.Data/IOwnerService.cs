namespace DogCarePlatform.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using DogCarePlatform.Web.ViewModels.Person;

    public interface IOwnerService
    {
        Task AddPersonalInfoAsync();
    }
}
