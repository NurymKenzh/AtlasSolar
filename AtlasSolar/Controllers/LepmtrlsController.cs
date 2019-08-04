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
    public class LepmtrlsController : Controller
    {
        private NpgsqlContext db = new NpgsqlContext();

        // GET: Lepmtrls
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Index(string SortOrder, string Code, string NameEN, string NameKZ, string NameRU, int? Page)
        {
            var lepmtrls = db.Lepmtrls
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
                lepmtrls = lepmtrls.Where(m => m.Code == Code);
            }
            if (!string.IsNullOrEmpty(NameEN))
            {
                lepmtrls = lepmtrls.Where(m => m.NameEN.ToLower().Contains(NameEN.ToLower()));
            }
            if (!string.IsNullOrEmpty(NameKZ))
            {
                lepmtrls = lepmtrls.Where(m => m.NameKZ.ToLower().Contains(NameKZ.ToLower()));
            }
            if (!string.IsNullOrEmpty(NameRU))
            {
                lepmtrls = lepmtrls.Where(m => m.NameRU.ToLower().Contains(NameRU.ToLower()));
            }

            switch (SortOrder)
            {
                case "Code":
                    lepmtrls = lepmtrls.OrderBy(m => m.Code);
                    break;
                case "CodeDesc":
                    lepmtrls = lepmtrls.OrderByDescending(m => m.Code);
                    break;
                case "NameEN":
                    lepmtrls = lepmtrls.OrderBy(m => m.NameEN);
                    break;
                case "NameENDesc":
                    lepmtrls = lepmtrls.OrderByDescending(m => m.NameEN);
                    break;
                case "NameKZ":
                    lepmtrls = lepmtrls.OrderBy(m => m.NameKZ);
                    break;
                case "NameKZDesc":
                    lepmtrls = lepmtrls.OrderByDescending(m => m.NameKZ);
                    break;
                case "NameRU":
                    lepmtrls = lepmtrls.OrderBy(m => m.NameRU);
                    break;
                case "NameRUDesc":
                    lepmtrls = lepmtrls.OrderByDescending(m => m.NameRU);
                    break;
                default:
                    lepmtrls = lepmtrls.OrderBy(m => m.Id);
                    break;
            }

            int PageSize = 50;
            int PageNumber = (Page ?? 1);

            return View(lepmtrls.ToPagedList(PageNumber, PageSize));
        }

        // GET: Lepmtrls/Details/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lepmtrl lepmtrl = await db.Lepmtrls.FindAsync(id);
            if (lepmtrl == null)
            {
                return HttpNotFound();
            }

            return View(lepmtrl);
        }

        // GET: Lepmtrls/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Lepmtrls/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Create([Bind(Include = "Id,Code,NameEN,NameKZ,NameRU")] Lepmtrl lepmtrl)
        {
            if (ModelState.IsValid)
            {
                db.Lepmtrls.Add(lepmtrl);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(lepmtrl);
        }

        // GET: Lepmtrls/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lepmtrl lepmtrl = await db.Lepmtrls.FindAsync(id);
            if (lepmtrl == null)
            {
                return HttpNotFound();
            }
            return View(lepmtrl);
        }

        // POST: Lepmtrls/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Code,NameEN,NameKZ,NameRU")] Lepmtrl lepmtrl)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lepmtrl).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(lepmtrl);
        }

        // GET: Lepmtrls/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Lepmtrl lepmtrl = await db.Lepmtrls.FindAsync(id);
            if (lepmtrl == null)
            {
                return HttpNotFound();
            }
            return View(lepmtrl);
        }

        // POST: Lepmtrls/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Lepmtrl lepmtrl = await db.Lepmtrls.FindAsync(id);
            db.Lepmtrls.Remove(lepmtrl);
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
