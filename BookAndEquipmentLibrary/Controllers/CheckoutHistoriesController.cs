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
    public class CheckoutHistoriesController : Controller
    {
        private BookAndEquipmentLibraryContext db = new BookAndEquipmentLibraryContext();

        //// GET: CheckoutHistories
        //public ActionResult Index()
        //{
        //    var checkoutHistories = db.CheckoutHistories.Include(c => c.Asset).Include(c => c.Patron);
        //    return View(checkoutHistories.ToList());
        //}

        // GET: CheckoutHistories with search string
        public ActionResult Index(string searchString)
        {
            try
            {
                IEnumerable<CheckoutHistory> checkoutHistories = new List<CheckoutHistory>();
                if (!String.IsNullOrEmpty(searchString))
                {
                    if (Int32.TryParse(searchString, out int searchInt))
                    {
                        checkoutHistories = db.CheckoutHistories.Include(c => c.Asset).Include(c => c.Patron)
                        .Where(x => x.Asset.AssetId.Equals(searchInt)
                        || x.Patron.PatronId.Equals(searchInt));
                    }
                    else
                    {
                        checkoutHistories = db.CheckoutHistories.Include(c => c.Asset).Include(c => c.Patron)
                            .Where(x => x.Asset.Name.Contains(searchString)
                            || x.Patron.Forename.Contains(searchString)
                            || x.Patron.Surname.Contains(searchString));
                    }
                }
                else
                {
                    checkoutHistories = db.CheckoutHistories.Include(c => c.Asset).Include(c => c.Patron);
                }

                return View(checkoutHistories.ToList());
            }
            catch (Exception ex)
            {
                ViewBag.errorMessage = (DebuggingUtilities.GetExceptionString(ex));
                return View("../Home/Index");
            }
        }

        // GET: CheckoutHistories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CheckoutHistory checkoutHistory = db.CheckoutHistories.Find(id);
            if (checkoutHistory == null)
            {
                return HttpNotFound();
            }
            return View(checkoutHistory);
        }

        // GET: CheckoutHistories/Create
        public ActionResult Create()
        {
            ViewBag.AssetId = new SelectList(db.Assets, "AssetId", "Name");
            ViewBag.PatronId = new SelectList(db.Patrons, "PatronId", "Forename");
            return View();
        }

        // POST: CheckoutHistories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CheckoutHistoryId,AssetId,PatronId,CheckedOutDate,CheckedInDate,Notes,RowVersion")] CheckoutHistory checkoutHistory)
        {
            if (ModelState.IsValid)
            {
                db.CheckoutHistories.Add(checkoutHistory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AssetId = new SelectList(db.Assets, "AssetId", "Name", checkoutHistory.AssetId);
            ViewBag.PatronId = new SelectList(db.Patrons, "PatronId", "Forename", checkoutHistory.PatronId);
            return View(checkoutHistory);
        }

        // GET: CheckoutHistories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CheckoutHistory checkoutHistory = db.CheckoutHistories.Find(id);
            if (checkoutHistory == null)
            {
                return HttpNotFound();
            }
            ViewBag.AssetId = new SelectList(db.Assets, "AssetId", "Name", checkoutHistory.AssetId);
            ViewBag.PatronId = new SelectList(db.Patrons, "PatronId", "Forename", checkoutHistory.PatronId);
            return View(checkoutHistory);
        }

        // POST: CheckoutHistories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CheckoutHistoryId,AssetId,PatronId,CheckedOutDate,CheckedInDate,Notes,RowVersion")] CheckoutHistory checkoutHistory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(checkoutHistory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AssetId = new SelectList(db.Assets, "AssetId", "Name", checkoutHistory.AssetId);
            ViewBag.PatronId = new SelectList(db.Patrons, "PatronId", "Forename", checkoutHistory.PatronId);
            return View(checkoutHistory);
        }

        // GET: CheckoutHistories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CheckoutHistory checkoutHistory = db.CheckoutHistories.Find(id);
            if (checkoutHistory == null)
            {
                return HttpNotFound();
            }
            return View(checkoutHistory);
        }

        // POST: CheckoutHistories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CheckoutHistory checkoutHistory = db.CheckoutHistories.Find(id);
            db.CheckoutHistories.Remove(checkoutHistory);
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
