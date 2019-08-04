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
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace AtlasSolar.Controllers
{
    public class MeteoDataTypesController : Controller
    {
        private NpgsqlContext db = new NpgsqlContext();
        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // GET: MeteoDataTypes
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<ActionResult> Index(string SortOrder, int? MeteoDataSourceId, int? MeteoDataPeriodicityId, string Code, string NameEN, string NameKZ, string NameRU, string GroupEN, string GroupKZ, string GroupRU, int? Page)
        {
            HttpCookie cookie = System.Web.HttpContext.Current.Request.Cookies["Language"];

            var meteodatatypes = db.MeteoDataTypes
                .Where(m => true)
                .Include(m => m.MeteoDataSource)
                .Include(m => m.MeteoDataPeriodicity);

            ViewBag.MeteoDataSourceIdFilter = MeteoDataSourceId;
            ViewBag.MeteoDataPeriodicityIdFilter = MeteoDataPeriodicityId;
            ViewBag.CodeFilter = Code;
            ViewBag.NameENFilter = NameEN;
            ViewBag.NameKZFilter = NameKZ;
            ViewBag.NameRUFilter = NameRU;
            ViewBag.GroupENFilter = GroupEN;
            ViewBag.GroupKZFilter = GroupKZ;
            ViewBag.GroupRUFilter = GroupRU;

            ViewBag.MeteoDataSourceSort = SortOrder == "MeteoDataSource" ? "MeteoDataSourceDesc" : "MeteoDataSource";
            ViewBag.MeteoDataPeriodicitySort = SortOrder == "MeteoDataPeriodicity" ? "MeteoDataPeriodicityDesc" : "MeteoDataPeriodicity";
            ViewBag.CodeSort = SortOrder == "Code" ? "CodeDesc" : "Code";
            ViewBag.NameENSort = SortOrder == "NameEN" ? "NameENDesc" : "NameEN";
            ViewBag.NameKZSort = SortOrder == "NameKZ" ? "NameKZDesc" : "NameKZ";
            ViewBag.NameRUSort = SortOrder == "NameRU" ? "NameRUDesc" : "NameRU";
            ViewBag.GroupENSort = SortOrder == "GroupEN" ? "GroupENDesc" : "GroupEN";
            ViewBag.GroupKZSort = SortOrder == "GroupKZ" ? "GroupKZDesc" : "GroupKZ";
            ViewBag.GroupRUSort = SortOrder == "GroupRU" ? "GroupRUDesc" : "GroupRU";

            if (MeteoDataSourceId != null)
            {
                meteodatatypes = meteodatatypes.Where(m => m.MeteoDataSourceId == MeteoDataSourceId);
            }
            if (MeteoDataPeriodicityId != null)
            {
                meteodatatypes = meteodatatypes.Where(m => m.MeteoDataPeriodicityId == MeteoDataPeriodicityId);
            }
            if (!string.IsNullOrEmpty(Code))
            {
                meteodatatypes = meteodatatypes.Where(m => m.Code.ToLower().Contains(Code.ToLower()));
            }
            if (!string.IsNullOrEmpty(NameEN))
            {
                meteodatatypes = meteodatatypes.Where(m => m.NameEN.ToLower().Contains(NameEN.ToLower()));
            }
            if (!string.IsNullOrEmpty(NameKZ))
            {
                meteodatatypes = meteodatatypes.Where(m => m.NameKZ.ToLower().Contains(NameKZ.ToLower()));
            }
            if (!string.IsNullOrEmpty(NameRU))
            {
                meteodatatypes = meteodatatypes.Where(m => m.NameRU.ToLower().Contains(NameRU.ToLower()));
            }
            if (!string.IsNullOrEmpty(GroupEN))
            {
                meteodatatypes = meteodatatypes.Where(m => m.GroupEN.ToLower().Contains(GroupEN.ToLower()));
            }
            if (!string.IsNullOrEmpty(GroupKZ))
            {
                meteodatatypes = meteodatatypes.Where(m => m.GroupKZ.ToLower().Contains(GroupKZ.ToLower()));
            }
            if (!string.IsNullOrEmpty(GroupRU))
            {
                meteodatatypes = meteodatatypes.Where(m => m.GroupRU.ToLower().Contains(GroupRU.ToLower()));
            }

            switch (SortOrder)
            {
                case "MeteoDataSource":
                    if (cookie != null)
                    {
                        if (!string.IsNullOrEmpty(cookie.Value))
                        {
                            if (cookie.Value == "en")
                            {
                                meteodatatypes = meteodatatypes
                                    .OrderBy(m => m.MeteoDataSource.NameEN);
                            }
                            if (cookie.Value == "kk")
                            {
                                meteodatatypes = meteodatatypes
                                    .OrderBy(m => m.MeteoDataSource.NameKZ);
                            }
                            if (cookie.Value == "ru")
                            {
                                meteodatatypes = meteodatatypes
                                    .OrderBy(m => m.MeteoDataSource.NameRU);
                            }
                        }
                    }
                    else
                    {
                        meteodatatypes = meteodatatypes
                            .OrderBy(m => m.MeteoDataSource.NameRU);
                    }
                    break;
                case "MeteoDataSourceDesc":
                    if (cookie != null)
                    {
                        if (!string.IsNullOrEmpty(cookie.Value))
                        {
                            if (cookie.Value == "en")
                            {
                                meteodatatypes = meteodatatypes
                                    .OrderByDescending(m => m.MeteoDataSource.NameEN);
                            }
                            if (cookie.Value == "kk")
                            {
                                meteodatatypes = meteodatatypes
                                    .OrderByDescending(m => m.MeteoDataSource.NameKZ);
                            }
                            if (cookie.Value == "ru")
                            {
                                meteodatatypes = meteodatatypes
                                    .OrderByDescending(m => m.MeteoDataSource.NameRU);
                            }
                        }
                    }
                    else
                    {
                        meteodatatypes = meteodatatypes
                            .OrderByDescending(m => m.MeteoDataSource.NameRU);
                    }
                    break;
                case "MeteoDataPeriodicity":
                    if (cookie != null)
                    {
                        if (!string.IsNullOrEmpty(cookie.Value))
                        {
                            if (cookie.Value == "en")
                            {
                                meteodatatypes = meteodatatypes
                                    .OrderBy(m => m.MeteoDataPeriodicity.NameEN);
                            }
                            if (cookie.Value == "kk")
                            {
                                meteodatatypes = meteodatatypes
                                    .OrderBy(m => m.MeteoDataPeriodicity.NameKZ);
                            }
                            if (cookie.Value == "ru")
                            {
                                meteodatatypes = meteodatatypes
                                    .OrderBy(m => m.MeteoDataPeriodicity.NameRU);
                            }
                        }
                    }
                    else
                    {
                        meteodatatypes = meteodatatypes
                            .OrderBy(m => m.MeteoDataPeriodicity.NameRU);
                    }
                    break;
                case "MeteoDataPeriodicityDesc":
                    if (cookie != null)
                    {
                        if (!string.IsNullOrEmpty(cookie.Value))
                        {
                            if (cookie.Value == "en")
                            {
                                meteodatatypes = meteodatatypes
                                    .OrderByDescending(m => m.MeteoDataPeriodicity.NameEN);
                            }
                            if (cookie.Value == "kk")
                            {
                                meteodatatypes = meteodatatypes
                                    .OrderByDescending(m => m.MeteoDataPeriodicity.NameKZ);
                            }
                            if (cookie.Value == "ru")
                            {
                                meteodatatypes = meteodatatypes
                                    .OrderByDescending(m => m.MeteoDataPeriodicity.NameRU);
                            }
                        }
                    }
                    else
                    {
                        meteodatatypes = meteodatatypes
                            .OrderByDescending(m => m.MeteoDataPeriodicity.NameRU);
                    }
                    break;
                case "Code":
                    meteodatatypes = meteodatatypes.OrderBy(m => m.Code);
                    break;
                case "CodeDesc":
                    meteodatatypes = meteodatatypes.OrderByDescending(m => m.Code);
                    break;
                case "NameEN":
                    meteodatatypes = meteodatatypes.OrderBy(m => m.NameEN);
                    break;
                case "NameENDesc":
                    meteodatatypes = meteodatatypes.OrderByDescending(m => m.NameEN);
                    break;
                case "NameKZ":
                    meteodatatypes = meteodatatypes.OrderBy(m => m.NameKZ);
                    break;
                case "NameKZDesc":
                    meteodatatypes = meteodatatypes.OrderByDescending(m => m.NameKZ);
                    break;
                case "NameRU":
                    meteodatatypes = meteodatatypes.OrderBy(m => m.NameRU);
                    break;
                case "NameRUDesc":
                    meteodatatypes = meteodatatypes.OrderByDescending(m => m.NameRU);
                    break;
                case "GroupEN":
                    meteodatatypes = meteodatatypes.OrderBy(m => m.GroupEN);
                    break;
                case "GroupENDesc":
                    meteodatatypes = meteodatatypes.OrderByDescending(m => m.GroupEN);
                    break;
                case "GroupKZ":
                    meteodatatypes = meteodatatypes.OrderBy(m => m.GroupKZ);
                    break;
                case "GroupKZDesc":
                    meteodatatypes = meteodatatypes.OrderByDescending(m => m.GroupKZ);
                    break;
                case "GroupRU":
                    meteodatatypes = meteodatatypes.OrderBy(m => m.GroupRU);
                    break;
                case "GroupRUDesc":
                    meteodatatypes = meteodatatypes.OrderByDescending(m => m.GroupRU);
                    break;
                default:
                    meteodatatypes = meteodatatypes.OrderBy(m => m.Id);
                    break;
            }

            IList<MeteoDataSource> meteodatasources = db.MeteoDataSources
                .Where(m => true)
                .ToList();
            IList<MeteoDataPeriodicity> meteodataperiodicities = db.MeteoDataPeriodicities
                .Where(m => true)
                .ToList();
            meteodatasources = meteodatasources
                .OrderBy(m => m.Name)
                .ToList();
            meteodataperiodicities = meteodataperiodicities
                .OrderBy(m => m.Name)
                .ToList();
            ViewBag.MeteoDataSources = new SelectList(meteodatasources, "Id", "Name");
            ViewBag.MeteoDataPeriodicities = new SelectList(meteodataperiodicities, "Id", "Name");

            ViewBag.Role = "";
            string CurrentUserId = User.Identity.GetUserId();
            if (UserManager.IsInRole(CurrentUserId, "Admin"))
            {
                ViewBag.Role = "Admin";
            }

            int PageSize = 50;
            int PageNumber = (Page ?? 1);

            return View(meteodatatypes.ToPagedList(PageNumber, PageSize));
        }

        // GET: MeteoDataTypes/Details/5
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MeteoDataType meteoDataType = await db.MeteoDataTypes.
                Where(m => m.Id == id)
                .Include(m => m.MeteoDataSource)
                .Include(m => m.MeteoDataPeriodicity)
                .FirstOrDefaultAsync();
            if (meteoDataType == null)
            {
                return HttpNotFound();
            }

            ViewBag.Role = "";
            string CurrentUserId = User.Identity.GetUserId();
            if (UserManager.IsInRole(CurrentUserId, "Admin"))
            {
                ViewBag.Role = "Admin";
            }

            return View(meteoDataType);
        }

        // GET: MeteoDataTypes/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            ViewBag.MeteoDataSourceId = new SelectList(db.MeteoDataSources
                .ToList()
                .OrderBy(m => m.Name), "Id", "Name");
            ViewBag.MeteoDataPeriodicityId = new SelectList(db.MeteoDataPeriodicities
                .ToList()
                .OrderBy(m => m.Name), "Id", "Name");
            return View();
        }

        // POST: MeteoDataTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Create([Bind(Include = "Id,MeteoDataSourceId,MeteoDataPeriodicityId,Code,GroupEN,GroupKZ,GroupRU,NameEN,NameKZ,NameRU,AdditionalEN,AdditionalKZ,AdditionalRU,DescriptionEN,DescriptionKZ,DescriptionRU")] MeteoDataType meteoDataType)
        {
            if (ModelState.IsValid)
            {
                db.MeteoDataTypes.Add(meteoDataType);
                await db.SaveChangesAsync();

                string comment = $"Id: {meteoDataType.Id} MeteoDataSourceId: {meteoDataType.MeteoDataSourceId} MeteoDataPeriodicityId: {meteoDataType.MeteoDataPeriodicityId} Code: {meteoDataType.Code}";
                if (!string.IsNullOrEmpty(meteoDataType.GroupEN))
                {
                    comment += $" GroupEN: {meteoDataType.GroupEN} GroupKZ: {meteoDataType.GroupKZ} GroupRU: {meteoDataType.GroupRU}";
                }
                comment += $" NameEN: {meteoDataType.NameEN} NameKZ: {meteoDataType.NameKZ} NameRU: {meteoDataType.NameRU}";
                if (!string.IsNullOrEmpty(meteoDataType.AdditionalEN))
                {
                    comment += $" AdditionalEN: {meteoDataType.AdditionalEN}";
                }
                if (!string.IsNullOrEmpty(meteoDataType.AdditionalKZ))
                {
                    comment += $" AdditionalKZ: {meteoDataType.AdditionalKZ}";
                }
                if (!string.IsNullOrEmpty(meteoDataType.AdditionalRU))
                {
                    comment += $" AdditionalRU: {meteoDataType.AdditionalRU}";
                }
                if (!string.IsNullOrEmpty(meteoDataType.DescriptionEN))
                {
                    comment += $" DescriptionEN: {meteoDataType.DescriptionEN} DescriptionKZ: {meteoDataType.DescriptionKZ} DescriptionRU: {meteoDataType.DescriptionRU}";
                }
                SystemLog.New("MeteoDataTypeCreate", comment, null, false);

                return RedirectToAction("Index");
            }

            ViewBag.MeteoDataSourceId = new SelectList(db.MeteoDataSources
                .ToList()
                .OrderBy(m => m.Name), "Id", "Name", meteoDataType.MeteoDataSourceId);
            ViewBag.MeteoDataPeriodicityId = new SelectList(db.MeteoDataPeriodicities
                .ToList()
                .OrderBy(m => m.Name), "Id", "Name", meteoDataType.MeteoDataPeriodicityId);
            return View(meteoDataType);
        }

        // GET: MeteoDataTypes/Edit/5
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MeteoDataType meteoDataType = await db.MeteoDataTypes.FindAsync(id);
            if (meteoDataType == null)
            {
                return HttpNotFound();
            }

            ViewBag.MeteoDataSourceId = new SelectList(db.MeteoDataSources
                .ToList()
                .OrderBy(m => m.Name), "Id", "Name", meteoDataType.MeteoDataSourceId);
            ViewBag.MeteoDataPeriodicityId = new SelectList(db.MeteoDataPeriodicities
                .ToList()
                .OrderBy(m => m.Name), "Id", "Name", meteoDataType.MeteoDataPeriodicityId);
            return View(meteoDataType);
        }

        // POST: MeteoDataTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<ActionResult> Edit([Bind(Include = "Id,MeteoDataSourceId,MeteoDataPeriodicityId,Code,GroupEN,GroupKZ,GroupRU,NameEN,NameKZ,NameRU,AdditionalEN,AdditionalKZ,AdditionalRU,DescriptionEN,DescriptionKZ,DescriptionRU")] MeteoDataType meteoDataType)
        {
            if (ModelState.IsValid)
            {
                MeteoDataType mdt = null;
                using (var dblocal = new NpgsqlContext())
                {
                    mdt = dblocal.MeteoDataTypes.Where(m => m.Id == meteoDataType.Id).FirstOrDefault();
                    dblocal.Dispose();
                    GC.Collect();
                }

                meteoDataType.Code = mdt.Code;
                meteoDataType.AdditionalEN = mdt.AdditionalEN;
                meteoDataType.GroupEN = mdt.GroupEN;

                db.Entry(meteoDataType).State = EntityState.Modified;
                await db.SaveChangesAsync();

                
                string comment = $"Id: {mdt.Id} MeteoDataSourceId: {mdt.MeteoDataSourceId} MeteoDataPeriodicityId: {mdt.MeteoDataPeriodicityId} Code: {mdt.Code}";
                if (!string.IsNullOrEmpty(mdt.GroupEN))
                {
                    comment += $" GroupEN: {mdt.GroupEN} GroupKZ: {mdt.GroupKZ} GroupRU: {mdt.GroupRU}";
                }
                comment += $" NameEN: {mdt.NameEN} NameKZ: {mdt.NameKZ} NameRU: {mdt.NameRU}";
                if (!string.IsNullOrEmpty(mdt.AdditionalEN))
                {
                    comment += $" AdditionalEN: {mdt.AdditionalEN}";
                }
                if (!string.IsNullOrEmpty(mdt.AdditionalKZ))
                {
                    comment += $" AdditionalKZ: {mdt.AdditionalKZ}";
                }
                if (!string.IsNullOrEmpty(mdt.AdditionalRU))
                {
                    comment += $" AdditionalRU: {mdt.AdditionalRU}";
                }
                if (!string.IsNullOrEmpty(mdt.DescriptionEN))
                {
                    comment += $" DescriptionEN: {mdt.DescriptionEN} DescriptionKZ: {mdt.DescriptionKZ} DescriptionRU: {mdt.DescriptionRU}";
                }
                comment += $" -> MeteoDataSourceId: {meteoDataType.MeteoDataSourceId} MeteoDataPeriodicityId: {meteoDataType.MeteoDataPeriodicityId} Code: {meteoDataType.Code}";
                if (!string.IsNullOrEmpty(meteoDataType.GroupEN))
                {
                    comment += $" GroupEN: {meteoDataType.GroupEN} GroupKZ: {meteoDataType.GroupKZ} GroupRU: {meteoDataType.GroupRU}";
                }
                comment += $" NameEN: {meteoDataType.NameEN} NameKZ: {meteoDataType.NameKZ} NameRU: {meteoDataType.NameRU}";
                if (!string.IsNullOrEmpty(meteoDataType.AdditionalEN))
                {
                    comment += $" AdditionalEN: {meteoDataType.AdditionalEN}";
                }
                if (!string.IsNullOrEmpty(meteoDataType.AdditionalKZ))
                {
                    comment += $" AdditionalKZ: {meteoDataType.AdditionalKZ}";
                }
                if (!string.IsNullOrEmpty(meteoDataType.AdditionalRU))
                {
                    comment += $" AdditionalRU: {meteoDataType.AdditionalRU}";
                }
                if (!string.IsNullOrEmpty(meteoDataType.DescriptionEN))
                {
                    comment += $" DescriptionEN: {meteoDataType.DescriptionEN} DescriptionKZ: {meteoDataType.DescriptionKZ} DescriptionRU: {meteoDataType.DescriptionRU}";
                }

                SystemLog.New("MeteoDataTypeEdit", comment, null, false);

                return RedirectToAction("Index");
            }

            ViewBag.MeteoDataSourceId = new SelectList(db.MeteoDataSources
                .ToList()
                .OrderBy(m => m.Name), "Id", "Name", meteoDataType.MeteoDataSourceId);
            ViewBag.MeteoDataPeriodicityId = new SelectList(db.MeteoDataPeriodicities
                .ToList()
                .OrderBy(m => m.Name), "Id", "Name", meteoDataType.MeteoDataPeriodicityId);
            return View(meteoDataType);
        }

        // GET: MeteoDataTypes/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MeteoDataType meteoDataType = await db.MeteoDataTypes.
                Where(m => m.Id == id)
                .Include(m => m.MeteoDataSource)
                .Include(m => m.MeteoDataPeriodicity)
                .FirstOrDefaultAsync();
            if (meteoDataType == null)
            {
                return HttpNotFound();
            }
            return View(meteoDataType);
        }

        // POST: MeteoDataTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            MeteoDataType meteoDataType = await db.MeteoDataTypes.FindAsync(id);

            string comment = $"Id: {meteoDataType.Id} MeteoDataSourceId: {meteoDataType.MeteoDataSourceId} MeteoDataPeriodicityId: {meteoDataType.MeteoDataPeriodicityId} Code: {meteoDataType.Code}";
            if (!string.IsNullOrEmpty(meteoDataType.GroupEN))
            {
                comment += $" GroupEN: {meteoDataType.GroupEN} GroupKZ: {meteoDataType.GroupKZ} GroupRU: {meteoDataType.GroupRU}";
            }
            comment += $" NameEN: {meteoDataType.NameEN} NameKZ: {meteoDataType.NameKZ} NameRU: {meteoDataType.NameRU}";
            if (!string.IsNullOrEmpty(meteoDataType.AdditionalEN))
            {
                comment += $" AdditionalEN: {meteoDataType.AdditionalEN} AdditionalKZ: {meteoDataType.AdditionalKZ} AdditionalRU: {meteoDataType.AdditionalRU}";
            }
            if (!string.IsNullOrEmpty(meteoDataType.DescriptionEN))
            {
                comment += $" DescriptionEN: {meteoDataType.DescriptionEN} DescriptionKZ: {meteoDataType.DescriptionKZ} DescriptionRU: {meteoDataType.DescriptionRU}";
            }
            SystemLog.New("MeteoDataTypeDelete", comment, null, false);

            db.MeteoDataTypes.Remove(meteoDataType);
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
