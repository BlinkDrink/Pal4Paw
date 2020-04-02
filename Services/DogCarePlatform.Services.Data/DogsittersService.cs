using DogCarePlatform.Data.Common.Repositories;
using DogCarePlatform.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DogCarePlatform.Services.Data
{
    public class DogsittersService : IDogsittersService
    {
        private readonly IDeletableEntityRepository<Dogsitter> dogsitterRepository;

        public DogsittersService(IDeletableEntityRepository<Dogsitter> dogsitterRepository)
        {
            this.dogsitterRepository = dogsitterRepository;
        }

        public Task CurrentUserAddInfo(string id)
        {
            throw new NotImplementedException();
            //var dogsitter = dogsitterRepository.All().Where(d => d.Id == id);
        }
    }
}
