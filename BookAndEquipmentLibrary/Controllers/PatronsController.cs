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
    public class PatronsController : Controller
    {
        private BookAndEquipmentLibraryContext db = new BookAndEquipmentLibraryContext();

        //// GET: Patrons
        //public ActionResult Index()
        //{
        //    return View(db.Patrons.ToList());
        //}

        // GET: Patrons with search string
        public ActionResult Index(string searchString)
        {
            try
            {
                IEnumerable<Patron> patrons = new List<Patron>();
                if (!String.IsNullOrEmpty(searchString))
                {
                    if (Int32.TryParse(searchString, out int searchInt))
                    {
                        patrons = db.Patrons
                        .Where(x => x.PatronId.Equals(searchInt));
                    }
                    else
                    {
                        patrons = db.Patrons
                            .Where(x => x.Forename.Contains(searchString)
                            || x.Surname.Contains(searchString));
                    }
                }
                else
                {
                    patrons = db.Patrons;
                }

                return View(patrons.ToList());
            }
            catch(Exception ex)
            {
                ViewBag.errorMessage = (DebuggingUtilities.GetExceptionString(ex));
                return View("../Home/Index");
            }

        }

        // GET: Patrons/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Patron patron = db.Patrons.Find(id);
            if (patron == null)
            {
                return HttpNotFound();
            }
            return View(patron);
        }

        // GET: Patrons/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Patrons/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PatronId,Forename,Surname,Phone,EmailAddress,RowVersion")] Patron patron)
        {
            if (ModelState.IsValid)
            {
                db.Patrons.Add(patron);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(patron);
        }

        // GET: Patrons/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Patron patron = db.Patrons.Find(id);
            if (patron == null)
            {
                return HttpNotFound();
            }
            return View(patron);
        }

        // POST: Patrons/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PatronId,Forename,Surname,Phone,EmailAddress,RowVersion")] Patron patron)
        {
            if (ModelState.IsValid)
            {
                db.Entry(patron).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(patron);
        }

        // GET: Patrons/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Patron patron = db.Patrons.Find(id);
            if (patron == null)
            {
                return HttpNotFound();
            }
            return View(patron);
        }

        // POST: Patrons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Patron patron = db.Patrons.Find(id);
            db.Patrons.Remove(patron);
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
