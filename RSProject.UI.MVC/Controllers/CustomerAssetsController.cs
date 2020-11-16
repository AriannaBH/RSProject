using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RSProject.Data.EF;

namespace RSProject.UI.MVC.Controllers
{
    public class CustomerAssetsController : Controller
    {
        private RSEntities db = new RSEntities();

        // GET: CustomerAssets
        public ActionResult Index()
        {
            var customerAssets = db.CustomerAssets.Include(c => c.UserDetail);
            return View(customerAssets.ToList());
        }

        // GET: CustomerAssets/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomerAsset customerAsset = db.CustomerAssets.Find(id);
            if (customerAsset == null)
            {
                return HttpNotFound();
            }
            return View(customerAsset);
        }

        // GET: CustomerAssets/Create
        public ActionResult Create()
        {
            ViewBag.OwnerID = new SelectList(db.UserDetails, "UserID", "FirstName");
            return View();
        }

        // POST: CustomerAssets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CustomerAssetID,AssetName,OwnerID,AssetPhoto,SpecialNotes,IsActive,DateAdded")] CustomerAsset customerAsset)
        {
            if (ModelState.IsValid)
            {
                db.CustomerAssets.Add(customerAsset);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.OwnerID = new SelectList(db.UserDetails, "UserID", "FirstName", customerAsset.OwnerID);
            return View(customerAsset);
        }

        // GET: CustomerAssets/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomerAsset customerAsset = db.CustomerAssets.Find(id);
            if (customerAsset == null)
            {
                return HttpNotFound();
            }
            ViewBag.OwnerID = new SelectList(db.UserDetails, "UserID", "FirstName", customerAsset.OwnerID);
            return View(customerAsset);
        }

        // POST: CustomerAssets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CustomerAssetID,AssetName,OwnerID,AssetPhoto,SpecialNotes,IsActive,DateAdded")] CustomerAsset customerAsset)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customerAsset).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.OwnerID = new SelectList(db.UserDetails, "UserID", "FirstName", customerAsset.OwnerID);
            return View(customerAsset);
        }

        // GET: CustomerAssets/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomerAsset customerAsset = db.CustomerAssets.Find(id);
            if (customerAsset == null)
            {
                return HttpNotFound();
            }
            return View(customerAsset);
        }

        // POST: CustomerAssets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CustomerAsset customerAsset = db.CustomerAssets.Find(id);
            db.CustomerAssets.Remove(customerAsset);
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
