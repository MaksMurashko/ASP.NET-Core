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
using UniqueProducts.ViewModels.Employees;

namespace UniqueProducts.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly UniqueProductsContext _context;

        public EmployeesController(UniqueProductsContext context)
        {
            _context = context;
        }

        // GET: Employees
        [Authorize(Roles = "Admin,SuperAdmin,User")]
        [AllowAnonymous]
        public IActionResult Index(int page = 1, SortState sortOrder = SortState.EmployeeSurnameAsc, string surname = "", string position = "")
        {
            IQueryable<Employee> employees = _context.Employees;
            int pageSize = 20;//кол-во записей на странице

            if (surname != null && surname.Trim() != "")
            {
                employees = employees.Where(e => e.EmployeeSurname.ToLower().Contains(surname.ToLower()));
            }

            if (position != null && position.Trim() != "")
            {
                employees = employees.Where(e => e.EmployeePosition.ToLower().Contains(position.ToLower()));
            }

            var count = employees.Count();
            var items = employees.Skip((page - 1) * pageSize).Take(pageSize);

            switch (sortOrder)
            {
                case SortState.EmployeeNameDesc:
                    items = items.OrderByDescending(e => e.EmployeeName);
                    break;
                case SortState.EmployeeMidNameDesc:
                    items = items.OrderByDescending(e => e.EmployeeMidname);
                    break;
                case SortState.EmployeeSurnameDesc:
                    items = items.OrderByDescending(e => e.EmployeeSurname);
                    break;
                case SortState.EmployeePositionDesc:
                    items = items.OrderByDescending(e => e.EmployeePosition);
                    break;
                case SortState.EmployeeNameAsc:
                    items = items.OrderBy(e => e.EmployeeName);
                    break;
                case SortState.EmployeeMidNameAsc:
                    items = items.OrderBy(e => e.EmployeeMidname);
                    break;
                case SortState.EmployeeSurnameAsc:
                    items = items.OrderBy(e => e.EmployeeSurname);
                    break;
                case SortState.EmployeePositionAsc:
                    items = items.OrderBy(e => e.EmployeePosition);
                    break;
            }
            PageViewModel pageViewModel = new(count, page, pageSize);
            PaginationViewModel<Employee, EmployeeFilterViewModel, EmployeeSortViewModel> viewModel = new(items, pageViewModel, new EmployeeFilterViewModel(position ?? "", surname ?? ""), new EmployeeSortViewModel(sortOrder));

            return _context.Employees != null ? 
                          View(viewModel) :
                          Problem("Entity set 'UniqueProductsContext.Employees'  is null.");
        }

        // GET: Employees/Details/5
        [Authorize(Roles = "Admin,SuperAdmin,User")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Employees == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .FirstOrDefaultAsync(m => m.EmployeeId == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // GET: Employees/Create
        [Authorize(Roles = "Admin,SuperAdmin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> Create([Bind("EmployeeId,EmployeeName,EmployeeSurname,EmployeeMidname,EmployeePosition")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                _context.Add(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        // GET: Employees/Edit/5
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Employees == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> Edit(int id, [Bind("EmployeeId,EmployeeName,EmployeeSurname,EmployeeMidname,EmployeePosition")] Employee employee)
        {
            if (id != employee.EmployeeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.EmployeeId))
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
            return View(employee);
        }

        // GET: Employees/Delete/5
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Employees == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .FirstOrDefaultAsync(m => m.EmployeeId == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Employees == null)
            {
                return Problem("Entity set 'UniqueProductsContext.Employees'  is null.");
            }
            var employee = await _context.Employees.FindAsync(id);
            if (employee != null)
            {
                _context.Employees.Remove(employee);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeExists(int id)
        {
          return (_context.Employees?.Any(e => e.EmployeeId == id)).GetValueOrDefault();
        }
    }
}
