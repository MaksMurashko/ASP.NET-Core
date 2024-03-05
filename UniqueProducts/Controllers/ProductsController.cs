using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UniqueProducts.Data;
using UniqueProducts.Models;
using UniqueProducts.ViewModels;
using UniqueProducts.ViewModels.Products;

namespace UniqueProducts.Controllers
{
    public class ProductsController : Controller
    {
        private readonly UniqueProductsContext _context;

        public ProductsController(UniqueProductsContext context)
        {
            _context = context;
        }

        // GET: Products
        [Authorize(Roles = "Admin,SuperAdmin,User")]
        [AllowAnonymous]
        public IActionResult Index(int page = 1, SortState sortOrder = SortState.ProductNameAsc, string name = "")
        {
            IQueryable<Product> products= _context.Products.Include(p => p.Material);

            int pageSize = 20;//кол-во записей на странице

            if (name != null && name.Trim() != "")
            {
                products = products.Where(p => p.ProductName.ToLower().Contains(name.ToLower()));
            }

            var count = products.Count();
            var items = products.Skip((page - 1) * pageSize).Take(pageSize);

            switch (sortOrder)
            {
                case SortState.ProductCodeDesc:
                    items = items.OrderByDescending(item => item.ProductId);
                    break;
                case SortState.ProductNameDesc:
                    items = items.OrderByDescending(item => item.ProductName);
                    break;
                case SortState.ProductDescriptDesc:
                    items = items.OrderByDescending(item => item.ProductDescript);
                    break;
                case SortState.ProductWeightDesc:
                    items = items.OrderByDescending(item => item.ProductWeight);
                    break;
                case SortState.ProductDiameterDesc:
                    items = items.OrderByDescending(item => item.ProductDiameter);
                    break;
                case SortState.ProductColorDesc:
                    items = items.OrderByDescending(item => item.ProductColor);
                    break;
                case SortState.ProductMaterialDesc:
                    items = items.OrderByDescending(item => item.Material.MaterialName);
                    break;
                case SortState.ProductPriceDesc:
                    items = items.OrderByDescending(item => item.ProductPrice);
                    break;
                case SortState.ProductCodeAsc:
                    items = items.OrderBy(item => item.ProductId);
                    break;
                case SortState.ProductNameAsc:
                    items = items.OrderBy(item => item.ProductName);
                    break;
                case SortState.ProductDescriptAsc:
                    items = items.OrderBy(item => item.ProductDescript);
                    break;
                case SortState.ProductWeightAsc:
                    items = items.OrderBy(item => item.ProductWeight);
                    break;
                case SortState.ProductDiameterAsc:
                    items = items.OrderBy(item => item.ProductDiameter);
                    break;
                case SortState.ProductColorAsc:
                    items = items.OrderBy(item => item.ProductColor);
                    break;
                case SortState.ProductMaterialAsc:
                    items = items.OrderBy(item => item.Material.MaterialName);
                    break;
                case SortState.ProductPriceAsc:
                    items = items.OrderBy(item => item.ProductPrice);
                    break;
            }

            PageViewModel pageViewModel = new(count, page, pageSize);
            PaginationViewModel<Product, ProductFilterViewModel, ProductSortViewModel> viewModel = new(items, pageViewModel, new ProductFilterViewModel(name ?? ""), new ProductSortViewModel(sortOrder));
            return items != null ?
                          View(viewModel) :
                          Problem("Entity set 'UniqueProductsContext.Products'  is null.");

        }

        // GET: Products/Details/5
        [Authorize(Roles = "Admin,SuperAdmin,User")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Material)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        [Authorize(Roles = "Admin,SuperAdmin")]
        public IActionResult Create()
        {
            ViewData["MaterialId"] = new SelectList(_context.Materials, "MaterialId", "MaterialName");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> Create([Bind("ProductId,ProductName,ProductDescript,ProductWeight,ProductDiameter,ProductColor,MaterialId,ProductPrice")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["MaterialId"] = new SelectList(_context.Materials, "MaterialId", "MaterialName", product.MaterialId);
            return View(product);
        }

        // GET: Products/Edit/5
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["MaterialId"] = new SelectList(_context.Materials, "MaterialId", "MaterialName", product.MaterialId);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,ProductName,ProductDescript,ProductWeight,ProductDiameter,ProductColor,MaterialId,ProductPrice")] Product product)
        {
            if (id != product.ProductId)
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
                    if (!ProductExists(product.ProductId))
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
            ViewData["MaterialId"] = new SelectList(_context.Materials, "MaterialId", "MaterialName", product.MaterialId);
            return View(product);
        }

        // GET: Products/Delete/5
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Material)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Products == null)
            {
                return Problem("Entity set 'UniqueProductsContext.Products'  is null.");
            }
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
          return (_context.Products?.Any(e => e.ProductId == id)).GetValueOrDefault();
        }
    }
}
