using System;
using System.Collections.Generic;
using System.Text;

namespace DogCarePlatform.Data.Models
{
    public class ApplicationUserOwner
    {
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public string OwnerId { get; set; }

        public Owner Owner { get; set; }
    }
}
