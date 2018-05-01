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
    public class EquipmentController : Controller
    {
        private BookAndEquipmentLibraryContext db = new BookAndEquipmentLibraryContext();

        // GET: Equipment with search string
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
                IEnumerable<Equipment> equipment = new List<Equipment>();
                if (!String.IsNullOrEmpty(searchString))
                {
                    if (Int32.TryParse(searchString, out int searchInt))
                    {
                        equipment = db.Assets.OfType<Equipment>()
                        .Include(a => a.Location)
                        .Include(a => a.Status)
                        .Include(a => a.AssetType)
                        .Where(x => x.AssetId.Equals(searchInt));
                    }
                    else
                    {
                        equipment = db.Assets.OfType<Equipment>()
                            .Include(a => a.Location)
                            .Include(a => a.Status)
                            .Include(a => a.AssetType)
                            .Where(x => x.Name.Contains(searchString)
                                || x.AssetType.Name.Contains(searchString));
                    }
                }
                else
                {
                    equipment = db.Assets.OfType<Equipment>()
                        .Include(a => a.Location)
                        .Include(a => a.Status)
                        .Include(a => a.AssetType);
                }


                //ordering
                ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_asc" : "";
                ViewBag.AssetTypeSortParm = String.IsNullOrEmpty(sortOrder) ? "assetType_asc" : "";
                ViewBag.LocationSortParm = String.IsNullOrEmpty(sortOrder) ? "location_asc" : "";


                switch (sortOrder)
                {
                    case "name_asc":
                        equipment = equipment.OrderBy(b => b.Name);
                        break;
                    case "assetType_asc":
                        equipment = equipment.OrderBy(b => b.AssetType);
                        break;
                    case "location_asc":
                        equipment = equipment.OrderBy(b => b.Location.Name);
                        break;
                    default:
                        equipment = equipment.OrderBy(b => b.AssetId);
                        break;
                }

                int pageSize = 20;
                int pageNumber = (page ?? 1);
                return View(equipment.ToPagedList(pageNumber, pageSize));
            }
            catch (Exception ex)
            {
                ViewBag.errorMessage = (DebuggingUtilities.GetExceptionString(ex));
                return View("../Home/Index");
            }
        }

        // GET: Equipment/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Equipment equipment = db.Assets.OfType<Equipment>()
                .Include(a => a.Location)
                .Include(a => a.Status)
                .Include(a => a.AssetType)
                .FirstOrDefault(a=> a.AssetId == id);
            if (equipment == null)
            {
                return HttpNotFound();
            }
            return View(equipment);
        }
        // GET: Equipment/Create
        public ActionResult Create()
        {
            ViewBag.AssetTypeId = new SelectList(db.AssetTypes, "AssetTypeId", "Name");
            ViewBag.LocationId = new SelectList(db.Locations, "LocationId", "Name");
            ViewBag.StatusId = new SelectList(db.Status, "StatusId", "Name", db.Status.FirstOrDefault(a=> a.Name == "CheckedIn").StatusId);
            return View();
        }

        // POST: Equipment/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AssetId,Name,Description,Year,LocationId,AssetTypeId,StatusId,RowVersion")] EquipmentViewModel model)
        {
            if (ModelState.IsValid)
            {
                Equipment equipment = new Equipment
                {
                    Name = model.Name,
                    Description = model.Description,
                    Year = model.Year,
                    LocationId = model.LocationId,
                    AssetTypeId = model.AssetTypeId,
                    StatusId = model.StatusId,
                    RowVersion = model.RowVersion
                };
                db.Assets.Add(equipment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AssetTypeId = new SelectList(db.AssetTypes, "AssetTypeId", "Name", model.AssetTypeId);
            ViewBag.LocationId = new SelectList(db.Locations, "LocationId", "Name", model.LocationId);
            ViewBag.StatusId = new SelectList(db.Status, "StatusId", "Name", model.StatusId);
            return View(model);
        }

        // GET: Equipment/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Asset equipment = db.Assets.Find(id);

            if (equipment == null)
            {
                return HttpNotFound();
            }
            ViewBag.AssetTypeId = new SelectList(db.AssetTypes, "AssetTypeId", "Name");
            ViewBag.LocationId = new SelectList(db.Locations, "LocationId", "Name", equipment.LocationId);
            ViewBag.StatusId = new SelectList(db.Status, "StatusId", "Name", equipment.StatusId);
            return View(equipment);
        }

        // POST: Equipment/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AssetId,Name,Description,Year,LocationId,AssetTypeId,StatusId,RowVersion")] EquipmentViewModel model)
        {
            if (ModelState.IsValid)
            {
                Equipment equipment = new Equipment
                {
                    AssetId = model.AssetId,
                    Name = model.Name,
                    Description = model.Description,
                    Year = model.Year,
                    LocationId = model.LocationId,
                    AssetTypeId = model.AssetTypeId,
                    StatusId = model.StatusId,
                    RowVersion = model.RowVersion
                };
                db.Entry(equipment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AssetTypeId = new SelectList(db.AssetTypes, "AssetTypeId", "Name", model.AssetTypeId);
            ViewBag.LocationId = new SelectList(db.Locations, "LocationId", "Name", model.LocationId);
            ViewBag.StatusId = new SelectList(db.Status, "StatusId", "Name", model.StatusId);
            return View(model);
        }

        public ActionResult CheckOut(Equipment model)
        {
            //TODO
            return View(model);
        }

        public ActionResult CheckIn(Equipment model)
        {
            //TODO
            return View(model);
        }

        // GET: Equipment/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Equipment equipment = db.Assets.OfType<Equipment>()
                .Include(a => a.Location)
                .Include(a => a.Status)
                .Include(a => a.AssetType)
                .FirstOrDefault(a => a.AssetId == id);
            if (equipment == null)
            {
                return HttpNotFound();
            }
            return View(equipment);
        }

        // POST: Equipment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Equipment equipment = db.Assets.OfType<Equipment>()
                .Include(a => a.Location)
                .Include(a => a.Status)
                .Include(a => a.AssetType)
                .FirstOrDefault(a => a.AssetId == id);
            db.Assets.Remove(equipment);
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
