using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using NORTHWINDMVC.Models;
using NORTHWINDMVC.Viewmodels;

namespace NORTHWINDMVC.Controllers
{
    public class OrdersController : Controller
    {
        private NorthwindOriginalEntities db = new NorthwindOriginalEntities();

        // GET: Orders
        public ActionResult Index()
        {
            var orders = db.Orders.Include(o => o.Customers).Include(o => o.Employees).Include(o => o.Shippers);
            return View(orders.ToList());
        }

        // GET: Orders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Orders orders = db.Orders.Find(id);
            if (orders == null)
            {
                return HttpNotFound();
            }
            return View(orders);
        }

        // GET: Orders/Create
        public ActionResult Create()
        {
            ViewBag.CustomerID = new SelectList(db.Customers, "CustomerID", "CompanyName");
            ViewBag.EmployeeID = new SelectList(db.Employees, "EmployeeID", "LastName");
            ViewBag.ShipVia = new SelectList(db.Shippers, "ShipperID", "CompanyName");
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OrderID,CustomerID,EmployeeID,OrderDate,RequiredDate,ShippedDate,ShipVia,Freight,ShipName,ShipAddress,ShipCity,ShipRegion,ShipPostalCode,ShipCountry")] Orders orders)
        {
            if (ModelState.IsValid)
            {
                db.Orders.Add(orders);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CustomerID = new SelectList(db.Customers, "CustomerID", "CompanyName", orders.CustomerID);
            ViewBag.EmployeeID = new SelectList(db.Employees, "EmployeeID", "LastName", orders.EmployeeID);
            ViewBag.ShipVia = new SelectList(db.Shippers, "ShipperID", "CompanyName", orders.ShipVia);
            return View(orders);
        }

        // GET: Orders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Orders orders = db.Orders.Find(id);
            if (orders == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerID = new SelectList(db.Customers, "CustomerID", "CompanyName", orders.CustomerID);
            ViewBag.EmployeeID = new SelectList(db.Employees, "EmployeeID", "LastName", orders.EmployeeID);
            ViewBag.ShipVia = new SelectList(db.Shippers, "ShipperID", "CompanyName", orders.ShipVia);
            return View(orders);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OrderID,CustomerID,EmployeeID,OrderDate,RequiredDate,ShippedDate,ShipVia,Freight,ShipName,ShipAddress,ShipCity,ShipRegion,ShipPostalCode,ShipCountry")] Orders orders)
        {
            if (ModelState.IsValid)
            {
                db.Entry(orders).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CustomerID = new SelectList(db.Customers, "CustomerID", "CompanyName", orders.CustomerID);
            ViewBag.EmployeeID = new SelectList(db.Employees, "EmployeeID", "LastName", orders.EmployeeID);
            ViewBag.ShipVia = new SelectList(db.Shippers, "ShipperID", "CompanyName", orders.ShipVia);
            return View(orders);
        }

        // GET: Orders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Orders orders = db.Orders.Find(id);
            if (orders == null)
            {
                return HttpNotFound();
            }
            return View(orders);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Orders orders = db.Orders.Find(id);
            db.Orders.Remove(orders);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult TilausOtsikotJS(string searchString1)
        {
            var orders = db.Orders.Include(o => o.Customers).Include(o => o.Employees).Include(o => o.Shippers);
            orders = orders.OrderByDescending(o => o.OrderDate);
            #region//2. kotitehtävä
            if (!String.IsNullOrEmpty(searchString1))
            {
                orders = orders.Where(o =>
                o.Customers.CompanyName.Contains(searchString1) ||
                o.ShipCity.Contains(searchString1) ||
                o.Shippers.CompanyName.Contains(searchString1) ||
                o.ShipCountry.Contains(searchString1));
            }
            #endregion
            return View(orders.ToList());
        }

        #region 3.pv _TilausRivit, muodostetaan Partial View

        public ActionResult _TilausRivit(int? orderid)

        {

            if ((orderid == null) || (orderid == 0))

            {

                return HttpNotFound();

            }

            else

            {

                var orderRowsList = from od in db.Order_Details

                                    join p in db.Products on od.ProductID equals p.ProductID

                                    join c in db.Categories on p.CategoryID equals c.CategoryID

                                    where od.OrderID == orderid

                                    //orderby-lause

                                    select new OrderRows

                                    {

                                        OrderID = od.OrderID,

                                        ProductID = p.ProductID,

                                        UnitPrice = (decimal)p.UnitPrice,

                                        Quantity = (int)od.Quantity,

                                        Discount = (float?)od.Discount,

                                        ProductName = p.ProductName,

                                        SupplierID = (int)p.SupplierID,

                                        CategoryID = (int)c.CategoryID,

                                        QuantityPerUnit = p.QuantityPerUnit,

                                        UnitsInStock = (int)p.UnitsInStock,

                                        UnitsOnOrder = (int)p.UnitsOnOrder,

                                        ReorderLevel = (int)p.ReorderLevel,

                                        Discontinued = p.Discontinued,

                                        CategoryName = c.CategoryName,

                                        Description = c.Description,

                                        //ImageLink = p.ImageLink

                                        //Picture = (Image)c.Picture

                                    };

                return PartialView(orderRowsList);

            }

        }

        #endregion




    }
}
