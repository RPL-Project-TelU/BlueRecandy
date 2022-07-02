using BlueRecandy.Data;
using BlueRecandy.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace BlueRecandy.Services
{
	[ExcludeFromCodeCoverage]
	public class ProductsService : IProductsService
	{

		private readonly ApplicationDbContext _context;
	
		public ProductsService(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<int> AddProduct(Product product)
		{
			_context.Products.Add(product);
			return await _context.SaveChangesAsync();
		}

		public bool ValidateProduct(Product product)
		{
			bool externalUrlCheck;
			if (product.UseExternalURL)
			{
				externalUrlCheck = product.DownloadURL != null;
			}
			else
			{
				externalUrlCheck = product.SourceFileName != null;
			}


			bool detailsCheck = product.Name != null && product.OwnerId != null;
			bool priceCheck = product.Price >= 0;
			return detailsCheck && priceCheck && externalUrlCheck;
		}

		public async Task<Product?> GetProductById(int? id)
		{
			if (id == null) return null;

			var product = await _context.Products
				.Include(p => p.Owner)
				.Include(p => p.PurchaseLogs)
				.Include(p => p.ProductFeedbacks)
				.FirstAsync(m => m.Id == id);

			return product;
		}

		public IEnumerable<Product> GetProductsByOwner(string? ownerId)
		{
			if (ownerId == null) return Enumerable.Empty<Product>();

			var products = _context.Products
				.Include(p => p.Owner)
				.Include(p => p.PurchaseLogs)
				.Include(p => p.ProductFeedbacks)
				.AsEnumerable();

			return products.Where(p => p.OwnerId == ownerId);
		}

		public IQueryable<Product> GetProductsIncludeOwner()
        {
			var queryProducts = _context.Products.Include(p => p.Owner);

			return queryProducts;
		}

		public bool IsProductExists(int id)
		{
			return _context.Products.Any(p => p.Id == id);
		}

		public async Task<int> UpdateProduct(Product product)
		{
			_context.Update(product);
			return await _context.SaveChangesAsync();
		}

		public async Task<int> DeleteProduct(Product product)
		{
			_context.Remove(product);
			return await _context.SaveChangesAsync();
		}
	}
}
