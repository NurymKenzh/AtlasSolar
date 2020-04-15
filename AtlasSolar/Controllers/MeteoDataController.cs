using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AtlasSolar.Models;
using PagedList;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace AtlasSolar.Controllers
{
    public class MeteoDataController : Controller
    {
        private NpgsqlContext db = new NpgsqlContext();
        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // GET: MeteoData
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<ActionResult> Index(string SortOrder, int? MeteoDataTypeId, int? Year, int? Month, int? Day, decimal? Longitude, decimal? Latitude, int? Page)
        {
            HttpCookie cookie = System.Web.HttpContext.Current.Request.Cookies["Language"];

            var meteodatas = db.MeteoDatas
                .Where(m => true)
                .Include(m => m.MeteoDataType);

            ViewBag.MeteoDataTypeIdFilter = MeteoDataTypeId;
            ViewBag.YearFilter = Year;
            ViewBag.MonthFilter = Month;
            ViewBag.DayFilter = Day;
            ViewBag.LongitudeFilter = Longitude;
            ViewBag.LatitudeFilter = Latitude;

            ViewBag.MeteoDataTypeSort = SortOrder == "MeteoDataType" ? "MeteoDataTypeDesc" : "MeteoDataType";
            ViewBag.YearSort = SortOrder == "Year" ? "YearDesc" : "Year";
            ViewBag.MonthSort = SortOrder == "Month" ? "MonthDesc" : "Month";
            ViewBag.DaySort = SortOrder == "Day" ? "DayDesc" : "Day";
            ViewBag.LongitudeSort = SortOrder == "Longitude" ? "LongitudeDesc" : "Longitude";
            ViewBag.LatitudeSort = SortOrder == "Latitude" ? "LatitudeDesc" : "Latitude";

            if (MeteoDataTypeId != null)
            {
                meteodatas = meteodatas.Where(m => m.MeteoDataTypeId == MeteoDataTypeId);
            }
            if (Year != null)
            {
                meteodatas = meteodatas.Where(m => m.Year == Year);
            }
            if (Month != null)
            {
                meteodatas = meteodatas.Where(m => m.Month == Month);
            }
            if (Day != null)
            {
                meteodatas = meteodatas.Where(m => m.Day == Day);
            }
            if (Longitude != null)
            {
                meteodatas = meteodatas.Where(m => m.Longitude == Longitude);
            }
            if (Latitude != null)
            {
                meteodatas = meteodatas.Where(m => m.Latitude == Latitude);
            }

            IList<MeteoDataType> meteodatatypes = db.MeteoDataTypes
                .Where(m => true)
                .ToList();
            meteodatatypes = meteodatatypes
                .OrderBy(m => m.Name)
                .ToList();
            ViewBag.MeteoDataTypes = new SelectList(meteodatatypes, "Id", "NameGroupAdditional");

            ViewBag.Role = "";
            string CurrentUserId = User.Identity.GetUserId();
            if (UserManager.IsInRole(CurrentUserId, "Admin"))
            {
                ViewBag.Role = "Admin";
            }

            int PageSize = 50;
            int PageNumber = (Page ?? 1);

            return View(meteodatas.Take(100000).OrderBy(m => m.Id).ToPagedList(PageNumber, PageSize));
        }

        // GET: MeteoData/Details/5
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MeteoData meteoData = await db.MeteoDatas
                .Where(m => m.Id == id)
                .Include(m => m.MeteoDataType)
                .FirstOrDefaultAsync();
            if (meteoData == null)
            {
                return HttpNotFound();
            }

            ViewBag.Role = "";
            string CurrentUserId = User.Identity.GetUserId();
            if (UserManager.IsInRole(CurrentUserId, "Admin"))
            {
                ViewBag.Role = "Admin";
            }

            return View(meteoData);
        }

        // GET: MeteoData/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            ViewBag.MeteoDataTypeId = new SelectList(db.MeteoDataTypes
                .ToList()
                .OrderBy(m => m.Name), "Id", "NameGroupAdditional");
            return View();
        }

        // POST: MeteoData/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Create([Bind(Include = "Id,MeteoDataTypeId,Year,Month,Day,Longitude,Latitude,Value")] MeteoData meteoData)
        {
            if (ModelState.IsValid)
            {
                db.MeteoDatas.Add(meteoData);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.MeteoDataTypeId = new SelectList(db.MeteoDataTypes
                .ToList()
                .OrderBy(m => m.Name), "Id", "NameGroupAdditional", meteoData.MeteoDataTypeId);
            return View(meteoData);
        }

        // GET: MeteoData/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MeteoData meteoData = await db.MeteoDatas.FindAsync(id);
            if (meteoData == null)
            {
                return HttpNotFound();
            }
            ViewBag.MeteoDataTypeId = new SelectList(db.MeteoDataTypes
                .ToList()
                .OrderBy(m => m.Name), "Id", "NameGroupAdditional", meteoData.MeteoDataTypeId);
            return View(meteoData);
        }

        // POST: MeteoData/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit([Bind(Include = "Id,MeteoDataTypeId,Year,Month,Day,Longitude,Latitude,Value")] MeteoData meteoData)
        {
            if (ModelState.IsValid)
            {
                db.Entry(meteoData).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.MeteoDataTypeId = new SelectList(db.MeteoDataTypes
                .ToList()
                .OrderBy(m => m.Name), "Id", "NameGroupAdditional", meteoData.MeteoDataTypeId);
            return View(meteoData);
        }

        // GET: MeteoData/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MeteoData meteoData = await db.MeteoDatas
                .Where(m => m.Id == id)
                .Include(m => m.MeteoDataType)
                .FirstOrDefaultAsync();
            if (meteoData == null)
            {
                return HttpNotFound();
            }
            return View(meteoData);
        }

        // POST: MeteoData/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            MeteoData meteoData = await db.MeteoDatas.FindAsync(id);
            db.MeteoDatas.Remove(meteoData);
            await db.SaveChangesAsync();
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
