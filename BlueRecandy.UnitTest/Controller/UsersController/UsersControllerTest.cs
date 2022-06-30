using BlueRecandy.Controllers;
using BlueRecandy.Models;
using BlueRecandy.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BlueRecandy.UnitTest.Controller.UsersController
{
	public class UsersControllerTest
	{

		[Fact]
		public void Index()
		{
			// Arrange
			var mockUserService = new Mock<IUsersService>();
			var mockProductService = new Mock<IProductsService>();
			var mockLogService = new Mock<IPurchaseLogsService>();

			var principal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
			{
				new Claim(ClaimTypes.NameIdentifier, "Alex Vender Joz"),
				new Claim(ClaimTypes.Name, "alexvenderjoz@gmail.com"),
			}, "TestAuthentication"));

			var user = new ApplicationUser();
			user.Id = "1";
			user.Email = "alexvenderjoz@gmail.com";
			user.FullName = "Alex Vender Joz";

			mockUserService.Setup(x => x.GetUserByClaims(principal)).ReturnsAsync(user);

			var products = new List<Product>()
			{
				new Product(){ Id = 1, Name = "Test", Owner = user, OwnerId = user.Id }
			}.AsEnumerable();
			mockProductService.Setup(x => x.GetProductsByOwner(user.Id)).Returns(products);

			var logs = new List<PurchaseLog>()
			{
				new PurchaseLog() { UserId = user.Id, ProductId = 23}
			};
			mockLogService.Setup(x => x.GetPurchaseLogsByUserId(user.Id)).Returns(logs);

			var httpContext = new DefaultHttpContext() { User = principal };
			var context = new ControllerContext() { HttpContext = httpContext };
			var controller = new UserController(mockUserService.Object, mockProductService.Object, mockLogService.Object);
			controller.ControllerContext = context;
			// Act
			var actionResult = controller.Index();
			var result = actionResult.Result;

			// Assert
			Assert.NotNull(result);
			var view = Assert.IsType<ViewResult>(result);
			var model = view.Model as IEnumerable<Product>;
			Assert.NotNull(model);
			Assert.Single(model);
		}


		[Fact]
		public void ManageYourProducts()
		{
			// Arrange
			var mockUserService = new Mock<IUsersService>();
			var mockProductService = new Mock<IProductsService>();
			var mockLogService = new Mock<IPurchaseLogsService>();

			var user = new ApplicationUser() { Id = "user_id", Email = "test@gmail.com", FullName = "Tester" };
			var principal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
			{
				new Claim(ClaimTypes.NameIdentifier, "user_id"),
				new Claim(ClaimTypes.Email, "test@gmail.com")
			}));
			mockUserService.Setup(x => x.GetUserByClaims(principal)).ReturnsAsync(user);

			var products = new List<Product>()
			{
				new Product(){ Id = 1, Name = "product", Owner = user, OwnerId = user.Id }
			}.AsEnumerable();
			mockProductService.Setup(x => x.GetProductsByOwner(user.Id)).Returns(products);

			var httpContext = new DefaultHttpContext() { User = principal};
			var controllerContext = new ControllerContext() { HttpContext = httpContext };
			var controller = new UserController(mockUserService.Object, mockProductService.Object, mockLogService.Object)
			{
				ControllerContext = controllerContext
			};

			// Act
			var actionResult = controller.ManageYourProducts();
			var result = actionResult.Result;

			// Assert
			Assert.NotNull(result);
			var view = Assert.IsType<ViewResult>(result);
			var model = view.Model as ApplicationUser;
			Assert.NotNull(model);
			Assert.Equal(user, model);
		}

	}
}
