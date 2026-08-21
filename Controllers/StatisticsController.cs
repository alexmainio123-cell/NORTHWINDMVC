using Newtonsoft.Json;
using NORTHWINDMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NORTHWINDMVC.Viewmodels;

namespace NORTHWINDMVC.Controllers
{
    public class StatisticsController : Controller
    {
        private NorthwindOriginalEntities db = new NorthwindOriginalEntities();
        // GET: Statistics
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult CategorySales()
        {
            var categorySalesData = db.Category_Sales_for_1997
                                      .Select(cs => new CategorysalesVM
                                      {
                                          CategoryName = cs.CategoryName,
                                          CategorySales = (decimal)cs.CategorySales
                                      })
                                      .ToList();

            // Muodostetaan suoraan JSON-objektit, joita voidaan käyttää suoraan ViewBag:issä
            ViewBag.categoryName = JsonConvert.SerializeObject(categorySalesData.Select(n => n.CategoryName).ToList());
            ViewBag.categorySales = JsonConvert.SerializeObject(categorySalesData.Select(n => n.CategorySales).ToList());

            return View();
        }
    }

    internal class CategorysalesVM
    {
        public string CategoryName { get; set; }
        public decimal CategorySales { get; set; }
    }
}