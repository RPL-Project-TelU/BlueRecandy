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
        private readonly IProductsService _productsService;
        private readonly IUsersService _usersService;

        public ProductsController(IProductsService service, IUsersService usersService)
        {
            _productsService = service;
            _usersService = usersService;
        }

        // GET: Products
        [AllowAnonymous]
        public IActionResult Index()
        {
            var products = _productsService.GetProductsIncludeOwner();
            var view = View(products);
            view.ViewData["Title"] = "Products";
            return view;
        }

        // GET: Products/ShowSearchForm
        [AllowAnonymous]
        public IActionResult ShowSearchForm()
        {
            return View();
        }


        // PoST: Products/ShowSearchResults
        [AllowAnonymous]
        public IActionResult ShowSearchResults(string SearchPhrase)
        {
            var products = _productsService.GetProductsIncludeOwner();
            ViewBag.SearchStatus = true;
            if (SearchPhrase == null)
            {
                ViewBag.SearchStatus = false;
            }
            var searchResult = products.Where(j => j.Name.Contains(SearchPhrase) || j.Description.Contains(SearchPhrase))
                .ToList();
            return View("Index", searchResult);
        }

        // GET: Products/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _productsService.GetProductById(id);
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

            var product = await _productsService.GetProductById(id);
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

            var owner = await _usersService.GetUserByClaims(User);
            product.OwnerId = owner.Id;

            if (_productsService.ValidateProduct(product)) {
                await _productsService.AddProduct(product);
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _productsService.GetProductById(id);
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
                if (_productsService.ValidateProduct(product))
                {
                    await _productsService.UpdateProduct(product);
				}
            }
            catch (DbUpdateConcurrencyException)
            {
                bool isProductExists = _productsService.IsProductExists(product.Id);
                if (!isProductExists)
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

            var product = await _productsService.GetProductById(id);
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
            var product = await _productsService.GetProductById(id);
            await _productsService.DeleteProduct(product);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Download(int? id)
        {

            var product = await _productsService.GetProductById(id);

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
