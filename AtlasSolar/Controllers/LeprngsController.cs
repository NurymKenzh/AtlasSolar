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
    public class LeprngsController : Controller
    {
        private NpgsqlContext db = new NpgsqlContext();

        // GET: Leprngs
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Index(string SortOrder, string Code, string NameEN, string NameKZ, string NameRU, int? Page)
        {
            var leprngs = db.Leprngs
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
                leprngs = leprngs.Where(m => m.Code == Code);
            }
            if (!string.IsNullOrEmpty(NameEN))
            {
                leprngs = leprngs.Where(m => m.NameEN.ToLower().Contains(NameEN.ToLower()));
            }
            if (!string.IsNullOrEmpty(NameKZ))
            {
                leprngs = leprngs.Where(m => m.NameKZ.ToLower().Contains(NameKZ.ToLower()));
            }
            if (!string.IsNullOrEmpty(NameRU))
            {
                leprngs = leprngs.Where(m => m.NameRU.ToLower().Contains(NameRU.ToLower()));
            }

            switch (SortOrder)
            {
                case "Code":
                    leprngs = leprngs.OrderBy(m => m.Code);
                    break;
                case "CodeDesc":
                    leprngs = leprngs.OrderByDescending(m => m.Code);
                    break;
                case "NameEN":
                    leprngs = leprngs.OrderBy(m => m.NameEN);
                    break;
                case "NameENDesc":
                    leprngs = leprngs.OrderByDescending(m => m.NameEN);
                    break;
                case "NameKZ":
                    leprngs = leprngs.OrderBy(m => m.NameKZ);
                    break;
                case "NameKZDesc":
                    leprngs = leprngs.OrderByDescending(m => m.NameKZ);
                    break;
                case "NameRU":
                    leprngs = leprngs.OrderBy(m => m.NameRU);
                    break;
                case "NameRUDesc":
                    leprngs = leprngs.OrderByDescending(m => m.NameRU);
                    break;
                default:
                    leprngs = leprngs.OrderBy(m => m.Id);
                    break;
            }

            int PageSize = 50;
            int PageNumber = (Page ?? 1);

            return View(leprngs.ToPagedList(PageNumber, PageSize));
        }

        // GET: Leprngs/Details/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Leprng leprng = await db.Leprngs.FindAsync(id);
            if (leprng == null)
            {
                return HttpNotFound();
            }

            return View(leprng);
        }

        // GET: Leprngs/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Leprngs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Create([Bind(Include = "Id,Code,NameEN,NameKZ,NameRU")] Leprng leprng)
        {
            if (ModelState.IsValid)
            {
                db.Leprngs.Add(leprng);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(leprng);
        }

        // GET: Leprngs/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Leprng leprng = await db.Leprngs.FindAsync(id);
            if (leprng == null)
            {
                return HttpNotFound();
            }
            return View(leprng);
        }

        // POST: Leprngs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Code,NameEN,NameKZ,NameRU")] Leprng leprng)
        {
            if (ModelState.IsValid)
            {
                db.Entry(leprng).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(leprng);
        }

        // GET: Leprngs/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Leprng leprng = await db.Leprngs.FindAsync(id);
            if (leprng == null)
            {
                return HttpNotFound();
            }
            return View(leprng);
        }

        // POST: Leprngs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Leprng leprng = await db.Leprngs.FindAsync(id);
            db.Leprngs.Remove(leprng);
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
