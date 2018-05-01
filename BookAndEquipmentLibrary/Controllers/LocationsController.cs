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
    public class LocationsController : Controller
    {
        private BookAndEquipmentLibraryContext db = new BookAndEquipmentLibraryContext();

        //// GET: Locations
        //public ActionResult Index()
        //{
        //    return View(db.Locations.ToList());
        //}

        // GET: Locations with search string
        public ActionResult Index(string searchString)
        {
            try
            {
                IEnumerable<Location> locations = new List<Location>();
                if (!String.IsNullOrEmpty(searchString))
                {
                    if (Int32.TryParse(searchString, out int searchInt))
                    {
                        locations = db.Locations.Where(x => x.LocationId.Equals(searchInt));
                    }
                    else
                    {
                        locations = db.Locations
                        .Where(x => x.Name.Contains(searchString));
                    }
                }
                else
                {
                    locations = db.Locations;
                }

                return View(locations.ToList());
            }
            catch (Exception ex)
            {
                ViewBag.errorMessage = (DebuggingUtilities.GetExceptionString(ex));
                return View("../Home/Index");
            }
        }

        // GET: Locations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Location location = db.Locations.Find(id);
            if (location == null)
            {
                return HttpNotFound();
            }
            return View(location);
        }

        // GET: Locations/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Locations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "LocationId,Name,Description,RowVersion")] Location location)
        {
            if (ModelState.IsValid)
            {
                db.Locations.Add(location);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(location);
        }

        // GET: Locations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Location location = db.Locations.Find(id);
            if (location == null)
            {
                return HttpNotFound();
            }
            return View(location);
        }

        // POST: Locations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "LocationId,Name,Description,RowVersion")] Location location)
        {
            if (ModelState.IsValid)
            {
                db.Entry(location).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(location);
        }

        // GET: Locations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Location location = db.Locations.Find(id);
            if (location == null)
            {
                return HttpNotFound();
            }
            return View(location);
        }

        // POST: Locations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Location location = db.Locations.Find(id);
            db.Locations.Remove(location);
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
