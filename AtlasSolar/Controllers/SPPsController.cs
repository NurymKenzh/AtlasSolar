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
using System.IO;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;

namespace AtlasSolar.Controllers
{
    public class SPPsController : Controller
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

        // GET: SPPs
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<ActionResult> Index(string SortOrder, string Name, int? Page)
        {
            var sPPs = db.SPPs.Include(s => s.PanelOrientation).Include(s => s.SPPPurpose).Include(s => s.SPPStatus);

            ViewBag.NameFilter = Name;

            ViewBag.NameSort = SortOrder == "Name" ? "NameDesc" : "Name";
            ViewBag.PowerSort = SortOrder == "Power" ? "PowerDesc" : "Power";
            ViewBag.StartupSort = SortOrder == "Startup" ? "StartupDesc" : "Startup";

            if (!string.IsNullOrEmpty(Name))
            {
                sPPs = sPPs.Where(s => s.Name.ToLower().Contains(Name.ToLower()));
            }

            switch (SortOrder)
            {
                case "Name":
                    sPPs = sPPs.OrderBy(s => s.Name);
                    break;
                case "NameDesc":
                    sPPs = sPPs.OrderByDescending(s => s.Name);
                    break;
                case "Power":
                    sPPs = sPPs.OrderBy(s => s.Power);
                    break;
                case "PowerDesc":
                    sPPs = sPPs.OrderByDescending(s => s.Power);
                    break;
                case "Startup":
                    sPPs = sPPs.OrderBy(s => s.Startup);
                    break;
                case "StartupDesc":
                    sPPs = sPPs.OrderByDescending(s => s.Startup);
                    break;
                default:
                    sPPs = sPPs.OrderBy(s => s.Id);
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

            return View(sPPs.ToPagedList(PageNumber, PageSize));
        }

        // GET: SPPs/Details/5
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SPP sPP = await db.SPPs
                .Where(s => s.Id == id)
                .Include(s => s.PanelOrientation)
                .Include(s => s.SPPPurpose)
                .Include(s => s.SPPStatus)
                .FirstOrDefaultAsync();
            if (sPP == null)
            {
                return HttpNotFound();
            }
            return View(sPP);
        }

        // GET: SPPs/Create
        [Authorize(Roles = "Admin,Moderator")]
        public ActionResult Create()
        {
            ViewBag.PanelOrientationId = new SelectList(db.PanelOrientations, "Id", "Name");
            ViewBag.SPPPurposeId = new SelectList(db.SPPPurposes, "Id", "Name");
            ViewBag.SPPStatusId = new SelectList(db.SPPStatus, "Id", "Name");
            return View();
        }

        // POST: SPPs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<ActionResult> Create([Bind(Include = "Id,Count,Power,Cost,Startup,Link,Customer,Investor,Executor,Name,CapacityFactor,SPPStatusId,SPPPurposeId,PanelOrientationId,Coordinates")] SPP sPP, HttpPostedFileBase Photo)
        {
            if (ModelState.IsValid)
            {
                if (Photo != null)
                {
                    MemoryStream target = new MemoryStream();
                    Photo.InputStream.CopyTo(target);
                    sPP.Photo = target.ToArray();
                }
                db.SPPs.Add(sPP);
                await db.SaveChangesAsync();

                string comment = $"Id: {sPP.Id}";
                comment += $" Count: {sPP.Count.ToString()}";
                comment += $" Power: {sPP.Power.ToString()}";
                comment += $" Cost: {sPP.Cost}";
                comment += $" Startup: {sPP.Startup.ToString()}";
                comment += $" Link: {sPP.Link}";
                comment += $" Customer: {sPP.Customer}";
                comment += $" Investor: {sPP.Investor}";
                comment += $" Executor: {sPP.Executor}";
                comment += $" CapacityFactor: {sPP.CapacityFactor.ToString()}";
                comment += $" SPPStatusId: {sPP.SPPStatusId.ToString()}";
                comment += $" SPPPurposeId: {sPP.SPPPurposeId.ToString()}";
                comment += $" PanelOrientationId: {sPP.PanelOrientationId.ToString()}";
                comment += $" Coordinates: {sPP.Coordinates}";
                SystemLog.New("SPPCreate", comment, null, false);

                return RedirectToAction("Index");
            }

            ViewBag.PanelOrientationId = new SelectList(db.PanelOrientations, "Id", "Name", sPP.PanelOrientationId);
            ViewBag.SPPPurposeId = new SelectList(db.SPPPurposes, "Id", "Name", sPP.SPPPurposeId);
            ViewBag.SPPStatusId = new SelectList(db.SPPStatus, "Id", "Name", sPP.SPPStatusId);
            return View(sPP);
        }

        // GET: SPPs/Edit/5
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SPP sPP = await db.SPPs.FindAsync(id);
            if (sPP == null)
            {
                return HttpNotFound();
            }
            ViewBag.PanelOrientationId = new SelectList(db.PanelOrientations, "Id", "Name", sPP.PanelOrientationId);
            ViewBag.SPPPurposeId = new SelectList(db.SPPPurposes, "Id", "Name", sPP.SPPPurposeId);
            ViewBag.SPPStatusId = new SelectList(db.SPPStatus, "Id", "Name", sPP.SPPStatusId);
            return View(sPP);
        }

        // POST: SPPs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Count,Power,Cost,Startup,Link,Customer,Investor,Executor,Name,CapacityFactor,SPPStatusId,SPPPurposeId,PanelOrientationId,Coordinates")] SPP sPP, HttpPostedFileBase Photo)
        {
            if (ModelState.IsValid)
            {
                SPP sPPdb = null;
                using (var dblocal = new NpgsqlContext())
                {
                    sPPdb = dblocal.SPPs.Find(sPP.Id);
                }

                sPP.Photo = sPPdb.Photo;
                if (Photo != null && Photo.ContentLength > 0)
                {
                    sPP.Photo = null;
                    MemoryStream target = new MemoryStream();
                    Photo.InputStream.CopyTo(target);
                    sPP.Photo = target.ToArray();
                }

                string comment = $"Id: {sPPdb.Id}";
                comment += $" Count: {sPPdb.Count.ToString()}";
                comment += $" Power: {sPPdb.Power.ToString()}";
                comment += $" Cost: {sPPdb.Cost}";
                comment += $" Startup: {sPPdb.Startup.ToString()}";
                comment += $" Link: {sPPdb.Link}";
                comment += $" Customer: {sPPdb.Customer}";
                comment += $" Investor: {sPPdb.Investor}";
                comment += $" Executor: {sPPdb.Executor}";
                comment += $" CapacityFactor: {sPPdb.CapacityFactor.ToString()}";
                comment += $" SPPStatusId: {sPPdb.SPPStatusId.ToString()}";
                comment += $" SPPPurposeId: {sPPdb.SPPPurposeId.ToString()}";
                comment += $" PanelOrientationId: {sPPdb.PanelOrientationId.ToString()}";
                comment += $" Coordinates: {sPPdb.Coordinates}";
                comment += $" -> Count: {sPP.Count.ToString()}";
                comment += $" Power: {sPP.Power.ToString()}";
                comment += $" Cost: {sPP.Cost}";
                comment += $" Startup: {sPP.Startup.ToString()}";
                comment += $" Link: {sPP.Link}";
                comment += $" Customer: {sPP.Customer}";
                comment += $" Investor: {sPP.Investor}";
                comment += $" Executor: {sPP.Executor}";
                comment += $" CapacityFactor: {sPP.CapacityFactor.ToString()}";
                comment += $" SPPStatusId: {sPP.SPPStatusId.ToString()}";
                comment += $" SPPPurposeId: {sPP.SPPPurposeId.ToString()}";
                comment += $" PanelOrientationId: {sPP.PanelOrientationId.ToString()}";
                comment += $" Coordinates: {sPP.Coordinates}";
                SystemLog.New("SPPEdit", comment, null, false);

                db.Entry(sPP).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }            

            ViewBag.PanelOrientationId = new SelectList(db.PanelOrientations, "Id", "Name", sPP.PanelOrientationId);
            ViewBag.SPPPurposeId = new SelectList(db.SPPPurposes, "Id", "Name", sPP.SPPPurposeId);
            ViewBag.SPPStatusId = new SelectList(db.SPPStatus, "Id", "Name", sPP.SPPStatusId);
            return View(sPP);
        }

        // GET: SPPs/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SPP sPP = await db.SPPs
                .Where(s => s.Id == id)
                .Include(s => s.PanelOrientation)
                .Include(s => s.SPPPurpose)
                .Include(s => s.SPPStatus)
                .FirstOrDefaultAsync();
            if (sPP == null)
            {
                return HttpNotFound();
            }
            return View(sPP);
        }

        // POST: SPPs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            SPP sPP = await db.SPPs.FindAsync(id);

            string comment = $"Id: {sPP.Id}";
            comment += $" Count: {sPP.Count.ToString()}";
            comment += $" Power: {sPP.Power.ToString()}";
            comment += $" Cost: {sPP.Cost}";
            comment += $" Startup: {sPP.Startup.ToString()}";
            comment += $" Link: {sPP.Link}";
            comment += $" Customer: {sPP.Customer}";
            comment += $" Investor: {sPP.Investor}";
            comment += $" Executor: {sPP.Executor}";
            comment += $" CapacityFactor: {sPP.CapacityFactor.ToString()}";
            comment += $" SPPStatusId: {sPP.SPPStatusId.ToString()}";
            comment += $" SPPPurposeId: {sPP.SPPPurposeId.ToString()}";
            comment += $" PanelOrientationId: {sPP.PanelOrientationId.ToString()}";
            comment += $" Coordinates: {sPP.Coordinates}";
            SystemLog.New("SPPCreate", comment, null, false);

            db.SPPs.Remove(sPP);
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
