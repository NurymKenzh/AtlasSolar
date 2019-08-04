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
    public class LeptypesController : Controller
    {
        private NpgsqlContext db = new NpgsqlContext();

        // GET: Leptypes
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Index(string SortOrder, string Code, string NameEN, string NameKZ, string NameRU, int? Page)
        {
            var leptypes = db.Leptypes
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
                leptypes = leptypes.Where(m => m.Code == Code);
            }
            if (!string.IsNullOrEmpty(NameEN))
            {
                leptypes = leptypes.Where(m => m.NameEN.ToLower().Contains(NameEN.ToLower()));
            }
            if (!string.IsNullOrEmpty(NameKZ))
            {
                leptypes = leptypes.Where(m => m.NameKZ.ToLower().Contains(NameKZ.ToLower()));
            }
            if (!string.IsNullOrEmpty(NameRU))
            {
                leptypes = leptypes.Where(m => m.NameRU.ToLower().Contains(NameRU.ToLower()));
            }

            switch (SortOrder)
            {
                case "Code":
                    leptypes = leptypes.OrderBy(m => m.Code);
                    break;
                case "CodeDesc":
                    leptypes = leptypes.OrderByDescending(m => m.Code);
                    break;
                case "NameEN":
                    leptypes = leptypes.OrderBy(m => m.NameEN);
                    break;
                case "NameENDesc":
                    leptypes = leptypes.OrderByDescending(m => m.NameEN);
                    break;
                case "NameKZ":
                    leptypes = leptypes.OrderBy(m => m.NameKZ);
                    break;
                case "NameKZDesc":
                    leptypes = leptypes.OrderByDescending(m => m.NameKZ);
                    break;
                case "NameRU":
                    leptypes = leptypes.OrderBy(m => m.NameRU);
                    break;
                case "NameRUDesc":
                    leptypes = leptypes.OrderByDescending(m => m.NameRU);
                    break;
                default:
                    leptypes = leptypes.OrderBy(m => m.Id);
                    break;
            }

            int PageSize = 50;
            int PageNumber = (Page ?? 1);

            return View(leptypes.ToPagedList(PageNumber, PageSize));
        }

        // GET: Leptypes/Details/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Leptype leptype = await db.Leptypes.FindAsync(id);
            if (leptype == null)
            {
                return HttpNotFound();
            }

            return View(leptype);
        }

        // GET: Leptypes/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Leptypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Create([Bind(Include = "Id,Code,NameEN,NameKZ,NameRU")] Leptype leptype)
        {
            if (ModelState.IsValid)
            {
                db.Leptypes.Add(leptype);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(leptype);
        }

        // GET: Leptypes/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Leptype leptype = await db.Leptypes.FindAsync(id);
            if (leptype == null)
            {
                return HttpNotFound();
            }
            return View(leptype);
        }

        // POST: Leptypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Code,NameEN,NameKZ,NameRU")] Leptype leptype)
        {
            if (ModelState.IsValid)
            {
                db.Entry(leptype).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(leptype);
        }

        // GET: Leptypes/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Leptype leptype = await db.Leptypes.FindAsync(id);
            if (leptype == null)
            {
                return HttpNotFound();
            }
            return View(leptype);
        }

        // POST: Leptypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Leptype leptype = await db.Leptypes.FindAsync(id);
            db.Leptypes.Remove(leptype);
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
