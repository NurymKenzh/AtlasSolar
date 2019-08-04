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
    public class LepinsttpsController : Controller
    {
        private NpgsqlContext db = new NpgsqlContext();

        // GET: Lepinsttps
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Index(string SortOrder, string Code, string NameEN, string NameKZ, string NameRU, int? Page)
        {
            var lepinsttps = db.Lepinsttps
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
                lepinsttps = lepinsttps.Where(m => m.Code == Code);
            }
            if (!string.IsNullOrEmpty(NameEN))
            {
                lepinsttps = lepinsttps.Where(m => m.NameEN.ToLower().Contains(NameEN.ToLower()));
            }
            if (!string.IsNullOrEmpty(NameKZ))
            {
                lepinsttps = lepinsttps.Where(m => m.NameKZ.ToLower().Contains(NameKZ.ToLower()));
            }
            if (!string.IsNullOrEmpty(NameRU))
            {
                lepinsttps = lepinsttps.Where(m => m.NameRU.ToLower().Contains(NameRU.ToLower()));
            }

            switch (SortOrder)
            {
                case "Code":
                    lepinsttps = lepinsttps.OrderBy(m => m.Code);
                    break;
                case "CodeDesc":
                    lepinsttps = lepinsttps.OrderByDescending(m => m.Code);
                    break;
                case "NameEN":
                    lepinsttps = lepinsttps.OrderBy(m => m.NameEN);
                    break;
                case "NameENDesc":
                    lepinsttps = lepinsttps.OrderByDescending(m => m.NameEN);
                    break;
                case "NameKZ":
                    lepinsttps = lepinsttps.OrderBy(m => m.NameKZ);
                    break;
                case "NameKZDesc":
                    lepinsttps = lepinsttps.OrderByDescending(m => m.NameKZ);
                    break;
                case "NameRU":
                    lepinsttps = lepinsttps.OrderBy(m => m.NameRU);
                    break;
                case "NameRUDesc":
                    lepinsttps = lepinsttps.OrderByDescending(m => m.NameRU);
                    break;
                default:
                    lepinsttps = lepinsttps.OrderBy(m => m.Id);
                    break;
            }

            int PageSize = 50;
            int PageNumber = (Page ?? 1);

            return View(lepinsttps.ToPagedList(PageNumber, PageSize));
        }

        // GET: Lepinsttps/Details/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lepinsttp lepinsttp = await db.Lepinsttps.FindAsync(id);
            if (lepinsttp == null)
            {
                return HttpNotFound();
            }

            return View(lepinsttp);
        }

        // GET: Lepinsttps/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Lepinsttps/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Create([Bind(Include = "Id,Code,NameEN,NameKZ,NameRU")] Lepinsttp lepinsttp)
        {
            if (ModelState.IsValid)
            {
                db.Lepinsttps.Add(lepinsttp);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(lepinsttp);
        }

        // GET: Lepinsttps/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lepinsttp lepinsttp = await db.Lepinsttps.FindAsync(id);
            if (lepinsttp == null)
            {
                return HttpNotFound();
            }
            return View(lepinsttp);
        }

        // POST: Lepinsttps/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Code,NameEN,NameKZ,NameRU")] Lepinsttp lepinsttp)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lepinsttp).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(lepinsttp);
        }

        // GET: Lepinsttps/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lepinsttp lepinsttp = await db.Lepinsttps.FindAsync(id);
            if (lepinsttp == null)
            {
                return HttpNotFound();
            }
            return View(lepinsttp);
        }

        // POST: Lepinsttps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Lepinsttp lepinsttp = await db.Lepinsttps.FindAsync(id);
            db.Lepinsttps.Remove(lepinsttp);
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
