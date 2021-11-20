using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Doan_ASP.NET_MVC.Models;

namespace Doan_ASP.NET_MVC.Controllers
{
    public class Image_detailController : Controller
    {
        private ShopModelContext db = new ShopModelContext();

        // GET: Image_detail
        
        public ActionResult GetProduct(int id)
        {
            var img = (from a in db.Image_detail
                      
                      where a.product_id == id
                      select a).ToList();
            return View(img);
        }

        // GET: Image_detail/Details/5
        
        // GET: Image_detail/Create
        public ActionResult Create()
        {
            
            ViewBag.product_id = new SelectList(db.Products, "product_id", "product_name");
            return View();
        }

        // POST: Image_detail/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "image_id,product_id,product_image_detail")] Image_detail image_detail)
        {
            if (ModelState.IsValid)
            {
                
                db.Image_detail.Add(image_detail);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.product_id = new SelectList(db.Products, "product_id", "product_name", image_detail.product_id);
            return View(image_detail);
        }

        // GET: Image_detail/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Image_detail image_detail = db.Image_detail.Find(id);
            if (image_detail == null)
            {
                return HttpNotFound();
            }
            ViewBag.product_id = new SelectList(db.Products, "product_id", "product_name", image_detail.product_id);
            return View(image_detail);
        }

        // POST: Image_detail/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "image_id,product_id,product_image_detail")] Image_detail image_detail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(image_detail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.product_id = new SelectList(db.Products, "product_id", "product_name", image_detail.product_id);
            return View(image_detail);
        }

        // GET: Image_detail/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Image_detail image_detail = db.Image_detail.Find(id);
            if (image_detail == null)
            {
                return HttpNotFound();
            }
            return View(image_detail);
        }

        // POST: Image_detail/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Image_detail image_detail = db.Image_detail.Find(id);
            db.Image_detail.Remove(image_detail);
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
