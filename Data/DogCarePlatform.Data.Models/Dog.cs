namespace DogCarePlatform.Data.Models
{
    using System;

    using DogCarePlatform.Data.Common.Models;

    public class Dog : BaseDeletableModel<int>
    {
        public string Name { get; set; }

        public int Age { get; set; }

        public string Breed { get; set; }

        public string Description { get; set; }
    }
}
