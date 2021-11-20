using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Doan_ASP.NET_MVC.Models;

namespace Doan_ASP.NET_MVC.Controllers
{
    public class AdminController : Controller
    {
        private ShopModelContext db = new ShopModelContext();

        // GET: Products
        [Authorize(Roles = "admin")]
        public ActionResult Index()
        {
            var products = db.Products.Include(p => p.Brand).Include(p => p.Category).Include(p => p.Origin).Include(p => p.sale);
            return View(products.ToList());
        }

        public ActionResult GetProductById(int id)
        {
            var img = (from a in db.Products

                       where a.category_id == id
                       select a).ToList();
            return View(img);
        }
        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            ViewBag.brand_id = new SelectList(db.Brands, "brand_id", "brand_name");
            ViewBag.category_id = new SelectList(db.Categories, "category_id", "category_name");
            ViewBag.origin_id = new SelectList(db.Origins, "origin_id", "origin_name");
            //ViewBag.sale_id = new SelectList(db.sales, "sale_id", "sale_name");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(Product product,HttpPostedFileBase file)
        {
            //lưu tên file
            var fileName = Path.GetFileName(file.FileName);
            //lưu dường dẫn
            var path = Path.Combine(Server.MapPath("~/Content/images/product/small-size"), fileName);
            file.SaveAs(path);
            
                //luu vao db
                product.product_image = "Content/images/product/small-size/" + fileName;
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            

            ViewBag.brand_id = new SelectList(db.Brands, "brand_id", "brand_name", product.brand_id);
            ViewBag.category_id = new SelectList(db.Categories, "category_id", "category_name", product.category_id);
            ViewBag.origin_id = new SelectList(db.Origins, "origin_id", "origin_name", product.origin_id);
            //ViewBag.sale_id = new SelectList(db.sales, "sale_id", "sale_id", product.sale_id);
            return View(product);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.brand_id = new SelectList(db.Brands, "brand_id", "brand_name", product.brand_id);
            ViewBag.category_id = new SelectList(db.Categories, "category_id", "category_name", product.category_id);
            ViewBag.origin_id = new SelectList(db.Origins, "origin_id", "origin_name", product.origin_id);
            ViewBag.sale_id = new SelectList(db.sales, "sale_id", "sale_name");
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "product_id,category_id,product_name,product_image,product_price,product_description,product_quantity,brand_id,product_size,origin_id,hot_product,sale_id")] Product product)
        {
            if (ModelState.IsValid)
            {
                
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.brand_id = new SelectList(db.Brands, "brand_id", "brand_name", product.brand_id);
            ViewBag.category_id = new SelectList(db.Categories, "category_id", "category_name", product.category_id);
            ViewBag.origin_id = new SelectList(db.Origins, "origin_id", "origin_name", product.origin_id);
            ViewBag.sale_id = new SelectList(db.sales, "sale_id", "sale_id", product.sale_id);
            return View(product);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            
            db.Products.Remove(product);
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
