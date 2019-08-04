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
    public class OptionsController : Controller
    {
        private NpgsqlContext db = new NpgsqlContext();

        // GET: Options
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Index(string SortOrder, string Code, string DescriptionKZ, string DescriptionRU, int? Page)
        {
            var options = db.Options
                .Where(o => true);

            ViewBag.CodeFilter = Code;
            ViewBag.DescriptionKZFilter = DescriptionKZ;
            ViewBag.DescriptionRUFilter = DescriptionRU;

            ViewBag.CodeSort = SortOrder == "Code" ? "CodeDesc" : "Code";
            ViewBag.DescriptionKZSort = SortOrder == "DescriptionKZ" ? "DescriptionKZDesc" : "DescriptionKZ";
            ViewBag.DescriptionRUSort = SortOrder == "DescriptionRU" ? "DescriptionRUDesc" : "DescriptionRU";

            if (!string.IsNullOrEmpty(Code))
            {
                options = options.Where(o => o.Code == Code);
            }
            if (!string.IsNullOrEmpty(DescriptionKZ))
            {
                options = options.Where(o => o.DescriptionKZ.ToLower().Contains(DescriptionKZ.ToLower()));
            }
            if (!string.IsNullOrEmpty(DescriptionRU))
            {
                options = options.Where(o => o.DescriptionRU.ToLower().Contains(DescriptionRU.ToLower()));
            }

            switch (SortOrder)
            {
                case "Code":
                    options = options.OrderBy(o => o.Code);
                    break;
                case "CodeDesc":
                    options = options.OrderByDescending(o => o.Code);
                    break;
                case "DescriptionKZ":
                    options = options.OrderBy(o => o.DescriptionKZ);
                    break;
                case "DescriptionKZDesc":
                    options = options.OrderByDescending(o => o.DescriptionKZ);
                    break;
                case "DescriptionRU":
                    options = options.OrderBy(o => o.DescriptionRU);
                    break;
                case "DescriptionRUDesc":
                    options = options.OrderByDescending(o => o.DescriptionRU);
                    break;
                default:
                    options = options.OrderBy(o => o.Id);
                    break;
            }

            int PageSize = 50;
            int PageNumber = (Page ?? 1);

            return View(options.ToPagedList(PageNumber, PageSize));
        }

        // GET: Options/Details/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Option option = await db.Options.FindAsync(id);
            if (option == null)
            {
                return HttpNotFound();
            }
            return View(option);
        }

        // GET: Options/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Options/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Create([Bind(Include = "Id,Code,DescriptionKZ,DescriptionRU,Value")] Option option)
        {
            if (ModelState.IsValid)
            {
                db.Options.Add(option);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(option);
        }

        // GET: Options/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Option option = await db.Options.FindAsync(id);
            if (option == null)
            {
                return HttpNotFound();
            }
            return View(option);
        }

        // POST: Options/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Code,DescriptionKZ,DescriptionRU,Value")] Option option)
        {
            if (ModelState.IsValid)
            {
                db.Entry(option).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(option);
        }

        // GET: Options/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Option option = await db.Options.FindAsync(id);
            if (option == null)
            {
                return HttpNotFound();
            }
            return View(option);
        }

        // POST: Options/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Option option = await db.Options.FindAsync(id);
            db.Options.Remove(option);
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
