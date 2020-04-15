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
