﻿#nullable disable
using BlueRecandy.Data;
using BlueRecandy.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BlueRecandy.Controllers
{
	[Authorize]
	public class ProductsController : Controller
	{
		private readonly ApplicationDbContext _context;
		private readonly UserManager<ApplicationUser> _userManager;

		public ProductsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
		{
			_context = context;
			_userManager = userManager;
		}

		// GET: Products
		[AllowAnonymous]
		public async Task<IActionResult> Index()
		{
			var applicationDbContext = _context.Products.Include(p => p.Owner);
			return View(await applicationDbContext.ToListAsync());
		}

		// GET: Products/ShowSearchForm
		[AllowAnonymous]
		public async Task<IActionResult> ShowSearchForm()
		{
			var applicationDbContext = _context.Products.Include(p => p.Owner);
			return View();
		}

		// PoST: Products/ShowSearchResults
		[AllowAnonymous]
		public async Task<IActionResult> ShowSearchResults(String SearchPhrase)
		{
			var applicationDbContext = _context.Products.Include(p => p.Owner);
			ViewBag.SearchStatus = true;
			if (SearchPhrase == null)
			{
				ViewBag.SearchStatus = false;
			}
			return View("Index", await _context.Products.Where(j => j.Description.Contains
			(SearchPhrase)).ToListAsync());
		}

		// GET: Products/Details/5
		[AllowAnonymous]
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var product = await _context.Products
				.Include(p => p.Owner)
				.Include(p => p.PurchaseLogs)
				.FirstOrDefaultAsync(m => m.Id == id);
			if (product == null)
			{
				return NotFound();
			}

			return View(product);
		}


		public async Task<IActionResult> PaymentProceed(int? id, bool paymentSuccess)
		{
			if (id == null)
			{
				return NotFound();
			}

			var product = await _context.Products
				.Include(p => p.Owner)
				.Include(p => p.PurchaseLogs)
				.FirstOrDefaultAsync(m => m.Id == id);
			ViewBag.PaymentSuccess = paymentSuccess;

			if (product == null)
			{
				return NotFound();
			}

			return View("Details", product);
		}

		// GET: Products/Create
		public IActionResult Create()
		{
 			ViewData["OwnerId"] = new SelectList(_context.Users, "Id", "Id");
			return View();
		}

		// POST: Products/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("Id,Name,Description,DownloadURL,Price")] Product product)
		{
			product.OwnerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			_context.Add(product);
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		// GET: Products/Edit/5
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var product = await _context.Products.FindAsync(id);
			if (product == null)
			{
				return NotFound();
			}
			ViewData["OwnerId"] = new SelectList(_context.Users, "Id", "Id", product.OwnerId);
			return View(product);
		}

		// POST: Products/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,DownloadURL,OwnerId")] Product product)
		{
			if (id != product.Id)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(product);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!ProductExists(product.Id))
					{
						return NotFound();
					}
					else
					{
						throw;
					}
				}
				return RedirectToAction(nameof(Index));
			}
			ViewData["OwnerId"] = new SelectList(_context.Users, "Id", "Id", product.OwnerId);
			return View(product);
		}

		// GET: Products/Delete/5
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var product = await _context.Products
				.Include(p => p.Owner)
				.FirstOrDefaultAsync(m => m.Id == id);
			if (product == null)
			{
				return NotFound();
			}

			return View(product);
		}

		// POST: Products/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var product = await _context.Products.FindAsync(id);
			_context.Products.Remove(product);
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		private bool ProductExists(int id)
		{
			return _context.Products.Any(e => e.Id == id);
		}

	}
}
