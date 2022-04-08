using BlueRecandy.Data;
using BlueRecandy.Models;
using Microsoft.AspNetCore.Mvc;

namespace BlueRecandy.Controllers
{
	public class AccountController : Controller
	{

		private readonly ApplicationDbContext _context;
		public AccountController(ApplicationDbContext context)
		{
			_context = context;
		}

		public async Task<IActionResult> Purchase(int? id)
		{
			var product = await _context.Products.FindAsync(id);
			

			if (product == null)
			{
				return NotFound();
			}
			else
			{
				PurchaseLog purchaseLog = new PurchaseLog();
			}


			return View();
		}
	}
}
