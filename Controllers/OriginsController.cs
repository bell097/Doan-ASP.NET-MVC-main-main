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
    public class OriginsController : Controller
    {
        private ShopModelContext db = new ShopModelContext();

        // GET: Origins
        public ActionResult Index()
        {
            return View(db.Origins.ToList());
        }


        // GET: Origins/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Origins/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "origin_id,origin_name")] Origin origin)
        {
            if (ModelState.IsValid)
            {
                db.Origins.Add(origin);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(origin);
        }

        // GET: Origins/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Origin origin = db.Origins.Find(id);
            if (origin == null)
            {
                return HttpNotFound();
            }
            return View(origin);
        }

        // POST: Origins/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "origin_id,origin_name")] Origin origin)
        {
            if (ModelState.IsValid)
            {
                db.Entry(origin).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(origin);
        }

        // GET: Origins/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Origin origin = db.Origins.Find(id);
            if (origin == null)
            {
                return HttpNotFound();
            }
            return View(origin);
        }

        // POST: Origins/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            Origin origin = db.Origins.Find(id);
            db.Origins.Remove(origin);
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
