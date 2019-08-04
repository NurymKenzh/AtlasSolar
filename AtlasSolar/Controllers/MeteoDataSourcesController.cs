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
    public class MeteoDataSourcesController : Controller
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

        // GET: MeteoDataSources
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<ActionResult> Index(string SortOrder, string Code, string NameEN, string NameKZ, string NameRU, int? Page)
        {
            var meteodatasources = db.MeteoDataSources
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
                meteodatasources = meteodatasources.Where(m => m.Code == Code);
            }
            if (!string.IsNullOrEmpty(NameEN))
            {
                meteodatasources = meteodatasources.Where(m => m.NameEN.ToLower().Contains(NameEN.ToLower()));
            }
            if (!string.IsNullOrEmpty(NameKZ))
            {
                meteodatasources = meteodatasources.Where(m => m.NameKZ.ToLower().Contains(NameKZ.ToLower()));
            }
            if (!string.IsNullOrEmpty(NameRU))
            {
                meteodatasources = meteodatasources.Where(m => m.NameRU.ToLower().Contains(NameRU.ToLower()));
            }

            switch (SortOrder)
            {
                case "Code":
                    meteodatasources = meteodatasources.OrderBy(m => m.Code);
                    break;
                case "CodeDesc":
                    meteodatasources = meteodatasources.OrderByDescending(m => m.Code);
                    break;
                case "NameEN":
                    meteodatasources = meteodatasources.OrderBy(m => m.NameEN);
                    break;
                case "NameENDesc":
                    meteodatasources = meteodatasources.OrderByDescending(m => m.NameEN);
                    break;
                case "NameKZ":
                    meteodatasources = meteodatasources.OrderBy(m => m.NameKZ);
                    break;
                case "NameKZDesc":
                    meteodatasources = meteodatasources.OrderByDescending(m => m.NameKZ);
                    break;
                case "NameRU":
                    meteodatasources = meteodatasources.OrderBy(m => m.NameRU);
                    break;
                case "NameRUDesc":
                    meteodatasources = meteodatasources.OrderByDescending(m => m.NameRU);
                    break;
                default:
                    meteodatasources = meteodatasources.OrderBy(m => m.Id);
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

            return View(meteodatasources.ToPagedList(PageNumber, PageSize));
        }

        // GET: MeteoDataSources/Details/5
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MeteoDataSource meteoDataSource = await db.MeteoDataSources.FindAsync(id);
            if (meteoDataSource == null)
            {
                return HttpNotFound();
            }

            ViewBag.Role = "";
            string CurrentUserId = User.Identity.GetUserId();
            if (UserManager.IsInRole(CurrentUserId, "Admin"))
            {
                ViewBag.Role = "Admin";
            }

            return View(meteoDataSource);
        }

        // GET: MeteoDataSources/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: MeteoDataSources/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Create([Bind(Include = "Id,Code,NameEN,NameKZ,NameRU")] MeteoDataSource meteoDataSource)
        {
            if (ModelState.IsValid)
            {
                db.MeteoDataSources.Add(meteoDataSource);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(meteoDataSource);
        }

        // GET: MeteoDataSources/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MeteoDataSource meteoDataSource = await db.MeteoDataSources.FindAsync(id);
            if (meteoDataSource == null)
            {
                return HttpNotFound();
            }
            return View(meteoDataSource);
        }

        // POST: MeteoDataSources/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Code,NameEN,NameKZ,NameRU")] MeteoDataSource meteoDataSource)
        {
            if (ModelState.IsValid)
            {
                db.Entry(meteoDataSource).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(meteoDataSource);
        }

        // GET: MeteoDataSources/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MeteoDataSource meteoDataSource = await db.MeteoDataSources.FindAsync(id);
            if (meteoDataSource == null)
            {
                return HttpNotFound();
            }
            return View(meteoDataSource);
        }

        // POST: MeteoDataSources/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            MeteoDataSource meteoDataSource = await db.MeteoDataSources.FindAsync(id);
            db.MeteoDataSources.Remove(meteoDataSource);
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
