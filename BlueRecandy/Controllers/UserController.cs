using BlueRecandy.Data;
using BlueRecandy.Models;
using BlueRecandy.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlueRecandy.Controllers
{
	[Authorize]
	public class UserController : Controller
	{
		private readonly IUsersService _usersService;
		private readonly IProductsService _productsService;
		private readonly ApplicationDbContext _context;
		private readonly IPurchaseLogsService _purchaseLogsService;

		public enum ManagementPage
        {
			Summary = 1, 
			YourProducts, 
			PurchaseLogs
        }

		public UserController(IUsersService usersService, IProductsService productsService, IPurchaseLogsService purchaseLogsService, ApplicationDbContext context)
		{
			_usersService = usersService;
			_productsService = productsService;
			_purchaseLogsService = purchaseLogsService;
			_context = context;
		}

		public async Task<IActionResult> Index()
		{
			var user = await _usersService.GetUserByClaims(User);
			var products = _productsService.GetProductsByOwner(user.Id);
			var logs = _purchaseLogsService.GetPurchaseLogsByUserId(user.Id);

			ViewBag.User = user;
			ViewBag.PurchaseLogs = logs;
			ViewBag.Products = products;

			ViewBag.ManagementPage = ManagementPage.Summary;
			return View(products);
		}

		public async Task<IActionResult> ManageYourProducts()
        {
			var user = await _usersService.GetUserByClaims(User);
			var products = _productsService.GetProductsByOwner(user.Id);

			ViewBag.User = user;
			ViewBag.Products = products;
			ViewBag.ManagementPage = ManagementPage.YourProducts;

			return View(viewName: "Index", user);
        }

		public async Task<IActionResult> ManagePurchaseLogs()
        {
			var user = await _usersService.GetUserByClaims(User);
			var logs = _purchaseLogsService.GetPurchaseLogsByUserId(user.Id);

			ViewBag.User = user;
			ViewBag.PurchaseLogs = logs;
			ViewBag.ManagementPage = ManagementPage.PurchaseLogs;

			return View(viewName: "Index", user);
		}

		public async Task<IActionResult> Purchase(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var product = await _productsService.GetProductById(id);
			var user = await _usersService.GetUserByClaims(User);

			PurchaseLog? purchaseLog;

			if (product == null || user == null)
			{
				return NotFound();
			}else
			{
				purchaseLog = product.PurchaseLogs.Find(pl => pl.UserId == user.Id);
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
