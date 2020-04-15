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

namespace AtlasSolar.Controllers
{
    public class AppliancesController : Controller
    {
        private NpgsqlContext db = new NpgsqlContext();

        // GET: Appliances
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<ActionResult> Index(string SortOrder, int? ApplianceTypeId, int? Power, int? Page)
        {
            var appliances = db.Appliances
                .Where(a => true)
                .Include(a => a.ApplianceType);

            ViewBag.ApplianceTypeIdFilter = ApplianceTypeId;
            ViewBag.PowerFilter = Power;

            ViewBag.ApplianceTypeSort = SortOrder == "ApplianceType" ? "ApplianceTypeDesc" : "ApplianceType";
            ViewBag.PowerSort = SortOrder == "Power" ? "PowerDesc" : "Power";

            if (ApplianceTypeId != null)
            {
                appliances = appliances.Where(a => a.ApplianceTypeId == ApplianceTypeId);
            }
            if (Power != null)
            {
                appliances = appliances.Where(a => a.Power == Power);
            }

            switch (SortOrder)
            {
                case "ApplianceType":
                    appliances = appliances.OrderBy(a => a.ApplianceType.Name);
                    break;
                case "ApplianceTypeDesc":
                    appliances = appliances.OrderByDescending(a => a.ApplianceType.Name);
                    break;
                case "Power":
                    appliances = appliances.OrderBy(a => a.Power);
                    break;
                case "PowerDesc":
                    appliances = appliances.OrderByDescending(a => a.Power);
                    break;
                default:
                    appliances = appliances.OrderBy(a => a.Id);
                    break;
            }

            IList<ApplianceType> appliancetypes = db.ApplianceTypes
                .Where(a => true)
                .ToList();
            appliancetypes = appliancetypes
                .OrderBy(a => a.Name)
                .ToList();
            ViewBag.ApplianceTypes = new SelectList(appliancetypes, "Id", "Name");

            int PageSize = 50;
            int PageNumber = (Page ?? 1);

            return View(appliances.ToPagedList(PageNumber, PageSize));
        }

        // GET: Appliances/Details/5
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Appliance appliance = await db.Appliances
                .Where(a => a.Id == id)
                .Include(a => a.ApplianceType)
                .FirstOrDefaultAsync();
            if (appliance == null)
            {
                return HttpNotFound();
            }
            return View(appliance);
        }

        // GET: Appliances/Create
        [Authorize(Roles = "Admin,Moderator")]
        public ActionResult Create()
        {
            IList<ApplianceType> appliancetypes = db.ApplianceTypes
                .Where(a => true)
                .ToList();
            appliancetypes = appliancetypes
                .OrderBy(a => a.Name)
                .ToList();
            ViewBag.ApplianceTypeId = new SelectList(appliancetypes, "Id", "Name");
            return View();
        }

        // POST: Appliances/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<ActionResult> Create([Bind(Include = "Id,ApplianceTypeId,Power")] Appliance appliance)
        {
            if (ModelState.IsValid)
            {
                db.Appliances.Add(appliance);
                await db.SaveChangesAsync();

                string comment = $"Id: {appliance.Id} ApplianceTypeId: {appliance.ApplianceTypeId} Power: {appliance.Power.ToString()}";
                SystemLog.New("ApplianceCreate", comment, null, false);

                return RedirectToAction("Index");
            }

            IList<ApplianceType> appliancetypes = db.ApplianceTypes
                .Where(a => true)
                .ToList();
            appliancetypes = appliancetypes
                .OrderBy(a => a.Name)
                .ToList();
            ViewBag.ApplianceTypeId = new SelectList(appliancetypes, "Id", "Name", appliance.ApplianceTypeId);
            return View(appliance);
        }

        // GET: Appliances/Edit/5
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Appliance appliance = await db.Appliances.FindAsync(id);
            if (appliance == null)
            {
                return HttpNotFound();
            }
            IList<ApplianceType> appliancetypes = db.ApplianceTypes
                .Where(a => true)
                .ToList();
            appliancetypes = appliancetypes
                .OrderBy(a => a.Name)
                .ToList();
            ViewBag.ApplianceTypeId = new SelectList(appliancetypes, "Id", "Name", appliance.ApplianceTypeId);
            return View(appliance);
        }

        // POST: Appliances/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<ActionResult> Edit([Bind(Include = "Id,ApplianceTypeId,Power")] Appliance appliance)
        {
            if (ModelState.IsValid)
            {
                Appliance app = null;
                using (var dblocal = new NpgsqlContext())
                {
                    app = dblocal.Appliances.Where(a => a.Id == appliance.Id).FirstOrDefault();
                    dblocal.Dispose();
                    GC.Collect();
                }

                string comment = $"Id: {appliance.Id} ApplianceTypeId: {appliance.ApplianceTypeId} Power: {appliance.Power.ToString()}";
                comment += $" -> ApplianceTypeId: {app.ApplianceTypeId} Power: {app.Power.ToString()}";
                SystemLog.New("ApplianceEdit", comment, null, false);

                db.Entry(appliance).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            IList<ApplianceType> appliancetypes = db.ApplianceTypes
                .Where(a => true)
                .ToList();
            appliancetypes = appliancetypes
                .OrderBy(a => a.Name)
                .ToList();
            ViewBag.ApplianceTypeId = new SelectList(appliancetypes, "Id", "Name", appliance.ApplianceTypeId);
            return View(appliance);
        }

        // GET: Appliances/Delete/5
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Appliance appliance = await db.Appliances
                .Where(a => a.Id == id)
                .Include(a => a.ApplianceType)
                .FirstOrDefaultAsync();
            if (appliance == null)
            {
                return HttpNotFound();
            }
            return View(appliance);
        }

        // POST: Appliances/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Appliance appliance = await db.Appliances.FindAsync(id);

            string comment = $"Id: {appliance.Id} ApplianceTypeId: {appliance.ApplianceTypeId} Power: {appliance.Power.ToString()}";
            SystemLog.New("ApplianceDelete", comment, null, false);

            db.Appliances.Remove(appliance);
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
