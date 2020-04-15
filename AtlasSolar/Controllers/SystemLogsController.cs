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
    public class SystemLogsController : Controller
    {
        private NpgsqlContext db = new NpgsqlContext();

        // GET: SystemLogs
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Index(string SortOrder, string User, string Operation, string ActionName, int? Page)
        {
            var systemlogs = db.SystemLogs
                .Where(s => true);

            ViewBag.UserFilter = User;
            ViewBag.OperationFilter = Operation;
            ViewBag.ActionNameFilter = ActionName;

            ViewBag.DateTimeSort = SortOrder == "DateTime" ? "DateTimeDesc" : "DateTime";
            ViewBag.UserSort = SortOrder == "User" ? "UserDesc" : "User";
            ViewBag.OperationSort = SortOrder == "Operation" ? "OperationDesc" : "Operation";
            ViewBag.ActionNameSort = SortOrder == "ActionName" ? "ActionNameDesc" : "ActionName";

            if (!string.IsNullOrEmpty(User))
            {
                systemlogs = systemlogs.Where(s => s.User.Contains(User));
            }
            if (!string.IsNullOrEmpty(Operation))
            {
                systemlogs = systemlogs.Where(s => s.Operation.Contains(Operation));
            }
            if (!string.IsNullOrEmpty(ActionName))
            {
                systemlogs = systemlogs.Where(s => s.Action.Contains(ActionName));
            }

            switch (SortOrder)
            {
                case "DateTime":
                    systemlogs = systemlogs.OrderBy(s => s.DateTime);
                    break;
                case "DateTimeDesc":
                    systemlogs = systemlogs.OrderByDescending(s => s.DateTime);
                    break;
                case "User":
                    systemlogs = systemlogs.OrderBy(s => s.User);
                    break;
                case "UserDesc":
                    systemlogs = systemlogs.OrderByDescending(s => s.User);
                    break;
                case "Operation":
                    systemlogs = systemlogs.OrderBy(s => s.Operation);
                    break;
                case "OperationDesc":
                    systemlogs = systemlogs.OrderByDescending(s => s.Operation);
                    break;
                case "ActionName":
                    systemlogs = systemlogs.OrderBy(s => s.Action);
                    break;
                case "ActionNameDesc":
                    systemlogs = systemlogs.OrderByDescending(s => s.Action);
                    break;
                default:
                    systemlogs = systemlogs.OrderBy(s => s.Id);
                    break;
            }

            int PageSize = 50;
            int PageNumber = (Page ?? 1);

            return View(systemlogs.ToPagedList(PageNumber, PageSize));
        }

        // GET: SystemLogs/Details/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SystemLog systemLog = await db.SystemLogs.FindAsync(id);
            if (systemLog == null)
            {
                return HttpNotFound();
            }
            return View(systemLog);
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
