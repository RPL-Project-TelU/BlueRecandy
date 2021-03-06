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
		private readonly IPurchaseLogsService _purchaseLogsService;

		public enum ManagementPage
        {
			Summary = 1, 
			YourProducts, 
			PurchaseLogs
        }

		public UserController(IUsersService usersService, IProductsService productsService, IPurchaseLogsService purchaseLogsService)
		{
			_usersService = usersService;
			_productsService = productsService;
			_purchaseLogsService = purchaseLogsService;
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
				bool isLogNotFound = purchaseLog == null;

				if (isLogNotFound)
				{
					bool isOwner = product.OwnerId == user.Id;
					if (isOwner)
					{
						return RedirectToAction(controllerName: "Products", actionName: "Details", routeValues: new { id = id });
					}

					bool isEnoughWallet = user.Wallet >= product.Price;
					if (isEnoughWallet)
                    {
						purchaseLog = new PurchaseLog();
						purchaseLog.UserId = user.Id;
						purchaseLog.User = user;
						purchaseLog.ProductId = id.Value;
						purchaseLog.Product = product;

						user.Wallet -= product.Price;
						await _purchaseLogsService.AddPurchaseLog(purchaseLog);
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
