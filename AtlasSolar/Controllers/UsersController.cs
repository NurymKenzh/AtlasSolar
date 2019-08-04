using AtlasSolar.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AtlasSolar.Controllers
{
    public class UsersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

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

        // GET: Users
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            IList<UserViewModel> users = db.Users
                .ToList()
                .Select(u => new UserViewModel() { Id = u.Id, Email = u.Email, Roles = new List<string>() { } })
                .Where(u => true)
                .ToList();

            for (int i = 0; i < users.Count; i++)
            {
                users[i].Roles = new List<string>() { };
                foreach (var role in db.Roles)
                {
                    if (UserManager.IsInRole(users[i].Id, role.Name))
                    {
                        users[i].Roles.Add(role.Name);
                    }
                }
            }

            return View(users);
        }

        // GET
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit(string id)
        {
            if ((id == null) || (id == ""))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var User = await UserManager.FindByIdAsync(id);

            if (User == null)
            {
                return HttpNotFound();
            }
            UserViewModel user = new UserViewModel() { Id = id, Roles = new List<string>() { }, Email = User.Email };
            foreach (var role in db.Roles)
            {
                if (UserManager.IsInRole(user.Id, role.Name))
                {
                    user.Roles.Add(role.Name);
                }
            }
            ViewBag.Roles = db.Roles
                .Select(r => r.Name)
                .ToList();
            return View(user);
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit([Bind(Include = "Id")] UserViewModel user, string[] Roles)
        {
            if (ModelState.IsValid)
            {
                ApplicationDbContext context = new ApplicationDbContext();
                var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(context));
                foreach (var role in db.Roles)
                {
                    if (UserManager.IsInRole(user.Id, role.Name))
                    {
                        UserManager.RemoveFromRole(user.Id, role.Name);
                    }
                }
                foreach (var role in Roles)
                {
                    userManager.AddToRole(user.Id, role);
                }
                return RedirectToAction("Index");
            }

            user.Roles = new List<string>() { };
            foreach (var role in db.Roles)
            {
                if (UserManager.IsInRole(user.Id, role.Name))
                {
                    user.Roles.Add(role.Name);
                }
            }
            ViewBag.Roles = db.Roles
                .Select(r => r.Name)
                .ToList();
            return View(user);
        }
    }
}