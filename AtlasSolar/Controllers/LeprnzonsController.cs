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
    public class LeprnzonsController : Controller
    {
        private NpgsqlContext db = new NpgsqlContext();

        // GET: Leprnzons
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Index(string SortOrder, string Code, string NameEN, string NameKZ, string NameRU, int? Page)
        {
            var leprnzons = db.Leprnzons
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
                leprnzons = leprnzons.Where(m => m.Code == Code);
            }
            if (!string.IsNullOrEmpty(NameEN))
            {
                leprnzons = leprnzons.Where(m => m.NameEN.ToLower().Contains(NameEN.ToLower()));
            }
            if (!string.IsNullOrEmpty(NameKZ))
            {
                leprnzons = leprnzons.Where(m => m.NameKZ.ToLower().Contains(NameKZ.ToLower()));
            }
            if (!string.IsNullOrEmpty(NameRU))
            {
                leprnzons = leprnzons.Where(m => m.NameRU.ToLower().Contains(NameRU.ToLower()));
            }

            switch (SortOrder)
            {
                case "Code":
                    leprnzons = leprnzons.OrderBy(m => m.Code);
                    break;
                case "CodeDesc":
                    leprnzons = leprnzons.OrderByDescending(m => m.Code);
                    break;
                case "NameEN":
                    leprnzons = leprnzons.OrderBy(m => m.NameEN);
                    break;
                case "NameENDesc":
                    leprnzons = leprnzons.OrderByDescending(m => m.NameEN);
                    break;
                case "NameKZ":
                    leprnzons = leprnzons.OrderBy(m => m.NameKZ);
                    break;
                case "NameKZDesc":
                    leprnzons = leprnzons.OrderByDescending(m => m.NameKZ);
                    break;
                case "NameRU":
                    leprnzons = leprnzons.OrderBy(m => m.NameRU);
                    break;
                case "NameRUDesc":
                    leprnzons = leprnzons.OrderByDescending(m => m.NameRU);
                    break;
                default:
                    leprnzons = leprnzons.OrderBy(m => m.Id);
                    break;
            }

            int PageSize = 50;
            int PageNumber = (Page ?? 1);

            return View(leprnzons.ToPagedList(PageNumber, PageSize));
        }

        // GET: Leprnzons/Details/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Leprnzon leprnzon = await db.Leprnzons.FindAsync(id);
            if (leprnzon == null)
            {
                return HttpNotFound();
            }

            return View(leprnzon);
        }

        // GET: Leprnzons/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Leprnzons/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Create([Bind(Include = "Id,Code,NameEN,NameKZ,NameRU")] Leprnzon leprnzon)
        {
            if (ModelState.IsValid)
            {
                db.Leprnzons.Add(leprnzon);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(leprnzon);
        }

        // GET: Leprnzons/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Leprnzon leprnzon = await db.Leprnzons.FindAsync(id);
            if (leprnzon == null)
            {
                return HttpNotFound();
            }
            return View(leprnzon);
        }

        // POST: Leprnzons/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Code,NameEN,NameKZ,NameRU")] Leprnzon leprnzon)
        {
            if (ModelState.IsValid)
            {
                db.Entry(leprnzon).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(leprnzon);
        }

        // GET: Leprnzons/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Leprnzon leprnzon = await db.Leprnzons.FindAsync(id);
            if (leprnzon == null)
            {
                return HttpNotFound();
            }
            return View(leprnzon);
        }

        // POST: Leprnzons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Leprnzon leprnzon = await db.Leprnzons.FindAsync(id);
            db.Leprnzons.Remove(leprnzon);
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
