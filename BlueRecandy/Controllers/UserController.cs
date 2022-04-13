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
			var user = await _userManager.GetUserAsync(User);

			PurchaseLog? purchaseLog;

			if (product == null || user == null)
			{
				return NotFound();
			}else
			{
				purchaseLog = user.PurchaseLogs == null ? null : user.PurchaseLogs.Find(x => x.UserId == user.Id && x.ProductId == id.Value);
				if (purchaseLog == null)
				{

					if (user.Wallet >= product.Price)
                    {
						purchaseLog = new PurchaseLog();
						purchaseLog.UserId = user.Id;
						purchaseLog.User = user;
						purchaseLog.ProductId = id.Value;
						purchaseLog.Product = product;

						_context.Add(purchaseLog);

						user.Wallet -= product.Price;
						await _context.SaveChangesAsync();
						return RedirectToAction(controllerName: "Products", actionName: "PaymentProceed", routeValues: new { id = id, paymentSuccess = true });
					}
                    else
                    {
						// Error: Not enough balance
						return RedirectToAction(controllerName: "Products", actionName: "PaymentProceed", routeValues: new { id = id, paymentSuccess = false });
					}
				}
				else
				{
					return RedirectToAction(controllerName: "Products", actionName: "Details", routeValues: new {id = id});
				}

			}
		}
	}
}
