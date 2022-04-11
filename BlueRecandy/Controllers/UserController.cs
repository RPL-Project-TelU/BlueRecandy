using BlueRecandy.Data;
using BlueRecandy.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BlueRecandy.Controllers
{
	[Authorize]
	public class UserController : Controller
	{

		private readonly ApplicationDbContext _context;
		private readonly UserManager<ApplicationUser> _userManager;

		public UserController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
		{
			_context = context;
			_userManager = userManager;
		}

		public IActionResult Index()
		{
			return View();
		}

		public async Task<IActionResult> Purchase(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var product = await _context.Products.FindAsync(id);
			var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

			PurchaseLog? purchaseLog;

			if (product == null || userId == null)
			{
				return NotFound();
			}else
			{
				purchaseLog = await _context.PurchaseLogs.FindAsync(userId, product.Id);
				if (purchaseLog == null)
				{

					purchaseLog = new PurchaseLog();
					purchaseLog.UserId = userId;
					purchaseLog.User = _userManager.Users.First(x => x.Id == userId);
					purchaseLog.ProductId = id.Value;
					purchaseLog.Product = product;

					_context.Add(purchaseLog);
					await _context.SaveChangesAsync();
					return RedirectToAction(controllerName: "Products", actionName: "Index");
				}
				else
				{
					return RedirectToAction(controllerName: "Products", actionName: "Details", routeValues: new {id = id});
				}

			}
		}
	}
}
