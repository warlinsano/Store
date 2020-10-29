using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Store.Web.Data;
using Store.Web.Data.Entities;
using Store.Web.Models;

namespace Store.Web.Controllers
{
    public class OrdersController : Controller
    {
        private readonly DataContext _context;

        public OrdersController(DataContext context)
        {
            _context = context;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            var northwindContext = _context.Orders.Include(o => o.Customer).Include(o => o.Employee).Include(o => o.ShipViaNavigation);
            return View(await northwindContext.ToListAsync());
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orders = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Employee)
                .Include(o => o.ShipViaNavigation)
                .Include(o => o.OrderDetails).ThenInclude(o => o.Product)
                .FirstOrDefaultAsync(m => m.OrderId == id);

            if (orders == null)
            {
                return NotFound();
            }
            //return new ViewAsPdf("Details", orders)
            //{ // ...
            //    // Establece el número de página.
            //    CustomSwitches = "--page-offset 0 --footer-center [page] --footer-font-size 12"
            //};
            return View(orders);
        }

        // GET: Orders/Create
        public IActionResult Create()
        {
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerNameDoc");
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "EmployeeId", "FirstName");
            ViewData["ShipVia"] = new SelectList(_context.Shippers, "ShipperId", "CompanyName");
            return View(new OrdersViewModels());
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create(Orders orders)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(orders);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerId", orders.CustomerId);
        //    ViewData["EmployeeId"] = new SelectList(_context.Employees, "EmployeeId", "FirstName", orders.EmployeeId);
        //    ViewData["ShipVia"] = new SelectList(_context.Shippers, "ShipperId", "CompanyName", orders.ShipVia);
        //    return View(orders);
        //}

        // GET: Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orders = await _context.Orders.FindAsync(id);
            if (orders == null)
            {
                return NotFound();
            }
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerId", orders.CustomerId);
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "EmployeeId", "FirstName", orders.EmployeeId);
            ViewData["ShipVia"] = new SelectList(_context.Shippers, "ShipperId", "CompanyName", orders.ShipVia);
            return View(orders);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Orders orders)
        {
            if (id != orders.OrderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(orders);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrdersExists(orders.OrderId))
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
            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerId", orders.CustomerId);
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "EmployeeId", "FirstName", orders.EmployeeId);
            ViewData["ShipVia"] = new SelectList(_context.Shippers, "ShipperId", "CompanyName", orders.ShipVia);
            return View(orders);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orders = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Employee)
                .Include(o => o.ShipViaNavigation)
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (orders == null)
            {
                return NotFound();
            }

            return View(orders);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var orders = await _context.Orders.FindAsync(id);
            _context.Orders.Remove(orders);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrdersExists(int id)
        {
            return _context.Orders.Any(e => e.OrderId == id);
        }

        //----------------------------------------------
        public IActionResult Prueva()
        {
            return View();
        }

        public JsonResult BuscarProducto(string nombre)
        {
            var productos = _context.Products.OrderBy(x => x.ProductName)
                               .Where(x => x.ProductName.Contains(nombre))
                               .Take(10)
                               .ToList();
            return Json(productos);
        }

        public JsonResult BuscarCustomer(string nombre)
        {
            var Customers = _context.Customers.OrderBy(x => x.ContactName)
                               .Where(x => x.ContactName.Contains(nombre))
                               .Take(10)
                               .ToList();
            return Json(Customers);
        }

        public Orders ToModel(OrdersViewModels model)
        {
            var order = new Orders();
            order.EmployeeId = 1;
            order.CustomerId = model.Clienteid;
            order.OrderDate = DateTime.Now;
            order.RequiredDate = DateTime.Now;
            order.ShippedDate = DateTime.Now;
            //order.ShipVia = 2;
            //order.Freight
            //order.ShipName
            //order.ShipAddress
            //order.ShipCity
            //order.ShipRegion
            //order.ShipPostalCode
            //order.ShipCountry
            //order.Customer = _context.Customers.Find(model.Clienteid);
            //order.Employee
            //order.ShipViaNavigation

            //comprobante.Total = this.Total();
            foreach (var d in model.ComprobanteDetalle)
            {
                order.OrderDetails.Add(new OrderDetails
                {
                    ProductId = d.ProductoId,
                    UnitPrice = d.PrecioUnitario,
                    Quantity = d.Cantidad,
                    // Discount
                    //Monto = d.Monto(),
                });
            }
            return order;
        }

        public bool Save(Orders model)
        {
            using (var tran = _context.Database.BeginTransaction())
            {
                try
                {
                    //if (ModelState.IsValid)
                    //{ 

                    _context.Add(model);
                    _context.SaveChanges();
                    //var num = 0;
                    //var error = 2 / num;
                    //}
                }
                catch (Exception)
                {
                    tran.Rollback();
                    return false;
                }
                tran.Commit();
                return true;
            }
        }

        public bool SeAgregoUnProductoValido(OrdersViewModels model)
        {
            return !(model.CabeceraProductoId == 0 ||
                string.IsNullOrEmpty(model.CabeceraProductoNombre) ||
                model.CabeceraProductoCantidad == 0 || model.CabeceraProductoPrecio == 0);
        }

        public bool ExisteEnDetalle(int ProductoId, OrdersViewModels model)
        {
            return model.ComprobanteDetalle.Any(x => x.ProductoId == ProductoId);
        }

        public void AgregarItemADetalle(OrdersViewModels model)
        {
            model.ComprobanteDetalle.Add(new OrderDetailViewModels
            {
                ProductoId = model.CabeceraProductoId,
                ProductoNombre = model.CabeceraProductoNombre,
                PrecioUnitario = model.CabeceraProductoPrecio,
                Cantidad = model.CabeceraProductoCantidad,
            });

            Refrescar(model);
        }

        public void Refrescar(OrdersViewModels model)
        {
            model.CabeceraProductoId = 0;
            model.CabeceraProductoNombre = null;
            model.CabeceraProductoCantidad = 1;
            model.CabeceraProductoPrecio = 0;
        }

        public void RetirarItemDeDetalle(OrdersViewModels model)
        {
            if (model.ComprobanteDetalle.Count > 0)
            {
                var detalleARetirar = model.ComprobanteDetalle.Where(x => x.Retirar)
                                                        .SingleOrDefault();
                model.ComprobanteDetalle.Remove(detalleARetirar);
            }
        }

        [HttpPost]
        public ActionResult Create(OrdersViewModels model, string action)
        {
            if (action == "generar")
            {
                if (!string.IsNullOrEmpty(model.Cliente) && !string.IsNullOrEmpty(model.Clienteid))
                {
                    if (Save(ToModel(model)))
                    {
                        return Redirect("~/");
                    }
                }
                else
                {
                    ModelState.AddModelError("cliente", "Debe agregar un cliente para el comprobante");
                }
            }
            else if (action == "agregar_producto")
            {
                // Si no ha pasado nuestra validación, mostramos el mensaje personalizado de error
                if (!SeAgregoUnProductoValido(model))
                {
                    ModelState.AddModelError("CabeceraProductoNombre", "Solo puede agregar un producto válido al detalle");
                }
                else if (ExisteEnDetalle(model.CabeceraProductoId, model))
                {
                    ModelState.AddModelError("CabeceraProductoNombre", "El producto elegido ya existe en el detalle");
                }
                else
                {
                    AgregarItemADetalle(model);
                }
            }
            else if (action == "retirar_producto")
            {
                RetirarItemDeDetalle(model);
            }
            else
            {
                throw new Exception("Acción no definida ..");
            }

            ViewData["CustomerId"] = new SelectList(_context.Customers, "CustomerId", "CustomerNameDoc");
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "EmployeeId", "FirstName");
            ViewData["ShipVia"] = new SelectList(_context.Shippers, "ShipperId", "CompanyName");
            return View(model);
        }
        //------------------------
    }
}
