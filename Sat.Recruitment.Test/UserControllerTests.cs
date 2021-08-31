
using Sat.Recruitment.Api.Controllers;
using Sat.Recruitment.ResourceAccess.Entities;
using Sat.Recruitment.ResourceAccess.Responses;
using Sat.Recruitment.Services.Abstractions;
using Sat.Recruitment.Services.Implementations;
using Xunit;

namespace Sat.Recruitment.Test
{
    [CollectionDefinition("Tests", DisableParallelization = true)]
    public class UserControllerTests
    {
        private readonly UsersController _usersController;
        public UserControllerTests()
        {
            IUserService userService = new UserService();
            _usersController = new UsersController(userService);
        }

        [Fact]
        public void CreateUser_WhenCreateNewUser_ReturnsTrue()
        {
            ValidationResponse result = _usersController.CreateUser
                ("Mike", "mike@gmail.com", "Av. Juan G", "+349 1122354215", "Normal", "124").Result;

            Assert.True(result.Success);
            Assert.Equal(User.Validations.UserCreated, result.Message);
        }

        [Fact]
        public void CreateUser_WhenCreateDuplicateUser_ReturnsFalse()
        {
            ValidationResponse result = _usersController.CreateUser
                ("Agustina", "Agustina@gmail.com", "Av. Juan G", "+349 1122354215", "Normal", "124").Result;

            Assert.False(result.Success);
            Assert.Equal(User.Validations.UserDuplicated, result.Message);
        }
    }
}
