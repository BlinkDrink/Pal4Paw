// ReSharper disable VirtualMemberCallInConstructor
namespace DogCarePlatform.Data.Models
{
    using System;
    using System.Collections.Generic;

    using DogCarePlatform.Data.Common.Models;

    using Microsoft.AspNetCore.Identity;

    public class ApplicationUser : IdentityUser, IAuditInfo, IDeletableEntity
    {
        public ApplicationUser()
        {
            // Application user
            this.Id = Guid.NewGuid().ToString();
            this.Roles = new HashSet<IdentityUserRole<string>>();
            this.Claims = new HashSet<IdentityUserClaim<string>>();
            this.Logins = new HashSet<IdentityUserLogin<string>>();

            // Dogsitters and Owners
            this.Dogsitters = new HashSet<Dogsitter>();
            this.Owners = new HashSet<Owner>();
        }

        // Audit info
        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        // Deletable entity
        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public virtual ICollection<IdentityUserRole<string>> Roles { get; set; }

        public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; }

        public virtual ICollection<IdentityUserLogin<string>> Logins { get; set; }

        public virtual ICollection<Dogsitter> Dogsitters { get; set; }

        public virtual ICollection<Owner> Owners { get; set; }
    }
}
