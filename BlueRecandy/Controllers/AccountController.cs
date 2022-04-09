using BlueRecandy.Data;
using BlueRecandy.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BlueRecandy.Controllers
{
	[Authorize]
	public class AccountController : Controller
	{

		private readonly ApplicationDbContext _context;
		private readonly UserManager<ApplicationUser> _userManager;

		public AccountController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
		{
			_context = context;
			_userManager = userManager;
		}

		public async Task<IActionResult> Purchase(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var product = await _context.Products.FindAsync(id);
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			if (product == null || userId == null)
			{
				return NotFound();
			}
			else
			{
				PurchaseLog purchaseLog = new PurchaseLog();
				purchaseLog.UserId = userId;
			}


			return View();
		}
	}
}
