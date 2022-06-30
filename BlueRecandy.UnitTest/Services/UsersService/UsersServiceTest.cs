using BlueRecandy.Models;
using BlueRecandy.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BlueRecandy.UnitTest.Services.UsersService
{
    public class UsersServiceTest
    {
        [Fact]
        public void GetUserByClaims_ClaimsNotNull_UserIsNotNull()
        {
            // Arrange
            var mockService = new Mock<IUsersService>();
            var principal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, "Alex Vender Joz"),
                new Claim(ClaimTypes.Name, "alexvenderjoz@gmail.com"),
            },  "TestAuthentication"));

            var user = new ApplicationUser();
            user.Id = "1";
            user.Email = "alexvenderjoz@gmail.com";
            user.FullName = "Alex Vender Joz";

            mockService.Setup(m => m.GetUserByClaims(principal)).ReturnsAsync(user);
            var service = mockService.Object;

            // Act
            var actionResult = service.GetUserByClaims(principal);
            var result = actionResult.Result;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(result.Email, user.Email);
        }

        [Fact]
        public void GetUserByClaims_ClaimsNotNull_UserIsNull()
        {
            // Arrange
            var mockService = new Mock<IUsersService>();
            var principal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, "Alex Vender Joz"),
                new Claim(ClaimTypes.Name, "alexvenderjoz@gmail.com"),
            }, "TestAuthentication"));

            var user = new ApplicationUser();
            user.Id = "1";
            user.Email = "alexvenderjoz@gmail.com";
            user.FullName = "Alex Vender Joz";

            mockService.Setup(m => m.GetUserByClaims(principal)).ReturnsAsync(value: null);
            var service = mockService.Object;

            // Act
            var actionResult = service.GetUserByClaims(null);
            var result = actionResult.Result;

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetUserByClaims_ClaimsIsNull_UserIsNull()
        {
            // Arrange
            var mockService = new Mock<IUsersService>();
            var principal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, "Alex Vender Joz"),
                new Claim(ClaimTypes.Name, "alexvenderjoz@gmail.com"),
            }, "TestAuthentication"));

            var user = new ApplicationUser();
            user.Id = "1";
            user.Email = "alexvenderjoz@gmail.com";
            user.FullName = "Alex Vender Joz";

            mockService.Setup(m => m.GetUserByClaims(null)).ReturnsAsync(value: null);
            var service = mockService.Object;

            // Act
            var actionResult = service.GetUserByClaims(null);
            var result = actionResult.Result;

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetUserByEmail_EmailNotNull_UserIsNotNull()
        {
            // Arrange
            var mockService = new Mock<IUsersService>();
            var user = new ApplicationUser();
            user.Id = "1";
            user.Email = "alexvenderjoz@gmail.com";
            user.FullName = "Alex Vender Joz";

            mockService.Setup(m => m.GetUserByEmail("alexvenderjoz@gmail.com")).ReturnsAsync(user);
            var service = mockService.Object;

            // Act
            var actionResult = service.GetUserByEmail("alexvenderjoz@gmail.com");
            var result = actionResult.Result;

            // Assert 
            Assert.NotNull(result);
        }

        [Fact]
        public void GetUserByEmail_EmailNotNull_UserIsNull()
        {
            // Arrange
            var mockService = new Mock<IUsersService>();
            var user = new ApplicationUser();
            user.Id = "1";
            user.Email = "alexvenderjoz@gmail.com";
            user.FullName = "Alex Vender Joz";

            mockService.Setup(m => m.GetUserByEmail("alexvenderjoz@gmail.com")).ReturnsAsync(value: null);
            var service = mockService.Object;

            // Act
            var actionResult = service.GetUserByEmail("alexvenderjoz@gmail.com");
            var result = actionResult.Result;

            // Assert 
            Assert.Null(result);
        }

        [Fact]
        public void GetUserByEmail_EmailIsNull_UserIsNull()
        {
            // Arrange
            var mockService = new Mock<IUsersService>();
            var user = new ApplicationUser();
            user.Id = "1";
            user.Email = "alexvenderjoz@gmail.com";
            user.FullName = "Alex Vender Joz";

            mockService.Setup(m => m.GetUserByEmail(null)).ReturnsAsync(value: null);
            var service = mockService.Object;

            // Act
            var actionResult = service.GetUserByEmail(null);
            var result = actionResult.Result;

            // Assert 
            Assert.Null(result);
        }

        [Fact]
        public void GetUserById_IdNotNull_UserIsNotNull()
        {
            // Arrange
            var mockService = new Mock<IUsersService>();
            var User = new ApplicationUser();
            User.Id = "1";
            User.FullName = "Ardian prasetyo";
            mockService.Setup(m => m.GetUserById("1")).ReturnsAsync(User);
            var service = mockService.Object;

            // Act
            var actionResult = service.GetUserById(User.Id);
            var result = actionResult.Result;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void GetUserById_IdIsNull_UserIsNull()
        {
            var mockService = new Mock<IUsersService>();
            var User = new ApplicationUser();
            User.Id = "1";
            User.FullName = "Ardian prasetyo";
            mockService.Setup(m => m.GetUserById("1")).ReturnsAsync(User);
            var service = mockService.Object;

            // Act
            var actionResult = service.GetUserById(null);
            var result = actionResult.Result;

            // Assert
            Assert.Null(result);

        }
    }
}
