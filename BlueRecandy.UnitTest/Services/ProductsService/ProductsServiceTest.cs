using Xunit;
using Moq;
using BlueRecandy.Services;
using BlueRecandy.Models;
using System.Collections.Generic;
using System.Linq;
using BlueRecandy.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Security.Claims;

namespace BlueRecandy.UnitTest
{
	public class ProductsServiceTest
	{
		[Fact]
		public void GetProductById_IdNotNull_ProductIsNotNull()
		{
			// Arrange
			var mockService = new Mock<IProductsService>();
			var product = new Product();
			product.Id = 1;
			product.Name = "My Product";
			mockService.Setup(m => m.GetProductById(1)).ReturnsAsync(product);
			var service = mockService.Object;

			// Act
			var actionResult = service.GetProductById(product.Id);
			var result = actionResult.Result;

			// Assert
			Assert.NotNull(result);
		}

		[Fact]
		public void GetProductById_IdIsNull_ProductIsNull()
		{
			// Arrange
			var mockService = new Mock<IProductsService>();
			var product = new Product();
			product.Id = 1;
			product.Name = "My Product";
			mockService.Setup(m => m.GetProductById(1)).ReturnsAsync(product);
			var service = mockService.Object;

			// Act
			var actionResult = service.GetProductById(null);
			var result = actionResult.Result;

			// Assert
			Assert.Null(result);
		}

		[Fact]
		public void GetProductByOwner_OwnerNotNull_HasProduct()
		{
			// Arrange
			var mockService = new Mock<IProductsService>();
			var owner = new ApplicationUser();
			owner.Id = "1";
			owner.Email = "test@gmail.com";

			var products = new List<Product>();

			var productA = new Product();
			productA.Id = 1;
			productA.Name = "My Product";
			productA.Owner = owner;
			productA.OwnerId = owner.Id;
			products.Add(productA);

			mockService.Setup(m => m.GetProductsByOwner("1")).Returns(products.AsEnumerable());
			var service = mockService.Object;

			// Act
			var result = service.GetProductsByOwner("1");

			// Assert
			Assert.True(result.Count() > 0);
		}

		[Fact]
		public void GetProductByOwner_OwnerNotNull_EmptyProduct()
		{
			// Arrange
			var mockService = new Mock<IProductsService>();
			var owner = new ApplicationUser();
			owner.Id = "1";
			owner.Email = "test@gmail.com";

			var products = new List<Product>();

			var productA = new Product();
			productA.Id = 1;
			productA.Name = "My Product";
			productA.Owner = owner;
			productA.OwnerId = owner.Id;
			products.Add(productA);

			mockService.Setup(m => m.GetProductsByOwner("1")).Returns(products.AsEnumerable());
			var service = mockService.Object;

			// Act
			var result = service.GetProductsByOwner("2");

			// Assert
			Assert.True(result.Count() == 0);
		}

		[Fact]
		public void GetProductByOwner_OwnerIsNull_EmptyProduct()
		{
			// Arrange
			var mockService = new Mock<IProductsService>();

			var products = new List<Product>();

			mockService.Setup(m => m.GetProductsByOwner(null)).Returns(products.AsEnumerable());
			var service = mockService.Object;

			// Act
			var result = service.GetProductsByOwner(null);

			// Assert
			Assert.True(result.Count() == 0);
		}

		[Fact]
		public void IsProductExists_CorrectId_IsExists()
		{
			// Arrange
			var mockService = new Mock<IProductsService>();
			var product = new Product();
			product.Id = 1;
			product.Name = "My Product";
			mockService.Setup(m => m.IsProductExists(1)).Returns(true);
			var service = mockService.Object;

			// Act
			var result = service.IsProductExists(1);

			// Assert
			Assert.True(result);
		}

		[Fact]
		public void IsProductExists_IncorrectId_NotExists()
		{
			// Arrange
			var mockService = new Mock<IProductsService>();
			var product = new Product();
			product.Id = 1;
			product.Name = "My Product";
			mockService.Setup(m => m.IsProductExists(1)).Returns(true);
			var service = mockService.Object;

			// Act
			var result = service.IsProductExists(2);

			// Assert
			Assert.False(result);
		}

		[Fact]
		public void GetProductsIncludeOwner_NoProducts_EmptyQuery()
		{
			// Arrange
			var mockService = new Mock<IProductsService>();
			var emptyEnum = Enumerable.Empty<Product>();
			mockService.Setup(m => m.GetProductsIncludeOwner()).Returns(emptyEnum.AsQueryable());
			var service = mockService.Object;

			// Act
			var actionResult = service.GetProductsIncludeOwner();

			// Assert
			Assert.Equal(Enumerable.Empty<Product>(), actionResult);
		}

		[Fact]
		public void GetProductsIncludeOwner_HasProducts_HasQuery()
		{
			// Arrange
			var mockService = new Mock<IProductsService>();
			var product = new Product();
			product.Id = It.IsAny<int>();
			product.Name = It.IsAny<string>();

			var products = new List<Product>() { product };
			
			mockService.Setup(m => m.GetProductsIncludeOwner()).Returns(products.AsQueryable());
			var service = mockService.Object;

			// Act
			var actionResult = service.GetProductsIncludeOwner();

			// Assert
			Assert.True(actionResult.Count() > 0);
		}
	}
}