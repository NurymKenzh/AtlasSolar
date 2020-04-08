using AtlasSolar.Models;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.CodeDom.Providers.DotNetCompilerPlatform;
using Newtonsoft.Json.Linq;
using OSGeo.GDAL;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace AtlasSolar.Controllers
{
    public class MapsController : Controller
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

        struct PlotData
        {
            public decimal X { get; set; }
            public decimal Y { get; set; }
        }

        struct PlotDataS
        {
            public string X { get; set; }
            public decimal Y { get; set; }
        }

        // GET: Maps
        public ActionResult Demo(string map)
        {
            //List<SelectListItem> PVOrientations = new List<SelectListItem>()
            //{
            //    new SelectListItem() {Text="-Следящая по двум осям-", Value="1"}
            //};
            //ViewBag.PVOrientations = PVOrientations;
            ViewBag.PVSystemMaterials = new SelectList(db.PVSystemMaterials
                .ToList()
                //.OrderBy(p => p.Name)
                .OrderBy(p =>
                    p.NameEN.Contains("Mono") ? 1 :
                    p.NameEN.Contains("Poly") ? 2 :
                    p.NameEN.Contains("Amorp") ? 3 :
                    p.NameEN.Contains("CdTe ") ? 4 :
                    p.NameEN.Contains("CIGS  ") ? 5 : 6)
                , "Id", "Name");
            
            HttpCookie cookie = System.Web.HttpContext.Current.Request.Cookies["Language"];
            ViewBag.Language = "en";
            if (cookie != null)
            {
                if (cookie.Value != null)
                {
                    ViewBag.Language = cookie.Value;
                }
            }

            List<SelectListItem> Languages = new List<SelectListItem>()
            {
                new SelectListItem() {Text="English", Value="en", Selected=ViewBag.Language=="en"},
                new SelectListItem() {Text="Қазақ", Value="kk", Selected=ViewBag.Language=="kk"},
                new SelectListItem() {Text="Русский", Value="ru", Selected=ViewBag.Language=="ru"}
            };
            ViewBag.Languages = Languages;

            IList<MeteoDataPeriodicity> meteodataperiodicities = db.MeteoDataPeriodicities
                .ToList();
            meteodataperiodicities = meteodataperiodicities
                .OrderBy(m => m.Name)
                .ToList();
            IList<MeteoDataSource> meteodatasources = db.MeteoDataSources
                //.Where(m => true)
                .ToList();
            meteodatasources = meteodatasources
                .OrderBy(m => m.Name)
                .ToList();
            ViewBag.MeteoDataPeriodicities = new SelectList(meteodataperiodicities, "Id", "Name");
            ViewBag.MeteoDataSources = new SelectList(meteodatasources, "Id", "Name");

            IList<SPPStatus> sppstatuses = db.SPPStatus
                .ToList();
            sppstatuses = sppstatuses
                .OrderBy(s => s.Name)
                .ToList();
            ViewBag.SPPStatuses = new SelectList(sppstatuses, "Id", "Name");
            IList<SPPPurpose> spppurpose = db.SPPPurposes
                .ToList();
            spppurpose = spppurpose
                .OrderBy(s => s.Name)
                .ToList();
            ViewBag.SPPPurposes = new SelectList(spppurpose, "Id", "Name", spppurpose.FirstOrDefault(s => s.NameEN == "Autonomous"));
            IList<PanelOrientation> panelorientations = db.PanelOrientations
                .ToList();
            panelorientations = panelorientations
                //.OrderBy(p => p.Name)
                .OrderBy(p =>
                    p.Code == Properties.Settings.Default.PanelOriantationFixedCode ? 1 :
                    p.Code == Properties.Settings.Default.PanelOriantationFixedCorrectableCode ? 2 :
                    p.Code == Properties.Settings.Default.PanelOriantationVerticalCode ? 3 :
                    p.Code == Properties.Settings.Default.PanelOriantationHorizontalCode ? 4 :
                    p.Code == Properties.Settings.Default.PanelOriantation2AxisCode ? 5 : 6)
                .ToList();
            ViewBag.PanelOrientations = new SelectList(panelorientations, "Id", "Name");

            IList<PanelOrientation> panelorientationswithcode = db.PanelOrientations
                .ToList();
            panelorientationswithcode = panelorientations
                .Where(p => !string.IsNullOrEmpty(p.Code))
                //.OrderBy(p => p.Name)
                .OrderBy(p =>
                    p.Code == Properties.Settings.Default.PanelOriantationFixedCode ? 1 :
                    p.Code == Properties.Settings.Default.PanelOriantationFixedCorrectableCode ? 2 :
                    p.Code == Properties.Settings.Default.PanelOriantationVerticalCode ? 3 :
                    p.Code == Properties.Settings.Default.PanelOriantationHorizontalCode ? 4 :
                    p.Code == Properties.Settings.Default.PanelOriantation2AxisCode ? 5 : 6)
                .ToList();
            ViewBag.PanelOrientationsWithCode = new SelectList(panelorientationswithcode, "Id", "Name");

            string decimaldelimiter = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            List<SPP> spps = db.SPPs.ToList();
            JObject o = JObject.FromObject(new
            {
                type = "FeatureCollection",
                crs = new
                {
                    type = "name",
                    properties = new
                    {
                        name = "urn:ogc:def:crs:EPSG::3857"
                    }
                },
                features = from spp in spps
                           select new
                           {
                               type = "Feature",
                               properties = new
                               {
                                   Id = spp.Id,
                                   Name = spp.Name,
                                   Count = spp.Count.ToString(),
                                   Power = spp.Power.ToString(),
                                   Cost = spp.Cost,
                                   Startup = spp.Startup.ToString(),
                                   Link = spp.Link,
                                   Customer = spp.Customer,
                                   Investor = spp.Investor,
                                   Executor = spp.Executor,
                                   CapacityFactor = spp.CapacityFactor.ToString(),
                                   Coordinates = spp.Coordinates,
                                   SPPStatusId = spp.SPPStatusId,
                                   SPPPurposeId = spp.SPPPurposeId,
                                   PanelOrientationId = spp.PanelOrientationId
                               },
                               geometry = new
                               {
                                   type = "Point",
                                   coordinates = new List<decimal>
                                {
                                    Convert.ToDecimal(spp.Coordinates.Split(',')[0].Replace(".",decimaldelimiter)),
                                    Convert.ToDecimal(spp.Coordinates.Split(',')[1].Replace(".",decimaldelimiter))
                                },
                               }
                           }
            });
            ViewBag.SPPLayerJson = o.ToString();

            ViewBag.Role = "";
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.Role = "User";
                string CurrentUserId = User.Identity.GetUserId();
                if (UserManager.IsInRole(CurrentUserId, "Moderator"))
                {
                    ViewBag.Role = "Moderator";
                }
                if (UserManager.IsInRole(CurrentUserId, "Admin"))
                {
                    ViewBag.Role = "Admin";
                }
            }

            //IList<Appliance> A_lamps = db.Appliances
            //    .Include(a => a.ApplianceType)
            //    .Where(a => a.ApplianceType.NameEN.ToLower().Contains("lamp"))
            //    .ToList();
            //ViewBag.ALamps = A_lamps;
            IList<Appliance> Appliances = db.Appliances
                .Include(a => a.ApplianceType)
                .ToList()
                .OrderBy(a => a.ApplianceType.Name)
                .ToList();
            ViewBag.Appliances = new SelectList(Appliances, "Id", "NamePower");

            List<SelectListItem> MapSources = new List<SelectListItem>();
            var MapSourcesData = new[]{
                    new SelectListItem{ Value="OpenStreetMap",Text="Open Street Map", Selected = true},
                    new SelectListItem{ Value="ArcGIS",Text="ArcGIS"},
                    new SelectListItem{ Value="Bing",Text="Bing"},
                    new SelectListItem{ Value="BingAerial",Text="Bing Aerial"},
                    new SelectListItem{ Value="Stamen",Text="Stamen"}
                    };
            MapSources = MapSourcesData.OrderBy(s => s.Text).ToList();
            ViewBag.MapSources = MapSources;

            List<SelectListItem> Importances = new List<SelectListItem>()
            {
                new SelectListItem() {Text=Resources.Common.disregard, Value="0"},
                new SelectListItem() {Text=Resources.Common.notVeryImportant, Value="25"},
                new SelectListItem() {Text=Resources.Common.important, Value="50"},
                new SelectListItem() {Text=Resources.Common.veryImportant, Value="75"},
                new SelectListItem() {Text=Resources.Common.criticallyImportant, Value="100"},
            };
            ViewBag.Importances = Importances;

            IList<Leprng> leprngs = db.Leprngs
                .OrderBy(l => l.Code)
                .ToList();
            ViewBag.Leprngs = new SelectList(leprngs, "Code", "Name");
            IList<Lepinsttp> lepinsttps = db.Lepinsttps
                .OrderBy(l => l.Code)
                .ToList();
            ViewBag.Lepinsttps = new SelectList(lepinsttps, "Code", "Name");
            IList<Lepmtrl> lepmtrls = db.Lepmtrls
                .OrderBy(l => l.Code)
                .ToList();
            ViewBag.Lepmtrls = new SelectList(lepmtrls, "Code", "Name");
            IList<Leprnzon> leprnzons = db.Leprnzons
                .OrderBy(l => l.Code)
                .ToList();
            ViewBag.Leprnzons = new SelectList(leprnzons, "Code", "Name");
            IList<Leptype> leptypes = db.Leptypes
                .OrderBy(l => l.Code)
                .ToList();
            ViewBag.Leptypes = new SelectList(leptypes, "Code", "Name");

            ViewBag.Map = map;

            MapDescription md_avg_dnr = db.MapDescriptions
                .Where(m => m.LayersCode == "avg_dnr")
                .FirstOrDefault();
            ViewBag.Name_avg_dnr = md_avg_dnr.Name;
            ViewBag.Description_avg_dnr = md_avg_dnr.Description;
            ViewBag.Appointment_avg_dnr = md_avg_dnr.Appointment;
            ViewBag.Resolution_avg_dnr = md_avg_dnr.Resolution;
            ViewBag.Source_avg_dnr = md_avg_dnr.Source;
            ViewBag.Additional_avg_dnr = md_avg_dnr.Additional;

            MapDescription md_swv_dwn = db.MapDescriptions
                .Where(m => m.LayersCode == "swv_dwn")
                .FirstOrDefault();
            ViewBag.Name_swv_dwn = md_swv_dwn.Name;
            ViewBag.Description_swv_dwn = md_swv_dwn.Description;
            ViewBag.Appointment_swv_dwn = md_swv_dwn.Appointment;
            ViewBag.Resolution_swv_dwn = md_swv_dwn.Resolution;
            ViewBag.Source_swv_dwn = md_swv_dwn.Source;
            ViewBag.Additional_swv_dwn = md_swv_dwn.Additional;

            MapDescription md_exp_dif = db.MapDescriptions
                .Where(m => m.LayersCode == "exp_dif")
                .FirstOrDefault();
            ViewBag.Name_exp_dif = md_exp_dif.Name;
            ViewBag.Description_exp_dif = md_exp_dif.Description;
            ViewBag.Appointment_exp_dif = md_exp_dif.Appointment;
            ViewBag.Resolution_exp_dif = md_exp_dif.Resolution;
            ViewBag.Source_exp_dif = md_exp_dif.Source;
            ViewBag.Additional_exp_dif = md_exp_dif.Additional;

            MapDescription md_dni = db.MapDescriptions
                .Where(m => m.LayersCode == "dni")
                .FirstOrDefault();
            ViewBag.Name_dni = md_dni.Name;
            ViewBag.Description_dni = md_dni.Description;
            ViewBag.Appointment_dni = md_dni.Appointment;
            ViewBag.Resolution_dni = md_dni.Resolution;
            ViewBag.Source_dni = md_dni.Source;
            ViewBag.Additional_dni = md_dni.Additional;

            MapDescription md_sic = db.MapDescriptions
                .Where(m => m.LayersCode == "sic")
                .FirstOrDefault();
            ViewBag.Name_sic = md_sic.Name;
            ViewBag.Description_sic = md_sic.Description;
            ViewBag.Appointment_sic = md_sic.Appointment;
            ViewBag.Resolution_sic = md_sic.Resolution;
            ViewBag.Source_sic = md_sic.Source;
            ViewBag.Additional_sic = md_sic.Additional;

            MapDescription md_sid = db.MapDescriptions
                .Where(m => m.LayersCode == "sid")
                .FirstOrDefault();
            ViewBag.Name_sid = md_sid.Name;
            ViewBag.Description_sid = md_sid.Description;
            ViewBag.Appointment_sid = md_sid.Appointment;
            ViewBag.Resolution_sid = md_sid.Resolution;
            ViewBag.Source_sid = md_sid.Source;
            ViewBag.Additional_sid = md_sid.Additional;

            MapDescription md_sis = db.MapDescriptions
                .Where(m => m.LayersCode == "sis")
                .FirstOrDefault();
            ViewBag.Name_sis = md_sis.Name;
            ViewBag.Description_sis = md_sis.Description;
            ViewBag.Appointment_sis = md_sis.Appointment;
            ViewBag.Resolution_sis = md_sis.Resolution;
            ViewBag.Source_sis = md_sis.Source;
            ViewBag.Additional_sis = md_sis.Additional;

            MapDescription md_sis_klr = db.MapDescriptions
                .Where(m => m.LayersCode == "sis_klr")
                .FirstOrDefault();
            ViewBag.Name_sis_klr = md_sis_klr.Name;
            ViewBag.Description_sis_klr = md_sis_klr.Description;
            ViewBag.Appointment_sis_klr = md_sis_klr.Appointment;
            ViewBag.Resolution_sis_klr = md_sis_klr.Resolution;
            ViewBag.Source_sis_klr = md_sis_klr.Source;
            ViewBag.Additional_sis_klr = md_sis_klr.Additional;

            MapDescription md_t10m = db.MapDescriptions
                .Where(m => m.LayersCode == "t10m")
                .FirstOrDefault();
            ViewBag.Name_t10m = md_t10m.Name;
            ViewBag.Description_t10m = md_t10m.Description;
            ViewBag.Appointment_t10m = md_t10m.Appointment;
            ViewBag.Resolution_t10m = md_t10m.Resolution;
            ViewBag.Source_t10m = md_t10m.Source;
            ViewBag.Additional_t10m = md_t10m.Additional;

            MapDescription md_lep = db.MapDescriptions
                .Where(m => m.LayersCode == "lep")
                .FirstOrDefault();
            ViewBag.Name_lep = md_lep.Name;
            ViewBag.Description_lep = md_lep.Description;
            ViewBag.Appointment_lep = md_lep.Appointment;
            ViewBag.Resolution_lep = md_lep.Resolution;
            ViewBag.Source_lep = md_lep.Source;
            ViewBag.Additional_lep = md_lep.Additional;

            MapDescription md_srtm = db.MapDescriptions
                .Where(m => m.LayersCode == "srtm")
                .FirstOrDefault();
            ViewBag.Name_srtm = md_srtm.Name;
            ViewBag.Description_srtm = md_srtm.Description;
            ViewBag.Appointment_srtm = md_srtm.Appointment;
            ViewBag.Resolution_srtm = md_srtm.Resolution;
            ViewBag.Source_srtm = md_srtm.Source;
            ViewBag.Additional_srtm = md_srtm.Additional;

            MapDescription md_aspect_srtm = db.MapDescriptions
                .Where(m => m.LayersCode == "aspect_srtm")
                .FirstOrDefault();
            ViewBag.Name_aspect_srtm = md_aspect_srtm.Name;
            ViewBag.Description_aspect_srtm = md_aspect_srtm.Description;
            ViewBag.Appointment_aspect_srtm = md_aspect_srtm.Appointment;
            ViewBag.Resolution_aspect_srtm = md_aspect_srtm.Resolution;
            ViewBag.Source_aspect_srtm = md_aspect_srtm.Source;
            ViewBag.Additional_aspect_srtm = md_aspect_srtm.Additional;

            MapDescription md_slope_srtm = db.MapDescriptions
                .Where(m => m.LayersCode == "slope_srtm")
                .FirstOrDefault();
            ViewBag.Name_slope_srtm = md_slope_srtm.Name;
            ViewBag.Description_slope_srtm = md_slope_srtm.Description;
            ViewBag.Appointment_slope_srtm = md_slope_srtm.Appointment;
            ViewBag.Resolution_slope_srtm = md_slope_srtm.Resolution;
            ViewBag.Source_slope_srtm = md_slope_srtm.Source;
            ViewBag.Additional_slope_srtm = md_slope_srtm.Additional;

            MapDescription md_pamyatnikprirodypol = db.MapDescriptions
                .Where(m => m.LayersCode == "pamyatnikprirodypol")
                .FirstOrDefault();
            ViewBag.Name_pamyatnikprirodypol = md_pamyatnikprirodypol.Name;
            ViewBag.Description_pamyatnikprirodypol = md_pamyatnikprirodypol.Description;
            ViewBag.Appointment_pamyatnikprirodypol = md_pamyatnikprirodypol.Appointment;
            ViewBag.Resolution_pamyatnikprirodypol = md_pamyatnikprirodypol.Resolution;
            ViewBag.Source_pamyatnikprirodypol = md_pamyatnikprirodypol.Source;
            ViewBag.Additional_pamyatnikprirodypol = md_pamyatnikprirodypol.Additional;

            MapDescription md_prirparki = db.MapDescriptions
                .Where(m => m.LayersCode == "prirparki")
                .FirstOrDefault();
            ViewBag.Name_prirparki = md_prirparki.Name;
            ViewBag.Description_prirparki = md_prirparki.Description;
            ViewBag.Appointment_prirparki = md_prirparki.Appointment;
            ViewBag.Resolution_prirparki = md_prirparki.Resolution;
            ViewBag.Source_prirparki = md_prirparki.Source;
            ViewBag.Additional_prirparki = md_prirparki.Additional;

            MapDescription md_zakazniky = db.MapDescriptions
                .Where(m => m.LayersCode == "zakazniky")
                .FirstOrDefault();
            ViewBag.Name_zakazniky = md_zakazniky.Name;
            ViewBag.Description_zakazniky = md_zakazniky.Description;
            ViewBag.Appointment_zakazniky = md_zakazniky.Appointment;
            ViewBag.Resolution_zakazniky = md_zakazniky.Resolution;
            ViewBag.Source_zakazniky = md_zakazniky.Source;
            ViewBag.Additional_zakazniky = md_zakazniky.Additional;

            MapDescription md_rezervaty = db.MapDescriptions
                .Where(m => m.LayersCode == "rezervaty")
                .FirstOrDefault();
            ViewBag.Name_rezervaty = md_rezervaty.Name;
            ViewBag.Description_rezervaty = md_rezervaty.Description;
            ViewBag.Appointment_rezervaty = md_rezervaty.Appointment;
            ViewBag.Resolution_rezervaty = md_rezervaty.Resolution;
            ViewBag.Source_rezervaty = md_rezervaty.Source;
            ViewBag.Additional_rezervaty = md_rezervaty.Additional;

            MapDescription md_zapovedniki = db.MapDescriptions
                .Where(m => m.LayersCode == "zapovedniki")
                .FirstOrDefault();
            ViewBag.Name_zapovedniki = md_zapovedniki.Name;
            ViewBag.Description_zapovedniki = md_zapovedniki.Description;
            ViewBag.Appointment_zapovedniki = md_zapovedniki.Appointment;
            ViewBag.Resolution_zapovedniki = md_zapovedniki.Resolution;
            ViewBag.Source_zapovedniki = md_zapovedniki.Source;
            ViewBag.Additional_zapovedniki = md_zapovedniki.Additional;

            MapDescription md_zapovedzony = db.MapDescriptions
                .Where(m => m.LayersCode == "zapovedzony")
                .FirstOrDefault();
            ViewBag.Name_zapovedzony = md_zapovedzony.Name;
            ViewBag.Description_zapovedzony = md_zapovedzony.Description;
            ViewBag.Appointment_zapovedzony = md_zapovedzony.Appointment;
            ViewBag.Resolution_zapovedzony = md_zapovedzony.Resolution;
            ViewBag.Source_zapovedzony = md_zapovedzony.Source;
            ViewBag.Additional_zapovedzony = md_zapovedzony.Additional;

            MapDescription md_hidroohrzony = db.MapDescriptions
                .Where(m => m.LayersCode == "hidroohrzony")
                .FirstOrDefault();
            ViewBag.Name_hidroohrzony = md_hidroohrzony.Name;
            ViewBag.Description_hidroohrzony = md_hidroohrzony.Description;
            ViewBag.Appointment_hidroohrzony = md_hidroohrzony.Appointment;
            ViewBag.Resolution_hidroohrzony = md_hidroohrzony.Resolution;
            ViewBag.Source_hidroohrzony = md_hidroohrzony.Source;
            ViewBag.Additional_hidroohrzony = md_hidroohrzony.Additional;

            MapDescription md_arheopamyat = db.MapDescriptions
                .Where(m => m.LayersCode == "arheopamyat")
                .FirstOrDefault();
            ViewBag.Name_arheopamyat = md_arheopamyat.Name;
            ViewBag.Description_arheopamyat = md_arheopamyat.Description;
            ViewBag.Appointment_arheopamyat = md_arheopamyat.Appointment;
            ViewBag.Resolution_arheopamyat = md_arheopamyat.Resolution;
            ViewBag.Source_arheopamyat = md_arheopamyat.Source;
            ViewBag.Additional_arheopamyat = md_arheopamyat.Additional;

            MapDescription md_kzcover = db.MapDescriptions
                .Where(m => m.LayersCode == "kzcover")
                .FirstOrDefault();
            ViewBag.Name_kzcover = md_kzcover.Name;
            ViewBag.Description_kzcover = md_kzcover.Description;
            ViewBag.Appointment_kzcover = md_kzcover.Appointment;
            ViewBag.Resolution_kzcover = md_kzcover.Resolution;
            ViewBag.Source_kzcover = md_kzcover.Source;
            ViewBag.Additional_kzcover = md_kzcover.Additional;

            MapDescription md_rettlt0opt = db.MapDescriptions
                .Where(m => m.LayersCode == "rettlt0opt")
                .FirstOrDefault();
            ViewBag.Name_rettlt0opt = md_rettlt0opt.Name;
            ViewBag.Description_rettlt0opt = md_rettlt0opt.Description;
            ViewBag.Appointment_rettlt0opt = md_rettlt0opt.Appointment;
            ViewBag.Resolution_rettlt0opt = md_rettlt0opt.Resolution;
            ViewBag.Source_rettlt0opt = md_rettlt0opt.Source;
            ViewBag.Additional_rettlt0opt = md_rettlt0opt.Additional;

            MapDescription md_clrskyavrg = db.MapDescriptions
                .Where(m => m.LayersCode == "clrskyavrg")
                .FirstOrDefault();
            ViewBag.Name_clrskyavrg = md_clrskyavrg.Name;
            ViewBag.Description_clrskyavrg = md_clrskyavrg.Description;
            ViewBag.Appointment_clrskyavrg = md_clrskyavrg.Appointment;
            ViewBag.Resolution_clrskyavrg = md_clrskyavrg.Resolution;
            ViewBag.Source_clrskyavrg = md_clrskyavrg.Source;
            ViewBag.Additional_clrskyavrg = md_clrskyavrg.Additional;

            MapDescription md_retesh0mim = db.MapDescriptions
                .Where(m => m.LayersCode == "retesh0mim")
                .FirstOrDefault();
            ViewBag.Name_retesh0mim = md_retesh0mim.Name;
            ViewBag.Description_retesh0mim = md_retesh0mim.Description;
            ViewBag.Appointment_retesh0mim = md_retesh0mim.Appointment;
            ViewBag.Resolution_retesh0mim = md_retesh0mim.Resolution;
            ViewBag.Source_retesh0mim = md_retesh0mim.Source;
            ViewBag.Additional_retesh0mim = md_retesh0mim.Additional;

            MapDescription md_rainavgesm = db.MapDescriptions
                .Where(m => m.LayersCode == "rainavgesm")
                .FirstOrDefault();
            ViewBag.Name_rainavgesm = md_rainavgesm.Name;
            ViewBag.Description_rainavgesm = md_rainavgesm.Description;
            ViewBag.Appointment_rainavgesm = md_rainavgesm.Appointment;
            ViewBag.Resolution_rainavgesm = md_rainavgesm.Resolution;
            ViewBag.Source_rainavgesm = md_rainavgesm.Source;
            ViewBag.Additional_rainavgesm = md_rainavgesm.Additional;

            MapDescription md_t10mmax = db.MapDescriptions
                .Where(m => m.LayersCode == "t10mmax")
                .FirstOrDefault();
            ViewBag.Name_t10mmax = md_t10mmax.Name;
            ViewBag.Description_t10mmax = md_t10mmax.Description;
            ViewBag.Appointment_t10mmax = md_t10mmax.Appointment;
            ViewBag.Resolution_t10mmax = md_t10mmax.Resolution;
            ViewBag.Source_t10mmax = md_t10mmax.Source;
            ViewBag.Additional_t10mmax = md_t10mmax.Additional;

            MapDescription md_t10m_min = db.MapDescriptions
                .Where(m => m.LayersCode == "t10m_min")
                .FirstOrDefault();
            ViewBag.Name_t10m_min = md_t10m_min.Name;
            ViewBag.Description_t10m_min = md_t10m_min.Description;
            ViewBag.Appointment_t10m_min = md_t10m_min.Appointment;
            ViewBag.Resolution_t10m_min = md_t10m_min.Resolution;
            ViewBag.Source_t10m_min = md_t10m_min.Source;
            ViewBag.Additional_t10m_min = md_t10m_min.Additional;

            MapDescription md_tskinavg = db.MapDescriptions
                .Where(m => m.LayersCode == "tskinavg")
                .FirstOrDefault();
            ViewBag.Name_tskinavg = md_tskinavg.Name;
            ViewBag.Description_tskinavg = md_tskinavg.Description;
            ViewBag.Appointment_tskinavg = md_tskinavg.Appointment;
            ViewBag.Resolution_tskinavg = md_tskinavg.Resolution;
            ViewBag.Source_tskinavg = md_tskinavg.Source;
            ViewBag.Additional_tskinavg = md_tskinavg.Additional;

            MapDescription md_srfalbavg = db.MapDescriptions
                .Where(m => m.LayersCode == "srfalbavg")
                .FirstOrDefault();
            ViewBag.Name_srfalbavg = md_srfalbavg.Name;
            ViewBag.Description_srfalbavg = md_srfalbavg.Description;
            ViewBag.Appointment_srfalbavg = md_srfalbavg.Appointment;
            ViewBag.Resolution_srfalbavg = md_srfalbavg.Resolution;
            ViewBag.Source_srfalbavg = md_srfalbavg.Source;
            ViewBag.Additional_srfalbavg = md_srfalbavg.Additional;

            MapDescription md_avg_kt_22 = db.MapDescriptions
                .Where(m => m.LayersCode == "avg_kt_22")
                .FirstOrDefault();
            ViewBag.Name_avg_kt_22 = md_avg_kt_22.Name;
            ViewBag.Description_avg_kt_22 = md_avg_kt_22.Description;
            ViewBag.Appointment_avg_kt_22 = md_avg_kt_22.Appointment;
            ViewBag.Resolution_avg_kt_22 = md_avg_kt_22.Resolution;
            ViewBag.Source_avg_kt_22 = md_avg_kt_22.Source;
            ViewBag.Additional_avg_kt_22 = md_avg_kt_22.Additional;

            MapDescription md_avg_nkt_22 = db.MapDescriptions
                .Where(m => m.LayersCode == "avg_nkt_22")
                .FirstOrDefault();
            ViewBag.Name_avg_nkt_22 = md_avg_nkt_22.Name;
            ViewBag.Description_avg_nkt_22 = md_avg_nkt_22.Description;
            ViewBag.Appointment_avg_nkt_22 = md_avg_nkt_22.Appointment;
            ViewBag.Resolution_avg_nkt_22 = md_avg_nkt_22.Resolution;
            ViewBag.Source_avg_nkt_22 = md_avg_nkt_22.Source;
            ViewBag.Additional_avg_nkt_22 = md_avg_nkt_22.Additional;

            MapDescription md_day_cld_22 = db.MapDescriptions
                .Where(m => m.LayersCode == "day_cld_22")
                .FirstOrDefault();
            ViewBag.Name_day_cld_22 = md_day_cld_22.Name;
            ViewBag.Description_day_cld_22 = md_day_cld_22.Description;
            ViewBag.Appointment_day_cld_22 = md_day_cld_22.Appointment;
            ViewBag.Resolution_day_cld_22 = md_day_cld_22.Resolution;
            ViewBag.Source_day_cld_22 = md_day_cld_22.Source;
            ViewBag.Additional_day_cld_22 = md_day_cld_22.Additional;

            MapDescription md_daylghtav = db.MapDescriptions
                .Where(m => m.LayersCode == "daylghtav")
                .FirstOrDefault();
            ViewBag.Name_daylghtav = md_daylghtav.Name;
            ViewBag.Description_daylghtav = md_daylghtav.Description;
            ViewBag.Appointment_daylghtav = md_daylghtav.Appointment;
            ViewBag.Resolution_daylghtav = md_daylghtav.Resolution;
            ViewBag.Source_daylghtav = md_daylghtav.Source;
            ViewBag.Additional_daylghtav = md_daylghtav.Additional;

            //// delete get max, min of provinces
            //var provinces = db.Provinces
            //    .Where(p => true);
            //GdalConfiguration.ConfigureGdal();
            //foreach (Province province in provinces)
            //{
            //    //string auto_dist_file_name = Path.Combine(Properties.Settings.Default.GeoServerPath + @"\data_dir\coverages\AtlasSolar\Provinces", province.Code + "auto_dist.tif");
            //    //Dataset auto_dist_ds = Gdal.Open(auto_dist_file_name, Access.GA_ReadOnly);
            //    //Band auto_dist_band = auto_dist_ds.GetRasterBand(1);
            //    //int auto_dist_width = auto_dist_band.XSize;
            //    //int auto_dist_height = auto_dist_band.YSize;
            //    //float[] auto_dist_array = new float[auto_dist_width * auto_dist_height];
            //    //auto_dist_band.ReadRaster(0, 0, auto_dist_width, auto_dist_height, auto_dist_array, auto_dist_width, auto_dist_height, 0, 0);
            //    //double auto_dist_out_val;
            //    //int auto_dist_out_hasval;
            //    //float auto_dist_NoDataValue = -1;
            //    //auto_dist_band.GetNoDataValue(out auto_dist_out_val, out auto_dist_out_hasval);
            //    //if (auto_dist_out_hasval != 0)
            //    //    auto_dist_NoDataValue = (float)auto_dist_out_val;
            //    //float auto_dist_minimum = float.MaxValue,
            //    //    auto_dist_maximum = auto_dist_array.Max();
            //    //foreach(float v in auto_dist_array)
            //    //{
            //    //    if(v != auto_dist_NoDataValue && v < auto_dist_minimum)
            //    //    {
            //    //        auto_dist_minimum = v;
            //    //    }
            //    //}
            //    //province.Max_auto_dist = (decimal?)auto_dist_maximum;
            //    //province.Min_auto_dist = (decimal?)auto_dist_minimum;

            //    //string lep_dist_file_name = Path.Combine(Properties.Settings.Default.GeoServerPath + @"\data_dir\coverages\AtlasSolar\Provinces", province.Code + "lep_dist.tif");
            //    //Dataset lep_dist_ds = Gdal.Open(lep_dist_file_name, Access.GA_ReadOnly);
            //    //Band lep_dist_band = lep_dist_ds.GetRasterBand(1);
            //    //int lep_dist_width = lep_dist_band.XSize;
            //    //int lep_dist_height = lep_dist_band.YSize;
            //    //float[] lep_dist_array = new float[lep_dist_width * lep_dist_height];
            //    //lep_dist_band.ReadRaster(0, 0, lep_dist_width, lep_dist_height, lep_dist_array, lep_dist_width, lep_dist_height, 0, 0);
            //    //double lep_dist_out_val;
            //    //int lep_dist_out_hasval;
            //    //float lep_dist_NoDataValue = -1;
            //    //lep_dist_band.GetNoDataValue(out lep_dist_out_val, out lep_dist_out_hasval);
            //    //if (lep_dist_out_hasval != 0)
            //    //    lep_dist_NoDataValue = (float)lep_dist_out_val;
            //    //float lep_dist_minimum = float.MaxValue,
            //    //    lep_dist_maximum = lep_dist_array.Max();
            //    //foreach (float v in lep_dist_array)
            //    //{
            //    //    if (v != lep_dist_NoDataValue && v < lep_dist_minimum)
            //    //    {
            //    //        lep_dist_minimum = v;
            //    //    }
            //    //}
            //    //province.Max_lep_dist = (decimal?)lep_dist_maximum;
            //    //province.Min_lep_dist = (decimal?)lep_dist_minimum;

            //    //string np_dist_file_name = Path.Combine(Properties.Settings.Default.GeoServerPath + @"\data_dir\coverages\AtlasSolar\Provinces", province.Code + "np_dist.tif");
            //    //Dataset np_dist_ds = Gdal.Open(np_dist_file_name, Access.GA_ReadOnly);
            //    //Band np_dist_band = np_dist_ds.GetRasterBand(1);
            //    //int np_dist_width = np_dist_band.XSize;
            //    //int np_dist_height = np_dist_band.YSize;
            //    //float[] np_dist_array = new float[np_dist_width * np_dist_height];
            //    //np_dist_band.ReadRaster(0, 0, np_dist_width, np_dist_height, np_dist_array, np_dist_width, np_dist_height, 0, 0);
            //    //double np_dist_out_val;
            //    //int np_dist_out_hasval;
            //    //float np_dist_NoDataValue = -1;
            //    //np_dist_band.GetNoDataValue(out np_dist_out_val, out np_dist_out_hasval);
            //    //if (np_dist_out_hasval != 0)
            //    //    np_dist_NoDataValue = (float)np_dist_out_val;
            //    //float np_dist_minimum = float.MaxValue,
            //    //    np_dist_maximum = np_dist_array.Max();
            //    //foreach (float v in np_dist_array)
            //    //{
            //    //    if (v != np_dist_NoDataValue && v < np_dist_minimum)
            //    //    {
            //    //        np_dist_minimum = v;
            //    //    }
            //    //}
            //    //province.Max_np_dist = (decimal?)np_dist_maximum;
            //    //province.Min_np_dist = (decimal?)np_dist_minimum;

            //    //string slope_srtm_file_name = Path.Combine(Properties.Settings.Default.GeoServerPath + @"\data_dir\coverages\AtlasSolar\Provinces", province.Code + "slope_srtm.tif");
            //    //Dataset slope_srtm_ds = Gdal.Open(slope_srtm_file_name, Access.GA_ReadOnly);
            //    //Band slope_srtm_band = slope_srtm_ds.GetRasterBand(1);
            //    //int slope_srtm_width = slope_srtm_band.XSize;
            //    //int slope_srtm_height = slope_srtm_band.YSize;
            //    //float[] slope_srtm_array = new float[slope_srtm_width * slope_srtm_height];
            //    //slope_srtm_band.ReadRaster(0, 0, slope_srtm_width, slope_srtm_height, slope_srtm_array, slope_srtm_width, slope_srtm_height, 0, 0);
            //    //double slope_srtm_out_val;
            //    //int slope_srtm_out_hasval;
            //    //float slope_srtm_NoDataValue = -1;
            //    //slope_srtm_band.GetNoDataValue(out slope_srtm_out_val, out slope_srtm_out_hasval);
            //    //if (slope_srtm_out_hasval != 0)
            //    //    slope_srtm_NoDataValue = (float)slope_srtm_out_val;
            //    //float slope_srtm_minimum = float.MaxValue,
            //    //    slope_srtm_maximum = slope_srtm_array.Max();
            //    //foreach (float v in slope_srtm_array)
            //    //{
            //    //    if (v != slope_srtm_NoDataValue && v < slope_srtm_minimum)
            //    //    {
            //    //        slope_srtm_minimum = v;
            //    //    }
            //    //}
            //    //province.Max_slope_srtm = (decimal?)slope_srtm_maximum;
            //    //province.Min_slope_srtm = (decimal?)slope_srtm_minimum;

            //    string srtm_file_name = Path.Combine(Properties.Settings.Default.GeoServerPath + @"\data_dir\coverages\AtlasSolar\Provinces", province.Code + "srtm.tif");
            //    Dataset srtm_ds = Gdal.Open(srtm_file_name, Access.GA_ReadOnly);
            //    Band srtm_band = srtm_ds.GetRasterBand(1);
            //    int srtm_width = srtm_band.XSize;
            //    int srtm_height = srtm_band.YSize;
            //    int[] srtm_array = new int[srtm_width * srtm_height];
            //    srtm_band.ReadRaster(0, 0, srtm_width, srtm_height, srtm_array, srtm_width, srtm_height, 0, 0);
            //    double srtm_out_val;
            //    int srtm_out_hasval;
            //    int srtm_NoDataValue = -1;
            //    srtm_band.GetNoDataValue(out srtm_out_val, out srtm_out_hasval);
            //    if (srtm_out_hasval != 0)
            //        srtm_NoDataValue = (int)srtm_out_val;
            //    int srtm_minimum = int.MaxValue,
            //        srtm_maximum = int.MinValue;
            //    foreach (int v in srtm_array)
            //    {
            //        if (v != srtm_NoDataValue && v < srtm_minimum)
            //        {
            //            srtm_minimum = v;
            //        }
            //        if (v != srtm_NoDataValue && v > srtm_maximum)
            //        {
            //            srtm_maximum = v;
            //        }
            //    }
            //    srtm_array = null;
            //    province.Max_srtm = srtm_maximum;
            //    province.Min_srtm = srtm_minimum;
            //    GC.Collect();

            //    //string swvdwnyear_file_name = Path.Combine(Properties.Settings.Default.GeoServerPath + @"\data_dir\coverages\AtlasSolar\Provinces", province.Code + "swvdwnyear.tif");
            //    //Dataset swvdwnyear_ds = Gdal.Open(swvdwnyear_file_name, Access.GA_ReadOnly);
            //    //Band swvdwnyear_band = swvdwnyear_ds.GetRasterBand(1);
            //    //int swvdwnyear_width = swvdwnyear_band.XSize;
            //    //int swvdwnyear_height = swvdwnyear_band.YSize;
            //    //float[] swvdwnyear_array = new float[swvdwnyear_width * swvdwnyear_height];
            //    //swvdwnyear_band.ReadRaster(0, 0, swvdwnyear_width, swvdwnyear_height, swvdwnyear_array, swvdwnyear_width, swvdwnyear_height, 0, 0);
            //    //double swvdwnyear_out_val;
            //    //int swvdwnyear_out_hasval;
            //    //float swvdwnyear_NoDataValue = -1;
            //    //swvdwnyear_band.GetNoDataValue(out swvdwnyear_out_val, out swvdwnyear_out_hasval);
            //    //if (swvdwnyear_out_hasval != 0)
            //    //    swvdwnyear_NoDataValue = (float)swvdwnyear_out_val;
            //    //float swvdwnyear_minimum = float.MaxValue,
            //    //    swvdwnyear_maximum = swvdwnyear_array.Max();
            //    //foreach (float v in swvdwnyear_array)
            //    //{
            //    //    if (v != swvdwnyear_NoDataValue && v < swvdwnyear_minimum)
            //    //    {
            //    //        swvdwnyear_minimum = v;
            //    //    }
            //    //}
            //    //province.Max_swvdwnyear = (decimal?)swvdwnyear_maximum;
            //    //province.Min_swvdwnyear = (decimal?)swvdwnyear_minimum;

            //    db.Entry(province).State = EntityState.Modified;
            //}
            //db.SaveChangesAsync();

            IList<Province> provinces = db.Provinces
                .ToList();
            provinces = provinces
                .OrderBy(p => p.Name)
                .ToList();
            ViewBag.Provinces = new SelectList(provinces, "Code", "Name");

            IList<MeteoDataSource> calcmeteodatasources = db.MeteoDataSources
                .Where(m => m.Code == Properties.Settings.Default.NASASSECode || m.Code == Properties.Settings.Default.SARAHECode)
                .ToList();
            calcmeteodatasources = calcmeteodatasources
                .OrderBy(m => m.Name)
                .ToList();
            ViewBag.CalcMeteoDataSources = new SelectList(calcmeteodatasources, "Id", "Name");

            ViewBag.gip = Properties.Settings.Default.GeoServerIP;
            return View();
        }

        public ActionResult Help()
        {
            return View();
        }

        public ActionResult Test2()
        {
            List<SelectListItem> PVOrientations = new List<SelectListItem>()
            {
                new SelectListItem() {Text="-Следящая по двум осям-", Value="1"}
            };
            ViewBag.PVOrientations = PVOrientations;
            ViewBag.PVSystemMaterials = new SelectList(db.PVSystemMaterials
                .ToList()
                .OrderBy(p => p.Name), "Id", "Name");
            
            HttpCookie cookie = System.Web.HttpContext.Current.Request.Cookies["Language"];
            ViewBag.Language = "ru";
            if (cookie != null)
            {
                if (cookie.Value != null)
                {
                    ViewBag.Language = cookie.Value;
                }
            }

            List<SelectListItem> Languages = new List<SelectListItem>()
            {
                new SelectListItem() {Text="English", Value="en", Selected=ViewBag.Language=="en"},
                new SelectListItem() {Text="Қазақ", Value="kk", Selected=ViewBag.Language=="kk"},
                new SelectListItem() {Text="Русский", Value="ru", Selected=ViewBag.Language=="ru"}
            };
            ViewBag.Languages = Languages;

            IList<MeteoDataPeriodicity> meteodataperiodicities = db.MeteoDataPeriodicities
                .ToList();
            meteodataperiodicities = meteodataperiodicities
                .OrderBy(m => m.Name)
                .ToList();
            IList<MeteoDataSource> meteodatasources = db.MeteoDataSources
                //.Where(m => true)
                .ToList();
            meteodatasources = meteodatasources
                .OrderBy(m => m.Name)
                .ToList();
            ViewBag.MeteoDataPeriodicities = new SelectList(meteodataperiodicities, "Id", "Name");
            ViewBag.MeteoDataSources = new SelectList(meteodatasources, "Id", "Name");

            IList<SPPStatus> sppstatuses = db.SPPStatus
                .ToList();
            sppstatuses = sppstatuses
                .OrderBy(s => s.Name)
                .ToList();
            ViewBag.SPPStatuses = new SelectList(sppstatuses, "Id", "Name");
            IList<SPPPurpose> spppurpose = db.SPPPurposes
                .ToList();
            spppurpose = spppurpose
                .OrderBy(s => s.Name)
                .ToList();
            ViewBag.SPPPurposes = new SelectList(spppurpose, "Id", "Name");
            IList<PanelOrientation> panelorientations = db.PanelOrientations
                .ToList();
            panelorientations = panelorientations
                .OrderBy(s => s.Name)
                .ToList();
            ViewBag.PanelOrientations = new SelectList(panelorientations, "Id", "Name");

            string decimaldelimiter = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            List<SPP> spps = db.SPPs.ToList();
            JObject o = JObject.FromObject(new
            {
                type = "FeatureCollection",
                crs = new
                {
                    type = "name",
                    properties = new
                    {
                        name = "urn:ogc:def:crs:EPSG::3857"
                    }
                },
                features = from spp in spps
                           select new
                           {
                               type = "Feature",
                               properties = new
                               {
                                   Id = spp.Id,
                                   Name = spp.Name,
                                   Count = spp.Count,
                                   Power = spp.Power,
                                   Cost = spp.Cost,
                                   Startup = spp.Startup,
                                   Link = spp.Link,
                                   Customer = spp.Customer,
                                   Investor = spp.Investor,
                                   Executor = spp.Executor,
                                   CapacityFactor = spp.CapacityFactor,
                                   Coordinates = spp.Coordinates,
                                   SPPStatusId = spp.SPPStatusId,
                                   SPPPurposeId = spp.SPPPurposeId,
                                   PanelOrientationId = spp.PanelOrientationId
                               },
                               geometry = new
                               {
                                   type = "Point",
                                   coordinates = new List<decimal>
                                {
                                    Convert.ToDecimal(spp.Coordinates.Split(',')[0].Replace(".",decimaldelimiter)),
                                    Convert.ToDecimal(spp.Coordinates.Split(',')[1].Replace(".",decimaldelimiter))
                                },
                               }
                           }
            });
            ViewBag.SPPLayerJson = o.ToString();

            ViewBag.Role = "";
            if (User.Identity.IsAuthenticated)
            {
                ViewBag.Role = "User";
                string CurrentUserId = User.Identity.GetUserId();
                if (UserManager.IsInRole(CurrentUserId, "Moderator"))
                {
                    ViewBag.Role = "Moderator";
                }
                if (UserManager.IsInRole(CurrentUserId, "Admin"))
                {
                    ViewBag.Role = "Admin";
                }
            }


            return View();
        }

        [HttpPost]
        public ActionResult Calc(decimal X, decimal Y)
        {
            decimal Xleft = 0,
                    Xright = 0,
                    Ybottom = 0,
                    Ytop = 0;
            if (X - (int)X >= 0.5M)
            {
                Xleft = (int)X + 0.5M;
                Xright = Xleft + 1;
            }
            else
            {
                Xleft = (int)X - 0.5M;
                Xright = Xleft + 1;
            }
            if (Y - (int)Y >= 0.5M)
            {
                Ybottom = (int)Y + 0.5M;
                Ytop = Ybottom + 1;
            }
            else
            {
                Ybottom = (int)Y - 0.5M;
                Ytop = Ybottom + 1;
            }
            // расстояния до углов
            decimal Rleftbottom = (decimal)Math.Sqrt(Math.Pow((double)(X - Xleft), 2) + Math.Pow((double)(Y - Ybottom), 2));
            decimal Rlefttop = (decimal)Math.Sqrt(Math.Pow((double)(X - Xleft), 2) + Math.Pow((double)(Y - Ytop), 2));
            decimal Rrighttop = (decimal)Math.Sqrt(Math.Pow((double)(X - Xright), 2) + Math.Pow((double)(Y - Ytop), 2));
            decimal Rrightbottom = (decimal)Math.Sqrt(Math.Pow((double)(X - Xright), 2) + Math.Pow((double)(Y - Ybottom), 2));
            // сумма расстояний
            decimal Rsum = Rleftbottom + Rlefttop + Rrighttop + Rrightbottom;

            // значения в углах
            decimal? Vleftbottom = db.MeteoDatas
                .Where(m => m.Latitude == Ybottom && m.Longitude == Xleft && m.MeteoDataTypeId == 5)
                .Select(m => m.Value)
                .Average();
            decimal? Vlefttop = db.MeteoDatas
                .Where(m => m.Latitude == Ytop && m.Longitude == Xleft && m.MeteoDataTypeId == 5)
                .Select(m => m.Value)
                .Average();
            decimal? Vrighttop = db.MeteoDatas
                .Where(m => m.Latitude == Ytop && m.Longitude == Xright && m.MeteoDataTypeId == 5)
                .Select(m => m.Value)
                .Average();
            decimal? Vrightbottom = db.MeteoDatas
                .Where(m => m.Latitude == Ybottom && m.Longitude == Xright && m.MeteoDataTypeId == 5)
                .Select(m => m.Value)
                .Average();
            // расчет
            decimal V = 0;
            if (Vleftbottom == null || Vlefttop == null || Vrighttop == null || Vrightbottom == null)
            {
                V = 0;
            }
            else
            {
                V = (decimal)(Vleftbottom * Rleftbottom + Vlefttop * Rlefttop + Vrighttop * Rrighttop + Vrightbottom * Rrightbottom) / Rsum;
            }

            JsonResult Data = new JsonResult();
            Data.Data = V;
            return Data;
        }

        [HttpPost]
        public ActionResult GetPVSystemMaterial(int Id)
        {
            PVSystemMaterial pvsystemmaterial = db.PVSystemMaterials.Where(p => p.Id == Id).FirstOrDefault();
            return Json(new
            {
                Efficiency = pvsystemmaterial.Efficiency,
                RatedOperatingTemperature = pvsystemmaterial.RatedOperatingTemperature,
                ThermalPowerFactor = pvsystemmaterial.ThermalPowerFactor
            });
        }

        //[HttpPost]
        //public ActionResult CalcPV(
        //    decimal Longitude,
        //    decimal Latitude,
        //    decimal Energy,
        //    int PVSystemMaterialId,
        //    decimal Efficiency,
        //    int RatedOperatingTemperature,
        //    decimal ThermalPowerFactor,
        //    decimal WiringEfficiency,
        //    decimal ControllerEfficiency,
        //    decimal InverterEfficiency,
        //    decimal BatteryEfficiency
        //    )
        //{
        //    //Longitude = 69.873046875M;
        //    //Latitude = 46.31658418182221M;
        //    PVSystemMaterial pvsystemmaterial = db.PVSystemMaterials.Where(p => p.Id == PVSystemMaterialId).FirstOrDefault();
        //    int NASASSEId = db.MeteoDataSources.Where(m => m.NameEN == "NASA SSE").FirstOrDefault().Id;
        //    int PeriodicityId = db.MeteoDataPeriodicities.Where(m => m.Code.Contains("Monthly average")).FirstOrDefault().Id;
        //    // количество дней по месяцам
        //    decimal[] days = new decimal[12];
        //    days[0] = 31;
        //    days[1] = 622 / 22;
        //    days[2] = 31;
        //    days[3] = 30;
        //    days[4] = 31;
        //    days[5] = 30;
        //    days[6] = 31;
        //    days[7] = 31;
        //    days[8] = 30;
        //    days[9] = 31;
        //    days[10] = 30;
        //    days[11] = 31;
        //    // радиация на горизонтальную поверхность
        //    int HpMeteoDataTypeId = db.MeteoDataTypes
        //        .Where(m => m.Code == "swv_dwn" && m.MeteoDataSourceId == NASASSEId && m.MeteoDataPeriodicityId == PeriodicityId)
        //        .FirstOrDefault().Id;
        //    IList<MeteoData> Hp = GetMonthlyAverage(Longitude, Latitude, HpMeteoDataTypeId);
        //    // полная радиация на ориентированную наклонную поверхность (для следящих по двум осям принимается равной Hp)
        //    // edit
        //    IList<MeteoData> Ht = Hp;
        //    // продолжительность светового дня
        //    int DMeteoDataTypeId = db.MeteoDataTypes
        //        .Where(m => m.Code == "daylight" && m.MeteoDataSourceId == NASASSEId && m.MeteoDataPeriodicityId == PeriodicityId)
        //        .FirstOrDefault().Id;
        //    IList<MeteoData> D = GetMonthlyAverage(Longitude, Latitude, DMeteoDataTypeId);
        //    // количество приходящей радиации в сумме в месяц
        //    IList<MeteoData> Ht1 = new List<MeteoData>();
        //    // количество приходящей радиации в час по месяцам
        //    IList<MeteoData> Ht2 = new List<MeteoData>();
        //    foreach (MeteoData hp in Hp)
        //    {
        //        if (hp.Month < 13)
        //        {
        //            Ht1.Add(new MeteoData()
        //            {
        //                Day = hp.Day,
        //                Latitude = hp.Latitude,
        //                Longitude = hp.Longitude,
        //                MeteoDataTypeId = hp.MeteoDataTypeId,
        //                Month = hp.Month,
        //                Year = hp.Year,
        //                Value = hp.Value * days[(int)hp.Month - 1]
        //            });
        //            Ht2.Add(new MeteoData()
        //            {
        //                Day = hp.Day,
        //                Latitude = hp.Latitude,
        //                Longitude = hp.Longitude,
        //                MeteoDataTypeId = hp.MeteoDataTypeId,
        //                Month = hp.Month,
        //                Year = hp.Year,
        //                Value = hp.Value / D.Where(d => d.Month == hp.Month).FirstOrDefault().Value
        //            });
        //        }
        //    }
        //    // суммарная продолжительность светового дня (часов работы установки в месяц)
        //    // edit (Dm from D)
        //    IList<MeteoData> Dm = GetMonthlyAverage(Longitude, Latitude, DMeteoDataTypeId);
        //    for (int i = 1; i <= 12; i++)
        //    {
        //        MeteoData Dmi = Dm.Where(d => d.Month == i).FirstOrDefault();
        //        Dmi.Value *= days[i - 1];
        //    }
        //    // средняя температура наружного воздуха град С
        //    int TaMeteoDataTypeId = db.MeteoDataTypes
        //        .Where(m => m.Code == "T10M" && m.MeteoDataSourceId == NASASSEId && m.MeteoDataPeriodicityId == PeriodicityId && m.AdditionalEN.Contains("22-year Average"))
        //        .FirstOrDefault().Id;
        //    IList<MeteoData> Ta = GetMonthlyAverage(Longitude, Latitude, TaMeteoDataTypeId);
        //    // температура ячейки в рабочем режиме
        //    IList<decimal> Tc = new List<decimal>();
        //    //foreach(MeteoData ta in Ta.OrderBy(t => t.Month))
        //    for (int i = 1; i <= 12; i++)
        //    {
        //        MeteoData ht2 = Ht2.Where(h => h.Month == Ta[i - 1].Month).FirstOrDefault();
        //        Tc.Add((decimal)Ta[i - 1].Value + (decimal)ht2.Value / 0.8M * (RatedOperatingTemperature - 20));
        //    }
        //    // эффективность солнечной панели в зависимости от температуры
        //    IList<decimal> Nt = new List<decimal>();
        //    for (int i = 1; i <= 12; i++)
        //    {
        //        Nt.Add(Efficiency * (1 - ThermalPowerFactor * (Tc[i - 1] - 25)));
        //    }
        //    // общий коэффициент потерь
        //    IList<decimal> N = new List<decimal>();
        //    for (int i = 1; i <= 12; i++)
        //    {
        //        N.Add(WiringEfficiency * ControllerEfficiency * InverterEfficiency * BatteryEfficiency * Nt[i - 1]);
        //    }
        //    // общая площадь
        //    List<decimal> S = new List<decimal>();
        //    for (int i = 1; i <= 12; i++)
        //    {
        //        S.Add(Energy / (N[i - 1] * (decimal)Ht[i - 1].Value));
        //    }
        //    List<string> Ss = new List<string>();
        //    for (int i = 0; i < S.Count(); i++)
        //    {
        //        Ss.Add(S[i].ToString("F"));
        //    }
        //    // данные для графика
        //    List<PlotData> PD = new List<PlotData>();
        //    for (int i = 0; i < S.Count(); i++)
        //    {
        //        PD.Add(new PlotData() { X = i + 1, Y = S[i] });
        //    }
        //    return Json(new
        //    {
        //        PVSystemType = pvsystemmaterial.Name,
        //        PVSystemEfficiency = Efficiency,
        //        PVSystemRatedOperatingTemperature = RatedOperatingTemperature,
        //        PVSystemThermalPowerFactor = ThermalPowerFactor,
        //        Ss = Ss,
        //        S = S,
        //        PD = PD
        //    });
        //}

        [HttpPost]
        public ActionResult CalcPV(
            decimal? Longitude,
            decimal? Latitude,
            int MeteoDataSourceId,
            int? Wload,
            int SPPPurposeId,
            int PanelOrientationId,
            int PVSystemMaterialId,
            decimal? RatedPower,
            int? PanelsCount,
            decimal? PanelEfficiency,
            int? RatedOperatingTemperature,
            decimal? ThermalPowerFactor,
            decimal? WiringEfficiency,
            decimal? ControllerEfficiency,
            decimal? InverterEfficiency,
            decimal? BatteryEfficiency,
            int? Azimuth,
            int? Tilt,
            decimal? LossesSoil,
            decimal? LossesSnow,
            int[] LossesSnowMoths,
            decimal? LossesShad,
            decimal? LossesMis
            )
        {
            decimal longitude = Longitude == null ? 0 : (decimal)Longitude;
            decimal latitude = Latitude == null ? 0 : (decimal)Latitude;
            //decimal longitude = string.IsNullOrEmpty(Longitude) ? 0 : Convert.ToDecimal(Longitude);
            //decimal latitude = string.IsNullOrEmpty(Latitude) ? 0 : Convert.ToDecimal(Latitude);
            decimal wload = Wload == null ? 0 : (int)Wload;
            PanelOrientation panel_orientation = db.PanelOrientations.FirstOrDefault(p => p.Id == PanelOrientationId);
            decimal azimuth = Azimuth == null ? 0 : (int)Azimuth;
            decimal tilt = Tilt == null ? 0 : (int)Tilt;
            int NOCT = RatedOperatingTemperature == null ? 0 : (int)RatedOperatingTemperature;
            decimal panel_efficiency = PanelEfficiency == null ? 0 : (decimal)PanelEfficiency;
            decimal thermal_power_factor = ThermalPowerFactor == null ? 0 : (decimal)ThermalPowerFactor;
            decimal wiring_efficiency = WiringEfficiency == null ? 0 : (decimal)WiringEfficiency;
            decimal controller_efficiency = ControllerEfficiency == null ? 0 : (decimal)ControllerEfficiency;
            decimal inverter_efficiency = InverterEfficiency == null ? 0 : (decimal)InverterEfficiency;
            decimal battery_efficiency = BatteryEfficiency == null ? 0 : (decimal)BatteryEfficiency;
            int panels_count = PanelsCount == null ? 0 : (int)PanelsCount;
            decimal rated_power = RatedPower == null ? 0 : (decimal)RatedPower;
            decimal losses_soil = LossesSoil == null ? 0 : (decimal)LossesSoil;
            decimal losses_snow = LossesSnow == null ? 0 : (decimal)LossesSnow;
            decimal losses_shad = LossesShad == null ? 0 : (decimal)LossesShad;
            decimal losses_mis = LossesMis == null ? 0 : (decimal)LossesMis;

            //int SPPPurposeId,
            //int PVSystemMaterialId,
            //decimal? ThermalPowerFactor,


            PVSystemMaterial pvsystemmaterial = db.PVSystemMaterials.Where(p => p.Id == PVSystemMaterialId).FirstOrDefault();

            int NASASSEId = db.MeteoDataSources.Where(m => m.Code.ToLower() == Properties.Settings.Default.NASASSECode.ToLower()).FirstOrDefault().Id;
            int NASAPOWERId = db.MeteoDataSources.Where(m => m.Code.ToLower() == Properties.Settings.Default.NASAPOWERCode.ToLower()).FirstOrDefault().Id;
            int PeriodicityMAId = db.MeteoDataPeriodicities.Where(m => m.Code.ToLower().Contains(Properties.Settings.Default.Monthly) && m.Code.ToLower().Contains(Properties.Settings.Default.Average)).FirstOrDefault().Id;

            // количество дней по месяцам
            decimal[] days = new decimal[12];
            days[0] = 31;
            days[1] = 622 / 22;
            days[2] = 31;
            days[3] = 30;
            days[4] = 31;
            days[5] = 30;
            days[6] = 31;
            days[7] = 31;
            days[8] = 30;
            days[9] = 31;
            days[10] = 30;
            days[11] = 31;

            // Monthly Averaged Insolation Incident On A Horizontal Surface (kWh/m2/day)
            IList<MeteoData> H = null;
            // Monthly Averaged Diffuse Radiation Incident On A Horizontal Surface (kWh/m2/day)
            IList<MeteoData> Hd = null;
            MeteoDataSource mds = db.MeteoDataSources
                .Where(m => m.Id == MeteoDataSourceId)
                .FirstOrDefault();
            if(mds.Code == Properties.Settings.Default.NASASSECode)
            {
                int HMeteoDataTypeId = db.MeteoDataTypes
                    .Where(m => m.Code == "swv_dwn" && m.MeteoDataSourceId == NASASSEId && m.MeteoDataPeriodicityId == PeriodicityMAId)
                    .FirstOrDefault().Id;
                H = GetMonthlyAverage(longitude, latitude, HMeteoDataTypeId);
                for (int i = 1; i <= 12; i++)
                {
                    H.FirstOrDefault(h => h.Month == i).Value = H.FirstOrDefault(h => h.Month == i).Value == null ? 0 : H.FirstOrDefault(h => h.Month == i).Value;
                }
                
                int HdMeteoDataTypeId = db.MeteoDataTypes
                    .Where(m => m.Code == "exp_dif" && m.MeteoDataSourceId == NASASSEId && m.MeteoDataPeriodicityId == PeriodicityMAId && m.AdditionalEN == "22-year Average")
                    .FirstOrDefault().Id;
                Hd = GetMonthlyAverage(longitude, latitude, HdMeteoDataTypeId);
                for (int i = 1; i <= 12; i++)
                {
                    Hd.FirstOrDefault(h => h.Month == i).Value = Hd.FirstOrDefault(h => h.Month == i).Value == null ? 0 : Hd.FirstOrDefault(h => h.Month == i).Value;
                }
            }
            else
            if(mds.Code == Properties.Settings.Default.SARAHECode)
            {
                int HMeteoDataTypeId = db.MeteoDataTypes
                    .Where(m => m.Code == "SIS" && m.MeteoDataSourceId == MeteoDataSourceId && m.MeteoDataPeriodicityId == PeriodicityMAId)
                    .FirstOrDefault().Id;
                H = GetMonthlyAverage(longitude, latitude, HMeteoDataTypeId);
                for (int i = 1; i <= 12; i++)
                {
                    H.FirstOrDefault(h => h.Month == i).Value = H.FirstOrDefault(h => h.Month == i).Value == null ? 0 : H.FirstOrDefault(h => h.Month == i).Value;
                }

                int HdMeteoDataTypeId = db.MeteoDataTypes
                    .Where(m => m.Code == "SID" && m.MeteoDataSourceId == MeteoDataSourceId && m.MeteoDataPeriodicityId == PeriodicityMAId)
                    .FirstOrDefault().Id;
                Hd = GetMonthlyAverage(longitude, latitude, HdMeteoDataTypeId);
                for (int i = 1; i <= 12; i++)
                {
                    Hd.FirstOrDefault(h => h.Month == i).Value = Hd.FirstOrDefault(h => h.Month == i).Value == null ? 0 : Hd.FirstOrDefault(h => h.Month == i).Value;
                }
                for (int i = 1; i <= 12; i++)
                {
                    Hd.FirstOrDefault(h => h.Month == i).Value = H.FirstOrDefault(h => h.Month == i).Value - Hd.FirstOrDefault(h => h.Month == i).Value;
                }
            }

            // Monthly Averaged Surface Albedo
            int PsMeteoDataTypeId = db.MeteoDataTypes
                .Where(m => m.Code == "srf_alb" && m.MeteoDataSourceId == NASASSEId && m.MeteoDataPeriodicityId == PeriodicityMAId)
                .FirstOrDefault().Id;
            IList<MeteoData> Ps = GetMonthlyAverage(longitude, latitude, PsMeteoDataTypeId);
            for (int i = 1; i <= 12; i++)
            {
                Ps.FirstOrDefault(p => p.Month == i).Value = Ps.FirstOrDefault(p => p.Month == i).Value == null ? 0 : Ps.FirstOrDefault(p => p.Month == i).Value;
            }

            // The angle relative to the horizontal for which the monthly averaged total solar radiation is a maximum (degrees)
            int OPT_ANGMeteoDataTypeId = db.MeteoDataTypes
                .Where(m => m.Code == "ret_tlt0" && m.MeteoDataSourceId == NASASSEId && m.MeteoDataPeriodicityId == PeriodicityMAId && m.AdditionalEN == "OPT ANG")
                .FirstOrDefault().Id;
            IList<MeteoData> OPT_ANG = GetMonthlyAverage(longitude, latitude, OPT_ANGMeteoDataTypeId);
            for (int i = 1; i <= 12; i++)
            {
                OPT_ANG.FirstOrDefault(o => o.Month == i).Value = OPT_ANG.FirstOrDefault(o => o.Month == i).Value == null ? 0 : OPT_ANG.FirstOrDefault(o => o.Month == i).Value;
            }

            // Monthly Averaged Air Temperature At 2 m Above The Surface Of The Earth For Indicated GMT Times (°C) (NASA POWER)
            //int TaMeteoDataTypeId = db.MeteoDataTypes
            //    .Where(m => m.Code == "T2M" && m.MeteoDataSourceId == NASAPOWERId && m.MeteoDataPeriodicityId == PeriodicityMAId)
            //    .FirstOrDefault().Id;
            //int TaMeteoDataTypeId = db.MeteoDataTypes
            //    .Where(m => m.Code == "T10M" && m.MeteoDataSourceId == NASASSEId && m.MeteoDataPeriodicityId == PeriodicityMAId)
            //    .FirstOrDefault().Id;
            //IList<MeteoData> TaM = GetMonthlyAverage(longitude, latitude, TaMeteoDataTypeId);

            int Ta03MeteoDataTypeId = db.MeteoDataTypes
                .Where(m => m.Code == "T2M3" && m.MeteoDataSourceId == NASAPOWERId && m.MeteoDataPeriodicityId == PeriodicityMAId && m.AdditionalEN == "Average@03")
                .FirstOrDefault().Id;
            IList<MeteoData> Ta03 = GetMonthlyAverage(longitude, latitude, Ta03MeteoDataTypeId);
            int Ta06MeteoDataTypeId = db.MeteoDataTypes
                .Where(m => m.Code == "T2M3" && m.MeteoDataSourceId == NASAPOWERId && m.MeteoDataPeriodicityId == PeriodicityMAId && m.AdditionalEN == "Average@06")
                .FirstOrDefault().Id;
            IList<MeteoData> Ta06 = GetMonthlyAverage(longitude, latitude, Ta06MeteoDataTypeId);
            int Ta09MeteoDataTypeId = db.MeteoDataTypes
                .Where(m => m.Code == "T2M3" && m.MeteoDataSourceId == NASAPOWERId && m.MeteoDataPeriodicityId == PeriodicityMAId && m.AdditionalEN == "Average@09")
                .FirstOrDefault().Id;
            IList<MeteoData> Ta09 = GetMonthlyAverage(longitude, latitude, Ta09MeteoDataTypeId);
            int Ta12MeteoDataTypeId = db.MeteoDataTypes
                .Where(m => m.Code == "T2M3" && m.MeteoDataSourceId == NASAPOWERId && m.MeteoDataPeriodicityId == PeriodicityMAId && m.AdditionalEN == "Average@12")
                .FirstOrDefault().Id;
            IList<MeteoData> Ta12 = GetMonthlyAverage(longitude, latitude, Ta12MeteoDataTypeId);
            IList<MeteoData> Ta = new List<MeteoData>();
            for (int i = 1; i <= 12; i++)
            {
                // (1.3)
                Ta.Add(new MeteoData
                {
                    Month = i,
                    Value = (Ta03.FirstOrDefault(t => t.Month == i).Value + Ta06.FirstOrDefault(t => t.Month == i).Value + Ta09.FirstOrDefault(t => t.Month == i).Value + Ta12.FirstOrDefault(t => t.Month == i).Value) / 4m
                });
            }


            // расчет дневной среднемесячной солнечной радиации
            // >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            latitude = latitude / 180 * (decimal)Math.PI;
            tilt = tilt / 180 * (decimal)Math.PI;
            azimuth = azimuth / 180 * (decimal)Math.PI;
            //H[0].Value = 1.14M;
            //Hd[0].Value = 0.69M;
            //Ps[0].Value = 0.35M;
            //H[6].Value = 5.93M;
            //Hd[6].Value = 2.58M;
            //Ps[6].Value = 0.16M;


            // порядковый номер дня в году, отсчитываемый от 1 января
            int[] N = new int[12];
            N[0] = 17;
            N[1] = 47;
            N[2] = 75;
            N[3] = 105;
            N[4] = 135;
            N[5] = 162;
            N[6] = 198;
            N[7] = 228;
            N[8] = 258;
            N[9] = 288;
            N[10] = 318;
            N[11] = 344;

            // значения склонения Солнца (рад)
            decimal[] δ = new decimal[12];
            for (int i = 0; i < 12; i++)
            {
                // (2.8)
                δ[i] = 23.45M * (decimal)Math.PI / 180 * (decimal)Math.Sin((double)((360M * (284M + N[i]) / 365M) / 180M * (decimal)Math.PI));
            }

            // sunset hour angle
            decimal[] ω_s = new decimal[12];
            for (int i = 0; i < 12; i++)
            {
                // (2.7)
                //ω_s[i] = acos(sin(latitude) * sin(δ[i]) / cos(latitude) / cos(δ[i]));
                ω_s[i] = (decimal)Math.Acos(-Math.Sin((double)latitude) * Math.Sin((double)δ[i]) / Math.Cos((double)latitude) / Math.Cos((double)δ[i]));
            }

            decimal[] A = new decimal[12];
            decimal[] B = new decimal[12];
            for (int i = 0; i < 12; i++)
            {
                // (2.5)
                A[i] = 0.409m + 0.5016m * (decimal)Math.Sin((double)(ω_s[i] - (decimal)Math.PI / 3));
                // (2.6)
                B[i] = 0.6609m - 0.4767m * (decimal)Math.Sin((double)(ω_s[i] - (decimal)Math.PI / 3));
            }

            // solar hour angle for each daylight hour relative to solar noon.
            // Часовой угол ω (рад) – угол, который определяет угловое смещение Солнца в течение суток.
            // Один час соответствует π/12 рад или 15 град углового смещения.
            // В полдень часовой угол равен нулю.
            // Значения часового угла до полудня считаются отрицательными, после полудня – положительными.
            decimal[] ω = new decimal[25];
            int w_begin = 2,
                w_end = 22;
            for (int i = w_begin; i <= w_end; i++)
            {
                ω[i] = (decimal)(i * 15 - 12 * 15) / 180 * (decimal)Math.PI;
            }

            decimal[,] rt = new decimal[12, 25];
            decimal[,] rd = new decimal[12, 25];
            decimal[,] Hh = new decimal[12, 25];
            decimal[,] Hdh = new decimal[12, 25];
            for (int i = 0; i < 12; i++)
            {
                for (int j = w_begin; j <= w_end; j++)
                {
                    // (2.3)
                    rt[i, j] = (decimal)Math.PI / 24 * (A[i] + B[i] * (decimal)Math.Cos((double)ω[j])) * ((decimal)Math.Cos((double)ω[j]) - (decimal)Math.Cos((double)ω_s[i])) / ((decimal)Math.Sin((double)ω_s[i]) - ω_s[i] * (decimal)Math.Cos((double)ω_s[i]));
                    // (2.4)
                    rd[i, j] = (decimal)Math.PI / 24 * ((decimal)Math.Cos((double)ω[j]) - (decimal)Math.Cos((double)ω_s[i])) / ((decimal)Math.Sin((double)ω_s[i]) - ω_s[i] * (decimal)Math.Cos((double)ω_s[i]));
                    // (2.1)
                    Hh[i, j] = 1000 * rt[i, j] * (decimal)H.FirstOrDefault(m => m.Month == i + 1).Value;
                    // (2.2)
                    Hdh[i, j] = 1000 * rd[i, j] * (decimal)Hd.FirstOrDefault(m => m.Month == i + 1).Value;
                }
            }

            decimal[] σ_w = new decimal[25];
            for (int i = w_begin; i <= w_end; i++)
            {
                // (2.17)
                σ_w[i] = ω[i] >= 0 ? 1 : -1;
            }

            decimal[] σ_ns = new decimal[12];
            for (int i = 0; i < 12; i++)
            {
                // (2.16)
                σ_ns[i] = latitude * (latitude - δ[i]) >= 0 ? 1 : -1;
            }

            decimal[] ω_ew = new decimal[12];
            for (int i = 0; i < 12; i++)
            {
                // (2.14)
                ω_ew[i] = (decimal)Math.Acos((double)((decimal)Math.Cos((double)latitude) * (decimal)Math.Tan((double)δ[i])));
            }

            decimal[,] σ_ew = new decimal[12, 25];
            for (int i = 0; i < 12; i++)
            {
                for (int j = w_begin; j <= w_end; j++)
                {
                    // (2.15)
                    σ_ew[i, j] = Math.Abs(ω[j]) <= ω_ew[i] ? 1 : -1;
                }
            }

            // hourly cosine of zenith angle (the angle between the vertical and a ray from the sun)
            decimal[,] cosθ_zh = new decimal[12, 25];
            //decimal[,] sinθ_zh = new decimal[12, 25];
            //decimal[,] tanθ_zh = new decimal[12, 25];
            decimal[,] θ_zh = new decimal[12, 25];
            for (int i = 0; i < 12; i++)
            {
                for (int j = w_begin; j <= w_end; j++)
                {
                    // (2.11)
                    cosθ_zh[i, j] = (decimal)Math.Sin((double)latitude) * (decimal)Math.Sin((double)δ[i]) + (decimal)Math.Cos((double)latitude) * (decimal)Math.Cos((double)δ[i]) * (decimal)Math.Cos((double)ω[j]);
                    //sinθ_zh[i, j] = (decimal)Math.Pow(1 - Math.Pow((double)cosθ_zh[i, j], 2), 0.5);
                    //tanθ_zh[i, j] = sinθ_zh[i, j] / cosθ_zh[i, j];
                    θ_zh[i, j] = (decimal)Math.Acos((double)cosθ_zh[i, j]);
                }
            }

            decimal[,] γ_sh0 = new decimal[12, 25];
            for (int i = 0; i < 12; i++)
            {
                for (int j = w_begin; j <= w_end; j++)
                {
                    // (2.13)
                    try
                    {
                        γ_sh0[i, j] = (decimal)Math.Asin((double)((decimal)Math.Sin((double)ω[j]) * (decimal)Math.Cos((double)δ[i]) / (decimal)Math.Sin((double)θ_zh[i, j])));
                    }
                    catch
                    {
                        γ_sh0[i, j] = 0;
                    }

                }
            }

            // hourly solar azimuth angle; angle between the line of sight of the Sun into the horizontal surface and the local meridian
            decimal[,] γ_sh = new decimal[12, 25];
            for (int i = 0; i < 12; i++)
            {
                for (int j = w_begin; j <= w_end; j++)
                {
                    // (2.12)
                    γ_sh[i, j] = σ_ew[i, j] * σ_ns[i] * γ_sh0[i, j] + (1 - σ_ew[i, j] * σ_ns[i]) / 2 * σ_w[j] * (decimal)Math.PI;
                }
            }

            // hourly surface azimuth of the tilted surface; angle between the projection of the normal to the surface into the horizontal surface and the local meridian.
            // Azimuth is zero facing the equator, positive west, and negative east
            decimal[,] γ_h = new decimal[12, 25];
            // ?????????????????????????????????????????????????????????????????????



            // Следящая по горизонтальной оси, корректируемая по азимуту солнца 2 раза в год
            decimal[,] β_h0 = new decimal[12, 25];
            decimal[,] σ_β = new decimal[12, 25];
            // hourly slope of the PV array relative to horizontal surface
            decimal[,] β_h = new decimal[12, 25];
            if (panel_orientation.Code.ToLower() == Properties.Settings.Default.PanelOriantationHorizontalCode.ToLower())
            {
                for (int i = 0; i < 12; i++)
                {
                    for (int j = w_begin; j <= w_end; j++)
                    {
                        γ_h[i, j] = azimuth;
                        // (2.20)
                        β_h0[i, j] = (decimal)Math.Atan((double)((decimal)Math.Tan((double)θ_zh[i, j]) * (decimal)Math.Cos((double)(γ_h[i, j] - γ_sh[i, j]))));
                        // (2.21)
                        σ_β[i, j] = β_h0[i, j] >= 0 ? 0 : 1;
                        // (2.19)
                        β_h[i, j] = β_h0[i, j] + σ_β[i, j] * (decimal)Math.PI;
                    }
                }
            }

            // Следящая по двум осям
            if (panel_orientation.Code.ToLower() == Properties.Settings.Default.PanelOriantation2AxisCode.ToLower())
            {
                for (int i = 0; i < 12; i++)
                {
                    for (int j = w_begin; j <= w_end; j++)
                    {
                        // (2.22)
                        //γ_sh[i, j] = azimuth;????????????????????
                        γ_h[i, j] = γ_sh[i, j];
                        β_h[i, j] = θ_zh[i, j];
                    }
                }
            }

            // Фиксированная, азимут и угол наклона корректируются 2 раза в год
            // Значение углов наклона панели
            //decimal[] β = new decimal[12];
            string[] β_h_s = new string[12];
            if (panel_orientation.Code.ToLower() == Properties.Settings.Default.PanelOriantationFixedCorrectableCode.ToLower())
            {
                for (int i = 0; i < 12; i++)
                {
                    if (i >= 3 && i <= 8) //
                    {
                        for (int j = w_begin; j <= w_end; j++)
                        {
                            β_h[i, j] = (decimal)OPT_ANG.FirstOrDefault(o => o.Month == 7).Value / 180 * (decimal)Math.PI;
                        }
                    }
                    else
                    {
                        for (int j = w_begin; j <= w_end; j++)
                        {
                            β_h[i, j] = (decimal)OPT_ANG.FirstOrDefault(o => o.Month == 1).Value / 180 * (decimal)Math.PI;
                        }
                    }
                    β_h_s[i] = (β_h[i, 12] * 180 / (decimal)Math.PI).ToString("0.00");
                }
            }

            // Следящая по вертикальной оси, корректируемая по углу наклона 2 раза в год
            if (panel_orientation.Code.ToLower() == Properties.Settings.Default.PanelOriantationVerticalCode.ToLower())
            {
                for (int i = 0; i < 12; i++)
                {
                    for (int j = w_begin; j <= w_end; j++)
                    {
                        γ_h[i, j] = γ_sh[i, j];
                    }
                    //if (i >= 2 && i < 8)
                    //{
                    //    for (int j = w_begin; j <= w_end; j++)
                    //    {
                    //        β_h[i, j] = (decimal)OPT_ANG.FirstOrDefault(o => o.Month == 3).Value;
                    //    }
                    //}
                    //else
                    //{
                    //    for (int j = w_begin; j <= w_end; j++)
                    //    {
                    //        β_h[i, j] = (decimal)OPT_ANG.FirstOrDefault(o => o.Month == 9).Value;
                    //    }
                    //}
                }
            }

            // Фиксированная, азимут и угол наклона корректируются 2 раза в год
            if (panel_orientation.Code.ToLower() == Properties.Settings.Default.PanelOriantationFixedCode.ToLower())
            {
                for (int i = 0; i < 12; i++)
                {
                    for (int j = w_begin; j <= w_end; j++)
                    {
                        //β_h[i, j] = tilt;
                        γ_h[i, j] = azimuth;
                    }
                }
            }

            // Фиксированная
            if (panel_orientation.Code.ToLower() == Properties.Settings.Default.PanelOriantationFixedCode.ToLower())
            {
                for (int i = 0; i < 12; i++)
                {
                    for (int j = w_begin; j <= w_end; j++)
                    {
                        β_h[i, j] = tilt;
                    }
                }
            }

            // Следящая по вертикальной оси, корректируемая по углу наклона 2 раза в год
            if (panel_orientation.Code.ToLower() == Properties.Settings.Default.PanelOriantationVerticalCode.ToLower())
            {
                for (int i = 0; i < 12; i++)
                {
                    for (int j = w_begin; j <= w_end; j++)
                    {
                        // (2.18)
                        γ_h[i, j] = γ_sh[i, j];
                        β_h[i, j] = tilt;
                    }
                }
            }

            // hourly cosine of incidence angle (angle between a ray from the sun and the surface normal)
            decimal[,] cosθ_h = new decimal[12, 25];
            for (int i = 0; i < 12; i++)
            {
                for (int j = w_begin; j <= w_end; j++)
                {
                    cosθ_h[i, j] = (decimal)Math.Sin((double)θ_zh[i, j]) * (decimal)Math.Sin((double)β_h[i, j]) * (decimal)Math.Cos⁡((double)(γ_sh[i, j] - γ_h[i, j])) + cosθ_zh[i, j] * (decimal)Math.Cos((double)β_h[i, j]);
                }
            }

            decimal[,] H_th = new decimal[12, 25];
            for (int i = 0; i < 12; i++)
            {
                for (int j = w_begin; j <= w_end; j++)
                {
                    // (2.10)
                    H_th[i, j] = (Hh[i, j] - Hdh[i, j]) * cosθ_h[i, j] / cosθ_zh[i, j] + Hdh[i, j] * (1 + (decimal)Math.Cos((double)β_h[i, j])) / 2 + Hh[i, j] * (decimal)Ps.FirstOrDefault(p => p.Month == i + 1).Value * (1 - (decimal)Math.Cos((double)β_h[i, j])) / 2;
                }
            }

            decimal[] H_td = new decimal[12];
            string[] H_td_s = new string[12];
            for (int i = 0; i < 12; i++)
            {
                H_td[i] = 0;
                for (int j = w_begin; j <= w_end; j++)
                {
                    H_th[i, j] = cosθ_zh[i, j] <= 0 ? 0 : H_th[i, j];
                    H_th[i, j] = H_th[i, j] <= 0 ? 0 : H_th[i, j];
                    H_td[i] += H_th[i, j];
                }
                H_td_s[i] = (H_td[i] / 1000m).ToString("0.00");
            }
            // <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
            // Прочие потери в PV панели
            // (1.5)
            //decimal Lopv = 1 - (1 - losses_snow) * (1 - losses_soil) * (1 - losses_shad) * (1 - losses_mis);
            decimal[] Lopv = new decimal[12];
            for (int i = 0; i < 12; i++)
            {
                bool losses_soil_month_b = false;
                for (int j = 0; j < LossesSnowMoths.Length; j++)
                {
                    if (LossesSnowMoths[j] == i)
                    {
                        losses_soil_month_b = true;
                        break;
                    }
                }
                decimal losses_soil_month = losses_soil_month_b ? losses_soil : 0m;
            }

            // the coefficient of efficiency of other power losses in PV module
            // (1.6)
            //decimal η_opv = 1 - Lopv;
            decimal[] η_opv = new decimal[12];
            for (int i = 0; i < 12; i++)
            {
                η_opv[i] = 1 - Lopv[i];
            }

            //// Средняя дневная температура наружного воздуха град С
            //// (1.3)
            //decimal Ta = (decimal)(TaM.FirstOrDefault(t => t.Month == 3).Value +
            //    TaM.FirstOrDefault(t => t.Month == 3).Value +
            //    TaM.FirstOrDefault(t => t.Month == 3).Value +
            //    TaM.FirstOrDefault(t => t.Month == 3).Value) / 4;
            string[] Ta_s = new string[12];
            for (int i = 0; i < 12; i++)
            {
                Ta_s[i] = ((decimal)Ta.FirstOrDefault(t => t.Month == i + 1).Value).ToString("0.00");
            }

            // Monthly averaged hourly solar radiation on a tilted surface [kWh/m2/h]
            decimal[] H_tah = new decimal[12];
            string[] H_tah_s = new string[12];
            string[] H_tm_s = new string[12];
            string[] D_s = new string[12];
            for (int i = 0; i < 12; i++)
            {
                decimal bufer = 0;
                int bufer_count = 0;
                decimal D = 0;
                int D_count = 0;
                for (int j = w_begin; j <= w_end; j++)
                {
                    if (H_th[i, j] > 0)
                    {
                        D += H_th[i, j];
                        D_count++;
                    }
                    //H_tah[i, j] = H_td[i] / j;
                    bufer += H_th[i, j];
                    bufer_count++;
                }
                D_s[i] = D_count.ToString();
                H_tah[i] = D / D_count;
                H_tm_s[i] = (D / 1000M * days[i]).ToString("0.00");
                H_tah_s[i] = (D / D_count / 1000m).ToString("0.00");
            }

            // Температура ячейки в рабочем режиме для каждого месяца
            decimal[] T_c = new decimal[12];
            string[] T_c_s = new string[12];
            for (int i = 0; i < 12; i++)
            {
                //for (int j = w_begin; j <= w_end; j++)
                {
                    // (1.2)
                    T_c[i] = (decimal)Ta.FirstOrDefault(t => t.Month == i + 1).Value + H_tah[i] / 1000m / 0.8m * (NOCT - 20);
                    T_c_s[i] = T_c[i].ToString("0.00");
                }
            }

            // коэффициент эффективности солнечного панеля при различных температурах ячейки
            decimal[] η_tpv = new decimal[12];
            string[] η_tpv_s = new string[12];
            for (int i = 0; i < 12; i++)
            {
                decimal bufer = 0;
                int bufer_count = 0;
                //for (int j = w_begin; j <= w_end; j++)
                {
                    // (1.1)
                    η_tpv[i] = panel_efficiency * (1 + thermal_power_factor * (T_c[i] - 25));
                    bufer += η_tpv[i];
                    bufer_count++;
                }
                η_tpv_s[i] = (bufer / bufer_count).ToString("0.00");
            }

            // Эффективеность электрических устройств
            // (1.7)
            decimal η_epv = wiring_efficiency * controller_efficiency * inverter_efficiency * battery_efficiency;

            // Коэффициент эффективности PV системы
            decimal[] η_spv = new decimal[12];
            string[] η_spv_s = new string[12];
            for (int i = 0; i < 12; i++)
            {
                decimal bufer = 0;
                int bufer_count = 0;
                //for (int j = w_begin; j <= w_end; j++)
                {
                    // (1.8)
                    η_spv[i] = η_tpv[i] * η_epv * η_opv[i];
                    bufer += η_spv[i];
                    bufer_count++;
                }
                η_spv_s[i] = (bufer / bufer_count).ToString("0.00");
            }

            //// Area of Solar PV panel [m2]
            //// (1.9-2)
            //decimal S_pv = panels_count * rated_power / 1000 / panel_efficiency;
            // Среднемесячная необходимая общая площадь солнечных PV-систем (kWh/day)
            decimal[] S_pv = new decimal[12];
            string[] S_pv_s = new string[12];
            for (int i = 0; i < 12; i++)
            {
                // (31)
                S_pv[i] = 1000 * wload / (η_spv[i] * H_td[i]);
                S_pv_s[i] = S_pv[i].ToString("0.00");
            }

            //// Monthly averaged energy output from the Solar PV panel [kWh/day]
            //decimal[] W_PV = new decimal[12];
            //for (int i = 0; i < 12; i++)
            //{
            //    //for (int j = w_begin; j <= w_end; j++)
            //    {
            //        // (1.9-1)
            //        W_PV[i] = S_pv * η_spv[i] * H_td[i];
            //    }
            //}
            //string[] W_PV_s = new string[12];
            //for (int i = 0; i < 12; i++)
            //{
            //    decimal bufer = 0;
            //    //for (int j = w_begin; j <= w_end; j++)
            //    {
            //        bufer += W_PV[i] / 1000m;
            //    }
            //    W_PV_s[i] = bufer.ToString("0.00");
            //}
            // Общая необходимая номинальная мощность солнечных PV-систем
            decimal[] P_tpv = new decimal[12];
            string[] P_tpv_s = new string[12];
            for (int i = 0; i < 12; i++)
            {
                // (32)
                //P_tpv[i] = S_pv[i] / η_spv[i] * 1000;
                P_tpv[i] = S_pv[i] * 1000 * panel_efficiency;
                P_tpv_s[i] = P_tpv[i].ToString("0.00");
            }

            // данные для графика
            //List<PlotData> W_PV_c = new List<PlotData>();
            //for (int i = 0; i < W_PV.Count(); i++)
            //{
            //    W_PV_c.Add(new PlotData() { X = i + 1, Y = W_PV[i] / 1000m });
            //}
            List<PlotData> S_pv_c = new List<PlotData>();
            for (int i = 0; i < S_pv.Count(); i++)
            {
                S_pv_c.Add(new PlotData() { X = i + 1, Y = S_pv[i] });
            }

            return Json(new
            {
                //Longitude = Longitude.ToString(),
                //Latitude = Latitude.ToString(),
                //SPPPurpose = db.SPPPurposes.FirstOrDefault(s => s.Id == SPPPurposeId).Name,
                //PanelOrientation = panel_orientation.Name,
                //PVSystemMaterial = pvsystemmaterial.Name,
                //RatedPower = RatedPower.ToString(),
                //PanelsCount = PanelsCount.ToString(),
                //PanelEfficiency = PanelEfficiency.ToString(),
                //RatedOperatingTemperature = RatedOperatingTemperature.ToString(),
                //ThermalPowerFactor = ThermalPowerFactor.ToString(),
                //Tilt = Tilt.ToString(),
                //Azimuth = Azimuth.ToString(),
                //WiringEfficiency = WiringEfficiency.ToString(),
                //ControllerEfficiency = ControllerEfficiency.ToString(),
                //InverterEfficiency = InverterEfficiency.ToString(),
                //BatteryEfficiency = BatteryEfficiency.ToString(),
                //LossesSoil = LossesSoil.ToString(),
                //LossesSnow = LossesSnow.ToString(),
                //LossesShad = LossesShad.ToString(),
                //LossesMis = LossesMis.ToString(),
                //H_td_average = H_td.Average().ToString("0.00"),
                //η_epv = η_epv.ToString(),
                S_pv = S_pv_s,
                //W_PV = W_PV_s,
                //H_td = H_td_s,
                //H_tah = H_tah_s,
                //H_tm = H_tm_s,
                //Ta = Ta_s,
                //D = D_s,
                //η_tpv = η_tpv_s,
                //η_spv = η_spv_s,
                S_pv_c = S_pv_c,
                P_tpv = P_tpv_s
                //T_c = T_c_s,
                //β_h = β_h_s
            });
        }

        IList<MeteoData> GetMonthlyAverage(decimal X, decimal Y, int MeteoDataTypeId)
        {
            decimal Xleft = 0,
                    Xright = 0,
                    Ybottom = 0,
                    Ytop = 0;
            if (X - (int)X >= 0.5M)
            {
                Xleft = (int)X + 0.5M;
                Xright = Xleft + 1;
            }
            else
            {
                Xleft = (int)X - 0.5M;
                Xright = Xleft + 1;
            }
            if (Y - (int)Y >= 0.5M)
            {
                Ybottom = (int)Y + 0.5M;
                Ytop = Ybottom + 1;
            }
            else
            {
                Ybottom = (int)Y - 0.5M;
                Ytop = Ybottom + 1;
            }
            // расстояния до углов
            decimal Rleftbottom = (decimal)Math.Sqrt(Math.Pow((double)(X - Xleft), 2) + Math.Pow((double)(Y - Ybottom), 2));
            decimal Rlefttop = (decimal)Math.Sqrt(Math.Pow((double)(X - Xleft), 2) + Math.Pow((double)(Y - Ytop), 2));
            decimal Rrighttop = (decimal)Math.Sqrt(Math.Pow((double)(X - Xright), 2) + Math.Pow((double)(Y - Ytop), 2));
            decimal Rrightbottom = (decimal)Math.Sqrt(Math.Pow((double)(X - Xright), 2) + Math.Pow((double)(Y - Ybottom), 2));
            // сумма расстояний
            decimal Rsum = Rleftbottom + Rlefttop + Rrighttop + Rrightbottom;

            // значения в углах
            List<MeteoData> Vleftbottom = db.MeteoDatas
                .Where(m => m.Latitude == Ybottom && m.Longitude == Xleft && m.MeteoDataTypeId == MeteoDataTypeId)
                .ToList();
            List<MeteoData> Vlefttop = db.MeteoDatas
                .Where(m => m.Latitude == Ytop && m.Longitude == Xleft && m.MeteoDataTypeId == MeteoDataTypeId)
                .ToList();
            List<MeteoData> Vrighttop = db.MeteoDatas
                .Where(m => m.Latitude == Ytop && m.Longitude == Xright && m.MeteoDataTypeId == MeteoDataTypeId)
                .ToList();
            List<MeteoData> Vrightbottom = db.MeteoDatas
                .Where(m => m.Latitude == Ybottom && m.Longitude == Xright && m.MeteoDataTypeId == MeteoDataTypeId)
                .ToList();
            // расчет
            IList<MeteoData> r = new List<MeteoData>();

            //// edit
            //Vleftbottom[6].Value = 9.84M;
            //Vlefttop[6].Value = 9.84M;
            //Vrighttop[6].Value = 9.84M;
            //Vrightbottom[6].Value = 9.84M;

            for (int i = 0; i < Vleftbottom.Count; i++)
            {
                r.Add(Vleftbottom[i]);
                r[i].Latitude = Y;
                r[i].Longitude = X;
                r[i].Value = (decimal)(Vleftbottom[i].Value * Rleftbottom + Vlefttop[i].Value * Rlefttop + Vrighttop[i].Value * Rrighttop + Vrightbottom[i].Value * Rrightbottom) / Rsum;
            }
            if (new[] { Vlefttop.Count, Vrighttop.Count, Vrightbottom.Count }.All(x => x == Vleftbottom.Count))
            {
                return r;
            }
            else
            {
                return null;
            }

        }

        IList<MeteoData> GetMeteoData(decimal Longitude, decimal Latitude, int MeteoDataTypeId)
        {
            MeteoDataType mdt = db.MeteoDataTypes
                    .Where(m => m.Id == MeteoDataTypeId)
                    .FirstOrDefault();
            MeteoDataSource mds = db.MeteoDataSources
                .Where(m => m.Id == mdt.MeteoDataSourceId)
                .FirstOrDefault();
            MeteoDataPeriodicity mdp = db.MeteoDataPeriodicities
                .Where(m => m.Id == mdt.MeteoDataPeriodicityId)
                .FirstOrDefault();


            decimal Xleft = 0,
                    Xright = 0,
                    Ybottom = 0,
                    Ytop = 0,
                    step = 0;
            decimal longitude_min = 0,
                    longitude_max = 0,
                    latitude_min = 0,
                    latitude_max = 0;
            //if (mds.Code == Properties.Settings.Default.NASASSECode)
            //{
            //    longitude_min = Properties.Settings.Default.NASASSELongitudeMin;
            //    longitude_max = Properties.Settings.Default.NASASSELongitudeMax;
            //    latitude_min = Properties.Settings.Default.NASASSELatitudeMin;
            //    latitude_max = Properties.Settings.Default.NASASSELatitudeMax;
            //    step = Properties.Settings.Default.NASASSECoordinatesStep;
            //}
            //if (mds.Code == Properties.Settings.Default.NASAPOWERCode)
            //{
            //    longitude_min = Properties.Settings.Default.NASAPOWERLongitudeMin;
            //    longitude_max = Properties.Settings.Default.NASAPOWERLongitudeMax;
            //    latitude_min = Properties.Settings.Default.NASAPOWERLatitudeMin;
            //    latitude_max = Properties.Settings.Default.NASAPOWERLatitudeMax;
            //    step = Properties.Settings.Default.NASAPOWERCoordinatesStep;
            //    if (mdp.Code.ToLower().Contains(Properties.Settings.Default.Average.ToLower()))
            //    {
            //        longitude_min = Properties.Settings.Default.NASASSELongitudeMin;
            //        longitude_max = Properties.Settings.Default.NASASSELongitudeMax;
            //        latitude_min = Properties.Settings.Default.NASASSELatitudeMin;
            //        latitude_max = Properties.Settings.Default.NASASSELatitudeMax;
            //        step = Properties.Settings.Default.NASASSECoordinatesStep;
            //    }
            //}
            //if (mds.Code == Properties.Settings.Default.SARAHECode)
            //{
            //    using (var dblocal = new NpgsqlContext())
            //    {
            //        Option o_longitude_min = dblocal.Options.Where(o => o.Code == Properties.Settings.Default.SARAHELongitudeMinOption).FirstOrDefault();
            //        Option o_longitude_max = dblocal.Options.Where(o => o.Code == Properties.Settings.Default.SARAHELongitudeMaxOption).FirstOrDefault();
            //        Option o_latitude_min = dblocal.Options.Where(o => o.Code == Properties.Settings.Default.SARAHELatitudeMinOption).FirstOrDefault();
            //        Option o_latitude_max = dblocal.Options.Where(o => o.Code == Properties.Settings.Default.SARAHELatitudeMaxOption).FirstOrDefault();
            //        longitude_min = Convert.ToDecimal(o_longitude_min.Value.Replace('.', ','));
            //        longitude_max = Convert.ToDecimal(o_longitude_max.Value.Replace('.', ','));
            //        latitude_min = Convert.ToDecimal(o_latitude_min.Value.Replace('.', ','));
            //        latitude_max = Convert.ToDecimal(o_latitude_max.Value.Replace('.', ','));
            //    }
            //    step = Properties.Settings.Default.SARAHECoordinatesStep;
            //}
            if (mds.Code == Properties.Settings.Default.NASASSECode)
            {
                longitude_min = Properties.Settings.Default.NASASSELongitudeMin;
                longitude_max = Properties.Settings.Default.NASASSELongitudeMax;
                latitude_min = Properties.Settings.Default.NASASSELatitudeMin;
                latitude_max = Properties.Settings.Default.NASASSELatitudeMax;
                step = Properties.Settings.Default.NASASSECoordinatesStep;
            }
            if (mds.Code == Properties.Settings.Default.NASAPOWERCode)
            {
                longitude_min = Properties.Settings.Default.NASAPOWERLongitudeMin;
                longitude_max = Properties.Settings.Default.NASAPOWERLongitudeMax;
                latitude_min = Properties.Settings.Default.NASAPOWERLatitudeMin;
                latitude_max = Properties.Settings.Default.NASAPOWERLatitudeMax;
                step = Properties.Settings.Default.NASAPOWERCoordinatesStep;
                if (mdp.Code.ToLower().Contains(Properties.Settings.Default.Monthly))
                {
                    longitude_min = Properties.Settings.Default.NASASSELongitudeMin;
                    longitude_max = Properties.Settings.Default.NASASSELongitudeMax;
                    latitude_min = Properties.Settings.Default.NASASSELatitudeMin;
                    latitude_max = Properties.Settings.Default.NASASSELatitudeMax;
                    step = Properties.Settings.Default.NASASSECoordinatesStep;
                }
            }
            if (mds.Code == Properties.Settings.Default.SARAHECode)
            {
                using (var dblocal = new NpgsqlContext())
                {
                    Option o_longitude_min = dblocal.Options.Where(o => o.Code == Properties.Settings.Default.SARAHELongitudeMinOption).FirstOrDefault();
                    Option o_longitude_max = dblocal.Options.Where(o => o.Code == Properties.Settings.Default.SARAHELongitudeMaxOption).FirstOrDefault();
                    Option o_latitude_min = dblocal.Options.Where(o => o.Code == Properties.Settings.Default.SARAHELatitudeMinOption).FirstOrDefault();
                    Option o_latitude_max = dblocal.Options.Where(o => o.Code == Properties.Settings.Default.SARAHELatitudeMaxOption).FirstOrDefault();
                    longitude_min = Convert.ToDecimal(o_longitude_min.Value.Replace('.', ','));
                    longitude_max = Convert.ToDecimal(o_longitude_max.Value.Replace('.', ','));
                    latitude_min = Convert.ToDecimal(o_latitude_min.Value.Replace('.', ','));
                    latitude_max = Convert.ToDecimal(o_latitude_max.Value.Replace('.', ','));
                }
                step = Properties.Settings.Default.SARAHECoordinatesStep;
            }
            if (mds.Code == Properties.Settings.Default.CLARACode)
            {
                using (var dblocal = new NpgsqlContext())
                {
                    Option o_longitude_min = dblocal.Options.Where(o => o.Code == Properties.Settings.Default.CLARALongitudeMinOption).FirstOrDefault();
                    Option o_longitude_max = dblocal.Options.Where(o => o.Code == Properties.Settings.Default.CLARALongitudeMaxOption).FirstOrDefault();
                    Option o_latitude_min = dblocal.Options.Where(o => o.Code == Properties.Settings.Default.CLARALatitudeMinOption).FirstOrDefault();
                    Option o_latitude_max = dblocal.Options.Where(o => o.Code == Properties.Settings.Default.CLARALatitudeMaxOption).FirstOrDefault();
                    longitude_min = Convert.ToDecimal(o_longitude_min.Value.Replace('.', ','));
                    longitude_max = Convert.ToDecimal(o_longitude_max.Value.Replace('.', ','));
                    latitude_min = Convert.ToDecimal(o_latitude_min.Value.Replace('.', ','));
                    latitude_max = Convert.ToDecimal(o_latitude_max.Value.Replace('.', ','));
                }
                step = Properties.Settings.Default.CLARACoordinatesStep;
            }
            Xleft = longitude_min;
            Xright = longitude_max;
            Ybottom = latitude_min;
            Ytop = latitude_max;
            // 4 ближайшие точки
            for (decimal longitude = longitude_min; longitude <= longitude_max; longitude += step)
            {
                if (Xleft <= longitude && Xleft <= Longitude && longitude <= Longitude)
                {
                    Xleft = longitude;
                }
            }
            for (decimal longitude = longitude_max; longitude >= longitude_min; longitude -= step)
            {
                if (Xright >= longitude && Xright >= Longitude && longitude >= Longitude)
                {
                    Xright = longitude;
                }
            }
            for (decimal latitude = latitude_min; latitude <= latitude_max; latitude += step)
            {
                if (Ybottom <= latitude && Ybottom <= Latitude && latitude <= Latitude)
                {
                    Ybottom = latitude;
                }
            }
            for (decimal latitude = latitude_max; latitude >= latitude_min; latitude -= step)
            {
                if (Ytop >= latitude && Ytop >= Latitude && latitude >= Latitude)
                {
                    Ytop = latitude;
                }
            }
            // расстояния до углов
            decimal Rleftbottom = (decimal)Math.Sqrt(Math.Pow((double)(Longitude - Xleft), 2) + Math.Pow((double)(Latitude - Ybottom), 2));
            decimal Rlefttop = (decimal)Math.Sqrt(Math.Pow((double)(Longitude - Xleft), 2) + Math.Pow((double)(Latitude - Ytop), 2));
            decimal Rrighttop = (decimal)Math.Sqrt(Math.Pow((double)(Longitude - Xright), 2) + Math.Pow((double)(Latitude - Ytop), 2));
            decimal Rrightbottom = (decimal)Math.Sqrt(Math.Pow((double)(Longitude - Xright), 2) + Math.Pow((double)(Latitude - Ybottom), 2));
            // сумма расстояний
            decimal Rsum = Rleftbottom + Rlefttop + Rrighttop + Rrightbottom;

            // значения в углах
            List<MeteoData> Vleftbottom = db.MeteoDatas
                .Where(m => m.Latitude == Ybottom && m.Longitude == Xleft && m.MeteoDataTypeId == MeteoDataTypeId)
                .ToList()
                .OrderBy(m => m.Year)
                .ThenBy(m => m.Month)
                .ThenBy(m => m.Day)
                .ToList();
            List<MeteoData> Vlefttop = db.MeteoDatas
                .Where(m => m.Latitude == Ytop && m.Longitude == Xleft && m.MeteoDataTypeId == MeteoDataTypeId)
                .ToList()
                .OrderBy(m => m.Year)
                .ThenBy(m => m.Month)
                .ThenBy(m => m.Day)
                .ToList();
            List<MeteoData> Vrighttop = db.MeteoDatas
                .Where(m => m.Latitude == Ytop && m.Longitude == Xright && m.MeteoDataTypeId == MeteoDataTypeId)
                .ToList()
                .OrderBy(m => m.Year)
                .ThenBy(m => m.Month)
                .ThenBy(m => m.Day)
                .ToList();
            List<MeteoData> Vrightbottom = db.MeteoDatas
                .Where(m => m.Latitude == Ybottom && m.Longitude == Xright && m.MeteoDataTypeId == MeteoDataTypeId)
                .ToList()
                .OrderBy(m => m.Year)
                .ThenBy(m => m.Month)
                .ThenBy(m => m.Day)
                .ToList();
            // расчет
            IList<MeteoData> r = new List<MeteoData>();
            for (int i = 0; i < Vleftbottom.Count; i++)
            {
                r.Add(Vleftbottom[i]);
                r[i].Latitude = Latitude;
                r[i].Longitude = Longitude;
                r[i].Value = (decimal)(
                    (Vleftbottom[i].Value == null ? 0 : Vleftbottom[i].Value) * Rleftbottom +
                    (Vlefttop[i].Value == null ? 0 : Vlefttop[i].Value) * Rlefttop +
                    (Vrighttop[i].Value == null ? 0 : Vrighttop[i].Value) * Rrighttop +
                    (Vrightbottom[i].Value == null ? 0 : Vrightbottom[i].Value) * Rrightbottom) / Rsum;
            }
            //foreach (MeteoData md in Vleftbottom)
            //{
            //    if (r.Count(m => m.Year == md.Year && m.Month == md.Month && m.Day == md.Day) == 0)
            //    {
            //        decimal? Vleftbottom_v = Vleftbottom.FirstOrDefault(m => m.Year == md.Year && m.Month == md.Month && m.Day == md.Day).Value;
            //        Vleftbottom_v = Vleftbottom_v == null ? 0 : Vleftbottom_v;
            //        decimal? Vlefttop_v = Vlefttop.FirstOrDefault(m => m.Year == md.Year && m.Month == md.Month && m.Day == md.Day).Value;
            //        Vlefttop_v = Vlefttop_v == null ? 0 : Vlefttop_v;
            //        decimal? Vrighttop_v = Vrighttop.FirstOrDefault(m => m.Year == md.Year && m.Month == md.Month && m.Day == md.Day).Value;
            //        Vrighttop_v = Vrighttop_v == null ? 0 : Vrighttop_v;
            //        decimal? Vrightbottom_v = Vrightbottom.FirstOrDefault(m => m.Year == md.Year && m.Month == md.Month && m.Day == md.Day).Value;
            //        Vrightbottom_v = Vrightbottom_v == null ? 0 : Vrightbottom_v;

            //        r.Add(md);
            //        r.Last().Latitude = Latitude;
            //        r.Last().Longitude = Longitude;
            //        r.Last().Value = (decimal)(
            //            Vleftbottom_v * Rleftbottom +
            //            Vlefttop_v * Rlefttop +
            //            Vrighttop_v * Rrighttop +
            //            Vrightbottom_v * Rrightbottom) / Rsum;
            //    }
            //}
            //foreach (MeteoData md in Vlefttop)
            //{
            //    if (r.Count(m => m.Year == md.Year && m.Month == md.Month && m.Day == md.Day) == 0)
            //    {
            //        decimal? Vleftbottom_v = Vleftbottom.FirstOrDefault(m => m.Year == md.Year && m.Month == md.Month && m.Day == md.Day).Value;
            //        Vleftbottom_v = Vleftbottom_v == null ? 0 : Vleftbottom_v;
            //        decimal? Vlefttop_v = Vlefttop.FirstOrDefault(m => m.Year == md.Year && m.Month == md.Month && m.Day == md.Day).Value;
            //        Vlefttop_v = Vlefttop_v == null ? 0 : Vlefttop_v;
            //        decimal? Vrighttop_v = Vrighttop.FirstOrDefault(m => m.Year == md.Year && m.Month == md.Month && m.Day == md.Day).Value;
            //        Vrighttop_v = Vrighttop_v == null ? 0 : Vrighttop_v;
            //        decimal? Vrightbottom_v = Vrightbottom.FirstOrDefault(m => m.Year == md.Year && m.Month == md.Month && m.Day == md.Day).Value;
            //        Vrightbottom_v = Vrightbottom_v == null ? 0 : Vrightbottom_v;

            //        r.Add(md);
            //        r.Last().Latitude = Latitude;
            //        r.Last().Longitude = Longitude;
            //        r.Last().Value = (decimal)(
            //            Vleftbottom_v * Rleftbottom +
            //            Vlefttop_v * Rlefttop +
            //            Vrighttop_v * Rrighttop +
            //            Vrightbottom_v * Rrightbottom) / Rsum;
            //    }
            //}
            //foreach (MeteoData md in Vrighttop)
            //{
            //    if (r.Count(m => m.Year == md.Year && m.Month == md.Month && m.Day == md.Day) == 0)
            //    {
            //        decimal? Vleftbottom_v = Vleftbottom.FirstOrDefault(m => m.Year == md.Year && m.Month == md.Month && m.Day == md.Day).Value;
            //        Vleftbottom_v = Vleftbottom_v == null ? 0 : Vleftbottom_v;
            //        decimal? Vlefttop_v = Vlefttop.FirstOrDefault(m => m.Year == md.Year && m.Month == md.Month && m.Day == md.Day).Value;
            //        Vlefttop_v = Vlefttop_v == null ? 0 : Vlefttop_v;
            //        decimal? Vrighttop_v = Vrighttop.FirstOrDefault(m => m.Year == md.Year && m.Month == md.Month && m.Day == md.Day).Value;
            //        Vrighttop_v = Vrighttop_v == null ? 0 : Vrighttop_v;
            //        decimal? Vrightbottom_v = Vrightbottom.FirstOrDefault(m => m.Year == md.Year && m.Month == md.Month && m.Day == md.Day).Value;
            //        Vrightbottom_v = Vrightbottom_v == null ? 0 : Vrightbottom_v;

            //        r.Add(md);
            //        r.Last().Latitude = Latitude;
            //        r.Last().Longitude = Longitude;
            //        r.Last().Value = (decimal)(
            //            Vleftbottom_v * Rleftbottom +
            //            Vlefttop_v * Rlefttop +
            //            Vrighttop_v * Rrighttop +
            //            Vrightbottom_v * Rrightbottom) / Rsum;
            //    }
            //}
            //foreach (MeteoData md in Vrightbottom)
            //{
            //    if (r.Count(m => m.Year == md.Year && m.Month == md.Month && m.Day == md.Day) == 0)
            //    {
            //        decimal? Vleftbottom_v = Vleftbottom.FirstOrDefault(m => m.Year == md.Year && m.Month == md.Month && m.Day == md.Day).Value;
            //        Vleftbottom_v = Vleftbottom_v == null ? 0 : Vleftbottom_v;
            //        decimal? Vlefttop_v = Vlefttop.FirstOrDefault(m => m.Year == md.Year && m.Month == md.Month && m.Day == md.Day).Value;
            //        Vlefttop_v = Vlefttop_v == null ? 0 : Vlefttop_v;
            //        decimal? Vrighttop_v = Vrighttop.FirstOrDefault(m => m.Year == md.Year && m.Month == md.Month && m.Day == md.Day).Value;
            //        Vrighttop_v = Vrighttop_v == null ? 0 : Vrighttop_v;
            //        decimal? Vrightbottom_v = Vrightbottom.FirstOrDefault(m => m.Year == md.Year && m.Month == md.Month && m.Day == md.Day).Value;
            //        Vrightbottom_v = Vrightbottom_v == null ? 0 : Vrightbottom_v;

            //        r.Add(md);
            //        r.Last().Latitude = Latitude;
            //        r.Last().Longitude = Longitude;
            //        r.Last().Value = (decimal)(
            //            Vleftbottom_v * Rleftbottom +
            //            Vlefttop_v * Rlefttop +
            //            Vrighttop_v * Rrighttop +
            //            Vrightbottom_v * Rrightbottom) / Rsum;
            //    }
            //}
            return r;
        }

        [HttpPost]
        public JsonResult GetMeteoDataTypes(int MeteoDataPeriodicityId, int MeteoDataSourceId)
        {
            List<int> mdtIds = new List<int>();
            mdtIds.Add(2);
            mdtIds.Add(5);
            mdtIds.Add(8);
            mdtIds.Add(9);
            mdtIds.Add(10);
            mdtIds.Add(14);
            mdtIds.Add(29);
            mdtIds.Add(34);
            mdtIds.Add(97);
            mdtIds.Add(98);
            mdtIds.Add(99);
            mdtIds.Add(102);
            mdtIds.Add(108);
            mdtIds.Add(109);
            mdtIds.Add(110);
            mdtIds.Add(113);
            mdtIds.Add(119);
            mdtIds.Add(120);
            mdtIds.Add(145);
            mdtIds.Add(178);
            mdtIds.Add(179);
            mdtIds.Add(180);
            mdtIds.Add(181);
            mdtIds.Add(186);
            mdtIds.Add(187);
            mdtIds.Add(188);
            mdtIds.Add(189);
            mdtIds.Add(191);
            mdtIds.Add(192);
            mdtIds.Add(195);
            mdtIds.Add(196);
            mdtIds.Add(197);
            mdtIds.Add(198);
            mdtIds.Add(199);
            mdtIds.Add(200);
            mdtIds.Add(209);
            mdtIds.Add(241);
            mdtIds.Add(245);
            mdtIds.Add(247);
            mdtIds.Add(387);
            mdtIds.Add(389);
            mdtIds.Add(390);
            mdtIds.Add(391);
            mdtIds.Add(396);
            mdtIds.Add(397);
            mdtIds.Add(398);
            mdtIds.Add(400);
            mdtIds.Add(402);
            mdtIds.Add(403);
            mdtIds.Add(404);
            mdtIds.Add(405);
            mdtIds.Add(406);
            mdtIds.Add(408);
            mdtIds.Add(409);
            mdtIds.Add(410);
            mdtIds.Add(411);
            mdtIds.Add(412);
            mdtIds.Add(413);
            mdtIds.Add(415);
            mdtIds.Add(417);
            mdtIds.Add(418);
            mdtIds.Add(419);

            var MeteoDataTypesList = db.MeteoDataTypes
                .Where(m => m.MeteoDataPeriodicityId == MeteoDataPeriodicityId && m.MeteoDataSourceId == MeteoDataSourceId)
                .ToList()
                .OrderBy(m => m.NameGroupAdditional);
            MeteoDataTypesList = MeteoDataTypesList
                .Where(m => mdtIds.Contains(m.Id))
                .ToList()
                .OrderBy(m => m.NameGroupAdditional);
            SelectList MeteoDataTypes = new SelectList(MeteoDataTypesList, "Id", "NameGroupAdditional");
            JsonResult result = new JsonResult();
            result.Data = MeteoDataTypes;
            return result;
        }

        [HttpPost]
        public ActionResult GetMeteoData(int MeteoDataTypeId, decimal Longitude, decimal Latitude)
        {
            try
            {
                string userid = User.Identity.GetUserId();
                string path = "~/Download/";

                //удаление предыдущих файлов и папок
                DirectoryInfo di = new DirectoryInfo(Server.MapPath(path));
                foreach (FileInfo file in di.GetFiles())
                {
                    try
                    {
                        file.Delete();
                    }
                    catch
                    {
                    }
                }
                foreach (DirectoryInfo dir in di.GetDirectories())
                {
                    if (!dir.Name.Contains("AnalizeTerrain") && !dir.Name.Contains("Images") && !dir.Name.Contains("csv"))
                    {
                        try
                        {
                            dir.Delete(true);
                        }
                        catch
                        {
                        }
                    }
                }
                path = "~/Download/" + userid;
                if (!Directory.Exists(Server.MapPath(path)))
                {
                    DirectoryInfo di2 = Directory.CreateDirectory(Server.MapPath(path));
                }
                MeteoDataType mdt = db.MeteoDataTypes
                    .Where(m => m.Id == MeteoDataTypeId)
                    .FirstOrDefault();
                MeteoDataSource mds = db.MeteoDataSources
                    .Where(m => m.Id == mdt.MeteoDataSourceId)
                    .FirstOrDefault();
                MeteoDataPeriodicity mdp = db.MeteoDataPeriodicities
                    .Where(m => m.Id == mdt.MeteoDataPeriodicityId)
                    .FirstOrDefault();
                //string filenameout = $"{mds.Name} {mdp.Name} {mdt.NameFull} {Longitude.ToString()}-{Latitude.ToString()}";
                string filenameout = $"{mds.Name} {mdp.Name} {mdt.Id} {Longitude.ToString()}-{Latitude.ToString()}";
                filenameout = Path.GetInvalidFileNameChars().Aggregate(filenameout, (current, c) => current.Replace(c.ToString(), string.Empty));
                filenameout = Path.Combine(Server.MapPath(path), $"{filenameout}.csv");
                // получение данных
                decimal Xleft = 0,
                        Xright = 0,
                        Ybottom = 0,
                        Ytop = 0,
                        step = 0;
                decimal longitude_min = 0,
                        longitude_max = 0,
                        latitude_min = 0,
                        latitude_max = 0;
                //if (mds.Code == Properties.Settings.Default.NASASSECode)
                //{
                //    longitude_min = Properties.Settings.Default.NASASSELongitudeMin;
                //    longitude_max = Properties.Settings.Default.NASASSELongitudeMax;
                //    latitude_min = Properties.Settings.Default.NASASSELatitudeMin;
                //    latitude_max = Properties.Settings.Default.NASASSELatitudeMax;
                //    step = Properties.Settings.Default.NASASSECoordinatesStep;
                //}
                //if (mds.Code == Properties.Settings.Default.NASAPOWERCode)
                //{
                //    longitude_min = Properties.Settings.Default.NASAPOWERLongitudeMin;
                //    longitude_max = Properties.Settings.Default.NASAPOWERLongitudeMax;
                //    latitude_min = Properties.Settings.Default.NASAPOWERLatitudeMin;
                //    latitude_max = Properties.Settings.Default.NASAPOWERLatitudeMax;
                //    step = Properties.Settings.Default.NASAPOWERCoordinatesStep;
                //    if (mdp.Code.ToLower().Contains(Properties.Settings.Default.Average.ToLower()))
                //    {
                //        longitude_min = Properties.Settings.Default.NASASSELongitudeMin;
                //        longitude_max = Properties.Settings.Default.NASASSELongitudeMax;
                //        latitude_min = Properties.Settings.Default.NASASSELatitudeMin;
                //        latitude_max = Properties.Settings.Default.NASASSELatitudeMax;
                //        step = Properties.Settings.Default.NASASSECoordinatesStep;
                //    }
                //}
                //if (mds.Code == Properties.Settings.Default.SARAHECode)
                //{
                //    using (var dblocal = new NpgsqlContext())
                //    {
                //        Option o_longitude_min = dblocal.Options.Where(o => o.Code == Properties.Settings.Default.SARAHELongitudeMinOption).FirstOrDefault();
                //        Option o_longitude_max = dblocal.Options.Where(o => o.Code == Properties.Settings.Default.SARAHELongitudeMaxOption).FirstOrDefault();
                //        Option o_latitude_min = dblocal.Options.Where(o => o.Code == Properties.Settings.Default.SARAHELatitudeMinOption).FirstOrDefault();
                //        Option o_latitude_max = dblocal.Options.Where(o => o.Code == Properties.Settings.Default.SARAHELatitudeMaxOption).FirstOrDefault();
                //        longitude_min = Convert.ToDecimal(o_longitude_min.Value.Replace('.', ','));
                //        longitude_max = Convert.ToDecimal(o_longitude_max.Value.Replace('.', ','));
                //        latitude_min = Convert.ToDecimal(o_latitude_min.Value.Replace('.', ','));
                //        latitude_max = Convert.ToDecimal(o_latitude_max.Value.Replace('.', ','));
                //    }
                //    step = Properties.Settings.Default.SARAHECoordinatesStep;
                //}
                if (mds.Code == Properties.Settings.Default.NASASSECode)
                {
                    longitude_min = Properties.Settings.Default.NASASSELongitudeMin;
                    longitude_max = Properties.Settings.Default.NASASSELongitudeMax;
                    latitude_min = Properties.Settings.Default.NASASSELatitudeMin;
                    latitude_max = Properties.Settings.Default.NASASSELatitudeMax;
                    step = Properties.Settings.Default.NASASSECoordinatesStep;
                }
                if (mds.Code == Properties.Settings.Default.NASAPOWERCode)
                {
                    longitude_min = Properties.Settings.Default.NASAPOWERLongitudeMin;
                    longitude_max = Properties.Settings.Default.NASAPOWERLongitudeMax;
                    latitude_min = Properties.Settings.Default.NASAPOWERLatitudeMin;
                    latitude_max = Properties.Settings.Default.NASAPOWERLatitudeMax;
                    step = Properties.Settings.Default.NASAPOWERCoordinatesStep;
                    if (mdp.Code.ToLower().Contains(Properties.Settings.Default.Monthly))
                    {
                        longitude_min = Properties.Settings.Default.NASASSELongitudeMin;
                        longitude_max = Properties.Settings.Default.NASASSELongitudeMax;
                        latitude_min = Properties.Settings.Default.NASASSELatitudeMin;
                        latitude_max = Properties.Settings.Default.NASASSELatitudeMax;
                        step = Properties.Settings.Default.NASASSECoordinatesStep;
                    }
                }
                if (mds.Code == Properties.Settings.Default.SARAHECode)
                {
                    using (var dblocal = new NpgsqlContext())
                    {
                        Option o_longitude_min = dblocal.Options.Where(o => o.Code == Properties.Settings.Default.SARAHELongitudeMinOption).FirstOrDefault();
                        Option o_longitude_max = dblocal.Options.Where(o => o.Code == Properties.Settings.Default.SARAHELongitudeMaxOption).FirstOrDefault();
                        Option o_latitude_min = dblocal.Options.Where(o => o.Code == Properties.Settings.Default.SARAHELatitudeMinOption).FirstOrDefault();
                        Option o_latitude_max = dblocal.Options.Where(o => o.Code == Properties.Settings.Default.SARAHELatitudeMaxOption).FirstOrDefault();
                        longitude_min = Convert.ToDecimal(o_longitude_min.Value.Replace('.', ','));
                        longitude_max = Convert.ToDecimal(o_longitude_max.Value.Replace('.', ','));
                        latitude_min = Convert.ToDecimal(o_latitude_min.Value.Replace('.', ','));
                        latitude_max = Convert.ToDecimal(o_latitude_max.Value.Replace('.', ','));
                    }
                    step = Properties.Settings.Default.SARAHECoordinatesStep;
                }
                if (mds.Code == Properties.Settings.Default.CLARACode)
                {
                    using (var dblocal = new NpgsqlContext())
                    {
                        Option o_longitude_min = dblocal.Options.Where(o => o.Code == Properties.Settings.Default.CLARALongitudeMinOption).FirstOrDefault();
                        Option o_longitude_max = dblocal.Options.Where(o => o.Code == Properties.Settings.Default.CLARALongitudeMaxOption).FirstOrDefault();
                        Option o_latitude_min = dblocal.Options.Where(o => o.Code == Properties.Settings.Default.CLARALatitudeMinOption).FirstOrDefault();
                        Option o_latitude_max = dblocal.Options.Where(o => o.Code == Properties.Settings.Default.CLARALatitudeMaxOption).FirstOrDefault();
                        longitude_min = Convert.ToDecimal(o_longitude_min.Value.Replace('.', ','));
                        longitude_max = Convert.ToDecimal(o_longitude_max.Value.Replace('.', ','));
                        latitude_min = Convert.ToDecimal(o_latitude_min.Value.Replace('.', ','));
                        latitude_max = Convert.ToDecimal(o_latitude_max.Value.Replace('.', ','));
                    }
                    step = Properties.Settings.Default.CLARACoordinatesStep;
                }
                Xleft = longitude_min;
                Xright = longitude_max;
                Ybottom = latitude_min;
                Ytop = latitude_max;
                // 4 ближайшие точки
                for (decimal longitude = longitude_min; longitude <= longitude_max; longitude += step)
                {
                    if (Xleft <= longitude && Xleft <= Longitude && longitude <= Longitude)
                    {
                        Xleft = longitude;
                    }
                }
                for (decimal longitude = longitude_max; longitude >= longitude_min; longitude -= step)
                {
                    if (Xright >= longitude && Xright >= Longitude && longitude >= Longitude)
                    {
                        Xright = longitude;
                    }
                }
                for (decimal latitude = latitude_min; latitude <= latitude_max; latitude += step)
                {
                    if (Ybottom <= latitude && Ybottom <= Latitude && latitude <= Latitude)
                    {
                        Ybottom = latitude;
                    }
                }
                for (decimal latitude = latitude_max; latitude >= latitude_min; latitude -= step)
                {
                    if (Ytop >= latitude && Ytop >= Latitude && latitude >= Latitude)
                    {
                        Ytop = latitude;
                    }
                }
                // расстояния до углов
                decimal Rleftbottom = (decimal)Math.Sqrt(Math.Pow((double)(Longitude - Xleft), 2) + Math.Pow((double)(Latitude - Ybottom), 2));
                decimal Rlefttop = (decimal)Math.Sqrt(Math.Pow((double)(Longitude - Xleft), 2) + Math.Pow((double)(Latitude - Ytop), 2));
                decimal Rrighttop = (decimal)Math.Sqrt(Math.Pow((double)(Longitude - Xright), 2) + Math.Pow((double)(Latitude - Ytop), 2));
                decimal Rrightbottom = (decimal)Math.Sqrt(Math.Pow((double)(Longitude - Xright), 2) + Math.Pow((double)(Latitude - Ybottom), 2));
                // сумма расстояний
                decimal Rsum = Rleftbottom + Rlefttop + Rrighttop + Rrightbottom;
                // значения в углах
                List<MeteoData> Vleftbottom = db.MeteoDatas
                    .Where(m => m.Latitude == Ybottom && m.Longitude == Xleft && m.MeteoDataTypeId == MeteoDataTypeId)
                    .OrderBy(m => m.Year)
                    .ThenBy(m => m.Month)
                    .ThenBy(m => m.Day)
                    .ToList();
                List<MeteoData> Vlefttop = db.MeteoDatas
                    .Where(m => m.Latitude == Ytop && m.Longitude == Xleft && m.MeteoDataTypeId == MeteoDataTypeId)
                    .OrderBy(m => m.Year)
                    .ThenBy(m => m.Month)
                    .ThenBy(m => m.Day)
                    .ToList();
                List<MeteoData> Vrighttop = db.MeteoDatas
                    .Where(m => m.Latitude == Ytop && m.Longitude == Xright && m.MeteoDataTypeId == MeteoDataTypeId)
                    .OrderBy(m => m.Year)
                    .ThenBy(m => m.Month)
                    .ThenBy(m => m.Day)
                    .ToList();
                List<MeteoData> Vrightbottom = db.MeteoDatas
                    .Where(m => m.Latitude == Ybottom && m.Longitude == Xright && m.MeteoDataTypeId == MeteoDataTypeId)
                    .OrderBy(m => m.Year)
                    .ThenBy(m => m.Month)
                    .ThenBy(m => m.Day)
                    .ToList();
                // расчет
                IList<MeteoData> mdl = new List<MeteoData>();
                for (int i = 0; i < Vleftbottom.Count; i++)
                {
                    mdl.Add(Vleftbottom[i]);
                    mdl[i].Latitude = Latitude;
                    mdl[i].Longitude = Longitude;
                    mdl[i].Value = Vleftbottom[i].Value == null || Vlefttop[i].Value == null || Vrighttop[i].Value == null || Vrightbottom[i].Value == null ? (decimal?)null : (decimal)(Vleftbottom[i].Value * Rleftbottom + Vlefttop[i].Value * Rlefttop + Vrighttop[i].Value * Rrighttop + Vrightbottom[i].Value * Rrightbottom) / Rsum;
                }
                // сохранение в файл
                using (var sw = new StreamWriter(
                        new FileStream(filenameout, FileMode.CreateNew, FileAccess.Write),
                        Encoding.UTF8))
                {
                    sw.WriteLine(Resources.Common.MeteoDataSource + ";" + mds.Name);
                    sw.WriteLine(Resources.Common.MeteoDataPeriodicity + ";" + mdp.Name);
                    sw.WriteLine(Resources.Common.MeteoDataType + ";" + mdt.NameFull);
                    sw.WriteLine(Resources.Common.Longitude + ";" + Longitude.ToString());
                    sw.WriteLine(Resources.Common.Latitude + ";" + Latitude.ToString());
                    sw.WriteLine(Resources.Common.Year.ToString() + ";" +
                        Resources.Common.Month.ToString() + ";" +
                        Resources.Common.Day.ToString() + ";" +
                        Resources.Common.Value.ToString()
                        );
                    for (int i = 0; i < mdl.Count; i++)
                    {
                        sw.WriteLine(mdl[i].Year.ToString() + ";" +
                            mdl[i].Month.ToString() + ";" +
                            mdl[i].Day.ToString() + ";" +
                            mdl[i].Value.ToString()
                            );
                    }
                }

                return Json(new
                {
                    FilePath = "/Download/" + userid + "/" + Path.GetFileName(filenameout)
                });
            }
            catch (Exception e)
            {
                return Json(new
                {
                    Error = e.Message
                });
            }
        }

        [HttpPost]
        public ActionResult ComparePoints(int MeteoDataTypeId, decimal? Longitude1, decimal? Latitude1, decimal? Longitude2, decimal? Latitude2, decimal? Longitude3, decimal? Latitude3)
        {
            MeteoDataType mdt = db.MeteoDataTypes.FirstOrDefault(m => m.Id == MeteoDataTypeId);
            MeteoDataPeriodicity mdp = db.MeteoDataPeriodicities.FirstOrDefault(m => m.Id == mdt.MeteoDataPeriodicityId);

            List<MeteoData> p1 = new List<MeteoData>();
            List<MeteoData> p2 = new List<MeteoData>();
            List<MeteoData> p3 = new List<MeteoData>();
            if (Longitude1 != null && Latitude1 != null)
            {
                p1 = GetMeteoData((decimal)Longitude1, (decimal)Latitude1, MeteoDataTypeId).ToList();
            }
            if (Longitude2 != null && Latitude2 != null)
            {
                p2 = GetMeteoData((decimal)Longitude2, (decimal)Latitude2, MeteoDataTypeId).ToList();
            }
            if (Longitude3 != null && Latitude3 != null)
            {
                p3 = GetMeteoData((decimal)Longitude3, (decimal)Latitude3, MeteoDataTypeId).ToList();
            }

            // данные для графика
            List<PlotDataS> xaxis = new List<PlotDataS>();
            List<PlotDataS> r1 = new List<PlotDataS>();
            if (mdp.Code.ToLower().Contains(Properties.Settings.Default.Average.ToLower()) && mdp.Code.ToLower().Contains(Properties.Settings.Default.Monthly.ToLower()))
            {
                for (int i = 0; i < p1.Count() - 1; i++)
                {
                    r1.Add(new PlotDataS()
                    {
                        Y = (decimal)p1[i].Value,
                        X = (i + 1).ToString()
                    });
                    xaxis.Add(new PlotDataS()
                    {
                        Y = i,
                        X = (i + 1) == 13 ? Resources.Common.Year : (i + 1).ToString()
                    });
                }
            }
            else
            if (mdp.Code.ToLower().Contains(Properties.Settings.Default.Daily.ToLower()))
            {
                for (int i = 0; i < p1.Count(); i++)
                {
                    r1.Add(new PlotDataS()
                    {
                        Y = (decimal)p1[i].Value,
                        X = (i + 1).ToString()
                    });
                    xaxis.Add(new PlotDataS()
                    {
                        Y = i,
                        X = (i == 0 || i == p1.Count() - 1) ? ((int)p1[i].Year).ToString() : ""
                    });
                }
            }
            else
            {
                for (int i = 0; i < p1.Count(); i++)
                {
                    r1.Add(new PlotDataS()
                    {
                        Y = (decimal)p1[i].Value,
                        X = (i + 1).ToString()
                    });
                    xaxis.Add(new PlotDataS()
                    {
                        Y = i,
                        X = (i == 0 || i == p1.Count() - 1) ? ((int)p1[i].Year).ToString() : ""
                    });
                }
            }
            List<PlotDataS> r2 = new List<PlotDataS>();
            if (mdp.Code.ToLower().Contains(Properties.Settings.Default.Average.ToLower()) && mdp.Code.ToLower().Contains(Properties.Settings.Default.Monthly.ToLower()))
            {
                for (int i = 0; i < p2.Count() - 1; i++)
                {
                    r2.Add(new PlotDataS()
                    {
                        Y = (decimal)p2[i].Value,
                        X = (i + 1).ToString()
                    });
                }
            }
            else
            if (mdp.Code.ToLower().Contains(Properties.Settings.Default.Daily.ToLower()))
            {
                for (int i = 0; i < p2.Count(); i++)
                {
                    r2.Add(new PlotDataS()
                    {
                        Y = (decimal)p2[i].Value,
                        X = (i + 1).ToString()
                    });
                }
            }
            else
            {
                for (int i = 0; i < p2.Count(); i++)
                {
                    r2.Add(new PlotDataS()
                    {
                        Y = (decimal)p2[i].Value,
                        X = (i + 1).ToString()
                    });
                }
            }
            List<PlotDataS> r3 = new List<PlotDataS>();
            if (mdp.Code.ToLower().Contains(Properties.Settings.Default.Average.ToLower()) && mdp.Code.ToLower().Contains(Properties.Settings.Default.Monthly.ToLower()))
            {
                for (int i = 0; i < p3.Count() - 1; i++)
                {
                    r3.Add(new PlotDataS()
                    {
                        Y = (decimal)p3[i].Value,
                        X = (i + 1).ToString()
                    });
                }
            }
            else
            if (mdp.Code.ToLower().Contains(Properties.Settings.Default.Daily.ToLower()))
            {
                for (int i = 0; i < p3.Count(); i++)
                {
                    r3.Add(new PlotDataS()
                    {
                        Y = (decimal)p3[i].Value,
                        X = (i + 1).ToString()
                    });
                }
            }
            else
            {
                for (int i = 0; i < p3.Count(); i++)
                {
                    r3.Add(new PlotDataS()
                    {
                        Y = (decimal)p3[i].Value,
                        X = (i + 1).ToString()
                    });
                }
            }
            return Json(new
            {
                P1 = r1,
                P2 = r2,
                P3 = r3,
                XAxis = xaxis
            });
        }

        [HttpPost]
        public ActionResult GetOPTANG(decimal? Longitude1, decimal? Latitude1)
        {
            MeteoDataPeriodicity mdp = db.MeteoDataPeriodicities
                .Where(m => m.Code.ToLower().Contains(Properties.Settings.Default.Monthly.ToLower()) && m.Code.ToLower().Contains(Properties.Settings.Default.Average.ToLower()))
                .FirstOrDefault();
            MeteoDataSource mds = db.MeteoDataSources
                .Where(m => m.Code == Properties.Settings.Default.NASASSECode)
                .FirstOrDefault();

            MeteoDataType mdt = db.MeteoDataTypes
                .Where(m => m.MeteoDataPeriodicityId == mdp.Id && m.MeteoDataSourceId == mds.Id && m.Code == "ret_tlt0" && m.AdditionalEN == "OPT ANG")
                .FirstOrDefault();

            List<MeteoData> p1 = new List<MeteoData>();
            if (Longitude1 != null && Latitude1 != null)
            {
                p1 = GetMeteoData((decimal)Longitude1, (decimal)Latitude1, mdt.Id).ToList();
            }
            p1 = p1.OrderBy(p => p.Month).ToList();
            decimal[] opt_ang = new decimal[12];
            for (int i = 0; i < p1.Count() - 1; i++)
            {
                opt_ang[i] = (decimal)p1[i].Value;
            }

            return Json(new
            {
                OPT_ANG = opt_ang
            });
        }

        [HttpPost]
        public JsonResult GetSPPs()
        {
            //List<SPP> spps = new List<SPP>()
            //{
            //    new SPP()
            //    {
            //        Name = "One",
            //        Coordinates = "77,43"
            //    },
            //    new SPP()
            //    {
            //        Name = "Two",
            //        Coordinates = "78,44"
            //    }
            //};
            List<SPP> spps = db.SPPs.ToList();
            JObject o = JObject.FromObject(new
            {
                type = "FeatureCollection",
                crs = new
                {
                    type = "name",
                    properties = new
                    {
                        name = "urn:ogc:def:crs:EPSG::3857"
                    }
                },
                features = from spp in spps
                           select new
                           {
                               type = "Feature",
                               properties = new
                               {
                                   name = spp.Name
                               },
                               geometry = new
                               {
                                   type = "Point",
                                   coordinates = new List<decimal>
                                {
                                    Convert.ToDecimal(spp.Coordinates.Split(',')[0].Replace('.',',')),
                                    Convert.ToDecimal(spp.Coordinates.Split(',')[1].Replace('.',','))
                                },
                               }
                           }
            });

            string jsonstr = o.ToString();

            JsonResult result = new JsonResult();
            result.Data = jsonstr;//result.Data = "{\"type\": \"FeatureCollection\",\"crs\": { \"type\": \"name\", \"properties\": { \"name\": \"urn:ogc:def:crs:EPSG::3857\" }},\"features\": [{ \"type\": \"Feature\", \"properties\": { \"OBJECTID\": 5, \"count\": 0, \"power\": 0.5, \"cost\": \"270 000 000 тенге\", \"startup\": 2017, \"link\": \"www.samruk-green.kz\", \"customer\": \"ТОО «Samruk-Green Energy»\", \"investor\": \"ТОО «Samruk-Green Energy»\", \"executor\": \"ПСД - АО КазНИПИИТЭС «Энергия»\", \"name\": \"СЭС 2,5 МВт в г. Капшагай\", \"percent\": 18.0, \"statusid\": 1, \"purposeid\": 1, \"pvorientid\": 5 }, \"geometry\": { \"type\": \"Point\", \"coordinates\": [ 8580101.2472526263, 5448245.8648402235 ]} }]}";
            return result;
        }

        [HttpPost]
        public JsonResult GetSPPsFromMap()
        {
            int Id = Convert.ToInt32(Request["Id"]);
            SPP spp = new SPP();
            if (Id > 0)
            {
                spp = db.SPPs.FirstOrDefault(s => s.Id == Id);
            }
            else
            {
                spp = new SPP();
            }

            //spp.Id = Convert.ToInt32(Request["Id"]);
            spp.Name = Request["Name"];
            spp.Count = Request["Count"] != "" ? (int?)Convert.ToInt32(Request["Count"]) : null;
            spp.Power = Request["Power"] != "" ? (decimal?)Convert.ToDecimal(Request["Power"]) : null;
            spp.Cost = Request["Cost"];
            spp.Startup = Request["Startup"] != "" ? (int?)Convert.ToInt32(Request["Startup"]) : null;
            spp.Link = Request["Link"];
            spp.Customer = Request["Customer"];
            spp.Investor = Request["Investor"];
            spp.Executor = Request["Executor"];
            spp.CapacityFactor = Request["CapacityFactor"] != "" ? (decimal?)Convert.ToDecimal(Request["CapacityFactor"]) : null;
            spp.Coordinates = Request["Coordinates"];
            spp.SPPStatusId = Convert.ToInt32(Request["SPPStatusId"]);
            spp.SPPPurposeId = Convert.ToInt32(Request["SPPPurposeId"]);
            spp.PanelOrientationId = Convert.ToInt32(Request["PanelOrientationId"]);
            for (int i = 0; i < Request.Files.Count; i++)
            {
                HttpPostedFileBase file = Request.Files[i]; //Uploaded file
                                                            //Use the following properties to get file's name, size and MIMEType
                                                            //int fileSize = file.ContentLength;
                                                            //string fileName = file.FileName;
                                                            //string mimeType = file.ContentType;
                                                            //System.IO.Stream fileContent = file.InputStream;
                                                            ////To save file, use SaveAs method
                                                            //file.SaveAs(Server.MapPath("~/") + fileName); //File will be saved in application root
                if (file != null)
                {
                    MemoryStream target = new MemoryStream();
                    file.InputStream.CopyTo(target);
                    spp.Photo = target.ToArray();
                }
            }
            if (Id > 0)
            {
                SPP spp_db = db.SPPs.FirstOrDefault(s => s.Id == spp.Id);
                if (spp.Photo == null)
                {
                    spp.Photo = spp_db.Photo;
                }
                db.Entry(spp).State = EntityState.Modified;
                db.SaveChanges();

                string comment = $"Id: {spp_db.Id}";
                comment += $" Count: {spp_db.Count.ToString()}";
                comment += $" Power: {spp_db.Power.ToString()}";
                comment += $" Cost: {spp_db.Cost}";
                comment += $" Startup: {spp_db.Startup.ToString()}";
                comment += $" Link: {spp_db.Link}";
                comment += $" Customer: {spp_db.Customer}";
                comment += $" Investor: {spp_db.Investor}";
                comment += $" Executor: {spp_db.Executor}";
                comment += $" CapacityFactor: {spp_db.CapacityFactor.ToString()}";
                comment += $" SPPStatusId: {spp_db.SPPStatusId.ToString()}";
                comment += $" SPPPurposeId: {spp_db.SPPPurposeId.ToString()}";
                comment += $" PanelOrientationId: {spp_db.PanelOrientationId.ToString()}";
                comment += $" Coordinates: {spp_db.Coordinates}";
                comment = $" ->";
                comment += $" Count: {spp.Count.ToString()}";
                comment += $" Power: {spp.Power.ToString()}";
                comment += $" Cost: {spp.Cost}";
                comment += $" Startup: {spp.Startup.ToString()}";
                comment += $" Link: {spp.Link}";
                comment += $" Customer: {spp.Customer}";
                comment += $" Investor: {spp.Investor}";
                comment += $" Executor: {spp.Executor}";
                comment += $" CapacityFactor: {spp.CapacityFactor.ToString()}";
                comment += $" SPPStatusId: {spp.SPPStatusId.ToString()}";
                comment += $" SPPPurposeId: {spp.SPPPurposeId.ToString()}";
                comment += $" PanelOrientationId: {spp.PanelOrientationId.ToString()}";
                comment += $" Coordinates: {spp.Coordinates}";
                SystemLog.New("SPPEdit", comment, null, false);
            }
            else
            {
                db.SPPs.Add(spp);
                db.SaveChanges();
                string comment = $"Id: {Id}";
                comment += $" Count: {spp.Count.ToString()}";
                comment += $" Power: {spp.Power.ToString()}";
                comment += $" Cost: {spp.Cost}";
                comment += $" Startup: {spp.Startup.ToString()}";
                comment += $" Link: {spp.Link}";
                comment += $" Customer: {spp.Customer}";
                comment += $" Investor: {spp.Investor}";
                comment += $" Executor: {spp.Executor}";
                comment += $" CapacityFactor: {spp.CapacityFactor.ToString()}";
                comment += $" SPPStatusId: {spp.SPPStatusId.ToString()}";
                comment += $" SPPPurposeId: {spp.SPPPurposeId.ToString()}";
                comment += $" PanelOrientationId: {spp.PanelOrientationId.ToString()}";
                comment += $" Coordinates: {spp.Coordinates}";
                SystemLog.New("SPPCreate", comment, null, false);
            }
            JsonResult result = new JsonResult();
            result.Data = spp.Id;
            return result;
        }

        [HttpPost]
        public JsonResult GetSPPPhoto(int SPPId)
        {
            JsonResult result = new JsonResult();
            SPP spp = db.SPPs.FirstOrDefault(s => s.Id == SPPId);
            if (spp != null)
            {
                var base64 = "";
                var imgSrc = "";
                if (spp.Photo != null)
                {
                    base64 = Convert.ToBase64String(spp.Photo);
                    imgSrc = String.Format("data:image/gif;base64,{0}", base64);
                }
                result.Data = imgSrc;
                return result;
            }
            else
            {
                result.Data = null;
                return null;
            }

        }

        //decimal sin(decimal degree)
        //{
        //    return (decimal)Math.Sin((double)degree / 180.0 * Math.PI);
        //}

        //decimal cos(decimal degree)
        //{
        //    return (decimal)Math.Cos((double)degree / 180.0 * Math.PI);
        //}

        //decimal tan(decimal degree)
        //{
        //    return (decimal)Math.Tan((double)degree / 180.0 * Math.PI);
        //}

        //decimal asin(decimal value)
        //{
        //    return (decimal)(Math.Asin((double)value) * 180.0 / Math.PI);
        //}

        //decimal acos(decimal value)
        //{
        //    return (decimal)(Math.Acos((double)value) * 180.0 / Math.PI);
        //}

        //decimal atan(decimal value)
        //{
        //    return (decimal)(Math.Atan((double)value) * 180.0 / Math.PI);
        //}

        [HttpPost]
        public ActionResult CalcEfficiency(
            decimal? Longitude,
            decimal? Latitude,
            int MeteoDataSourceId,
            int SPPPurposeId,
            int PanelOrientationId,
            int PVSystemMaterialId,
            //decimal? RatedPower,
            //int? PanelsCount,
            decimal? PanelsArea,
            decimal? PanelEfficiency,
            int? RatedOperatingTemperature,
            decimal? ThermalPowerFactor,
            decimal? WiringEfficiency,
            decimal? ControllerEfficiency,
            decimal? InverterEfficiency,
            decimal? BatteryEfficiency,
            int? Azimuth,
            int? Tilt,
            decimal? LossesSoil,
            decimal? LossesSnow,
            int[] LossesSnowMoths,
            decimal? LossesShad,
            decimal? LossesMis
            )
        {
            decimal longitude = Longitude == null ? 0 : (decimal)Longitude;
            decimal latitude = Latitude == null ? 0 : (decimal)Latitude;
            PanelOrientation panel_orientation = db.PanelOrientations.FirstOrDefault(p => p.Id == PanelOrientationId);
            decimal azimuth = Azimuth == null ? 0 : (int)Azimuth;
            decimal tilt = Tilt == null ? 0 : (int)Tilt;
            int NOCT = RatedOperatingTemperature == null ? 0 : (int)RatedOperatingTemperature;
            decimal panel_efficiency = PanelEfficiency == null ? 0 : (decimal)PanelEfficiency;
            decimal thermal_power_factor = ThermalPowerFactor == null ? 0 : (decimal)ThermalPowerFactor;
            decimal wiring_efficiency = WiringEfficiency == null ? 0 : (decimal)WiringEfficiency;
            decimal controller_efficiency = ControllerEfficiency == null ? 0 : (decimal)ControllerEfficiency;
            decimal inverter_efficiency = InverterEfficiency == null ? 0 : (decimal)InverterEfficiency;
            decimal battery_efficiency = BatteryEfficiency == null ? 0 : (decimal)BatteryEfficiency;
            //int panels_count = PanelsCount == null ? 0 : (int)PanelsCount;
            //decimal rated_power = RatedPower == null ? 0 : (decimal)RatedPower;
            decimal panels_area = PanelsArea == null ? 0 : (decimal)PanelsArea;
            decimal losses_soil = LossesSoil == null ? 0 : (decimal)LossesSoil;
            decimal losses_snow = LossesSnow == null ? 0 : (decimal)LossesSnow;
            decimal losses_shad = LossesShad == null ? 0 : (decimal)LossesShad;
            decimal losses_mis = LossesMis == null ? 0 : (decimal)LossesMis;

            //int SPPPurposeId,
            //int PVSystemMaterialId,
            //decimal? ThermalPowerFactor,


            PVSystemMaterial pvsystemmaterial = db.PVSystemMaterials.Where(p => p.Id == PVSystemMaterialId).FirstOrDefault();

            int NASASSEId = db.MeteoDataSources.Where(m => m.Code.ToLower() == Properties.Settings.Default.NASASSECode.ToLower()).FirstOrDefault().Id;
            int NASAPOWERId = db.MeteoDataSources.Where(m => m.Code.ToLower() == Properties.Settings.Default.NASAPOWERCode.ToLower()).FirstOrDefault().Id;
            int PeriodicityMAId = db.MeteoDataPeriodicities.Where(m => m.Code.ToLower().Contains(Properties.Settings.Default.Monthly) && m.Code.ToLower().Contains(Properties.Settings.Default.Average)).FirstOrDefault().Id;

            // количество дней по месяцам
            decimal[] days = new decimal[12];
            days[0] = 31;
            days[1] = 113 / 4;
            days[2] = 31;
            days[3] = 30;
            days[4] = 31;
            days[5] = 30;
            days[6] = 31;
            days[7] = 31;
            days[8] = 30;
            days[9] = 31;
            days[10] = 30;
            days[11] = 31;

            // Monthly Averaged Insolation Incident On A Horizontal Surface (kWh/m2/day)
            IList<MeteoData> H = null;
            // Monthly Averaged Diffuse Radiation Incident On A Horizontal Surface (kWh/m2/day)
            IList<MeteoData> Hd = null;
            MeteoDataSource mds = db.MeteoDataSources
                .Where(m => m.Id == MeteoDataSourceId)
                .FirstOrDefault();
            if (mds.Code == Properties.Settings.Default.NASASSECode)
            {
                int HMeteoDataTypeId = db.MeteoDataTypes
                    .Where(m => m.Code == "swv_dwn" && m.MeteoDataSourceId == NASASSEId && m.MeteoDataPeriodicityId == PeriodicityMAId)
                    .FirstOrDefault().Id;
                H = GetMonthlyAverage(longitude, latitude, HMeteoDataTypeId);
                for (int i = 1; i <= 12; i++)
                {
                    H.FirstOrDefault(h => h.Month == i).Value = H.FirstOrDefault(h => h.Month == i).Value == null ? 0 : H.FirstOrDefault(h => h.Month == i).Value;
                }

                int HdMeteoDataTypeId = db.MeteoDataTypes
                    .Where(m => m.Code == "exp_dif" && m.MeteoDataSourceId == NASASSEId && m.MeteoDataPeriodicityId == PeriodicityMAId && m.AdditionalEN == "22-year Average")
                    .FirstOrDefault().Id;
                Hd = GetMonthlyAverage(longitude, latitude, HdMeteoDataTypeId);
                for (int i = 1; i <= 12; i++)
                {
                    Hd.FirstOrDefault(h => h.Month == i).Value = Hd.FirstOrDefault(h => h.Month == i).Value == null ? 0 : Hd.FirstOrDefault(h => h.Month == i).Value;
                }
            }
            else
            if (mds.Code == Properties.Settings.Default.SARAHECode)
            {
                int HMeteoDataTypeId = db.MeteoDataTypes
                    .Where(m => m.Code == "SIS" && m.MeteoDataSourceId == MeteoDataSourceId && m.MeteoDataPeriodicityId == PeriodicityMAId)
                    .FirstOrDefault().Id;
                H = GetMonthlyAverage(longitude, latitude, HMeteoDataTypeId);
                for (int i = 1; i <= 12; i++)
                {
                    H.FirstOrDefault(h => h.Month == i).Value = H.FirstOrDefault(h => h.Month == i).Value == null ? 0 : H.FirstOrDefault(h => h.Month == i).Value;
                }

                int HdMeteoDataTypeId = db.MeteoDataTypes
                    .Where(m => m.Code == "SID" && m.MeteoDataSourceId == MeteoDataSourceId && m.MeteoDataPeriodicityId == PeriodicityMAId)
                    .FirstOrDefault().Id;
                Hd = GetMonthlyAverage(longitude, latitude, HdMeteoDataTypeId);
                for (int i = 1; i <= 12; i++)
                {
                    Hd.FirstOrDefault(h => h.Month == i).Value = Hd.FirstOrDefault(h => h.Month == i).Value == null ? 0 : Hd.FirstOrDefault(h => h.Month == i).Value;
                }
                for (int i = 1; i <= 12; i++)
                {
                    Hd.FirstOrDefault(h => h.Month == i).Value = H.FirstOrDefault(h => h.Month == i).Value - Hd.FirstOrDefault(h => h.Month == i).Value;
                }

                for (int i = 1; i <= 12; i++)
                {
                    H.FirstOrDefault(h => h.Month == i).Value = H.FirstOrDefault(h => h.Month == i).Value * 0.024M;
                    Hd.FirstOrDefault(h => h.Month == i).Value = H.FirstOrDefault(h => h.Month == i).Value * 0.024M;
                }
            }

            // Monthly Averaged Surface Albedo
            int PsMeteoDataTypeId = db.MeteoDataTypes
                .Where(m => m.Code == "srf_alb" && m.MeteoDataSourceId == NASASSEId && m.MeteoDataPeriodicityId == PeriodicityMAId)
                .FirstOrDefault().Id;
            IList<MeteoData> Ps = GetMonthlyAverage(longitude, latitude, PsMeteoDataTypeId);
            for (int i = 1; i <= 12; i++)
            {
                Ps.FirstOrDefault(p => p.Month == i).Value = Ps.FirstOrDefault(p => p.Month == i).Value == null ? 0 : Ps.FirstOrDefault(p => p.Month == i).Value;
            }

            // The angle relative to the horizontal for which the monthly averaged total solar radiation is a maximum (degrees)
            int OPT_ANGMeteoDataTypeId = db.MeteoDataTypes
                .Where(m => m.Code == "ret_tlt0" && m.MeteoDataSourceId == NASASSEId && m.MeteoDataPeriodicityId == PeriodicityMAId && m.AdditionalEN == "OPT ANG")
                .FirstOrDefault().Id;
            IList<MeteoData> OPT_ANG = GetMonthlyAverage(longitude, latitude, OPT_ANGMeteoDataTypeId);
            for (int i = 1; i <= 12; i++)
            {
                OPT_ANG.FirstOrDefault(o => o.Month == i).Value = OPT_ANG.FirstOrDefault(o => o.Month == i).Value == null ? 0 : OPT_ANG.FirstOrDefault(o => o.Month == i).Value;
            }

            // Monthly Averaged Air Temperature At 2 m Above The Surface Of The Earth For Indicated GMT Times (°C) (NASA POWER)
            //int TaMeteoDataTypeId = db.MeteoDataTypes
            //    .Where(m => m.Code == "T2M" && m.MeteoDataSourceId == NASAPOWERId && m.MeteoDataPeriodicityId == PeriodicityMAId)
            //    .FirstOrDefault().Id;
            //int TaMeteoDataTypeId = db.MeteoDataTypes
            //    .Where(m => m.Code == "T10M" && m.MeteoDataSourceId == NASASSEId && m.MeteoDataPeriodicityId == PeriodicityMAId)
            //    .FirstOrDefault().Id;
            //IList<MeteoData> TaM = GetMonthlyAverage(longitude, latitude, TaMeteoDataTypeId);

            int Ta03MeteoDataTypeId = db.MeteoDataTypes
                .Where(m => m.Code == "T2M3" && m.MeteoDataSourceId == NASAPOWERId && m.MeteoDataPeriodicityId == PeriodicityMAId && m.AdditionalEN == "Average@03")
                .FirstOrDefault().Id;
            IList<MeteoData> Ta03 = GetMonthlyAverage(longitude, latitude, Ta03MeteoDataTypeId);
            int Ta06MeteoDataTypeId = db.MeteoDataTypes
                .Where(m => m.Code == "T2M3" && m.MeteoDataSourceId == NASAPOWERId && m.MeteoDataPeriodicityId == PeriodicityMAId && m.AdditionalEN == "Average@06")
                .FirstOrDefault().Id;
            IList<MeteoData> Ta06 = GetMonthlyAverage(longitude, latitude, Ta06MeteoDataTypeId);
            int Ta09MeteoDataTypeId = db.MeteoDataTypes
                .Where(m => m.Code == "T2M3" && m.MeteoDataSourceId == NASAPOWERId && m.MeteoDataPeriodicityId == PeriodicityMAId && m.AdditionalEN == "Average@09")
                .FirstOrDefault().Id;
            IList<MeteoData> Ta09 = GetMonthlyAverage(longitude, latitude, Ta09MeteoDataTypeId);
            int Ta12MeteoDataTypeId = db.MeteoDataTypes
                .Where(m => m.Code == "T2M3" && m.MeteoDataSourceId == NASAPOWERId && m.MeteoDataPeriodicityId == PeriodicityMAId && m.AdditionalEN == "Average@12")
                .FirstOrDefault().Id;
            IList<MeteoData> Ta12 = GetMonthlyAverage(longitude, latitude, Ta12MeteoDataTypeId);
            IList<MeteoData> Ta = new List<MeteoData>();

            //string[] Ta_s = new string[12];
            for (int i = 1; i <= 12; i++)
            {
                // (1.3)
                Ta.Add(new MeteoData
                {
                    Month = i,
                    Value = (Ta03.FirstOrDefault(t => t.Month == i).Value + Ta06.FirstOrDefault(t => t.Month == i).Value + Ta09.FirstOrDefault(t => t.Month == i).Value + Ta12.FirstOrDefault(t => t.Month == i).Value) / 4m
                });
                //Ta_s[i] = ((decimal)Ta[i - 1].Value).ToString("0.00");
            }

            // расчет дневной среднемесячной солнечной радиации
            // >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            latitude = latitude / 180 * (decimal)Math.PI;
            tilt = tilt / 180 * (decimal)Math.PI;
            azimuth = azimuth / 180 * (decimal)Math.PI;
            //H[0].Value = 1.14M;
            //Hd[0].Value = 0.69M;
            //Ps[0].Value = 0.35M;
            //H[6].Value = 5.93M;
            //Hd[6].Value = 2.58M;
            //Ps[6].Value = 0.16M;


            // порядковый номер дня в году, отсчитываемый от 1 января
            int[] N = new int[12];
            N[0] = 17;
            N[1] = 47;
            N[2] = 75;
            N[3] = 105;
            N[4] = 135;
            N[5] = 162;
            N[6] = 198;
            N[7] = 228;
            N[8] = 258;
            N[9] = 288;
            N[10] = 318;
            N[11] = 344;

            // значения склонения Солнца (рад)
            decimal[] δ = new decimal[12];
            for (int i = 0; i < 12; i++)
            {
                // (2.8)
                δ[i] = 23.45M * (decimal)Math.PI / 180 * (decimal)Math.Sin((double)((360M * (284M + N[i]) / 365M) / 180M * (decimal)Math.PI));
            }

            // sunset hour angle
            decimal[] ω_s = new decimal[12];
            for (int i = 0; i < 12; i++)
            {
                // (2.7)
                //ω_s[i] = acos(sin(latitude) * sin(δ[i]) / cos(latitude) / cos(δ[i]));
                ω_s[i] = (decimal)Math.Acos(-Math.Sin((double)latitude) * Math.Sin((double)δ[i]) / Math.Cos((double)latitude) / Math.Cos((double)δ[i]));
            }

            decimal[] A = new decimal[12];
            decimal[] B = new decimal[12];
            for (int i = 0; i < 12; i++)
            {
                // (2.5)
                A[i] = 0.409m + 0.5016m * (decimal)Math.Sin((double)(ω_s[i] - (decimal)Math.PI / 3));
                // (2.6)
                B[i] = 0.6609m - 0.4767m * (decimal)Math.Sin((double)(ω_s[i] - (decimal)Math.PI / 3));
            }

            // solar hour angle for each daylight hour relative to solar noon.
            // Часовой угол ω (рад) – угол, который определяет угловое смещение Солнца в течение суток.
            // Один час соответствует π/12 рад или 15 град углового смещения.
            // В полдень часовой угол равен нулю.
            // Значения часового угла до полудня считаются отрицательными, после полудня – положительными.
            decimal[] ω = new decimal[25];
            int w_begin = 2,
                w_end = 22;
            for (int i = w_begin; i <= w_end; i++)
            {
                ω[i] = (decimal)(i * 15 - 12 * 15) / 180 * (decimal)Math.PI;
            }

            decimal[,] rt = new decimal[12, 25];
            decimal[,] rd = new decimal[12, 25];
            decimal[,] Hh = new decimal[12, 25];
            decimal[,] Hdh = new decimal[12, 25];
            for (int i = 0; i < 12; i++)
            {
                for (int j = w_begin; j <= w_end; j++)
                {
                    // (2.3)
                    rt[i, j] = (decimal)Math.PI / 24 * (A[i] + B[i] * (decimal)Math.Cos((double)ω[j])) * ((decimal)Math.Cos((double)ω[j]) - (decimal)Math.Cos((double)ω_s[i])) / ((decimal)Math.Sin((double)ω_s[i]) - ω_s[i] * (decimal)Math.Cos((double)ω_s[i]));
                    // (2.4)
                    rd[i, j] = (decimal)Math.PI / 24 * ((decimal)Math.Cos((double)ω[j]) - (decimal)Math.Cos((double)ω_s[i])) / ((decimal)Math.Sin((double)ω_s[i]) - ω_s[i] * (decimal)Math.Cos((double)ω_s[i]));
                    // (2.1)
                    Hh[i, j] = 1000 * rt[i, j] * (decimal)H.FirstOrDefault(m => m.Month == i + 1).Value;
                    // (2.2)
                    Hdh[i, j] = 1000 * rd[i, j] * (decimal)Hd.FirstOrDefault(m => m.Month == i + 1).Value;
                }
            }

            decimal[] σ_w = new decimal[25];
            for (int i = w_begin; i <= w_end; i++)
            {
                // (2.17)
                σ_w[i] = ω[i] >= 0 ? 1 : -1;
            }

            decimal[] σ_ns = new decimal[12];
            for (int i = 0; i < 12; i++)
            {
                // (2.16)
                σ_ns[i] = latitude * (latitude - δ[i]) >= 0 ? 1 : -1;
            }

            decimal[] ω_ew = new decimal[12];
            for (int i = 0; i < 12; i++)
            {
                // (2.14)
                ω_ew[i] = (decimal)Math.Acos((double)((decimal)Math.Cos((double)latitude) * (decimal)Math.Tan((double)δ[i])));
            }

            decimal[,] σ_ew = new decimal[12, 25];
            for (int i = 0; i < 12; i++)
            {
                for (int j = w_begin; j <= w_end; j++)
                {
                    // (2.15)
                    σ_ew[i, j] = Math.Abs(ω[j]) <= ω_ew[i] ? 1 : -1;
                }
            }

            // hourly cosine of zenith angle (the angle between the vertical and a ray from the sun)
            decimal[,] cosθ_zh = new decimal[12, 25];
            //decimal[,] sinθ_zh = new decimal[12, 25];
            //decimal[,] tanθ_zh = new decimal[12, 25];
            decimal[,] θ_zh = new decimal[12, 25];
            for (int i = 0; i < 12; i++)
            {
                for (int j = w_begin; j <= w_end; j++)
                {
                    // (2.11)
                    cosθ_zh[i, j] = (decimal)Math.Sin((double)latitude) * (decimal)Math.Sin((double)δ[i]) + (decimal)Math.Cos((double)latitude) * (decimal)Math.Cos((double)δ[i]) * (decimal)Math.Cos((double)ω[j]);
                    //sinθ_zh[i, j] = (decimal)Math.Pow(1 - Math.Pow((double)cosθ_zh[i, j], 2), 0.5);
                    //tanθ_zh[i, j] = sinθ_zh[i, j] / cosθ_zh[i, j];
                    θ_zh[i, j] = (decimal)Math.Acos((double)cosθ_zh[i, j]);
                }
            }

            decimal[,] γ_sh0 = new decimal[12, 25];
            for (int i = 0; i < 12; i++)
            {
                for (int j = w_begin; j <= w_end; j++)
                {
                    // (2.13)
                    try
                    {
                        γ_sh0[i, j] = (decimal)Math.Asin((double)((decimal)Math.Sin((double)ω[j]) * (decimal)Math.Cos((double)δ[i]) / (decimal)Math.Sin((double)θ_zh[i, j])));
                    }
                    catch
                    {
                        γ_sh0[i, j] = 0;
                    }

                }
            }

            // hourly solar azimuth angle; angle between the line of sight of the Sun into the horizontal surface and the local meridian
            decimal[,] γ_sh = new decimal[12, 25];
            for (int i = 0; i < 12; i++)
            {
                for (int j = w_begin; j <= w_end; j++)
                {
                    // (2.12)
                    γ_sh[i, j] = σ_ew[i, j] * σ_ns[i] * γ_sh0[i, j] + (1 - σ_ew[i, j] * σ_ns[i]) / 2 * σ_w[j] * (decimal)Math.PI;
                }
            }

            // hourly surface azimuth of the tilted surface; angle between the projection of the normal to the surface into the horizontal surface and the local meridian.
            // Azimuth is zero facing the equator, positive west, and negative east
            decimal[,] γ_h = new decimal[12, 25];
            // ?????????????????????????????????????????????????????????????????????



            // Следящая по горизонтальной оси, корректируемая по азимуту солнца 2 раза в год
            decimal[,] β_h0 = new decimal[12, 25];
            decimal[,] σ_β = new decimal[12, 25];
            // hourly slope of the PV array relative to horizontal surface
            decimal[,] β_h = new decimal[12, 25];
            if (panel_orientation.Code.ToLower() == Properties.Settings.Default.PanelOriantationHorizontalCode.ToLower())
            {
                for (int i = 0; i < 12; i++)
                {
                    for (int j = w_begin; j <= w_end; j++)
                    {
                        γ_h[i, j] = azimuth;
                        // (2.20)
                        β_h0[i, j] = (decimal)Math.Atan((double)((decimal)Math.Tan((double)θ_zh[i, j]) * (decimal)Math.Cos((double)(γ_h[i, j] - γ_sh[i, j]))));
                        // (2.21)
                        σ_β[i, j] = β_h0[i, j] >= 0 ? 0 : 1;
                        // (2.19)
                        β_h[i, j] = β_h0[i, j] + σ_β[i, j] * (decimal)Math.PI;
                    }
                }
            }

            // Следящая по двум осям
            if (panel_orientation.Code.ToLower() == Properties.Settings.Default.PanelOriantation2AxisCode.ToLower())
            {
                for (int i = 0; i < 12; i++)
                {
                    for (int j = w_begin; j <= w_end; j++)
                    {
                        // (2.22)
                        //γ_sh[i, j] = azimuth;????????????????????
                        γ_h[i, j] = γ_sh[i, j];
                        β_h[i, j] = θ_zh[i, j];
                    }
                }
            }

            // Фиксированная, азимут и угол наклона корректируются 2 раза в год
            // Значение углов наклона панели
            //decimal[] β = new decimal[12];
            string[] β_h_s = new string[12];
            if (panel_orientation.Code.ToLower() == Properties.Settings.Default.PanelOriantationFixedCorrectableCode.ToLower())
            {
                for (int i = 0; i < 12; i++)
                {
                    if (i >= 3 && i <= 8) //
                    {
                        for (int j = w_begin; j <= w_end; j++)
                        {
                            β_h[i, j] = (decimal)OPT_ANG.FirstOrDefault(o => o.Month == 7).Value / 180 * (decimal)Math.PI;
                        }
                    }
                    else
                    {
                        for (int j = w_begin; j <= w_end; j++)
                        {
                            β_h[i, j] = (decimal)OPT_ANG.FirstOrDefault(o => o.Month == 1).Value / 180 * (decimal)Math.PI;
                        }
                    }
                    β_h_s[i] = (β_h[i, 12] * 180 / (decimal)Math.PI).ToString("0.00");
                }
            }

            // Следящая по вертикальной оси, корректируемая по углу наклона 2 раза в год
            if (panel_orientation.Code.ToLower() == Properties.Settings.Default.PanelOriantationVerticalCode.ToLower())
            {
                for (int i = 0; i < 12; i++)
                {
                    for (int j = w_begin; j <= w_end; j++)
                    {
                        γ_h[i, j] = γ_sh[i, j];
                    }
                    //if (i >= 2 && i < 8)
                    //{
                    //    for (int j = w_begin; j <= w_end; j++)
                    //    {
                    //        β_h[i, j] = (decimal)OPT_ANG.FirstOrDefault(o => o.Month == 3).Value;
                    //    }
                    //}
                    //else
                    //{
                    //    for (int j = w_begin; j <= w_end; j++)
                    //    {
                    //        β_h[i, j] = (decimal)OPT_ANG.FirstOrDefault(o => o.Month == 9).Value;
                    //    }
                    //}
                }
            }

            // Фиксированная, азимут и угол наклона корректируются 2 раза в год
            if (panel_orientation.Code.ToLower() == Properties.Settings.Default.PanelOriantationFixedCode.ToLower())
            {
                for (int i = 0; i < 12; i++)
                {
                    for (int j = w_begin; j <= w_end; j++)
                    {
                        //β_h[i, j] = tilt;
                        γ_h[i, j] = azimuth;
                    }
                }
            }

            // Фиксированная
            if (panel_orientation.Code.ToLower() == Properties.Settings.Default.PanelOriantationFixedCode.ToLower())
            {
                for (int i = 0; i < 12; i++)
                {
                    for (int j = w_begin; j <= w_end; j++)
                    {
                        β_h[i, j] = tilt;
                    }
                }
            }

            // Следящая по вертикальной оси, корректируемая по углу наклона 2 раза в год
            if (panel_orientation.Code.ToLower() == Properties.Settings.Default.PanelOriantationVerticalCode.ToLower())
            {
                for (int i = 0; i < 12; i++)
                {
                    for (int j = w_begin; j <= w_end; j++)
                    {
                        // (2.18)
                        γ_h[i, j] = γ_sh[i, j];
                        β_h[i, j] = tilt;
                    }
                }
            }

            // hourly cosine of incidence angle (angle between a ray from the sun and the surface normal)
            decimal[,] cosθ_h = new decimal[12, 25];
            for (int i = 0; i < 12; i++)
            {
                for (int j = w_begin; j <= w_end; j++)
                {
                    cosθ_h[i, j] = (decimal)Math.Sin((double)θ_zh[i, j]) * (decimal)Math.Sin((double)β_h[i, j]) * (decimal)Math.Cos⁡((double)(γ_sh[i, j] - γ_h[i, j])) + cosθ_zh[i, j] * (decimal)Math.Cos((double)β_h[i, j]);
                }
            }

            decimal[,] H_th = new decimal[12, 25];
            for (int i = 0; i < 12; i++)
            {
                for (int j = w_begin; j <= w_end; j++)
                {
                    // (2.10)
                    H_th[i, j] = (Hh[i, j] - Hdh[i, j]) * cosθ_h[i, j] / cosθ_zh[i, j] + Hdh[i, j] * (1 + (decimal)Math.Cos((double)β_h[i, j])) / 2 + Hh[i, j] * (decimal)Ps.FirstOrDefault(p => p.Month == i + 1).Value * (1 - (decimal)Math.Cos((double)β_h[i, j])) / 2;
                }
            }

            decimal[] H_td = new decimal[12];
            string[] H_td_s = new string[12];
            for (int i = 0; i < 12; i++)
            {
                H_td[i] = 0;
                for (int j = w_begin; j <= w_end; j++)
                {
                    H_th[i, j] = cosθ_zh[i, j] <= 0 ? 0 : H_th[i, j];
                    H_th[i, j] = H_th[i, j] <= 0 ? 0 : H_th[i, j];
                    H_td[i] += H_th[i, j];
                }
                H_td_s[i] = (H_td[i] / 1000m).ToString("0.00");
            }
            // <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
            // Прочие потери в PV панели
            // (1.5)
            //decimal Lopv = 1 - (1 - losses_snow) * (1 - losses_soil) * (1 - losses_shad) * (1 - losses_mis);
            decimal[] Lopv = new decimal[12];
            for (int i = 0; i < 12; i++)
            {
                bool losses_soil_month_b = false;
                for (int j = 0; j < LossesSnowMoths.Length; j++)
                {
                    if (LossesSnowMoths[j] == i)
                    {
                        losses_soil_month_b = true;
                        break;
                    }
                }
                decimal losses_soil_month = losses_soil_month_b ? losses_soil : 0m;
            }

            // the coefficient of efficiency of other power losses in PV module
            // (1.6)
            //decimal η_opv = 1 - Lopv;
            decimal[] η_opv = new decimal[12];
            for (int i = 0; i < 12; i++)
            {
                η_opv[i] = 1 - Lopv[i];
            }

            //// Средняя дневная температура наружного воздуха град С
            //// (1.3)
            //decimal Ta = (decimal)(TaM.FirstOrDefault(t => t.Month == 3).Value +
            //    TaM.FirstOrDefault(t => t.Month == 3).Value +
            //    TaM.FirstOrDefault(t => t.Month == 3).Value +
            //    TaM.FirstOrDefault(t => t.Month == 3).Value) / 4;
            string[] Ta_s = new string[12];
            for (int i = 0; i < 12; i++)
            {
                Ta_s[i] = ((decimal)Ta.FirstOrDefault(t => t.Month == i + 1).Value).ToString("0.00");
            }

            // Monthly averaged hourly solar radiation on a tilted surface [kWh/m2/h]
            decimal[] H_tah = new decimal[12];
            string[] H_tah_s = new string[12];
            string[] H_tm_s = new string[12];
            string[] D_s = new string[12];
            for (int i = 0; i < 12; i++)
            {
                decimal bufer = 0;
                int bufer_count = 0;
                decimal D = 0;
                int D_count = 0;
                for (int j = w_begin; j <= w_end; j++)
                {
                    if (H_th[i, j] > 0)
                    {
                        D += H_th[i, j];
                        D_count++;
                    }
                    //H_tah[i, j] = H_td[i] / j;
                    bufer += H_th[i, j];
                    bufer_count++;
                }
                D_s[i] = D_count.ToString();
                H_tah[i] = D / D_count;
                H_tm_s[i] = (D / 1000M * days[i]).ToString("0.00");
                H_tah_s[i] = (D / D_count / 1000m).ToString("0.00");
            }

            // Температура ячейки в рабочем режиме для каждого месяца
            decimal[] T_c = new decimal[12];
            string[] T_c_s = new string[12];
            for (int i = 0; i < 12; i++)
            {
                //for (int j = w_begin; j <= w_end; j++)
                {
                    // (1.2)
                    T_c[i] = (decimal)Ta.FirstOrDefault(t => t.Month == i + 1).Value + H_tah[i] / 1000m / 0.8m * (NOCT - 20);
                    T_c_s[i] = T_c[i].ToString("0.00");
                }
            }

            // коэффициент эффективности солнечного панеля при различных температурах ячейки
            decimal[] η_tpv = new decimal[12];
            string[] η_tpv_s = new string[12];
            for (int i = 0; i < 12; i++)
            {
                decimal bufer = 0;
                int bufer_count = 0;
                //for (int j = w_begin; j <= w_end; j++)
                {
                    // (1.1)
                    η_tpv[i] = panel_efficiency * (1 + thermal_power_factor * (T_c[i] - 25));
                    bufer += η_tpv[i];
                    bufer_count++;
                }
                η_tpv_s[i] = (bufer / bufer_count).ToString("0.00");
            }

            // Эффективеность электрических устройств
            // (1.7)
            decimal η_epv = 1M;
            if (SPPPurposeId == 1) //сетевая
            {
                η_epv = wiring_efficiency * inverter_efficiency;
            }
            else
            {
                η_epv = wiring_efficiency * controller_efficiency * inverter_efficiency * battery_efficiency;
            }
            

            // Коэффициент эффективности PV системы
            decimal[] η_spv = new decimal[12];
            string[] η_spv_s = new string[12];
            for (int i = 0; i < 12; i++)
            {
                decimal bufer = 0;
                int bufer_count = 0;
                //for (int j = w_begin; j <= w_end; j++)
                {
                    // (1.8)
                    η_spv[i] = η_tpv[i] * η_epv * η_opv[i];
                    bufer += η_spv[i];
                    bufer_count++;
                }
                η_spv_s[i] = (bufer / bufer_count).ToString("0.00");
            }

            // Area of Solar PV panel [m2]
            // (1.9-2)
            //decimal S_pv = panels_count * rated_power / 1000 / panel_efficiency;

            // Monthly averaged energy output from the Solar PV panel [kWh/day]
            decimal[] W_PV = new decimal[12];
            for (int i = 0; i < 12; i++)
            {
                //for (int j = w_begin; j <= w_end; j++)
                {
                    // (1.9-1)
                    W_PV[i] = panels_area * η_spv[i] * H_td[i];
                }
            }
            string[] W_PV_s = new string[12];
            for (int i = 0; i < 12; i++)
            {
                decimal bufer = 0;
                //for (int j = w_begin; j <= w_end; j++)
                {
                    bufer += W_PV[i] / 1000m;
                }
                W_PV_s[i] = bufer.ToString("0.00");
            }

            // данные для графика
            List<PlotData> W_PV_c = new List<PlotData>();
            for (int i = 0; i < W_PV.Count(); i++)
            {
                W_PV_c.Add(new PlotData() { X = i + 1, Y = W_PV[i] / 1000m });
            }

            // year
            decimal W_PV_year = 0,
                H_td_year = 0;
            for (int i = 0; i < W_PV.Count(); i++)
            {
                W_PV_year += W_PV[i] * days[i];
                H_td_year += H_td[i] * days[i];
            }
            W_PV_year /= 365.25M * 1000M;
            H_td_year /= 365.25M * 1000M;

            return Json(new
            {
                //Longitude = Longitude.ToString(),
                //Latitude = Latitude.ToString(),
                //SPPPurpose = db.SPPPurposes.FirstOrDefault(s => s.Id == SPPPurposeId).Name,
                //PanelOrientation = panel_orientation.Name,
                //PVSystemMaterial = pvsystemmaterial.Name,
                //RatedPower = RatedPower.ToString(),
                //PanelsCount = PanelsCount.ToString(),
                //PanelEfficiency = PanelEfficiency.ToString(),
                //RatedOperatingTemperature = RatedOperatingTemperature.ToString(),
                //ThermalPowerFactor = ThermalPowerFactor.ToString(),
                //Tilt = Tilt.ToString(),
                //Azimuth = Azimuth.ToString(),
                //WiringEfficiency = WiringEfficiency.ToString(),
                //ControllerEfficiency = ControllerEfficiency.ToString(),
                //InverterEfficiency = InverterEfficiency.ToString(),
                //BatteryEfficiency = BatteryEfficiency.ToString(),
                //LossesSoil = LossesSoil.ToString(),
                //LossesSnow = LossesSnow.ToString(),
                //LossesShad = LossesShad.ToString(),
                //LossesMis = LossesMis.ToString(),
                //H_td_average = H_td.Average().ToString("0.00"),
                //η_epv = η_epv.ToString(),
                //S_pv = S_pv.ToString(),
                W_PV = W_PV_s,
                H_td = H_td_s,
                //H_tah = H_tah_s,
                //H_tm = H_tm_s,
                //Ta = Ta_s,
                //D = D_s,
                //η_tpv = η_tpv_s,
                //η_spv = η_spv_s,
                W_PV_c = W_PV_c,
                //T_c = T_c_s,
                //β_h = β_h_s
                W_PV_year = W_PV_year.ToString("0.00"),
                H_td_year = H_td_year.ToString("0.00")
            });
        }

        [HttpPost]
        public ActionResult GetCSV(string[][] Array)
        {
            try
            {
                string path = "~/Download/csv/";
                //удаление предыдущих файлов и папок
                DirectoryInfo di = new DirectoryInfo(Server.MapPath(path));
                foreach (FileInfo file in di.GetFiles())
                {
                    try
                    {
                        file.Delete();
                    }
                    catch
                    {
                    }
                }
                foreach (DirectoryInfo dir in di.GetDirectories())
                {
                    if (!dir.Name.Contains("AnalizeTerrain"))
                    {
                        try
                        {
                            dir.Delete(true);
                        }
                        catch
                        {
                        }
                    }
                }
                string filenameout = $"{DateTime.Now.ToString("yyyy.MM.dd.hh.mm.ss.FFFFFFF")}.csv";
                filenameout = Path.Combine(Server.MapPath(path), filenameout);
                // сохранение в файл
                using (var sw = new StreamWriter(
                        new FileStream(filenameout, FileMode.CreateNew, FileAccess.Write),
                        Encoding.UTF8))
                {

                    for (int i = 0; i < Array.Length; i++)
                    {
                        string line = "";
                        for (int j = 0; j < Array[i].Length; j++)
                        {
                            line += Array[i][j] + ";";
                        }
                        line.Remove(line.Length - 1, 1);
                        sw.WriteLine(line);
                    }
                }

                return Json(new
                {
                    FilePath = "/Download/csv/" + Path.GetFileName(filenameout)
                });
            }
            catch (Exception e)
            {
                return Json(new
                {
                    Error = e.Message
                });
            }
        }

        [HttpPost]
        public ActionResult GetChart(string source)
        {
            try
            {
                string path = "~/Download/Images/";
                //удаление предыдущих файлов и папок
                DirectoryInfo di = new DirectoryInfo(Server.MapPath(path));
                foreach (FileInfo file in di.GetFiles())
                {
                    try
                    {
                        file.Delete();
                    }
                    catch
                    {
                    }
                }
                foreach (DirectoryInfo dir in di.GetDirectories())
                {
                    if(!dir.Name.Contains("AnalizeTerrain"))
                    {
                        try
                        {
                            dir.Delete(true);
                        }
                        catch
                        {
                        }
                    }
                }
                string filenameout = $"{DateTime.Now.ToString("yyyy.MM.dd.hh.mm.ss.FFFFFFF")}.png";
                filenameout = Path.Combine(Server.MapPath(path), filenameout);

                string base64 = source.Substring(source.IndexOf(',') + 1);
                base64 = base64.Trim('\0');
                byte[] data = Convert.FromBase64String(base64);
                Image image;
                using (MemoryStream ms = new MemoryStream(data))
                {
                    image = Image.FromStream(ms);
                }
                image.Save(filenameout);
                return Json(new
                {
                    FilePath = "/Download/Images/" + Path.GetFileName(filenameout)
                });

                //var imgPath = "/Download/Images/" + Path.GetFileName(filenameout);
                //return base.File(imgPath, "application/unknown");

                //var imgPath = "/Download/Images/" + Path.GetFileName(filenameout);
                //Response.AddHeader("Content-Disposition", "attachment;filename=DealerAdTemplate.png");
                //Response.WriteFile(imgPath);
                //Response.End();
                //return null;

                //byte[] fileBytes = System.IO.File.ReadAllBytes("/Download/Images/" + Path.GetFileName(filenameout));
                //string fileName = "myfile.png";
                //return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);

                //var image = db.Images.Where(x => x.ImageID == id).Select(x => x).Single();
                //return File(image.ImageData, image.ImageMimeType);
                //return File(new MemoryStream(data), "image/png");
            }
            catch (Exception e)
            {
                return Json(new
                {
                    Error = e.Message
                });
            }
        }

        [HttpPost]
        public ActionResult AnalizeTerrainRayon(
            int RayonId,
            int Importances_sum_swv_dwn,    // analize_terrain_sum_swv_dwn
            int? Min_sum_swv_dwn,            // analize_terrain_sum_swv_dwn_min
            int Importances_lep,            // analize_terrain_lep_dwn
            int? Max_lep,                    // analize_terrain_lep_min
            int Importances_road,            // analize_terrain_road_dwn
            int? Max_road,                    // analize_terrain_road_min
            int Importances_np,            // analize_terrain_np_dwn
            int? Max_np,                    // analize_terrain_np_min
            int Importances_slope_blh,            // analize_terrain_slope_blh_dwn
            int? Max_slope_blh,                    // analize_terrain_slope_blh_min
            int Importances_srtm_blh,            // analize_terrain_srtm_blh_dwn
            int? Max_srtm_blh,                    // analize_terrain_srtm_blh_min
                                                 //string[] Excludes 
            bool Exclude_oopt,  // analize_terrain_oopt_exclude
            bool Exclude_wood,  // analize_terrain_wood_exclude
            bool Exclude_hydro,  // analize_terrain_hydro_exclude
            bool Exclude_pamatniki,  // analize_terrain_pamatniki_exclude
            bool Exclude_np  // analize_terrain_np_exclude
            )
        {
            int min_sum_swv_dwn = Min_sum_swv_dwn == null ? 1000 : (int)Min_sum_swv_dwn,
                max_sum_swv_dwn = 2000,
                max_lep = (Max_lep == null || Max_lep > 300000) ? 300000 : (int)Max_lep,
                max_road = (Max_road == null || Max_road > 200000) ? 200000 : (int)Max_road,
                max_np = (Max_np == null || Max_np > 300000) ? 300000 : (int)Max_np,
                max_slope_blh = (Max_slope_blh == null || Max_slope_blh > 90) ? 90 : (int)Max_slope_blh,
                max_srtm_blh = (Max_srtm_blh == null || Max_srtm_blh > 5000) ? 5000 : (int)Max_srtm_blh;

            string GeoServerdatadir_pre = "~/Download/AnalizeTerrain";
            string GeoServerdatadir = @"F:\GeoServer 2.9.0\data_dir\coverages\AtlasSolar\AnalizeTerrain";
            string sum_swv_dwn_file_name = $"{RayonId.ToString()}_sum_swv_dwn.tif";
            string srtm_blh_file_name = $"{RayonId.ToString()}_srtm_blh.tif";
            string aspect_blh_file_name = $"{RayonId.ToString()}_aspect_blh.tif";
            string slope_blh_file_name = $"{RayonId.ToString()}_slope_blh.tif";
            string rstr_iskl_file_name = $"{RayonId.ToString()}_rstr_iskl.tif";
            string np_dist_file_name = $"{RayonId.ToString()}_np_dist.tif";
            string lep_dist_file_name = $"{RayonId.ToString()}_lep_dist.tif";
            string ador_dist_file_name = $"{RayonId.ToString()}_ador_dist.tif";
            // delete .AddDays(-1)
            string out_file_name_pure = $"{DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss_FFFFFFF")}";
            string out_file_name = $"{out_file_name_pure}.tif";
            string out_file_name_GeoServer = out_file_name;
            string maps_path = "~/Maps";
            string error = "";
            try
            {
                // delete old files
                DirectoryInfo di = new DirectoryInfo(GeoServerdatadir);
                foreach (FileInfo file in di.GetFiles("*.tif"))
                {
                    DateTime fdt = new DateTime(
                        Convert.ToInt32(file.Name.Split('_')[0]),
                        Convert.ToInt32(file.Name.Split('_')[1]),
                        Convert.ToInt32(file.Name.Split('_')[2]),
                        Convert.ToInt32(file.Name.Split('_')[3]),
                        Convert.ToInt32(file.Name.Split('_')[4]),
                        Convert.ToInt32(file.Name.Split('_')[5]));
                    if ((DateTime.Now - fdt).Days > 0)
                    {
                        // delete in project directory
                        System.IO.File.Delete(Path.Combine(Server.MapPath(GeoServerdatadir_pre), file.Name));
                        // delete storage in GeoServer
                        string batfilenamedelete = Path.ChangeExtension(Path.Combine(Server.MapPath(GeoServerdatadir_pre), file.Name), ".bat");
                        StreamWriter batdelete = new StreamWriter(batfilenamedelete);
                        batdelete.WriteLine($"curl -v -u admin:Ki5sh6fohh -XDELETE \"http://localhost:8080/geoserver/rest/layers/AtlasSolar:{file.Name}\"");
                        batdelete.WriteLine($"curl -v -u admin:Ki5sh6fohh -XDELETE \"http://localhost:8080/geoserver/rest/workspaces/AtlasSolar/coveragestores/{file.Name}?recurse=true\"");
                        batdelete.Close();
                        Process procdelete = new System.Diagnostics.Process();
                        procdelete.StartInfo.FileName = batfilenamedelete;
                        procdelete.StartInfo.Arguments = "/c start /wait " + Path.GetFileName(batfilenamedelete);
                        procdelete.StartInfo.WorkingDirectory = Path.GetDirectoryName(batfilenamedelete);
                        procdelete.Start();
                        procdelete.WaitForExit();
                        System.IO.File.Delete(batfilenamedelete);
                        // delete in GeoServer directory
                        System.IO.File.Delete(Path.ChangeExtension(file.FullName + "_delete", ".bat"));
                        file.Delete();
                    }
                }

                sum_swv_dwn_file_name = Path.Combine(Server.MapPath(maps_path), Path.GetFileName(sum_swv_dwn_file_name));
                srtm_blh_file_name = Path.Combine(Server.MapPath(maps_path), Path.GetFileName(srtm_blh_file_name));
                aspect_blh_file_name = Path.Combine(Server.MapPath(maps_path), Path.GetFileName(aspect_blh_file_name));
                slope_blh_file_name = Path.Combine(Server.MapPath(maps_path), Path.GetFileName(slope_blh_file_name));
                rstr_iskl_file_name = Path.Combine(Server.MapPath(maps_path), Path.GetFileName(rstr_iskl_file_name));
                np_dist_file_name = Path.Combine(Server.MapPath(maps_path), Path.GetFileName(np_dist_file_name));
                lep_dist_file_name = Path.Combine(Server.MapPath(maps_path), Path.GetFileName(lep_dist_file_name));
                ador_dist_file_name = Path.Combine(Server.MapPath(maps_path), Path.GetFileName(ador_dist_file_name));
                //out_file_name = Path.Combine(GeoServerdatadir, Path.GetFileName(out_file_name));
                out_file_name = Path.Combine(Server.MapPath(GeoServerdatadir_pre), Path.GetFileName(out_file_name));
                out_file_name_GeoServer = Path.Combine(GeoServerdatadir, Path.GetFileName(out_file_name_GeoServer));

                // open tifs
                ////Gdal.AllRegister();
                GdalConfiguration.ConfigureGdal();
                Dataset sum_swv_dwn_ds = Gdal.Open(sum_swv_dwn_file_name, Access.GA_ReadOnly);
                Band sum_swv_dwn_band = sum_swv_dwn_ds.GetRasterBand(1);
                int sum_swv_dwn_width = sum_swv_dwn_band.XSize;
                int sum_swv_dwn_height = sum_swv_dwn_band.YSize;
                float[] sum_swv_dwn_r = new float[sum_swv_dwn_width * sum_swv_dwn_height];
                sum_swv_dwn_band.ReadRaster(0, 0, sum_swv_dwn_width, sum_swv_dwn_height, sum_swv_dwn_r, sum_swv_dwn_width, sum_swv_dwn_height, 0, 0);
                //---
                Dataset srtm_blh_ds = Gdal.Open(srtm_blh_file_name, Access.GA_ReadOnly);
                Band srtm_blh_band = srtm_blh_ds.GetRasterBand(1);
                int srtm_blh_width = srtm_blh_band.XSize;
                int srtm_blh_height = srtm_blh_band.YSize;
                float[] srtm_blh_r = new float[srtm_blh_width * srtm_blh_height];
                srtm_blh_band.ReadRaster(0, 0, srtm_blh_width, srtm_blh_height, srtm_blh_r, srtm_blh_width, srtm_blh_height, 0, 0);
                //---
                Dataset aspect_blh_ds = Gdal.Open(sum_swv_dwn_file_name, Access.GA_ReadOnly);
                Band aspect_blh_band = aspect_blh_ds.GetRasterBand(1);
                int aspect_blh_width = aspect_blh_band.XSize;
                int aspect_blh_height = aspect_blh_band.YSize;
                float[] aspect_blh_r = new float[aspect_blh_width * aspect_blh_height];
                aspect_blh_band.ReadRaster(0, 0, aspect_blh_width, aspect_blh_height, aspect_blh_r, aspect_blh_width, aspect_blh_height, 0, 0);
                //---
                Dataset slope_blh_ds = Gdal.Open(slope_blh_file_name, Access.GA_ReadOnly);
                Band slope_blh_band = slope_blh_ds.GetRasterBand(1);
                int slope_blh_width = slope_blh_band.XSize;
                int slope_blh_height = slope_blh_band.YSize;
                float[] slope_blh_r = new float[slope_blh_width * slope_blh_height];
                slope_blh_band.ReadRaster(0, 0, slope_blh_width, slope_blh_height, slope_blh_r, slope_blh_width, slope_blh_height, 0, 0);
                //---
                Dataset rstr_iskl_ds = Gdal.Open(rstr_iskl_file_name, Access.GA_ReadOnly);
                Band rstr_iskl_band = rstr_iskl_ds.GetRasterBand(1);
                int rstr_iskl_width = rstr_iskl_band.XSize;
                int rstr_iskl_height = rstr_iskl_band.YSize;
                float[] rstr_iskl_r = new float[rstr_iskl_width * rstr_iskl_height];
                rstr_iskl_band.ReadRaster(0, 0, rstr_iskl_width, rstr_iskl_height, rstr_iskl_r, rstr_iskl_width, rstr_iskl_height, 0, 0);
                //---
                //Dataset np_dist_ds = Gdal.Open(np_dist_file_name, Access.GA_ReadOnly);
                //Band np_dist_band = np_dist_ds.GetRasterBand(1);
                //int np_dist_width = np_dist_band.XSize;
                //int np_dist_height = np_dist_band.YSize;
                //float[] np_dist_r = new float[np_dist_width * np_dist_height];
                //np_dist_band.ReadRaster(0, 0, np_dist_width, np_dist_height, np_dist_r, np_dist_width, np_dist_height, 0, 0);
                ////---
                //Dataset lep_dist_ds = Gdal.Open(lep_dist_file_name, Access.GA_ReadOnly);
                //Band lep_dist_band = lep_dist_ds.GetRasterBand(1);
                //int lep_dist_width = lep_dist_band.XSize;
                //int lep_dist_height = lep_dist_band.YSize;
                //float[] lep_dist_r = new float[lep_dist_width * lep_dist_height];
                //lep_dist_band.ReadRaster(0, 0, lep_dist_width, lep_dist_height, lep_dist_r, lep_dist_width, lep_dist_height, 0, 0);
                ////---
                //Dataset ador_dist_ds = Gdal.Open(ador_dist_file_name, Access.GA_ReadOnly);
                //Band ador_dist_band = ador_dist_ds.GetRasterBand(1);
                //int ador_dist_width = ador_dist_band.XSize;
                //int ador_dist_height = ador_dist_band.YSize;
                //float[] ador_dist_r = new float[ador_dist_width * ador_dist_height];
                //ador_dist_band.ReadRaster(0, 0, ador_dist_width, ador_dist_height, ador_dist_r, ador_dist_width, ador_dist_height, 0, 0);

                // create out tif
                Driver out_drv = Gdal.GetDriverByName("GTiff");
                int out_BlockXSize,
                    out_BlockYSize;
                sum_swv_dwn_band.GetBlockSize(out out_BlockXSize, out out_BlockYSize);
                double out_val;
                int out_hasval;
                double out_minimum,
                    out_maximum,
                    out_NoDataValue = -1;
                sum_swv_dwn_band.GetMinimum(out out_val, out out_hasval);
                if (out_hasval != 0)
                    out_minimum = out_val;
                sum_swv_dwn_band.GetMaximum(out out_val, out out_hasval);
                if (out_hasval != 0)
                    out_maximum = out_val;
                sum_swv_dwn_band.GetNoDataValue(out out_val, out out_hasval);
                if (out_hasval != 0)
                    out_NoDataValue = out_val;
                string[] out_options = new string[] { "BLOCKXSIZE=" + out_BlockXSize, "BLOCKYSIZE=" + out_BlockYSize, "NODATAVALUE=" + out_NoDataValue };
                float[] out_buffer = new float[sum_swv_dwn_width * sum_swv_dwn_height];
                Random rnd = new Random();
                // delete
                sum_swv_dwn_width = rstr_iskl_band.XSize;
                sum_swv_dwn_height = rstr_iskl_band.YSize;
                for (int i = sum_swv_dwn_width - 1; i >= 0; i--)
                {
                    for (int j = sum_swv_dwn_height - 1; j >= 0; j--)
                    {
                        float value = 0;
                        if(sum_swv_dwn_r[i + j * sum_swv_dwn_width] != out_NoDataValue)
                        {
                            if (Exclude_oopt && rstr_iskl_r[i + j * sum_swv_dwn_width] == 1)
                            {
                                value = 0;
                            }
                            else
                            {
                                float sum_swv_dwn_r_v = sum_swv_dwn_r[i + j * sum_swv_dwn_width] < 0 ? 0 : sum_swv_dwn_r[i + j * sum_swv_dwn_width];
                                float sum_swv_dwn = sum_swv_dwn_r_v < min_sum_swv_dwn ? 0 : (sum_swv_dwn_r_v - min_sum_swv_dwn) / (max_sum_swv_dwn - min_sum_swv_dwn) * 100;
                                sum_swv_dwn *= Importances_sum_swv_dwn / 100;

                                float srtm_blh_r_v = srtm_blh_r[i + j * srtm_blh_width] < -227 ? -227 : srtm_blh_r[i + j * srtm_blh_width];
                                float srtm_blh = srtm_blh_r_v > max_srtm_blh ? 0 : (srtm_blh_r_v - max_srtm_blh) / ((-227) - max_srtm_blh) * 100;
                                srtm_blh *= Importances_srtm_blh / 100;

                                float slope_blh_r_v = slope_blh_r[i + j * slope_blh_width] < 0 ? 0 : slope_blh_r[i + j * slope_blh_width];
                                float slope_blh = slope_blh_r_v > max_slope_blh ? 0 : (slope_blh_r_v - max_slope_blh) / (0 - max_slope_blh) * 100;
                                slope_blh *= Importances_slope_blh / 100;

                                //float sum_swv_dwn_r_v = sum_swv_dwn_r[i + j * sum_swv_dwn_width] < 0 ? 0 : sum_swv_dwn_r[i + j * sum_swv_dwn_width];
                                //float sum_swv_dwn = (sum_swv_dwn_r_v - min_sum_swv_dwn) / (max_sum_swv_dwn - min_sum_swv_dwn) * 100;

                                //float srtm_blh_r_v = srtm_blh_r[i + j * srtm_blh_width] < -227 ? -227 : srtm_blh_r[i + j * srtm_blh_width];
                                //float srtm_blh = (srtm_blh_r_v - (-227)) / (max_srtm_blh - (-227)) * 100;

                                //float slope_blh_r_v = slope_blh_r[i + j * slope_blh_width] < 0 ? 0 : slope_blh_r[i + j * slope_blh_width];
                                //float slope_blh = (slope_blh_r_v - 0) / (max_slope_blh - 0) * 100;

                                value = (sum_swv_dwn + srtm_blh + slope_blh) / 3;
                            }
                        }
                        else
                        {
                            value = (float)out_NoDataValue;
                        }
                        out_buffer[i + j * sum_swv_dwn_width] = value;
                    }
                }
                Dataset out_ds = out_drv.Create(out_file_name, sum_swv_dwn_width, sum_swv_dwn_height, 1, DataType.GDT_Float32, out_options);
                Band out_band = out_ds.GetRasterBand(1);
                out_band.WriteRaster(0, 0, sum_swv_dwn_width, sum_swv_dwn_height, out_buffer, sum_swv_dwn_width, sum_swv_dwn_height, 0, 0);
                out_band.SetNoDataValue(out_NoDataValue);
                out_ds.SetProjection(sum_swv_dwn_ds.GetProjection());
                double[] out_pGT = new double[6];
                sum_swv_dwn_ds.GetGeoTransform(out_pGT);
                out_ds.SetGeoTransform(out_pGT);
                out_ds.SetGCPs(sum_swv_dwn_ds.GetGCPs(), "");
                out_band.FlushCache();
                out_ds.FlushCache();
                out_band.Dispose();
                out_ds.Dispose();

                System.IO.File.Copy(out_file_name, out_file_name_GeoServer, true);

                string batfilename = Path.ChangeExtension(out_file_name, ".bat");
                StreamWriter bat = new StreamWriter(batfilename);
                bat.WriteLine($"curl -u admin:Ki5sh6fohh -v -XPOST -H \"Content-type: text/xml\" \\ -d \" <coverageStore> <name>{out_file_name_pure}</name> <workspace>AtlasSolar</workspace> <enabled>true</enabled> <type>GeoTIFF</type> <url>/coverages/AtlasSolar/AnalizeTerrain/{out_file_name_pure}.tif</url> </coverageStore>\" \\ \"http://localhost:8080/geoserver/rest/workspaces/AtlasSolar/coveragestores?configure=all\"");
                bat.WriteLine($"curl -u admin:Ki5sh6fohh -v -XPOST -H \"Content-type: text/xml\" \\ -d \" <coverage> <name>{out_file_name_pure}</name> <title>{out_file_name_pure}</title> <nativeCRS>EPSG:3857</nativeCRS> <srs>EPSG:3857</srs> <projectionPolicy>FORCE_DECLARED</projectionPolicy> <defaultInterpolationMethod><name>nearest neighbor</name></defaultInterpolationMethod> </coverage> \" \\ \"http://localhost:8080/geoserver/rest/workspaces/AtlasSolar/coveragestores/{out_file_name_pure}/coverages?recalculate=nativebbox\"");
                bat.WriteLine($"curl -v -u admin:Ki5sh6fohh -XPUT -H \"Content-type: text/xml\" \\ -d \"<layer><defaultStyle><name>AnalizeTerrain</name></defaultStyle></layer>\" http://localhost:8080/geoserver/rest/layers/AtlasSolar:{out_file_name_pure}");
                bat.Close();

                Process proc = new System.Diagnostics.Process();
                proc.StartInfo.FileName = batfilename;
                proc.StartInfo.Arguments = "/c start /wait " + Path.GetFileName(batfilename);
                proc.StartInfo.WorkingDirectory = Path.GetDirectoryName(batfilename);
                proc.Start();
                proc.WaitForExit();
            }
            catch (Exception ex)
            {
                error = ex.Message;
                error += ex.InnerException != null ? ". " + ex.InnerException.Message : "";
                error += ex.InnerException != null ? ". " + ex.InnerException.InnerException.Message : "";
            }
            return Json(new
            {
                Layer = out_file_name_pure,
                ErrorMessage = error
            });
        }

        [HttpPost]
        public ActionResult AnalizeTerrain(
            int OblastId,
            int Importances_swvdwnyear,
            int? Min_swvdwnyear,
            int Importances_np_dist,
            int? Max_np_dist,
            int Importances_lep_dist,
            int? Max_lep_dist,
            int Importances_auto_dist,
            int? Max_auto_dist,
            int Importances_slope_srtm,
            int? Max_slope_srtm,
            int Importances_srtm,
            int? Max_srtm,
            bool Exclude_settlement,
            bool Exclude_agri,
            bool Exclude_wood,
            bool Exclude_bush,
            bool Exclude_hydro,
            bool Exclude_takyr,
            bool Exclude_desert,
            bool Exclude_solon,
            bool Exclude_stone,
            bool Exclude_protect,
            bool Exclude_reserve,
            bool Exclude_park,
            bool Exclude_natres,
            bool Exclude_preserve
            )
        {
            string error = "";
            string GeoServerAtlasSolar = Properties.Settings.Default.GeoServerPath + @"\data_dir\coverages\AtlasSolar\";
            string out_file_name_pure = $"{DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss_FFFFFFF")}";
            string out_file_name = $"{out_file_name_pure}.tif";
            out_file_name = Path.Combine(GeoServerAtlasSolar + "AnalizeTerrain", out_file_name);
            try
            {
                int min_swvdwnyear = Min_swvdwnyear == null ? 1100 : (int)Min_swvdwnyear,
                max_swvdwnyear = 1700,
                max_np_dist = (Max_np_dist == null || Max_np_dist > 430) ? 430 : (int)Max_np_dist,
                max_lep_dist = (Max_lep_dist == null || Max_lep_dist > 340) ? 340 : (int)Max_lep_dist,
                max_auto_dist = (Max_auto_dist == null || Max_auto_dist > 420) ? 420 : (int)Max_auto_dist,
                max_slope_srtm = (Max_slope_srtm == null || Max_slope_srtm > 75) ? 75 : (int)Max_slope_srtm,
                max_srtm = (Max_srtm == null || Max_srtm > 6600) ? 6600 : (int)Max_srtm;

                max_lep_dist *= 1000;
                max_auto_dist *= 1000;
                max_np_dist *= 1000;
                
                // delete old files
                DirectoryInfo di = new DirectoryInfo(GeoServerAtlasSolar + "AnalizeTerrain");
                foreach (FileInfo file in di.GetFiles("*.tif"))
                {
                    DateTime fdt = new DateTime(
                        Convert.ToInt32(file.Name.Split('_')[0]),
                        Convert.ToInt32(file.Name.Split('_')[1]),
                        Convert.ToInt32(file.Name.Split('_')[2]),
                        Convert.ToInt32(file.Name.Split('_')[3]),
                        Convert.ToInt32(file.Name.Split('_')[4]),
                        Convert.ToInt32(file.Name.Split('_')[5]));
                    if ((DateTime.Now - fdt).Days > 0)
                    {
                        // delete storage in GeoServer
                        string batfilenamedelete = Path.ChangeExtension(Path.Combine(GeoServerAtlasSolar + "AnalizeTerrain", file.Name), ".bat");
                        StreamWriter batdelete = new StreamWriter(batfilenamedelete);
                        batdelete.WriteLine($"curl -v -u admin:Ki5sh6fohh -XDELETE \"http://localhost:8080/geoserver/rest/layers/AtlasSolar:{file.Name}\"");
                        batdelete.WriteLine($"curl -v -u admin:Ki5sh6fohh -XDELETE \"http://localhost:8080/geoserver/rest/workspaces/AtlasSolar/coveragestores/{file.Name}?recurse=true\"");
                        batdelete.Close();
                        Process procdelete = new System.Diagnostics.Process();
                        procdelete.StartInfo.FileName = batfilenamedelete;
                        procdelete.StartInfo.Arguments = "/c start /wait " + Path.GetFileName(batfilenamedelete);
                        procdelete.StartInfo.WorkingDirectory = Path.GetDirectoryName(batfilenamedelete);
                        procdelete.Start();
                        procdelete.WaitForExit();
                        System.IO.File.Delete(batfilenamedelete);
                        // delete in GeoServer directory
                        System.IO.File.Delete(Path.ChangeExtension(file.FullName + "_delete", ".bat")); //?
                        file.Delete();
                    }
                }

                // create raster
                GdalConfiguration.ConfigureGdal();
                string auto_dist_file_name = Path.Combine(Properties.Settings.Default.GeoServerPath + @"\data_dir\coverages\AtlasSolar\Provinces\" + OblastId.ToString() + "auto_dist.tif");
                Dataset auto_dist_ds = Gdal.Open(auto_dist_file_name, Access.GA_ReadOnly);
                Band auto_dist_band = auto_dist_ds.GetRasterBand(1);
                int auto_dist_width = auto_dist_band.XSize;
                int auto_dist_height = auto_dist_band.YSize;
                float[] auto_dist_array = new float[auto_dist_width * auto_dist_height];
                auto_dist_band.ReadRaster(0, 0, auto_dist_width, auto_dist_height, auto_dist_array, auto_dist_width, auto_dist_height, 0, 0);
                double auto_dist_out_val;
                int auto_dist_out_hasval;
                float auto_dist_NoDataValue = -1;
                auto_dist_band.GetNoDataValue(out auto_dist_out_val, out auto_dist_out_hasval);
                if (auto_dist_out_hasval != 0)
                    auto_dist_NoDataValue = (float)auto_dist_out_val;
                string kzcoveriskl_file_name = Path.Combine(AtlasSolar.Properties.Settings.Default.GeoServerPath + @"\data_dir\coverages\AtlasSolar\Provinces\" + OblastId.ToString() + "kzcoveriskl.tif");
                Dataset kzcoveriskl_ds = Gdal.Open(kzcoveriskl_file_name, Access.GA_ReadOnly);
                Band kzcoveriskl_band = kzcoveriskl_ds.GetRasterBand(1);
                int kzcoveriskl_width = kzcoveriskl_band.XSize;
                int kzcoveriskl_height = kzcoveriskl_band.YSize;
                byte[] kzcoveriskl_array = new byte[kzcoveriskl_width * kzcoveriskl_height];
                kzcoveriskl_band.ReadRaster(0, 0, kzcoveriskl_width, kzcoveriskl_height, kzcoveriskl_array, kzcoveriskl_width, kzcoveriskl_height, 0, 0);
                double kzcoveriskl_out_val;
                int kzcoveriskl_out_hasval;
                byte kzcoveriskl_NoDataValue = 255;
                kzcoveriskl_band.GetNoDataValue(out kzcoveriskl_out_val, out kzcoveriskl_out_hasval);
                if (kzcoveriskl_out_hasval != 0)
                    kzcoveriskl_NoDataValue = (byte)kzcoveriskl_out_val;
                string lep_dist_file_name = Path.Combine(AtlasSolar.Properties.Settings.Default.GeoServerPath + @"\data_dir\coverages\AtlasSolar\Provinces\" + OblastId.ToString() + "lep_dist.tif");
                Dataset lep_dist_ds = Gdal.Open(lep_dist_file_name, Access.GA_ReadOnly);
                Band lep_dist_band = lep_dist_ds.GetRasterBand(1);
                int lep_dist_width = lep_dist_band.XSize;
                int lep_dist_height = lep_dist_band.YSize;
                float[] lep_dist_array = new float[lep_dist_width * lep_dist_height];
                lep_dist_band.ReadRaster(0, 0, lep_dist_width, lep_dist_height, lep_dist_array, lep_dist_width, lep_dist_height, 0, 0);
                double lep_dist_out_val;
                int lep_dist_out_hasval;
                float lep_dist_NoDataValue = -1;
                lep_dist_band.GetNoDataValue(out lep_dist_out_val, out lep_dist_out_hasval);
                if (lep_dist_out_hasval != 0)
                    lep_dist_NoDataValue = (float)lep_dist_out_val;
                string np_dist_file_name = Path.Combine(AtlasSolar.Properties.Settings.Default.GeoServerPath + @"\data_dir\coverages\AtlasSolar\Provinces\" + OblastId.ToString() + "np_dist.tif");
                Dataset np_dist_ds = Gdal.Open(np_dist_file_name, Access.GA_ReadOnly);
                Band np_dist_band = np_dist_ds.GetRasterBand(1);
                int np_dist_width = np_dist_band.XSize;
                int np_dist_height = np_dist_band.YSize;
                float[] np_dist_array = new float[np_dist_width * np_dist_height];
                np_dist_band.ReadRaster(0, 0, np_dist_width, np_dist_height, np_dist_array, np_dist_width, np_dist_height, 0, 0);
                double np_dist_out_val;
                int np_dist_out_hasval;
                float np_dist_NoDataValue = -1;
                np_dist_band.GetNoDataValue(out np_dist_out_val, out np_dist_out_hasval);
                if (np_dist_out_hasval != 0)
                    np_dist_NoDataValue = (float)np_dist_out_val;
                string ooptiskl_file_name = Path.Combine(AtlasSolar.Properties.Settings.Default.GeoServerPath + @"\data_dir\coverages\AtlasSolar\Provinces\" + OblastId.ToString() + "ooptiskl.tif");
                Dataset ooptiskl_ds = Gdal.Open(ooptiskl_file_name, Access.GA_ReadOnly);
                Band ooptiskl_band = ooptiskl_ds.GetRasterBand(1);
                int ooptiskl_width = ooptiskl_band.XSize;
                int ooptiskl_height = ooptiskl_band.YSize;
                byte[] ooptiskl_array = new byte[ooptiskl_width * ooptiskl_height];
                ooptiskl_band.ReadRaster(0, 0, ooptiskl_width, ooptiskl_height, ooptiskl_array, ooptiskl_width, ooptiskl_height, 0, 0);
                double ooptiskl_out_val;
                int ooptiskl_out_hasval;
                byte ooptiskl_NoDataValue = 255;
                ooptiskl_band.GetNoDataValue(out ooptiskl_out_val, out ooptiskl_out_hasval);
                if (ooptiskl_out_hasval != 0)
                    ooptiskl_NoDataValue = (byte)ooptiskl_out_val;
                string slope_srtm_file_name = Path.Combine(AtlasSolar.Properties.Settings.Default.GeoServerPath + @"\data_dir\coverages\AtlasSolar\Provinces\" + OblastId.ToString() + "slope_srtm.tif");
                Dataset slope_srtm_ds = Gdal.Open(slope_srtm_file_name, Access.GA_ReadOnly);
                Band slope_srtm_band = slope_srtm_ds.GetRasterBand(1);
                int slope_srtm_width = slope_srtm_band.XSize;
                int slope_srtm_height = slope_srtm_band.YSize;
                float[] slope_srtm_array = new float[slope_srtm_width * slope_srtm_height];
                slope_srtm_band.ReadRaster(0, 0, slope_srtm_width, slope_srtm_height, slope_srtm_array, slope_srtm_width, slope_srtm_height, 0, 0);
                double slope_srtm_out_val;
                int slope_srtm_out_hasval;
                float slope_srtm_NoDataValue = -1;
                slope_srtm_band.GetNoDataValue(out slope_srtm_out_val, out slope_srtm_out_hasval);
                if (slope_srtm_out_hasval != 0)
                    slope_srtm_NoDataValue = (float)slope_srtm_out_val;
                string srtm_file_name = Path.Combine(AtlasSolar.Properties.Settings.Default.GeoServerPath + @"\data_dir\coverages\AtlasSolar\Provinces\" + OblastId.ToString() + "srtm.tif");
                Dataset srtm_ds = Gdal.Open(srtm_file_name, Access.GA_ReadOnly);
                Band srtm_band = srtm_ds.GetRasterBand(1);
                int srtm_width = srtm_band.XSize;
                int srtm_height = srtm_band.YSize;
                int[] srtm_array = new int[srtm_width * srtm_height];
                srtm_band.ReadRaster(0, 0, srtm_width, srtm_height, srtm_array, srtm_width, srtm_height, 0, 0);
                double srtm_out_val;
                int srtm_out_hasval;
                int srtm_NoDataValue = -1;
                srtm_band.GetNoDataValue(out srtm_out_val, out srtm_out_hasval);
                if (srtm_out_hasval != 0)
                    srtm_NoDataValue = (int)srtm_out_val;
                string swvdwnyear_file_name = Path.Combine(AtlasSolar.Properties.Settings.Default.GeoServerPath + @"\data_dir\coverages\AtlasSolar\Provinces\" + OblastId.ToString() + "swvdwnyear.tif");
                Dataset swvdwnyear_ds = Gdal.Open(swvdwnyear_file_name, Access.GA_ReadOnly);
                Band swvdwnyear_band = swvdwnyear_ds.GetRasterBand(1);
                int swvdwnyear_width = swvdwnyear_band.XSize;
                int swvdwnyear_height = swvdwnyear_band.YSize;
                float[] swvdwnyear_array = new float[swvdwnyear_width * swvdwnyear_height];
                swvdwnyear_band.ReadRaster(0, 0, swvdwnyear_width, swvdwnyear_height, swvdwnyear_array, swvdwnyear_width, swvdwnyear_height, 0, 0);
                double swvdwnyear_out_val;
                int swvdwnyear_out_hasval;
                float swvdwnyear_NoDataValue = -1;
                swvdwnyear_band.GetNoDataValue(out swvdwnyear_out_val, out swvdwnyear_out_hasval);
                if (swvdwnyear_out_hasval != 0)
                    swvdwnyear_NoDataValue = (float)swvdwnyear_out_val;
                // out tif file
                int min_width = Math.Min(
                    auto_dist_width,
                    Math.Min(kzcoveriskl_width,
                    Math.Min(lep_dist_width,
                    Math.Min(np_dist_width,
                    Math.Min(ooptiskl_width,
                    Math.Min(slope_srtm_width,
                    Math.Min(srtm_width,
                    swvdwnyear_width))))))),
                min_height = Math.Min(
                    auto_dist_height,
                    Math.Min(kzcoveriskl_height,
                    Math.Min(lep_dist_height,
                    Math.Min(np_dist_height,
                    Math.Min(ooptiskl_height,
                    Math.Min(slope_srtm_height,
                    Math.Min(srtm_height,
                    swvdwnyear_height)))))));

                Driver out_drv = Gdal.GetDriverByName("GTiff");
                int out_BlockXSize,
                    out_BlockYSize;
                auto_dist_band.GetBlockSize(out out_BlockXSize, out out_BlockYSize);
                byte out_NoDataValue = 255;
                string[] out_options = new string[] { "BLOCKXSIZE = " + out_BlockXSize, "BLOCKYSIZE = " + out_BlockYSize, "NODATAVALUE = " + out_NoDataValue };
                byte[] out_buffer_array = new byte[min_width * min_height];
                for (int i = min_width - 1; i >= 0; i--)
                {
                    for (int j = min_height - 1; j >= 0; j--)
                    {
                        if ((auto_dist_array[i + j * auto_dist_width] == auto_dist_NoDataValue)
                            //|| (kzcoveriskl_array[i + j * kzcoveriskl_width] == kzcoveriskl_NoDataValue)
                            //|| (lep_dist_array[i + j * lep_dist_width] == lep_dist_NoDataValue)
                            //|| (np_dist_array[i + j * np_dist_width] == np_dist_NoDataValue)
                            //|| (ooptiskl_array[i + j * ooptiskl_width] == ooptiskl_NoDataValue)
                            //|| (slope_srtm_array[i + j * slope_srtm_width] == slope_srtm_NoDataValue)
                            //|| (srtm_array[i + j * srtm_width] == srtm_NoDataValue)
                            //|| (swvdwnyear_array[i + j * swvdwnyear_width] == swvdwnyear_NoDataValue)
                            )
                        {
                            out_buffer_array[i + j * min_width] = out_NoDataValue;
                        }
                        else
                        {
                            if((Exclude_settlement && kzcoveriskl_array[i + j * kzcoveriskl_width] == 1)
                                || (Exclude_agri && kzcoveriskl_array[i + j * kzcoveriskl_width] == 2)
                                || (Exclude_wood && kzcoveriskl_array[i + j * kzcoveriskl_width] == 3)
                                || (Exclude_bush && kzcoveriskl_array[i + j * kzcoveriskl_width] == 4)
                                || (Exclude_hydro && kzcoveriskl_array[i + j * kzcoveriskl_width] == 5)
                                || (Exclude_takyr && kzcoveriskl_array[i + j * kzcoveriskl_width] == 6)
                                || (Exclude_desert && kzcoveriskl_array[i + j * kzcoveriskl_width] == 7)
                                || (Exclude_solon && kzcoveriskl_array[i + j * kzcoveriskl_width] == 8)
                                || (Exclude_stone && kzcoveriskl_array[i + j * kzcoveriskl_width] == 9)
                                || (Exclude_protect && ooptiskl_array[i + j * ooptiskl_width] == 11)
                                || (Exclude_reserve && ooptiskl_array[i + j * ooptiskl_width] == 22)
                                || (Exclude_park && ooptiskl_array[i + j * ooptiskl_width] == 33)
                                || (Exclude_natres && ooptiskl_array[i + j * ooptiskl_width] == 44)
                                || (Exclude_preserve && ooptiskl_array[i + j * ooptiskl_width] == 55)
                                )
                            {
                                out_buffer_array[i + j * min_width] = 0;
                            }
                            else
                            {
                                float swvdwnyear_v = swvdwnyear_array[i + j * swvdwnyear_width] < 0 ? 0 : swvdwnyear_array[i + j * swvdwnyear_width];
                                float sum_swv_dwn = swvdwnyear_v < min_swvdwnyear ? 0 : (swvdwnyear_v - min_swvdwnyear) / (max_swvdwnyear - min_swvdwnyear) * 100;
                                sum_swv_dwn *= (float)Importances_swvdwnyear / 100;

                                float np_dist_v = np_dist_array[i + j * np_dist_width] < 0 ? 0 : np_dist_array[i + j * np_dist_width];
                                float np_dist = np_dist_v > max_np_dist ? 0 : (np_dist_v - max_np_dist) / (0 - max_np_dist) * 100;
                                np_dist *= (float)Importances_np_dist / 100;

                                float lep_dist_v = lep_dist_array[i + j * lep_dist_width] < 0 ? 0 : lep_dist_array[i + j * lep_dist_width];
                                float lep_dist = lep_dist_v > max_lep_dist ? 0 : (lep_dist_v - max_lep_dist) / (0 - max_lep_dist) * 100;
                                lep_dist *= (float)Importances_lep_dist / 100;

                                float auto_dist_v = auto_dist_array[i + j * auto_dist_width] < 0 ? 0 : auto_dist_array[i + j * auto_dist_width];
                                float auto_dist = auto_dist_v > max_auto_dist ? 0 : (auto_dist_v - max_auto_dist) / (0 - max_auto_dist) * 100;
                                auto_dist *= (float)Importances_auto_dist / 100;

                                float slope_srtm_v = slope_srtm_array[i + j * slope_srtm_width] < 0 ? 0 : slope_srtm_array[i + j * slope_srtm_width];
                                float slope_srtm = slope_srtm_v > max_slope_srtm ? 0 : (slope_srtm_v - max_slope_srtm) / (0 - max_slope_srtm) * 100;
                                slope_srtm *= (float)Importances_slope_srtm / 100;

                                float srtm_v = srtm_array[i + j * srtm_width] < -230 ? -230 : srtm_array[i + j * srtm_width];
                                float srtm = srtm_v > max_srtm ? 0 : (srtm_v - max_srtm) / (-230 - max_srtm) * 100;
                                srtm *= (float)Importances_srtm / 100;

                                int Importances_count = 0;
                                if(Importances_swvdwnyear>0)
                                {
                                    Importances_count++;
                                }
                                if (Importances_np_dist > 0)
                                {
                                    Importances_count++;
                                }
                                if (Importances_lep_dist > 0)
                                {
                                    Importances_count++;
                                }
                                if (Importances_auto_dist > 0)
                                {
                                    Importances_count++;
                                }
                                if (Importances_slope_srtm > 0)
                                {
                                    Importances_count++;
                                }
                                if (Importances_srtm > 0)
                                {
                                    Importances_count++;
                                }

                                if(Importances_count == 0)
                                {
                                    out_buffer_array[i + j * min_width] = 0;
                                }
                                else
                                {
                                    out_buffer_array[i + j * min_width] = Convert.ToByte((sum_swv_dwn + np_dist + lep_dist + auto_dist + slope_srtm + srtm) / Importances_count);
                                }
                            }
                            
                            //float SLOPE = slope_srtm_array[i + j * slope_srtm_width];
                            //float ALTITUDE = srtm_array[i + j * srtm_width];
                        }
                    }
                }
                out_file_name = Path.Combine(GeoServerAtlasSolar + "AnalizeTerrain", out_file_name);
                Dataset out_ds = out_drv.Create(out_file_name, min_width, min_height, 1, DataType.GDT_Byte, out_options);
                Band out_band = out_ds.GetRasterBand(1);
                out_band.WriteRaster(0, 0, min_width, min_height, out_buffer_array, min_width, min_height, 0, 0);
                out_band.SetNoDataValue(out_NoDataValue);
                out_ds.SetProjection(auto_dist_ds.GetProjection());
                double[] out_pGT = new double[6];
                auto_dist_ds.GetGeoTransform(out_pGT);
                out_ds.SetGeoTransform(out_pGT);
                out_ds.SetGCPs(auto_dist_ds.GetGCPs(), "");
                out_band.FlushCache();
                out_ds.FlushCache();
                out_band.Dispose();
                out_ds.Dispose();
                auto_dist_array = null;
                kzcoveriskl_array = null;
                lep_dist_array = null;
                np_dist_array = null;
                ooptiskl_array = null;
                slope_srtm_array = null;
                srtm_array = null;
                swvdwnyear_array = null;
                GC.Collect();

                // публикация слоя
                string batfilename = Path.ChangeExtension(out_file_name, ".bat");
                StreamWriter bat = new StreamWriter(batfilename);
                bat.WriteLine($"curl -u admin:Ki5sh6fohh -v -XPOST -H \"Content-type: text/xml\" \\ -d \" <coverageStore> <name>{out_file_name_pure}</name> <workspace>AtlasSolar</workspace> <enabled>true</enabled> <type>GeoTIFF</type> <url>/coverages/AtlasSolar/AnalizeTerrain/{out_file_name_pure}.tif</url> </coverageStore>\" \\ \"http://localhost:8080/geoserver/rest/workspaces/AtlasSolar/coveragestores?configure=all\"");
                bat.WriteLine($"curl -u admin:Ki5sh6fohh -v -XPOST -H \"Content-type: text/xml\" \\ -d \" <coverage> <name>{out_file_name_pure}</name> <title>{out_file_name_pure}</title> <nativeCRS>EPSG:3857</nativeCRS> <srs>EPSG:3857</srs> <projectionPolicy>FORCE_DECLARED</projectionPolicy> <defaultInterpolationMethod><name>nearest neighbor</name></defaultInterpolationMethod> </coverage> \" \\ \"http://localhost:8080/geoserver/rest/workspaces/AtlasSolar/coveragestores/{out_file_name_pure}/coverages?recalculate=nativebbox\"");
                bat.WriteLine($"curl -v -u admin:Ki5sh6fohh -XPUT -H \"Content-type: text/xml\" \\ -d \"<layer><defaultStyle><name>AnalizeTerrain</name></defaultStyle></layer>\" http://localhost:8080/geoserver/rest/layers/AtlasSolar:{out_file_name_pure}");
                bat.Close();

                Process proc = new System.Diagnostics.Process();
                proc.StartInfo.FileName = batfilename;
                proc.StartInfo.Arguments = "/c start /wait " + Path.GetFileName(batfilename);
                proc.StartInfo.WorkingDirectory = Path.GetDirectoryName(batfilename);
                proc.Start();
                proc.WaitForExit();
            }
            catch(Exception ex)
            {
                error = ex.Message;
                error += ex.InnerException != null ? ". " + ex.InnerException.Message : "";
                error += ex.InnerException != null ? ". " + ex.InnerException.InnerException.Message : "";
            }
            
            return Json(new
            {
                Layer = out_file_name_pure,
                ErrorMessage = error
            });
        }

        [HttpPost]
        public ActionResult FindTerrain(int OblastId, string Formula)
        {
            string GeoServerAtlasSolar = Properties.Settings.Default.GeoServerPath + @"\data_dir\coverages\AtlasSolar\";
            string out_file_name_pure = $"{DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss_FFFFFFF")}";
            string out_file_name = $"{out_file_name_pure}.tif";
            out_file_name = Path.Combine(GeoServerAtlasSolar + "FindTerrain", out_file_name);

            // delete old files
            DirectoryInfo di = new DirectoryInfo(GeoServerAtlasSolar + "FindTerrain");
            foreach (FileInfo file in di.GetFiles("*.tif"))
            {
                DateTime fdt = new DateTime(
                    Convert.ToInt32(file.Name.Split('_')[0]),
                    Convert.ToInt32(file.Name.Split('_')[1]),
                    Convert.ToInt32(file.Name.Split('_')[2]),
                    Convert.ToInt32(file.Name.Split('_')[3]),
                    Convert.ToInt32(file.Name.Split('_')[4]),
                    Convert.ToInt32(file.Name.Split('_')[5]));
                if ((DateTime.Now - fdt).Days > 0)
                {
                    // delete storage in GeoServer
                    string batfilenamedelete = Path.ChangeExtension(Path.Combine(GeoServerAtlasSolar + "FindTerrain", file.Name), ".bat");
                    StreamWriter batdelete = new StreamWriter(batfilenamedelete);
                    batdelete.WriteLine($"curl -v -u admin:Ki5sh6fohh -XDELETE \"http://localhost:8080/geoserver/rest/layers/AtlasSolar:{file.Name}\"");
                    batdelete.WriteLine($"curl -v -u admin:Ki5sh6fohh -XDELETE \"http://localhost:8080/geoserver/rest/workspaces/AtlasSolar/coveragestores/{file.Name}?recurse=true\"");
                    batdelete.Close();
                    Process procdelete = new System.Diagnostics.Process();
                    procdelete.StartInfo.FileName = batfilenamedelete;
                    procdelete.StartInfo.Arguments = "/c start /wait " + Path.GetFileName(batfilenamedelete);
                    procdelete.StartInfo.WorkingDirectory = Path.GetDirectoryName(batfilenamedelete);
                    procdelete.Start();
                    procdelete.WaitForExit();
                    System.IO.File.Delete(batfilenamedelete);
                    // delete in GeoServer directory
                    System.IO.File.Delete(Path.ChangeExtension(file.FullName + "_delete", ".bat")); //?
                    file.Delete();
                }
            }

            // формирование растра
            string formula = Formula;
            formula = formula.Replace("=", "==");
            formula = formula.Replace(",", ".");
            // проверка формулы
            string formula_test = formula;
            formula_test = formula_test.Replace("==", "");
            formula_test = formula_test.Replace(".", "");
            formula_test = formula_test.Replace(">", "");
            formula_test = formula_test.Replace("<", "");
            formula_test = formula_test.Replace("+", "");
            formula_test = formula_test.Replace("-", "");
            formula_test = formula_test.Replace("*", "");
            formula_test = formula_test.Replace("/", "");
            formula_test = formula_test.Replace("&&", "");
            formula_test = formula_test.Replace("||", "");
            formula_test = formula_test.Replace("!", "");
            formula_test = formula_test.Replace("true", "");
            formula_test = formula_test.Replace("false", "");
            formula_test = formula_test.Replace("(", "");
            formula_test = formula_test.Replace(")", "");
            for (int n = 0; n <= 9; n++)
            {
                formula_test = formula_test.Replace(n.ToString(), "");
            }
            formula_test = formula_test.Replace("RADIATIONNASA", "");
            formula_test = formula_test.Replace("SETTLEDIST", "");
            formula_test = formula_test.Replace("POWERDIST", "");
            formula_test = formula_test.Replace("ROADDIST", "");
            formula_test = formula_test.Replace("SLOPE", "");
            formula_test = formula_test.Replace("ALTITUDE", "");
            formula_test = formula_test.Replace("SETTLEMENT", "");
            formula_test = formula_test.Replace("AGRI", "");
            formula_test = formula_test.Replace("WOOD", "");
            formula_test = formula_test.Replace("BUSH", "");
            formula_test = formula_test.Replace("HYDRO", "");
            formula_test = formula_test.Replace("TAKYR", "");
            formula_test = formula_test.Replace("DESERT", "");
            formula_test = formula_test.Replace("SOLON", "");
            formula_test = formula_test.Replace("STONE", "");
            formula_test = formula_test.Replace("PROTECT", "");
            formula_test = formula_test.Replace("RESERVE", "");
            formula_test = formula_test.Replace("PARK", "");
            formula_test = formula_test.Replace("NATRES", "");
            formula_test = formula_test.Replace("PRESERVE", "");
            if(formula_test.Trim()!="")
            {
                return Json(new
                {
                    Layer = "",
                    ErrorMessage = Resources.Common.InvalidFormula
                });
            }
            object result = EvalFindTerrain(formula, GeoServerAtlasSolar, out_file_name_pure, OblastId.ToString());
            if(result != null)
            {
                // публикация слоя
                string batfilename = Path.ChangeExtension(out_file_name, ".bat");
                StreamWriter bat = new StreamWriter(batfilename);
                bat.WriteLine($"curl -u admin:Ki5sh6fohh -v -XPOST -H \"Content-type: text/xml\" \\ -d \" <coverageStore> <name>{out_file_name_pure}</name> <workspace>AtlasSolar</workspace> <enabled>true</enabled> <type>GeoTIFF</type> <url>/coverages/AtlasSolar/FindTerrain/{out_file_name_pure}.tif</url> </coverageStore>\" \\ \"http://localhost:8080/geoserver/rest/workspaces/AtlasSolar/coveragestores?configure=all\"");
                bat.WriteLine($"curl -u admin:Ki5sh6fohh -v -XPOST -H \"Content-type: text/xml\" \\ -d \" <coverage> <name>{out_file_name_pure}</name> <title>{out_file_name_pure}</title> <nativeCRS>EPSG:3857</nativeCRS> <srs>EPSG:3857</srs> <projectionPolicy>FORCE_DECLARED</projectionPolicy> <defaultInterpolationMethod><name>nearest neighbor</name></defaultInterpolationMethod> </coverage> \" \\ \"http://localhost:8080/geoserver/rest/workspaces/AtlasSolar/coveragestores/{out_file_name_pure}/coverages?recalculate=nativebbox\"");
                bat.WriteLine($"curl -v -u admin:Ki5sh6fohh -XPUT -H \"Content-type: text/xml\" \\ -d \"<layer><defaultStyle><name>FindTerrain</name></defaultStyle></layer>\" http://localhost:8080/geoserver/rest/layers/AtlasSolar:{out_file_name_pure}");
                bat.Close();

                Process proc = new System.Diagnostics.Process();
                proc.StartInfo.FileName = batfilename;
                proc.StartInfo.Arguments = "/c start /wait " + Path.GetFileName(batfilename);
                proc.StartInfo.WorkingDirectory = Path.GetDirectoryName(batfilename);
                proc.Start();
                proc.WaitForExit();
            }
            else
            {
                return Json(new
                {
                    Layer = "",
                    ErrorMessage = Resources.Common.InvalidFormula
                });
            }
            
            
            return Json(new
            {
                Layer = out_file_name_pure,
                ErrorMessage = ""
            });
        }
        
        public static object EvalFindTerrain(string sCSCode, string GeoServerAtlasSolar, string OutFileName, string OblastId)
        {

            CSharpCodeProvider c = new CSharpCodeProvider();
            ICodeCompiler icc = c.CreateCompiler();
            CompilerParameters cp = new CompilerParameters();

            cp.ReferencedAssemblies.Add("system.dll");
            //cp.ReferencedAssemblies.Add("AtlasSolar.dll");

            cp.CompilerOptions = "/t:library";
            cp.GenerateInMemory = true;

            GeoServerAtlasSolar = GeoServerAtlasSolar.Replace("\\", "\\\\");
            StringBuilder sb = new StringBuilder("");
            sb.Append("using AtlasSolar;\n");
            sb.Append("using OSGeo.GDAL;\n");
            sb.Append("using System;\n");
            sb.Append("using System.Collections.Generic;\n");
            sb.Append("using System.Diagnostics;\n");
            sb.Append("using System.IO;\n");
            sb.Append("namespace CSCodeEvaler\n");
            sb.Append("{\n");
            sb.Append("    public class CSCodeEvaler\n");
            sb.Append("    {\n");
            sb.Append("        public object EvalCode()\n");
            sb.Append("        {\n");
            sb.Append("            string GeoServerAtlasSolar = \"" + GeoServerAtlasSolar + "\";\n");
            sb.Append("            string out_file_name_pure = $\"" + OutFileName + "\";\n");
            sb.Append("            string out_file_name = $\"{ out_file_name_pure}.tif\";\n");
            sb.Append("            int OblastId = 11;\n");
            sb.Append("            GdalConfiguration.ConfigureGdal();\n");
            sb.Append("            string auto_dist_file_name = Path.Combine(AtlasSolar.Properties.Settings.Default.GeoServerPath + @\"\\data_dir\\coverages\\AtlasSolar\\Provinces\", " + OblastId + " + \"auto_dist.tif\");\n");
            sb.Append("            Dataset auto_dist_ds = Gdal.Open(auto_dist_file_name, Access.GA_ReadOnly);\n");
            sb.Append("            Band auto_dist_band = auto_dist_ds.GetRasterBand(1);\n");
            sb.Append("            int auto_dist_width = auto_dist_band.XSize;\n");
            sb.Append("            int auto_dist_height = auto_dist_band.YSize;\n");
            sb.Append("            float[] auto_dist_array = new float[auto_dist_width * auto_dist_height];\n");
            sb.Append("            auto_dist_band.ReadRaster(0, 0, auto_dist_width, auto_dist_height, auto_dist_array, auto_dist_width, auto_dist_height, 0, 0);\n");
            sb.Append("            double auto_dist_out_val;\n");
            sb.Append("            int auto_dist_out_hasval;\n");
            sb.Append("            float auto_dist_NoDataValue = -1;\n");
            sb.Append("            auto_dist_band.GetNoDataValue(out auto_dist_out_val, out auto_dist_out_hasval);\n");
            sb.Append("            if (auto_dist_out_hasval != 0)\n");
            sb.Append("                auto_dist_NoDataValue = (float)auto_dist_out_val;\n");
            sb.Append("            string kzcoveriskl_file_name = Path.Combine(AtlasSolar.Properties.Settings.Default.GeoServerPath + @\"\\data_dir\\coverages\\AtlasSolar\\Provinces\", " + OblastId + " + \"kzcoveriskl.tif\");\n");
            sb.Append("            Dataset kzcoveriskl_ds = Gdal.Open(kzcoveriskl_file_name, Access.GA_ReadOnly);\n");
            sb.Append("            Band kzcoveriskl_band = kzcoveriskl_ds.GetRasterBand(1);\n");
            sb.Append("            int kzcoveriskl_width = kzcoveriskl_band.XSize;\n");
            sb.Append("            int kzcoveriskl_height = kzcoveriskl_band.YSize;\n");
            sb.Append("            byte[] kzcoveriskl_array = new byte[kzcoveriskl_width * kzcoveriskl_height];\n");
            sb.Append("            kzcoveriskl_band.ReadRaster(0, 0, kzcoveriskl_width, kzcoveriskl_height, kzcoveriskl_array, kzcoveriskl_width, kzcoveriskl_height, 0, 0);\n");
            sb.Append("            double kzcoveriskl_out_val;\n");
            sb.Append("            int kzcoveriskl_out_hasval;\n");
            sb.Append("            byte kzcoveriskl_NoDataValue = 255;\n");
            sb.Append("            kzcoveriskl_band.GetNoDataValue(out kzcoveriskl_out_val, out kzcoveriskl_out_hasval);\n");
            sb.Append("            if (kzcoveriskl_out_hasval != 0)\n");
            sb.Append("                kzcoveriskl_NoDataValue = (byte)kzcoveriskl_out_val;\n");
            sb.Append("            string lep_dist_file_name = Path.Combine(AtlasSolar.Properties.Settings.Default.GeoServerPath + @\"\\data_dir\\coverages\\AtlasSolar\\Provinces\", " + OblastId + " + \"lep_dist.tif\");\n");
            sb.Append("            Dataset lep_dist_ds = Gdal.Open(lep_dist_file_name, Access.GA_ReadOnly);\n");
            sb.Append("            Band lep_dist_band = lep_dist_ds.GetRasterBand(1);\n");
            sb.Append("            int lep_dist_width = lep_dist_band.XSize;\n");
            sb.Append("            int lep_dist_height = lep_dist_band.YSize;\n");
            sb.Append("            float[] lep_dist_array = new float[lep_dist_width * lep_dist_height];\n");
            sb.Append("            lep_dist_band.ReadRaster(0, 0, lep_dist_width, lep_dist_height, lep_dist_array, lep_dist_width, lep_dist_height, 0, 0);\n");
            sb.Append("            double lep_dist_out_val;\n");
            sb.Append("            int lep_dist_out_hasval;\n");
            sb.Append("            float lep_dist_NoDataValue = -1;\n");
            sb.Append("            lep_dist_band.GetNoDataValue(out lep_dist_out_val, out lep_dist_out_hasval);\n");
            sb.Append("            if (lep_dist_out_hasval != 0)\n");
            sb.Append("                lep_dist_NoDataValue = (float)lep_dist_out_val;\n");
            sb.Append("            string np_dist_file_name = Path.Combine(AtlasSolar.Properties.Settings.Default.GeoServerPath + @\"\\data_dir\\coverages\\AtlasSolar\\Provinces\", " + OblastId + " + \"np_dist.tif\");\n");
            sb.Append("            Dataset np_dist_ds = Gdal.Open(np_dist_file_name, Access.GA_ReadOnly);\n");
            sb.Append("            Band np_dist_band = np_dist_ds.GetRasterBand(1);\n");
            sb.Append("            int np_dist_width = np_dist_band.XSize;\n");
            sb.Append("            int np_dist_height = np_dist_band.YSize;\n");
            sb.Append("            float[] np_dist_array = new float[np_dist_width * np_dist_height];\n");
            sb.Append("            np_dist_band.ReadRaster(0, 0, np_dist_width, np_dist_height, np_dist_array, np_dist_width, np_dist_height, 0, 0);\n");
            sb.Append("            double np_dist_out_val;\n");
            sb.Append("            int np_dist_out_hasval;\n");
            sb.Append("            float np_dist_NoDataValue = -1;\n");
            sb.Append("            np_dist_band.GetNoDataValue(out np_dist_out_val, out np_dist_out_hasval);\n");
            sb.Append("            if (np_dist_out_hasval != 0)\n");
            sb.Append("                np_dist_NoDataValue = (float)np_dist_out_val;\n");
            sb.Append("            string ooptiskl_file_name = Path.Combine(AtlasSolar.Properties.Settings.Default.GeoServerPath + @\"\\data_dir\\coverages\\AtlasSolar\\Provinces\", " + OblastId + " + \"ooptiskl.tif\");\n");
            sb.Append("            Dataset ooptiskl_ds = Gdal.Open(ooptiskl_file_name, Access.GA_ReadOnly);\n");
            sb.Append("            Band ooptiskl_band = ooptiskl_ds.GetRasterBand(1);\n");
            sb.Append("            int ooptiskl_width = ooptiskl_band.XSize;\n");
            sb.Append("            int ooptiskl_height = ooptiskl_band.YSize;\n");
            sb.Append("            byte[] ooptiskl_array = new byte[ooptiskl_width * ooptiskl_height];\n");
            sb.Append("            ooptiskl_band.ReadRaster(0, 0, ooptiskl_width, ooptiskl_height, ooptiskl_array, ooptiskl_width, ooptiskl_height, 0, 0);\n");
            sb.Append("            double ooptiskl_out_val;\n");
            sb.Append("            int ooptiskl_out_hasval;\n");
            sb.Append("            byte ooptiskl_NoDataValue = 255;\n");
            sb.Append("            ooptiskl_band.GetNoDataValue(out ooptiskl_out_val, out ooptiskl_out_hasval);\n");
            sb.Append("            if (ooptiskl_out_hasval != 0)\n");
            sb.Append("                ooptiskl_NoDataValue = (byte)ooptiskl_out_val;\n");
            sb.Append("            string slope_srtm_file_name = Path.Combine(AtlasSolar.Properties.Settings.Default.GeoServerPath + @\"\\data_dir\\coverages\\AtlasSolar\\Provinces\", " + OblastId + " + \"slope_srtm.tif\");\n");
            sb.Append("            Dataset slope_srtm_ds = Gdal.Open(slope_srtm_file_name, Access.GA_ReadOnly);\n");
            sb.Append("            Band slope_srtm_band = slope_srtm_ds.GetRasterBand(1);\n");
            sb.Append("            int slope_srtm_width = slope_srtm_band.XSize;\n");
            sb.Append("            int slope_srtm_height = slope_srtm_band.YSize;\n");
            sb.Append("            float[] slope_srtm_array = new float[slope_srtm_width * slope_srtm_height];\n");
            sb.Append("            slope_srtm_band.ReadRaster(0, 0, slope_srtm_width, slope_srtm_height, slope_srtm_array, slope_srtm_width, slope_srtm_height, 0, 0);\n");
            sb.Append("            double slope_srtm_out_val;\n");
            sb.Append("            int slope_srtm_out_hasval;\n");
            sb.Append("            float slope_srtm_NoDataValue = -1;\n");
            sb.Append("            slope_srtm_band.GetNoDataValue(out slope_srtm_out_val, out slope_srtm_out_hasval);\n");
            sb.Append("            if (slope_srtm_out_hasval != 0)\n");
            sb.Append("                slope_srtm_NoDataValue = (float)slope_srtm_out_val;\n");
            sb.Append("            string srtm_file_name = Path.Combine(AtlasSolar.Properties.Settings.Default.GeoServerPath + @\"\\data_dir\\coverages\\AtlasSolar\\Provinces\", " + OblastId + " + \"srtm.tif\");\n");
            sb.Append("            Dataset srtm_ds = Gdal.Open(srtm_file_name, Access.GA_ReadOnly);\n");
            sb.Append("            Band srtm_band = srtm_ds.GetRasterBand(1);\n");
            sb.Append("            int srtm_width = srtm_band.XSize;\n");
            sb.Append("            int srtm_height = srtm_band.YSize;\n");
            sb.Append("            int[] srtm_array = new int[srtm_width * srtm_height];\n");
            sb.Append("            srtm_band.ReadRaster(0, 0, srtm_width, srtm_height, srtm_array, srtm_width, srtm_height, 0, 0);\n");
            sb.Append("            double srtm_out_val;\n");
            sb.Append("            int srtm_out_hasval;\n");
            sb.Append("            int srtm_NoDataValue = -1;\n");
            sb.Append("            srtm_band.GetNoDataValue(out srtm_out_val, out srtm_out_hasval);\n");
            sb.Append("            if (srtm_out_hasval != 0)\n");
            sb.Append("                srtm_NoDataValue = (int)srtm_out_val;\n");
            sb.Append("            string swvdwnyear_file_name = Path.Combine(AtlasSolar.Properties.Settings.Default.GeoServerPath + @\"\\data_dir\\coverages\\AtlasSolar\\Provinces\", " + OblastId + " + \"swvdwnyear.tif\");\n");
            sb.Append("            Dataset swvdwnyear_ds = Gdal.Open(swvdwnyear_file_name, Access.GA_ReadOnly);\n");
            sb.Append("            Band swvdwnyear_band = swvdwnyear_ds.GetRasterBand(1);\n");
            sb.Append("            int swvdwnyear_width = swvdwnyear_band.XSize;\n");
            sb.Append("            int swvdwnyear_height = swvdwnyear_band.YSize;\n");
            sb.Append("            float[] swvdwnyear_array = new float[swvdwnyear_width * swvdwnyear_height];\n");
            sb.Append("            swvdwnyear_band.ReadRaster(0, 0, swvdwnyear_width, swvdwnyear_height, swvdwnyear_array, swvdwnyear_width, swvdwnyear_height, 0, 0);\n");
            sb.Append("            double swvdwnyear_out_val;\n");
            sb.Append("            int swvdwnyear_out_hasval;\n");
            sb.Append("            float swvdwnyear_NoDataValue = -1;\n");
            sb.Append("            swvdwnyear_band.GetNoDataValue(out swvdwnyear_out_val, out swvdwnyear_out_hasval);\n");
            sb.Append("            if (swvdwnyear_out_hasval != 0)\n");
            sb.Append("                swvdwnyear_NoDataValue = (float)swvdwnyear_out_val;\n");
            sb.Append("            // out tif file\n");
            sb.Append("            int min_width = Math.Min(\n");
            sb.Append("                auto_dist_width,\n");
            sb.Append("                Math.Min(kzcoveriskl_width,\n");
            sb.Append("                Math.Min(lep_dist_width,\n");
            sb.Append("                Math.Min(np_dist_width,\n");
            sb.Append("                Math.Min(ooptiskl_width,\n");
            sb.Append("                Math.Min(slope_srtm_width,\n");
            sb.Append("                Math.Min(srtm_width,\n");
            sb.Append("                swvdwnyear_width))))))),\n");
            sb.Append("            min_height = Math.Min(\n");
            sb.Append("                auto_dist_height,\n");
            sb.Append("                Math.Min(kzcoveriskl_height,\n");
            sb.Append("                Math.Min(lep_dist_height,\n");
            sb.Append("                Math.Min(np_dist_height,\n");
            sb.Append("                Math.Min(ooptiskl_height,\n");
            sb.Append("                Math.Min(slope_srtm_height,\n");
            sb.Append("                Math.Min(srtm_height,\n");
            sb.Append("                swvdwnyear_height)))))));\n");
            sb.Append("            Driver out_drv = Gdal.GetDriverByName(\"GTiff\");\n");
            sb.Append("            int out_BlockXSize,\n");
            sb.Append("                out_BlockYSize;\n");
            sb.Append("            auto_dist_band.GetBlockSize(out out_BlockXSize, out out_BlockYSize);\n");
            sb.Append("            byte out_NoDataValue = 255;\n");
            sb.Append("            string[] out_options = new string[] { \"BLOCKXSIZE = \" + out_BlockXSize, \"BLOCKYSIZE = \" + out_BlockYSize, \"NODATAVALUE = \" + out_NoDataValue };\n");
            sb.Append("            byte[] out_buffer_array = new byte[min_width * min_height];\n");
            sb.Append("            for (int i = min_width - 1; i >= 0; i--)\n");
            sb.Append("            {\n");
            sb.Append("                for (int j = min_height - 1; j >= 0; j--)\n");
            sb.Append("                {\n");
            sb.Append("                    if (auto_dist_array[i + j * auto_dist_width] == auto_dist_NoDataValue)\n");
            sb.Append("                    {\n");
            sb.Append("                        out_buffer_array[i + j * min_width] = out_NoDataValue;\n");
            sb.Append("                    }\n");
            sb.Append("                    else\n");
            sb.Append("                    {\n");
            sb.Append("                        float RADIATIONNASA = swvdwnyear_array[i + j * swvdwnyear_width];\n");
            sb.Append("                        float SETTLEDIST = np_dist_array[i + j * np_dist_width] / 1000F;\n");
            sb.Append("                        float POWERDIST = lep_dist_array[i + j * lep_dist_width] / 1000F;\n");
            sb.Append("                        float ROADDIST = auto_dist_array[i + j * auto_dist_width] / 1000F;\n");
            sb.Append("                        float SLOPE = slope_srtm_array[i + j * slope_srtm_width];\n");
            sb.Append("                        float ALTITUDE = srtm_array[i + j * srtm_width];\n");
            sb.Append("                        bool SETTLEMENT = kzcoveriskl_array[i + j * kzcoveriskl_width] == 1 ? true : false;\n");
            sb.Append("                        bool AGRI = kzcoveriskl_array[i + j * kzcoveriskl_width] == 2 ? true : false;\n");
            sb.Append("                        bool WOOD = kzcoveriskl_array[i + j * kzcoveriskl_width] == 3 ? true : false;\n");
            sb.Append("                        bool BUSH = kzcoveriskl_array[i + j * kzcoveriskl_width] == 4 ? true : false;\n");
            sb.Append("                        bool HYDRO = kzcoveriskl_array[i + j * kzcoveriskl_width] == 5 ? true : false;\n");
            sb.Append("                        bool TAKYR = kzcoveriskl_array[i + j * kzcoveriskl_width] == 6 ? true : false;\n");
            sb.Append("                        bool DESERT = kzcoveriskl_array[i + j * kzcoveriskl_width] == 7 ? true : false;\n");
            sb.Append("                        bool SOLON = kzcoveriskl_array[i + j * kzcoveriskl_width] == 8 ? true : false;\n");
            sb.Append("                        bool STONE = kzcoveriskl_array[i + j * kzcoveriskl_width] == 9 ? true : false;\n");
            sb.Append("                        bool PROTECT = ooptiskl_array[i + j * ooptiskl_width] == 11 ? true : false;\n");
            sb.Append("                        bool RESERVE = ooptiskl_array[i + j * ooptiskl_width] == 22 ? true : false;\n");
            sb.Append("                        bool PARK = ooptiskl_array[i + j * ooptiskl_width] == 33 ? true : false;\n");
            sb.Append("                        bool NATRES = ooptiskl_array[i + j * ooptiskl_width] == 44 ? true : false;\n");
            sb.Append("                        bool PRESERVE = ooptiskl_array[i + j * ooptiskl_width] == 55 ? true : false;\n");
            sb.Append("                        out_buffer_array[i + j * min_width] = " + sCSCode + " ? (byte)1 : (byte)0;\n");
            sb.Append("                    }\n");
            sb.Append("                }\n");
            sb.Append("            }\n");
            sb.Append("            out_file_name = Path.Combine(GeoServerAtlasSolar + \"FindTerrain\", out_file_name);\n");
            sb.Append("            Dataset out_ds = out_drv.Create(out_file_name, min_width, min_height, 1, DataType.GDT_Byte, out_options);\n");
            sb.Append("            Band out_band = out_ds.GetRasterBand(1);\n");
            sb.Append("            out_band.WriteRaster(0, 0, min_width, min_height, out_buffer_array, min_width, min_height, 0, 0);\n");
            sb.Append("            out_band.SetNoDataValue(out_NoDataValue);\n");
            sb.Append("            out_ds.SetProjection(auto_dist_ds.GetProjection());\n");
            sb.Append("            double[] out_pGT = new double[6];\n");
            sb.Append("            auto_dist_ds.GetGeoTransform(out_pGT);\n");
            sb.Append("            out_ds.SetGeoTransform(out_pGT);\n");
            sb.Append("            out_ds.SetGCPs(auto_dist_ds.GetGCPs(), \"\");\n");
            sb.Append("            out_band.FlushCache();\n");
            sb.Append("            out_ds.FlushCache();\n");
            sb.Append("            out_band.Dispose();\n");
            sb.Append("            out_ds.Dispose();\n");
            sb.Append("            auto_dist_array = null;\n");
            sb.Append("            kzcoveriskl_array = null;\n");
            sb.Append("            lep_dist_array = null;\n");
            sb.Append("            np_dist_array = null;\n");
            sb.Append("            ooptiskl_array = null;\n");
            sb.Append("            slope_srtm_array = null;\n");
            sb.Append("            srtm_array = null;\n");
            sb.Append("            swvdwnyear_array = null;\n");
            sb.Append("            GC.Collect();\n");
            sb.Append("            return true;\n");
            sb.Append("        }\n");
            sb.Append("    }\n");
            sb.Append("}\n");

            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                try
                {
                    string location = assembly.Location;
                    if (!String.IsNullOrEmpty(location))
                    {
                        cp.ReferencedAssemblies.Add(location);
                    }
                }
                catch(NotSupportedException)
                {

                }
                
            }

            CompilerResults cr = icc.CompileAssemblyFromSource(cp, sb.ToString());
            if (cr.Errors.Count > 0)
            {
                return null;
            }

            
            System.Reflection.Assembly a = cr.CompiledAssembly;
            object o = a.CreateInstance("CSCodeEvaler.CSCodeEvaler");

            Type t = o.GetType();
            MethodInfo mi = t.GetMethod("EvalCode");

            object s = mi.Invoke(o, null);
            return s;
        }

        //[HttpPost]
        public FilePathResult DownloadMap(string FileName, string Folder)
        {
            string GeoServerAtlasSolar = Properties.Settings.Default.GeoServerPath + @"\data_dir\coverages\AtlasSolar\";
            return new FilePathResult(Path.Combine(GeoServerAtlasSolar + Folder, FileName + ".tif"), "image/tiff");
        }

        [HttpPost]
        public JsonResult GetMeteoDataPeriodicitiesBySource(int MeteoDataSourceId)
        {
            var meteodataperiodicities = db.MeteoDataPeriodicities
                .Where(m => false)
                .ToList();
            var meteodatatypes = db.MeteoDataTypes
                .Where(m => true)
                .Include(m => m.MeteoDataPeriodicity)
                .Include(m => m.MeteoDataSource)
                .ToList();
            foreach(MeteoDataType mdt in meteodatatypes)
            {
                if(mdt.MeteoDataSourceId == MeteoDataSourceId)
                {
                    meteodataperiodicities.Add(mdt.MeteoDataPeriodicity);
                }
            }
            meteodataperiodicities = meteodataperiodicities
                .Distinct()
                .ToList()
                .OrderBy(m => m.Name)
                .ToList();
            SelectList MeteoDataPeriodicities = new SelectList(meteodataperiodicities, "Id", "Name");
            JsonResult result = new JsonResult();
            result.Data = MeteoDataPeriodicities;
            return result;
        }

        [HttpPost]
        public JsonResult GetMeteoDataPeriodicitiesBySourceComparePoints(int MeteoDataSourceId)
        {
            var meteodataperiodicities = db.MeteoDataPeriodicities
                .Where(m => true)
                .ToList();
            var meteodatatypes = db.MeteoDataTypes
                .Where(m => true)
                .Include(m => m.MeteoDataPeriodicity)
                .Include(m => m.MeteoDataSource)
                .ToList();
            foreach (MeteoDataType mdt in meteodatatypes)
            {
                if (mdt.MeteoDataSourceId == MeteoDataSourceId)
                {
                    meteodataperiodicities.Add(mdt.MeteoDataPeriodicity);
                }
            }
            meteodataperiodicities = meteodataperiodicities
                .Distinct()
                .ToList()
                .Where(m => m.Code.ToLower().Contains(Properties.Settings.Default.Average) && m.Code.ToLower().Contains(Properties.Settings.Default.Monthly))
                .OrderBy(m => m.Name)
                .ToList();
            SelectList MeteoDataPeriodicities = new SelectList(meteodataperiodicities, "Id", "Name");
            JsonResult result = new JsonResult();
            result.Data = MeteoDataPeriodicities;
            return result;
        }

        [HttpPost]
        public JsonResult GetMeteoDataGroupsBySourcePeriodicity(int MeteoDataSourceId, int MeteoDataPeriodicityId)
        {
            var meteodatatypes = db.MeteoDataTypes
                .Where(m => m.MeteoDataSourceId == MeteoDataSourceId && m.MeteoDataPeriodicityId == MeteoDataPeriodicityId)
                .DistinctBy(m => m.Group)
                .ToList()
                .Where(m => m.Group != null && m.Group != "")
                .OrderBy(m => m.Group)
                .ToList();
            if(meteodatatypes.Count == 0)
            {
                meteodatatypes.Add(new MeteoDataType()
                {
                    GroupEN = Resources.Common.All,
                    GroupKZ = Resources.Common.All,
                    GroupRU = Resources.Common.All
                });
            }
            SelectList MeteoDataTypes = new SelectList(meteodatatypes, "Group", "Group");
            JsonResult result = new JsonResult();
            result.Data = MeteoDataTypes;
            return result;
        }

        [HttpPost]
        public JsonResult GetMeteoDataTypesBySourcePeriodicityGroup(int MeteoDataSourceId, int MeteoDataPeriodicityId, string Group)
        {
            var meteodatatypes = db.MeteoDataTypes
                .Where(m => m.MeteoDataSourceId == MeteoDataSourceId && m.MeteoDataPeriodicityId == MeteoDataPeriodicityId)
                .ToList()
                .Where(m => m.Group == Group || m.Group == null || m.Group == "")
                .OrderBy(m => m.NameGroupAdditional)
                .ToList();
            SelectList MeteoDataTypes = new SelectList(meteodatatypes, "Id", "NameGroupAdditional");
            JsonResult result = new JsonResult();
            result.Data = MeteoDataTypes;
            return result;
        }

        [HttpPost]
        public JsonResult GetMeteoDataTypesBySourcePeriodicity(int MeteoDataSourceId, int MeteoDataPeriodicityId)
        {
            var meteodatatypes = db.MeteoDataTypes
                .Where(m => m.MeteoDataSourceId == MeteoDataSourceId && m.MeteoDataPeriodicityId == MeteoDataPeriodicityId)
                .ToList()
                .OrderBy(m => m.NameGroupAdditional)
                .ToList();
            SelectList MeteoDataTypes = new SelectList(meteodatatypes, "Id", "NameGroupAdditional");
            JsonResult result = new JsonResult();
            result.Data = MeteoDataTypes;
            return result;
        }

        [HttpPost]
        public ActionResult GetMeteoDataRegion(int MeteoDataTypeId, int OblastId)
        {
            return Json(new
            {
                FilePath = "/DownloadMeteoData/" + MeteoDataTypeId.ToString() + " " + OblastId.ToString() + ".zip"
            });
        }
    }
}