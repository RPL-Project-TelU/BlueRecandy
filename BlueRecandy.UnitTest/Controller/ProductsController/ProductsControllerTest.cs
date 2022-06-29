using BlueRecandy.Controllers;
using BlueRecandy.Models;
using BlueRecandy.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Linq;
using System.Collections.Generic;
using Xunit;
using BlueRecandy.Data;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using System.Threading;
using System.Security.Principal;
using System.IO;

namespace BlueRecandy.UnitTest.Controller.ProductController
{
	public class ProductsControllerTest
	{
		[Fact]
		public void Index_ShouldViewPage()
		{
			// Arrange
			var owner = new ApplicationUser();
			owner.Id = "asdasd";
			
			var products =new List<Product>()
			{
				new Product { Id = 1, Name = "Tes", Owner = owner, OwnerId = owner.Id }
			}.AsQueryable();

			var mockProductService = new Mock<IProductsService>();
			mockProductService.Setup(x => x.GetProductsIncludeOwner()).Returns(products);
			var productService = mockProductService.Object;

			var controller = new ProductsController(productService, null);
			// Act
			var result = controller.Index();

			// Assert
			Assert.NotNull(result);

			var view = Assert.IsType<ViewResult>(result);
			Assert.Equal("Products", view.ViewData["Title"]);

			var model = view.Model as IEnumerable<Product>;
			Assert.Single(model);
		}

		[Fact]
		public void Details_HasId_HasProduct_ShouldOpenView()
		{
			// Arrange
			var mockProductService = new Mock<IProductsService>();
			var mockProdouct = new Product() { Id = 1, Name = "Test" };
			mockProductService.Setup(m => m.GetProductById(1)).ReturnsAsync(mockProdouct);
			var productService = mockProductService.Object;
			var controller = new ProductsController(productService, null);

			// Act
			var actionResult = controller.Details(1);
			var result = actionResult.Result;

			// Assert
			Assert.NotNull(result);
			var view = Assert.IsType<ViewResult>(result);
			Assert.NotNull(view.Model);
		}

		[Fact]
		public void Details_HasId_NoProduct_ShouldNotFound()
		{
			// Arrange
			var mockProductService = new Mock<IProductsService>();

			mockProductService.Setup(m => m.GetProductById(It.IsAny<int>())).ReturnsAsync(value: null);

			var controller = new ProductsController(mockProductService.Object, null);

			// Act
			var actionResult = controller.Details(It.IsAny<int>());
			var result = actionResult.Result as NotFoundResult;

			// Assert
			Assert.NotNull(result);
			Assert.IsType<NotFoundResult>(result);
		}

		[Fact]
		public void Details_NoId_ShouldNotFound()
		{
			// Arrange
			var mockProductService = new Mock<IProductsService>();

			mockProductService.Setup(m => m.GetProductById(null)).ReturnsAsync(value: null);

			var controller = new ProductsController(mockProductService.Object, null);

			// Act
			var actionResult = controller.Details(null);
			var result = actionResult.Result as NotFoundResult;

			// Assert
			Assert.NotNull(result);
			Assert.IsType<NotFoundResult>(result);
		}

		[Fact]
		public void Create_GetPage_ShowCreatePage()
		{
			var controller = new ProductsController(null, null);

			var result = controller.Create();

			Assert.NotNull(result);
			Assert.IsType<ViewResult>(result);
		}

		[Fact]
		public void Create_UploadProduct_CreateProduct()
		{
			// Arrange
			var mockUserService = new Mock<IUsersService>();
			var mockProductService = new Mock<IProductsService>();

			var user = new ApplicationUser()
			{
				Id = "1",
				FullName = "Aaaa",
				Email = "a@gmail.com",
				UserName = "Aaaaa"
			};
			mockUserService.Setup(x => x.GetUserByClaims(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);

			var product = new Product()
			{
				Id = 1,
				Name = "Test",
				Description = "Tes",
				Price = 0,
				UseExternalURL = true,
				DownloadURL = "yes.com"
			};

			var controller = new ProductsController(mockProductService.Object, mockUserService.Object);
			// Act
			var actionResult = controller.Create(null, product);
			var result = actionResult.Result;

			// Assert
			Assert.NotNull(result);
			var redirect = Assert.IsType<RedirectToActionResult>(result);
			Assert.Equal("Index", redirect.ActionName);
		}


		[Fact]
		public void Edit_ProductIdIsNull_ShouldNotFound()
		{
			// Arrange
			var mockUserService = new Mock<IUsersService>();
			var mockProductService = new Mock<IProductsService>();

			var controller = new ProductsController(mockProductService.Object, mockUserService.Object);
			// Act
			var actionResult = controller.Edit(null);
			var result = actionResult.Result;

			// Assert
			Assert.NotNull(result);
			Assert.IsType<NotFoundResult>(result);
		}

		[Fact]
		public void Edit_ProductIdNotNull_HasProduct_NotOwner_BackToIndex()
		{
			// Arrange
			var mockUserService = new Mock<IUsersService>();
			var mockProductService = new Mock<IProductsService>();

			var product = new Product() { Id = 1, Name = "aa", OwnerId = "owner" };
			mockProductService.Setup(x => x.GetProductById(1)).ReturnsAsync(product);

			var context = new ControllerContext()
			{
				HttpContext = new DefaultHttpContext()
				{
					User = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.NameIdentifier, "not_owner") }))
				}
			};

			var controller = new ProductsController(mockProductService.Object, mockUserService.Object);
			controller.ControllerContext = context;
			// Act
			var actionResult = controller.Edit(1);
			var result = actionResult.Result;

			// Assert
			Assert.NotNull(result);
			var redirect = Assert.IsType<RedirectToActionResult>(result);
			Assert.Equal("Index", redirect.ActionName);
		}

		[Fact]
		public void Edit_ProductIdNotNull_HasProduct_IsOwner_ShouldView()
		{
			// Arrange
			var mockUserService = new Mock<IUsersService>();
			var mockProductService = new Mock<IProductsService>();

			var product = new Product() { Id = 1, Name = "aa", OwnerId = "owner_id" };
			mockProductService.Setup(x => x.GetProductById(1)).ReturnsAsync(product);

			var context = new ControllerContext()
			{
				HttpContext = new DefaultHttpContext()
				{
					User = new ClaimsPrincipal(new ClaimsIdentity(new[] {
						new Claim(ClaimTypes.NameIdentifier, "owner_id"),
						new Claim(ClaimTypes.Email, "tes@gmail.com")
					}))
				}
			};

			var controller = new ProductsController(mockProductService.Object, mockUserService.Object);
			controller.ControllerContext = context;
			// Act
			var actionResult = controller.Edit(1);
			var result = actionResult.Result;

			// Assert
			Assert.NotNull(controller.User);
			Assert.NotNull(result);
			Assert.IsType<ViewResult>(result);
		}

		[Fact]
		public void Delete_HasId_ShouldViewPage()
		{
			// Arrange
			var mockProductService = new Mock<IProductsService>(MockBehavior.Strict);
			var product = new Product()
			{
				Id = 1,
				Name = "aaa",
				OwnerId = "owner"
			};

			mockProductService.Setup(x => x.GetProductById(1)).ReturnsAsync(product);

			var controller = new ProductsController(mockProductService.Object, null);
			// Act
			var actionResult = controller.Delete(1);
			var result = actionResult.Result;

			// Assert
			Assert.NotNull(result);
			Assert.IsType<ViewResult>(result);
		}

		[Fact]
		public void Delete_IdIsNull_ShouldNotFound()
		{
			// Arrange
			var mockProductService = new Mock<IProductsService>(MockBehavior.Strict);

			mockProductService.Setup(x => x.GetProductById(null)).ReturnsAsync(value: null);

			var controller = new ProductsController(mockProductService.Object, null);
			// Act
			var actionResult = controller.Delete(null);
			var result = actionResult.Result;

			// Assert
			Assert.NotNull(result);
			Assert.IsType<NotFoundResult>(result);
		}

		[Fact]
		public void DeleteConfirmed_HasId_ShouldRedirect()
		{
			// Arrange
			var mockProductService = new Mock<IProductsService>(MockBehavior.Strict);
			var product = new Product()
			{
				Id = 1,
				Name = "aaa",
				OwnerId = "owner"
			};

			mockProductService.Setup(x => x.GetProductById(1)).ReturnsAsync(product);
			mockProductService.Setup(x => x.DeleteProduct(product)).ReturnsAsync(1);

			var controller = new ProductsController(mockProductService.Object, null);
			// Act
			var actionResult = controller.DeleteConfirmed(1);
			var result = actionResult.Result;

			// Assert
			Assert.NotNull(result);
			Assert.IsType<RedirectToActionResult>(result);
		}

		[Fact]
		public void ProductExists_CorrectId_ShouldExists()
		{
			// Arrange
			var mockProductService = new Mock<IProductsService>(MockBehavior.Strict);
			mockProductService.Setup(x => x.IsProductExists(1)).Returns(true);

			var controller = new ProductsController(mockProductService.Object, null);
			// Act
			var result = controller.ProductExists(1);

			// Assert
			Assert.True(result);
		}

		[Fact]
		public void ProductExists_IncorrectId_ShouldNotExists()
		{
			// Arrange
			var mockProductService = new Mock<IProductsService>(MockBehavior.Strict);
			mockProductService.Setup(x => x.IsProductExists(1)).Returns(false);

			var controller = new ProductsController(mockProductService.Object, null);
			// Act
			var result = controller.ProductExists(1);

			// Assert
			Assert.False(result);
		}

		[Fact]
		public void Download_HasSourceFile_ShouldDownload()
		{
			// Arrange
			var mockProductService = new Mock<IProductsService>(MockBehavior.Strict);
			var mockFile = new Mock<IFormFile>();
			mockFile.Setup(x => x.FileName).Returns("Test");
			mockFile.Setup(x => x.ContentType).Returns("text/plain");
			mockFile.Setup(x => x.Length).Returns(0);

			var file = mockFile.Object;

			var product = new Product()
			{
				Id = 1,
				Name = "Test",
				OwnerId = "aaa1",
				UseExternalURL = false,
				SourceFileName = file.FileName,
				SourceFileContentType = file.ContentType,
				SourceFileContents = new byte[0]
			};

			mockProductService.Setup(x => x.GetProductById(1)).ReturnsAsync(product);

			var controller = new ProductsController(mockProductService.Object, null);
			// Act
			var actionResult = controller.Download(1);
			var result = actionResult.Result;

			// Assert
			Assert.NotNull(result);
			Assert.IsType<FileContentResult>(result);
		}

		[Fact]
		public void Download_NoSourceFile_ShouldNotFound()
		{
			// Arrange
			var mockProductService = new Mock<IProductsService>(MockBehavior.Strict);
			var product = new Product()
			{
				Id = 1,
				Name = "Test",
				OwnerId = "aaa1",
				UseExternalURL = false
			};

			mockProductService.Setup(x => x.GetProductById(1)).ReturnsAsync(product);

			var controller = new ProductsController(mockProductService.Object, null);
			// Act
			var actionResult = controller.Download(1);
			var result = actionResult.Result;

			// Assert
			Assert.NotNull(result);
			Assert.IsType<NotFoundResult>(result);
		}

		[Fact]
		public void ShowSearchForm()
		{
			// Arrange
			var controller = new ProductsController(null, null);
            // Act
            var result = controller.ShowSearchForm();
			// Assert
			Assert.NotNull(result);
			var view = Assert.IsType<ViewResult>(result);
        }

        [Fact]
		public void ShowSearchResults()
		{
			// Arrange 
			var product = new List<Product>().AsQueryable();
			var mockProductService = new Mock<IProductsService>(MockBehavior.Strict);
			mockProductService.Setup(x => x.GetProductsIncludeOwner()).Returns(product);
			var productService = mockProductService.Object;

			var controller = new ProductsController(productService, null);
			// Act
			var result = controller.ShowSearchResults("BirdyMail");
			var result2 = result.Result;
			// Assert
			Assert.NotNull(result2);
			var view = Assert.IsType<ViewResult>(result2);
			Assert.NotNull(view);
			var Model = view.Model as IEnumerable<Product>;	
			Assert.Empty(Model);
		}
	}
}
