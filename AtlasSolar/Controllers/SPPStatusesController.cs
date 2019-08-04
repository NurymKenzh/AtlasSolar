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
    public class SPPStatusesController : Controller
    {
        private NpgsqlContext db = new NpgsqlContext();

        // GET: SPPStatuses
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Index(string SortOrder, string NameEN, string NameKZ, string NameRU, int? Page)
        {
            var sppstatuses = db.SPPStatus
                .Where(m => true);
            
            ViewBag.NameENFilter = NameEN;
            ViewBag.NameKZFilter = NameKZ;
            ViewBag.NameRUFilter = NameRU;
            
            ViewBag.NameENSort = SortOrder == "NameEN" ? "NameENDesc" : "NameEN";
            ViewBag.NameKZSort = SortOrder == "NameKZ" ? "NameKZDesc" : "NameKZ";
            ViewBag.NameRUSort = SortOrder == "NameRU" ? "NameRUDesc" : "NameRU";

            if (!string.IsNullOrEmpty(NameEN))
            {
                sppstatuses = sppstatuses.Where(s => s.NameEN.ToLower().Contains(NameEN.ToLower()));
            }
            if (!string.IsNullOrEmpty(NameKZ))
            {
                sppstatuses = sppstatuses.Where(s => s.NameKZ.ToLower().Contains(NameKZ.ToLower()));
            }
            if (!string.IsNullOrEmpty(NameRU))
            {
                sppstatuses = sppstatuses.Where(s => s.NameRU.ToLower().Contains(NameRU.ToLower()));
            }

            switch (SortOrder)
            {
                case "NameEN":
                    sppstatuses = sppstatuses.OrderBy(s => s.NameEN);
                    break;
                case "NameENDesc":
                    sppstatuses = sppstatuses.OrderByDescending(s => s.NameEN);
                    break;
                case "NameKZ":
                    sppstatuses = sppstatuses.OrderBy(s => s.NameKZ);
                    break;
                case "NameKZDesc":
                    sppstatuses = sppstatuses.OrderByDescending(s => s.NameKZ);
                    break;
                case "NameRU":
                    sppstatuses = sppstatuses.OrderBy(s => s.NameRU);
                    break;
                case "NameRUDesc":
                    sppstatuses = sppstatuses.OrderByDescending(s => s.NameRU);
                    break;
                default:
                    sppstatuses = sppstatuses.OrderBy(s => s.Id);
                    break;
            }

            int PageSize = 50;
            int PageNumber = (Page ?? 1);

            return View(sppstatuses.ToPagedList(PageNumber, PageSize));
        }

        // GET: SPPStatuses/Details/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SPPStatus sPPStatus = await db.SPPStatus.FindAsync(id);
            if (sPPStatus == null)
            {
                return HttpNotFound();
            }
            return View(sPPStatus);
        }

        // GET: SPPStatuses/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: SPPStatuses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Create([Bind(Include = "Id,NameEN,NameKZ,NameRU")] SPPStatus sPPStatus)
        {
            if (ModelState.IsValid)
            {
                db.SPPStatus.Add(sPPStatus);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(sPPStatus);
        }

        // GET: SPPStatuses/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SPPStatus sPPStatus = await db.SPPStatus.FindAsync(id);
            if (sPPStatus == null)
            {
                return HttpNotFound();
            }
            return View(sPPStatus);
        }

        // POST: SPPStatuses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit([Bind(Include = "Id,NameEN,NameKZ,NameRU")] SPPStatus sPPStatus)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sPPStatus).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(sPPStatus);
        }

        // GET: SPPStatuses/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SPPStatus sPPStatus = await db.SPPStatus.FindAsync(id);
            if (sPPStatus == null)
            {
                return HttpNotFound();
            }
            return View(sPPStatus);
        }

        // POST: SPPStatuses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            SPPStatus sPPStatus = await db.SPPStatus.FindAsync(id);
            db.SPPStatus.Remove(sPPStatus);
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
