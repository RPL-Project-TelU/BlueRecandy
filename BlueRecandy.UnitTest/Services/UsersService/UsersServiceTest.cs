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
            Assert.Equal(result.Email, user.Email);
        }
         
        [Fact]
        public void GetUserByClaims_ClaimsIsNull_UserIsNull()
        {
            // Arrange
            var mockService = new Mock<IUsersService>();
            var user = new ApplicationUser();
            user.Id = "1";
            user.Email = null;
            user.FullName = null;

            mockService.Setup(m => m.GetUserByClaims(null)).ReturnsAsync(user);
            var service = mockService.Object;

            // Act
            var actionResult = service.GetUserByClaims(null);
            var result = actionResult.Result;

            // Assert
            Assert.Equal(result.Email, user.Email);
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
        public void GetUserByEmail_EmailIsNull_UserIsNull()
        {
            // Arrange
            var mockService = new Mock<IUsersService>();
            var user = new ApplicationUser();
            user.Id = "1";
            user.Email = null;
            user.FullName = null;

            mockService.Setup(m => m.GetUserByEmail("alexvenderjoz@gmail.com")).ReturnsAsync(user);
            var service = mockService.Object;

            // Act
            var actionResult = service.GetUserByEmail(null);
            var result = actionResult.Result;

            // Assert 
            Assert.Null(result);
        }
    }
}
