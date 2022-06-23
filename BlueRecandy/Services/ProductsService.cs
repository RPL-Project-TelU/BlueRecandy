using BlueRecandy.Data;
using BlueRecandy.Models;
using Microsoft.EntityFrameworkCore;

namespace BlueRecandy.Services
{
	public class ProductsService : IProductsService
	{

		private readonly ApplicationDbContext _context;
	
		public ProductsService(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<Product> GetProductById(int? id)
		{
			var product = await _context.Products
				.Include(p => p.Owner)
				.Include(p => p.PurchaseLogs)
				.Include(p => p.ProductFeedbacks)
				.FirstAsync(m => m.Id == id);

			return product;
		}

		public IEnumerable<Product> GetProductsByOwner(string ownerId)
		{
			var products = _context.Products
				.Include(p => p.Owner)
				.Include(p => p.PurchaseLogs)
				.Include(p => p.ProductFeedbacks)
				.AsEnumerable();

			return products.Where(p => p.OwnerId == ownerId);
		}

		public IQueryable<Product?> GetProductsIncludeOwner()
        {
			var queryProducts = _context.Products.Include(p => p.Owner);

			return queryProducts;
		}
    }
}
