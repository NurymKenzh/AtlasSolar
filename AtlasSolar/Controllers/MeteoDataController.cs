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
    public class MeteoDataController : Controller
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

        // GET: MeteoData
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<ActionResult> Index(string SortOrder, int? MeteoDataTypeId, int? Year, int? Month, int? Day, decimal? Longitude, decimal? Latitude, int? Page)
        {
            HttpCookie cookie = System.Web.HttpContext.Current.Request.Cookies["Language"];

            var meteodatas = db.MeteoDatas
                .Where(m => true)
                .Include(m => m.MeteoDataType);

            ViewBag.MeteoDataTypeIdFilter = MeteoDataTypeId;
            ViewBag.YearFilter = Year;
            ViewBag.MonthFilter = Month;
            ViewBag.DayFilter = Day;
            ViewBag.LongitudeFilter = Longitude;
            ViewBag.LatitudeFilter = Latitude;

            ViewBag.MeteoDataTypeSort = SortOrder == "MeteoDataType" ? "MeteoDataTypeDesc" : "MeteoDataType";
            ViewBag.YearSort = SortOrder == "Year" ? "YearDesc" : "Year";
            ViewBag.MonthSort = SortOrder == "Month" ? "MonthDesc" : "Month";
            ViewBag.DaySort = SortOrder == "Day" ? "DayDesc" : "Day";
            ViewBag.LongitudeSort = SortOrder == "Longitude" ? "LongitudeDesc" : "Longitude";
            ViewBag.LatitudeSort = SortOrder == "Latitude" ? "LatitudeDesc" : "Latitude";

            if (MeteoDataTypeId != null)
            {
                meteodatas = meteodatas.Where(m => m.MeteoDataTypeId == MeteoDataTypeId);
            }
            if (Year != null)
            {
                meteodatas = meteodatas.Where(m => m.Year == Year);
            }
            if (Month != null)
            {
                meteodatas = meteodatas.Where(m => m.Month == Month);
            }
            if (Day != null)
            {
                meteodatas = meteodatas.Where(m => m.Day == Day);
            }
            if (Longitude != null)
            {
                meteodatas = meteodatas.Where(m => m.Longitude == Longitude);
            }
            if (Latitude != null)
            {
                meteodatas = meteodatas.Where(m => m.Latitude == Latitude);
            }

            // сортировка работает очень медленно
            //switch (SortOrder)
            //{
            //    case "MeteoDataType":
            //        if (cookie != null)
            //        {
            //            if (!string.IsNullOrEmpty(cookie.Value))
            //            {
            //                if (cookie.Value == "en")
            //                {
            //                    meteodatas = meteodatas
            //                        .OrderBy(m => m.MeteoDataType.NameEN)
            //                        .ThenBy(m => m.MeteoDataType.AdditionalEN);
            //                }
            //                if (cookie.Value == "kk")
            //                {
            //                    meteodatas = meteodatas
            //                        .OrderBy(m => m.MeteoDataType.NameKZ)
            //                        .ThenBy(m => m.MeteoDataType.AdditionalKZ);
            //                }
            //                if (cookie.Value == "ru")
            //                {
            //                    meteodatas = meteodatas
            //                        .OrderBy(m => m.MeteoDataType.NameRU)
            //                        .ThenBy(m => m.MeteoDataType.AdditionalRU);
            //                }
            //            }
            //        }
            //        else
            //        {
            //            meteodatas = meteodatas
            //                .OrderBy(m => m.MeteoDataType.NameRU)
            //                .ThenBy(m => m.MeteoDataType.AdditionalRU);
            //        }
            //        break;
            //    case "MeteoDataTypeDesc":
            //        if (cookie != null)
            //        {
            //            if (!string.IsNullOrEmpty(cookie.Value))
            //            {
            //                if (cookie.Value == "en")
            //                {
            //                    meteodatas = meteodatas
            //                        .OrderByDescending(m => m.MeteoDataType.NameEN)
            //                        .ThenByDescending(m => m.MeteoDataType.AdditionalEN);
            //                }
            //                if (cookie.Value == "kk")
            //                {
            //                    meteodatas = meteodatas
            //                        .OrderByDescending(m => m.MeteoDataType.NameKZ)
            //                        .ThenByDescending(m => m.MeteoDataType.AdditionalKZ);
            //                }
            //                if (cookie.Value == "ru")
            //                {
            //                    meteodatas = meteodatas
            //                        .OrderByDescending(m => m.MeteoDataType.NameRU)
            //                        .ThenByDescending(m => m.MeteoDataType.AdditionalRU);
            //                }
            //            }
            //        }
            //        else
            //        {
            //            meteodatas = meteodatas
            //                .OrderByDescending(m => m.MeteoDataType.NameRU)
            //                .ThenByDescending(m => m.MeteoDataType.AdditionalRU);
            //        }
            //        break;
            //    case "Year":
            //        meteodatas = meteodatas.OrderBy(m => m.Year);
            //        break;
            //    case "YearDesc":
            //        meteodatas = meteodatas.OrderByDescending(m => m.Year);
            //        break;
            //    case "Month":
            //        meteodatas = meteodatas.OrderBy(m => m.Month);
            //        break;
            //    case "MonthDesc":
            //        meteodatas = meteodatas.OrderByDescending(m => m.Month);
            //        break;
            //    case "Day":
            //        meteodatas = meteodatas.OrderBy(m => m.Day);
            //        break;
            //    case "DayDesc":
            //        meteodatas = meteodatas.OrderByDescending(m => m.Day);
            //        break;
            //    case "Longitude":
            //        meteodatas = meteodatas.OrderBy(m => m.Longitude);
            //        break;
            //    case "LongitudeDesc":
            //        meteodatas = meteodatas.OrderByDescending(m => m.Longitude);
            //        break;
            //    case "Latitude":
            //        meteodatas = meteodatas.OrderBy(m => m.Latitude);
            //        break;
            //    case "LatitudeDesc":
            //        meteodatas = meteodatas.OrderByDescending(m => m.Latitude);
            //        break;
            //    default:
            //        meteodatas = meteodatas.OrderBy(m => m.Id);
            //        break;
            //}

            IList<MeteoDataType> meteodatatypes = db.MeteoDataTypes
                .Where(m => true)
                .ToList();
            meteodatatypes = meteodatatypes
                .OrderBy(m => m.Name)
                .ToList();
            ViewBag.MeteoDataTypes = new SelectList(meteodatatypes, "Id", "NameAdditional");

            ViewBag.Role = "";
            string CurrentUserId = User.Identity.GetUserId();
            if (UserManager.IsInRole(CurrentUserId, "Admin"))
            {
                ViewBag.Role = "Admin";
            }

            int PageSize = 50;
            int PageNumber = (Page ?? 1);

            //return View(meteodatas.Take(100000).ToPagedList(PageNumber, PageSize));
            return View(meteodatas.Take(100000).OrderBy(m => m.Id).ToPagedList(PageNumber, PageSize));
        }

        // GET: MeteoData/Details/5
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MeteoData meteoData = await db.MeteoDatas
                .Where(m => m.Id == id)
                .Include(m => m.MeteoDataType)
                .FirstOrDefaultAsync();
            if (meteoData == null)
            {
                return HttpNotFound();
            }

            ViewBag.Role = "";
            string CurrentUserId = User.Identity.GetUserId();
            if (UserManager.IsInRole(CurrentUserId, "Admin"))
            {
                ViewBag.Role = "Admin";
            }

            return View(meteoData);
        }

        // GET: MeteoData/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            ViewBag.MeteoDataTypeId = new SelectList(db.MeteoDataTypes
                .ToList()
                .OrderBy(m => m.Name), "Id", "NameAdditional");
            return View();
        }

        // POST: MeteoData/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Create([Bind(Include = "Id,MeteoDataTypeId,Year,Month,Day,Longitude,Latitude,Value")] MeteoData meteoData)
        {
            if (ModelState.IsValid)
            {
                db.MeteoDatas.Add(meteoData);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.MeteoDataTypeId = new SelectList(db.MeteoDataTypes
                .ToList()
                .OrderBy(m => m.Name), "Id", "NameAdditional", meteoData.MeteoDataTypeId);
            return View(meteoData);
        }

        // GET: MeteoData/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MeteoData meteoData = await db.MeteoDatas.FindAsync(id);
            if (meteoData == null)
            {
                return HttpNotFound();
            }
            ViewBag.MeteoDataTypeId = new SelectList(db.MeteoDataTypes
                .ToList()
                .OrderBy(m => m.Name), "Id", "NameAdditional", meteoData.MeteoDataTypeId);
            return View(meteoData);
        }

        // POST: MeteoData/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Edit([Bind(Include = "Id,MeteoDataTypeId,Year,Month,Day,Longitude,Latitude,Value")] MeteoData meteoData)
        {
            if (ModelState.IsValid)
            {
                db.Entry(meteoData).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.MeteoDataTypeId = new SelectList(db.MeteoDataTypes
                .ToList()
                .OrderBy(m => m.Name), "Id", "NameAdditional", meteoData.MeteoDataTypeId);
            return View(meteoData);
        }

        // GET: MeteoData/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MeteoData meteoData = await db.MeteoDatas
                .Where(m => m.Id == id)
                .Include(m => m.MeteoDataType)
                .FirstOrDefaultAsync();
            if (meteoData == null)
            {
                return HttpNotFound();
            }
            return View(meteoData);
        }

        // POST: MeteoData/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            MeteoData meteoData = await db.MeteoDatas.FindAsync(id);
            db.MeteoDatas.Remove(meteoData);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        //// GET
        //[Authorize(Roles = "Admin")]
        //public ActionResult Parse()
        //{
        //    return View();
        //}

        //[HttpPost]
        //[Authorize(Roles = "Admin")]
        //public ActionResult Parse(bool? post)
        //{
        //    // формирование списка ежедневных данных NASA old
        //    string report = "";
        //    DateTime start = DateTime.Now;

        //    //List<NASADaily> data = new List<NASADaily>();

        //    decimal longitude_min = 45 - 0.5m,
        //            longitude_max = 95 + 0.5m,
        //            latitude_min = 39 - 0.5m,
        //            latitude_max = 56 + 0.5m;
        //    //decimal longitude_min = 45 - 0.5m,
        //    //        longitude_max = 44 + 0.5m,
        //    //        latitude_min = 39 - 0.5m,
        //    //        latitude_max = 38 + 0.5m;

        //    int count = 0;
        //    bool go = false;
        //    //db.Configuration.AutoDetectChangesEnabled = false;
        //    for (decimal latitude = latitude_min; latitude <= latitude_max; latitude++)
        //    {
        //        for (decimal longitude = longitude_min; longitude <= longitude_max; longitude++)
        //        {
        //            if (!go)
        //            {
        //                using (var ctx = new NpgsqlContext())
        //                {
        //                    if (ctx.MeteoDatas.Where(n => n.Longitude == longitude && n.Latitude == latitude && n.MeteoDataTypeId >= 249).FirstOrDefault() != null)
        //                    {
        //                        ctx.Dispose();
        //                        GC.Collect();
        //                        continue;
        //                    }
        //                    go = true;
        //                    ctx.Dispose();
        //                    GC.Collect();
        //                }
        //            }

        //            string url = "https://eosweb.larc.nasa.gov/cgi-bin/sse/daily.cgi?email=skip%40larc.nasa.gov&step=1&lat=" +
        //                        latitude.ToString().Replace(",", ".") +
        //                        "&lon=" +
        //                        longitude.ToString().Replace(",", ".") +
        //                        "&sitelev=&ms=1&ds=1&ys=1983&me=12&de=31&ye=2005&p=swv_dwn&p=avg_kt&p=clr_sky&p=clr_dif&p=clr_dnr&p=clr_kt&p=lwv_dwn&p=toa_dwn&p=PS&p=TSKIN&p=T10M&p=T10MN&p=T10MX&p=Q10M&p=RH10M&p=DFP10M&submit=Submit&plot=swv_dwn";
        //            HtmlAgilityPack.HtmlDocument html = new HtmlAgilityPack.HtmlDocument();
        //            html.LoadHtml(new WebClient().DownloadString(url));

        //            HtmlAgilityPack.HtmlNode root = html.DocumentNode;

        //            string slat = "",
        //                    slon = "";
        //            foreach (HtmlAgilityPack.HtmlNode td in root.Descendants("td"))
        //            {
        //                if (td.InnerText.IndexOf("Center") >= 0)
        //                {
        //                    IEnumerable<HtmlAgilityPack.HtmlNode> bs = td.Descendants("b");
        //                    slat = bs.First().InnerText;
        //                    slon = bs.Last().InnerText;
        //                    break;
        //                }
        //            }

        //            string file_url = "https://eosweb.larc.nasa.gov";
        //            foreach (HtmlAgilityPack.HtmlNode node in root.Descendants())
        //            {
        //                if ((node.Name == "a") && (node.InnerText == "Download a text file"))
        //                {
        //                    file_url += node.GetAttributeValue("href", "");
        //                    break;
        //                }
        //            }

        //            string file = (new WebClient()).DownloadString(file_url);
        //            //string[] lines = System.IO.File.ReadAllLines((new WebClient()).DownloadString(file_url));
        //            string[] lines = file.Split('\n');
        //            List<string> columns = new List<string>();
        //            using (var ctx = new NpgsqlContext())
        //            {
        //                ctx.Configuration.AutoDetectChangesEnabled = false;
        //                foreach (string line in lines)
        //                {
        //                    if (line.Length == 0)
        //                    {
        //                        continue;
        //                    }

        //                    string s = line;
        //                    while (s.Contains("  "))
        //                    {
        //                        s = s.Replace("  ", " ");
        //                    }
        //                    string[] linecolumns = s.Split(' ');



        //                    if (columns.Count > 0)
        //                    {
        //                        for (int c = 3; c < columns.Count; c++)
        //                        {
        //                            //NASADaily nd = new NASADaily()
        //                            //{
        //                            //    Year = Convert.ToInt32(linecolumns[0]),
        //                            //    Month = Convert.ToInt32(linecolumns[1]),
        //                            //    Day = Convert.ToInt32(linecolumns[2]),
        //                            //    Longitude = Convert.ToDecimal(slon.Replace(".", ",")),
        //                            //    Latitude = Convert.ToDecimal(slat.Replace(".", ",")),
        //                            //    Name = columns[c],
        //                            //    Value = Convert.ToDecimal(linecolumns[c].Replace(".", ",")),
        //                            //};
        //                            decimal out_decimal = 0;
        //                            ctx.MeteoDatas.Add(new MeteoData()
        //                            {
        //                                Year = Convert.ToInt32(linecolumns[0]),
        //                                Month = Convert.ToInt32(linecolumns[1]),
        //                                Day = Convert.ToInt32(linecolumns[2]),
        //                                Longitude = Convert.ToDecimal(slon.Replace(".", ",")),
        //                                Latitude = Convert.ToDecimal(slat.Replace(".", ",")),
        //                                //Name = columns[c],
        //                                MeteoDataTypeId = c + 246,
        //                                //Value = Convert.ToDecimal(linecolumns[c].Replace(".", ",")),
        //                                Value = decimal.TryParse(linecolumns[c].Replace(".", ","), out out_decimal) ? Convert.ToDecimal(linecolumns[c].Replace(".", ",")) : (decimal?)null
        //                            });

        //                            count++;
        //                        }
        //                    }



        //                    if (line.Contains("YEAR MO DY"))
        //                    {
        //                        foreach (string linecolumn in linecolumns)
        //                        {
        //                            columns.Add(linecolumn);
        //                        }
        //                    }
        //                }
        //                ctx.SaveChanges();
        //                ctx.Dispose();
        //                GC.Collect();
        //            }
        //            //db.SaveChanges();
        //            //db.Dispose();
        //            //GC.Collect();
        //            //using (var ctx = new NpgsqlContext())
        //            //{
        //            //    // do the inserts
        //            //    ctx.SaveChanges();
        //            //    ctx.Dispose();
        //            //    GC.Collect();
        //            //}
        //        }
        //    }
        //    //db.SaveChanges();

        //    TimeSpan time = DateTime.Now - start;
        //    report += "<br/>" + time.ToString() + "<br/>" + count.ToString() + "<br/>";
        //    ViewBag.Report = report;

        //    //===================================================================================================================================
        //    // формирование списка среднемесячных данных NASA old
        //    //string report = "";
        //    //DateTime start = DateTime.Now;

        //    //List<MeteoDataType> meteodatatypes_0 = db.MeteoDataTypes.ToList();

        //    //List <MeteoData> data = new List<MeteoData>();

        //    ////decimal longitude_min = 45 - 0.5m,
        //    ////        longitude_max = 95 + 0.5m,
        //    ////        latitude_min = 39 - 0.5m,
        //    ////        latitude_max = 56 + 0.5m;
        //    //decimal longitude_min = 45 - 0.5m,
        //    //        longitude_max = 44 + 0.5m,
        //    //        latitude_min = 39 - 0.5m,
        //    //        latitude_max = 38 + 0.5m;

        //    //int count = 0;
        //    ////db.Configuration.AutoDetectChangesEnabled = false;
        //    //for (decimal latitude = latitude_min; latitude <= latitude_max; latitude++)
        //    //{
        //    //    for (decimal longitude = longitude_min; longitude <= longitude_max; longitude++)
        //    //    {
        //    //        string url = "https://eosweb.larc.nasa.gov/cgi-bin/sse/grid.cgi?&num=225129&lat=" +
        //    //                        latitude.ToString().Replace(",", ".") +
        //    //                        "&hgt=100&submit=Submit&veg=17&sitelev=&email=skip@larc.nasa.gov&p=grid_id&step=2&lon=" +
        //    //                        longitude.ToString().Replace(",", ".");
        //    //        HtmlAgilityPack.HtmlDocument html = new HtmlAgilityPack.HtmlDocument();
        //    //        html.LoadHtml(new WebClient().DownloadString(url));

        //    //        HtmlAgilityPack.HtmlNode root = html.DocumentNode;

        //    //        string slat = "",
        //    //                slon = "";
        //    //        foreach (HtmlAgilityPack.HtmlNode td in root.Descendants("td"))
        //    //        {
        //    //            if (td.InnerText.IndexOf("Center") >= 0)
        //    //            {
        //    //                IEnumerable<HtmlAgilityPack.HtmlNode> bs = td.Descendants("b");
        //    //                slat = bs.First().InnerText;
        //    //                slon = bs.Last().InnerText;
        //    //                break;
        //    //            }
        //    //        }

        //    //        string current_table_name = "";
        //    //        List<MeteoDataType> meteodatatypes = meteodatatypes_0;
        //    //        using (var ctx = new NpgsqlContext())
        //    //        {
        //    //            ctx.Configuration.AutoDetectChangesEnabled = false;
        //    //            foreach (HtmlAgilityPack.HtmlNode node in root.Descendants())
        //    //            {
        //    //                if (node.Name == "a")
        //    //                {
        //    //                    if (node.GetAttributeValue("name", "") != "")
        //    //                    {
        //    //                        current_table_name = node.GetAttributeValue("name", "");
        //    //                    }
        //    //                }
        //    //                if ((current_table_name != "") && (node.Name == "div"))
        //    //                {
        //    //                    int current_tr_i = 0;
        //    //                    foreach (HtmlAgilityPack.HtmlNode tr in node.Descendants("tr"))
        //    //                    {
        //    //                        current_tr_i++;
        //    //                        int mdt_id = 0;
        //    //                        if (current_tr_i > 1)
        //    //                        {
        //    //                            int current_td_i = 0;
        //    //                            string row_name = "";

        //    //                            //foreach (MeteoDataType mdt in meteodatatypes)
        //    //                            //{
        //    //                            //    if ((mdt.Code == current_table_name) && (mdt.AdditionalEN == tr.FirstChild.InnerText.Trim()))
        //    //                            //    {
        //    //                            //        mdt_id = mdt.Id;
        //    //                            //        meteodatatypes.Remove(mdt);
        //    //                            //        break;
        //    //                            //    }
        //    //                            //}
        //    //                            mdt_id = 2;

        //    //                            foreach (HtmlAgilityPack.HtmlNode td in tr.Descendants("td"))
        //    //                            {
        //    //                                current_td_i++;
        //    //                                if (current_td_i == 1)
        //    //                                {
        //    //                                    row_name = td.InnerText;

        //    //                                    //foreach (MeteoDataType mdt in meteodatatypes)
        //    //                                    //{
        //    //                                    //    if ((mdt.Code == current_table_name)&&(mdt.AdditionalEN == row_name.Trim()))
        //    //                                    //    {
        //    //                                    //        mdt_id = mdt.Id;
        //    //                                    //        break;
        //    //                                    //    }
        //    //                                    //}
        //    //                                }
        //    //                                else
        //    //                                {

        //    //                                    decimal out_decimal = 0;
        //    //                                    if(mdt_id!=0)
        //    //                                    {
        //    //                                        ctx.MeteoData.Add(new MeteoData()
        //    //                                        {
        //    //                                            Latitude = Convert.ToDecimal(slat.Replace(".", ",")),
        //    //                                            Longitude = Convert.ToDecimal(slon.Replace(".", ",")),
        //    //                                            Year = null,
        //    //                                            Month = current_td_i - 1,
        //    //                                            Day = null,
        //    //                                            //Rowname = row_name.Trim(),
        //    //                                            //Tablename = current_table_name.Trim(),
        //    //                                            MeteoDataTypeId = mdt_id,
        //    //                                            Value = decimal.TryParse(td.InnerText.Replace(".", ","), out out_decimal) ? Convert.ToDecimal(td.InnerText.Replace(".", ",")) : (decimal?)null
        //    //                                        });
        //    //                                        count++;
        //    //                                    }
        //    //                                    else
        //    //                                    {
        //    //                                        report += "Unknoun type: " + current_table_name + ", " + row_name.Trim() + "<br/>";
        //    //                                    }

        //    //                                }
        //    //                            }
        //    //                        }
        //    //                    }
        //    //                    current_table_name = "";
        //    //                }
        //    //            }

        //    //            ctx.SaveChanges();
        //    //            ctx.Dispose();
        //    //            GC.Collect();
        //    //        }

        //    //        //db.SaveChanges();
        //    //    }
        //    //}
        //    //db.SaveChanges();

        //    //TimeSpan time = DateTime.Now - start;
        //    //report += time.ToString() + "<br/>";
        //    //ViewBag.Report = report;



        //    //===================================================================================================================================
        //    // формирование списка среднемесячных типов данных NASA old
        //    //string url =    "https://eosweb.larc.nasa.gov/cgi-bin/sse/grid.cgi?&num=225129&lat=" +
        //    //                38.ToString().Replace(",", ".") +
        //    //                "&hgt=100&submit=Submit&veg=17&sitelev=&email=skip@larc.nasa.gov&p=grid_id&step=2&lon=" +
        //    //                44.ToString().Replace(",", ".");
        //    //HtmlAgilityPack.HtmlDocument html = new HtmlAgilityPack.HtmlDocument();
        //    //html.LoadHtml(new WebClient().DownloadString(url));
        //    //HtmlAgilityPack.HtmlNode root = html.DocumentNode;

        //    //List<MeteoDataType> meteodatatypes = new List<MeteoDataType>();
        //    //string code = "";
        //    //string name = "";
        //    //int current_tr_i = 0;
        //    //foreach (HtmlAgilityPack.HtmlNode node in root.Descendants())
        //    //{
        //    //    if (node.Name == "a")
        //    //    {
        //    //        if (node.GetAttributeValue("name", "") != "")
        //    //        {
        //    //            code = node.GetAttributeValue("name", "");
        //    //        }
        //    //    }
        //    //    if((code!="")&&(name=="")&&(node.Name == "b"))
        //    //    {
        //    //        name = node.InnerText;
        //    //    }

        //    //    if ((code != "") && (name != "") )
        //    //    {
        //    //        if(node.ParentNode.Name == "table")
        //    //        {
        //    //            current_tr_i = 0;
        //    //            foreach (HtmlAgilityPack.HtmlNode tr in node.ParentNode.Descendants("tr"))
        //    //            {
        //    //                current_tr_i++;
        //    //                if(current_tr_i>1)
        //    //                {
        //    //                    string additional = "";
        //    //                    // переписать foreach
        //    //                    foreach (HtmlAgilityPack.HtmlNode td in tr.Descendants("td"))
        //    //                    {
        //    //                        additional = td.InnerText.Trim();
        //    //                        break;
        //    //                    }
        //    //                    meteodatatypes.Add(new MeteoDataType()
        //    //                    {
        //    //                        Code = code,
        //    //                        MeteoDataPeriodicityId = 1,
        //    //                        NameEN = name,
        //    //                        Source = "NASA old",
        //    //                        AdditionalEN = additional
        //    //                    });

        //    //                }
        //    //            }
        //    //            code = "";
        //    //            name = "";
        //    //        }
        //    //    }
        //    //}
        //    //foreach(MeteoDataType mdt in meteodatatypes)
        //    //{
        //    //    db.MeteoDataTypes.Add(mdt);
        //    //}
        //    //db.SaveChanges();
        //    return View();
        //}

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
