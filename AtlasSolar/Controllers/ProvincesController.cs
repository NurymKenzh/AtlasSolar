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
    public class ProvincesController : Controller
    {
        private NpgsqlContext db = new NpgsqlContext();

        // GET: Provinces
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Index(string SortOrder, string Code, string NameRU, int? Page)
        {
            var provinces = db.Provinces
                .Where(p => true);

            ViewBag.CodeFilter = Code;
            ViewBag.NameRUFilter = NameRU;

            ViewBag.CodeSort = SortOrder == "Code" ? "CodeDesc" : "Code";
            ViewBag.NameRUSort = SortOrder == "NameRU" ? "NameRUDesc" : "NameRU";

            if (!string.IsNullOrEmpty(Code))
            {
                provinces = provinces.Where(p => p.Code.ToLower().Contains(Code.ToLower()));
            }
            if (!string.IsNullOrEmpty(NameRU))
            {
                provinces = provinces.Where(p => p.NameRU.ToLower().Contains(NameRU.ToLower()));
            }

            switch (SortOrder)
            {
                case "Code":
                    provinces = provinces.OrderBy(p => p.Code);
                    break;
                case "CodeDesc":
                    provinces = provinces.OrderByDescending(p => p.Code);
                    break;
                case "NameRU":
                    provinces = provinces.OrderBy(p => p.NameRU);
                    break;
                case "NameRUDesc":
                    provinces = provinces.OrderByDescending(p => p.NameRU);
                    break;
                default:
                    provinces = provinces.OrderBy(p => p.Id);
                    break;
            }

            int PageSize = 50;
            int PageNumber = (Page ?? 1);

            return View(provinces.ToPagedList(PageNumber, PageSize));
        }

        // GET: Provinces/Details/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Province province = await db.Provinces.FindAsync(id);
            if (province == null)
            {
                return HttpNotFound();
            }
            return View(province);
        }

        // GET: Provinces/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Provinces/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Create([Bind(Include = "Id,Code,NameEN,NameKZ,NameRU,Max_auto_dist,Min_auto_dist,Max_lep_dist,Min_lep_dist,Max_np_dist,Min_np_dist,Max_slope_srtm,Min_slope_srtm,Max_srtm,Min_srtm,Max_swvdwnyear,Min_swvdwnyear,Max_longitude,Min_longitude,Max_latitude,Min_latitude")] Province province)
        {
            if (ModelState.IsValid)
            {
                db.Provinces.Add(province);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(province);
        }

        // GET: Provinces/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Province province = await db.Provinces.FindAsync(id);
            if (province == null)
            {
                return HttpNotFound();
            }
            return View(province);
        }

        // POST: Provinces/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Code,NameEN,NameKZ,NameRU,Max_auto_dist,Min_auto_dist,Max_lep_dist,Min_lep_dist,Max_np_dist,Min_np_dist,Max_slope_srtm,Min_slope_srtm,Max_srtm,Min_srtm,Max_swvdwnyear,Min_swvdwnyear,Max_longitude,Min_longitude,Max_latitude,Min_latitude")] Province province)
        {
            if (ModelState.IsValid)
            {
                db.Entry(province).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(province);
        }

        // GET: Provinces/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Province province = await db.Provinces.FindAsync(id);
            if (province == null)
            {
                return HttpNotFound();
            }
            return View(province);
        }

        // POST: Provinces/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Province province = await db.Provinces.FindAsync(id);
            db.Provinces.Remove(province);
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
