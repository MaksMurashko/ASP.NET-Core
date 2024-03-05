using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UniqueProducts.Data;
using UniqueProducts.Models;
using UniqueProducts.ViewModels;
using UniqueProducts.ViewModels.Clients;

namespace UniqueProducts.Controllers
{
    public class ClientsController : Controller
    {
        private readonly UniqueProductsContext _context;

        public ClientsController(UniqueProductsContext context)
        {
            _context = context;
        }

        // GET: Clients
        [Authorize(Roles = "Admin,SuperAdmin,User")]
        [AllowAnonymous]
        public IActionResult Index(int page = 1, SortState sortOrder = SortState.CompanyAsc, string company = "", string phone = "")
        {
            IQueryable<Client> clients =  _context.Clients;
            int pageSize = 20;//кол-во записей на странице

            if (company != null && company.Trim() != "")
            {
                clients = clients.Where(c => c.Company.ToLower().Contains(company.ToLower()));
            }

            if (phone != null && phone.Trim() != "")
            {
                clients = clients.Where(c => c.Phone.ToLower().Contains(phone.ToLower()));
            }

            var count = clients.Count();
            var items = clients.Skip((page - 1) * pageSize).Take(pageSize);

            switch (sortOrder)
            {
                case SortState.CompanyDesc:
                    items = items.OrderByDescending(item => item.Company);
                    break;
                case SortState.RepresentativeDesc:
                    items = items.OrderByDescending(item => item.Representative);
                    break;
                case SortState.PhoneDesc:
                    items = items.OrderByDescending(item => item.Phone);
                    break;
                case SortState.CompanyAddressDesc:
                    items = items.OrderByDescending(item => item.CompanyAddress);
                    break;
                case SortState.CompanyAsc:
                    items = items.OrderBy(item => item.Company);
                    break;
                case SortState.RepresentativeAsc:
                    items = items.OrderBy(item => item.Representative);
                    break;
                case SortState.PhoneAsc:
                    items = items.OrderBy(item => item.Phone);
                    break;
                case SortState.CompanyAddressAsc:
                    items = items.OrderBy(item => item.CompanyAddress);
                    break;
            }

            PageViewModel pageViewModel = new(count, page, pageSize);
            PaginationViewModel<Client, ClientFilterViewModel, ClientSortViewModel> viewModel = new(items, pageViewModel, new ClientFilterViewModel(company ?? "", phone ?? ""), new ClientSortViewModel(sortOrder));
            return _context.Clients != null ?
                          View(viewModel) :
                          Problem("Entity set 'UniqueProductsContext.Clients'  is null.");
        }

        // GET: Clients/Details/5
        [Authorize(Roles = "Admin,SuperAdmin,User")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Clients == null)
            {
                return NotFound();
            }

            var client = await _context.Clients
                .FirstOrDefaultAsync(m => m.ClientId == id);
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        // GET: Clients/Create
        [Authorize(Roles = "Admin,SuperAdmin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Clients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> Create([Bind("ClientId,Company,Representative,Phone,CompanyAddress")] Client client)
        {
            if (ModelState.IsValid)
            {
                _context.Add(client);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(client);
        }

        // GET: Clients/Edit/5
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Clients == null)
            {
                return NotFound();
            }

            var client = await _context.Clients.FindAsync(id);
            if (client == null)
            {
                return NotFound();
            }
            return View(client);
        }

        // POST: Clients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> Edit(int id, [Bind("ClientId,Company,Representative,Phone,CompanyAddress")] Client client)
        {
            if (id != client.ClientId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(client);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClientExists(client.ClientId))
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
            return View(client);
        }

        // GET: Clients/Delete/5
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Clients == null)
            {
                return NotFound();
            }

            var client = await _context.Clients
                .FirstOrDefaultAsync(m => m.ClientId == id);
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        // POST: Clients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Clients == null)
            {
                return Problem("Entity set 'UniqueProductsContext.Clients'  is null.");
            }
            var client = await _context.Clients.FindAsync(id);
            if (client != null)
            {
                _context.Clients.Remove(client);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClientExists(int id)
        {
          return (_context.Clients?.Any(e => e.ClientId == id)).GetValueOrDefault();
        }
    }
}
