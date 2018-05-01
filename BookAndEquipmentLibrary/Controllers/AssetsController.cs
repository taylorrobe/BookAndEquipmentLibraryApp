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

namespace BookAndEquipmentLibrary.Controllers
{
    public class AssetsController : Controller
    {
        private BookAndEquipmentLibraryContext db = new BookAndEquipmentLibraryContext();

        // GET: Assets with search string
        public ActionResult Index(string searchString)
        {
            try
            {
                IEnumerable<Asset> assets = new List<Asset>();
                if (!String.IsNullOrEmpty(searchString))
                {
                    if (Int32.TryParse(searchString, out int searchInt))
                    {
                        assets = db.Assets.Where(x => x.AssetId.Equals(searchInt));
                    }
                    else
                    {
                        assets = db.Assets.Where(x => x.Name.Contains(searchString));
                    }
                }
                else
                {
                    assets = db.Assets;
                }

                return View(assets.ToList());
            }
            catch (Exception ex)
            {
                ViewBag.errorMessage = (DebuggingUtilities.GetExceptionString(ex));
                return View("../Home/Index");
            }
        }

        // GET: Assets/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Asset asset = db.Assets.Find(id);
            if (asset == null)
            {
                return HttpNotFound();
            }
            return View(asset);
        }

        // GET: Assets/Create
        public ActionResult Create()
        {
            ViewBag.LocationId = new SelectList(db.Locations, "LocationId", "Name");
            ViewBag.StatusId = new SelectList(db.Status, "StatusId", "Name");
            return View();
        }

        // POST: Assets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AssetId,Name,Description,Year,LocationId,StatusId,RowVersion")] Asset asset)
        {
            if (ModelState.IsValid)
            {
                db.Assets.Add(asset);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.LocationId = new SelectList(db.Locations, "LocationId", "Name", asset.LocationId);
            ViewBag.StatusId = new SelectList(db.Status, "StatusId", "Name", asset.StatusId);
            return View(asset);
        }

        // GET: Assets/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Asset asset = db.Assets.Find(id);
            if (asset == null)
            {
                return HttpNotFound();
            }
            ViewBag.LocationId = new SelectList(db.Locations, "LocationId", "Name", asset.LocationId);
            ViewBag.StatusId = new SelectList(db.Status, "StatusId", "Name", asset.StatusId);
            return View(asset);
        }

        // POST: Assets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AssetId,Name,Description,Year,LocationId,StatusId,RowVersion")] Asset asset)
        {
            if (ModelState.IsValid)
            {
                db.Entry(asset).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.LocationId = new SelectList(db.Locations, "LocationId", "Name", asset.LocationId);
            ViewBag.StatusId = new SelectList(db.Status, "StatusId", "Name", asset.StatusId);
            return View(asset);
        }

        // GET: Assets/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Asset asset = db.Assets.Find(id);
            if (asset == null)
            {
                return HttpNotFound();
            }
            return View(asset);
        }

        // POST: Assets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Asset asset = db.Assets.Find(id);
            db.Assets.Remove(asset);
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
