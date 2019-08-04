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
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;

namespace AtlasSolar.Controllers
{
    public class MapDescriptionsController : Controller
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

        // GET: MapDescriptions
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<ActionResult> Index(string LayersCode, string DescriptionForUser, int? Page)
        {
            var mapdescriptions = db.MapDescriptions
                .Where(m => true);

            ViewBag.LayersCodeFilter = LayersCode;
            ViewBag.DescriptionForUserFilter = DescriptionForUser;

            if (!string.IsNullOrEmpty(LayersCode))
            {
                mapdescriptions = mapdescriptions.Where(m => m.LayersCode.ToLower().Contains(LayersCode.ToLower()));
            }
            if (!string.IsNullOrEmpty(DescriptionForUser))
            {
                mapdescriptions = mapdescriptions.Where(m => m.DescriptionForUser.ToLower().Contains(DescriptionForUser.ToLower()));
            }

            ViewBag.Role = "";
            string CurrentUserId = User.Identity.GetUserId();
            if (UserManager.IsInRole(CurrentUserId, "Admin"))
            {
                ViewBag.Role = "Admin";
            }

            int PageSize = 50;
            int PageNumber = (Page ?? 1);

            return View(mapdescriptions
                .OrderBy(m => m.Id)
                .ToPagedList(PageNumber, PageSize));
        }

        // GET: MapDescriptions/Details/5
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MapDescription mapDescription = await db.MapDescriptions.FindAsync(id);
            if (mapDescription == null)
            {
                return HttpNotFound();
            }

            return View(mapDescription);
        }

        // GET: MapDescriptions/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: MapDescriptions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Create([Bind(Include = "Id,LayersCode,DescriptionForUser,NameEN,NameKZ,NameRU,DescriptionEN,DescriptionKZ,DescriptionRU,AppointmentEN,AppointmentKZ,AppointmentRU,SourceEN,SourceKZ,SourceRU,AdditionalEN,AdditionalKZ,AdditionalRU,ResolutionEN,ResolutionKZ,ResolutionRU")] MapDescription mapDescription)
        {
            if (ModelState.IsValid)
            {
                db.MapDescriptions.Add(mapDescription);
                await db.SaveChangesAsync();

                string comment = $"Id: {mapDescription.Id} LayersCode: {mapDescription.LayersCode} DescriptionForUser: {mapDescription.DescriptionForUser}";
                comment += $" NameEN: {mapDescription.NameEN} NameKZ: {mapDescription.NameKZ} NameRU: {mapDescription.NameRU}";
                comment += $" DescriptionEN: {mapDescription.DescriptionEN} DescriptionKZ: {mapDescription.DescriptionKZ} DescriptionRU: {mapDescription.DescriptionRU}";
                comment += $" AppointmentEN: {mapDescription.AppointmentEN} AppointmentKZ: {mapDescription.AppointmentKZ} AppointmentRU: {mapDescription.AppointmentRU}";
                comment += $" SourceEN: {mapDescription.SourceEN} SourceKZ: {mapDescription.SourceKZ} SourceRU: {mapDescription.SourceRU}";
                comment += $" AdditionalEN: {mapDescription.AdditionalEN} AdditionalKZ: {mapDescription.AdditionalKZ} AdditionalRU: {mapDescription.AdditionalRU}";
                comment += $" ResolutionEN: {mapDescription.ResolutionEN} ResolutionKZ: {mapDescription.ResolutionKZ} ResolutionRU: {mapDescription.ResolutionRU}";
                SystemLog.New("MapDescriptionCreate", comment, null, false);

                return RedirectToAction("Index");
            }

            return View(mapDescription);
        }

        // GET: MapDescriptions/Edit/5
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MapDescription mapDescription = await db.MapDescriptions.FindAsync(id);
            if (mapDescription == null)
            {
                return HttpNotFound();
            }
            return View(mapDescription);
        }

        // POST: MapDescriptions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<ActionResult> Edit([Bind(Include = "Id,LayersCode,DescriptionForUser,NameEN,NameKZ,NameRU,DescriptionEN,DescriptionKZ,DescriptionRU,AppointmentEN,AppointmentKZ,AppointmentRU,SourceEN,SourceKZ,SourceRU,AdditionalEN,AdditionalKZ,AdditionalRU,ResolutionEN,ResolutionKZ,ResolutionRU")] MapDescription mapDescription)
        {
            if (ModelState.IsValid)
            {
                MapDescription md = null;
                using (var dblocal = new NpgsqlContext())
                {
                    md = dblocal.MapDescriptions.Where(m => m.Id == mapDescription.Id).FirstOrDefault();
                    dblocal.Dispose();
                    GC.Collect();
                }

                mapDescription.LayersCode = md.LayersCode;

                db.Entry(mapDescription).State = EntityState.Modified;
                await db.SaveChangesAsync();

                string comment = $"Id: {md.Id} LayersCode: {md.LayersCode} DescriptionForUser: {md.DescriptionForUser}";
                comment += $" NameEN: {md.NameEN} NameKZ: {md.NameKZ} NameRU: {md.NameRU}";
                comment += $" DescriptionEN: {md.DescriptionEN} DescriptionKZ: {md.DescriptionKZ} DescriptionRU: {md.DescriptionRU}";
                comment += $" AppointmentEN: {md.AppointmentEN} AppointmentKZ: {md.AppointmentKZ} AppointmentRU: {md.AppointmentRU}";
                comment += $" SourceEN: {md.SourceEN} SourceKZ: {md.SourceKZ} SourceRU: {md.SourceRU}";
                comment += $" AdditionalEN: {md.AdditionalEN} AdditionalKZ: {md.AdditionalKZ} AdditionalRU: {md.AdditionalRU}";
                comment += $" ResolutionEN: {md.ResolutionEN} ResolutionKZ: {md.ResolutionKZ} ResolutionRU: {md.ResolutionRU}";
                comment = $" -> Id: {mapDescription.Id} LayersCode: {mapDescription.LayersCode} DescriptionForUser: {mapDescription.DescriptionForUser}";
                comment += $" NameEN: {mapDescription.NameEN} NameKZ: {mapDescription.NameKZ} NameRU: {mapDescription.NameRU}";
                comment += $" DescriptionEN: {mapDescription.DescriptionEN} DescriptionKZ: {mapDescription.DescriptionKZ} DescriptionRU: {mapDescription.DescriptionRU}";
                comment += $" AppointmentEN: {mapDescription.AppointmentEN} AppointmentKZ: {mapDescription.AppointmentKZ} AppointmentRU: {mapDescription.AppointmentRU}";
                comment += $" SourceEN: {mapDescription.SourceEN} SourceKZ: {mapDescription.SourceKZ} SourceRU: {mapDescription.SourceRU}";
                comment += $" AdditionalEN: {mapDescription.AdditionalEN} AdditionalKZ: {mapDescription.AdditionalKZ} AdditionalRU: {mapDescription.AdditionalRU}";
                comment += $" ResolutionEN: {mapDescription.ResolutionEN} ResolutionKZ: {mapDescription.ResolutionKZ} ResolutionRU: {mapDescription.ResolutionRU}";

                SystemLog.New("MapDescriptionEdit", comment, null, false);

                return RedirectToAction("Index");
            }
            return View(mapDescription);
        }

        // GET: MapDescriptions/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MapDescription mapDescription = await db.MapDescriptions.FindAsync(id);
            if (mapDescription == null)
            {
                return HttpNotFound();
            }
            return View(mapDescription);
        }

        // POST: MapDescriptions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            MapDescription mapDescription = await db.MapDescriptions.FindAsync(id);

            string comment = $"Id: {mapDescription.Id} LayersCode: {mapDescription.LayersCode} DescriptionForUser: {mapDescription.DescriptionForUser}";
            comment += $" NameEN: {mapDescription.NameEN} NameKZ: {mapDescription.NameKZ} NameRU: {mapDescription.NameRU}";
            comment += $" DescriptionEN: {mapDescription.DescriptionEN} DescriptionKZ: {mapDescription.DescriptionKZ} DescriptionRU: {mapDescription.DescriptionRU}";
            comment += $" AppointmentEN: {mapDescription.AppointmentEN} AppointmentKZ: {mapDescription.AppointmentKZ} AppointmentRU: {mapDescription.AppointmentRU}";
            comment += $" SourceEN: {mapDescription.SourceEN} SourceKZ: {mapDescription.SourceKZ} SourceRU: {mapDescription.SourceRU}";
            comment += $" AdditionalEN: {mapDescription.AdditionalEN} AdditionalKZ: {mapDescription.AdditionalKZ} AdditionalRU: {mapDescription.AdditionalRU}";
            comment += $" ResolutionEN: {mapDescription.ResolutionEN} ResolutionKZ: {mapDescription.ResolutionKZ} ResolutionRU: {mapDescription.ResolutionRU}";
            SystemLog.New("MapDescriptionDelete", comment, null, false);

            db.MapDescriptions.Remove(mapDescription);
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
