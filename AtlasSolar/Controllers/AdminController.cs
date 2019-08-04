using AtlasSolar.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using OSGeo.GDAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Resources;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.IO.Compression;

namespace AtlasSolar.Controllers
{
    public class AdminController : Controller
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

        // GET: Admin
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            return View();
        }

        private int AddMeteoDataPeriodicityIfNotExist(MeteoDataPeriodicity MDP, ref string Error)
        {
            try
            {
                int count = db.MeteoDataPeriodicities.Where(m => m.Code == MDP.Code).Count();
                if (count == 0)
                {
                    db.MeteoDataPeriodicities.Add(MDP);
                    db.SaveChanges();
                    return 1;
                }
                return 0;
            }
            catch (Exception ex)
            {
                Error = ex.Message;
                return 0;
            }
        }

        private int AddMeteoDataSourceIfNotExist(MeteoDataSource MDS, ref string Error)
        {
            try
            {
                int count = db.MeteoDataSources.Where(m => m.Code == MDS.Code).Count();
                if (count == 0)
                {
                    db.MeteoDataSources.Add(MDS);
                    db.SaveChanges();
                    return 1;
                }
                return 0;
            }
            catch (Exception ex)
            {
                Error = ex.Message;
                return 0;
            }
        }

        // GET
        [Authorize(Roles = "Admin")]
        public ActionResult Init()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        // определение минимальных необходимых данных, запускается один раз
        public ActionResult Init(bool? post)
        {
            SystemLog.New("Init", true);

            List<string> errors = new List<string>();

            // метео периодичности
            List<MeteoDataPeriodicity> mdps = new List<MeteoDataPeriodicity>();
            mdps.Add(new MeteoDataPeriodicity()
            {
                Code = "Monthly average",
                NameEN = "Monthly average",
                NameKZ = "Ай орташа",
                NameRU = "Среднемесячные"
            });
            mdps.Add(new MeteoDataPeriodicity()
            {
                Code = "Daily",
                NameEN = "Daily",
                NameKZ = "Күндізгі",
                NameRU = "Дневные"
            });
            mdps.Add(new MeteoDataPeriodicity()
            {
                Code = "Monthly",
                NameEN = "Monthly",
                NameKZ = "Ай",
                NameRU = "Месячные"
            });
            int mdp_count = 0,
                mdp_count_no = 0;
            foreach (MeteoDataPeriodicity mdp in mdps)
            {
                string error = "";
                int r = AddMeteoDataPeriodicityIfNotExist(mdp, ref error);
                mdp_count += r;
                if (r == 0)
                {
                    mdp_count_no++;
                }
                if (!string.IsNullOrEmpty(error))
                {
                    errors.Add(error);
                }
            }

            // метео источники
            List<MeteoDataSource> mdss = new List<MeteoDataSource>();
            mdss.Add(new MeteoDataSource()
            {
                Code = "NASA SSE",
                NameEN = "NASA SSE",
                NameKZ = "NASA SSE",
                NameRU = "NASA SSE"
            });
            mdss.Add(new MeteoDataSource()
            {
                Code = "NASA POWER",
                NameEN = "NASA POWER",
                NameKZ = "NASA POWER",
                NameRU = "NASA POWER"
            });
            mdss.Add(new MeteoDataSource()
            {
                Code = "SARAH-E",
                NameEN = "SARAH-E",
                NameKZ = "SARAH-E",
                NameRU = "SARAH-E"
            });
            mdss.Add(new MeteoDataSource()
            {
                Code = "CLARA",
                NameEN = "CLARA",
                NameKZ = "CLARA",
                NameRU = "CLARA"
            });
            int mds_count = 0,
                mds_count_no = 0;
            foreach (MeteoDataSource mds in mdss)
            {
                string error = "";
                int r = AddMeteoDataSourceIfNotExist(mds, ref error);
                mds_count += r;
                if (r == 0)
                {
                    mds_count_no++;
                }
                if (!string.IsNullOrEmpty(error))
                {
                    errors.Add(error);
                }
            }

            errors = errors.Distinct().ToList();

            string comment = $"Добавлено {mdp_count.ToString()} метеорологических периодичностей из {(mdp_count + mdp_count_no).ToString()}.";
            comment += $" Добавлено {mds_count.ToString()} метеорологических источников из {(mds_count + mds_count_no).ToString()};";
            string errorss = errors.Count() == 0 ? null : string.Join("; ", errors.ToArray());
            SystemLog.New("Init",
                comment,
                errorss,
                false);

            ViewBag.Report = (comment + Environment.NewLine + errorss).Replace(Environment.NewLine, "<br />");

            return View();
        }

        // GET
        [Authorize(Roles = "Admin")]
        public ActionResult ParseMeteoDataTypes()
        {
            return View();
        }

        // получение списка метеорологических параметров NASA SSE, NASA POWER
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult ParseMeteoDataTypes(bool? post)
        {
            string userid = User.Identity.GetUserId(),
                    user = User.Identity.Name;
            SystemLog.New("ParseMeteoDataTypes", true);
            new Thread(() =>
            {
                List<string> errors = new List<string>();
                string comment = "";

                // среднемесячные типы NASA SSE
                try
                {
                    int count_NASA_SSE = 0;

                    using (var db = new NpgsqlContext())
                    {
                        List<MeteoDataType> newmeteodatatypes = new List<MeteoDataType>();
                        for (decimal latitude = Properties.Settings.Default.NASASSELatitudeMin; latitude <= Properties.Settings.Default.NASASSELatitudeMax; latitude += Properties.Settings.Default.NASASSECoordinatesStep)
                        {
                            HtmlAgilityPack.HtmlDocument html = new HtmlAgilityPack.HtmlDocument();
                            HtmlAgilityPack.HtmlNode root = html.DocumentNode;
                            decimal longitude = Properties.Settings.Default.NASASSELongitudeMin;
                            string url = "https://eosweb.larc.nasa.gov/cgi-bin/sse/grid.cgi?&num=225129&lat=" +
                                latitude.ToString().Replace(",", ".") +
                                "&hgt=100&submit=Submit&veg=17&sitelev=&email=skip@larc.nasa.gov&p=grid_id&step=2&lon=" +
                                longitude.ToString().Replace(",", ".");
                            html = new HtmlAgilityPack.HtmlDocument();
                            html.LoadHtml(new WebClient().DownloadString(url));
                            root = html.DocumentNode;
                            string group = "";
                            string code = "";
                            string name = "";
                            int current_tr_i = 0;

                            List<MeteoDataType> meteodatatypes = db.MeteoDataTypes.ToList();
                            List<MeteoDataPeriodicity> meteodataperiodicities = db.MeteoDataPeriodicities.ToList();
                            List<MeteoDataSource> meteodatasources = db.MeteoDataSources.ToList();

                            foreach (HtmlAgilityPack.HtmlNode node in root.Descendants())
                            {
                                if (node.Name == "big")
                                {
                                    group = node.FirstChild.FirstChild.InnerText.Replace(":", "").Trim();
                                }
                                if (node.Name == "a")
                                {
                                    if (node.GetAttributeValue("name", "") != "")
                                    {
                                        code = node.GetAttributeValue("name", "").Trim();
                                    }
                                }
                                if ((code != "") && (name == "") && (node.Name == "b"))
                                {
                                    name = HttpUtility.HtmlDecode(node.InnerText.Trim()).Trim();
                                }

                                if ((code != "") && (name != ""))
                                {
                                    if (node.ParentNode.Name == "table")
                                    {
                                        current_tr_i = 0;
                                        foreach (HtmlAgilityPack.HtmlNode tr in node.ParentNode.Descendants("tr"))
                                        {
                                            current_tr_i++;
                                            if (current_tr_i > 1)
                                            {
                                                string additional = "";
                                                int MeteoDataSourceId = meteodatasources.Where(m => m.Code == Properties.Settings.Default.NASASSECode).FirstOrDefault().Id;
                                                int MeteoDataPeriodicityId = meteodataperiodicities.Where(m => m.Code.ToLower().Contains(Properties.Settings.Default.Monthly.ToLower()) && m.Code.ToLower().Contains(Properties.Settings.Default.Average.ToLower())).FirstOrDefault().Id;
                                                additional = HttpUtility.HtmlDecode(tr.Descendants("td").FirstOrDefault().InnerText.Trim());
                                                int is_yet = meteodatatypes
                                                    .Where(m => m.MeteoDataSourceId == MeteoDataSourceId && m.MeteoDataPeriodicityId == MeteoDataPeriodicityId && m.Code == code && m.AdditionalEN == additional && m.GroupEN == group)
                                                    .Count();
                                                is_yet += newmeteodatatypes
                                                    .Where(m => m.MeteoDataSourceId == MeteoDataSourceId && m.MeteoDataPeriodicityId == MeteoDataPeriodicityId && m.Code == code && m.AdditionalEN == additional && m.GroupEN == group)
                                                    .Count();
                                                if (is_yet > 0)
                                                {
                                                    continue;
                                                }
                                                else
                                                {
                                                    newmeteodatatypes.Add(new MeteoDataType()
                                                    {
                                                        MeteoDataSourceId = MeteoDataSourceId,
                                                        MeteoDataPeriodicityId = MeteoDataPeriodicityId,
                                                        Code = code,
                                                        NameEN = name,
                                                        NameRU = name,
                                                        NameKZ = name,
                                                        AdditionalEN = additional,
                                                        AdditionalKZ = additional,
                                                        AdditionalRU = additional,
                                                        GroupEN = group,
                                                        GroupKZ = group,
                                                        GroupRU = group
                                                    });
                                                }
                                            }
                                        }
                                        code = "";
                                        name = "";
                                    }
                                }
                            }
                        }

                        foreach (MeteoDataType mdt in newmeteodatatypes.Distinct())
                        {
                            db.MeteoDataTypes.Add(mdt);
                            count_NASA_SSE++;
                        }

                        db.SaveChanges();
                        db.Dispose();
                        GC.Collect();
                    }

                    comment = $"Добавлено {count_NASA_SSE.ToString()} среднемесячных метеорологических параметров {Properties.Settings.Default.NASASSECode}.{Environment.NewLine}";
                }
                catch (Exception ex)
                {
                    errors.Add(ex.Message);
                }

                // ежедневные типы NASA SSE
                try
                {
                    int count_NASA_SSE = 0;

                    HtmlAgilityPack.HtmlDocument html = new HtmlAgilityPack.HtmlDocument();
                    HtmlAgilityPack.HtmlNode root = html.DocumentNode;
                    decimal longitude = Properties.Settings.Default.NASASSELongitudeMin,
                            latitude = Properties.Settings.Default.NASASSELatitudeMin;
                    int year = Properties.Settings.Default.NASASSEYearStart;
                    string url = "https://eosweb.larc.nasa.gov/cgi-bin/sse/daily.cgi?email=skip%40larc.nasa.gov&step=1&lat=" +
                        latitude.ToString().Replace(",", ".") +
                        "&lon=" +
                        longitude.ToString().Replace(",", ".") +
                        "&sitelev=&ms=1&ds=1&ys=" +
                        year.ToString() +
                        "&me=12&de=31&ye=" +
                        year.ToString() +
                        "&p=swv_dwn&p=avg_kt&p=clr_sky&p=clr_dif&p=clr_dnr&p=clr_kt&p=lwv_dwn&p=toa_dwn&p=PS&p=TSKIN&p=T10M&p=T10MN&p=T10MX&p=Q10M&p=RH10M&p=DFP10M&submit=Submit&plot=swv_dwn";
                    html = new HtmlAgilityPack.HtmlDocument();
                    html.LoadHtml(new WebClient().DownloadString(url));
                    root = html.DocumentNode;
                    string file_url = "https://eosweb.larc.nasa.gov";
                    foreach (HtmlAgilityPack.HtmlNode node in root.Descendants())
                    {
                        if ((node.Name == "a") && (node.InnerText == "Download a text file"))
                        {
                            file_url += node.GetAttributeValue("href", "");
                            break;
                        }
                    }
                    string file = (new WebClient()).DownloadString(file_url);
                    string[] lines = file.Split('\n');
                    using (var db = new NpgsqlContext())
                    {
                        List<MeteoDataType> meteodatatypes = db.MeteoDataTypes.ToList();
                        List<MeteoDataPeriodicity> meteodataperiodicities = db.MeteoDataPeriodicities.ToList();
                        List<MeteoDataSource> meteodatasources = db.MeteoDataSources.ToList();
                        bool begin = false;
                        foreach (string line in lines)
                        {
                            if (string.IsNullOrEmpty(line))
                            {
                                break;
                            }
                            if (begin)
                            {
                                string linen = line;
                                while (linen.Contains("   "))
                                {
                                    linen = linen.Replace("   ", "  ");
                                }
                                string[] linecolumns = linen.Split(new string[] { "  " }, StringSplitOptions.None);
                                int MeteoDataSourceId = meteodatasources.Where(m => m.Code == Properties.Settings.Default.NASASSECode).FirstOrDefault().Id;
                                int MeteoDataPeriodicityId = meteodataperiodicities.Where(m => m.Code.ToLower() == Properties.Settings.Default.Daily).FirstOrDefault().Id;
                                int is_yet = meteodatatypes
                                    .Where(m => m.MeteoDataSourceId == MeteoDataSourceId && m.MeteoDataPeriodicityId == MeteoDataPeriodicityId && m.Code == linecolumns[0])
                                    .Count();
                                if (is_yet > 0)
                                {
                                    continue;
                                }
                                else
                                {
                                    db.MeteoDataTypes.Add(new MeteoDataType()
                                    {
                                        MeteoDataSourceId = MeteoDataSourceId,
                                        MeteoDataPeriodicityId = MeteoDataPeriodicityId,
                                        Code = linecolumns[0].Trim(),
                                        NameEN = linecolumns[1].Trim(),
                                        NameKZ = linecolumns[1].Trim(),
                                        NameRU = linecolumns[1].Trim()
                                    });
                                    count_NASA_SSE++;
                                }
                            }
                            if (line.Contains("Parameter(s):"))
                            {
                                begin = true;
                            }
                        }
                        db.SaveChanges();
                        db.Dispose();
                        GC.Collect();
                        comment += $"Добавлено {count_NASA_SSE.ToString()} ежедневных метеорологических параметров {Properties.Settings.Default.NASASSECode}.{Environment.NewLine}";
                    }
                }
                catch (Exception ex)
                {
                    errors.Add(ex.Message);
                }

                // ежедневные типы NASA POWER
                try
                {
                    int count_NASA_POWER = 0;

                    string file_url = "https://power.larc.nasa.gov/cgi-bin/hirestimeser.cgi?email=hirestimeser%40larc.nasa.gov&step=1&lat=" +
                        Properties.Settings.Default.NASAPOWERLatitudeMin.ToString().Replace(",", ".") +
                        "&lon=" +
                        Properties.Settings.Default.NASAPOWERLongitudeMin.ToString().Replace(",", ".") +
                        "&ms=1&ds=1&ys=" +
                        Properties.Settings.Default.NASAPOWERYearStart.ToString() +
                        "&me=1&de=1&ye=" +
                        Properties.Settings.Default.NASAPOWERYearStart.ToString() +
                        "&p=MHswv_dwn&p=MHlwv_dwn&p=MHtoa_dwn&p=MHclr_sky&p=MHPS&p=MHT2M&p=MHT2MN&p=MHT2MX&p=MHQV2M&p=MHRH2M&p=MHDFP2M&p=MHTS&p=MHWS10M&p=MHPRECTOT&submit=Submit";
                    string file = "";
                    bool ok = false;
                    while (!ok)
                    {
                        try
                        {
                            file = (new WebClient()).DownloadString(file_url);
                            ok = true;
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                    string[] lines = file.Split('\n');
                    using (var db = new NpgsqlContext())
                    {
                        List<MeteoDataType> meteodatatypes = db.MeteoDataTypes.ToList();
                        List<MeteoDataPeriodicity> meteodataperiodicities = db.MeteoDataPeriodicities.ToList();
                        List<MeteoDataSource> meteodatasources = db.MeteoDataSources.ToList();
                        bool begin = false;
                        foreach (string line in lines)
                        {
                            if (line.Contains("YEAR MO DY"))
                            {
                                break;
                            }
                            if (begin)
                            {
                                string linen = line;
                                while (linen.Contains("   "))
                                {
                                    linen = linen.Replace("   ", "  ");
                                }
                                string[] linecolumns = linen.Split(new string[] { "  " }, StringSplitOptions.None);
                                int MeteoDataSourceId = meteodatasources.Where(m => m.Code == Properties.Settings.Default.NASAPOWERCode).FirstOrDefault().Id;
                                int MeteoDataPeriodicityId = meteodataperiodicities.Where(m => m.Code.ToLower() == Properties.Settings.Default.Daily).FirstOrDefault().Id;
                                int is_yet = meteodatatypes
                                    .Where(m => m.MeteoDataSourceId == MeteoDataSourceId && m.MeteoDataPeriodicityId == MeteoDataPeriodicityId && m.Code == linecolumns[0])
                                    .Count();
                                if (is_yet > 0)
                                {
                                    continue;
                                }
                                else
                                {
                                    db.MeteoDataTypes.Add(new MeteoDataType()
                                    {
                                        MeteoDataSourceId = MeteoDataSourceId,
                                        MeteoDataPeriodicityId = MeteoDataPeriodicityId,
                                        Code = linecolumns[0].Trim(),
                                        NameEN = linecolumns[1].Trim(),
                                        NameKZ = linecolumns[1].Trim(),
                                        NameRU = linecolumns[1].Trim()
                                    });
                                    count_NASA_POWER++;
                                }
                            }
                            if (line.Contains("Parameter(s):"))
                            {
                                begin = true;
                            }
                        }
                        db.SaveChanges();
                        db.Dispose();
                        GC.Collect();
                        comment += $"Добавлено {count_NASA_POWER.ToString()} ежедневных метеорологических параметров {Properties.Settings.Default.NASAPOWERCode}.";
                    }
                }
                catch (Exception ex)
                {
                    errors.Add(ex.Message);
                }

                // среднемесячные типы NASA POWER
                try
                {
                    int count_NASA_POWER = 0;

                    using (var db = new NpgsqlContext())
                    {
                        List<MeteoDataType> newmeteodatatypes = new List<MeteoDataType>();
                        for (decimal latitude = Properties.Settings.Default.NASASSELatitudeMin; latitude <= Properties.Settings.Default.NASASSELatitudeMax; latitude += Properties.Settings.Default.NASASSECoordinatesStep)
                        {
                            HtmlAgilityPack.HtmlDocument html = new HtmlAgilityPack.HtmlDocument();
                            HtmlAgilityPack.HtmlNode root = html.DocumentNode;
                            decimal longitude = Properties.Settings.Default.NASASSELongitudeMin;
                            string url = "https://power.larc.nasa.gov/cgi-bin/sustain.cgi?email=sustain%40larc.nasa.gov&step=2&lat=" +
                                latitude.ToString().Replace(",", ".") +
                                "&lon=" +
                                longitude.ToString().Replace(",", ".") +
                                "&num=225129&submit=Submit&p=grid_id&veg=17&hgt=10&sitelev=";
                            html = new HtmlAgilityPack.HtmlDocument();
                            html.LoadHtml(new WebClient().DownloadString(url));
                            root = html.DocumentNode;
                            string group = "";
                            string code = "";
                            string name = "";
                            int current_tr_i = 0;

                            List<MeteoDataType> meteodatatypes = db.MeteoDataTypes.ToList();
                            List<MeteoDataPeriodicity> meteodataperiodicities = db.MeteoDataPeriodicities.ToList();
                            List<MeteoDataSource> meteodatasources = db.MeteoDataSources.ToList();

                            foreach (HtmlAgilityPack.HtmlNode node in root.Descendants())
                            {
                                if (node.Name == "big")
                                {
                                    if (node.FirstChild.FirstChild != null)
                                    {
                                        group = node.FirstChild.FirstChild.InnerText.Replace(":", "").Trim();
                                    }
                                }
                                if (node.Name == "a")
                                {
                                    if (node.GetAttributeValue("name", "") != "")
                                    {
                                        code = node.GetAttributeValue("name", "").Trim();
                                    }
                                }
                                if ((code != "") && (name == "") && (node.Name == "b"))
                                {
                                    name = HttpUtility.HtmlDecode(node.InnerText.Trim()).Trim();
                                }

                                if ((code != "") && (name != ""))
                                {
                                    if (node.ParentNode.Name == "table")
                                    {
                                        current_tr_i = 0;
                                        foreach (HtmlAgilityPack.HtmlNode tr in node.ParentNode.Descendants("tr"))
                                        {
                                            current_tr_i++;
                                            if (current_tr_i > 1)
                                            {
                                                string additional = "";
                                                int MeteoDataSourceId = meteodatasources.Where(m => m.Code == Properties.Settings.Default.NASAPOWERCode).FirstOrDefault().Id;
                                                int MeteoDataPeriodicityId = meteodataperiodicities.Where(m => m.Code.ToLower().Contains(Properties.Settings.Default.Monthly.ToLower()) && m.Code.ToLower().Contains(Properties.Settings.Default.Average.ToLower())).FirstOrDefault().Id;
                                                additional = HttpUtility.HtmlDecode(tr.Descendants("td").FirstOrDefault().InnerText.Trim());
                                                int is_yet = meteodatatypes
                                                    .Where(m => m.MeteoDataSourceId == MeteoDataSourceId && m.MeteoDataPeriodicityId == MeteoDataPeriodicityId && m.Code == code && m.AdditionalEN == additional && m.GroupEN == group)
                                                    .Count();
                                                is_yet += newmeteodatatypes
                                                    .Where(m => m.MeteoDataSourceId == MeteoDataSourceId && m.MeteoDataPeriodicityId == MeteoDataPeriodicityId && m.Code == code && m.AdditionalEN == additional && m.GroupEN == group)
                                                    .Count();
                                                if (is_yet > 0)
                                                {
                                                    continue;
                                                }
                                                else
                                                {
                                                    newmeteodatatypes.Add(new MeteoDataType()
                                                    {
                                                        MeteoDataSourceId = MeteoDataSourceId,
                                                        MeteoDataPeriodicityId = MeteoDataPeriodicityId,
                                                        Code = code,
                                                        NameEN = name,
                                                        NameRU = name,
                                                        NameKZ = name,
                                                        AdditionalEN = additional,
                                                        AdditionalKZ = additional,
                                                        AdditionalRU = additional,
                                                        GroupEN = group,
                                                        GroupKZ = group,
                                                        GroupRU = group
                                                    });
                                                }
                                            }
                                        }
                                        code = "";
                                        name = "";
                                    }
                                }
                            }
                        }

                        foreach (MeteoDataType mdt in newmeteodatatypes.Distinct())
                        {
                            db.MeteoDataTypes.Add(mdt);
                            count_NASA_POWER++;
                        }

                        db.SaveChanges();
                        db.Dispose();
                        GC.Collect();
                    }

                    comment = $"Добавлено {count_NASA_POWER.ToString()} среднемесячных метеорологических параметров {Properties.Settings.Default.NASAPOWERCode}.{Environment.NewLine}";
                }
                catch (Exception ex)
                {
                    errors.Add(ex.Message);
                }

                string errorss = errors.Count() == 0 ? null : string.Join("; ", errors.ToArray());
                SystemLog.New(
                    userid,
                    user,
                    "ParseMeteoDataTypes",
                    comment,
                    errorss,
                    false);
            }).Start();
            ViewBag.Report = "Операция ParseMeteoDataTypes запущена.";
            return View();
        }

        // GET
        [Authorize(Roles = "Admin")]
        public ActionResult ParseMeteoDataNASASSEMonthlyAverage()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public void ParseMeteoDataNASASSEMonthlyAverageTask()
        {
            string userid = User.Identity.GetUserId(),
                    user = User.Identity.Name;

            List<string> errors = new List<string>();
            string comment = "";

            try
            {
                int count = 0;

                List<MeteoDataType> meteodatatypes = new List<MeteoDataType>();
                using (var db = new NpgsqlContext())
                {
                    meteodatatypes = db.MeteoDataTypes
                        .Where(m => m.MeteoDataSource.Code == Properties.Settings.Default.NASASSECode
                            && m.MeteoDataPeriodicity.Code.ToLower().Contains(Properties.Settings.Default.Monthly)
                            && m.MeteoDataPeriodicity.Code.ToLower().Contains(Properties.Settings.Default.Average))
                        .ToList();
                    db.Dispose();
                    GC.Collect();
                }

                List<int> meteodatatypeids = meteodatatypes.Select(m => m.Id).ToList();
                MeteoData md = db.MeteoDatas.Where(m => meteodatatypeids.Contains(m.MeteoDataTypeId)).FirstOrDefault();
                if (md != null)
                {
                    comment = $"Добавлено {count.ToString()} среднемесячных метеорологических данных {Properties.Settings.Default.NASASSECode}.";
                }
                else
                {
                    decimal longitude_min = Properties.Settings.Default.NASASSELongitudeMin,
                            longitude_max = Properties.Settings.Default.NASASSELongitudeMax,
                            latitude_min = Properties.Settings.Default.NASASSELatitudeMin,
                            latitude_max = Properties.Settings.Default.NASASSELatitudeMax;
                    string CurrentUserId = userid;
                    string path = "~/Upload/" + CurrentUserId;
                    if (!Directory.Exists(Server.MapPath(path)))
                    {
                        DirectoryInfo di = Directory.CreateDirectory(Server.MapPath(path));
                    }
                    string filenameimport = Path.Combine(Server.MapPath(path), $"{Properties.Settings.Default.NASASSECode} - {Properties.Settings.Default.Monthly} - {Properties.Settings.Default.Average}.txt");
                    System.IO.File.Delete(filenameimport);
                    using (StreamWriter sw = System.IO.File.AppendText(filenameimport))
                    {
                        for (decimal latitude = latitude_min; latitude <= latitude_max; latitude += Properties.Settings.Default.NASASSECoordinatesStep)
                        {
                            for (decimal longitude = longitude_min; longitude <= longitude_max; longitude += Properties.Settings.Default.NASASSECoordinatesStep)
                            {
                                string url = "https://eosweb.larc.nasa.gov/cgi-bin/sse/grid.cgi?&num=225129&lat=" +
                                                latitude.ToString().Replace(",", ".") +
                                                "&hgt=100&submit=Submit&veg=17&sitelev=&email=skip@larc.nasa.gov&p=grid_id&step=2&lon=" +
                                                longitude.ToString().Replace(",", ".");
                                HtmlAgilityPack.HtmlDocument html = new HtmlAgilityPack.HtmlDocument();
                                html.LoadHtml(new WebClient().DownloadString(url));

                                HtmlAgilityPack.HtmlNode root = html.DocumentNode;

                                string slat = "",
                                        slon = "";
                                foreach (HtmlAgilityPack.HtmlNode td in root.Descendants("td"))
                                {
                                    if (td.InnerText.IndexOf("Center") >= 0)
                                    {
                                        IEnumerable<HtmlAgilityPack.HtmlNode> bs = td.Descendants("b");
                                        slat = bs.First().InnerText;
                                        slon = bs.Last().InnerText;
                                        break;
                                    }
                                }

                                string current_table_name = "";
                                string group = "";
                                foreach (HtmlAgilityPack.HtmlNode node in root.Descendants())
                                {
                                    if (node.Name == "big")
                                    {
                                        group = node.FirstChild.FirstChild.InnerText.Replace(":", "").Trim();
                                    }
                                    if (node.Name == "a")
                                    {
                                        if (node.GetAttributeValue("name", "") != "")
                                        {
                                            current_table_name = HttpUtility.HtmlDecode(node.GetAttributeValue("name", "").Trim());
                                        }
                                    }
                                    if ((current_table_name != "") && (node.Name == "div"))
                                    {
                                        int current_tr_i = 0;
                                        foreach (HtmlAgilityPack.HtmlNode tr in node.Descendants("tr"))
                                        {
                                            current_tr_i++;
                                            if (current_tr_i > 1)
                                            {
                                                int current_td_i = 0;
                                                string row_name = "";
                                                foreach (HtmlAgilityPack.HtmlNode td in tr.Descendants("td"))
                                                {
                                                    current_td_i++;
                                                    if (current_td_i == 1)
                                                    {
                                                        row_name = HttpUtility.HtmlDecode(td.InnerText.Trim());
                                                    }
                                                    else
                                                    {
                                                        decimal out_decimal = 0;
                                                        int MeteoDataTypeId = meteodatatypes
                                                            .Where(m => m.Code == current_table_name && m.AdditionalEN == row_name && m.AdditionalEN == group)
                                                            .FirstOrDefault()
                                                            .Id;
                                                        string V = td.InnerText.Replace("*", "").Trim();
                                                        decimal? value = decimal.TryParse(V.Replace(".", ","), out out_decimal) ? Convert.ToDecimal(V.Replace(".", ",")) : (decimal?)null;
                                                        sw.WriteLine(
                                                            MeteoDataTypeId.ToString() + "\t" +
                                                            "\t" +
                                                            (current_td_i - 1).ToString() + "\t" +
                                                            "\t" +
                                                            Convert.ToDecimal(slon.Replace(".", ",")).ToString().Replace(",", ".") + "\t" +
                                                            Convert.ToDecimal(slat.Replace(".", ",")).ToString().Replace(",", ".") + "\t" +
                                                            value.ToString().Replace(",", ".")
                                                            );
                                                        count++;
                                                    }
                                                }
                                            }
                                        }
                                        current_table_name = "";
                                    }
                                }
                            }
                        }
                    }

                    string query = "COPY \"MeteoData\" (\"MeteoDataTypeId\", \"Year\", \"Month\", \"Day\", \"Longitude\", \"Latitude\", \"Value\") FROM '" + filenameimport + "' WITH NULL AS ''";
                    try
                    {
                        db.MeteoDatas.SqlQuery(query).SingleOrDefault();
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message != "The data reader is incompatible with the specified 'AtlasSolar.Models.MeteoData'. A member of the type, 'Id', does not have a corresponding column in the data reader with the same name.")
                        {
                            errors.Add(ex.Message);
                        }
                    }
                    if (errors.Count() == 0)
                    {
                        System.IO.File.Delete(filenameimport);
                        comment = $"Добавлено {count.ToString()} среднемесячных метеорологических данных {Properties.Settings.Default.NASASSECode}.";
                    }
                }
            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
            }

            string errorss = errors.Count() == 0 ? null : string.Join("; ", errors.ToArray());
            SystemLog.New(
                userid,
                user,
                "ParseMeteoDataNASASSEMonthlyAverage",
                comment,
                errorss,
                false);
        }

        // получение среднемесячных метеорологических данных NASA SSE
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult ParseMeteoDataNASASSEMonthlyAverage(bool? post)
        {
            string userid = User.Identity.GetUserId(),
                    user = User.Identity.Name;
            SystemLog.New("ParseMeteoDataNASASSEMonthlyAverage", true);

            Task t = new Task(ParseMeteoDataNASASSEMonthlyAverageTask);
            t.Start();

            ViewBag.Report = "Операция ParseMeteoDataNASASSEMonthlyAverage запущена.";
            return View();
        }

        // GET
        [Authorize(Roles = "Admin")]
        public ActionResult ParseMeteoDataNASAPOWERMonthlyAverage()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public void ParseMeteoDataNASAPOWERMonthlyAverageTask()
        {
            string userid = User.Identity.GetUserId(),
                    user = User.Identity.Name;

            List<string> errors = new List<string>();
            string comment = "";

            try
            {
                int count = 0;

                List<MeteoDataType> meteodatatypes = new List<MeteoDataType>();
                using (var db = new NpgsqlContext())
                {
                    meteodatatypes = db.MeteoDataTypes
                        .Where(m => m.MeteoDataSource.Code == Properties.Settings.Default.NASAPOWERCode
                            && m.MeteoDataPeriodicity.Code.ToLower().Contains(Properties.Settings.Default.Monthly.ToLower())
                            && m.MeteoDataPeriodicity.Code.ToLower().Contains(Properties.Settings.Default.Average.ToLower()))
                        .ToList();
                    db.Dispose();
                    GC.Collect();
                }

                List<int> meteodatatypeids = meteodatatypes.Select(m => m.Id).ToList();
                //MeteoData md = db.MeteoDatas.Where(m => meteodatatypeids.Contains(m.MeteoDataTypeId)).FirstOrDefault(); // uncomment
                MeteoData md = null;//delete

                if (md != null)
                {
                    comment = $"Добавлено {count.ToString()} среднемесячных метеорологических данных {Properties.Settings.Default.NASAPOWERCode}.";
                }
                else
                {
                    decimal longitude_min = Properties.Settings.Default.NASASSELongitudeMin,
                            longitude_max = Properties.Settings.Default.NASASSELongitudeMax,
                            latitude_min = Properties.Settings.Default.NASASSELatitudeMin,
                            latitude_max = Properties.Settings.Default.NASASSELatitudeMax;
                    string CurrentUserId = userid;
                    string path = "~/Upload/" + CurrentUserId;
                    if (!Directory.Exists(Server.MapPath(path)))
                    {
                        DirectoryInfo di = Directory.CreateDirectory(Server.MapPath(path));
                    }
                    string filenameimport = Path.Combine(Server.MapPath(path), $"{Properties.Settings.Default.NASAPOWERCode} - {Properties.Settings.Default.Monthly} - {Properties.Settings.Default.Average}.txt");
                    System.IO.File.Delete(filenameimport);
                    using (StreamWriter sw = System.IO.File.AppendText(filenameimport))
                    {
                        for (decimal latitude = latitude_min; latitude <= latitude_max; latitude += Properties.Settings.Default.NASASSECoordinatesStep)
                        {
                            for (decimal longitude = longitude_min; longitude <= longitude_max; longitude += Properties.Settings.Default.NASASSECoordinatesStep)
                            {
                                string url = "https://power.larc.nasa.gov/cgi-bin/sustain.cgi?email=sustain%40larc.nasa.gov&step=2&lat=" +
                                    latitude.ToString().Replace(",", ".") +
                                    "&lon=" +
                                    longitude.ToString().Replace(",", ".") +
                                    "&num=225129&submit=Submit&p=grid_id&veg=17&hgt=10&sitelev=";
                                HtmlAgilityPack.HtmlDocument html = new HtmlAgilityPack.HtmlDocument();
                                html.LoadHtml(new WebClient().DownloadString(url));

                                HtmlAgilityPack.HtmlNode root = html.DocumentNode;

                                string slat = "",
                                        slon = "";
                                foreach (HtmlAgilityPack.HtmlNode td in root.Descendants("td"))
                                {
                                    if (td.InnerText.IndexOf("Center") >= 0)
                                    {
                                        IEnumerable<HtmlAgilityPack.HtmlNode> bs = td.Descendants("b");
                                        slat = bs.First().InnerText;
                                        slon = bs.Last().InnerText;
                                        break;
                                    }
                                }

                                string current_table_name = "";
                                string group = "";
                                foreach (HtmlAgilityPack.HtmlNode node in root.Descendants())
                                {
                                    if (node.Name == "big")
                                    {
                                        if (node.FirstChild.FirstChild != null)
                                        {
                                            group = node.FirstChild.FirstChild.InnerText.Replace(":", "").Trim();
                                        }
                                    }
                                    if (node.Name == "a")
                                    {
                                        if (node.GetAttributeValue("name", "") != "")
                                        {
                                            current_table_name = HttpUtility.HtmlDecode(node.GetAttributeValue("name", "").Trim());
                                        }
                                    }
                                    if ((current_table_name != "") && (node.Name == "a"))
                                    {
                                        int current_tr_i = 0;
                                        foreach (HtmlAgilityPack.HtmlNode tr in node.Descendants("table").FirstOrDefault().Descendants("tr"))
                                        {
                                            current_tr_i++;
                                            if (current_tr_i > 1)
                                            {
                                                int current_td_i = 0;
                                                string row_name = "";
                                                foreach (HtmlAgilityPack.HtmlNode td in tr.Descendants("td"))
                                                {
                                                    current_td_i++;
                                                    if (current_td_i == 1)
                                                    {
                                                        row_name = HttpUtility.HtmlDecode(td.InnerText.Trim());
                                                    }
                                                    else
                                                    {
                                                        decimal out_decimal = 0;
                                                        int MeteoDataTypeId = meteodatatypes
                                                            .Where(m => m.Code == current_table_name && m.AdditionalEN == row_name && m.GroupEN == group)
                                                            .FirstOrDefault()
                                                            .Id;
                                                        string V = td.InnerText.Replace("*", "").Trim();
                                                        decimal? value = decimal.TryParse(V.Replace(".", ","), out out_decimal) ? Convert.ToDecimal(V.Replace(".", ",")) : (decimal?)null;
                                                        sw.WriteLine(
                                                            MeteoDataTypeId.ToString() + "\t" +
                                                            "\t" +
                                                            (current_td_i - 1).ToString() + "\t" +
                                                            "\t" +
                                                            Convert.ToDecimal(slon.Replace(".", ",")).ToString().Replace(",", ".") + "\t" +
                                                            Convert.ToDecimal(slat.Replace(".", ",")).ToString().Replace(",", ".") + "\t" +
                                                            value.ToString().Replace(",", ".")
                                                            );
                                                        count++;
                                                    }
                                                }
                                            }
                                        }
                                        current_table_name = "";
                                    }
                                }
                            }
                        }
                    }

                    string query = "COPY \"MeteoData\" (\"MeteoDataTypeId\", \"Year\", \"Month\", \"Day\", \"Longitude\", \"Latitude\", \"Value\") FROM '" + filenameimport + "' WITH NULL AS ''";
                    try
                    {
                        db.MeteoDatas.SqlQuery(query).SingleOrDefault();
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message != "The data reader is incompatible with the specified 'AtlasSolar.Models.MeteoData'. A member of the type, 'Id', does not have a corresponding column in the data reader with the same name.")
                        {
                            errors.Add(ex.Message);
                        }
                    }
                    if (errors.Count() == 0)
                    {
                        System.IO.File.Delete(filenameimport);
                        comment = $"Добавлено {count.ToString()} среднемесячных метеорологических данных {Properties.Settings.Default.NASAPOWERCode}.";
                    }
                }
            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
            }

            string errorss = errors.Count() == 0 ? null : string.Join("; ", errors.ToArray());
            SystemLog.New(
                userid,
                user,
                "ParseMeteoDataNASAPOWERMonthlyAverage",
                comment,
                errorss,
                false);
        }

        // получение среднемесячных метеорологических данных NASA POWER
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult ParseMeteoDataNASAPOWERMonthlyAverage(bool? post)
        {
            string userid = User.Identity.GetUserId(),
                    user = User.Identity.Name;
            SystemLog.New("ParseMeteoDataNASAPOWERMonthlyAverage", true);

            Task t = new Task(ParseMeteoDataNASAPOWERMonthlyAverageTask);
            t.Start();

            ViewBag.Report = "Операция ParseMeteoDataNASAPOWERMonthlyAverage запущена.";
            return View();
        }

        // GET
        [Authorize(Roles = "Admin")]
        public ActionResult ParseMeteoDataNASASSEDaily()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public void ParseMeteoDataNASASSEDailyTask()
        {
            string userid = User.Identity.GetUserId(),
                    user = User.Identity.Name;

            List<string> errors = new List<string>();
            string comment = "";

            try
            {
                int count = 0;

                List<MeteoDataType> meteodatatypes = new List<MeteoDataType>();
                using (var db = new NpgsqlContext())
                {
                    meteodatatypes = db.MeteoDataTypes
                        .Where(m => m.MeteoDataSource.Code == Properties.Settings.Default.NASASSECode
                            && m.MeteoDataPeriodicity.Code.ToLower().Contains(Properties.Settings.Default.Daily))
                        .ToList();
                    db.Dispose();
                    GC.Collect();
                }

                List<int> meteodatatypeids = meteodatatypes.Select(m => m.Id).ToList();
                MeteoData md = db.MeteoDatas.Where(m => meteodatatypeids.Contains(m.MeteoDataTypeId)).FirstOrDefault();
                if (md != null)
                {
                    comment = $"Добавлено {count.ToString()} ежедневных метеорологических данных {Properties.Settings.Default.NASASSECode}.";
                }
                else
                {
                    decimal longitude_min = Properties.Settings.Default.NASASSELongitudeMin,
                            longitude_max = Properties.Settings.Default.NASASSELongitudeMax,
                            latitude_min = Properties.Settings.Default.NASASSELatitudeMin,
                            latitude_max = Properties.Settings.Default.NASASSELatitudeMax;
                    string CurrentUserId = userid;
                    string path = "~/Upload/" + CurrentUserId;
                    if (!Directory.Exists(Server.MapPath(path)))
                    {
                        DirectoryInfo di = Directory.CreateDirectory(Server.MapPath(path));
                    }
                    string filenameimport = Path.Combine(Server.MapPath(path), $"{Properties.Settings.Default.NASASSECode} - {Properties.Settings.Default.Daily}.txt");
                    System.IO.File.Delete(filenameimport);
                    using (StreamWriter sw = System.IO.File.AppendText(filenameimport))
                    {
                        for (decimal latitude = latitude_min; latitude <= latitude_max; latitude += Properties.Settings.Default.NASASSECoordinatesStep)
                        {
                            for (decimal longitude = longitude_min; longitude <= longitude_max; longitude += Properties.Settings.Default.NASASSECoordinatesStep)
                            {
                                string url = "https://eosweb.larc.nasa.gov/cgi-bin/sse/daily.cgi?email=skip%40larc.nasa.gov&step=1&lat=" +
                                        latitude.ToString().Replace(",", ".") +
                                        "&lon=" +
                                        longitude.ToString().Replace(",", ".") +
                                        "&sitelev=&ms=1&ds=1&ys=" +
                                        Properties.Settings.Default.NASASSEYearStart.ToString().Replace(",", ".") +
                                        "&me=12&de=31&ye=" +
                                        Properties.Settings.Default.NASASSEYearFinish.ToString().Replace(",", ".") +
                                        "&p=swv_dwn&p=avg_kt&p=clr_sky&p=clr_dif&p=clr_dnr&p=clr_kt&p=lwv_dwn&p=toa_dwn&p=PS&p=TSKIN&p=T10M&p=T10MN&p=T10MX&p=Q10M&p=RH10M&p=DFP10M&submit=Submit&plot=swv_dwn";
                                HtmlAgilityPack.HtmlDocument html = new HtmlAgilityPack.HtmlDocument();
                                html.LoadHtml(new WebClient().DownloadString(url));
                                HtmlAgilityPack.HtmlNode root = html.DocumentNode;
                                string slat = "",
                                        slon = "";
                                foreach (HtmlAgilityPack.HtmlNode td in root.Descendants("td"))
                                {
                                    if (td.InnerText.IndexOf("Center") >= 0)
                                    {
                                        IEnumerable<HtmlAgilityPack.HtmlNode> bs = td.Descendants("b");
                                        slat = bs.First().InnerText;
                                        slon = bs.Last().InnerText;
                                        break;
                                    }
                                }
                                string file_url = "https://eosweb.larc.nasa.gov";
                                foreach (HtmlAgilityPack.HtmlNode node in root.Descendants())
                                {
                                    if ((node.Name == "a") && (node.InnerText == "Download a text file"))
                                    {
                                        file_url += node.GetAttributeValue("href", "");
                                        break;
                                    }
                                }
                                string file = (new WebClient()).DownloadString(file_url);
                                string[] lines = file.Split('\n');
                                List<string> columns = new List<string>();
                                foreach (string line in lines)
                                {
                                    if (line.Length == 0)
                                    {
                                        continue;
                                    }

                                    string s = line;
                                    while (s.Contains("  "))
                                    {
                                        s = s.Replace("  ", " ");
                                    }
                                    string[] linecolumns = s.Split(' ');

                                    if (columns.Count > 0)
                                    {
                                        for (int c = 3; c < columns.Count; c++)
                                        {
                                            sw.WriteLine(
                                                meteodatatypes.Where(m => m.Code == columns[c]).FirstOrDefault().Id.ToString() + "\t" +
                                                linecolumns[0] + "\t" +
                                                linecolumns[1] + "\t" +
                                                linecolumns[2] + "\t" +
                                                slon + "\t" +
                                                slat + "\t" +
                                                ((linecolumns[c] == "-") ? "" : linecolumns[c])
                                                );
                                            count++;
                                        }
                                    }

                                    if (line.Contains("YEAR MO DY"))
                                    {
                                        foreach (string linecolumn in linecolumns)
                                        {
                                            columns.Add(linecolumn);
                                        }
                                    }
                                }
                            }
                        }
                    }

                    string query = "COPY \"MeteoData\" (\"MeteoDataTypeId\", \"Year\", \"Month\", \"Day\", \"Longitude\", \"Latitude\", \"Value\") FROM '" + filenameimport + "' WITH NULL AS ''";
                    try
                    {
                        db.MeteoDatas.SqlQuery(query).SingleOrDefault();
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message != "The data reader is incompatible with the specified 'AtlasSolar.Models.MeteoData'. A member of the type, 'Id', does not have a corresponding column in the data reader with the same name.")
                        {
                            errors.Add(ex.Message);
                        }
                    }
                    if (errors.Count() == 0)
                    {
                        System.IO.File.Delete(filenameimport);
                        comment = $"Добавлено {count.ToString()} ежедневных метеорологических данных {Properties.Settings.Default.NASASSECode}.";
                    }
                }
            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
            }

            string errorss = errors.Count() == 0 ? null : string.Join("; ", errors.ToArray());
            SystemLog.New(
                userid,
                user,
                "ParseMeteoDataNASASSEDaily",
                comment,
                errorss,
                false);
        }

        // получение ежедневных метеорологических данных NASA SSE
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult ParseMeteoDataNASASSEDaily(bool? post)
        {
            string userid = User.Identity.GetUserId(),
                    user = User.Identity.Name;
            SystemLog.New("ParseMeteoDataNASASSEDaily", true);

            Task t = new Task(ParseMeteoDataNASASSEDailyTask);
            t.Start();

            ViewBag.Report = "Операция ParseMeteoDataNASASSEDaily запущена.";
            return View();
        }

        // GET
        [Authorize(Roles = "Admin")]
        public ActionResult ParseMeteoDataNASAPOWERDaily()
        {
            List<int> years = Enumerable.Range(Properties.Settings.Default.NASAPOWERYearStart, DateTime.Today.Year - Properties.Settings.Default.NASAPOWERYearStart).ToList();
            ViewBag.Year = new SelectList(years);
            List<int> months = Enumerable.Range(1, 12).ToList();
            ViewBag.Month = new SelectList(months);
            return View();
        }

        [Authorize(Roles = "Admin")]
        public void ParseMeteoDataNASAPOWERDailyTask(int Year, int Month)
        {
            string userid = User.Identity.GetUserId(),
                    user = User.Identity.Name;

            List<string> errors = new List<string>();
            string comment = "";

            try
            {
                int count = 0;

                List<MeteoDataType> meteodatatypes = new List<MeteoDataType>();
                using (var db = new NpgsqlContext())
                {
                    meteodatatypes = db.MeteoDataTypes
                        .Where(m => m.MeteoDataSource.Code == Properties.Settings.Default.NASAPOWERCode
                            && m.MeteoDataPeriodicity.Code.ToLower().Contains(Properties.Settings.Default.Daily))
                        .ToList();
                    db.Dispose();
                    GC.Collect();
                }

                List<int> meteodatatypeids = meteodatatypes.Select(m => m.Id).ToList();
                MeteoData md = db.MeteoDatas.Where(m => meteodatatypeids.Contains(m.MeteoDataTypeId) && m.Year == Year && m.Month == Month && m.Day == 1).FirstOrDefault();
                if (md != null)
                {
                    comment = $"Добавлено {count.ToString()} ежедневных метеорологических данных {Properties.Settings.Default.NASAPOWERCode}.";
                }
                else
                {
                    decimal longitude_min = Properties.Settings.Default.NASAPOWERLongitudeMin,
                            longitude_max = Properties.Settings.Default.NASAPOWERLongitudeMax,
                            latitude_min = Properties.Settings.Default.NASAPOWERLatitudeMin,
                            latitude_max = Properties.Settings.Default.NASAPOWERLatitudeMax;
                    string CurrentUserId = userid;
                    string path = "~/Upload/" + CurrentUserId;
                    if (!Directory.Exists(Server.MapPath(path)))
                    {
                        DirectoryInfo di = Directory.CreateDirectory(Server.MapPath(path));
                    }
                    string filenameimport = Path.Combine(Server.MapPath(path), $"{Properties.Settings.Default.NASAPOWERCode} - {Properties.Settings.Default.Daily} - {Year.ToString()} - {Month.ToString()}.txt");
                    System.IO.File.Delete(filenameimport);
                    using (StreamWriter sw = System.IO.File.AppendText(filenameimport))
                    {
                        for (decimal latitude = latitude_min; latitude <= latitude_max; latitude += Properties.Settings.Default.NASAPOWERCoordinatesStep)
                        {
                            for (decimal longitude = longitude_min; longitude <= longitude_max; longitude += Properties.Settings.Default.NASAPOWERCoordinatesStep)
                            {
                                int monthlastday = 31;
                                switch (Month)
                                {
                                    case 1:
                                        monthlastday = 31;
                                        break;
                                    case 2:
                                        if (Year % 4 == 0)
                                        {
                                            monthlastday = 29;
                                        }
                                        else
                                        {
                                            monthlastday = 28;
                                        }
                                        break;
                                    case 3:
                                        monthlastday = 31;
                                        break;
                                    case 4:
                                        monthlastday = 30;
                                        break;
                                    case 5:
                                        monthlastday = 31;
                                        break;
                                    case 6:
                                        monthlastday = 30;
                                        break;
                                    case 7:
                                        monthlastday = 31;
                                        break;
                                    case 8:
                                        monthlastday = 31;
                                        break;
                                    case 9:
                                        monthlastday = 30;
                                        break;
                                    case 10:
                                        monthlastday = 31;
                                        break;
                                    case 11:
                                        monthlastday = 30;
                                        break;
                                    case 12:
                                        monthlastday = 31;
                                        break;
                                }
                                string file_url = "https://power.larc.nasa.gov/cgi-bin/hirestimeser.cgi?email=hirestimeser%40larc.nasa.gov&step=1&lat=" +
                                    latitude.ToString().Replace(",", ".") +
                                    "&lon=" +
                                    longitude.ToString().Replace(",", ".") +
                                    "&ms=" +
                                    Month.ToString() +
                                    "&ds=1&ys=" +
                                    Year.ToString() +
                                    "&me=1&de=" +
                                    monthlastday.ToString() +
                                    "&ye=" +
                                    Year.ToString() +
                                    "&p=MHswv_dwn&p=MHlwv_dwn&p=MHtoa_dwn&p=MHclr_sky&p=MHPS&p=MHT2M&p=MHT2MN&p=MHT2MX&p=MHQV2M&p=MHRH2M&p=MHDFP2M&p=MHTS&p=MHWS10M&p=MHPRECTOT&submit=Submit";
                                string file = "";
                                bool ok = false;
                                while (!ok)
                                {
                                    try
                                    {
                                        file = (new WebClient()).DownloadString(file_url);
                                        ok = true;
                                    }
                                    catch (Exception ex)
                                    {

                                    }
                                }
                                string[] lines = file.Split('\n');
                                List<string> columns = new List<string>();
                                foreach (string line in lines)
                                {
                                    if (line.Contains("Completed"))
                                    {
                                        break;
                                    }

                                    if (line.Length == 0)
                                    {
                                        continue;
                                    }

                                    string s = line;
                                    while (s.Contains("  "))
                                    {
                                        s = s.Replace("  ", " ");
                                    }
                                    string[] linecolumns = s.Split(' ');

                                    if ((columns.Count > 0) && (linecolumns.Length == 17))
                                    {
                                        for (int c = 3; c < columns.Count; c++)
                                        {
                                            decimal out_decimal = 0;
                                            sw.WriteLine(
                                                meteodatatypes.Where(m => m.Code == columns[c]).FirstOrDefault().Id.ToString() + "\t" +
                                                linecolumns[0] + "\t" +
                                                linecolumns[1] + "\t" +
                                                linecolumns[2] + "\t" +
                                                longitude.ToString().Replace(",", ".") + "\t" +
                                                latitude.ToString().Replace(",", ".") + "\t" +
                                                (decimal.TryParse(linecolumns[c].Replace(".", ","), out out_decimal) ? Convert.ToDecimal(linecolumns[c].Replace(".", ",")) : (decimal?)null).ToString().Replace(",", ".")
                                                );
                                            count++;
                                        }
                                    }

                                    if (line.Contains("YEAR MO DY"))
                                    {
                                        foreach (string linecolumn in linecolumns)
                                        {
                                            columns.Add(linecolumn);
                                        }
                                    }
                                }
                            }
                        }
                    }

                    string query = "COPY \"MeteoData\" (\"MeteoDataTypeId\", \"Year\", \"Month\", \"Day\", \"Longitude\", \"Latitude\", \"Value\") FROM '" + filenameimport + "' WITH NULL AS ''";
                    try
                    {
                        db.MeteoDatas.SqlQuery(query).SingleOrDefault();
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message != "The data reader is incompatible with the specified 'AtlasSolar.Models.MeteoData'. A member of the type, 'Id', does not have a corresponding column in the data reader with the same name.")
                        {
                            errors.Add(ex.Message);
                        }
                    }
                    if (errors.Count() == 0)
                    {
                        System.IO.File.Delete(filenameimport);
                        comment = $"Добавлено {count.ToString()} ежедневных метеорологических данных {Properties.Settings.Default.NASAPOWERCode}.";
                        comment += $" Год: {Year.ToString()}, месяц {Month.ToString()}.";
                    }
                }
            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
            }

            string errorss = errors.Count() == 0 ? null : string.Join("; ", errors.ToArray());
            SystemLog.New(
                userid,
                user,
                "ParseMeteoDataNASAPOWERDaily",
                comment,
                errorss,
                false);
        }

        // получение ежедневных метеорологических данных NASA POWER
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult ParseMeteoDataNASAPOWERDaily(int Year, int Month)
        {
            string userid = User.Identity.GetUserId(),
                    user = User.Identity.Name;
            SystemLog.New("ParseMeteoDataNASAPOWERDaily", $"Год: {Year.ToString()}, месяц {Month.ToString()}.", null, true);

            Task t = new Task(() => ParseMeteoDataNASAPOWERDailyTask(Year, Month));
            t.Start();

            ViewBag.Report = "Операция ParseMeteoDataNASAPOWERDaily запущена.";
            return View();
        }

        // GET
        [Authorize(Roles = "Admin")]
        public ActionResult UploadSARAHE()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public void UploadSARAHETask(IEnumerable<HttpPostedFileBase> Files, string UserId, string User)
        {
            //string userid = User.Identity.GetUserId(),
            //        user = User.Identity.Name;
            string userid = UserId,
                    user = User;

            List<string> errors = new List<string>();
            string comment = "";

            try
            {
                Option o_longitude_min = db.Options.Where(o => o.Code == Properties.Settings.Default.SARAHELongitudeMinOption).FirstOrDefault();
                Option o_longitude_max = db.Options.Where(o => o.Code == Properties.Settings.Default.SARAHELongitudeMaxOption).FirstOrDefault();
                Option o_latitude_min = db.Options.Where(o => o.Code == Properties.Settings.Default.SARAHELatitudeMinOption).FirstOrDefault();
                Option o_latitude_max = db.Options.Where(o => o.Code == Properties.Settings.Default.SARAHELatitudeMaxOption).FirstOrDefault();

                int count = 0;

                List<MeteoDataType> meteodatatypesdaily = new List<MeteoDataType>();
                List<MeteoDataType> meteodatatypesmonthly = new List<MeteoDataType>();
                int meteodatasourceid = -1;
                List<MeteoDataPeriodicity> meteodataperiodicities = new List<MeteoDataPeriodicity>();
                List<MeteoDataType> meteodatatypes = new List<MeteoDataType>();
                using (var db = new NpgsqlContext())
                {
                    meteodatatypesdaily = db.MeteoDataTypes
                        .Where(m => m.MeteoDataSource.Code == Properties.Settings.Default.SARAHECode
                            && m.MeteoDataPeriodicity.Code.ToLower().Contains(Properties.Settings.Default.Daily))
                        .ToList();
                    meteodatatypesmonthly = db.MeteoDataTypes
                        .Where(m => m.MeteoDataSource.Code == Properties.Settings.Default.SARAHECode
                            && m.MeteoDataPeriodicity.Code.ToLower().Contains(Properties.Settings.Default.Monthly))
                        .ToList();
                    meteodataperiodicities = db.MeteoDataPeriodicities.ToList();
                    meteodatatypes = db.MeteoDataTypes.ToList();
                    //db.Dispose();
                    //GC.Collect();
                }

                string CurrentUserId = userid;
                string path = "~/Upload/" + CurrentUserId;

                //if (!Directory.Exists(Server.MapPath(path)))
                //{
                //    DirectoryInfo di = Directory.CreateDirectory(Server.MapPath(path));
                //}
                string filenameimport = Path.Combine(Server.MapPath(path), $"{Properties.Settings.Default.SARAHECode}.txt");
                System.IO.File.Delete(filenameimport);
                using (StreamWriter sw = System.IO.File.AppendText(filenameimport))
                {
                    string batfilename = Path.ChangeExtension(Path.Combine(Server.MapPath(path), DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss")), ".bat");
                    StreamWriter bat = new StreamWriter(batfilename);
                    foreach (HttpPostedFileBase file in Files)
                    {
                        //string path_filename = Path.Combine(Server.MapPath(path), Path.GetFileName(file.FileName));
                        //file.SaveAs(path_filename);
                        string filename = Path.GetFileNameWithoutExtension(file.FileName);
                        bat.WriteLine("ncdump -f c " + filename + ".nc > " + filename + ".txt");
                    }
                    bat.Close();

                    Process proc = new System.Diagnostics.Process();
                    proc.StartInfo.FileName = batfilename;
                    proc.StartInfo.Arguments = "/c start /wait " + Path.GetFileName(batfilename);
                    proc.StartInfo.WorkingDirectory = Path.GetDirectoryName(batfilename);
                    proc.Start();
                    proc.WaitForExit();

                    //string outs = "";
                    //var process = new Process();
                    //var startinfo = new ProcessStartInfo("cmd.exe", "/c start /wait " + Path.GetFileName(batfilename));
                    //startinfo.RedirectStandardOutput = true;
                    //startinfo.UseShellExecute = false;
                    //startinfo.FileName = batfilename;
                    //startinfo.WorkingDirectory = Path.GetDirectoryName(batfilename);
                    //process.StartInfo = startinfo;
                    //process.OutputDataReceived += (sender, args) => outs = args.Data; // do whatever processing you need to do in this handler
                    //process.Start();
                    //process.BeginOutputReadLine();
                    //process.WaitForExit();

                    //System.Diagnostics.Process proc = new System.Diagnostics.Process();
                    //proc.StartInfo.FileName = "C:\\test\\netcdf.bat";
                    //proc.StartInfo.WorkingDirectory = "C:\\test";
                    //proc.StartInfo.UseShellExecute = true;
                    //proc.Start();

                    //Process proc = new System.Diagnostics.Process();
                    //proc.StartInfo.FileName = "C:\\test\\netcdf.bat";
                    //proc.StartInfo.Arguments = "/c start /wait " + Path.GetFileName("C:\\test\\netcdf.bat");
                    //proc.StartInfo.WorkingDirectory = Path.GetDirectoryName("C:\\test\\netcdf.bat");
                    //proc.Start();
                    //proc.WaitForExit();

                    //string batDir = Path.GetDirectoryName(batfilename);
                    //var proc = new Process();
                    //proc.StartInfo.WorkingDirectory = batDir;
                    //proc.StartInfo.FileName = batfilename;
                    //proc.StartInfo.CreateNoWindow = true;
                    //proc.Start();
                    //proc.WaitForExit();



                    System.IO.File.Delete(batfilename);
                    // загрузка данных из файлов
                    foreach (HttpPostedFileBase file in Files)
                    {
                        bool break_file = false;

                        List<decimal> lats = new List<decimal>();
                        List<decimal> lons = new List<decimal>();
                        List<DateTime> times = new List<DateTime>();
                        string[] lines = System.IO.File.ReadAllLines(Path.ChangeExtension(Path.Combine(Server.MapPath(path), Path.GetFileName(file.FileName)), "txt"));

                        bool variables = false,
                            global_attributes = false,
                            data = false;

                        string meteodataperiodicitycode = "";
                        int meteodatatypeid = -1;
                        int meteodataperiodicityid = -1;
                        int meteodatatypescount = -1;

                        List<string> meteodatatypecodes = new List<string>();
                        List<string> meteodatatypenameENs = new List<string>();

                        foreach (string line in lines)
                        {
                            if (break_file)
                            {
                                break;
                            }
                            if (variables)
                            {
                                if (line.Contains("time, lat, lon"))
                                {
                                    meteodatatypecodes.Add(line.Split(' ')[1].Split('(')[0].Trim());
                                    meteodatatypenameENs.Add("");
                                }
                                if (line.Contains("long_name"))
                                {
                                    for (int i = 0; i < meteodatatypecodes.Count; i++)
                                    {
                                        if (line.Contains(meteodatatypecodes[i]))
                                        {
                                            string newname = line.Split('"')[1];
                                            newname = newname.First().ToString().ToUpper() + newname.Substring(1);
                                            meteodatatypenameENs[i] = newname;
                                            break;
                                        }
                                    }
                                }
                                for (int i = 0; i < meteodatatypecodes.Count; i++)
                                {
                                    if (line.Contains(meteodatatypecodes[i] + ":units") && meteodatatypecodes[i] != "")
                                    {
                                        if (line.Contains(meteodatatypecodes[i]))
                                        {
                                            meteodatatypenameENs[i] += " " + line.Split('"')[1];
                                            break;
                                        }
                                    }
                                }
                            }
                            if (global_attributes)
                            {
                                if (line.Contains(":time_coverage_resolution"))
                                {
                                    meteodataperiodicitycode = line.Split('"')[1];
                                    switch (line.Split('"')[1])
                                    {
                                        case "P1M":
                                            meteodataperiodicitycode = Properties.Settings.Default.Monthly;
                                            break;
                                        case "P1D":
                                            meteodataperiodicitycode = Properties.Settings.Default.Daily;
                                            break;
                                        case "PT1H":
                                            meteodataperiodicitycode = Properties.Settings.Default.Hourly;
                                            break;
                                        default:
                                            meteodataperiodicitycode = "";
                                            break;
                                    }

                                    meteodataperiodicityid = meteodataperiodicities
                                        .Where(m => m.Code.ToLower().Contains(meteodataperiodicitycode.ToLower()) && !m.Code.ToLower().Contains(Properties.Settings.Default.Average.ToLower()))
                                        .FirstOrDefault()
                                        .Id;
                                }
                            }
                            if (data)
                            {
                                if (meteodatasourceid < 0)
                                {
                                    using (var db = new NpgsqlContext())
                                    {
                                        meteodatasourceid = db.MeteoDataSources
                                            .Where(m => m.Code == Properties.Settings.Default.SARAHECode)
                                            .FirstOrDefault()
                                            .Id;
                                        for (int i = 0; i < meteodatatypecodes.Count; i++)
                                        {
                                            string meteodatatypecode = meteodatatypecodes[i];
                                            meteodatatypescount = db.MeteoDataTypes
                                                .Where(m => m.Code == meteodatatypecode && m.MeteoDataSourceId == meteodatasourceid && m.MeteoDataPeriodicityId == meteodataperiodicityid)
                                                .Count();
                                            if (meteodatatypescount == 0)
                                            {
                                                db.MeteoDataTypes.Add(new MeteoDataType()
                                                {
                                                    MeteoDataSourceId = meteodatasourceid,
                                                    MeteoDataPeriodicityId = meteodataperiodicityid,
                                                    Code = meteodatatypecodes[i],
                                                    NameEN = meteodatatypenameENs[i],
                                                    NameRU = meteodatatypenameENs[i],
                                                    NameKZ = meteodatatypenameENs[i]
                                                });
                                                db.SaveChanges();
                                            }
                                        }
                                        meteodatatypes = db.MeteoDataTypes.ToList();
                                        //db.Dispose();
                                        //GC.Collect();
                                    }
                                }

                                if (line.Contains("lon"))
                                {
                                    if (line.Contains("lon ="))
                                    {
                                        lons.Add(Convert.ToDecimal(line.Replace("lon =", "").Split(',')[0].Trim().Replace('.', ',')));
                                    }
                                    else
                                    {
                                        if (line.Contains(","))
                                        {
                                            lons.Add(Convert.ToDecimal(line.Split(',')[0].Trim().Replace('.', ',')));
                                        }
                                        if (line.Contains(";"))
                                        {
                                            lons.Add(Convert.ToDecimal(line.Split(';')[0].Trim().Replace('.', ',')));
                                        }
                                    }
                                }
                                if (line.Contains("lat"))
                                {
                                    if (line.Contains("lat ="))
                                    {
                                        lats.Add(Convert.ToDecimal(line.Replace("lat =", "").Split(',')[0].Trim().Replace('.', ',')));
                                    }
                                    else
                                    {
                                        if (line.Contains(","))
                                        {
                                            lats.Add(Convert.ToDecimal(line.Split(',')[0].Trim().Replace('.', ',')));
                                        }
                                        if (line.Contains(";"))
                                        {
                                            lats.Add(Convert.ToDecimal(line.Split(';')[0].Trim().Replace('.', ',')));
                                        }
                                    }
                                }
                                if (line.Contains("time"))
                                {
                                    if (line.Contains("time ="))
                                    {
                                        if (line.Contains(","))
                                        {
                                            times.Add(new DateTime(1983, 1, 1).AddHours(Convert.ToInt32(line.Replace("time =", "").Split(',')[0].Trim())));
                                        }
                                        if (line.Contains(";"))
                                        {
                                            times.Add(new DateTime(1983, 1, 1).AddHours(Convert.ToInt32(line.Replace("time =", "").Split(';')[0].Trim())));
                                        }
                                    }
                                    else
                                    {
                                        if (line.Contains(","))
                                        {
                                            times.Add(new DateTime(1983, 1, 1).AddHours(Convert.ToInt32(line.Split(',')[0].Trim())));
                                        }
                                        if (line.Contains(";"))
                                        {
                                            times.Add(new DateTime(1983, 1, 1).AddHours(Convert.ToInt32(line.Split(';')[0].Trim())));
                                        }
                                    }
                                }
                                for (int i = 0; i < meteodatatypecodes.Count; i++)
                                {
                                    if (line.Contains(meteodatatypecodes[i]) && !line.Contains("="))
                                    {
                                        meteodatatypeid = meteodatatypes
                                            .Where(m => m.Code == meteodatatypecodes[i] && m.MeteoDataSourceId == meteodatasourceid && m.MeteoDataPeriodicityId == meteodataperiodicityid)
                                            .FirstOrDefault()
                                            .Id;

                                        string p = line.Split('(')[1];
                                        p = p.Substring(0, p.Length - 1);
                                        string[] pi = p.Split(',');
                                        string v = line.Trim().Split(' ')[0];
                                        v = v.Substring(0, v.Length - 1);
                                        decimal value = Convert.ToDecimal(v);
                                        int? year = times[Convert.ToInt32(pi[0])].Year,
                                            month = times[Convert.ToInt32(pi[0])].Month,
                                            day = null;

                                        MeteoData md = new MeteoData();
                                        if (meteodataperiodicitycode.ToLower() == Properties.Settings.Default.Daily.ToLower())
                                        {
                                            day = times[Convert.ToInt32(pi[0])].Day;
                                            md = db.MeteoDatas
                                                .Where(m => m.MeteoDataTypeId == meteodatatypeid && m.Year == year && m.Month == month && m.Day == day)
                                                .FirstOrDefault();
                                        }
                                        else
                                        {
                                            md = db.MeteoDatas
                                                .Where(m => m.MeteoDataTypeId == meteodatatypeid && m.Year == year && m.Month == month)
                                                .FirstOrDefault();
                                        }

                                        if (md != null)
                                        {
                                            string path_filename_ = Path.Combine(Server.MapPath(path), Path.GetFileName(file.FileName));
                                            string filenameout_ = Path.Combine(Server.MapPath(path), Path.GetFileName(Path.ChangeExtension(Path.GetFileNameWithoutExtension(file.FileName) + "_out", "txt")));
                                            System.IO.File.Delete(path_filename_);
                                            System.IO.File.Delete(Path.ChangeExtension(path_filename_, "txt"));
                                            break_file = true;
                                            break;
                                        }

                                        sw.WriteLine(meteodatatypeid.ToString() + "\t" +
                                            year.ToString() + "\t" +
                                            month.ToString() + "\t" +
                                            (meteodataperiodicitycode == Properties.Settings.Default.Daily || meteodataperiodicitycode == Properties.Settings.Default.Hourly ? day : null).ToString() + "\t" +
                                            //(meteodataperiodicitycode == Properties.Settings.Default.Hourly ? (int?)times[Convert.ToInt32(pi[0])].Hour : null).ToString() + "\t" +
                                            lons[Convert.ToInt32(pi[2])].ToString().Replace(',', '.') + "\t" +
                                            lats[Convert.ToInt32(pi[1])].ToString().Replace(',', '.') + "\t" +
                                            value.ToString().Replace(',', '.')
                                            );
                                        count++;

                                        /////////////////////
                                        if (o_longitude_min == null)
                                        {
                                            o_longitude_min = new Option()
                                            {
                                                Code = Properties.Settings.Default.SARAHELongitudeMinOption,
                                                Value = lons[Convert.ToInt32(pi[2])].ToString().Replace(',', '.')
                                            };
                                        }
                                        else if (lons[Convert.ToInt32(pi[2])] < Convert.ToDecimal(o_longitude_min.Value.Replace('.', ',')))
                                        {
                                            o_longitude_min.Value = lons[Convert.ToInt32(pi[2])].ToString().Replace(',', '.');
                                        }
                                        ////////////////////////////////
                                        if (o_longitude_max == null)
                                        {
                                            o_longitude_max = new Option()
                                            {
                                                Code = Properties.Settings.Default.SARAHELongitudeMaxOption,
                                                Value = lons[Convert.ToInt32(pi[2])].ToString().Replace(',', '.')
                                            };
                                        }
                                        else if (lons[Convert.ToInt32(pi[2])] > Convert.ToDecimal(o_longitude_max.Value.Replace('.', ',')))
                                        {
                                            o_longitude_max.Value = lons[Convert.ToInt32(pi[2])].ToString().Replace(',', '.');
                                        }
                                        //////////////////////////////////
                                        if (o_latitude_min == null)
                                        {
                                            o_latitude_min = new Option()
                                            {
                                                Code = Properties.Settings.Default.SARAHELatitudeMinOption,
                                                Value = lats[Convert.ToInt32(pi[1])].ToString().Replace(',', '.')
                                            };
                                        }
                                        else if (lats[Convert.ToInt32(pi[1])] < Convert.ToDecimal(o_latitude_min.Value.Replace('.', ',')))
                                        {
                                            o_latitude_min.Value = lats[Convert.ToInt32(pi[1])].ToString().Replace(',', '.');
                                        }
                                        ////////////////////////////////
                                        if (o_latitude_max == null)
                                        {
                                            o_latitude_max = new Option()
                                            {
                                                Code = Properties.Settings.Default.SARAHELatitudeMaxOption,
                                                Value = lats[Convert.ToInt32(pi[1])].ToString().Replace(',', '.')
                                            };
                                        }
                                        else if (lats[Convert.ToInt32(pi[1])] > Convert.ToDecimal(o_latitude_max.Value.Replace('.', ',')))
                                        {
                                            o_latitude_max.Value = lats[Convert.ToInt32(pi[1])].ToString().Replace(',', '.');
                                        }
                                        ////////////////////////////
                                    }
                                }
                            }
                            if (line.Contains("variables"))
                            {
                                variables = true;
                                global_attributes = false;
                                data = false;
                            }
                            if (line.Contains("global attributes"))
                            {
                                variables = false;
                                global_attributes = true;
                                data = false;
                            }
                            if (line == "data:")
                            {
                                variables = false;
                                global_attributes = false;
                                data = true;
                            }
                        }
                        string path_filename = Path.Combine(Server.MapPath(path), Path.GetFileName(file.FileName));
                        string filenameout = Path.Combine(Server.MapPath(path), Path.GetFileName(Path.ChangeExtension(Path.GetFileNameWithoutExtension(file.FileName) + "_out", "txt")));
                        System.IO.File.Delete(path_filename);
                        System.IO.File.Delete(Path.ChangeExtension(path_filename, "txt"));
                    }
                }

                Option o_longitude_min_db = db.Options.Where(o => o.Code == Properties.Settings.Default.SARAHELongitudeMinOption).FirstOrDefault();
                Option o_longitude_max_db = db.Options.Where(o => o.Code == Properties.Settings.Default.SARAHELongitudeMaxOption).FirstOrDefault();
                Option o_latitude_min_db = db.Options.Where(o => o.Code == Properties.Settings.Default.SARAHELatitudeMinOption).FirstOrDefault();
                Option o_latitude_max_db = db.Options.Where(o => o.Code == Properties.Settings.Default.SARAHELatitudeMaxOption).FirstOrDefault();
                if (o_longitude_min_db == null)
                {
                    db.Options.Add(o_longitude_min);
                }
                else
                {
                    o_longitude_min_db.Value = o_longitude_min.Value;
                    db.Entry(o_longitude_min_db).State = EntityState.Modified;
                }
                if (o_longitude_max_db == null)
                {
                    db.Options.Add(o_longitude_max);
                }
                else
                {
                    o_longitude_max_db.Value = o_longitude_max.Value;
                    db.Entry(o_longitude_max_db).State = EntityState.Modified;
                }
                if (o_latitude_min_db == null)
                {
                    db.Options.Add(o_latitude_min);
                }
                else
                {
                    o_latitude_min_db.Value = o_latitude_min.Value;
                    db.Entry(o_latitude_min_db).State = EntityState.Modified;
                }
                if (o_latitude_max_db == null)
                {
                    db.Options.Add(o_latitude_max);
                }
                else
                {
                    o_latitude_max_db.Value = o_latitude_max.Value;
                    db.Entry(o_latitude_max_db).State = EntityState.Modified;
                }
                db.SaveChanges();

                string query = "COPY \"MeteoData\" (\"MeteoDataTypeId\", \"Year\", \"Month\", \"Day\", \"Longitude\", \"Latitude\", \"Value\") FROM '" + filenameimport + "' WITH NULL AS ''";
                try
                {
                    db.MeteoDatas.SqlQuery(query).SingleOrDefault();
                }
                catch (Exception ex)
                {
                    if (ex.Message != "The data reader is incompatible with the specified 'AtlasSolar.Models.MeteoData'. A member of the type, 'Id', does not have a corresponding column in the data reader with the same name.")
                    {
                        errors.Add(ex.Message);
                        System.IO.File.Delete(filenameimport);
                    }
                }
                if (errors.Count() == 0)
                {
                    System.IO.File.Delete(filenameimport);
                    comment = $"Добавлено {count.ToString()} метеорологических данных {Properties.Settings.Default.SARAHECode}.";
                }
            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
            }

            string errorss = errors.Count() == 0 ? null : string.Join("; ", errors.ToArray());
            SystemLog.New(
                userid,
                user,
                "UploadSARAHE",
                comment,
                errorss,
                false);
        }

        // загрузка данных SARAH-E
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult UploadSARAHE(IEnumerable<HttpPostedFileBase> Files)
        {
            //string batfilename = Path.ChangeExtension(Path.Combine(Server.MapPath("~/Upload/"), "bat"), ".bat");
            //Process proc = new System.Diagnostics.Process();
            //proc.StartInfo.FileName = batfilename;
            //proc.StartInfo.Arguments = "/c start /wait " + Path.GetFileName(batfilename);
            //proc.StartInfo.WorkingDirectory = Path.GetDirectoryName(batfilename);
            //proc.Start();
            //proc.WaitForExit();


            string userid = User.Identity.GetUserId(),
                    user = User.Identity.Name;
            SystemLog.New("UploadSARAHE", "", null, true);

            string CurrentUserId = userid;
            string path = "~/Upload/" + CurrentUserId;

            if (!Directory.Exists(Server.MapPath(path)))
            {
                DirectoryInfo di2 = Directory.CreateDirectory(Server.MapPath(path));
            }

            foreach (HttpPostedFileBase file in Files)
            {
                string path_filename = Path.Combine(Server.MapPath(path), Path.GetFileName(file.FileName));
                file.SaveAs(path_filename);
            }

            //// new
            //string filenameimport = Path.Combine(Server.MapPath(path), $"{Properties.Settings.Default.SARAHECode}.txt");
            //System.IO.File.Delete(filenameimport);
            //using (StreamWriter sw = System.IO.File.AppendText(filenameimport))
            //{
            //    string batfilename = Path.ChangeExtension(Path.Combine(Server.MapPath(path), DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss")), ".bat");
            //    StreamWriter bat = new StreamWriter(batfilename);
            //    foreach (HttpPostedFileBase file in Files)
            //    {
            //        string filename = Path.GetFileNameWithoutExtension(file.FileName);
            //        bat.WriteLine("ncdump -f c " + filename + ".nc > " + filename + ".txt");
            //    }
            //    bat.Close();

            //    Process proc = new System.Diagnostics.Process();
            //    proc.StartInfo.FileName = batfilename;
            //    proc.StartInfo.Arguments = "/c start /wait " + Path.GetFileName(batfilename);
            //    proc.StartInfo.WorkingDirectory = Path.GetDirectoryName(batfilename);
            //    proc.Start();
            //    proc.WaitForExit();

            //    System.IO.File.Delete(batfilename);
            //}

            Task t = new Task(() => UploadSARAHETask(Files, userid, user));
            t.Start();

            ViewBag.Report = "Операция UploadSARAHE запущена.";
            return View();
        }

        // GET
        [Authorize(Roles = "Admin")]
        public ActionResult UploadCLARA()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public void UploadCLARATask(IEnumerable<HttpPostedFileBase> Files, string UserId, string User)
        {
            //string userid = User.Identity.GetUserId(),
            //        user = User.Identity.Name;
            string userid = UserId,
                    user = User;

            List<string> errors = new List<string>();
            string comment = "";

            try
            {
                Option o_longitude_min = db.Options.Where(o => o.Code == Properties.Settings.Default.CLARALongitudeMinOption).FirstOrDefault();
                Option o_longitude_max = db.Options.Where(o => o.Code == Properties.Settings.Default.CLARALongitudeMaxOption).FirstOrDefault();
                Option o_latitude_min = db.Options.Where(o => o.Code == Properties.Settings.Default.CLARALatitudeMinOption).FirstOrDefault();
                Option o_latitude_max = db.Options.Where(o => o.Code == Properties.Settings.Default.CLARALatitudeMaxOption).FirstOrDefault();

                int count = 0;

                List<MeteoDataType> meteodatatypesdaily = new List<MeteoDataType>();
                List<MeteoDataType> meteodatatypesmonthly = new List<MeteoDataType>();
                int meteodatasourceid = -1;
                List<MeteoDataPeriodicity> meteodataperiodicities = new List<MeteoDataPeriodicity>();
                List<MeteoDataType> meteodatatypes = new List<MeteoDataType>();
                using (var db = new NpgsqlContext())
                {
                    meteodatatypesdaily = db.MeteoDataTypes
                        .Where(m => m.MeteoDataSource.Code == Properties.Settings.Default.CLARACode
                            && m.MeteoDataPeriodicity.Code.ToLower().Contains(Properties.Settings.Default.Daily))
                        .ToList();
                    meteodatatypesmonthly = db.MeteoDataTypes
                        .Where(m => m.MeteoDataSource.Code == Properties.Settings.Default.CLARACode
                            && m.MeteoDataPeriodicity.Code.ToLower().Contains(Properties.Settings.Default.Monthly))
                        .ToList();
                    meteodataperiodicities = db.MeteoDataPeriodicities.ToList();
                    meteodatatypes = db.MeteoDataTypes.ToList();
                    //db.Dispose();
                    //GC.Collect();
                }

                string CurrentUserId = userid;
                string path = "~/Upload/" + CurrentUserId;

                //if (!Directory.Exists(Server.MapPath(path)))
                //{
                //    DirectoryInfo di = Directory.CreateDirectory(Server.MapPath(path));
                //}
                string filenameimport = Path.Combine(Server.MapPath(path), $"{Properties.Settings.Default.CLARACode}.txt");
                System.IO.File.Delete(filenameimport);
                using (StreamWriter sw = System.IO.File.AppendText(filenameimport))
                {
                    string batfilename = Path.ChangeExtension(Path.Combine(Server.MapPath(path), DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss")), ".bat");
                    StreamWriter bat = new StreamWriter(batfilename);
                    foreach (HttpPostedFileBase file in Files)
                    {
                        //string path_filename = Path.Combine(Server.MapPath(path), Path.GetFileName(file.FileName));
                        //file.SaveAs(path_filename);
                        string filename = Path.GetFileNameWithoutExtension(file.FileName);
                        bat.WriteLine("ncdump -f c " + filename + ".nc > " + filename + ".txt");
                    }
                    bat.Close();

                    Process proc = new System.Diagnostics.Process();
                    proc.StartInfo.FileName = batfilename;
                    proc.StartInfo.Arguments = "/c start /wait " + Path.GetFileName(batfilename);
                    proc.StartInfo.WorkingDirectory = Path.GetDirectoryName(batfilename);
                    proc.Start();
                    proc.WaitForExit();

                    //string outs = "";
                    //var process = new Process();
                    //var startinfo = new ProcessStartInfo("cmd.exe", "/c start /wait " + Path.GetFileName(batfilename));
                    //startinfo.RedirectStandardOutput = true;
                    //startinfo.UseShellExecute = false;
                    //startinfo.FileName = batfilename;
                    //startinfo.WorkingDirectory = Path.GetDirectoryName(batfilename);
                    //process.StartInfo = startinfo;
                    //process.OutputDataReceived += (sender, args) => outs = args.Data; // do whatever processing you need to do in this handler
                    //process.Start();
                    //process.BeginOutputReadLine();
                    //process.WaitForExit();

                    //System.Diagnostics.Process proc = new System.Diagnostics.Process();
                    //proc.StartInfo.FileName = "C:\\test\\netcdf.bat";
                    //proc.StartInfo.WorkingDirectory = "C:\\test";
                    //proc.StartInfo.UseShellExecute = true;
                    //proc.Start();

                    //Process proc = new System.Diagnostics.Process();
                    //proc.StartInfo.FileName = "C:\\test\\netcdf.bat";
                    //proc.StartInfo.Arguments = "/c start /wait " + Path.GetFileName("C:\\test\\netcdf.bat");
                    //proc.StartInfo.WorkingDirectory = Path.GetDirectoryName("C:\\test\\netcdf.bat");
                    //proc.Start();
                    //proc.WaitForExit();

                    //string batDir = Path.GetDirectoryName(batfilename);
                    //var proc = new Process();
                    //proc.StartInfo.WorkingDirectory = batDir;
                    //proc.StartInfo.FileName = batfilename;
                    //proc.StartInfo.CreateNoWindow = true;
                    //proc.Start();
                    //proc.WaitForExit();



                    System.IO.File.Delete(batfilename);
                    // загрузка данных из файлов
                    foreach (HttpPostedFileBase file in Files)
                    {
                        bool break_file = false;

                        List<decimal> lats = new List<decimal>();
                        List<decimal> lons = new List<decimal>();
                        List<DateTime> times = new List<DateTime>();
                        string[] lines = System.IO.File.ReadAllLines(Path.ChangeExtension(Path.Combine(Server.MapPath(path), Path.GetFileName(file.FileName)), "txt"));

                        bool variables = false,
                            global_attributes = false,
                            data = false;

                        string meteodataperiodicitycode = "";
                        int meteodatatypeid = -1;
                        int meteodataperiodicityid = -1;
                        int meteodatatypescount = -1;

                        List<string> meteodatatypecodes = new List<string>();
                        List<string> meteodatatypenameENs = new List<string>();

                        foreach (string line in lines)
                        {
                            if (break_file)
                            {
                                break;
                            }
                            if (variables)
                            {
                                if (line.Contains("time, lat, lon"))
                                {
                                    meteodatatypecodes.Add(line.Split(' ')[1].Split('(')[0].Trim());
                                    meteodatatypenameENs.Add("");
                                }
                                if (line.Contains("long_name"))
                                {
                                    for (int i = 0; i < meteodatatypecodes.Count; i++)
                                    {
                                        //if (line.Contains(meteodatatypecodes[i]))
                                        if (meteodatatypecodes[i] == line.Split(':')[0].Trim())
                                        {
                                            string newname = line.Split('"')[1];
                                            newname = newname.First().ToString().ToUpper() + newname.Substring(1);
                                            meteodatatypenameENs[i] = newname;
                                            break;
                                        }
                                    }
                                }
                                for (int i = 0; i < meteodatatypecodes.Count; i++)
                                {
                                    if (line.Contains(meteodatatypecodes[i] + ":units") && meteodatatypecodes[i] != "")
                                    {
                                        if (line.Contains(meteodatatypecodes[i]))
                                        {
                                            if(line.Split('"')[1] != "1")
                                            {
                                                meteodatatypenameENs[i] += " " + line.Split('"')[1];
                                            }
                                            break;
                                        }
                                    }
                                }
                            }
                            if (global_attributes)
                            {
                                if (line.Contains(":time_coverage_resolution"))
                                {
                                    meteodataperiodicitycode = line.Split('"')[1];
                                    switch (line.Split('"')[1])
                                    {
                                        case "P1M":
                                            meteodataperiodicitycode = Properties.Settings.Default.Monthly;
                                            break;
                                        case "P1D":
                                            meteodataperiodicitycode = Properties.Settings.Default.Daily;
                                            break;
                                        case "PT1H":
                                            meteodataperiodicitycode = Properties.Settings.Default.Hourly;
                                            break;
                                        default:
                                            meteodataperiodicitycode = "";
                                            break;
                                    }

                                    meteodataperiodicityid = meteodataperiodicities
                                        .Where(m => m.Code.ToLower().Contains(meteodataperiodicitycode.ToLower()) && !m.Code.ToLower().Contains(Properties.Settings.Default.Average.ToLower()))
                                        .FirstOrDefault()
                                        .Id;
                                }
                            }
                            if (data)
                            {
                                if (meteodatasourceid < 0)
                                {
                                    using (var db = new NpgsqlContext())
                                    {
                                        meteodatasourceid = db.MeteoDataSources
                                            .Where(m => m.Code == Properties.Settings.Default.CLARACode)
                                            .FirstOrDefault()
                                            .Id;
                                        for (int i = 0; i < meteodatatypecodes.Count; i++)
                                        {
                                            string meteodatatypecode = meteodatatypecodes[i];
                                            meteodatatypescount = db.MeteoDataTypes
                                                .Where(m => m.Code == meteodatatypecode && m.MeteoDataSourceId == meteodatasourceid && m.MeteoDataPeriodicityId == meteodataperiodicityid)
                                                .Count();
                                            if (meteodatatypescount == 0)
                                            {
                                                db.MeteoDataTypes.Add(new MeteoDataType()
                                                {
                                                    MeteoDataSourceId = meteodatasourceid,
                                                    MeteoDataPeriodicityId = meteodataperiodicityid,
                                                    Code = meteodatatypecodes[i],
                                                    NameEN = meteodatatypenameENs[i],
                                                    NameRU = meteodatatypenameENs[i],
                                                    NameKZ = meteodatatypenameENs[i]
                                                });
                                                db.SaveChanges();
                                            }
                                        }
                                        meteodatatypes = db.MeteoDataTypes.ToList();
                                        //db.Dispose();
                                        //GC.Collect();
                                    }
                                }

                                if (line.Contains("lon"))
                                {
                                    if (line.Contains("lon ="))
                                    {
                                        lons.Add(Convert.ToDecimal(line.Replace("lon =", "").Split(',')[0].Trim().Replace('.', ',')));
                                    }
                                    else
                                    {
                                        if (line.Contains(","))
                                        {
                                            lons.Add(Convert.ToDecimal(line.Split(',')[0].Trim().Replace('.', ',')));
                                        }
                                        if (line.Contains(";"))
                                        {
                                            lons.Add(Convert.ToDecimal(line.Split(';')[0].Trim().Replace('.', ',')));
                                        }
                                    }
                                }
                                if (line.Contains("lat"))
                                {
                                    if (line.Contains("lat ="))
                                    {
                                        lats.Add(Convert.ToDecimal(line.Replace("lat =", "").Split(',')[0].Trim().Replace('.', ',')));
                                    }
                                    else
                                    {
                                        if (line.Contains(","))
                                        {
                                            lats.Add(Convert.ToDecimal(line.Split(',')[0].Trim().Replace('.', ',')));
                                        }
                                        if (line.Contains(";"))
                                        {
                                            lats.Add(Convert.ToDecimal(line.Split(';')[0].Trim().Replace('.', ',')));
                                        }
                                    }
                                }
                                if (line.Contains("time") && !line.Contains("bnds"))
                                {
                                    if (line.Contains("time ="))
                                    {
                                        if (line.Contains(","))
                                        {
                                            times.Add(new DateTime(1970, 1, 1).AddDays(Convert.ToInt32(line.Replace("time =", "").Split(';')[0].Trim())));
                                        }
                                        if (line.Contains(";"))
                                        {
                                            times.Add(new DateTime(1970, 1, 1).AddDays(Convert.ToInt32(line.Replace("time =", "").Split(';')[0].Trim())));
                                        }
                                    }
                                    else
                                    {
                                        if (line.Contains(","))
                                        {
                                            times.Add(new DateTime(1970, 1, 1).AddDays(Convert.ToInt32(line.Split(',')[0].Trim())));
                                        }
                                        if (line.Contains(";"))
                                        {
                                            times.Add(new DateTime(1970, 1, 1).AddDays(Convert.ToInt32(line.Split(';')[0].Trim())));
                                        }
                                    }
                                }
                                for (int i = 0; i < meteodatatypecodes.Count; i++)
                                {
                                    if (line.Contains(meteodatatypecodes[i]) && !line.Contains("="))
                                    if (line.Split('(')[0].Split('/')[2].Trim() == meteodatatypecodes[i])
                                    {
                                        meteodatatypeid = meteodatatypes
                                            .Where(m => m.Code == meteodatatypecodes[i] && m.MeteoDataSourceId == meteodatasourceid && m.MeteoDataPeriodicityId == meteodataperiodicityid)
                                            .FirstOrDefault()
                                            .Id;

                                        string p = line.Split('(')[1];
                                        p = p.Substring(0, p.Length - 1);
                                        string[] pi = p.Split(',');
                                        string v = line.Trim().Split(' ')[0];
                                        v = v.Substring(0, v.Length - 1);
                                        decimal? value = null;
                                        try
                                            {
                                                value = Convert.ToDecimal(v.Replace('.', ','));
                                            }
                                        catch
                                            {
                                            }
                                        //if (v != "_" && !v.Contains("N"))
                                        //{
                                        //    value = Convert.ToDecimal(v.Replace('.',','));
                                        //}
                                        int? year = times[Convert.ToInt32(pi[0])].Year,
                                            month = times[Convert.ToInt32(pi[0])].Month,
                                            day = null;

                                        MeteoData md = new MeteoData();
                                        if (meteodataperiodicitycode.ToLower() == Properties.Settings.Default.Daily.ToLower())
                                        {
                                            day = times[Convert.ToInt32(pi[0])].Day;
                                            md = db.MeteoDatas
                                                .Where(m => m.MeteoDataTypeId == meteodatatypeid && m.Year == year && m.Month == month && m.Day == day)
                                                .FirstOrDefault();
                                        }
                                        else
                                        {
                                            md = db.MeteoDatas
                                                .Where(m => m.MeteoDataTypeId == meteodatatypeid && m.Year == year && m.Month == month)
                                                .FirstOrDefault();
                                        }

                                        if (md != null)
                                        {
                                            string path_filename_ = Path.Combine(Server.MapPath(path), Path.GetFileName(file.FileName));
                                            string filenameout_ = Path.Combine(Server.MapPath(path), Path.GetFileName(Path.ChangeExtension(Path.GetFileNameWithoutExtension(file.FileName) + "_out", "txt")));
                                            System.IO.File.Delete(path_filename_);
                                            System.IO.File.Delete(Path.ChangeExtension(path_filename_, "txt"));
                                            break_file = true;
                                            break;
                                        }

                                        sw.WriteLine(meteodatatypeid.ToString() + "\t" +
                                            year.ToString() + "\t" +
                                            month.ToString() + "\t" +
                                            (meteodataperiodicitycode == Properties.Settings.Default.Daily || meteodataperiodicitycode == Properties.Settings.Default.Hourly ? day : null).ToString() + "\t" +
                                            //(meteodataperiodicitycode == Properties.Settings.Default.Hourly ? (int?)times[Convert.ToInt32(pi[0])].Hour : null).ToString() + "\t" +
                                            lons[Convert.ToInt32(pi[2])].ToString().Replace(',', '.') + "\t" +
                                            lats[Convert.ToInt32(pi[1])].ToString().Replace(',', '.') + "\t" +
                                            value.ToString().Replace(',', '.')
                                            );
                                        count++;

                                        /////////////////////
                                        if (o_longitude_min == null)
                                        {
                                            o_longitude_min = new Option()
                                            {
                                                Code = Properties.Settings.Default.CLARALongitudeMinOption,
                                                Value = lons[Convert.ToInt32(pi[2])].ToString().Replace(',', '.')
                                            };
                                        }
                                        else if (lons[Convert.ToInt32(pi[2])] < Convert.ToDecimal(o_longitude_min.Value.Replace('.', ',')))
                                        {
                                            o_longitude_min.Value = lons[Convert.ToInt32(pi[2])].ToString().Replace(',', '.');
                                        }
                                        ////////////////////////////////
                                        if (o_longitude_max == null)
                                        {
                                            o_longitude_max = new Option()
                                            {
                                                Code = Properties.Settings.Default.CLARALongitudeMaxOption,
                                                Value = lons[Convert.ToInt32(pi[2])].ToString().Replace(',', '.')
                                            };
                                        }
                                        else if (lons[Convert.ToInt32(pi[2])] > Convert.ToDecimal(o_longitude_max.Value.Replace('.', ',')))
                                        {
                                            o_longitude_max.Value = lons[Convert.ToInt32(pi[2])].ToString().Replace(',', '.');
                                        }
                                        //////////////////////////////////
                                        if (o_latitude_min == null)
                                        {
                                            o_latitude_min = new Option()
                                            {
                                                Code = Properties.Settings.Default.CLARALatitudeMinOption,
                                                Value = lats[Convert.ToInt32(pi[1])].ToString().Replace(',', '.')
                                            };
                                        }
                                        else if (lats[Convert.ToInt32(pi[1])] < Convert.ToDecimal(o_latitude_min.Value.Replace('.', ',')))
                                        {
                                            o_latitude_min.Value = lats[Convert.ToInt32(pi[1])].ToString().Replace(',', '.');
                                        }
                                        ////////////////////////////////
                                        if (o_latitude_max == null)
                                        {
                                            o_latitude_max = new Option()
                                            {
                                                Code = Properties.Settings.Default.CLARALatitudeMaxOption,
                                                Value = lats[Convert.ToInt32(pi[1])].ToString().Replace(',', '.')
                                            };
                                        }
                                        else if (lats[Convert.ToInt32(pi[1])] > Convert.ToDecimal(o_latitude_max.Value.Replace('.', ',')))
                                        {
                                            o_latitude_max.Value = lats[Convert.ToInt32(pi[1])].ToString().Replace(',', '.');
                                        }
                                        ////////////////////////////
                                    }
                                }
                            }
                            if (line.Contains("variables"))
                            {
                                variables = true;
                                global_attributes = false;
                                data = false;
                            }
                            if (line.Contains("global attributes"))
                            {
                                variables = false;
                                global_attributes = true;
                                data = false;
                            }
                            if (line == "data:")
                            {
                                variables = false;
                                global_attributes = false;
                                data = true;
                            }
                        }
                        string path_filename = Path.Combine(Server.MapPath(path), Path.GetFileName(file.FileName));
                        string filenameout = Path.Combine(Server.MapPath(path), Path.GetFileName(Path.ChangeExtension(Path.GetFileNameWithoutExtension(file.FileName) + "_out", "txt")));
                        System.IO.File.Delete(path_filename);
                        System.IO.File.Delete(Path.ChangeExtension(path_filename, "txt"));
                    }
                }

                Option o_longitude_min_db = db.Options.Where(o => o.Code == Properties.Settings.Default.CLARALongitudeMinOption).FirstOrDefault();
                Option o_longitude_max_db = db.Options.Where(o => o.Code == Properties.Settings.Default.CLARALongitudeMaxOption).FirstOrDefault();
                Option o_latitude_min_db = db.Options.Where(o => o.Code == Properties.Settings.Default.CLARALatitudeMinOption).FirstOrDefault();
                Option o_latitude_max_db = db.Options.Where(o => o.Code == Properties.Settings.Default.CLARALatitudeMaxOption).FirstOrDefault();
                if (o_longitude_min_db == null)
                {
                    db.Options.Add(o_longitude_min);
                }
                else
                {
                    o_longitude_min_db.Value = o_longitude_min.Value;
                    db.Entry(o_longitude_min_db).State = EntityState.Modified;
                }
                if (o_longitude_max_db == null)
                {
                    db.Options.Add(o_longitude_max);
                }
                else
                {
                    o_longitude_max_db.Value = o_longitude_max.Value;
                    db.Entry(o_longitude_max_db).State = EntityState.Modified;
                }
                if (o_latitude_min_db == null)
                {
                    db.Options.Add(o_latitude_min);
                }
                else
                {
                    o_latitude_min_db.Value = o_latitude_min.Value;
                    db.Entry(o_latitude_min_db).State = EntityState.Modified;
                }
                if (o_latitude_max_db == null)
                {
                    db.Options.Add(o_latitude_max);
                }
                else
                {
                    o_latitude_max_db.Value = o_latitude_max.Value;
                    db.Entry(o_latitude_max_db).State = EntityState.Modified;
                }
                db.SaveChanges();

                string query = "COPY \"MeteoData\" (\"MeteoDataTypeId\", \"Year\", \"Month\", \"Day\", \"Longitude\", \"Latitude\", \"Value\") FROM '" + filenameimport + "' WITH NULL AS ''";
                try
                {
                    db.MeteoDatas.SqlQuery(query).SingleOrDefault();
                }
                catch (Exception ex)
                {
                    if (ex.Message != "The data reader is incompatible with the specified 'AtlasSolar.Models.MeteoData'. A member of the type, 'Id', does not have a corresponding column in the data reader with the same name.")
                    {
                        errors.Add(ex.Message);
                        System.IO.File.Delete(filenameimport);
                    }
                }
                if (errors.Count() == 0)
                {
                    // Uncomment!!!
                    System.IO.File.Delete(filenameimport);
                    comment = $"Добавлено {count.ToString()} метеорологических данных {Properties.Settings.Default.CLARACode}.";
                }
            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
            }

            string errorss = errors.Count() == 0 ? null : string.Join("; ", errors.ToArray());
            SystemLog.New(
                userid,
                user,
                "UploadCLARA",
                comment,
                errorss,
                false);
        }

        // загрузка данных CLARA
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult UploadCLARA(IEnumerable<HttpPostedFileBase> Files)
        {

            string userid = User.Identity.GetUserId(),
                    user = User.Identity.Name;
            SystemLog.New("UploadCLARA", "", null, true);

            string CurrentUserId = userid;
            string path = "~/Upload/" + CurrentUserId;

            if (!Directory.Exists(Server.MapPath(path)))
            {
                DirectoryInfo di2 = Directory.CreateDirectory(Server.MapPath(path));
            }

            foreach (HttpPostedFileBase file in Files)
            {
                string path_filename = Path.Combine(Server.MapPath(path), Path.GetFileName(file.FileName));
                file.SaveAs(path_filename);
            }

            Task t = new Task(() => UploadCLARATask(Files, userid, user));
            t.Start();

            ViewBag.Report = "Операция UploadCLARA запущена.";
            return View();
        }

        //// GET
        //[Authorize(Roles = "Admin")]
        //public ActionResult UploadCLARA()
        //{
        //    return View();
        //}

        //// загрузка данных CLARA
        //[HttpPost]
        //[Authorize(Roles = "Admin")]
        //public ActionResult UploadCLARA(IEnumerable<HttpPostedFileBase> Files)
        //{
        //    string userid = User.Identity.GetUserId(),
        //            user = User.Identity.Name;
        //    SystemLog.New("UploadSARAHE", "", null, true);

        //    string CurrentUserId = userid;
        //    string path = "~/Upload/" + CurrentUserId;

        //    if (!Directory.Exists(Server.MapPath(path)))
        //    {
        //        DirectoryInfo di2 = Directory.CreateDirectory(Server.MapPath(path));
        //    }

        //    foreach (HttpPostedFileBase file in Files)
        //    {
        //        string path_filename = Path.Combine(Server.MapPath(path), Path.GetFileName(file.FileName));
        //        file.SaveAs(path_filename);
        //    }

        //    Task t = new Task(() => UploadSARAHETask(Files, userid, user));
        //    t.Start();

        //    ViewBag.Report = "Операция UploadSARAHE запущена.";
        //    return View();
        //}

        // формирование среднемесячных метеоданных
        [Authorize(Roles = "Admin")]
        public ActionResult Average()
        {
            IList<MeteoDataPeriodicity> meteodataperiodicities = db.MeteoDataPeriodicities
                .Where(m => !m.Code.Contains(Properties.Settings.Default.Average.ToLower()))
                .ToList();
            meteodataperiodicities = meteodataperiodicities
                .OrderBy(m => m.Name)
                .ToList();
            IList<MeteoDataSource> meteodatasources = db.MeteoDataSources
                .Where(m => true)
                .ToList();
            meteodatasources = meteodatasources
                .OrderBy(m => m.Name)
                .ToList();
            ViewBag.MeteoDataPeriodicities = new SelectList(meteodataperiodicities, "Id", "Name");
            ViewBag.MeteoDataSources = new SelectList(meteodatasources, "Id", "Name");

            if (Session["MeteoDataPeriodicityIdErase"] != null)
            {
                if (!(bool)Session["MeteoDataPeriodicityIdErase"])
                {
                    ViewBag.MeteoDataPeriodicities = new SelectList(meteodataperiodicities, "Id", "Name", (int?)Session["MeteoDataPeriodicityId"]);
                    ViewBag.MeteoDataSources = new SelectList(meteodatasources, "Id", "Name", (int?)Session["MeteoDataSourceId"]);
                }
                else
                {
                    Session["MeteoDataPeriodicityId"] = null;
                    Session["MeteoDataSourceId"] = null;
                }
            }
            Session["MeteoDataPeriodicityIdErase"] = true;

            if (Session["MeteoDataPeriodicityId"] != null)
            {
                int? MeteoDataPeriodicityId = (int?)Session["MeteoDataPeriodicityId"];
                int? MeteoDataSourceId = (int?)Session["MeteoDataSourceId"];
                IList<MeteoDataType> meteodatapetypes = db.MeteoDataTypes
                    .Where(m => (m.MeteoDataPeriodicityId == MeteoDataPeriodicityId || MeteoDataPeriodicityId == null) && (m.MeteoDataSourceId == MeteoDataSourceId || MeteoDataSourceId == null))
                    .ToList();
                meteodatapetypes = meteodatapetypes
                    .OrderBy(m => m.CodeName)
                    .ToList();
                ViewBag.MeteoDataTypes = new SelectList(meteodatapetypes, "Id", "CodeName");
            }

            IList<MeteoDataPeriodicity> meteodataperiodicitiesto = db.MeteoDataPeriodicities
                .Where(m => m.Code.Contains(Properties.Settings.Default.Average.ToLower()))
                .ToList();
            meteodataperiodicitiesto = meteodataperiodicitiesto
                .OrderBy(m => m.Name)
                .ToList();
            ViewBag.MeteoDataPeriodicitiesTo = new SelectList(meteodataperiodicitiesto, "Id", "Name");
            return View();
        }

        [Authorize(Roles = "Admin")]
        public void AverageTask(int? MeteoDataPeriodicityId, int? MeteoDataPeriodicityToId, int? MeteoDataSourceId, int? MeteoDataTypeId, string UserId, string User)
        {
            string userid = UserId,
                    user = User;
            List<string> errors = new List<string>();
            string comment = "";
            try
            {
                int count = 0;
                int count_edit = 0;
                MeteoDataType meteodatatype = db.MeteoDataTypes
                    .Where(m => m.Id == MeteoDataTypeId)
                    .FirstOrDefault();
                MeteoDataPeriodicity meteodataperoidicity = db.MeteoDataPeriodicities
                    .Where(m => m.Id == meteodatatype.MeteoDataPeriodicityId)
                    .FirstOrDefault();
                MeteoDataPeriodicity meteodataperoidicityto = db.MeteoDataPeriodicities
                    .Where(m => m.Id == MeteoDataPeriodicityToId)
                    .FirstOrDefault();
                MeteoDataSource meteodatasource = db.MeteoDataSources
                    .Where(m => m.Id == meteodatatype.MeteoDataSourceId)
                    .FirstOrDefault();
                //----------------------------------------------------------------------------------------------------------------------------------------
                // ежесуточные или ежемесячные в среднемесячные
                if ((meteodataperoidicity.Code.ToLower().Contains(Properties.Settings.Default.Daily.ToLower()) || meteodataperoidicity.Code.ToLower().Contains(Properties.Settings.Default.Monthly.ToLower())) &&
                    !meteodataperoidicity.Code.ToLower().Contains(Properties.Settings.Default.Average.ToLower()) &&
                    meteodataperoidicityto.Code.ToLower().Contains(Properties.Settings.Default.Monthly.ToLower()) &&
                    meteodataperoidicityto.Code.ToLower().Contains(Properties.Settings.Default.Average.ToLower()))
                {
                    SystemLog.New(
                        userid,
                        user,
                        "Average",
                        $"{meteodatasource.Name} {meteodataperoidicity.Name} {meteodatatype.Name} ({meteodatatype.Code}) -> {meteodataperoidicityto.Name}",
                        null,
                        true);
                    // добавить новый тип метеоданных, если нет
                    using (var dblocal = new NpgsqlContext())
                    {
                        int ismeteodatatypeto = dblocal.MeteoDataTypes
                                .Where(m => m.MeteoDataPeriodicityId == meteodataperoidicityto.Id && m.MeteoDataSourceId == meteodatatype.MeteoDataSourceId && m.Code == meteodatatype.Code)
                                .Count();
                        if (ismeteodatatypeto == 0)
                        {
                            MeteoDataType meteodatatypetonew = new MeteoDataType()
                            {
                                AdditionalEN = meteodatatype.AdditionalEN,
                                GroupEN = meteodatatype.GroupEN,
                                GroupKZ = meteodatatype.GroupKZ,
                                GroupRU = meteodatatype.GroupRU,
                                Code = meteodatatype.Code,
                                MeteoDataSourceId = meteodatatype.MeteoDataSourceId,
                                NameEN = meteodatatype.NameEN,
                                NameKZ = meteodatatype.NameKZ,
                                NameRU = meteodatatype.NameRU,
                                MeteoDataPeriodicityId = meteodataperoidicityto.Id
                            };
                            dblocal.MeteoDataTypes.Add(meteodatatypetonew);
                            dblocal.SaveChanges();
                            comment += $"Добавлен 1 метеорологический параметр {meteodatasource.Name} {meteodataperoidicityto.Name} {meteodatatypetonew.Name} ({meteodatatypetonew.Code}). ";
                        }
                        //dblocal.Dispose();
                        //GC.Collect();
                    }
                    MeteoDataType meteodatatypeto = db.MeteoDataTypes
                        .Where(m => m.MeteoDataPeriodicityId == MeteoDataPeriodicityToId && m.MeteoDataSourceId == meteodatasource.Id)
                        .FirstOrDefault();
                    string path = "~/Upload/" + userid;
                    if (!Directory.Exists(Server.MapPath(path)))
                    {
                        DirectoryInfo di = Directory.CreateDirectory(Server.MapPath(path));
                    }
                    decimal longitude_min = 0,
                        longitude_max = 0,
                        latitude_min = 0,
                        latitude_max = 0,
                        step = 0;
                    if (meteodatasource.Code == Properties.Settings.Default.NASASSECode)
                    {
                        longitude_min = Properties.Settings.Default.NASASSELongitudeMin;
                        longitude_max = Properties.Settings.Default.NASASSELongitudeMax;
                        latitude_min = Properties.Settings.Default.NASASSELatitudeMin;
                        latitude_max = Properties.Settings.Default.NASASSELatitudeMax;
                        step = Properties.Settings.Default.NASASSECoordinatesStep;
                    }
                    if (meteodatasource.Code == Properties.Settings.Default.NASAPOWERCode)
                    {
                        longitude_min = Properties.Settings.Default.NASAPOWERLongitudeMin;
                        longitude_max = Properties.Settings.Default.NASAPOWERLongitudeMax;
                        latitude_min = Properties.Settings.Default.NASAPOWERLatitudeMin;
                        latitude_max = Properties.Settings.Default.NASAPOWERLatitudeMax;
                        step = Properties.Settings.Default.NASAPOWERCoordinatesStep;
                    }
                    if (meteodatasource.Code == Properties.Settings.Default.SARAHECode)
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
                    if (meteodatasource.Code == Properties.Settings.Default.CLARACode)
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
                    string filenameout = Path.Combine(Server.MapPath(path), $"Average {DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss")}.txt");
                    using (StreamWriter sw = System.IO.File.AppendText(filenameout))
                    {
                        for (decimal latitude = latitude_min; latitude <= latitude_max; latitude += step)
                        {
                            for (decimal longitude = longitude_min; longitude <= longitude_max; longitude += step)
                            {
                                using (var dblocal = new NpgsqlContext())
                                {
                                    dblocal.Configuration.AutoDetectChangesEnabled = false;
                                    List<MeteoData> meteodatasto = dblocal.MeteoDatas
                                        .Where(m => m.Latitude == latitude && m.Longitude == longitude && m.MeteoDataTypeId == meteodatatypeto.Id)
                                        .ToList();
                                    List<MeteoData> meteodatas = dblocal.MeteoDatas
                                        .Where(m => m.Latitude == latitude && m.Longitude == longitude && m.MeteoDataTypeId == meteodatatype.Id)
                                        .ToList();
                                    if (meteodatasto.Count() == 0)
                                    {
                                        for (int month = 1; month <= 12; month++)
                                        {
                                            List<MeteoData> meteodatasnew = meteodatas.
                                                Where(m => m.Month == month)
                                                .ToList();
                                            if (meteodatasnew.Count() > 0)
                                            {
                                                sw.WriteLine(meteodatatypeto.Id.ToString() + "\t" +
                                                    "\t" +
                                                    month.ToString() + "\t" +
                                                    "\t" +
                                                    longitude.ToString().Replace(',', '.') + "\t" +
                                                    latitude.ToString().Replace(',', '.') + "\t" +
                                                    meteodatasnew.Average(m => m.Value).ToString().Replace(',', '.')
                                                    );
                                                count++;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        for (int month = 1; month <= 12; month++)
                                        {
                                            List<MeteoData> meteodatasnew = meteodatas.
                                                Where(m => m.Month == month)
                                                .ToList();
                                            MeteoData mdmodified = meteodatasto.Where(m => m.Month == month).FirstOrDefault();
                                            if (mdmodified != null)
                                            {
                                                mdmodified.Value = meteodatasnew.Average(m => m.Value);
                                                dblocal.Entry(mdmodified).State = EntityState.Modified;
                                                count_edit++;
                                            }
                                        }
                                        dblocal.SaveChanges();
                                    }
                                    dblocal.Dispose();
                                    GC.Collect();
                                }
                            }
                        }
                    }
                    string query = "COPY \"MeteoData\" (\"MeteoDataTypeId\", \"Year\", \"Month\", \"Day\", \"Longitude\", \"Latitude\", \"Value\") FROM '" + filenameout + "' WITH NULL AS ''";
                    try
                    {
                        db.MeteoDatas.SqlQuery(query).SingleOrDefault();
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message != "The data reader is incompatible with the specified 'AtlasSolar.Models.MeteoData'. A member of the type, 'Id', does not have a corresponding column in the data reader with the same name.")
                        {
                            errors.Add(ex.Message);
                            System.IO.File.Delete(filenameout);
                        }
                    }
                    System.IO.File.Delete(filenameout);
                    comment += $"Добавлено {count.ToString()} метеорологических данных {meteodatasource.Code} {meteodataperoidicityto.Name} {meteodatatype.Code}. ";
                    comment += $"Изменено {count_edit.ToString()} метеорологических данных {meteodatasource.Code} {meteodataperoidicityto.Name} {meteodatatype.Code}. ";
                }
                //----------------------------------------------------------------------------------------------------------------------------------------
            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
            }
            string errorss = errors.Count() == 0 ? null : string.Join("; ", errors.ToArray());
            SystemLog.New(
                userid,
                user,
                "Average",
                comment,
                errorss,
                false);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Average(int? MeteoDataPeriodicityId, int? MeteoDataPeriodicityToId, int? MeteoDataSourceId, int? MeteoDataTypeId, string Action)
        {
            if (Action == "Change")
            {
                Session["MeteoDataPeriodicityId"] = MeteoDataPeriodicityId;
                Session["MeteoDataSourceId"] = MeteoDataSourceId;
                Session["MeteoDataPeriodicityIdErase"] = false;
                return RedirectToAction("Average");
            }
            Session["MeteoDataPeriodicityIdErase"] = true;
            if (MeteoDataTypeId != null && MeteoDataPeriodicityToId != null)
            {
                string userid = User.Identity.GetUserId(),
                        user = User.Identity.Name;
                //SystemLog.New("UploadSARAHE", "", null, true);

                Task t = new Task(() => AverageTask(MeteoDataPeriodicityId, MeteoDataPeriodicityToId, MeteoDataSourceId, MeteoDataTypeId, userid, user));
                t.Start();

                ViewBag.Report = "Операция Average запущена.";
                return View();
            }
            else
            {
                ViewBag.Report = "Выберите все необходимые данные!";
            }
            return View();
        }

        //[Authorize(Roles = "Admin")]
        //// delete
        //public ActionResult UploadNewMeteoDataTypes()
        //{
        //    return View();
        //}

        //[HttpPost]
        //[Authorize(Roles = "Admin")]
        //// удалить
        //public ActionResult UploadNewMeteoDataTypes(IEnumerable<HttpPostedFileBase> Files)
        //{
        //    string userid = User.Identity.GetUserId(),
        //            user = User.Identity.Name;
        //    string path = "~/Upload/" + userid;
        //    int count_deleted = 0,
        //            count_modified = 0;
        //    if (!Directory.Exists(Server.MapPath(path)))
        //    {
        //        DirectoryInfo di2 = Directory.CreateDirectory(Server.MapPath(path));
        //    }
        //    foreach (HttpPostedFileBase file in Files)
        //    {
        //        string path_filename = Path.Combine(Server.MapPath(path), Path.GetFileName(file.FileName));
        //        file.SaveAs(path_filename);


        //        using (StreamReader reader = new StreamReader(path_filename))
        //        {
        //            string line = "";

        //            while ((line = reader.ReadLine()) != null)
        //            {
        //                string[] strs = line.Split('\t');
        //                if (strs[0] != "Id")
        //                {
        //                    int id = Convert.ToInt32(strs[0]);
        //                    MeteoDataType mdt = db.MeteoDataTypes.FirstOrDefault(m => m.Id == id);
        //                    if (strs[16] == "delete")
        //                    {
        //                        db.MeteoDataTypes.Remove(mdt);
        //                        count_deleted++;
        //                    }
        //                    else
        //                    {
        //                        mdt.GroupKZ = strs[5];
        //                        mdt.GroupRU = strs[6];
        //                        mdt.NameEN = strs[7];
        //                        mdt.NameKZ = strs[8];
        //                        mdt.NameRU = strs[9];
        //                        mdt.AdditionalKZ = strs[11];
        //                        mdt.AdditionalRU = strs[12];
        //                        mdt.DescriptionEN = strs[13];
        //                        mdt.DescriptionKZ = strs[14];
        //                        mdt.DescriptionRU = strs[15];
        //                        db.Entry(mdt).State = EntityState.Modified;
        //                        count_modified++;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    db.SaveChanges();

        //    ViewBag.Report = $"Deleted: {count_deleted}; Modified: {count_modified}";
        //    return View();
        //}

        [Authorize(Roles = "Admin")]
        public ActionResult CreateLayerData()
        {
            IList<MeteoDataPeriodicity> meteodataperiodicities = db.MeteoDataPeriodicities
                .Where(m => m.Code.ToLower().Contains(Properties.Settings.Default.Average))
                .ToList();
            meteodataperiodicities = meteodataperiodicities
                .OrderBy(m => m.Name)
                .ToList();
            IList<MeteoDataSource> meteodatasources = db.MeteoDataSources
                .Where(m => true)
                .ToList();
            meteodatasources = meteodatasources
                .OrderBy(m => m.Name)
                .ToList();
            ViewBag.MeteoDataPeriodicities = new SelectList(meteodataperiodicities, "Id", "Name");
            ViewBag.MeteoDataSources = new SelectList(meteodatasources, "Id", "Name");
            if (Session["MeteoDataPeriodicityIdErase"] != null)
            {
                if (!(bool)Session["MeteoDataPeriodicityIdErase"])
                {
                    ViewBag.MeteoDataPeriodicities = new SelectList(meteodataperiodicities, "Id", "Name", (int?)Session["MeteoDataPeriodicityId"]);
                    ViewBag.MeteoDataSources = new SelectList(meteodatasources, "Id", "Name", (int?)Session["MeteoDataSourceId"]);
                }
                else
                {
                    Session["MeteoDataPeriodicityId"] = null;
                    Session["MeteoDataSourceId"] = null;
                }
            }
            Session["MeteoDataPeriodicityIdErase"] = true;

            if (Session["MeteoDataPeriodicityId"] != null)
            {
                int? MeteoDataPeriodicityId = (int?)Session["MeteoDataPeriodicityId"];
                int? MeteoDataSourceId = (int?)Session["MeteoDataSourceId"];
                IList<MeteoDataType> meteodatapetypes = db.MeteoDataTypes
                    .Where(m => (m.MeteoDataPeriodicityId == MeteoDataPeriodicityId || MeteoDataPeriodicityId == null) && (m.MeteoDataSourceId == MeteoDataSourceId || MeteoDataSourceId == null))
                    .ToList();
                meteodatapetypes = meteodatapetypes
                    .OrderBy(m => m.CodeAdditional)
                    .ToList();
                ViewBag.MeteoDataTypes = new SelectList(meteodatapetypes.OrderBy(m => m.CodeAdditional), "Id", "CodeAdditional");
            }

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult CreateLayerData(int? MeteoDataPeriodicityId, int? MeteoDataSourceId, int? MeteoDataTypeId, string Action)
        {
            string report = "";
            DateTime start = DateTime.Now;
            int count = 0;

            if (Action == "Change")
            {
                Session["MeteoDataPeriodicityId"] = MeteoDataPeriodicityId;
                Session["MeteoDataSourceId"] = MeteoDataSourceId;
                Session["MeteoDataPeriodicityIdErase"] = false;
                return RedirectToAction("CreateLayerData");
            }
            Session["MeteoDataPeriodicityIdErase"] = true;
            // CreateLayerData
            if (MeteoDataTypeId != null)
            {
                MeteoDataType meteodatatype = db.MeteoDataTypes
                    .Where(m => m.Id == MeteoDataTypeId)
                    .FirstOrDefault();
                MeteoDataPeriodicity meteodataperoidicity = db.MeteoDataPeriodicities
                    .Where(m => m.Id == meteodatatype.MeteoDataPeriodicityId)
                    .FirstOrDefault();
                MeteoDataSource meteodatasource = db.MeteoDataSources
                    .Where(m => m.Id == meteodatatype.MeteoDataSourceId)
                    .FirstOrDefault();
                //----------------------------------------------------------------------------------------------------------------------------------------
                if (meteodataperoidicity.Code.ToLower().Contains(Properties.Settings.Default.Monthly) && meteodataperoidicity.Code.ToLower().Contains(Properties.Settings.Default.Average))
                {
                    string CurrentUserId = User.Identity.GetUserId();
                    string path = "~/Download/" + CurrentUserId;
                    bool error = false;
                    try
                    {
                        if (!Directory.Exists(Server.MapPath(path)))
                        {
                            DirectoryInfo di = Directory.CreateDirectory(Server.MapPath(path));
                        }
                    }
                    catch (Exception e)
                    {
                        report += "<br/>" + e.Message;
                        error = true;
                    }
                    if (!error)
                    {
                        decimal longitude_min = 0,
                        longitude_max = 0,
                        latitude_min = 0,
                        latitude_max = 0,
                        step = 0;
                        if (meteodatasource.Code == Properties.Settings.Default.NASASSECode)
                        {
                            longitude_min = Properties.Settings.Default.NASASSELongitudeMin;
                            longitude_max = Properties.Settings.Default.NASASSELongitudeMax;
                            latitude_min = Properties.Settings.Default.NASASSELatitudeMin;
                            latitude_max = Properties.Settings.Default.NASASSELatitudeMax;
                            step = Properties.Settings.Default.NASASSECoordinatesStep;
                        }
                        if (meteodatasource.Code == Properties.Settings.Default.NASAPOWERCode)
                        {
                            longitude_min = Properties.Settings.Default.NASAPOWERLongitudeMin;
                            longitude_max = Properties.Settings.Default.NASAPOWERLongitudeMax;
                            latitude_min = Properties.Settings.Default.NASAPOWERLatitudeMin;
                            latitude_max = Properties.Settings.Default.NASAPOWERLatitudeMax;
                            step = Properties.Settings.Default.NASAPOWERCoordinatesStep;
                        }
                        if (meteodatasource.Code == Properties.Settings.Default.SARAHECode)
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
                        if (meteodatasource.Code == Properties.Settings.Default.CLARACode)
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
                        string invalid_filename = new string(Path.GetInvalidFileNameChars()),
                            invalid_path = new string(Path.GetInvalidPathChars()),
                            file_name_out = $"{meteodatatype.Code} {meteodatatype.AdditionalEN}.csv",
                            path_out = Server.MapPath(path);
                        foreach (char c in invalid_filename)
                        {
                            file_name_out = file_name_out.Replace(c.ToString(), "");
                        }
                        foreach (char c in invalid_path)
                        {
                            path_out = path_out.Replace(c.ToString(), "");
                        }
                        string filenameout = Path.Combine(path_out, file_name_out);

                        System.IO.File.Delete(filenameout);
                        using (StreamWriter sw = System.IO.File.AppendText(filenameout))
                        {
                            sw.WriteLine("latitude" + ";" +
                                "longitude" + ";" +
                                "sum month 1" + ";" +
                                "sum month 2" + ";" +
                                "sum month 3" + ";" +
                                "sum month 4" + ";" +
                                "sum month 5" + ";" +
                                "sum month 6" + ";" +
                                "sum month 7" + ";" +
                                "sum month 8" + ";" +
                                "sum month 9" + ";" +
                                "sum month 10" + ";" +
                                "sum month 11" + ";" +
                                "sum month 12" + ";" +
                                "average" + ";" +
                                "sum year" + ";" +
                                "average month 1" + ";" +
                                "average month 2" + ";" +
                                "average month 3" + ";" +
                                "average month 4" + ";" +
                                "average month 5" + ";" +
                                "average month 6" + ";" +
                                "average month 7" + ";" +
                                "average month 8" + ";" +
                                "average month 9" + ";" +
                                "average month 10" + ";" +
                                "average month 11" + ";" +
                                "average month 12"
                                );
                            for (decimal latitude = latitude_min; latitude <= latitude_max; latitude += step)
                            {
                                for (decimal longitude = longitude_min; longitude <= longitude_max; longitude += step)
                                {
                                    using (var db_ = new NpgsqlContext())
                                    {
                                        List<MeteoData> meteodatas = db_.MeteoDatas
                                            .Where(m => m.Latitude == latitude && m.Longitude == longitude && m.MeteoDataTypeId == meteodatatype.Id)
                                            .ToList();
                                        if (meteodatas.Count() > 0)
                                        {
                                            decimal?[] msperday = new decimal?[12];
                                            msperday[0] = meteodatas.Where(m => m.Month == 1).Select(m => m.Value).Sum();
                                            msperday[1] = meteodatas.Where(m => m.Month == 2).Select(m => m.Value).Sum();
                                            msperday[2] = meteodatas.Where(m => m.Month == 3).Select(m => m.Value).Sum();
                                            msperday[3] = meteodatas.Where(m => m.Month == 4).Select(m => m.Value).Sum();
                                            msperday[4] = meteodatas.Where(m => m.Month == 5).Select(m => m.Value).Sum();
                                            msperday[5] = meteodatas.Where(m => m.Month == 6).Select(m => m.Value).Sum();
                                            msperday[6] = meteodatas.Where(m => m.Month == 7).Select(m => m.Value).Sum();
                                            msperday[7] = meteodatas.Where(m => m.Month == 8).Select(m => m.Value).Sum();
                                            msperday[8] = meteodatas.Where(m => m.Month == 9).Select(m => m.Value).Sum();
                                            msperday[9] = meteodatas.Where(m => m.Month == 10).Select(m => m.Value).Sum();
                                            msperday[10] = meteodatas.Where(m => m.Month == 11).Select(m => m.Value).Sum();
                                            msperday[11] = meteodatas.Where(m => m.Month == 12).Select(m => m.Value).Sum();
                                            decimal?[] days = new decimal?[12];
                                            days[0] = 31;
                                            days[1] = 622 / 22; //!!!!!!!!!!!!!
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
                                            decimal? average = 0,
                                                    sum = 0,
                                                    days_count = 0;
                                            for (int i = 0; i < 12; i++)
                                            {
                                                sum += days[i] * msperday[i];
                                                days_count += days[i];
                                            }
                                            average = sum / days_count;
                                            sw.WriteLine(latitude.ToString() + ";" +
                                                longitude.ToString() + ";" +
                                                (msperday[0] * days[0]).ToString() + ";" +
                                                (msperday[1] * days[1]).ToString() + ";" +
                                                (msperday[2] * days[2]).ToString() + ";" +
                                                (msperday[3] * days[3]).ToString() + ";" +
                                                (msperday[4] * days[4]).ToString() + ";" +
                                                (msperday[5] * days[5]).ToString() + ";" +
                                                (msperday[6] * days[6]).ToString() + ";" +
                                                (msperday[7] * days[7]).ToString() + ";" +
                                                (msperday[8] * days[8]).ToString() + ";" +
                                                (msperday[9] * days[9]).ToString() + ";" +
                                                (msperday[10] * days[10]).ToString() + ";" +
                                                (msperday[11] * days[11]).ToString() + ";" +
                                                average.ToString() + ";" +
                                                sum.ToString() + ";" +
                                                msperday[0].ToString() + ";" +
                                                msperday[1].ToString() + ";" +
                                                msperday[2].ToString() + ";" +
                                                msperday[3].ToString() + ";" +
                                                msperday[4].ToString() + ";" +
                                                msperday[5].ToString() + ";" +
                                                msperday[6].ToString() + ";" +
                                                msperday[7].ToString() + ";" +
                                                msperday[8].ToString() + ";" +
                                                msperday[9].ToString() + ";" +
                                                msperday[10].ToString() + ";" +
                                                msperday[11].ToString()
                                                );
                                            count++;
                                        }

                                        db_.Dispose();
                                        GC.Collect();
                                    }

                                }
                            }
                        }
                    }
                }
                //----------------------------------------------------------------------------------------------------------------------
                TimeSpan time = DateTime.Now - start;
                report += "<br/>Time: " + time.ToString() + "<br/>Count: " + count.ToString();
                ViewBag.Report = report;
            }
            else
            {
                ViewBag.Report = "Select Periodicity and Source!";
            }
            return View();
        }

        // GET
        [Authorize(Roles = "Admin")]
        public ActionResult RefreshStatistics()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public void RefreshStatisticsTask()
        {
            string userid = User.Identity.GetUserId(),
                    user = User.Identity.Name;
            List<string> errors = new List<string>();
            string comment = "";

            try
            {
                List<Statistic> statistics = new List<Statistic>();
                List<MeteoDataSource> meteodatasources = new List<MeteoDataSource>();
                List<MeteoDataPeriodicity> meteodataperiodicities = new List<MeteoDataPeriodicity>();
                using (var dblocal = new NpgsqlContext())
                {
                    statistics = dblocal.Statistics.ToList();
                    meteodatasources = dblocal.MeteoDataSources.ToList();
                    meteodataperiodicities = dblocal.MeteoDataPeriodicities.ToList();

                    foreach (MeteoDataSource meteodatasource in meteodatasources)
                    {
                        foreach (MeteoDataPeriodicity meteodataperiodicity in meteodataperiodicities)
                        {
                            Statistic statistic_current = statistics.FirstOrDefault(s => s.MeteoDataSourceId == meteodatasource.Id && s.MeteoDataPeriodicityId == meteodataperiodicity.Id);
                            if (statistic_current == null)
                            {
                                statistic_current = new Statistic();
                                statistics.Add(statistic_current);
                            }
                            statistic_current.MeteoDataSourceId = meteodatasource.Id;
                            statistic_current.MeteoDataPeriodicityId = meteodataperiodicity.Id;
                            statistic_current.MeteoDataTypesCount = dblocal.MeteoDataTypes.Count(m => m.MeteoDataSourceId == meteodatasource.Id && m.MeteoDataPeriodicityId == meteodataperiodicity.Id);
                            List<MeteoDataType> meteodatatypes = dblocal.MeteoDataTypes.Where(m => m.MeteoDataSourceId == meteodatasource.Id && m.MeteoDataPeriodicityId == meteodataperiodicity.Id).ToList();
                            statistic_current.DataCount = 0;
                            foreach (MeteoDataType mdt in meteodatatypes)
                            {
                                statistic_current.DataCount += dblocal.MeteoDatas.Count(m => m.MeteoDataTypeId == mdt.Id);
                            }
                            statistic_current.DateTime = DateTime.Now;
                        }
                    }
                    dblocal.Dispose();
                    GC.Collect();
                }

                foreach (Statistic statistic in statistics)
                {
                    if (statistic.Id != 0)
                    {
                        db.Entry(statistic).State = EntityState.Modified;
                    }
                    else
                    {
                        db.Statistics.Add(statistic);
                    }
                }
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                errors.Add(ex.Message);
            }

            string errorss = errors.Count() == 0 ? null : string.Join("; ", errors.ToArray());
            SystemLog.New(
                userid,
                user,
                "RefreshStatistics",
                comment,
                errorss,
                false);
        }

        // получение ежедневных метеорологических данных NASA POWER
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult RefreshStatistics(bool? post)
        {
            SystemLog.New("RefreshStatistics", null, null, true);

            Task t = new Task(() => RefreshStatisticsTask());
            t.Start();

            ViewBag.Report = "Операция RefreshStatistics запущена.";
            return View();
        }

        [Authorize(Roles = "Admin")]
        // delete
        public ActionResult UploadNewMeteoDataTypes()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        // удалить
        public ActionResult UploadNewMeteoDataTypes(IEnumerable<HttpPostedFileBase> Files)
        {
            string userid = User.Identity.GetUserId(),
                    user = User.Identity.Name;
            int count_modified = 0;
            string path = "~/Upload/" + userid;
            if (!Directory.Exists(Server.MapPath(path)))
            {
                DirectoryInfo di2 = Directory.CreateDirectory(Server.MapPath(path));
            }
            foreach (HttpPostedFileBase file in Files)
            {
                string path_filename = Path.Combine(Server.MapPath(path), Path.GetFileName(file.FileName));
                file.SaveAs(path_filename);

                using (StreamReader reader = new StreamReader(path_filename))
                {
                    string line = "";

                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] strs = line.Split('\t');
                        if (strs[0] != "Id")
                        {
                            int id = Convert.ToInt32(strs[0]);
                            MeteoDataType mdt = db.MeteoDataTypes.FirstOrDefault(m => m.Id == id);
                            if (mdt != null)
                            {
                                if(strs[16] == "1")
                                {
                                    db.MeteoDataTypes.Remove(mdt);
                                    count_modified++;
                                }
                                else
                                {
                                    mdt.NameEN = strs[4];
                                    mdt.NameKZ = strs[5];
                                    mdt.NameRU = strs[6];
                                    mdt.GroupKZ = strs[9];
                                    mdt.GroupRU = strs[10];
                                    mdt.AdditionalKZ = strs[14];
                                    mdt.AdditionalRU = strs[15];
                                    db.Entry(mdt).State = EntityState.Modified;
                                }
                            }
                        }
                    }
                }
            }
            db.SaveChanges();

            ViewBag.Report = $"Modified: {count_modified}";
            return View();
        }

        [Authorize(Roles = "Admin")]
        // delete
        public ActionResult UploadRayon()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        // удалить
        public ActionResult UploadRayon(IEnumerable<HttpPostedFileBase> Files)
        {
            string userid = User.Identity.GetUserId(),
                    user = User.Identity.Name;
            int count_modified = 0;
            string path = "~/Upload/" + userid;
            if (!Directory.Exists(Server.MapPath(path)))
            {
                DirectoryInfo di2 = Directory.CreateDirectory(Server.MapPath(path));
            }
            foreach (HttpPostedFileBase file in Files)
            {
                //// .asc
                //string path_filename = Path.Combine(Server.MapPath(path), Path.GetFileName(file.FileName));
                //file.SaveAs(path_filename);
                //using (StreamReader reader = new StreamReader(path_filename))
                //{
                //    string line = "";
                //    while ((line = reader.ReadLine()) != null)
                //    {
                //    }
                //}

                // geoTiff
                try
                {
                    //http://trac.osgeo.org/gdal/browser/trunk/gdal/swig/csharp/apps/GDALRead.cs
                    string path_filename = Path.Combine(Server.MapPath(path), Path.GetFileName(file.FileName));
                    file.SaveAs(path_filename);
                    Gdal.AllRegister();

                    string[] args = new string[3];
                    args[0] = path_filename;
                    //string filename;
                    int iOverview = -1;
                    if (args.Length < 2)
                    {
                        int err = 1;
                    }
                    //if (args.Length == 3) iOverview = int.Parse(args[2]);

                    Dataset ds = Gdal.Open(args[0], Access.GA_ReadOnly);

                    Band band = ds.GetRasterBand(1);
                    if (iOverview >= 0 && band.GetOverviewCount() > iOverview)
                    {
                        band = band.GetOverview(iOverview);
                    }
                    int width = band.XSize; int height = band.YSize;
                    Bitmap bitmap = new Bitmap(width, height, PixelFormat.Format32bppRgb);
                    byte[] r = new byte[width * height];
                    band.ReadRaster(0, 0, width, height, r, width, height, 0, 0);
                    for (int i = 0; i < width; i++)
                    {
                        for (int j = 0; j < height; j++)
                        {
                            Color newColor = Color.FromArgb(Convert.ToInt32(r[i + j * width]), Convert.ToInt32(r[i + j * width]), Convert.ToInt32(r[i + j * width]));
                            bitmap.SetPixel(i, j, newColor);
                        }
                    }
                    bitmap.Save(Path.ChangeExtension(path_filename, ".bmp"));

                    // create geotiff
                    string path_filename_new = Path.Combine(Server.MapPath(path), "new_" + Path.GetFileName(file.FileName));
                    Driver drv = Gdal.GetDriverByName("GTiff");
                    //string[] options = new string[] { "TILED=YES" };
                    // options
                    int BlockXSize, BlockYSize;
                    band.GetBlockSize(out BlockXSize, out BlockYSize);
                    double val;
                    int hasval;
                    double minimum,
                        maximum,
                        NoDataValue;
                    band.GetMinimum(out val, out hasval);
                    if (hasval != 0) minimum = val;
                    band.GetMaximum(out val, out hasval);
                    if (hasval != 0) maximum = val;
                    band.GetNoDataValue(out val, out hasval);
                    if (hasval != 0) NoDataValue = val;

                    int bXSize = BlockXSize,
                        bYSize = BlockYSize;
                    int w = width,
                        h = height;
                    string[] options = new string[] { "BLOCKXSIZE=" + bXSize, "BLOCKYSIZE=" + bYSize };
                    float[] buffer = new float[w * h];
                    Random rnd = new Random();
                    for (int i = w - 1; i >= 0; i--)
                    {
                        for (int j = h - 1; j >= 0; j--)
                        {
                            buffer[i + j * w] = rnd.Next(350);//(i * 256 / w);
                        }
                    }

                    Dataset ds_new = drv.Create(path_filename_new, w, h, 1, DataType.GDT_Float32, options);
                    Band ba = ds_new.GetRasterBand(1);
                    ba.WriteRaster(0, 0, w, h, buffer, w, h, 0, 0);
                    ds_new.SetProjection(ds.GetProjection());
                    double[] pGT = new double[6];
                    ds.GetGeoTransform(pGT);
                    ds_new.SetGeoTransform(pGT);

                    ds_new.SetGCPs(ds.GetGCPs(), "");
                    ba.FlushCache();
                    ds_new.FlushCache();
                    //Dataset dso = drv.CreateCopy(path_filename_new, ds, 0, options, new Gdal.GDALProgressFuncDelegate(ProgressFunc), "Sample Data");
                }
                catch (Exception ex)
                {

                }
            }

            ViewBag.Report = $"Modified: {count_modified}";
            return View();
        }

        public static int ProgressFunc(double Complete, IntPtr Message, IntPtr Data)
        {
            //Console.Write("Processing ... " + Complete* 100 + "% Completed.");
            if (Message != IntPtr.Zero)
            {
                //Console.Write(" Message:" + System.Runtime.InteropServices.Marshal.PtrToStringAnsi(Message));
            }
            if (Data != IntPtr.Zero)
            {
                //Console.Write(" Data:" + System.Runtime.InteropServices.Marshal.PtrToStringAnsi(Data));
            }
            //Console.WriteLine("");
            return 1;
        }

        [Authorize(Roles = "Admin")]
        public ActionResult CreateDownloadData()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult CreateDownloadData(bool? post)
        {
            IList<MeteoDataPeriodicity> meteodataperiodicities = db.MeteoDataPeriodicities
                .Where(m => true)
                .ToList();
            IList<MeteoDataSource> meteodatasources = db.MeteoDataSources
                .Where(m => true)
                .ToList();
            IList<Province> provinces = db.Provinces
                .Where(p => true)
                .ToList();
            IList<MeteoDataType> meteodatatypes = db.MeteoDataTypes
                .Where(m => true)
                .ToList();
            foreach(MeteoDataType meteodatatype in meteodatatypes)
            {
                MeteoDataSource meteodatasource = meteodatasources
                    .Where(m => m.Id == meteodatatype.MeteoDataSourceId)
                    .FirstOrDefault();
                MeteoDataPeriodicity meteodataperoidicity = meteodataperiodicities
                    .Where(m => m.Id == meteodatatype.MeteoDataPeriodicityId)
                    .FirstOrDefault();
                decimal longitude_min = 0,
                        longitude_max = 0,
                        latitude_min = 0,
                        latitude_max = 0,
                        step = 1000;
                if (meteodatasource.Code == Properties.Settings.Default.NASASSECode)
                {
                    longitude_min = Properties.Settings.Default.NASASSELongitudeMin;
                    longitude_max = Properties.Settings.Default.NASASSELongitudeMax;
                    latitude_min = Properties.Settings.Default.NASASSELatitudeMin;
                    latitude_max = Properties.Settings.Default.NASASSELatitudeMax;
                    step = Properties.Settings.Default.NASASSECoordinatesStep;
                }
                if (meteodatasource.Code == Properties.Settings.Default.NASAPOWERCode)
                {
                    longitude_min = Properties.Settings.Default.NASAPOWERLongitudeMin;
                    longitude_max = Properties.Settings.Default.NASAPOWERLongitudeMax;
                    latitude_min = Properties.Settings.Default.NASAPOWERLatitudeMin;
                    latitude_max = Properties.Settings.Default.NASAPOWERLatitudeMax;
                    step = Properties.Settings.Default.NASAPOWERCoordinatesStep;
                    if(meteodataperoidicity.Code.ToLower().Contains(Properties.Settings.Default.Monthly))
                    {
                        longitude_min = Properties.Settings.Default.NASASSELongitudeMin;
                        longitude_max = Properties.Settings.Default.NASASSELongitudeMax;
                        latitude_min = Properties.Settings.Default.NASASSELatitudeMin;
                        latitude_max = Properties.Settings.Default.NASASSELatitudeMax;
                        step = Properties.Settings.Default.NASASSECoordinatesStep;
                    }
                }
                if (meteodatasource.Code == Properties.Settings.Default.SARAHECode)
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
                if (meteodatasource.Code == Properties.Settings.Default.CLARACode)
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
                foreach (Province province in provinces)
                {
                    for (decimal longitude = longitude_min; longitude <= longitude_max; longitude += step)
                    {
                        if (!(longitude >= province.Min_longitude && longitude <= province.Max_longitude))
                        {
                            continue;
                        }
                        for (decimal latitude = latitude_min; latitude <= latitude_max; latitude += step)
                        {
                            if (!(latitude >= province.Min_latitude && latitude <= province.Max_latitude))
                            {
                                continue;
                            }
                            using (var dblocal = new NpgsqlContext())
                            {
                                List<MeteoData> meteodatas = dblocal.MeteoDatas
                                    .Where(m => m.Latitude == latitude && m.Longitude == longitude && m.MeteoDataTypeId == meteodatatype.Id)
                                    .OrderBy(m => m.Year)
                                    .ThenBy(m => m.Month)
                                    .ThenBy(m => m.Day)
                                    .ToList();
                                string filenameout_pure = $"{meteodatatype.Id.ToString()} {province.Code} {longitude.ToString()}-{latitude.ToString()}.csv",
                                    filenameout = Path.Combine(Server.MapPath("~/Download/" + User.Identity.GetUserId()), filenameout_pure);
                                using (StreamWriter sw = new StreamWriter(filenameout, false, Encoding.UTF8))// System.IO.File.AppendText(filenameout))
                                {
                                    // добавить заголовки (тип данных, область, координаты)
                                    var rm = new ResourceManager(typeof(AtlasSolar.Resources.Common));
                                    string sMeteoDataTypeEN = rm.GetString("MeteoDataType", CultureInfo.CreateSpecificCulture("en")),
                                        sMeteoDataTypeKK = rm.GetString("MeteoDataType", CultureInfo.CreateSpecificCulture("kk")),
                                        sMeteoDataTypeRU = rm.GetString("MeteoDataType", CultureInfo.CreateSpecificCulture("ru"));
                                    string meteodatatypenameEN = "",
                                        meteodatatypenameRU = "",
                                        meteodatatypenameKZ = "";
                                    if (!string.IsNullOrEmpty(meteodatatype.GroupEN))
                                    {
                                        meteodatatypenameEN += meteodatatype.GroupEN;
                                    }
                                    if (!string.IsNullOrEmpty(meteodatatypenameEN))
                                    {
                                        meteodatatypenameEN += ": ";
                                    }
                                    meteodatatypenameEN += $"{meteodatatype.NameEN}";
                                    if (!string.IsNullOrEmpty(meteodatatype.AdditionalEN))
                                    {
                                        meteodatatypenameEN += $" {meteodatatype.AdditionalEN}";
                                    }
                                    if (!string.IsNullOrEmpty(meteodatatype.GroupRU))
                                    {
                                        meteodatatypenameRU += meteodatatype.GroupRU;
                                    }
                                    if (!string.IsNullOrEmpty(meteodatatypenameRU))
                                    {
                                        meteodatatypenameRU += ": ";
                                    }
                                    meteodatatypenameRU += $"{meteodatatype.NameRU}";
                                    if (!string.IsNullOrEmpty(meteodatatype.AdditionalRU))
                                    {
                                        meteodatatypenameRU += $" {meteodatatype.AdditionalRU}";
                                    }
                                    if (!string.IsNullOrEmpty(meteodatatype.GroupKZ))
                                    {
                                        meteodatatypenameKZ += meteodatatype.GroupKZ;
                                    }
                                    if (!string.IsNullOrEmpty(meteodatatypenameKZ))
                                    {
                                        meteodatatypenameKZ += ": ";
                                    }
                                    meteodatatypenameKZ += $"{meteodatatype.NameKZ}";
                                    if (!string.IsNullOrEmpty(meteodatatype.AdditionalKZ))
                                    {
                                        meteodatatypenameKZ += $" {meteodatatype.AdditionalKZ}";
                                    }

                                    string sProvinceEN = rm.GetString("Province", CultureInfo.CreateSpecificCulture("en")),
                                        sProvinceKK = rm.GetString("Province", CultureInfo.CreateSpecificCulture("kk")),
                                        sProvinceRU = rm.GetString("Province", CultureInfo.CreateSpecificCulture("ru"));

                                    string sCoordinatesEN = $"{rm.GetString("Coordinates", CultureInfo.CreateSpecificCulture("en"))} ({rm.GetString("Longitude", CultureInfo.CreateSpecificCulture("en"))}, {rm.GetString("Latitude", CultureInfo.CreateSpecificCulture("en"))})",
                                        sCoordinatesKK = $"{rm.GetString("Coordinates", CultureInfo.CreateSpecificCulture("kk"))} ({rm.GetString("Longitude", CultureInfo.CreateSpecificCulture("kk"))}, {rm.GetString("Latitude", CultureInfo.CreateSpecificCulture("kk"))})",
                                        sCoordinatesRU = $"{rm.GetString("Coordinates", CultureInfo.CreateSpecificCulture("ru"))} ({rm.GetString("Longitude", CultureInfo.CreateSpecificCulture("ru"))}, {rm.GetString("Latitude", CultureInfo.CreateSpecificCulture("ru"))})";

                                    sw.WriteLine($"{sMeteoDataTypeKK};{meteodatatypenameKZ};;");
                                    sw.WriteLine($"{sMeteoDataTypeRU};{meteodatatypenameRU};;");
                                    sw.WriteLine($"{sMeteoDataTypeEN};{meteodatatypenameEN};;");

                                    sw.WriteLine($"{sProvinceKK};{province.NameKZ};;");
                                    sw.WriteLine($"{sProvinceRU};{province.NameRU};;");
                                    sw.WriteLine($"{sProvinceEN};{province.NameEN};;");

                                    sw.WriteLine($"{sCoordinatesKK};{longitude};{latitude};");
                                    sw.WriteLine($"{sCoordinatesRU};{longitude};{latitude};");
                                    sw.WriteLine($"{sCoordinatesEN};{longitude};{latitude};");

                                    string sYearEN = rm.GetString("Year", CultureInfo.CreateSpecificCulture("en")),
                                        sYearKK = rm.GetString("Year", CultureInfo.CreateSpecificCulture("kk")),
                                        sYearRU = rm.GetString("Year", CultureInfo.CreateSpecificCulture("ru"));

                                    string sMonthEN = rm.GetString("Month", CultureInfo.CreateSpecificCulture("en")),
                                        sMonthKK = rm.GetString("Month", CultureInfo.CreateSpecificCulture("kk")),
                                        sMonthRU = rm.GetString("Month", CultureInfo.CreateSpecificCulture("ru"));

                                    string sDayEN = rm.GetString("Day", CultureInfo.CreateSpecificCulture("en")),
                                        sDayKK = rm.GetString("Day", CultureInfo.CreateSpecificCulture("kk")),
                                        sDayRU = rm.GetString("Day", CultureInfo.CreateSpecificCulture("ru"));

                                    string sValueEN = rm.GetString("Value", CultureInfo.CreateSpecificCulture("en")),
                                        sValueKK = rm.GetString("Value", CultureInfo.CreateSpecificCulture("kk")),
                                        sValueRU = rm.GetString("Value", CultureInfo.CreateSpecificCulture("ru"));

                                    sw.WriteLine($"{sYearKK};{sMonthKK};{sDayKK};{sValueKK}");
                                    sw.WriteLine($"{sYearRU};{sMonthRU};{sDayRU};{sValueRU}");
                                    sw.WriteLine($"{sYearEN};{sMonthEN};{sDayEN};{sValueEN}");

                                    foreach (MeteoData meteodata in meteodatas)
                                    {
                                        sw.WriteLine($"{meteodata.Year};{meteodata.Month};{meteodata.Day};{meteodata.Value}");
                                    }
                                }

                                string filenamezip_pure = $"{meteodatatype.Id.ToString()} {province.Code}.zip",
                                    filenamezip = Path.Combine(Server.MapPath("~/Download/" + User.Identity.GetUserId()), filenamezip_pure);
                                using (FileStream zipToOpen = new FileStream(filenamezip, FileMode.OpenOrCreate))
                                {
                                    using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Update))
                                    {
                                        ZipArchiveEntry readmeEntry = archive.CreateEntryFromFile(filenameout, filenameout_pure);
                                    }
                                }
                                System.IO.File.Delete(filenameout);
                            }
                        }
                    }
                }
            }
            return View();
        }
    }
}