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
    public class BillDetailsController : Controller
    {
        private ShopModelContext db = new ShopModelContext();

        // GET: BillDetails

        public ActionResult GetDetails(int id)
        {
            var detail = (from a in db.BillDetails
                          where a.bill_id == id
                          select a).ToList();

            foreach (BillDetail bills in detail)
            {
                var bill = from b in db.BillDetails
                           join p in db.Products on b.product_id equals p.product_id
                           select p;

                foreach (Product p in bill)
                {
                    bills.productname = p.product_name;
                }
            }
            return View(detail);
        }

        // GET: BillDetails/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BillDetail billDetail = db.BillDetails.Find(id);
            if (billDetail == null)
            {
                return HttpNotFound();
            }
            ViewBag.bill_id = new SelectList(db.Bills, "bill_id", "user_id", billDetail.bill_id);
            ViewBag.bill_id = new SelectList(db.Products, "product_id", "product_name", billDetail.bill_id);
            return View(billDetail);
        }

        // POST: BillDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "billdetail_id,bill_id,product_id,price,soluong")] BillDetail billDetail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(billDetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.bill_id = new SelectList(db.Bills, "bill_id", "user_id", billDetail.bill_id);
            ViewBag.bill_id = new SelectList(db.Products, "product_id", "product_name", billDetail.bill_id);
            return View(billDetail);
        }

        // GET: BillDetails/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BillDetail billDetail = db.BillDetails.Find(id);
            if (billDetail == null)
            {
                return HttpNotFound();
            }
            return View(billDetail);
        }

        // POST: BillDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BillDetail billDetail = db.BillDetails.Find(id);
            db.BillDetails.Remove(billDetail);
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
