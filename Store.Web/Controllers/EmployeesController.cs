using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Store.Web.Data;
using Store.Web.Data.Entities;
using Store.Web.Helpers;

namespace Store.Web.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly DataContext _context;

        private readonly IImageHelper _imageHelper;

        public EmployeesController(DataContext context, IImageHelper imageHelper)
        {
            _context = context;
            _imageHelper = imageHelper;
        }

        // GET: Employees
        public IActionResult Index()
        {
            return View();
        }

        // GET: All the Employees
        public async Task<JsonResult> Datos()
        {

            var employees = await _context.Employees.Select(e => new Employees
            {
                EmployeeId = e.EmployeeId,
                LastName = e.LastName,
                FirstName = e.FirstName,
                Title = e.Title,
                TitleOfCourtesy = e.TitleOfCourtesy,
                BirthDate = e.BirthDate,
                HireDate = e.HireDate,
                Address = e.Address,
                City = e.City,
                Region = e.Region,
                PostalCode = e.PostalCode,
                Country = e.Country,
                HomePhone = e.HomePhone,
                Extension = e.Extension,
                Photo = e.Photo,
                Notes = e.Notes,
                PhotoPath = e.PhotoPath,
                ReportsTo = e.ReportsTo
            }).ToListAsync();
            return Json(new { data = employees });
        }

        //@model IEnumerable<App_Business.Models.Employees>

        //var northwindContext = _context.Employees.Include(e => e.ReportsToNavigation);

        //return View(await northwindContext.Select(e => new Employees
        //{
        //    EmployeeId = e.EmployeeId,
        //    LastName = e.LastName,
        //    FirstName = e.FirstName,
        //    Title= e.Title,
        //    TitleOfCourtesy =e.TitleOfCourtesy,
        //    BirthDate = e.BirthDate,
        //    HireDate = e.HireDate,
        //    Address = e.Address,
        //    City = e.City,
        //    Region = e.Region,
        //    PostalCode = e.PostalCode,
        //    Country = e.Country,
        //    HomePhone = e.HomePhone,
        //    Extension = e.Extension,
        //    Photo = e.Photo,
        //    Notes = e.Notes,
        //    PhotoPath = e.PhotoPath,
        //    ReportsTo = e.ReportsTo
        //}).ToListAsync());

        //return View(await northwindContext.ToListAsync());


        // GET: Employees/Details/5

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employees = await _context.Employees
                .Include(e => e.ReportsToNavigation)
                .FirstOrDefaultAsync(m => m.EmployeeId == id);
            if (employees == null)
            {
                return NotFound();
            }

            return View(employees);
        }

        // GET: Employees/Create
        public IActionResult Create()
        {
            ViewData["ReportsTo"] = new SelectList(_context.Employees, "EmployeeId", "FirstName");
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Employees employees, IFormFile Image)
        {
            var resp = false;
            if (ModelState.IsValid)
            {
                try
                {
                    if (Image != null && Image.Length > 0)
                    {
                        //var _iImageHelper = new ImageHelper();
                        employees.Photo = _imageHelper.UploadImageDB(Image);
                        employees.PhotoPath = await _imageHelper.UploadImageDirectoryAsync(Image, "employees");
                    }
                    _context.Add(employees);
                    await _context.SaveChangesAsync();
                    resp = true;
                    return Json(resp);
                }
                catch (Exception)
                {
                    resp = false;
                }
            }
            return Json(resp);
        }

        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employees = await _context.Employees.FindAsync(id);
            if (employees == null)
            {
                return NotFound();
            }
            ViewData["ReportsTo"] = new SelectList(_context.Employees, "EmployeeId", "FirstName", employees.ReportsTo);
            return View(employees);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Employees employees, IFormFile Image)
        {
            if (id != employees.EmployeeId)
            {
                return NotFound();
            }

            //FormCollection
            if (ModelState.IsValid)
            {
                try
                {

                    ////else
                    ////{
                    ////    employees.Photo = _context.Employees.Where(p => p.EmployeeId == id).Select(x => x.Photo).FirstOrDefault();
                    ////}
                    if (Image != null && Image.Length > 0)
                    {
                        var _iImageHelper = new ImageHelper();
                        employees.Photo = _iImageHelper.UploadImageDB(Image);
                        employees.PhotoPath = await _iImageHelper.UploadImageDirectoryAsync(Image, "employees");
                    }
                    _context.Update(employees);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeesExists(employees.EmployeeId))
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
            ViewData["ReportsTo"] = new SelectList(_context.Employees, "EmployeeId", "FirstName", employees.ReportsTo);
            return View(employees);
        }


        //public async Task<IActionResult> Edit(int id, Employees employees)
        //{
        //    if (id != employees.EmployeeId)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(employees);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!EmployeesExists(employees.EmployeeId))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["ReportsTo"] = new SelectList(_context.Employees, "EmployeeId", "FirstName", employees.ReportsTo);
        //    return View(employees);
        //}

        // GET: Employees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employees = await _context.Employees
                .Include(e => e.ReportsToNavigation)
                .FirstOrDefaultAsync(m => m.EmployeeId == id);
            if (employees == null)
            {
                return NotFound();
            }

            return View(employees);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employees = await _context.Employees.FindAsync(id);

            _context.Employees.Remove(employees);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeesExists(int id)
        {
            return _context.Employees.Any(e => e.EmployeeId == id);
        }
    }
}
