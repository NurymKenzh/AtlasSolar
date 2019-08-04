using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace AtlasSolar.Controllers
{
    public class LanguageController : Controller
    {
        // GET: Language
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Change(string LanguageAbbrevation)
        {
            if (LanguageAbbrevation != null)
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(LanguageAbbrevation);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(LanguageAbbrevation);
            }

            HttpCookie cookie = new HttpCookie("Language");
            cookie.Value = LanguageAbbrevation;
            Response.Cookies.Add(cookie);

            return Redirect(Request.UrlReferrer.ToString());

            //return View("Index");
        }
    }
}