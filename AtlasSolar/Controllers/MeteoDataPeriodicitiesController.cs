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
    public class MeteoDataPeriodicitiesController : Controller
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

        // GET: MeteoDataPeriodicities
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<ActionResult> Index(string SortOrder, string Code, string NameEN, string NameKZ, string NameRU, int? Page)
        {
            var meteodataperiodicities = db.MeteoDataPeriodicities
                .Where(m => true);

            ViewBag.CodeFilter = Code;
            ViewBag.NameENFilter = NameEN;
            ViewBag.NameKZFilter = NameKZ;
            ViewBag.NameRUFilter = NameRU;

            ViewBag.CodeSort = SortOrder == "Code" ? "CodeDesc" : "Code";
            ViewBag.NameENSort = SortOrder == "NameEN" ? "NameENDesc" : "NameEN";
            ViewBag.NameKZSort = SortOrder == "NameKZ" ? "NameKZDesc" : "NameKZ";
            ViewBag.NameRUSort = SortOrder == "NameRU" ? "NameRUDesc" : "NameRU";

            if (!string.IsNullOrEmpty(Code))
            {
                meteodataperiodicities = meteodataperiodicities.Where(m => m.Code == Code);
            }
            if (!string.IsNullOrEmpty(NameEN))
            {
                meteodataperiodicities = meteodataperiodicities.Where(m => m.NameEN.ToLower().Contains(NameEN.ToLower()));
            }
            if (!string.IsNullOrEmpty(NameKZ))
            {
                meteodataperiodicities = meteodataperiodicities.Where(m => m.NameKZ.ToLower().Contains(NameKZ.ToLower()));
            }
            if (!string.IsNullOrEmpty(NameRU))
            {
                meteodataperiodicities = meteodataperiodicities.Where(m => m.NameRU.ToLower().Contains(NameRU.ToLower()));
            }

            switch (SortOrder)
            {
                case "Code":
                    meteodataperiodicities = meteodataperiodicities.OrderBy(m => m.Code);
                    break;
                case "CodeDesc":
                    meteodataperiodicities = meteodataperiodicities.OrderByDescending(m => m.Code);
                    break;
                case "NameEN":
                    meteodataperiodicities = meteodataperiodicities.OrderBy(m => m.NameEN);
                    break;
                case "NameENDesc":
                    meteodataperiodicities = meteodataperiodicities.OrderByDescending(m => m.NameEN);
                    break;
                case "NameKZ":
                    meteodataperiodicities = meteodataperiodicities.OrderBy(m => m.NameKZ);
                    break;
                case "NameKZDesc":
                    meteodataperiodicities = meteodataperiodicities.OrderByDescending(m => m.NameKZ);
                    break;
                case "NameRU":
                    meteodataperiodicities = meteodataperiodicities.OrderBy(m => m.NameRU);
                    break;
                case "NameRUDesc":
                    meteodataperiodicities = meteodataperiodicities.OrderByDescending(m => m.NameRU);
                    break;
                default:
                    meteodataperiodicities = meteodataperiodicities.OrderBy(m => m.Id);
                    break;
            }

            int PageSize = 50;
            int PageNumber = (Page ?? 1);

            ViewBag.Role = "";
            string CurrentUserId = User.Identity.GetUserId();
            if (UserManager.IsInRole(CurrentUserId, "Admin"))
            {
                ViewBag.Role = "Admin";
            }

            return View(meteodataperiodicities.ToPagedList(PageNumber, PageSize));
        }

        // GET: MeteoDataPeriodicities/Details/5
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MeteoDataPeriodicity meteoDataPeriodicity = await db.MeteoDataPeriodicities.FindAsync(id);
            if (meteoDataPeriodicity == null)
            {
                return HttpNotFound();
            }

            ViewBag.Role = "";
            string CurrentUserId = User.Identity.GetUserId();
            if (UserManager.IsInRole(CurrentUserId, "Admin"))
            {
                ViewBag.Role = "Admin";
            }

            return View(meteoDataPeriodicity);
        }

        // GET: MeteoDataPeriodicities/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: MeteoDataPeriodicities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Create([Bind(Include = "Id,Code,NameEN,NameKZ,NameRU")] MeteoDataPeriodicity meteoDataPeriodicity)
        {
            if (ModelState.IsValid)
            {
                db.MeteoDataPeriodicities.Add(meteoDataPeriodicity);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(meteoDataPeriodicity);
        }

        // GET: MeteoDataPeriodicities/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MeteoDataPeriodicity meteoDataPeriodicity = await db.MeteoDataPeriodicities.FindAsync(id);
            if (meteoDataPeriodicity == null)
            {
                return HttpNotFound();
            }
            return View(meteoDataPeriodicity);
        }

        // POST: MeteoDataPeriodicities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Code,NameEN,NameKZ,NameRU")] MeteoDataPeriodicity meteoDataPeriodicity)
        {
            if (ModelState.IsValid)
            {
                db.Entry(meteoDataPeriodicity).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(meteoDataPeriodicity);
        }

        // GET: MeteoDataPeriodicities/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MeteoDataPeriodicity meteoDataPeriodicity = await db.MeteoDataPeriodicities.FindAsync(id);
            if (meteoDataPeriodicity == null)
            {
                return HttpNotFound();
            }
            return View(meteoDataPeriodicity);
        }

        // POST: MeteoDataPeriodicities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            MeteoDataPeriodicity meteoDataPeriodicity = await db.MeteoDataPeriodicities.FindAsync(id);
            db.MeteoDataPeriodicities.Remove(meteoDataPeriodicity);
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
