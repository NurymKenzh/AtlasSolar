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
    public class LesfondtypesController : Controller
    {
        private NpgsqlContext db = new NpgsqlContext();

        // GET: Lesfondtypes
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Index(string SortOrder, string Code, string NameEN, string NameKZ, string NameRU, int? Page)
        {
            var lesfondtypes = db.Lesfondtypes
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
                lesfondtypes = lesfondtypes.Where(m => m.Code == Code);
            }
            if (!string.IsNullOrEmpty(NameEN))
            {
                lesfondtypes = lesfondtypes.Where(m => m.NameEN.ToLower().Contains(NameEN.ToLower()));
            }
            if (!string.IsNullOrEmpty(NameKZ))
            {
                lesfondtypes = lesfondtypes.Where(m => m.NameKZ.ToLower().Contains(NameKZ.ToLower()));
            }
            if (!string.IsNullOrEmpty(NameRU))
            {
                lesfondtypes = lesfondtypes.Where(m => m.NameRU.ToLower().Contains(NameRU.ToLower()));
            }

            switch (SortOrder)
            {
                case "Code":
                    lesfondtypes = lesfondtypes.OrderBy(m => m.Code);
                    break;
                case "CodeDesc":
                    lesfondtypes = lesfondtypes.OrderByDescending(m => m.Code);
                    break;
                case "NameEN":
                    lesfondtypes = lesfondtypes.OrderBy(m => m.NameEN);
                    break;
                case "NameENDesc":
                    lesfondtypes = lesfondtypes.OrderByDescending(m => m.NameEN);
                    break;
                case "NameKZ":
                    lesfondtypes = lesfondtypes.OrderBy(m => m.NameKZ);
                    break;
                case "NameKZDesc":
                    lesfondtypes = lesfondtypes.OrderByDescending(m => m.NameKZ);
                    break;
                case "NameRU":
                    lesfondtypes = lesfondtypes.OrderBy(m => m.NameRU);
                    break;
                case "NameRUDesc":
                    lesfondtypes = lesfondtypes.OrderByDescending(m => m.NameRU);
                    break;
                default:
                    lesfondtypes = lesfondtypes.OrderBy(m => m.Id);
                    break;
            }

            int PageSize = 50;
            int PageNumber = (Page ?? 1);

            return View(lesfondtypes.ToPagedList(PageNumber, PageSize));
        }

        // GET: Lesfondtypes/Details/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lesfondtype lesfondtype = await db.Lesfondtypes.FindAsync(id);
            if (lesfondtype == null)
            {
                return HttpNotFound();
            }

            return View(lesfondtype);
        }

        // GET: Lesfondtypes/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Lesfondtypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Create([Bind(Include = "Id,Code,NameEN,NameKZ,NameRU")] Lesfondtype lesfondtype)
        {
            if (ModelState.IsValid)
            {
                db.Lesfondtypes.Add(lesfondtype);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(lesfondtype);
        }

        // GET: Lesfondtypes/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lesfondtype lesfondtype = await db.Lesfondtypes.FindAsync(id);
            if (lesfondtype == null)
            {
                return HttpNotFound();
            }
            return View(lesfondtype);
        }

        // POST: Lesfondtypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Code,NameEN,NameKZ,NameRU")] Lesfondtype lesfondtype)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lesfondtype).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(lesfondtype);
        }

        // GET: Lesfondtypes/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lesfondtype lesfondtype = await db.Lesfondtypes.FindAsync(id);
            if (lesfondtype == null)
            {
                return HttpNotFound();
            }
            return View(lesfondtype);
        }

        // POST: Lesfondtypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Lesfondtype lesfondtype = await db.Lesfondtypes.FindAsync(id);
            db.Lesfondtypes.Remove(lesfondtype);
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
