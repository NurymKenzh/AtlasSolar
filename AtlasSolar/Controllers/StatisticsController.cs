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

namespace AtlasSolar.Controllers
{
    public class StatisticsController : Controller
    {
        private NpgsqlContext db = new NpgsqlContext();

        // GET: Statistics
        [Authorize(Roles = "Admin, Moderator")]
        public async Task<ActionResult> Index()
        {
            var statistics = db.Statistics.Include(s => s.MeteoDataPeriodicity).Include(s => s.MeteoDataSource);
            return View(await statistics.ToListAsync());
        }

        // GET: Statistics/Details/5
        [Authorize(Roles = "Admin, Moderator")]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Statistic statistic = await db.Statistics.FindAsync(id);
            Statistic statistic = await db.Statistics
                .Include(s => s.MeteoDataSource)
                .Include(s => s.MeteoDataPeriodicity)
                .FirstOrDefaultAsync(s => s.Id == id);
            if (statistic == null)
            {
                return HttpNotFound();
            }
            return View(statistic);
        }

        //// GET: Statistics/Create
        //public ActionResult Create()
        //{
        //    ViewBag.MeteoDataPeriodicityId = new SelectList(db.MeteoDataPeriodicities, "Id", "Code");
        //    ViewBag.MeteoDataSourceId = new SelectList(db.MeteoDataSources, "Id", "Code");
        //    return View();
        //}

        //// POST: Statistics/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Create([Bind(Include = "Id,DateTime,MeteoDataSourceId,MeteoDataPeriodicityId,MeteoDataTypesCount,DataCount")] Statistic statistic)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Statistics.Add(statistic);
        //        await db.SaveChangesAsync();
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.MeteoDataPeriodicityId = new SelectList(db.MeteoDataPeriodicities, "Id", "Code", statistic.MeteoDataPeriodicityId);
        //    ViewBag.MeteoDataSourceId = new SelectList(db.MeteoDataSources, "Id", "Code", statistic.MeteoDataSourceId);
        //    return View(statistic);
        //}

        //// GET: Statistics/Edit/5
        //public async Task<ActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Statistic statistic = await db.Statistics.FindAsync(id);
        //    if (statistic == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    ViewBag.MeteoDataPeriodicityId = new SelectList(db.MeteoDataPeriodicities, "Id", "Code", statistic.MeteoDataPeriodicityId);
        //    ViewBag.MeteoDataSourceId = new SelectList(db.MeteoDataSources, "Id", "Code", statistic.MeteoDataSourceId);
        //    return View(statistic);
        //}

        //// POST: Statistics/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Edit([Bind(Include = "Id,DateTime,MeteoDataSourceId,MeteoDataPeriodicityId,MeteoDataTypesCount,DataCount")] Statistic statistic)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(statistic).State = EntityState.Modified;
        //        await db.SaveChangesAsync();
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.MeteoDataPeriodicityId = new SelectList(db.MeteoDataPeriodicities, "Id", "Code", statistic.MeteoDataPeriodicityId);
        //    ViewBag.MeteoDataSourceId = new SelectList(db.MeteoDataSources, "Id", "Code", statistic.MeteoDataSourceId);
        //    return View(statistic);
        //}

        //// GET: Statistics/Delete/5
        //public async Task<ActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Statistic statistic = await db.Statistics.FindAsync(id);
        //    if (statistic == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(statistic);
        //}

        //// POST: Statistics/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> DeleteConfirmed(int id)
        //{
        //    Statistic statistic = await db.Statistics.FindAsync(id);
        //    db.Statistics.Remove(statistic);
        //    await db.SaveChangesAsync();
        //    return RedirectToAction("Index");
        //}

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
