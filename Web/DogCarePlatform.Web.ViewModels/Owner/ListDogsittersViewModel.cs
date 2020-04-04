namespace DogCarePlatform.Web.ViewModels.Owner
{
    using System.Collections.Generic;

    using DogCarePlatform.Data.Models;

    public class ListDogsittersViewModel
    {
        public ICollection<Dogsitter> Dogsitters { get; set; }
    }
}
