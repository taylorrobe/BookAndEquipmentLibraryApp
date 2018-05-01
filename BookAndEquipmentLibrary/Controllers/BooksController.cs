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
using BookAndEquipmentLibrary.Models.AssetViewModels;
using PagedList;

namespace BookAndEquipmentLibrary.Controllers
{
    public class BooksController : Controller
    {
        private BookAndEquipmentLibraryContext db = new BookAndEquipmentLibraryContext();

        // GET: Books with search string
        public ActionResult Index(string searchString, string sortOrder, string currentFilter, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;
            try
            {
                IEnumerable<Book> books = new List<Book>();

                //search filter
                if (!String.IsNullOrEmpty(searchString))
                {
                    if (Int32.TryParse(searchString, out int searchInt))
                    {
                        books = db.Assets.OfType<Book>()
                        .Include(a => a.Location)
                        .Include(a => a.Status)
                        .Where(x => x.AssetId.Equals(searchInt));
                    }
                    else
                    {
                        books = db.Assets.OfType<Book>().Where(x => x.Author.Contains(searchString)
                            || x.Name.Contains(searchString));
                    }
                }
                else
                {
                    books = db.Assets.OfType<Book>();
                }

                //ordering
                ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_asc" : "";
                ViewBag.AuthorSortParm = String.IsNullOrEmpty(sortOrder) ? "author_asc" : "";
                ViewBag.LocationSortParm = String.IsNullOrEmpty(sortOrder) ? "location_asc" : "";


                switch (sortOrder)
                {
                    case "name_asc":
                        books = books.OrderBy(b => b.Name);
                        break;
                    case "author_asc":
                        books = books.OrderBy(b => b.Author);
                        break;
                    case "location_asc":
                        books = books.OrderBy(b => b.Location.Name);
                        break;
                    default:
                        books = books.OrderBy(b => b.AssetId);
                        break;
                }

                int pageSize = 20;
                int pageNumber = (page ?? 1);
                return View(books.ToPagedList(pageNumber, pageSize));
            }
            catch (Exception ex)
            {
                ViewBag.errorMessage = (DebuggingUtilities.GetExceptionString(ex));
                return View("../Home/Index");
            }
        }
        //public ActionResult Index(string sortOrder)
        //{
        //    ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
        //    ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
        //    var students = from s in db.Students
        //                   select s;
        //    switch (sortOrder)
        //    {
        //        case "name_desc":
        //            students = students.OrderByDescending(s => s.LastName);
        //            break;
        //        case "Date":
        //            students = students.OrderBy(s => s.EnrollmentDate);
        //            break;
        //        case "date_desc":
        //            students = students.OrderByDescending(s => s.EnrollmentDate);
        //            break;
        //        default:
        //            students = students.OrderBy(s => s.LastName);
        //            break;
        //    }
        //    return View(students.ToList());
        //}

        // GET: Books/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Assets
                .OfType<Book>()
                .Include(a => a.Location)
                .Include(a => a.Status)
                .FirstOrDefault(a => a.AssetId == id);

            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // GET: Books/Create
        public ActionResult Create()
        {
            ViewBag.LocationId = new SelectList(db.Locations, "LocationId", "Name");
            ViewBag.StatusId = new SelectList(db.Status, "StatusId", "Name", db.Status.FirstOrDefault(a => a.Name == "CheckedIn").StatusId);
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name,Author, Publisher,ISBN,DeweyIndex,Description,Year,LocationId,StatusId,RowVersion")] BookViewModel model)
        {
            if (ModelState.IsValid)
            {
                Book book = new Book
                {
                    Name = model.Name,
                    Author = model.Author,
                    Publisher = model.Publisher,
                    ISBN = model.ISBN,
                    DeweyIndex = model.DeweyIndex,
                    Description = model.Description,
                    Year = model.Year,
                    LocationId = model.LocationId,
                    StatusId = model.StatusId,
                    RowVersion = model.RowVersion
                };
                db.Assets.Add(book);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.LocationId = new SelectList(db.Locations, "LocationId", "Name", model.LocationId);
            ViewBag.StatusId = new SelectList(db.Status, "StatusId", "Name", model.StatusId);
            return View(model);
        }

        // GET: Books/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Asset book = db.Assets.Find(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            ViewBag.LocationId = new SelectList(db.Locations, "LocationId", "Name", book.LocationId);
            ViewBag.StatusId = new SelectList(db.Status, "StatusId", "Name", book.StatusId);
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AssetId,Name,Author, Publisher,ISBN,DeweyIndex,Description,Year,LocationId,StatusId,RowVersion")] BookViewModel model)
        {
            if (ModelState.IsValid)
            {
                Book book = new Book
                {
                    AssetId = model.AssetId,
                    Name = model.Name,
                    Author = model.Author,
                    Publisher = model.Publisher,
                    ISBN = model.ISBN,
                    DeweyIndex = model.DeweyIndex,
                    Description = model.Description,
                    Year = model.Year,
                    LocationId = model.LocationId,
                    StatusId = model.StatusId,
                    RowVersion = model.RowVersion
                };
                db.Entry(book).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.LocationId = new SelectList(db.Locations, "LocationId", "Name", model.LocationId);
            ViewBag.StatusId = new SelectList(db.Status, "StatusId", "Name", model.StatusId);
            return View(model);
        }

        // GET: Books/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Book book = db.Assets.OfType<Book>()
                .Include(a => a.Location)
                .Include(a => a.Status)
                .FirstOrDefault(a=>a.AssetId == id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Book book = db.Assets.OfType<Book>()
                .Include(a => a.Location)
                .Include(a => a.Status)
                .FirstOrDefault(a => a.AssetId == id);
            db.Assets.Remove(book);
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
