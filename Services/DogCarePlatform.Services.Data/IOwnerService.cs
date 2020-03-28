namespace DogCarePlatform.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using DogCarePlatform.Data.Models;
    using DogCarePlatform.Web.ViewModels.Owner;

    public interface IOwnerService
    {
        Task AddPersonalInfoAsync(string address, string firstName, string middleName, string lastName, Gender gender, string imageUrl, string phoneNumber, ICollection<Dog> dogs, string userId);
    }
}
