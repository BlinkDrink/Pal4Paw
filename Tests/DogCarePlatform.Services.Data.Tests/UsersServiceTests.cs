using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using DogCarePlatform.Data;
using DogCarePlatform.Data.Models;
using DogCarePlatform.Data.Repositories;
using DogCarePlatform.Services.Mapping;
using DogCarePlatform.Web.ViewModels;
using DogCarePlatform.Web.ViewModels.Administration.Dashboard;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DogCarePlatform.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using Microsoft.AspNetCore.Identity;
    using Moq;
    using Xunit;

    public class UsersServiceTests
    {
        [Fact]
        public void AddUserToRoleShouldAddTheUserToTheRole()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var userRepository = new EfDeletableEntityRepository<ApplicationUser>(new ApplicationDbContext(options));

            var username = "user3";
            var users = new List<ApplicationUser>
            {
                new ApplicationUser { UserName = "user1", },
                new ApplicationUser { UserName = "user2", },
                new ApplicationUser { UserName = username, },
            };

            userRepository.AddRangeAsync(users);
            userRepository.SaveChangesAsync();

            var store = new Mock<IUserStore<ApplicationUser>>();
            var userManager = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);
            userManager.Setup(m => m.FindByNameAsync(username))
                .Returns(Task.FromResult<ApplicationUser>(users.FirstOrDefault(x => x.UserName == username)));

            var usersService = new UsersService(userRepository, userManager.Object);

            var role = "Admin";
            var isUserAddInRole = usersService.AddUserToRole(username, role);

            Assert.True(isUserAddInRole);
        }

        [Fact]
        public void AddUserToRoleShouldReturnFalseWhenInvalidUser()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var userRepository = new EfDeletableEntityRepository<ApplicationUser>(new ApplicationDbContext(options));

            var username = "user3";
            ApplicationUser user = null;

            var store = new Mock<IUserStore<ApplicationUser>>();
            var userManager = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);
            userManager.Setup(m => m.FindByNameAsync(username))
                .Returns(Task.FromResult<ApplicationUser>(user));

            var usersService = new UsersService(userRepository, userManager.Object);

            var role = "Admin";
            var isUserAddInRole = usersService.AddUserToRole(username, role);

            Assert.False(isUserAddInRole);
        }

        [Fact]
        public void RemoveUserFromToRoleShouldReturnFalseWhenInvalidUser()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var userRepository = new EfDeletableEntityRepository<ApplicationUser>(new ApplicationDbContext(options));

            var username = "user3";
            ApplicationUser user = null;

            var store = new Mock<IUserStore<ApplicationUser>>();
            var userManager = new Mock<UserManager<ApplicationUser>>(store.Object, null, null, null, null, null, null, null, null);
            userManager.Setup(m => m.FindByNameAsync(username))
                .Returns(Task.FromResult<ApplicationUser>(user));

            var usersService = new UsersService(userRepository, userManager.Object);

            var role = "Admin";
            var isUserRomeveFromRole = usersService.RemoveUserFromRole(username, role);

            Assert.False(isUserRomeveFromRole);
        }

    }
}
