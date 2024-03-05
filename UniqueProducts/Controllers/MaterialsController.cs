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
using UniqueProducts.ViewModels.Materials;

namespace UniqueProducts.Controllers
{
    public class MaterialsController : Controller
    {
        private readonly UniqueProductsContext _context;

        public MaterialsController(UniqueProductsContext context)
        {
            _context = context;
        }

        // GET: Materials
        [Authorize(Roles = "Admin,SuperAdmin,User")]
        [AllowAnonymous]
        public IActionResult Index(int page = 1, SortState sortOrder = SortState.MaterialNameAsc, string material = "", int code = 0)
        {
            IQueryable<Material> materials = _context.Materials;
            int pageSize = 20;//кол-во записей на странице

            if (material != null && material.Trim() != "")
            {
                materials = materials.Where(m => m.MaterialName.ToLower().Contains(material.ToLower()));
            }

            if (code != 0)
            {
                materials = materials.Where(m => m.MaterialId.ToString().StartsWith(code.ToString()));
            }

            var count = materials.Count();
            var items = materials.Skip((page - 1) * pageSize).Take(pageSize);

            switch (sortOrder)
            {
                case SortState.MaterialCodeDesc:
                    items = items.OrderByDescending(item => item.MaterialId);
                    break;
                case SortState.MaterialNameDesc:
                    items = items.OrderByDescending(item => item.MaterialName);
                    break;
                case SortState.MaterialDescriptDesc:
                    items = items.OrderByDescending(item => item.MaterialDescript);
                    break;
                case SortState.MaterialCodeAsc:
                    items = items.OrderBy(item => item.MaterialId);
                    break;
                case SortState.MaterialNameAsc:
                    items = items.OrderBy(item => item.MaterialName);
                    break;
                case SortState.MaterialDescriptAsc:
                    items = items.OrderBy(item => item.MaterialDescript);
                    break;
            }

            PageViewModel pageViewModel = new(count, page, pageSize);
            PaginationViewModel<Material, MaterialFilterViewModel, MaterialSortViewModel> viewModel = new(items, pageViewModel, new MaterialFilterViewModel(material ?? "", code), new MaterialSortViewModel(sortOrder));

            return _context.Materials != null ? 
                          View(viewModel) :
                          Problem("Entity set 'UniqueProductsContext.Materials'  is null.");
        }

        // GET: Materials/Details/5
        [Authorize(Roles = "Admin,SuperAdmin,User")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Materials == null)
            {
                return NotFound();
            }

            var material = await _context.Materials
                .FirstOrDefaultAsync(m => m.MaterialId == id);
            if (material == null)
            {
                return NotFound();
            }

            return View(material);
        }

        // GET: Materials/Create
        [Authorize(Roles = "Admin,SuperAdmin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Materials/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> Create([Bind("MaterialId,MaterialName,MaterialDescript")] Material material)
        {
            if (ModelState.IsValid)
            {
                _context.Add(material);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(material);
        }

        // GET: Materials/Edit/5
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Materials == null)
            {
                return NotFound();
            }

            var material = await _context.Materials.FindAsync(id);
            if (material == null)
            {
                return NotFound();
            }
            return View(material);
        }

        // POST: Materials/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> Edit(int id, [Bind("MaterialId,MaterialName,MaterialDescript")] Material material)
        {
            if (id != material.MaterialId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(material);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MaterialExists(material.MaterialId))
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
            return View(material);
        }

        // GET: Materials/Delete/5
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Materials == null)
            {
                return NotFound();
            }

            var material = await _context.Materials
                .FirstOrDefaultAsync(m => m.MaterialId == id);
            if (material == null)
            {
                return NotFound();
            }

            return View(material);
        }

        // POST: Materials/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Materials == null)
            {
                return Problem("Entity set 'UniqueProductsContext.Materials'  is null.");
            }
            var material = await _context.Materials.FindAsync(id);
            if (material != null)
            {
                _context.Materials.Remove(material);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MaterialExists(int id)
        {
          return (_context.Materials?.Any(e => e.MaterialId == id)).GetValueOrDefault();
        }
    }
}
