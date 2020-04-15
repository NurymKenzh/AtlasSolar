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
    public class FeedbacksController : Controller
    {
        private NpgsqlContext db = new NpgsqlContext();

        // GET: Feedbacks
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Index(string SortOrder, string Name, string Email, int? Page)
        {
            var feedbacks = db.Feedbacks
                .Where(f => true);

            ViewBag.NameFilter = Name;
            ViewBag.EmailFilter = Email;

            ViewBag.DateTimeSort = SortOrder == "DateTime" ? "DateTimeDesc" : "DateTime";
            ViewBag.NameSort = SortOrder == "Name" ? "NameDesc" : "Name";
            ViewBag.EmailSort = SortOrder == "Email" ? "EmailDesc" : "Email";

            if (!string.IsNullOrEmpty(Name))
            {
                feedbacks = feedbacks.Where(f => f.Name.ToLower().Contains(Name.ToLower()));
            }
            if (!string.IsNullOrEmpty(Email))
            {
                feedbacks = feedbacks.Where(f => f.Email.ToLower().Contains(Email.ToLower()));
            }

            switch (SortOrder)
            {
                case "DateTime":
                    feedbacks = feedbacks.OrderBy(m => m.DateTime);
                    break;
                case "DateTimeDesc":
                    feedbacks = feedbacks.OrderByDescending(m => m.DateTime);
                    break;
                case "Name":
                    feedbacks = feedbacks.OrderBy(m => m.Name);
                    break;
                case "NameDesc":
                    feedbacks = feedbacks.OrderByDescending(m => m.Name);
                    break;
                case "Email":
                    feedbacks = feedbacks.OrderBy(m => m.Email);
                    break;
                case "EmailDesc":
                    feedbacks = feedbacks.OrderByDescending(m => m.Email);
                    break;
                default:
                    feedbacks = feedbacks.OrderByDescending(m => m.DateTime);
                    break;
            }

            int PageSize = 50;
            int PageNumber = (Page ?? 1);

            return View(feedbacks.ToPagedList(PageNumber, PageSize));
        }

        // GET: Feedbacks/Details/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Feedback feedback = await db.Feedbacks.FindAsync(id);
            if (feedback == null)
            {
                return HttpNotFound();
            }
            return View(feedback);
        }

        // GET: Feedbacks/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Feedbacks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,Email,Text")] Feedback feedback)
        {
            if (ModelState.IsValid)
            {
                feedback.DateTime = DateTime.Now;
                db.Feedbacks.Add(feedback);
                await db.SaveChangesAsync();
                return RedirectToAction("Contact", "Home");
            }

            return View(feedback);
        }

        // GET: Feedbacks/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Feedback feedback = await db.Feedbacks.FindAsync(id);
            if (feedback == null)
            {
                return HttpNotFound();
            }
            return View(feedback);
        }

        // POST: Feedbacks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Feedback feedback = await db.Feedbacks.FindAsync(id);
            db.Feedbacks.Remove(feedback);
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
