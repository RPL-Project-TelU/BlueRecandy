using BlueRecandy.Data;
using BlueRecandy.Models;
using Microsoft.EntityFrameworkCore;

namespace BlueRecandy.Services
{
	public class PurchaseLogsService : IPurchaseLogsService
	{
		private readonly ApplicationDbContext _context;

		public PurchaseLogsService(ApplicationDbContext context)
		{
			_context = context;
		}

		public IEnumerable<PurchaseLog> GetPurchaseLogs()
		{
			var logs = _context.PurchaseLogs.Include(l => l.User).Include(l => l.Product);
			return logs.AsEnumerable();
		}

		public IEnumerable<PurchaseLog> GetPurchaseLogsByProductId(int productId)
		{
			var logs = _context.PurchaseLogs.Include(l => l.User).Include(l => l.Product).Where(x => x.ProductId == productId);
			return logs.AsEnumerable();
		}

		public IEnumerable<PurchaseLog> GetPurchaseLogsByUserId(string userId)
		{
			var logs = _context.PurchaseLogs.Include(l => l.User).Include(l => l.Product).Where(x => x.UserId == userId);
			return logs.AsEnumerable();
		}
	}
}
