using System;
using System.Collections.Generic;
using System.Text;

namespace DogCarePlatform.Data.Models
{
    public class ApplicationUserDogsitter
    {
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public string DogsitterId { get; set; }

        public Dogsitter Dogsitter { get; set; }
    }
}
