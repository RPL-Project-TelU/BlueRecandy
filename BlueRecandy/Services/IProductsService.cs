using BlueRecandy.Models;

namespace BlueRecandy.Services
{
	public interface IProductsService
	{

		bool IsProductExists(int id);

		Task<Product?> GetProductById(int? id);

		IQueryable<Product?> GetProductsIncludeOwner();

		IEnumerable<Product> GetProductsByOwner(string? ownerId);

		bool ValidateProduct(Product product);

		void AddProduct(Product product);

		void UpdateProduct(Product product);

		void DeleteProduct(Product product);
	}
}
