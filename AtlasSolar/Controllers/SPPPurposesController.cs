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
    public class SPPPurposesController : Controller
    {
        private NpgsqlContext db = new NpgsqlContext();

        // GET: SPPPurposes
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Index(string SortOrder, string NameEN, string NameKZ, string NameRU, int? Page)
        {
            var spppurposes = db.SPPPurposes
                .Where(m => true);

            ViewBag.NameENFilter = NameEN;
            ViewBag.NameKZFilter = NameKZ;
            ViewBag.NameRUFilter = NameRU;

            ViewBag.NameENSort = SortOrder == "NameEN" ? "NameENDesc" : "NameEN";
            ViewBag.NameKZSort = SortOrder == "NameKZ" ? "NameKZDesc" : "NameKZ";
            ViewBag.NameRUSort = SortOrder == "NameRU" ? "NameRUDesc" : "NameRU";

            if (!string.IsNullOrEmpty(NameEN))
            {
                spppurposes = spppurposes.Where(s => s.NameEN.ToLower().Contains(NameEN.ToLower()));
            }
            if (!string.IsNullOrEmpty(NameKZ))
            {
                spppurposes = spppurposes.Where(s => s.NameKZ.ToLower().Contains(NameKZ.ToLower()));
            }
            if (!string.IsNullOrEmpty(NameRU))
            {
                spppurposes = spppurposes.Where(s => s.NameRU.ToLower().Contains(NameRU.ToLower()));
            }

            switch (SortOrder)
            {
                case "NameEN":
                    spppurposes = spppurposes.OrderBy(s => s.NameEN);
                    break;
                case "NameENDesc":
                    spppurposes = spppurposes.OrderByDescending(s => s.NameEN);
                    break;
                case "NameKZ":
                    spppurposes = spppurposes.OrderBy(s => s.NameKZ);
                    break;
                case "NameKZDesc":
                    spppurposes = spppurposes.OrderByDescending(s => s.NameKZ);
                    break;
                case "NameRU":
                    spppurposes = spppurposes.OrderBy(s => s.NameRU);
                    break;
                case "NameRUDesc":
                    spppurposes = spppurposes.OrderByDescending(s => s.NameRU);
                    break;
                default:
                    spppurposes = spppurposes.OrderBy(s => s.Id);
                    break;
            }

            int PageSize = 50;
            int PageNumber = (Page ?? 1);

            return View(spppurposes.ToPagedList(PageNumber, PageSize));
        }

        // GET: SPPPurposes/Details/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SPPPurpose sPPPurpose = await db.SPPPurposes.FindAsync(id);
            if (sPPPurpose == null)
            {
                return HttpNotFound();
            }
            return View(sPPPurpose);
        }

        // GET: SPPPurposes/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: SPPPurposes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Create([Bind(Include = "Id,NameEN,NameKZ,NameRU")] SPPPurpose sPPPurpose)
        {
            if (ModelState.IsValid)
            {
                db.SPPPurposes.Add(sPPPurpose);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(sPPPurpose);
        }

        // GET: SPPPurposes/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SPPPurpose sPPPurpose = await db.SPPPurposes.FindAsync(id);
            if (sPPPurpose == null)
            {
                return HttpNotFound();
            }
            return View(sPPPurpose);
        }

        // POST: SPPPurposes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit([Bind(Include = "Id,NameEN,NameKZ,NameRU")] SPPPurpose sPPPurpose)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sPPPurpose).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(sPPPurpose);
        }

        // GET: SPPPurposes/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SPPPurpose sPPPurpose = await db.SPPPurposes.FindAsync(id);
            if (sPPPurpose == null)
            {
                return HttpNotFound();
            }
            return View(sPPPurpose);
        }

        // POST: SPPPurposes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            SPPPurpose sPPPurpose = await db.SPPPurposes.FindAsync(id);
            db.SPPPurposes.Remove(sPPPurpose);
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
