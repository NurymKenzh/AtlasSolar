using AtlasSolar.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace AtlasSolar.Controllers
{
    public class ParserController : Controller
    {
        private NpgsqlContext db = new NpgsqlContext();

        // GET: Parser
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            return View();
        }

        // GET
        [Authorize(Roles = "Admin")]
        public ActionResult EraseMeteoData()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult EraseMeteoData(bool? post)
        {
            string report = "";
            DateTime start = DateTime.Now;

            using (var db = new NpgsqlContext())
            {
                db.Database.ExecuteSqlCommand("TRUNCATE TABLE \"MeteoData\"");
            }

            TimeSpan time = DateTime.Now - start;
            report += "<br/>Time: " + time.ToString() + "<br/>";
            ViewBag.Report = report;
            return View();
        }

        // GET
        [Authorize(Roles = "Admin")]
        public ActionResult ParseNASA1983Daily()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        // получение списка ежедневных данных NASA (1983-2005)
        public ActionResult ParseNASA1983Daily(bool? post)
        {
            string report = "";
            DateTime start = DateTime.Now;
            int count = 0;

            decimal longitude_min = 45 - 0.5m,
                    longitude_max = 95 + 0.5m,
                    latitude_min = 39 - 0.5m,
                    latitude_max = 56 + 0.5m;
            //decimal longitude_min = 45 - 0.5m,
            //        longitude_max = 44 + 0.5m,
            //        latitude_min = 39 - 0.5m,
            //        latitude_max = 38 + 0.5m;

            List<MeteoDataType> meteodatatypes = new List<MeteoDataType>();
            using (var db = new NpgsqlContext())
            {
                meteodatatypes = db.MeteoDataTypes
                    .Where(m => m.MeteoDataSource.Code == "NASA (1983-2005)" && m.MeteoDataPeriodicity.Code == "Daily")
                    .ToList();
                db.Dispose();
                GC.Collect();
            }
            List<int> meteodatatypeids = meteodatatypes
                .Select(m => m.Id)
                .ToList();

            string CurrentUserId = User.Identity.GetUserId();
            string path = "~/Upload/" + CurrentUserId;

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
                //string filenameout = Path.Combine(Server.MapPath(path), "From NASA (1983-2005).txt");
                //using (StreamWriter sw = System.IO.File.AppendText(filenameout))
                //{
                bool go = false;
                for (decimal latitude = latitude_min; latitude <= latitude_max; latitude++)
                {
                    for (decimal longitude = longitude_min; longitude <= longitude_max; longitude++)
                    {
                        string filenameout = Path.Combine(Server.MapPath(path), $"From NASA (1983-2005) {latitude.ToString()} - {longitude.ToString()}.txt");
                        using (StreamWriter sw = System.IO.File.AppendText(filenameout))
                        {
                            if (!go)
                            {
                                using (var db = new NpgsqlContext())
                                {
                                    if (db.MeteoDatas.Where(m => m.Longitude == longitude && m.Latitude == latitude && meteodatatypeids.Contains(m.MeteoDataTypeId)).FirstOrDefault() != null)
                                    {
                                        db.Dispose();
                                        GC.Collect();
                                        continue;
                                    }
                                    go = true;
                                    db.Dispose();
                                    GC.Collect();
                                }
                            }

                            string url = "https://eosweb.larc.nasa.gov/cgi-bin/sse/daily.cgi?email=skip%40larc.nasa.gov&step=1&lat=" +
                                        latitude.ToString().Replace(",", ".") +
                                        "&lon=" +
                                        longitude.ToString().Replace(",", ".") +
                                        "&sitelev=&ms=1&ds=1&ys=1983&me=12&de=31&ye=2005&p=swv_dwn&p=avg_kt&p=clr_sky&p=clr_dif&p=clr_dnr&p=clr_kt&p=lwv_dwn&p=toa_dwn&p=PS&p=TSKIN&p=T10M&p=T10MN&p=T10MX&p=Q10M&p=RH10M&p=DFP10M&submit=Submit&plot=swv_dwn";
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
                            //using (var db = new NpgsqlContext())
                            //{
                            //db.Configuration.AutoDetectChangesEnabled = false;
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
                                        //decimal out_decimal = 0;
                                        //db.MeteoDatas.Add(new MeteoData()
                                        //{
                                        //    Year = Convert.ToInt32(linecolumns[0]),
                                        //    Month = Convert.ToInt32(linecolumns[1]),
                                        //    Day = Convert.ToInt32(linecolumns[2]),
                                        //    Longitude = Convert.ToDecimal(slon.Replace(".", ",")),
                                        //    Latitude = Convert.ToDecimal(slat.Replace(".", ",")),
                                        //    MeteoDataTypeId = meteodatatypes.Where(m => m.Code == columns[c]).FirstOrDefault().Id,
                                        //    Value = decimal.TryParse(linecolumns[c].Replace(".", ","), out out_decimal) ? Convert.ToDecimal(linecolumns[c].Replace(".", ",")) : (decimal?)null
                                        //});
                                        sw.WriteLine(meteodatatypes.Where(m => m.Code == columns[c]).FirstOrDefault().Id.ToString() + "\t" +
                                            linecolumns[0] + "\t" +
                                            linecolumns[1] + "\t" +
                                            linecolumns[2] + "\t" +
                                            "\t" +
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
                            ////db.SaveChanges();
                            //db.Dispose();
                            //GC.Collect();
                            //}
                        }
                    }

                }
                //}

                //using (var db = new NpgsqlContext())
                //{
                //    string query = "COPY \"MeteoData\" (\"MeteoDataTypeId\", \"Year\", \"Month\", \"Day\", \"Hour\", \"Longitude\", \"Latitude\", \"Value\") FROM '" + filenameout + "' WITH NULL AS ''";
                //    try
                //    {
                //        db.MeteoDatas.SqlQuery(query).SingleOrDefault();
                //    }
                //    catch (Exception ex)
                //    {

                //    }
                //    db.Dispose();
                //    GC.Collect();
                //}
                for (decimal latitude = latitude_min; latitude <= latitude_max; latitude++)
                {
                    for (decimal longitude = longitude_min; longitude <= longitude_max; longitude++)
                    {
                        string filenameout = Path.Combine(Server.MapPath(path), $"From NASA (1983-2005) {latitude.ToString()} - {longitude.ToString()}.txt");
                        using (var db = new NpgsqlContext())
                        {
                            string query = "COPY \"MeteoData\" (\"MeteoDataTypeId\", \"Year\", \"Month\", \"Day\", \"Hour\", \"Longitude\", \"Latitude\", \"Value\") FROM '" + filenameout + "' WITH NULL AS ''";
                            try
                            {
                                db.MeteoDatas.SqlQuery(query).SingleOrDefault();
                            }
                            catch (Exception ex)
                            {
                                if (ex.Message != "The data reader is incompatible with the specified 'AtlasSolar.Models.MeteoData'. A member of the type, 'Id', does not have a corresponding column in the data reader with the same name.")
                                {
                                    int re = 5;
                                }
                            }
                            db.Dispose();
                            GC.Collect();
                        }
                    }
                }

            }



            TimeSpan time = DateTime.Now - start;
            report += "<br/>Time: " + time.ToString() + "<br/>Count: " + count.ToString() + "<br/>";
            ViewBag.Report = report;
            return View();
        }

        // GET
        [Authorize(Roles = "Admin")]
        public ActionResult ParseNASA1983Monthly()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        // получение списка среднемесячных данных NASA (1983-2005)
        public ActionResult ParseNASA1983Monthly(bool? post)
        {
            string report = "";
            DateTime start = DateTime.Now;

            List<MeteoDataType> meteodatatypes = new List<MeteoDataType>();
            using (var db = new NpgsqlContext())
            {
                meteodatatypes = db.MeteoDataTypes
                    .Where(m => m.MeteoDataSource.Code == "NASA (1983-2005)" && m.MeteoDataPeriodicity.Code == "Monthly average")
                    .ToList();
                db.Dispose();
                GC.Collect();
            }

            decimal longitude_min = 45 - 0.5m,
                    longitude_max = 95 + 0.5m,
                    latitude_min = 39 - 0.5m,
                    latitude_max = 56 + 0.5m;
            //decimal longitude_min = 45 - 0.5m,
            //        longitude_max = 44 + 0.5m,
            //        latitude_min = 39 - 0.5m,
            //        latitude_max = 38 + 0.5m;

            int count = 0;
            for (decimal latitude = latitude_min; latitude <= latitude_max; latitude++)
            {
                for (decimal longitude = longitude_min; longitude <= longitude_max; longitude++)
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
                    using (var db = new NpgsqlContext())
                    {
                        db.Configuration.AutoDetectChangesEnabled = false;
                        foreach (HtmlAgilityPack.HtmlNode node in root.Descendants())
                        {
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
                                                    .Where(m => m.Code == current_table_name && m.AdditionalEN == row_name)
                                                    .FirstOrDefault()
                                                    .Id;
                                                string V = td.InnerText.Replace("*", "").Trim();
                                                db.MeteoDatas.Add(new MeteoData()
                                                {
                                                    Latitude = Convert.ToDecimal(slat.Replace(".", ",")),
                                                    Longitude = Convert.ToDecimal(slon.Replace(".", ",")),
                                                    Year = null,
                                                    Month = current_td_i - 1,
                                                    Day = null,
                                                    MeteoDataTypeId = MeteoDataTypeId,
                                                    Value = decimal.TryParse(V.Replace(".", ","), out out_decimal) ? Convert.ToDecimal(V.Replace(".", ",")) : (decimal?)null
                                                });

                                                count++;
                                            }
                                        }
                                    }
                                }
                                current_table_name = "";
                            }
                        }
                        db.SaveChanges();
                        db.Dispose();
                        GC.Collect();
                    }
                }
            }

            TimeSpan time = DateTime.Now - start;
            report += "<br/>Time: " + time.ToString() + "<br/>Count: " + count.ToString() + "<br/>";
            ViewBag.Report = report;
            return View();
        }

        // GET
        [Authorize(Roles = "Admin")]
        public ActionResult EraseMeteoDataTypes()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult EraseMeteoDataTypes(bool? post)
        {
            string report = "";
            DateTime start = DateTime.Now;

            using (var db = new NpgsqlContext())
            {
                db.Database.ExecuteSqlCommand("TRUNCATE TABLE \"MeteoDataTypes\" CASCADE");
            }

            TimeSpan time = DateTime.Now - start;
            report += "<br/>Time: " + time.ToString() + "<br/>";
            ViewBag.Report = report;
            return View();
        }

        // GET
        [Authorize(Roles = "Admin")]
        public ActionResult ParseMeteoDataTypes()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        // получение списка типов данных
        public ActionResult ParseMeteoDataTypes(bool? post)
        {
            string report = "";
            DateTime start = DateTime.Now;
            int count = 0;

            string url = "";
            HtmlAgilityPack.HtmlDocument html = new HtmlAgilityPack.HtmlDocument();
            HtmlAgilityPack.HtmlNode root = html.DocumentNode;

            // среднемесячные типы NASA (1983-2005)
            decimal longitude_min = 45 - 0.5m,
                    longitude_max = 45 - 0.5m,
                    latitude_min = 39 - 0.5m,
                    latitude_max = 56 + 0.5m;

            for (decimal latitude = latitude_min; latitude <= latitude_max; latitude++)
            {
                for (decimal longitude = longitude_min; longitude <= longitude_max; longitude++)
                {
                    url = "https://eosweb.larc.nasa.gov/cgi-bin/sse/grid.cgi?&num=225129&lat=" +
                        latitude.ToString().Replace(",", ".") +
                        "&hgt=100&submit=Submit&veg=17&sitelev=&email=skip@larc.nasa.gov&p=grid_id&step=2&lon=" +
                        longitude.ToString().Replace(",", ".");
                    html = new HtmlAgilityPack.HtmlDocument();
                    html.LoadHtml(new WebClient().DownloadString(url));
                    root = html.DocumentNode;

                    string code = "";
                    string name = "";
                    int current_tr_i = 0;
                    using (var db = new NpgsqlContext())
                    {
                        List<MeteoDataType> meteodatatypes = db.MeteoDataTypes.ToList();
                        List<MeteoDataPeriodicity> meteodataperiodicities = db.MeteoDataPeriodicities.ToList();
                        List<MeteoDataSource> meteodatasources = db.MeteoDataSources.ToList();

                        foreach (HtmlAgilityPack.HtmlNode node in root.Descendants())
                        {
                            if (node.Name == "a")
                            {
                                if (node.GetAttributeValue("name", "") != "")
                                {
                                    code = node.GetAttributeValue("name", "");
                                }
                            }
                            if ((code != "") && (name == "") && (node.Name == "b"))
                            {
                                name = HttpUtility.HtmlDecode(node.InnerText.Trim());
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
                                            int MeteoDataSourceId = meteodatasources.Where(m => m.Code == "NASA (1983-2005)").FirstOrDefault().Id;
                                            int MeteoDataPeriodicityId = meteodataperiodicities.Where(m => m.Code == "Monthly average").FirstOrDefault().Id;
                                            additional = HttpUtility.HtmlDecode(tr.Descendants("td").FirstOrDefault().InnerText.Trim());
                                            int is_yet = meteodatatypes
                                                .Where(m => m.MeteoDataSourceId == MeteoDataSourceId && m.MeteoDataPeriodicityId == MeteoDataPeriodicityId && m.Code == code && m.AdditionalEN == additional)
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
                                                    Code = code,
                                                    NameEN = name,
                                                    AdditionalEN = additional
                                                });
                                                count++;
                                            }
                                        }
                                    }
                                    code = "";
                                    name = "";
                                }
                            }
                        }
                        db.SaveChanges();
                        db.Dispose();
                        GC.Collect();
                    }
                }
            }

            // ежедневные типы NASA (1983-2005)
            url = "https://eosweb.larc.nasa.gov/cgi-bin/sse/daily.cgi?email=skip%40larc.nasa.gov&step=1&lat=" +
                "38.5".ToString().Replace(",", ".") +
                "&lon=" +
                "44.5".ToString().Replace(",", ".") +
                "&sitelev=&ms=1&ds=1&ys=1983&me=12&de=31&ye=2005&p=swv_dwn&p=avg_kt&p=clr_sky&p=clr_dif&p=clr_dnr&p=clr_kt&p=lwv_dwn&p=toa_dwn&p=PS&p=TSKIN&p=T10M&p=T10MN&p=T10MX&p=Q10M&p=RH10M&p=DFP10M&submit=Submit&plot=swv_dwn";
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
                        int MeteoDataSourceId = meteodatasources.Where(m => m.Code == "NASA (1983-2005)").FirstOrDefault().Id;
                        int MeteoDataPeriodicityId = meteodataperiodicities.Where(m => m.Code == "Daily").FirstOrDefault().Id;
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
                                Code = linecolumns[0],
                                NameEN = linecolumns[1]
                            });
                            count++;
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
            }

            // ежедневные типы NASA
            //file_url = "http://power.larc.nasa.gov/cgi-bin/hirestimeser.cgi?email=hirestimeser%40larc.nasa.gov&step=1&lat=" +
            //    "38.5".ToString().Replace(",", ".") +
            //    "&lon=" +
            //    "44.5".ToString().Replace(",", ".") +
            //    "&ms=1&ds=1&ys=1981&me=1&de=2&ye=1981&p=MHswv_dwn&p=MHclr_sky&p=MHT2M&p=MHT2MN&p=MHT2MX&p=MHRH2M&p=MHWS10M&p=MHPRECTOT&submit=Submit";
            //file_url = "http://power.larc.nasa.gov/cgi-bin/hirestimeser.cgi?&lat=38.5&ye=1981&p=MHswv_dwn&p=MHclr_sky&p=MHT2M&p=MHT2MN&p=MHT2MX&p=MHRH2M&p=MHWS10M&p=MHPRECTOT&de=2&submit=Submit&ms=1&me=1&lon=44.5&step=1&email=hirestimeser40larc.nasa.gov&ys=1981&ds=1";
            file_url = "https://power.larc.nasa.gov/cgi-bin/hirestimeser.cgi?email=hirestimeser%40larc.nasa.gov&step=1&lat=38&lon=44&ms=1&ds=1&ys=2016&me=1&de=2&ye=2016&p=MHswv_dwn&p=MHlwv_dwn&p=MHtoa_dwn&p=MHclr_sky&p=MHPS&p=MHT2M&p=MHT2MN&p=MHT2MX&p=MHQV2M&p=MHRH2M&p=MHDFP2M&p=MHTS&p=MHWS10M&p=MHPRECTOT&submit=Submit";
            file = (new WebClient()).DownloadString(file_url);
            lines = file.Split('\n');
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
                        int MeteoDataSourceId = meteodatasources.Where(m => m.Code == "NASA").FirstOrDefault().Id;
                        int MeteoDataPeriodicityId = meteodataperiodicities.Where(m => m.Code == "Daily").FirstOrDefault().Id;
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
                                Code = linecolumns[0],
                                NameEN = linecolumns[1]
                            });
                            count++;
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
            }

            TimeSpan time = DateTime.Now - start;
            report += "<br/>Time: " + time.ToString() + "<br/>Count: " + count.ToString() + "<br/>";
            ViewBag.Report = report;
            return View();
        }

        // GET
        [Authorize(Roles = "Admin")]
        public ActionResult ParseNASADaily()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        // получение списка ежедневных данных NASA
        public ActionResult ParseNASADaily(bool? post)
        {
            string report = "";
            DateTime start = DateTime.Now;

            decimal longitude_min = 45 - 0.25m,
                    longitude_max = 95 + 0.25m,
                    latitude_min = 39 - 0.25m,
                    latitude_max = 56 + 0.25m;
            //decimal longitude_min = 44.75m,
            //        longitude_max = 44.75m,
            //        latitude_min = 38.75m,
            //        latitude_max = 38.75m;

            List<MeteoDataType> meteodatatypes = new List<MeteoDataType>();
            using (var db = new NpgsqlContext())
            {
                meteodatatypes = db.MeteoDataTypes
                    .Where(m => true)
                    .Include(m => m.MeteoDataSource)
                    .Include(m => m.MeteoDataPeriodicity)
                    .Where(m => m.MeteoDataSource.Code == "NASA" && m.MeteoDataPeriodicity.Code == "Daily")
                    .ToList();
                db.Dispose();
                GC.Collect();
            }
            List<int> meteodatatypeids = meteodatatypes
                .Select(m => m.Id)
                .ToList();

            int count = 0;

            for (decimal latitude = latitude_min; latitude <= latitude_max; latitude += 0.5M)
            {
                for (decimal longitude = longitude_min; longitude <= longitude_max; longitude += 0.5M)
                {
                    bool go = false;
                    if (!go)
                    {
                        using (var db = new NpgsqlContext())
                        {
                            if (db.MeteoDatas.Where(m => m.Longitude == longitude && m.Latitude == latitude && m.Year == 1982 && meteodatatypeids.Contains(m.MeteoDataTypeId)).FirstOrDefault() != null)
                            {
                                db.Dispose();
                                GC.Collect();
                                continue;
                            }
                            go = true;
                            db.Dispose();
                            GC.Collect();
                        }
                    }

                    //string file_url = "http://power.larc.nasa.gov/cgi-bin/hirestimeser.cgi?email=hirestimeser%40larc.nasa.gov&step=1&lat=" +
                    //    latitude.ToString().Replace(",", ".") +
                    //    "&lon=" +
                    //    longitude.ToString().Replace(",", ".") +
                    //    "&ms=1&ds=1&ys=1981&me=12&de=31&ye=2016&p=MHswv_dwn&p=MHclr_sky&p=MHT2M&p=MHT2MN&p=MHT2MX&p=MHRH2M&p=MHWS10M&p=MHPRECTOT&submit=Submit";
                    string file_url = "https://power.larc.nasa.gov/cgi-bin/hirestimeser.cgi?email=hirestimeser%40larc.nasa.gov&step=1&lat=" +
                        latitude.ToString().Replace(",", ".") +
                        "&lon=" +
                        longitude.ToString().Replace(",", ".") +
                        "&ms=1&ds=1&ys=1982&me=1&de=31&ye=1982&p=MHswv_dwn&p=MHlwv_dwn&p=MHtoa_dwn&p=MHclr_sky&p=MHPS&p=MHT2M&p=MHT2MN&p=MHT2MX&p=MHQV2M&p=MHRH2M&p=MHDFP2M&p=MHTS&p=MHWS10M&p=MHPRECTOT&submit=Submit";

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
                    using (var db = new NpgsqlContext())
                    {
                        db.Configuration.AutoDetectChangesEnabled = false;
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
                                    try
                                    {
                                        db.MeteoDatas.Add(new MeteoData()
                                        {
                                            Year = Convert.ToInt32(linecolumns[0]),
                                            Month = Convert.ToInt32(linecolumns[1]),
                                            Day = Convert.ToInt32(linecolumns[2]),
                                            Longitude = longitude,
                                            Latitude = latitude,
                                            MeteoDataTypeId = meteodatatypes.Where(m => m.Code == columns[c]).FirstOrDefault().Id,
                                            Value = decimal.TryParse(linecolumns[c].Replace(".", ","), out out_decimal) ? Convert.ToDecimal(linecolumns[c].Replace(".", ",")) : (decimal?)null
                                        });
                                        count++;
                                    }
                                    catch (Exception ex)
                                    {

                                    }
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
                        db.SaveChanges();
                        db.Dispose();
                        GC.Collect();
                    }
                }
            }

            TimeSpan time = DateTime.Now - start;
            report += "<br/>Time: " + time.ToString() + "<br/>Count: " + count.ToString() + "<br/>";
            ViewBag.Report = report;
            return View();
        }

        // GET
        [Authorize(Roles = "Admin")]
        public ActionResult Test()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Test(bool? post)
        {
            new Thread(() => {
                //Do an advanced looging here which takes a while
                try
                {
                    string query = "COPY (SELECT \"MeteoDataTypeId\", \"Year\", \"Month\", \"Day\", \"Longitude\", \"Latitude\", \"Value\" FROM \"MeteoData\") TO 'D:\\My documents\\Test.csv' DELIMITER ',' CSV HEADER;";
                    db.MeteoDatas.SqlQuery(query).SingleOrDefault();
                    //await db.MeteoDatas.SqlQuery(query).SingleOrDefaultAsync();
                }
                catch (Exception ex)
                {

                }
            }).Start();
            ViewBag.Report = "Test started";
            return View();
        }
    }
}