using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DogCarePlatform.Services.Data
{
    public interface IDogsittersService
    {
        Task CurrentUserAddInfo(string id);
    }
}
