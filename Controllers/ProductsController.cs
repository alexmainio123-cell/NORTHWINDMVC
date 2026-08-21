using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using NORTHWINDMVC.Models;
using PagedList;
using NORTHWINDMVC.Viewmodels;

namespace NORTHWINDMVC.Controllers
{
    public class ProductsController : Controller
    {
        private NorthwindOriginalEntities db = new NorthwindOriginalEntities();

        // GET: Products
        public ActionResult Index(string searchString, string sortOrder, int? page, int? pageSize, string productCategory)
        {
            //if (Session["UserName"] == null)
            //{
            //    ViewBag.LoggedStatus = "Out";

            //    return RedirectToAction("Login");

              
            //}
            //else
            //{
            //    var products = db.Products.Include(p => p.Categories).Include(p => p.Suppliers);
            //    ViewBag.LoggedStatus = "In";
                
            //    return View(products.ToList());
            //}
            

        

            var products = db.Products.Include(p => p.Categories).Include(p => p.Suppliers).AsQueryable();


            var model = new ProductIndexViewModel
            {

                SearchString = searchString,

                SortOrder = sortOrder,

                ProductCategory = productCategory,

                PageNumber = page ?? 1,

                PageSize = pageSize ?? 10

            };


            //Tekstihaku
            if (!string.IsNullOrEmpty(model.SearchString))

            {

                products = products.Where(p =>
                    p.ProductName.Contains(model.SearchString) ||

                    p.Suppliers.CompanyName.Contains(model.SearchString) ||

                    p.Categories.CategoryName.Contains(model.SearchString));

            }


            //Kategoriafiltteri
            if (!string.IsNullOrEmpty(model.ProductCategory) && model.ProductCategory != "0")

            {

                int catId = int.Parse(model.ProductCategory);

                products = products.Where(p => p.CategoryID == catId);

            }


            if (model.SortByNameDesc)

            {

                products = products.OrderByDescending(p => p.ProductName);

            }

            else if (model.SortByPrice)

            {

                products = products.OrderBy(p => p.UnitPrice);

            }

            else if (model.SortByPriceDesc)

            {

                products = products.OrderByDescending(p => p.UnitPrice);

            }

            else
            {

                products = products.OrderBy(p => p.ProductName);

            }


            #region//Pudotusvalikko haettavien tietojen suodatuksessa

            List<Categories> lstCategories = new List<Categories>();


            //Tuotekategorioiden haku tietokannasta
            var categoryList = from cat in db.Categories

            select cat;


            //Luetteloon viedään ensin yksi tyhjä rivi
            Categories tyhjaCategory = new Categories();

            tyhjaCategory.CategoryID = 0;

            tyhjaCategory.CategoryName = "";

            tyhjaCategory.CategoryIDCategoryName = "";

            lstCategories.Add(tyhjaCategory);


            //Tietokannasta haetut rivit käsitellään silmukassa ja arvot viedään muuttujiin.
            //Luodaan yhdistelmämuuttuja, jossa on sekä avaintieto että sen selitys samassa muuttujassa
            foreach (Categories category in categoryList)

            {

                Categories yksiCategory = new Categories();

                yksiCategory.CategoryID = category.CategoryID;

                yksiCategory.CategoryName = category.CategoryName;

                //Taulun luokkamääritykseen Models - kansiossa lisätään "uusi" kenttä = CategoryIDCategoryName
                yksiCategory.CategoryIDCategoryName = category.CategoryID.ToString() + " - " + category.CategoryName;

                lstCategories.Add(yksiCategory);

            }

            //Lopuksi luodaan uusi SelectList ja se sijoitetaan ViewBag olioon//tätä käytetään View:n puolella pudotusvalikon luettelon muodostuksessa.     ViewBag.CategoryID = new SelectList(lstCategories, "CategoryID", "CategoryIDCategoryName", productCategory);

            #endregion 
            model.Products = products.ToPagedList(model.PageNumber == 0 ? 1 : model.PageNumber, model.PageSize == 0 ? 10 : model.PageSize);

            return View(model);

        }

                
            
        

        
        
        
        
        
        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products products = db.Products.Find(id);
            if (products == null)
            {
                return HttpNotFound();
            }
            return View(products);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName");
            ViewBag.SupplierID = new SelectList(db.Suppliers, "SupplierID", "CompanyName");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductID,ProductName,SupplierID,CategoryID,QuantityPerUnit,UnitPrice,UnitsInStock,UnitsOnOrder,ReorderLevel,Discontinued")] Products products)
        {
            if (ModelState.IsValid)
            {
                db.Products.Add(products);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", products.CategoryID);
            ViewBag.SupplierID = new SelectList(db.Suppliers, "SupplierID", "CompanyName", products.SupplierID);
            return View(products);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products products = db.Products.Find(id);
            if (products == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", products.CategoryID);
            ViewBag.SupplierID = new SelectList(db.Suppliers, "SupplierID", "CompanyName", products.SupplierID);
            return View(products);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductID,ProductName,SupplierID,CategoryID,QuantityPerUnit,UnitPrice,UnitsInStock,UnitsOnOrder,ReorderLevel,Discontinued")] Products products)
        {
            if (ModelState.IsValid)
            {
                db.Entry(products).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", products.CategoryID);
            ViewBag.SupplierID = new SelectList(db.Suppliers, "SupplierID", "CompanyName", products.SupplierID);
            return View(products);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products products = db.Products.Find(id);
            if (products == null)
            {
                return HttpNotFound();
            }
            return View(products);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Products products = db.Products.Find(id);
            db.Products.Remove(products);
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
    }
}
