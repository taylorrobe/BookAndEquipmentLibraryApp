using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using BookAndEquipmentLibrary.Controllers.Utilities;
using BookAndEquipmentLibrary.Models;
using BookAndEquipmentLibrary.Models.CheckoutViewModels;

namespace BookAndEquipmentLibrary.Controllers
{
    public class CheckoutsController : Controller
    {
        private BookAndEquipmentLibraryContext db = new BookAndEquipmentLibraryContext();

        // GET: Checkouts
        //public ActionResult Index()
        //{
        //    var checkouts = db.Checkouts.Include(c => c.Asset).Include(c => c.Patron);
        //    return View(checkouts.ToList());
        //}

        //the first parameter is the option that we choose and the second parameter will use the textbox value  
        public ActionResult Index(string searchString)
        {
            try
            {
                IEnumerable<Checkout> checkouts = new List<Checkout>();
                if (!String.IsNullOrEmpty(searchString))
                {
                    if (Int32.TryParse(searchString, out int searchInt))
                    {
                        checkouts = db.Checkouts.Include(c => c.Asset).Include(c => c.Patron)
                        .Where(x => x.Asset.AssetId.Equals(searchInt)
                        || x.Patron.PatronId.Equals(searchInt));
                    }
                    else
                    {
                        checkouts = db.Checkouts.Include(c => c.Asset).Include(c => c.Patron)
                            .Where(x => x.Patron.Forename.Contains(searchString)
                            || x.Patron.Surname.Contains(searchString)
                            || x.Asset.Name.Contains(searchString));
                    }
                }
                else
                {
                    checkouts = db.Checkouts.Include(c => c.Asset).Include(c => c.Patron);
                }

                return View(checkouts.ToList());
            }
            catch (Exception ex)
            {
                ViewBag.errorMessage = (DebuggingUtilities.GetExceptionString(ex));
                return View("../Home/Index");
            }
        }

        // GET: Checkouts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Checkout checkout = db.Checkouts.Find(id);
            if (checkout == null)
            {
                return HttpNotFound();
            }
            return View(checkout);
        }

        // GET: Checkouts/Create
        public ActionResult Create(int? assetId, int? patronId)
        {
            //if assetid supplied restrict to that asset
            if(assetId != null)
            {
                Asset asset = db.Assets.FirstOrDefault(a => a.AssetId == assetId);
                List<Asset> assets = new List<Asset>();
                assets.Add(asset);
                ViewBag.AssetId = new SelectList(assets, "AssetId", "DisplayName", asset.AssetId);
                ViewBag.FixedAsset = asset.AssetId;
            }
            else
            {
                ViewBag.AssetId = new SelectList(db.Assets.Where(a=>a.Status.Name.Equals("CheckedIn")), "AssetId", "DisplayName");
            }

            //if patronid supplied restrict to that patron
            if (patronId != null)
            {
                Patron patron = db.Patrons.FirstOrDefault(a => a.PatronId == patronId);
                List<Patron> patrons = new List<Patron>();
                patrons.Add(patron);
                ViewBag.PatronId = new SelectList(patrons, "PatronId", "DisplayName", patron.PatronId);
                ViewBag.FixedPatron = patron.PatronId;
            }
            else
            {
                ViewBag.PatronId = new SelectList(db.Patrons, "PatronId", "DisplayName");
            }

            //ViewBag.DefaultCheckoutDate = DateTime.Now;
            //ViewBag.DefaultReturnDate = DateTime.Today.AddMonths(1);
            CheckoutAssetOrForPatron checkout = new CheckoutAssetOrForPatron
            {
                CheckoutDate = DateTime.Now,
                ReturnDate = DateTime.Today.AddMonths(1)
            };

            return View(checkout);
        }

        // POST: Checkouts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CheckoutId,AssetId,PatronId,Checkoutdate,ReturnDate,Notes,RowVersion, FixedAsset, FixedPatron")] CheckoutAssetOrForPatron model)
        {
            if (ModelState.IsValid)
            {
                Checkout checkout = new Checkout
                {
                    AssetId = model.AssetId,
                    PatronId = model.PatronId,
                    CheckoutDate = model.CheckoutDate,
                    ReturnDate = model.ReturnDate,
                    Notes = model.Notes,
                    RowVersion = model.RowVersion,
                };
                db.Checkouts.Add(checkout);

                //also need to set asset to CheckedOut
                Asset assetModel = db.Assets.Find(model.AssetId);

                //first check if asset is available
                if(assetModel.StatusId != db.Status.FirstOrDefault(a => a.Name == "CheckedIn").StatusId)
                {
                    ViewBag.errorMessage = "The Asset is not available to be Checked out";
                    SetViewBag(model);
                    return View(model);
                }
                
                assetModel.StatusId = db.Status.FirstOrDefault(a => a.Name == "CheckedOut").StatusId;
                db.Entry(assetModel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            SetViewBag(model);

            return View(model);
        }

        // GET: Checkouts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Checkout checkout = db.Checkouts.Find(id);
            if (checkout == null)
            {
                return HttpNotFound();
            }
            ViewBag.AssetId = new SelectList(db.Assets, "AssetId", "Name", checkout.AssetId);
            ViewBag.PatronId = new SelectList(db.Patrons, "PatronId", "Surname", checkout.PatronId);
            return View(checkout);
        }

        // POST: Checkouts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CheckoutId,AssetId,PatronId,Checkoutdate,ReturnDate,Notes,RowVersion")] Checkout checkout)
        {
            if (ModelState.IsValid)
            {
                db.Entry(checkout).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AssetId = new SelectList(db.Assets, "AssetId", "Name", checkout.AssetId);
            ViewBag.PatronId = new SelectList(db.Patrons, "PatronId", "Forename", checkout.PatronId);
            return View(checkout);
        }

        // GET: Checkouts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Checkout checkout = db.Checkouts.Find(id);
            if (checkout == null)
            {
                return HttpNotFound();
            }
            return View(checkout);
        }

        // POST: Checkouts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Checkout checkout = db.Checkouts.Find(id);
            db.Checkouts.Remove(checkout);
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

        //Get
        public ActionResult CheckIn(int? checkoutId, int? assetId)
        {
            
            if ((checkoutId == null && assetId == null) || (checkoutId != null && assetId != null))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Checkout checkout = new Checkout();

            if (checkoutId != null)
            {
                checkout = db.Checkouts.FirstOrDefault(a => a.CheckoutId == checkoutId);
            }
            else
            {
                checkout = db.Checkouts.FirstOrDefault(a => a.AssetId == assetId);
            }

            if (checkout == null)
            {
                return HttpNotFound();
            }
            return View(checkout);
        }

        // POST: Checkouts/CheckIn/5
        [HttpPost, ActionName("CheckIn")]
        [ValidateAntiForgeryToken]
        public ActionResult CheckIn(int checkoutId)
        {
            Checkout checkout = db.Checkouts.FirstOrDefault(a=> a.CheckoutId == checkoutId);
            
            CheckoutHistory checkoutHistory = new CheckoutHistory
            {
                AssetId = checkout.AssetId,
                PatronId = checkout.PatronId,
                CheckedOutDate = checkout.CheckoutDate,
                CheckedInDate = DateTime.Now,
                Notes = checkout.Notes
            };
            //record checkout in history
            db.CheckoutHistories.Add(checkoutHistory);
            
            //also need to set asset to CheckedIn
            Asset assetModel = db.Assets.Find(checkout.AssetId);

            //first check if asset is currently checked out
            if (assetModel.StatusId == db.Status.FirstOrDefault(a => a.Name == "CheckedIn").StatusId)
            {
                ViewBag.errorMessage = "The Asset is already checked in";
                return View(checkout);
            }

            assetModel.StatusId = db.Status.FirstOrDefault(a => a.Name == "CheckedIn").StatusId;
            db.Entry(assetModel).State = EntityState.Modified;
            //delete checkout
            db.Checkouts.Remove(checkout);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public void SetViewBag(CheckoutAssetOrForPatron model)
        {
            if (model.FixedAsset != null)
            {
                Asset asset = db.Assets.FirstOrDefault(a => a.AssetId == model.FixedAsset);
                List<Asset> assets = new List<Asset>();
                assets.Add(asset);
                ViewBag.AssetId = new SelectList(assets, "AssetId", "Name", asset.AssetId);
                ViewBag.FixedAsset = asset.AssetId;
            }
            else
            {
                ViewBag.AssetId = new SelectList(db.Assets.Where(a => a.Status.Name.Equals("CheckedIn")), "AssetId", "Name");
            }

            if (model.FixedPatron != null)
            {
                Patron patron = db.Patrons.FirstOrDefault(a => a.PatronId == model.FixedPatron);
                List<Patron> patrons = new List<Patron>();
                patrons.Add(patron);
                ViewBag.PatronId = new SelectList(patrons, "PatronId", "Name", patron.PatronId);
                ViewBag.FixedPatron = patron.PatronId;
            }
            else
            {
                ViewBag.PatronId = new SelectList(db.Patrons, "PatronId", "Surname");
            }
        }
    }
}
