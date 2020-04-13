using DogCarePlatform.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DogCarePlatform.Services.Data
{
    public interface IDogsittersService
    {
        Task CurrentUserAddInfo(string userId, string firstName, string middleName, string lastName, DateTime dateOfBirth, string address, string description, string imageUrl, decimal wageRate);

        Dogsitter GetDogsitterByUserId(string id);

        Dogsitter GetDogsitterByDogsitterId(string id);
    }
}
