using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RSProject.Data.EF;
using Microsoft.AspNet.Identity;

namespace RSProject.UI.MVC.Controllers
{
    [Authorize]
    public class ReservationsController : Controller
    {
        private RSEntities db = new RSEntities();

        // GET: Reservations
        public ActionResult Index()
        {
            if (User.IsInRole("Customer"))
            {
                string currentUser = User.Identity.GetUserId();
                var reservations = db.Reservations.Where(r => r.CustomerAsset.OwnerID == currentUser).Include(i => i.CustomerAsset).Include(i => i.Location).Include(r => r.Service);
                return View(reservations.ToList());
            }
            else
            {

                var reservations = db.Reservations.Include(r => r.CustomerAsset).Include(r => r.Location).Include(r => r.Service);
                return View(reservations.ToList());
            }
        }

        // GET: Reservations/Details/5
        [Authorize(Roles = "Admin")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservation reservation = db.Reservations.Find(id);
            if (reservation == null)
            {
                return HttpNotFound();
            }
            return View(reservation);
        }

        // GET: Reservations/Create
        [Authorize(Roles = "Admin, Customer")]
        public ActionResult Create()
        {
            if (User.IsInRole("Customer"))
            {
                string currentUser = User.Identity.GetUserId();

                ViewBag.CustomerAssetID = new SelectList(db.CustomerAssets.Where(c => c.OwnerID == currentUser), "CustomerAssetID", "AssetName");
                ViewBag.LocationID = new SelectList(db.Locations, "LocationID", "City");
                ViewBag.ServiceID = new SelectList(db.Services, "ServiceID", "Name");

                return View();
            }
            else
            {


                ViewBag.CustomerAssetID = new SelectList(db.CustomerAssets, "CustomerAssetID", "AssetName");
                ViewBag.LocationID = new SelectList(db.Locations.OrderBy(o => o.LocationName), "LocationID", "City");
                ViewBag.ServiceID = new SelectList(db.Services, "ServiceID", "Name");

                return View();
            }

        }

        // POST: Reservations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Customer")]
        public ActionResult Create([Bind(Include = "ReservationID,CustomerAssetID,LocationID,ServiceID,ReservationDate,ReservationTime")] Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                if (User.IsInRole("Customer"))
                {
                    int limit = db.Locations.Where(l => l.LocationID == reservation.LocationID).Select(l => l.ReservationLimit).Single();

                    int numberOfRes = db.Reservations.Where(r => r.LocationID == reservation.LocationID && r.ReservationDate == reservation.ReservationDate).Count();

                    if (numberOfRes < limit)
                    {
                        db.Reservations.Add(reservation);
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ViewBag.LimitFull = $"Sorry reservation full, please choose another date or location!";
                        // return View(reservation);
                    }
                }
                else
                {
                    db.Reservations.Add(reservation);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

            }

            ViewBag.CustomerAssetID = new SelectList(db.CustomerAssets, "CustomerAssetID", "AssetName", reservation.CustomerAssetID);
            ViewBag.LocationID = new SelectList(db.Locations, "LocationID", "City", reservation.LocationID);
            ViewBag.ServiceID = new SelectList(db.Services, "ServiceID", "Name", reservation.ServiceID);
            return View(reservation);
        }

        // GET: Reservations/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservation reservation = db.Reservations.Find(id);
            if (reservation == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerAssetID = new SelectList(db.CustomerAssets, "CustomerAssetID", "AssetName", reservation.CustomerAssetID);
            ViewBag.LocationID = new SelectList(db.Locations, "LocationID", "City", reservation.LocationID);
            ViewBag.ServiceID = new SelectList(db.Services, "ServiceID", "Name", reservation.ServiceID);
            return View(reservation);
        }

        // POST: Reservations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "ReservationID,CustomerAssetID,LocationID,ServiceID,ReservationDate,ReservationTime")] Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(reservation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CustomerAssetID = new SelectList(db.CustomerAssets, "CustomerAssetID", "AssetName", reservation.CustomerAssetID);
            ViewBag.LocationID = new SelectList(db.Locations, "LocationID", "City", reservation.LocationID);
            ViewBag.ServiceID = new SelectList(db.Services, "ServiceID", "Name", reservation.ServiceID);
            return View(reservation);
        }

        // GET: Reservations/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Reservation reservation = db.Reservations.Find(id);
            if (reservation == null)
            {
                return HttpNotFound();
            }
            return View(reservation);
        }

        // POST: Reservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            Reservation reservation = db.Reservations.Find(id);
            db.Reservations.Remove(reservation);
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
