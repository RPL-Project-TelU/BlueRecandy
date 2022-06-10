#nullable disable
using BlueRecandy.Data;
using BlueRecandy.Models;
using BlueRecandy.Services;
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
        private readonly IProductsService _service;

        public ProductsController(ApplicationDbContext context, IProductsService service, UserManager<ApplicationUser> userManager)
        {
            _service = service;
            _context = context;
            _userManager = userManager;
        }

        // GET: Products
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _service.GetProductsIncludeOwner();
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Products/ShowSearchForm
        [AllowAnonymous]
        public async Task<IActionResult> ShowSearchForm()
        {
            var applicationDbContext = _service.GetProductsIncludeOwner();
            return View();
        }

        // PoST: Products/ShowSearchResults
        [AllowAnonymous]
        public async Task<IActionResult> ShowSearchResults(String SearchPhrase)
        {
            var applicationDbContext = _service.GetProductsIncludeOwner();
            ViewBag.SearchStatus = true;
            if (SearchPhrase == null)
            {
                ViewBag.SearchStatus = false;
            }
            return View("Index", await _context.Products.Where(j => j.Name.Contains(SearchPhrase) || j.Description.Contains
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

            var product = await _service.GetProductById(id);
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

            var product = await _service.GetProductById(id);
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
        public async Task<IActionResult> Create(IFormFile sourceFile, [Bind("Id,Name,Description,UseExternalURL,DownloadURL,Price")] Product product)
        {
            if (sourceFile != null)
            {
                using (Stream s = sourceFile.OpenReadStream())
                {
                    using (BinaryReader br = new BinaryReader(s))
                    {
                        byte[] contents = br.ReadBytes((int)sourceFile.Length);

                        product.SourceFileName = sourceFile.FileName;
                        product.SourceFileContentType = sourceFile.ContentType;
                        product.SourceFileContents = contents;

                    }
                }
            }

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

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (product.OwnerId != userId)
            {
                return RedirectToAction("Index");
            }

            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, IFormFile sourceFile, [Bind("Id,Name,Description,DownloadURL,Price,UseExternalURL")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            product.OwnerId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (sourceFile != null)
            {
                using (Stream s = sourceFile.OpenReadStream())
                {
                    using (BinaryReader br = new BinaryReader(s))
                    {
                        byte[] contents = br.ReadBytes((int)sourceFile.Length);

                        product.SourceFileName = sourceFile.FileName;
                        product.SourceFileContentType = sourceFile.ContentType;
                        product.SourceFileContents = contents;

                    }
                }
            }

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

        public async Task<IActionResult> Download(int? id)
        {

            var product = await _context.Products.FirstAsync(x => x.Id == id);

            if (product.SourceFileContents != null)
            {
                byte[] data = product.SourceFileContents;
                string fileName = product.SourceFileName;
                string contentType = product.SourceFileContentType;

                return File(data, contentType, fileName);
            }


            return NotFound();
        }

    }
}
