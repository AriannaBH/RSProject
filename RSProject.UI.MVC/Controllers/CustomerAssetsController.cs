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
using System.Drawing;
using RSProject.UI.MVC.Utilities;

namespace RSProject.UI.MVC.Controllers
{
    [Authorize]
    public class CustomerAssetsController : Controller
    {
        private RSEntities db = new RSEntities();

        // GET: CustomerAssets
        public ActionResult Index()
        {
            if (User.IsInRole("Customer"))
            {
                string currentUser = User.Identity.GetUserId();
                var customerAssets = db.CustomerAssets.Where(c => c.OwnerID == currentUser).Include(c => c.UserDetail);
                return View(customerAssets.ToList());
            }
            else
            {
                //string currentUser = User.Identity.GetUserId();
                var customerAssets = db.CustomerAssets.Include(c => c.UserDetail);
                return View(customerAssets.ToList());
            }


        }

        // GET: CustomerAssets/Details/5
        [Authorize]
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
        [Authorize]
        public ActionResult Create()
        {
            if (User.IsInRole("Customer"))
            {
                string currentUser = User.Identity.GetUserId();
                ViewBag.OwnerID = new SelectList(db.UserDetails.Where(o => o.AspNetUser.Id == currentUser), "UserID", "FullName");
                return View();
            }
            else
            {
                ViewBag.OwnerID = new SelectList(db.UserDetails, "UserID", "FullName");
                return View();
            }
        }

        // POST: CustomerAssets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create([Bind(Include = "CustomerAssetID,AssetName,AssetPhoto,OwnerID,SpecialNotes,IsActive,DateAdded")] CustomerAsset customerAsset, HttpPostedFileBase assetPhoto)
        {
            if (ModelState.IsValid)
            {
                #region File Upload - Using the Image Service
                string imgName = "noImage.png";

                if (assetPhoto != null)
                {
                    //retrieve the fileName and reassign it to the variable
                    imgName = assetPhoto.FileName;

                    //declare and assign the extension
                    string ext = imgName.Substring(imgName.LastIndexOf('.'));

                    //declare a good list of file extensions
                    string[] goodExts = { ".jpeg", ".jpg", ".gif", ".png" };

                    //check the variable (ToLower()) against the list and verify the content length is less than 4MB
                    if (goodExts.Contains(ext.ToLower()) && (assetPhoto.ContentLength <= 4194304))
                    {

                        //rename the file using a guid (see create POST for other unique naming options) - use the Covention in BOTH places
                        imgName = Guid.NewGuid() + ext.ToLower(); //ToLower() is optional, just cleans the files on the server

                        //ResizeImage Values
                        //path
                        string savePath = Server.MapPath("~/Content/CustomerAssets/");

                        //actual image (converted image)
                        Image convertedImage = Image.FromStream(assetPhoto.InputStream);

                        //maxImageSize
                        int maxImageSize = 500;

                        //maxThumbSize
                        int maxThumbSize = 100;

                        //Call the ImageService.ResizeImage()
                        ImageService.ResizeImage(savePath, imgName, convertedImage, maxImageSize, maxThumbSize);



                    }//end extgood if
                    else
                    {
                        imgName = "noImage.png";
                    }
                }//end if !=null

                //save the object ONLY if all other conditions are met
                customerAsset.AssetPhoto = imgName;
                #endregion

                //Default New Assets to Active
                customerAsset.IsActive = true;

                db.CustomerAssets.Add(customerAsset);
                db.SaveChanges();
                return RedirectToAction("Index");

            }
            ViewBag.OwnerID = new SelectList(db.UserDetails.Include(i => i.UserID), "UserID", "FullName", customerAsset.OwnerID);
            return View(customerAsset);
        }

        // GET: CustomerAssets/Edit/5
        [Authorize]
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
            ViewBag.OwnerID = new SelectList(db.UserDetails, "UserID", "FullName", customerAsset.OwnerID);
            return View(customerAsset);
        }

        // POST: CustomerAssets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit([Bind(Include = "CustomerAssetID,AssetName,OwnerID,AssetPhoto,SpecialNotes,IsActive,DateAdded")] CustomerAsset customerAsset, HttpPostedFileBase assetPhoto)
        {
            if (ModelState.IsValid)
            {
                #region File Upload - Using the Image Service

                if (assetPhoto != null)
                {
                    //retrieve the fileName and assign it to a variable
                    string imgName = assetPhoto.FileName;

                    //declare and assign the extension
                    string ext = imgName.Substring(imgName.LastIndexOf('.'));

                    //declare a good list of file extensions
                    string[] goodExts = { ".jpeg", ".jpg", ".gif", ".png" };

                    //check the variable (ToLower()) against the list and verify the content length is less than 4MB
                    if (goodExts.Contains(ext.ToLower()) && (assetPhoto.ContentLength <= 4194304))
                    {

                        //rename the file using a guid (see create POST for other unique naming options) - use the Covention in BOTH places
                        imgName = Guid.NewGuid() + ext.ToLower(); //ToLower() is optional, just cleans the files on the server

                        //ResizeImage Values
                        //path
                        string savePath = Server.MapPath("~/Content/CustomerAssets/");

                        //actual image (converted image)
                        Image convertedImage = Image.FromStream(assetPhoto.InputStream);

                        //maxImageSize
                        int maxImageSize = 500;

                        //maxThumbSize
                        int maxThumbSize = 100;

                        //Call the ImageService.ResizeImage()
                        ImageService.ResizeImage(savePath, imgName, convertedImage, maxImageSize, maxThumbSize);

                        //DELETE from the Image Service and delete the old image
                        //--Image Service Make sure the file is NOT noImage.png && that it exists on the server BEFORE deleting
                        //we dont need to do that check
                        ImageService.Delete(savePath, customerAsset.AssetPhoto);

                        //save the object ONLY if all other conditions are met
                        customerAsset.AssetPhoto = imgName;

                    }//end extgood if
                }//end if !=null

                #endregion
                db.Entry(customerAsset).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.OwnerID = new SelectList(db.UserDetails, "UserID", "FullName", customerAsset.OwnerID);
            return View(customerAsset);
        }

        // GET: CustomerAssets/Delete/5
        [Authorize]
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
        [Authorize]
        public ActionResult DeleteConfirmed(int id)
        {
            CustomerAsset customerAsset = db.CustomerAssets.Find(id);
            db.CustomerAssets.Remove(customerAsset);
            

            ImageService.Delete(Server.MapPath("~/Content/CustomerAssets/"), customerAsset.AssetPhoto);


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
