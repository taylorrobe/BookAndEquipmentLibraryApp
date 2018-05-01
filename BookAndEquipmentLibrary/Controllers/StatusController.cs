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
    public class StatusController : Controller
    {
        private BookAndEquipmentLibraryContext db = new BookAndEquipmentLibraryContext();

        //// GET: Status
        //public ActionResult Index()
        //{
        //    return View(db.Status.ToList());
        //}

        // GET: Status with search string
        public ActionResult Index(string searchString)
        {
            try
            {
                IEnumerable<Status> statuses = new List<Status>();
                if (!String.IsNullOrEmpty(searchString))
                {
                    if (Int32.TryParse(searchString, out int searchInt))
                    {
                        statuses = db.Status.Where(x => x.StatusId.Equals(searchInt));
                    }
                    else
                    {
                        statuses = db.Status
                        .Where(x => x.Name.Contains(searchString));
                    }
                }
                else
                {
                    statuses = db.Status;
                }

                return View(statuses.ToList());
            }
            catch (Exception ex)
            {
                ViewBag.errorMessage = (DebuggingUtilities.GetExceptionString(ex));
                return View("../Home/Index");
            }
        }

        // GET: Status/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Status status = db.Status.Find(id);
            if (status == null)
            {
                return HttpNotFound();
            }
            return View(status);
        }

        // GET: Status/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Status/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "StatusId,Name,Description,RowVersion")] Status status)
        {
            if (ModelState.IsValid)
            {
                db.Status.Add(status);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(status);
        }

        // GET: Status/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Status status = db.Status.Find(id);
            if (status == null)
            {
                return HttpNotFound();
            }
            return View(status);
        }

        // POST: Status/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "StatusId,Name,Description,RowVersion")] Status status)
        {
            if (ModelState.IsValid)
            {
                db.Entry(status).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(status);
        }

        // GET: Status/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Status status = db.Status.Find(id);
            if (status == null)
            {
                return HttpNotFound();
            }
            return View(status);
        }

        // POST: Status/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Status status = db.Status.Find(id);
            db.Status.Remove(status);
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
