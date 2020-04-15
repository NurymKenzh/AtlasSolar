using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AtlasSolar.Controllers
{
    public class HelpController : Controller
    {
        public ActionResult CalcEfficiency()
        {
            return View();
        }

        public ActionResult CalcEnergy()
        {
            return View();
        }
    }
}
