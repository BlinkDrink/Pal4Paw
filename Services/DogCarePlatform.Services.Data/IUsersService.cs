namespace DogCarePlatform.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using DogCarePlatform.Data.Models;

    public interface IUsersService
    {
        ApplicationUser GetUserByUsername(string username);

        bool AddUserToRole(string username, string role);

        bool RemoveUserFromRole(string name, string role);

        IEnumerable<ApplicationUser> GetUsersByRole(string role);

        //void EditFirstName(XeonUser user, string firstName);

        //void EditLastName(XeonUser user, string lastName);
    }
}
