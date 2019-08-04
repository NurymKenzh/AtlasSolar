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
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using PagedList;

namespace AtlasSolar.Controllers
{
    public class PVSystemMaterialsController : Controller
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

        // GET: PVSystemMaterials
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<ActionResult> Index(string SortOrder, string NameEN, string NameKZ, string NameRU, int? Page)
        {
            var pvsystemmaterials = db.PVSystemMaterials
                .Where(p => true);
            
            ViewBag.NameENFilter = NameEN;
            ViewBag.NameKZFilter = NameKZ;
            ViewBag.NameRUFilter = NameRU;
            
            ViewBag.NameENSort = SortOrder == "NameEN" ? "NameENDesc" : "NameEN";
            ViewBag.NameKZSort = SortOrder == "NameKZ" ? "NameKZDesc" : "NameKZ";
            ViewBag.NameRUSort = SortOrder == "NameRU" ? "NameRUDesc" : "NameRU";
            ViewBag.EfficiencySort = SortOrder == "Efficiency" ? "EfficiencyDesc" : "Efficiency";
            ViewBag.RatedOperatingTemperatureSort = SortOrder == "RatedOperatingTemperature" ? "RatedOperatingTemperatureDesc" : "RatedOperatingTemperature";
            ViewBag.ThermalPowerFactorSort = SortOrder == "ThermalPowerFactor" ? "ThermalPowerFactorDesc" : "ThermalPowerFactor";
            
            if (!string.IsNullOrEmpty(NameEN))
            {
                pvsystemmaterials = pvsystemmaterials.Where(p => p.NameEN.ToLower().Contains(NameEN.ToLower()));
            }
            if (!string.IsNullOrEmpty(NameKZ))
            {
                pvsystemmaterials = pvsystemmaterials.Where(p => p.NameKZ.ToLower().Contains(NameKZ.ToLower()));
            }
            if (!string.IsNullOrEmpty(NameRU))
            {
                pvsystemmaterials = pvsystemmaterials.Where(p => p.NameRU.ToLower().Contains(NameRU.ToLower()));
            }

            switch (SortOrder)
            {
                case "NameEN":
                    pvsystemmaterials = pvsystemmaterials.OrderBy(p => p.NameEN);
                    break;
                case "NameENDesc":
                    pvsystemmaterials = pvsystemmaterials.OrderByDescending(p => p.NameEN);
                    break;
                case "NameKZ":
                    pvsystemmaterials = pvsystemmaterials.OrderBy(p => p.NameKZ);
                    break;
                case "NameKZDesc":
                    pvsystemmaterials = pvsystemmaterials.OrderByDescending(p => p.NameKZ);
                    break;
                case "NameRU":
                    pvsystemmaterials = pvsystemmaterials.OrderBy(p => p.NameRU);
                    break;
                case "NameRUDesc":
                    pvsystemmaterials = pvsystemmaterials.OrderByDescending(p => p.NameRU);
                    break;
                case "Efficiency":
                    pvsystemmaterials = pvsystemmaterials.OrderBy(p => p.Efficiency);
                    break;
                case "EfficiencyDesc":
                    pvsystemmaterials = pvsystemmaterials.OrderByDescending(p => p.Efficiency);
                    break;
                case "RatedOperatingTemperature":
                    pvsystemmaterials = pvsystemmaterials.OrderBy(p => p.RatedOperatingTemperature);
                    break;
                case "RatedOperatingTemperatureDesc":
                    pvsystemmaterials = pvsystemmaterials.OrderByDescending(p => p.RatedOperatingTemperature);
                    break;
                case "ThermalPowerFactor":
                    pvsystemmaterials = pvsystemmaterials.OrderBy(p => p.ThermalPowerFactor);
                    break;
                case "ThermalPowerFactorDesc":
                    pvsystemmaterials = pvsystemmaterials.OrderByDescending(p => p.ThermalPowerFactor);
                    break;
                default:
                    pvsystemmaterials = pvsystemmaterials.OrderBy(p => p.Id);
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

            return View(pvsystemmaterials.ToPagedList(PageNumber, PageSize));
        }

        // GET: PVSystemMaterials/Details/5
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PVSystemMaterial pVSystemMaterial = await db.PVSystemMaterials.FindAsync(id);
            if (pVSystemMaterial == null)
            {
                return HttpNotFound();
            }

            ViewBag.Role = "";
            string CurrentUserId = User.Identity.GetUserId();
            if (UserManager.IsInRole(CurrentUserId, "Admin"))
            {
                ViewBag.Role = "Admin";
            }

            return View(pVSystemMaterial);
        }

        // GET: PVSystemMaterials/Create
        [Authorize(Roles = "Admin,Moderator")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: PVSystemMaterials/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<ActionResult> Create([Bind(Include = "Id,NameEN,NameKZ,NameRU,Efficiency,RatedOperatingTemperature,ThermalPowerFactor")] PVSystemMaterial pVSystemMaterial)
        {
            if (ModelState.IsValid)
            {
                db.PVSystemMaterials.Add(pVSystemMaterial);
                await db.SaveChangesAsync();

                string comment = $"Id: {pVSystemMaterial.Id} NameEN: {pVSystemMaterial.NameEN} NameKZ: {pVSystemMaterial.NameKZ} NameRU: {pVSystemMaterial.NameRU} Efficiency: {pVSystemMaterial.Efficiency} RatedOperatingTemperature: {pVSystemMaterial.RatedOperatingTemperature} ThermalPowerFactor: {pVSystemMaterial.ThermalPowerFactor}";
                SystemLog.New("PVSystemMaterialCreate", comment, null, false);

                return RedirectToAction("Index");
            }

            return View(pVSystemMaterial);
        }

        // GET: PVSystemMaterials/Edit/5
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PVSystemMaterial pVSystemMaterial = await db.PVSystemMaterials.FindAsync(id);
            if (pVSystemMaterial == null)
            {
                return HttpNotFound();
            }
            return View(pVSystemMaterial);
        }

        // POST: PVSystemMaterials/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<ActionResult> Edit([Bind(Include = "Id,NameEN,NameKZ,NameRU,Efficiency,RatedOperatingTemperature,ThermalPowerFactor")] PVSystemMaterial pVSystemMaterial)
        {
            if (ModelState.IsValid)
            {
                PVSystemMaterial pvsm = null;
                using (var dblocal = new NpgsqlContext())
                {
                    pvsm = dblocal.PVSystemMaterials.Where(p => p.Id == pVSystemMaterial.Id).FirstOrDefault();
                    dblocal.Dispose();
                    GC.Collect();
                }

                db.Entry(pVSystemMaterial).State = EntityState.Modified;
                await db.SaveChangesAsync();

                string comment = $"Id: {pvsm.Id} NameEN: {pvsm.NameEN} NameKZ: {pvsm.NameKZ} NameRU: {pvsm.NameRU} Efficiency: {pvsm.Efficiency} RatedOperatingTemperature: {pvsm.RatedOperatingTemperature} ThermalPowerFactor: {pvsm.ThermalPowerFactor}";
                comment += $"  -> Id: {pVSystemMaterial.Id} NameEN: {pVSystemMaterial.NameEN} NameKZ: {pVSystemMaterial.NameKZ} NameRU: {pVSystemMaterial.NameRU} Efficiency: {pVSystemMaterial.Efficiency} RatedOperatingTemperature: {pVSystemMaterial.RatedOperatingTemperature} ThermalPowerFactor: {pVSystemMaterial.ThermalPowerFactor}";

                SystemLog.New("PVSystemMaterialEdit", comment, null, false);

                return RedirectToAction("Index");
            }
            return View(pVSystemMaterial);
        }

        // GET: PVSystemMaterials/Delete/5
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PVSystemMaterial pVSystemMaterial = await db.PVSystemMaterials.FindAsync(id);
            if (pVSystemMaterial == null)
            {
                return HttpNotFound();
            }
            return View(pVSystemMaterial);
        }

        // POST: PVSystemMaterials/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            PVSystemMaterial pVSystemMaterial = await db.PVSystemMaterials.FindAsync(id);

            string comment = $"Id: {pVSystemMaterial.Id} NameEN: {pVSystemMaterial.NameEN} NameKZ: {pVSystemMaterial.NameKZ} NameRU: {pVSystemMaterial.NameRU} Efficiency: {pVSystemMaterial.Efficiency} RatedOperatingTemperature: {pVSystemMaterial.RatedOperatingTemperature} ThermalPowerFactor: {pVSystemMaterial.ThermalPowerFactor}";
            SystemLog.New("PVSystemMaterialDelete", comment, null, false);

            db.PVSystemMaterials.Remove(pVSystemMaterial);
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
