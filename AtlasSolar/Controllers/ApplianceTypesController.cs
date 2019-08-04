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
    public class ApplianceTypesController : Controller
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

        // GET: ApplianceTypes
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<ActionResult> Index(string SortOrder, string NameEN, string NameKZ, string NameRU, int? Page)
        {
            var appliancetypes = db.ApplianceTypes
                .Where(a => true);

            ViewBag.NameENFilter = NameEN;
            ViewBag.NameKZFilter = NameKZ;
            ViewBag.NameRUFilter = NameRU;

            ViewBag.NameENSort = SortOrder == "NameEN" ? "NameENDesc" : "NameEN";
            ViewBag.NameKZSort = SortOrder == "NameKZ" ? "NameKZDesc" : "NameKZ";
            ViewBag.NameRUSort = SortOrder == "NameRU" ? "NameRUDesc" : "NameRU";

            if (!string.IsNullOrEmpty(NameEN))
            {
                appliancetypes = appliancetypes.Where(a => a.NameEN.ToLower().Contains(NameEN.ToLower()));
            }
            if (!string.IsNullOrEmpty(NameKZ))
            {
                appliancetypes = appliancetypes.Where(a => a.NameKZ.ToLower().Contains(NameKZ.ToLower()));
            }
            if (!string.IsNullOrEmpty(NameRU))
            {
                appliancetypes = appliancetypes.Where(a => a.NameRU.ToLower().Contains(NameRU.ToLower()));
            }

            switch (SortOrder)
            {
                case "NameEN":
                    appliancetypes = appliancetypes.OrderBy(a => a.NameEN);
                    break;
                case "NameENDesc":
                    appliancetypes = appliancetypes.OrderByDescending(a => a.NameEN);
                    break;
                case "NameKZ":
                    appliancetypes = appliancetypes.OrderBy(a => a.NameKZ);
                    break;
                case "NameKZDesc":
                    appliancetypes = appliancetypes.OrderByDescending(a => a.NameKZ);
                    break;
                case "NameRU":
                    appliancetypes = appliancetypes.OrderBy(a => a.NameRU);
                    break;
                case "NameRUDesc":
                    appliancetypes = appliancetypes.OrderByDescending(a => a.NameRU);
                    break;
                default:
                    appliancetypes = appliancetypes.OrderBy(a => a.Id);
                    break;
            }

            ViewBag.Role = "";
            string CurrentUserId = User.Identity.GetUserId();
            if (UserManager.IsInRole(CurrentUserId, "Admin"))
            {
                ViewBag.Role = "Admin";
            }

            int PageSize = 50;
            int PageNumber = (Page ?? 1);

            return View(appliancetypes.ToPagedList(PageNumber, PageSize));
        }

        // GET: ApplianceTypes/Details/5
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplianceType applianceType = await db.ApplianceTypes.FindAsync(id);
            if (applianceType == null)
            {
                return HttpNotFound();
            }
            return View(applianceType);
        }

        // GET: ApplianceTypes/Create
        [Authorize(Roles = "Admin,Moderator")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: ApplianceTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<ActionResult> Create([Bind(Include = "Id,NameEN,NameKZ,NameRU")] ApplianceType applianceType)
        {
            if (ModelState.IsValid)
            {
                db.ApplianceTypes.Add(applianceType);
                await db.SaveChangesAsync();

                string comment = $"Id: {applianceType.Id} NameEN: {applianceType.NameEN} NameKZ: {applianceType.NameKZ} NameRU: {applianceType.NameRU}";
                SystemLog.New("ApplianceTypeCreate", comment, null, false);

                return RedirectToAction("Index");
            }

            return View(applianceType);
        }

        // GET: ApplianceTypes/Edit/5
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplianceType applianceType = await db.ApplianceTypes.FindAsync(id);
            if (applianceType == null)
            {
                return HttpNotFound();
            }
            return View(applianceType);
        }

        // POST: ApplianceTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<ActionResult> Edit([Bind(Include = "Id,NameEN,NameKZ,NameRU")] ApplianceType applianceType)
        {
            if (ModelState.IsValid)
            {
                ApplianceType at = null;
                using (var dblocal = new NpgsqlContext())
                {
                    at = dblocal.ApplianceTypes.Where(a => a.Id == applianceType.Id).FirstOrDefault();
                    dblocal.Dispose();
                    GC.Collect();
                }

                string comment = $"Id: {at.Id} NameEN: {at.NameEN} NameKZ: {at.NameKZ} NameRU: {at.NameRU}";
                comment += $" -> NameEN: {applianceType.NameEN} NameKZ: {applianceType.NameKZ} NameRU: {applianceType.NameRU}";
                SystemLog.New("ApplianceTypeEdit", comment, null, false);

                db.Entry(applianceType).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(applianceType);
        }

        // GET: ApplianceTypes/Delete/5
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplianceType applianceType = await db.ApplianceTypes.FindAsync(id);
            if (applianceType == null)
            {
                return HttpNotFound();
            }
            return View(applianceType);
        }

        // POST: ApplianceTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ApplianceType applianceType = await db.ApplianceTypes.FindAsync(id);

            string comment = $"Id: {applianceType.Id} NameEN: {applianceType.NameEN} NameKZ: {applianceType.NameKZ} NameRU: {applianceType.NameRU}";
            SystemLog.New("ApplianceTypeDelete", comment, null, false);

            db.ApplianceTypes.Remove(applianceType);
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
