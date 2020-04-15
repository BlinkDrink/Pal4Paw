namespace DogCarePlatform.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    using DogCarePlatform.Data.Models;

    public interface IDogsittersService
    {
        Task CurrentUserAddInfo(string userId, string firstName, string middleName, string lastName, string address, string description, string imageUrl, decimal wageRate);

        Dogsitter GetDogsitterByUserId(string id);

        Dogsitter GetDogsitterByDogsitterId(string id);
    }
}
