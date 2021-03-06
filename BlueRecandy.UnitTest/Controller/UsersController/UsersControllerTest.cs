using BlueRecandy.Controllers;
using BlueRecandy.Models;
using BlueRecandy.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Xunit;
using static BlueRecandy.Controllers.UserController;

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
			var page = view.ViewData["ManagementPage"];
			Assert.NotNull(page);
			Assert.Equal(ManagementPage.Summary, page);
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
			var page = view.ViewData["ManagementPage"];
			Assert.NotNull(page);
			Assert.Equal(ManagementPage.YourProducts, page);
		}

		[Fact]
		public void ManagePurchaseLogs()
		{
			// Arrange 
			var mockUserService = new Mock<IUsersService>();
			var mockProductService = new Mock<IProductsService>();
			var mockLogService = new Mock<IPurchaseLogsService>();

			var user = new ApplicationUser() { Id = "11231", Email = "a@gmail.com" };
			var principal = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
			{
				new Claim(ClaimTypes.NameIdentifier, user.Id),
				new Claim(ClaimTypes.Email, user.Email)
			}));
			mockUserService.Setup(x => x.GetUserByClaims(principal)).ReturnsAsync(user);

			var httpContext = new DefaultHttpContext() { User = principal };
			var controllerContext = new ControllerContext() { HttpContext = httpContext };
			var controller = new UserController(mockUserService.Object, mockProductService.Object, mockLogService.Object)
			{
				ControllerContext = controllerContext
			};
			// Act
			var actionResult = controller.ManagePurchaseLogs();
			var result = actionResult.Result;

			// Assert
			Assert.NotNull(result);
			var view = Assert.IsType<ViewResult>(result);
			var page = view.ViewData["ManagementPage"];
			Assert.NotNull(page);
			Assert.Equal(ManagementPage.PurchaseLogs, page);
		}

		[Fact]
		public void Purchase_ProductIdIsNull_ShouldNotFound()
		{
			// Arrange
			var mockUserService = new Mock<IUsersService>();
			var mockProductService = new Mock<IProductsService>();
			var mockLogService = new Mock<IPurchaseLogsService>();

			var controller = new UserController(mockUserService.Object, mockProductService.Object, mockLogService.Object);
			// Act
			var actionResult = controller.Purchase(null);
			var result = actionResult.Result;

			// Assert
			Assert.NotNull(result);
			Assert.IsType<NotFoundResult>(result);
		}

		[Fact]
		public void Purchase_ProductIdNotNull_ProductNotExists_ShouldNotFound()
		{
			// Arrange
			var mockUserService = new Mock<IUsersService>();
			var mockProductService = new Mock<IProductsService>();
			var mockLogService = new Mock<IPurchaseLogsService>();

			mockProductService.Setup(x => x.GetProductById(1)).ReturnsAsync(value: null);

			var controller = new UserController(mockUserService.Object, mockProductService.Object, mockLogService.Object);
			// Act
			var actionResult = controller.Purchase(1);
			var result = actionResult.Result;

			// Assert
			Assert.NotNull(result);
			Assert.IsType<NotFoundResult>(result);
		}

		[Fact]
		public void Purchase_ProductIdNotNull_UserNotFound_ShouldNotFound()
		{
			// Arrange
			var mockUserService = new Mock<IUsersService>();
			var mockProductService = new Mock<IProductsService>();
			var mockLogService = new Mock<IPurchaseLogsService>();

			var product = new Product() { Id = 1, Name = "test" };
			mockProductService.Setup(x => x.GetProductById(1)).ReturnsAsync(product);

			var principal = new ClaimsPrincipal(new ClaimsIdentity(new[]
			{
				new Claim(ClaimTypes.NameIdentifier, "none")
			}));
			mockUserService.Setup(x => x.GetUserByClaims(principal)).ReturnsAsync(It.IsAny<ApplicationUser>());

			var controller = new UserController(mockUserService.Object, mockProductService.Object, mockLogService.Object);
			// Act
			var actionResult = controller.Purchase(1);
			var result = actionResult.Result;

			// Assert
			Assert.NotNull(result);
			Assert.IsType<NotFoundResult>(result);
		}

		[Fact]
		public void Purchase_ProductIdNotNull_UserIsFound_HasPurchaseLog_ShouldToDetailsView()
		{
			// Arrange
			var mockUserService = new Mock<IUsersService>();
			var mockProductService = new Mock<IProductsService>();
			var mockLogService = new Mock<IPurchaseLogsService>();

			var user = new ApplicationUser() { Id = "11212", Email = "a@gmail.com" };
			var principal = new ClaimsPrincipal(new ClaimsIdentity(new[]
			{
				new Claim(ClaimTypes.NameIdentifier, user.Id),
				new Claim(ClaimTypes.Email, user.Email)
			}));
			mockUserService.Setup(x => x.GetUserByClaims(principal)).ReturnsAsync(user);

			var log = new PurchaseLog() { UserId = user.Id, ProductId = 1 };
			var logs = new List<PurchaseLog>() { log };
			var product = new Product() { Id = 1, Name = "test", OwnerId = "other_user", PurchaseLogs = logs };
			mockProductService.Setup(x => x.GetProductById(1)).ReturnsAsync(product);

			var httpContext = new DefaultHttpContext() { User = principal };
			var controllerContext = new ControllerContext() { HttpContext = httpContext };
			var controller = new UserController(mockUserService.Object, mockProductService.Object, mockLogService.Object)
			{
				ControllerContext = controllerContext
			};
			// Act
			var actionResult = controller.Purchase(1);
			var result = actionResult.Result;

			// Assert
			Assert.NotNull(result);
			var redirect = Assert.IsType<RedirectToActionResult>(result);
			var actionName = redirect.ActionName;
			Assert.Equal("Details", actionName);
		}

		[Fact]
		public void Purchase_ProductIdNotNull_UserIsFound_IsOwner_ShouldToDetailsView()
		{
			// Arrange
			var mockUserService = new Mock<IUsersService>();
			var mockProductService = new Mock<IProductsService>();
			var mockLogService = new Mock<IPurchaseLogsService>();

			var user = new ApplicationUser() { Id = "11212", Email = "a@gmail.com" };
			var principal = new ClaimsPrincipal(new ClaimsIdentity(new[]
			{
				new Claim(ClaimTypes.NameIdentifier, user.Id),
				new Claim(ClaimTypes.Email, user.Email)
			}));
			mockUserService.Setup(x => x.GetUserByClaims(principal)).ReturnsAsync(user);

			var logs = new List<PurchaseLog>();
			var product = new Product() { Id = 1, Name = "test", OwnerId = user.Id, PurchaseLogs = logs };
			mockProductService.Setup(x => x.GetProductById(1)).ReturnsAsync(product);

			var httpContext = new DefaultHttpContext() { User = principal };
			var controllerContext = new ControllerContext() { HttpContext = httpContext };
			var controller = new UserController(mockUserService.Object, mockProductService.Object, mockLogService.Object)
			{
				ControllerContext = controllerContext
			};
			// Act
			var actionResult = controller.Purchase(1);
			var result = actionResult.Result;

			// Assert
			Assert.NotNull(result);
			var redirect = Assert.IsType<RedirectToActionResult>(result);
			var actionName = redirect.ActionName;
			Assert.Equal("Details", actionName);
		}

		[Fact]
		public void Purchase_ProductIdNotNull_UserIsFound_NotOwner_HasEnoughMoney_ShouldPurchased()
		{
			// Arrange
			var mockUserService = new Mock<IUsersService>();
			var mockProductService = new Mock<IProductsService>();
			var mockLogService = new Mock<IPurchaseLogsService>();

			var user = new ApplicationUser() { Id = "11212", Email = "a@gmail.com", Wallet = 1200 };
			var principal = new ClaimsPrincipal(new ClaimsIdentity(new[]
			{
				new Claim(ClaimTypes.NameIdentifier, user.Id),
				new Claim(ClaimTypes.Email, user.Email)
			}));
			mockUserService.Setup(x => x.GetUserByClaims(principal)).ReturnsAsync(user);

			var logs = new List<PurchaseLog>();
			var product = new Product() { Id = 1, Name = "test", OwnerId = "other_owner", PurchaseLogs = logs, Price = 200 };
			mockProductService.Setup(x => x.GetProductById(1)).ReturnsAsync(product);

			var log = new PurchaseLog() { ProductId = product.Id, UserId = user.Id };
			mockLogService.Setup(x => x.AddPurchaseLog(log)).ReturnsAsync(1);

			var httpContext = new DefaultHttpContext() { User = principal };
			var controllerContext = new ControllerContext() { HttpContext = httpContext };
			var controller = new UserController(mockUserService.Object, mockProductService.Object, mockLogService.Object)
			{
				ControllerContext = controllerContext
			};
			// Act
			var actionResult = controller.Purchase(1);
			var result = actionResult.Result;

			// Assert
			Assert.NotNull(result);
			var redirect = Assert.IsType<RedirectToActionResult>(result);
			var actionName = redirect.ActionName;
			Assert.Equal("PaymentProceed", actionName);

			var routeValues = redirect.RouteValues;
			Assert.NotNull(routeValues);

			var paymentState = routeValues["paymentSuccess"];
			Assert.NotNull(paymentState);
			var paymentSuccess = Assert.IsType<bool>(paymentState);
			Assert.True(paymentSuccess);
		}

		[Fact]
		public void Purchase_ProductIdNotNull_UserIsFound_NotOwner_NotEnoughMoney_ShouldFailed()
		{
			// Arrange
			var mockUserService = new Mock<IUsersService>();
			var mockProductService = new Mock<IProductsService>();
			var mockLogService = new Mock<IPurchaseLogsService>();

			var user = new ApplicationUser() { Id = "11212", Email = "a@gmail.com", Wallet = 0 };
			var principal = new ClaimsPrincipal(new ClaimsIdentity(new[]
			{
				new Claim(ClaimTypes.NameIdentifier, user.Id),
				new Claim(ClaimTypes.Email, user.Email)
			}));
			mockUserService.Setup(x => x.GetUserByClaims(principal)).ReturnsAsync(user);

			var logs = new List<PurchaseLog>();
			var product = new Product() { Id = 1, Name = "test", OwnerId = "other_owner", PurchaseLogs = logs, Price = 200 };
			mockProductService.Setup(x => x.GetProductById(1)).ReturnsAsync(product);

			var log = new PurchaseLog() { ProductId = product.Id, UserId = user.Id };
			mockLogService.Setup(x => x.AddPurchaseLog(log)).ReturnsAsync(1);

			var httpContext = new DefaultHttpContext() { User = principal };
			var controllerContext = new ControllerContext() { HttpContext = httpContext };
			var controller = new UserController(mockUserService.Object, mockProductService.Object, mockLogService.Object)
			{
				ControllerContext = controllerContext
			};
			// Act
			var actionResult = controller.Purchase(1);
			var result = actionResult.Result;

			// Assert
			Assert.NotNull(result);
			var redirect = Assert.IsType<RedirectToActionResult>(result);
			var actionName = redirect.ActionName;
			Assert.Equal("PaymentProceed", actionName);

			var routeValues = redirect.RouteValues;
			Assert.NotNull(routeValues);

			var paymentState = routeValues["paymentSuccess"];
			Assert.NotNull(paymentState);
			var paymentSuccess = Assert.IsType<bool>(paymentState);
			Assert.False(paymentSuccess);
		}
	}
}
