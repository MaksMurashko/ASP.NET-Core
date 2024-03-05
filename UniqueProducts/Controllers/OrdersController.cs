using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UniqueProducts.Data;
using UniqueProducts.Models;
using UniqueProducts.ViewModels;
using UniqueProducts.ViewModels.Orders;

namespace UniqueProducts.Controllers
{
    public class OrdersController : Controller
    {
        private readonly UniqueProductsContext _context;

        public OrdersController(UniqueProductsContext context)
        {
            _context = context;
        }

        // GET: Orders
        [Authorize(Roles = "Admin,SuperAdmin,User")]
        [AllowAnonymous]
        public IActionResult Index(int page = 1, SortState sortOrder = SortState.OrderDateAsc, decimal price = 0, int month = 0)
        {
            IQueryable<Order> orders = _context.Orders.Include(o => o.Client).Include(o => o.Employee).Include(o => o.Product);
            int pageSize = 20;//кол-во записей на странице

            if (price!=0)
            {
                orders = orders.Where(o => o.TotalPrice<=price);
            }
            if (month != 0)
            {
                orders = orders.Where(o => o.OrderDate.HasValue && o.OrderDate.Value.Month == month);
            }


            var count = orders.Count();
            var items = orders.Skip((page - 1) * pageSize).Take(pageSize);

            switch (sortOrder)
            {
                case SortState.OrderNDesc:
                    items = items.OrderByDescending(item => item.OrderId);
                    break;
                case SortState.OrderDateDesc:
                    items = items.OrderByDescending(item => item.OrderDate);
                    break;
                case SortState.OrderClientDesc:
                    items = items.OrderByDescending(item => item.Client.Company);
                    break;
                case SortState.OrderProductDesc:
                    items = items.OrderByDescending(item => item.Product.ProductName);
                    break;
                case SortState.OrderAmountDesc:
                    items = items.OrderByDescending(item => item.OrderAmount);
                    break;
                case SortState.TotalPriceDesc:
                    items = items.OrderByDescending(item => item.TotalPrice);
                    break;
                case SortState.IsCompletedDesc:
                    items = items.OrderByDescending(item => item.IsCompleted);
                    break;
                case SortState.OrderEmployeeDesc:
                    items = items.OrderByDescending(item => item.Employee.EmployeeSurname);
                    break;
                case SortState.OrderNAsc:
                    items = items.OrderBy(item => item.OrderId);
                    break;
                case SortState.OrderDateAsc:
                    items = items.OrderBy(item => item.OrderDate);
                    break;
                case SortState.OrderClientAsc:
                    items = items.OrderBy(item => item.Client.Company);
                    break;
                case SortState.OrderProductAsc:
                    items = items.OrderBy(item => item.Product.ProductName);
                    break;
                case SortState.OrderAmountAsc:
                    items = items.OrderBy(item => item.OrderAmount);
                    break;
                case SortState.TotalPriceAsc:
                    items = items.OrderBy(item => item.TotalPrice);
                    break;
                case SortState.IsCompletedAsc:
                    items = items.OrderBy(item => item.IsCompleted);
                    break;
                case SortState.OrderEmployeeAsc:
                    items = items.OrderBy(item => item.Employee.EmployeeSurname);
                    break;
            }

            PageViewModel pageViewModel = new(count, page, pageSize);
            PaginationViewModel<Order, OrderFilterViewModel, OrderSortViewModel> viewModel = new(items, pageViewModel, new OrderFilterViewModel(price,month), new OrderSortViewModel(sortOrder));
            return items != null ?
                          View(viewModel) :
                          Problem("Entity set 'UniqueProductsContext.Orders'  is null.");
        }

        // GET: Orders/Details/5
        [Authorize(Roles = "Admin,SuperAdmin,User")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Client)
                .Include(o => o.Employee)
                .Include(o => o.Product)
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Orders/Create
        [Authorize(Roles = "Admin,SuperAdmin")]
        public IActionResult Create()
        {
            ViewData["ClientId"] = new SelectList(_context.Clients, "ClientId", "Company");
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "EmployeeId", "EmployeeSurname");
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductName");
            ViewData["IsCompleted"] = new SelectList(new[] { new { Value = true, Text = "Да" }, new { Value = false, Text = "Нет" } }, "Value", "Text");
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> Create([Bind("OrderId,OrderDate,ClientId,ProductId,OrderAmount,TotalPrice,IsCompleted,EmployeeId")] Order order)
        {
            if (ModelState.IsValid)
            {
                _context.Add(order);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClientId"] = new SelectList(_context.Clients, "ClientId", "Company", order.ClientId);
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "EmployeeId", "EmployeeSurname", order.EmployeeId);
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductName", order.ProductId);
            ViewData["IsCompleted"] = new SelectList(new[] { new { Value = true, Text = "Да" }, new { Value = false, Text = "Нет" } }, "Value", "Text", order.IsCompleted);
            return View(order);
        }

        // GET: Orders/Edit/5
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            ViewData["ClientId"] = new SelectList(_context.Clients, "ClientId", "Company", order.ClientId);
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "EmployeeId", "EmployeeSurname", order.EmployeeId);
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductName", order.ProductId);
            ViewData["IsCompleted"] = new SelectList(new[] { new { Value = true, Text = "Да" }, new { Value = false, Text = "Нет" } }, "Value", "Text", order.IsCompleted);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> Edit(int id, [Bind("OrderId,OrderDate,ClientId,ProductId,OrderAmount,TotalPrice,IsCompleted,EmployeeId")] Order order)
        {
            if (id != order.OrderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.OrderId))
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
            ViewData["ClientId"] = new SelectList(_context.Clients, "ClientId", "Company", order.ClientId);
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "EmployeeId", "EmployeeSurname", order.EmployeeId);
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductName", order.ProductId);
            ViewData["IsCompleted"] = new SelectList(new[] { new { Value = true, Text = "Да" }, new { Value = false, Text = "Нет" } }, "Value", "Text", order.IsCompleted);
            return View(order);
        }

        // GET: Orders/Delete/5
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Orders == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Client)
                .Include(o => o.Employee)
                .Include(o => o.Product)
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Orders == null)
            {
                return Problem("Entity set 'UniqueProductsContext.Orders'  is null.");
            }
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
          return (_context.Orders?.Any(e => e.OrderId == id)).GetValueOrDefault();
        }
    }
}
