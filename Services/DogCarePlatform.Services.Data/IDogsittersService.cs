using DogCarePlatform.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DogCarePlatform.Services.Data
{
    public interface IDogsittersService
    {
        Task CurrentUserAddInfo(string userId, string firstName, string middleName, string lastName, DateTime dateOfBirth, Gender gender, string address, string description, string imageUrl);
        Dogsitter GetDogsitterById(string id);
    }
}
