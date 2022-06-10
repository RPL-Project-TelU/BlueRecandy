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

		public async Task<Product?> GetProductById(int? id)
		{
			var product = await _context.Products
				.Include(p => p.Owner)
				.Include(p => p.PurchaseLogs)
				.Include(p => p.ProductFeedbacks)
				.FirstOrDefaultAsync(m => m.Id == id);

			return product;
		}
	}
}
