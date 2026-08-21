using NORTHWINDMVC.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using PagedList;


namespace NORTHWINDMVC.Viewmodels
{
    public class ProductIndexViewModel
    {
        public IPagedList<Products> Products { get; set; }


        // Haku & suodatus
        public string SearchString { get; set; }

        public string ProductCategory { get; set; }


        // Lajittelu
        public string SortOrder { get; set; }


        // Lajittelulinkkejä varten
        public string ProductNameSort =>
        SortOrder == "ProductName" ? "ProductName_desc" : "ProductName";

        public string UnitPriceSort =>
        SortOrder == "UnitPrice" ? "UnitPrice_desc" : "UnitPrice";


        // Tulkitsee lajittelun (controller käyttää näitä)
        public bool SortByNameDesc => SortOrder == "ProductName_desc";

        public bool SortByPrice => SortOrder == "UnitPrice";

        public bool SortByPriceDesc => SortOrder == "UnitPrice_desc";


        // Sivutus
        public int PageNumber { get; set; }

        public int PageSize { get; set; }


        public int ProductID { get; set; }

        public string ProductName { get; set; }

        public Nullable<int> SupplierID { get; set; }

        public Nullable<int> CategoryID { get; set; }

        public string QuantityPerUnit { get; set; }

        public Nullable<decimal> UnitPrice { get; set; }

        public Nullable<short> UnitsInStock { get; set; }

        public Nullable<short> UnitsOnOrder { get; set; }

        public Nullable<short> ReorderLevel { get; set; }

        public bool Discontinued { get; set; }

        public string RPAProcessed { get; set; }

        public string ImageLink { get; set; }

        public byte[] Photo { get; set; }


        public virtual Categories Categories { get; set; }


        public virtual ICollection<Order_Details> Order_Details { get; set; }

        public virtual Suppliers Suppliers { get; set; }
    }
}