using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AtlasSolar.Controllers
{
    public class ModeratorController : Controller
    {
        // GET: Moderator
        [Authorize(Roles = "Moderator")]
        public ActionResult Index()
        {
            return View();
        }
    }
}