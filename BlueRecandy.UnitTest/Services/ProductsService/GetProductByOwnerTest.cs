using BlueRecandy.Models;
using BlueRecandy.Services;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace BlueRecandy.UnitTest.Services.ProductsService
{
	public class GetProductByOwnerTest
	{

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

	}
}
