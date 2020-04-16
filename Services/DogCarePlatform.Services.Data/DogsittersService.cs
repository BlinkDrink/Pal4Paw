namespace DogCarePlatform.Services.Data
{
    using System.Linq;
    using System.Threading.Tasks;

    using DogCarePlatform.Data.Common.Repositories;
    using DogCarePlatform.Data.Models;

    public class DogsittersService : IDogsittersService
    {
        private readonly IDeletableEntityRepository<Dogsitter> dogsitterRepository;

        public DogsittersService(IDeletableEntityRepository<Dogsitter> dogsitterRepository)
        {
            this.dogsitterRepository = dogsitterRepository;
        }

        /// <summary>
        /// This method handles the profile information of a dogsitter.
        /// After being approved by Administrator, the dogsitters need to fill their profile information.
        /// It is called upon successfully filled profile input-fields.
        /// </summary>
        /// <param name="userId">ApplicationUser Id.</param>
        /// <param name="firstName">Dogsitter First Name.</param>
        /// <param name="middleName">Dogsitter Middle Name.</param>
        /// <param name="lastName">Dogsitter Last Name.</param>
        /// <param name="address">Dogsitter Address.</param>
        /// <param name="description">Dogsitter personal description.</param>
        /// <param name="imageUrl">Dogsitter Image URL.</param>
        /// <param name="wageRate">Dogsitter Wage Rate.</param>
        /// <returns>Adds the current information to the according dogsitter. Saves the changes.</returns>
        public async Task CurrentUserAddInfo(string userId, string firstName, string middleName, string lastName, string address, string description, string imageUrl, decimal wageRate)
        {
            var dogsitter = this.dogsitterRepository.All().Where(d => d.UserId == userId).FirstOrDefault();

            dogsitter.FirstName = firstName;
            dogsitter.MiddleName = middleName;
            dogsitter.LastName = lastName;
            dogsitter.Address = address;
            dogsitter.Description = description;
            dogsitter.ImageUrl = imageUrl;
            dogsitter.WageRate = wageRate;

            await this.dogsitterRepository.SaveChangesAsync();
        }

        /// <summary>
        /// This method returns the dogsitter with the given ApplicationUser Id.
        /// </summary>
        /// <param name="id">ApplicationUser Id.</param>
        /// <returns>Dogsitter.</returns>
        public Dogsitter GetDogsitterByUserId(string id)
        {
            return this.dogsitterRepository.All().Where(d => d.UserId == id).FirstOrDefault();
        }

        /// <summary>
        /// This method returns the dogsitter with the given Dogsitter Id.
        /// </summary>
        /// <param name="id">Dogsitter Id.</param>
        /// <returns>Dogsitter.</returns>
        public Dogsitter GetDogsitterByDogsitterId(string id)
        {
            return this.dogsitterRepository.All().Where(d => d.Id == id).FirstOrDefault();
        }
    }
}
