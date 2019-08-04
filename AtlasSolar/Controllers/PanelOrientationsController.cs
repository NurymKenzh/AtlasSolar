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
    public class PanelOrientationsController : Controller
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

        // GET: PanelOrientations
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<ActionResult> Index(string SortOrder, string Code, string NameEN, string NameKZ, string NameRU, int? Page)
        {
            var panelorientations = db.PanelOrientations
                .Where(p => true);

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
                panelorientations = panelorientations.Where(p => p.Code == Code);
            }
            if (!string.IsNullOrEmpty(NameEN))
            {
                panelorientations = panelorientations.Where(p => p.NameEN.ToLower().Contains(NameEN.ToLower()));
            }
            if (!string.IsNullOrEmpty(NameKZ))
            {
                panelorientations = panelorientations.Where(p => p.NameKZ.ToLower().Contains(NameKZ.ToLower()));
            }
            if (!string.IsNullOrEmpty(NameRU))
            {
                panelorientations = panelorientations.Where(p => p.NameRU.ToLower().Contains(NameRU.ToLower()));
            }

            switch (SortOrder)
            {
                case "Code":
                    panelorientations = panelorientations.OrderBy(p => p.Code);
                    break;
                case "CodeDesc":
                    panelorientations = panelorientations.OrderByDescending(p => p.Code);
                    break;
                case "NameEN":
                    panelorientations = panelorientations.OrderBy(p => p.NameEN);
                    break;
                case "NameENDesc":
                    panelorientations = panelorientations.OrderByDescending(p => p.NameEN);
                    break;
                case "NameKZ":
                    panelorientations = panelorientations.OrderBy(p => p.NameKZ);
                    break;
                case "NameKZDesc":
                    panelorientations = panelorientations.OrderByDescending(p => p.NameKZ);
                    break;
                case "NameRU":
                    panelorientations = panelorientations.OrderBy(p => p.NameRU);
                    break;
                case "NameRUDesc":
                    panelorientations = panelorientations.OrderByDescending(p => p.NameRU);
                    break;
                default:
                    panelorientations = panelorientations.OrderBy(p => p.Id);
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

            return View(panelorientations.ToPagedList(PageNumber, PageSize));
        }

        // GET: PanelOrientations/Details/5
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PanelOrientation panelOrientation = await db.PanelOrientations.FindAsync(id);
            if (panelOrientation == null)
            {
                return HttpNotFound();
            }

            return View(panelOrientation);
        }

        // GET: PanelOrientations/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: PanelOrientations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Create([Bind(Include = "Id,Code,NameEN,NameKZ,NameRU")] PanelOrientation panelOrientation)
        {
            if (ModelState.IsValid)
            {
                db.PanelOrientations.Add(panelOrientation);
                await db.SaveChangesAsync();

                string comment = $"Id: {panelOrientation.Id} Code: {panelOrientation.Code} NameEN: {panelOrientation.NameEN} NameKZ: {panelOrientation.NameKZ} NameRU: {panelOrientation.NameRU}";
                SystemLog.New("PanelOrientationCreate", comment, null, false);

                return RedirectToAction("Index");
            }

            return View(panelOrientation);
        }

        // GET: PanelOrientations/Edit/5
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PanelOrientation panelOrientation = await db.PanelOrientations.FindAsync(id);
            if (panelOrientation == null)
            {
                return HttpNotFound();
            }
            return View(panelOrientation);
        }

        // POST: PanelOrientations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Code,NameEN,NameKZ,NameRU")] PanelOrientation panelOrientation)
        {
            if (ModelState.IsValid)
            {
                PanelOrientation po = null;
                using (var dblocal = new NpgsqlContext())
                {
                    po = dblocal.PanelOrientations.Where(p => p.Id == panelOrientation.Id).FirstOrDefault();
                    dblocal.Dispose();
                    GC.Collect();
                }

                panelOrientation.Code = po.Code;

                string comment = $"Id: {po.Id} Code: {po.Code} NameEN: {po.NameEN} NameKZ: {po.NameKZ} NameRU: {po.NameRU}";
                comment += $" -> Code: {panelOrientation.Code} NameEN: {panelOrientation.NameEN} NameKZ: {panelOrientation.NameKZ} NameRU: {panelOrientation.NameRU}";
                SystemLog.New("PanelOrientationEdit", comment, null, false);

                db.Entry(panelOrientation).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(panelOrientation);
        }

        // GET: PanelOrientations/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PanelOrientation panelOrientation = await db.PanelOrientations.FindAsync(id);
            if (panelOrientation == null)
            {
                return HttpNotFound();
            }
            return View(panelOrientation);
        }

        // POST: PanelOrientations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            PanelOrientation panelOrientation = await db.PanelOrientations.FindAsync(id);

            string comment = $"Id: {panelOrientation.Id} Code: {panelOrientation.Code} NameEN: {panelOrientation.NameEN} NameKZ: {panelOrientation.NameKZ} NameRU: {panelOrientation.NameRU}";
            SystemLog.New("PanelOrientationDelete", comment, null, false);

            db.PanelOrientations.Remove(panelOrientation);
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
