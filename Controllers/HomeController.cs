using Doan_ASP.NET_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Doan_ASP.NET_MVC.Controllers
{
    public class HomeController : Controller
    {
        private ShopModelContext db = new ShopModelContext();

        // GET: Categories
        public ActionResult Index()
        {
            
            return View(db.Categories.ToList());
        }

        public ActionResult Getsale(int id)
        {
            var list = (from s in db.sales
                           join p in db.Products on s.sale_id equals p.sale_id
                           where p.category_id == id
                           orderby s.sale_name
                           select s).Distinct();
            foreach (sale s in list)
            {

                s.category1 = id;


            }
            return PartialView("partial_sale", list);
        }
        public ActionResult Getbrand(int id)
        {
            var brand = (from b in db.Brands
                          join p in db.Products on b.brand_id equals p.brand_id
                          where p.category_id == id
                          select b).Distinct();
            foreach (Brand b in brand)
            {

                b.category1 = id;


            }

            return PartialView("Getbrand",brand);
        }
        public ActionResult Getorigin(int id)
        {
            var origin = (from o in db.Origins
                          join p in db.Products on o.origin_id equals p.origin_id
                          where p.category_id == id
                          select o).Distinct();

            foreach (Origin o in origin)
            {

                o.category1 = id;


            }
            return PartialView("category_origin", origin);
        }

       
        public ActionResult Hotproduct()
        {
            IQueryable<Product> list = (from p in db.Products
                        where p.hot_product == true
                        orderby p.product_id
                        select p).Take(6);
            

            return PartialView("Hotproduct", list);
        }
        public ActionResult Quickview(int? id)
        {
            if (id == null)
            {
                var product = from p in db.Products
                              where p.product_id == 1
                              select p;
                return PartialView("Quickview",product);
            }
            else {
                var product = from p in db.Products
                       where p.product_id == id
                       select p;

                return PartialView("Quickview",product);
            }
        }


        public ActionResult HotproductLaptops()
        {
            var list = (from p in db.Products
                       where p.category_id == 1 orderby p.product_id
                       select p).Take(6);
            

            return PartialView("HotproductLaptops", list);
        }

        public ActionResult HotproductSmartphone()
        {
            var list = (from p in db.Products
                       where p.category_id == 2 orderby p.product_id
                       select p).Take(6);

           

            return PartialView("HotproductSmartphone", list);
        }
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}