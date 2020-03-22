namespace DogCarePlatform.Data.Models
{
    using System;

    public class Dog
    {
        public Dog()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }

        public string Breed { get; set; }

        public string Description { get; set; }
    }
}
