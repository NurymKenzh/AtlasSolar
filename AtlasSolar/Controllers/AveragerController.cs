using AtlasSolar.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AtlasSolar.Controllers
{
    public class AveragerController : Controller
    {
        private NpgsqlContext db = new NpgsqlContext();

        // GET: Averager
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Average()
        {
            IList<MeteoDataPeriodicity> meteodataperiodicities = db.MeteoDataPeriodicities
                .Where(m => !m.Code.Contains("average"))
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
                    .OrderBy(m => m.NameEN)
                    .ToList();
                ViewBag.MeteoDataTypes = new SelectList(meteodatapetypes, "Id", "NameEN");
            }

            IList<MeteoDataPeriodicity> meteodataperiodicitiesto = db.MeteoDataPeriodicities
                .Where(m => true)
                .ToList();
            meteodataperiodicitiesto = meteodataperiodicitiesto
                .OrderBy(m => m.Name)
                .ToList();
            ViewBag.MeteoDataPeriodicitiesTo = new SelectList(meteodataperiodicitiesto, "Id", "Name");

            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Average(int? MeteoDataPeriodicityId, int? MeteoDataPeriodicityToId, int? MeteoDataSourceId, int? MeteoDataTypeId, string Action)
        {
            string report = "";
            DateTime start = DateTime.Now;
            int count = 0;

            //if(MeteoDataTypeId==null)
            if (Action == "Change")
            {
                Session["MeteoDataPeriodicityId"] = MeteoDataPeriodicityId;
                Session["MeteoDataSourceId"] = MeteoDataSourceId;
                Session["MeteoDataPeriodicityIdErase"] = false;
                return RedirectToAction("Average");
            }
            Session["MeteoDataPeriodicityIdErase"] = true;
            // Average
            if (MeteoDataTypeId != null && MeteoDataPeriodicityToId != null)
            {
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
                if (meteodataperoidicity.Code.Contains("Daily") && meteodataperoidicityto.Code.Contains("Daily") && meteodataperoidicityto.Code.Contains("average"))
                {
                    // add a new type of weather data if not exists
                    int ismeteodatatypeto = db.MeteoDataTypes
                        .Where(m => m.MeteoDataPeriodicityId == meteodataperoidicityto.Id && m.MeteoDataSourceId == meteodatatype.MeteoDataSourceId)
                        .Count();
                    if (ismeteodatatypeto == 0)
                    {
                        MeteoDataType meteodatatypetonew = new MeteoDataType()
                        {
                            AdditionalEN = meteodatatype.AdditionalEN,
                            Code = meteodatatype.Code,
                            MeteoDataSourceId = meteodatatype.MeteoDataSourceId,
                            NameEN = meteodatatype.NameEN,
                            NameKZ = meteodatatype.NameKZ,
                            NameRU = meteodatatype.NameRU,
                            MeteoDataPeriodicityId = meteodataperoidicityto.Id
                        };
                        db.MeteoDataTypes.Add(meteodatatypetonew);
                        db.SaveChanges();
                    }
                    MeteoDataType meteodatatypeto = db.MeteoDataTypes
                       .Where(m => m.MeteoDataPeriodicityId == MeteoDataPeriodicityToId && m.MeteoDataSourceId == meteodatasource.Id)
                       .FirstOrDefault();

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
                        decimal longitude_min = 45 - 0.5m,
                            longitude_max = 95 + 0.5m,
                            latitude_min = 39 - 0.5m,
                            latitude_max = 56 + 0.5m;
                        string filenameout = Path.Combine(Server.MapPath(path), "Average.txt");
                        using (StreamWriter sw = System.IO.File.AppendText(filenameout))
                        {
                            for (decimal latitude = latitude_min; latitude <= latitude_max; latitude++)
                            {
                                for (decimal longitude = longitude_min; longitude <= longitude_max; longitude++)
                                {
                                    using (var db_ = new NpgsqlContext())
                                    {
                                        List<MeteoData> meteodatas = db_.MeteoDatas
                                            .Where(m => m.Latitude == latitude && m.Longitude == longitude && m.MeteoDataTypeId == meteodatatype.Id)
                                            .ToList();
                                        for (int month = 1; month <= 12; month++)
                                        {
                                            for (int day = 1; day <= 31; day++)
                                            {
                                                List<MeteoData> meteodatasnew = meteodatas.
                                                    Where(m => m.Month == month && m.Day == day)
                                                    .ToList();
                                                if (meteodatasnew.Count() > 0)
                                                {
                                                    sw.WriteLine(meteodatatypeto.Id.ToString() + "\t" +
                                                        "\t" +
                                                        month.ToString() + "\t" +
                                                        day.ToString() + "\t" +
                                                        longitude.ToString().Replace(',', '.') + "\t" +
                                                        latitude.ToString().Replace(',', '.') + "\t" +
                                                        meteodatasnew.Average(m => m.Value).ToString().Replace(',', '.')
                                                        );
                                                    count++;
                                                }
                                            }
                                        }
                                        db_.Dispose();
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

                            }
                        }
                        try
                        {
                            System.IO.File.Delete(filenameout);
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }
                //----------------------------------------------------------------------------------------------------------------------------------------
                if (meteodataperoidicity.Code.Contains("Daily") && meteodataperoidicityto.Code.Contains("Yearly") && meteodataperoidicityto.Code.Contains("average"))
                {
                    // add a new type of weather data if not exists
                    int ismeteodatatypeto = db.MeteoDataTypes
                        .Where(m => m.MeteoDataPeriodicityId == meteodataperoidicityto.Id && m.MeteoDataSourceId == meteodatatype.MeteoDataSourceId && m.Code == meteodatatype.Code)
                        .Count();
                    if (ismeteodatatypeto == 0)
                    {
                        MeteoDataType meteodatatypetonew = new MeteoDataType()
                        {
                            AdditionalEN = meteodatatype.AdditionalEN,
                            Code = meteodatatype.Code,
                            MeteoDataSourceId = meteodatatype.MeteoDataSourceId,
                            NameEN = meteodatatype.NameEN,
                            NameKZ = meteodatatype.NameKZ,
                            NameRU = meteodatatype.NameRU,
                            MeteoDataPeriodicityId = meteodataperoidicityto.Id
                        };
                        db.MeteoDataTypes.Add(meteodatatypetonew);
                        db.SaveChanges();
                    }
                    MeteoDataType meteodatatypeto = db.MeteoDataTypes
                       .Where(m => m.MeteoDataPeriodicityId == MeteoDataPeriodicityToId && m.MeteoDataSourceId == meteodatasource.Id)
                       .FirstOrDefault();

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
                        decimal longitude_min = 45 - 0.5m,
                            longitude_max = 95 + 0.5m,
                            latitude_min = 39 - 0.5m,
                            latitude_max = 56 + 0.5m;
                        string filenameout = Path.Combine(Server.MapPath(path), "Average.txt");
                        try
                        {
                            System.IO.File.Delete(filenameout);
                        }
                        catch (Exception ex)
                        {

                        }
                        using (StreamWriter sw = System.IO.File.AppendText(filenameout))
                        {
                            for (decimal latitude = latitude_min; latitude <= latitude_max; latitude++)
                            {
                                for (decimal longitude = longitude_min; longitude <= longitude_max; longitude++)
                                {
                                    using (var db_ = new NpgsqlContext())
                                    {
                                        List<MeteoData> meteodatas = db_.MeteoDatas
                                            .Where(m => m.Latitude == latitude && m.Longitude == longitude && m.MeteoDataTypeId == meteodatatype.Id)
                                            .ToList();
                                        List<MeteoData> meteodatasnew = meteodatas.ToList();
                                        if (meteodatasnew.Count() > 0)
                                        {
                                            sw.WriteLine(meteodatatypeto.Id.ToString() + "\t" +
                                                "\t" +
                                                "\t" +
                                                "\t" +
                                                longitude.ToString().Replace(',', '.') + "\t" +
                                                latitude.ToString().Replace(',', '.') + "\t" +
                                                meteodatasnew.Average(m => m.Value).ToString().Replace(',', '.')
                                                );
                                            count++;
                                        }
                                        db_.Dispose();
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

                            }
                        }
                        try
                        {
                            System.IO.File.Delete(filenameout);
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }
                //----------------------------------------------------------------------------------------------------------------------------------------
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

        [Authorize(Roles = "Admin")]
        public ActionResult CreateLayerData()
        {
            IList<MeteoDataPeriodicity> meteodataperiodicities = db.MeteoDataPeriodicities
                //.Where(m => !m.Code.Contains("average"))
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
                    .OrderBy(m => m.NameEN)
                    .ToList();
                ViewBag.MeteoDataTypes = new SelectList(meteodatapetypes.OrderBy(m => m.Code), "Id", "CodeAdditional");
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
            // Average
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
                if (meteodataperoidicity.Code.Contains("Monthly average"))
                {
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
                        decimal longitude_min = 45 - 0.5m,
                            longitude_max = 95 + 0.5m,
                            latitude_min = 39 - 0.5m,
                            latitude_max = 56 + 0.5m;

                        longitude_min = Properties.Settings.Default.NASASSELongitudeMin;
                        longitude_max = Properties.Settings.Default.NASASSELongitudeMax;
                        latitude_min = Properties.Settings.Default.NASASSELatitudeMin;
                        latitude_max = Properties.Settings.Default.NASASSELatitudeMax;
                        decimal step = Properties.Settings.Default.NASASSECoordinatesStep;

                        string filenameout = Path.Combine(Server.MapPath(path), $"{meteodatatype.Code} {meteodatatype.AdditionalEN}.csv");
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
    }
}