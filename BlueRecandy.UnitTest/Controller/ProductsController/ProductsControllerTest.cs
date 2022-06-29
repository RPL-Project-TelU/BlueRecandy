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

namespace BlueRecandy.UnitTest.Controller.ProductController
{
	public class ProductsControllerTest
	{
		[Fact]
		public void Index()
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
		public void Create()
		{

		}

		[Fact]
		public void Delete()
		{

		}

		[Fact]
		public void Edit()
		{

		}

		[Fact]
		public void ShowSearchResults()
		{

		}

	}
}
