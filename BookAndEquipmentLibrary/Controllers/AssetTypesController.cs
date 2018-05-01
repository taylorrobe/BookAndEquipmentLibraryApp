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
    public class AssetTypesController : Controller
    {
        private BookAndEquipmentLibraryContext db = new BookAndEquipmentLibraryContext();

        //// GET: AssetTypes
        //public ActionResult Index()
        //{
        //    return View(db.AssetTypes.ToList());
        //}

        // GET: AssetTypes with search string
        public ActionResult Index(string searchString)
        {
            try
            {
                IEnumerable<AssetType> assetTypes = new List<AssetType>();
                if (!String.IsNullOrEmpty(searchString))
                {
                    if (Int32.TryParse(searchString, out int searchInt))
                    {
                        assetTypes = db.AssetTypes.Where(x => x.AssetTypeId.Equals(searchInt));
                    }
                    else
                    {
                        assetTypes = db.AssetTypes.Where(x => x.Name.Contains(searchString));
                    }
                }
                else
                {
                    assetTypes = db.AssetTypes;
                }

                return View(assetTypes.ToList());
            }
            catch (Exception ex)
            {
                ViewBag.errorMessage = (DebuggingUtilities.GetExceptionString(ex));
                return View("../Home/Index");
            }
        }

        // GET: AssetTypes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AssetType assetType = db.AssetTypes.Find(id);
            if (assetType == null)
            {
                return HttpNotFound();
            }
            return View(assetType);
        }

        // GET: AssetTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AssetTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AssetTypeId,Name,Description,RowVersion")] AssetType assetType)
        {
            if (ModelState.IsValid)
            {
                db.AssetTypes.Add(assetType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(assetType);
        }

        // GET: AssetTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AssetType assetType = db.AssetTypes.Find(id);
            if (assetType == null)
            {
                return HttpNotFound();
            }
            return View(assetType);
        }

        // POST: AssetTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AssetTypeId,Name,Description,RowVersion")] AssetType assetType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(assetType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(assetType);
        }

        // GET: AssetTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AssetType assetType = db.AssetTypes.Find(id);
            if (assetType == null)
            {
                return HttpNotFound();
            }
            return View(assetType);
        }

        // POST: AssetTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AssetType assetType = db.AssetTypes.Find(id);
            db.AssetTypes.Remove(assetType);
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
