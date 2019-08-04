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
    public class LesfondOrgupsController : Controller
    {
        private NpgsqlContext db = new NpgsqlContext();

        // GET: LesfondOrgups
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Index(string SortOrder, string Code, string NameEN, string NameKZ, string NameRU, int? Page)
        {
            var lesfondorgups = db.LesfondOrgups
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
                lesfondorgups = lesfondorgups.Where(m => m.Code == Code);
            }
            if (!string.IsNullOrEmpty(NameEN))
            {
                lesfondorgups = lesfondorgups.Where(m => m.NameEN.ToLower().Contains(NameEN.ToLower()));
            }
            if (!string.IsNullOrEmpty(NameKZ))
            {
                lesfondorgups = lesfondorgups.Where(m => m.NameKZ.ToLower().Contains(NameKZ.ToLower()));
            }
            if (!string.IsNullOrEmpty(NameRU))
            {
                lesfondorgups = lesfondorgups.Where(m => m.NameRU.ToLower().Contains(NameRU.ToLower()));
            }

            switch (SortOrder)
            {
                case "Code":
                    lesfondorgups = lesfondorgups.OrderBy(m => m.Code);
                    break;
                case "CodeDesc":
                    lesfondorgups = lesfondorgups.OrderByDescending(m => m.Code);
                    break;
                case "NameEN":
                    lesfondorgups = lesfondorgups.OrderBy(m => m.NameEN);
                    break;
                case "NameENDesc":
                    lesfondorgups = lesfondorgups.OrderByDescending(m => m.NameEN);
                    break;
                case "NameKZ":
                    lesfondorgups = lesfondorgups.OrderBy(m => m.NameKZ);
                    break;
                case "NameKZDesc":
                    lesfondorgups = lesfondorgups.OrderByDescending(m => m.NameKZ);
                    break;
                case "NameRU":
                    lesfondorgups = lesfondorgups.OrderBy(m => m.NameRU);
                    break;
                case "NameRUDesc":
                    lesfondorgups = lesfondorgups.OrderByDescending(m => m.NameRU);
                    break;
                default:
                    lesfondorgups = lesfondorgups.OrderBy(m => m.Id);
                    break;
            }

            int PageSize = 50;
            int PageNumber = (Page ?? 1);

            return View(lesfondorgups.ToPagedList(PageNumber, PageSize));
        }

        // GET: LesfondOrgups/Details/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LesfondOrgup lesfondorgup = await db.LesfondOrgups.FindAsync(id);
            if (lesfondorgup == null)
            {
                return HttpNotFound();
            }

            return View(lesfondorgup);
        }

        // GET: LesfondOrgups/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: LesfondOrgups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Create([Bind(Include = "Id,Code,NameEN,NameKZ,NameRU")] LesfondOrgup lesfondorgup)
        {
            if (ModelState.IsValid)
            {
                db.LesfondOrgups.Add(lesfondorgup);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(lesfondorgup);
        }

        // GET: LesfondOrgups/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LesfondOrgup lesfondorgup = await db.LesfondOrgups.FindAsync(id);
            if (lesfondorgup == null)
            {
                return HttpNotFound();
            }
            return View(lesfondorgup);
        }

        // POST: LesfondOrgups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Code,NameEN,NameKZ,NameRU")] LesfondOrgup lesfondorgup)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lesfondorgup).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(lesfondorgup);
        }

        // GET: LesfondOrgups/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LesfondOrgup lesfondorgup = await db.LesfondOrgups.FindAsync(id);
            if (lesfondorgup == null)
            {
                return HttpNotFound();
            }
            return View(lesfondorgup);
        }

        // POST: LesfondOrgups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            LesfondOrgup lesfondorgup = await db.LesfondOrgups.FindAsync(id);
            db.LesfondOrgups.Remove(lesfondorgup);
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
