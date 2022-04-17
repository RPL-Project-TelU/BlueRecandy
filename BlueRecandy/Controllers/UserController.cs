using BlueRecandy.Data;
using BlueRecandy.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BlueRecandy.Controllers
{
	[Authorize]
	public class UserController : Controller
	{

		private readonly ApplicationDbContext _context;
		private readonly UserManager<ApplicationUser> _userManager;

		public enum ManagementPage
        {
			Summary = 1, 
			YourProducts, 
			PurchaseLogs
        }

		public UserController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
		{
			_context = context;
			_userManager = userManager;
		}

		public async Task<IActionResult> Index()
		{
			var user = await _userManager.GetUserAsync(User);
			var products = _context.Products.AsEnumerable();
			var logs = _context.PurchaseLogs.Include(l => l.User).AsEnumerable();

			var ownedProducts = products.Where(x => x.OwnerId == user.Id);
			var purchaseLogs = logs.Where(x => x.User.Id == user.Id);

			ViewBag.User = user;
			ViewBag.PurchaseLogs = purchaseLogs;
			ViewBag.Products = ownedProducts;

			ViewBag.ManagementPage = ManagementPage.Summary;
			return View(ownedProducts);
		}

		public async Task<IActionResult> ManageYourProducts()
        {
			var user = await _userManager.GetUserAsync(User);
			var products = _context.Products.AsEnumerable();

			ViewBag.User = user;
			ViewBag.Products = products.Where(x => x.OwnerId == user.Id);
			ViewBag.ManagementPage = ManagementPage.YourProducts;
			return View(viewName: "Index", user);
        }

		public async Task<IActionResult> ManagePurchaseLogs()
        {
			var user = await _userManager.GetUserAsync(User);
			var logs = _context.PurchaseLogs.Include(l => l.Product).Include(l => l.User).AsEnumerable();

			ViewBag.User = user;
			ViewBag.PurchaseLogs = logs.Where(x => x.UserId == user.Id);

			ViewBag.ManagementPage = ManagementPage.PurchaseLogs;
			return View(viewName: "Index", user);
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
					if (product.OwnerId == user.Id)
					{
						return RedirectToAction(controllerName: "Products", actionName: "Details", routeValues: new { id = id });
					}

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
