using AtlasSolar.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AtlasSolar.Controllers
{
    public class MeteoUploaderController : Controller
    {
        // GET: MeteoUploader
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            return View();
        }

        // GET
        [Authorize(Roles = "Admin")]
        public ActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Upload(IEnumerable<HttpPostedFileBase> Files)
        {
            string report = "";
            DateTime start = DateTime.Now;
            int count = 0;

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
                foreach (HttpPostedFileBase file in Files)
                {
                    string path_filename = Path.Combine(Server.MapPath(path), Path.GetFileName(file.FileName));
                    file.SaveAs(path_filename);
                }

                string batfilename = Path.ChangeExtension(Path.Combine(Server.MapPath(path), DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss")), ".bat");
                StreamWriter bat = new StreamWriter(batfilename);
                foreach (HttpPostedFileBase file in Files)
                {
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
                // loading data from files
                foreach (HttpPostedFileBase file in Files)
                {
                    string filenameout = Path.Combine(Server.MapPath(path), Path.GetFileName(Path.ChangeExtension(Path.GetFileNameWithoutExtension(file.FileName) + "_out", "txt")));

                    List<decimal> lats = new List<decimal>();
                    List<decimal> lons = new List<decimal>();
                    List<DateTime> times = new List<DateTime>();
                    string[] lines = System.IO.File.ReadAllLines(Path.ChangeExtension(Path.Combine(Server.MapPath(path), Path.GetFileName(file.FileName)), "txt"));

                    bool variables = false,
                        global_attributes = false,
                        data = false;

                    List<string> meteodatatypecodes = new List<string>();
                    List<string> meteodatatypenameENs = new List<string>();

                    string meteodataperiodicitycode = "";
                    string meteodatasourcecode = "SARAH-E";
                    int meteodatatypeid = -1;
                    using (var db = new NpgsqlContext())
                    {
                        db.Configuration.AutoDetectChangesEnabled = false;
                        db.Configuration.ValidateOnSaveEnabled = false;

                        List<MeteoDataType> meteodatatypes = db.MeteoDataTypes.ToList();

                        int meteodatasourceid = -1;
                        int meteodataperiodicityid = -1;
                        int meteodatatypescount = -1;

                        using (StreamWriter sw = System.IO.File.AppendText(filenameout))
                        {
                            foreach (string line in lines)
                            {
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
                                                meteodataperiodicitycode = "Monthly";
                                                break;
                                            case "P1D":
                                                meteodataperiodicitycode = "Daily";
                                                break;
                                            case "PT1H":
                                                meteodataperiodicitycode = "Hourly";
                                                break;
                                            default:
                                                meteodataperiodicitycode = "";
                                                break;
                                        }
                                    }
                                }
                                if (data)
                                {
                                    if (meteodatasourceid < 0)
                                    {
                                        meteodatasourceid = db.MeteoDataSources
                                            .Where(m => m.Code == meteodatasourcecode)
                                            .FirstOrDefault()
                                            .Id;
                                        meteodataperiodicityid = db.MeteoDataPeriodicities
                                            .Where(m => m.Code == meteodataperiodicitycode)
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
                                                    NameEN = meteodatatypenameENs[i]
                                                });
                                                db.SaveChanges();
                                            }
                                        }
                                        meteodatatypes = db.MeteoDataTypes.ToList();
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

                                            sw.WriteLine(meteodatatypeid.ToString() + "\t" +
                                                times[Convert.ToInt32(pi[0])].Year.ToString() + "\t" +
                                                times[Convert.ToInt32(pi[0])].Month.ToString() + "\t" +
                                                (meteodataperiodicitycode == "Daily" || meteodataperiodicitycode == "Hourly" ? (int?)times[Convert.ToInt32(pi[0])].Day : null).ToString() + "\t" +
                                                (meteodataperiodicitycode == "Hourly" ? (int?)times[Convert.ToInt32(pi[0])].Hour : null).ToString() + "\t" +
                                                lons[Convert.ToInt32(pi[2])].ToString().Replace(',', '.') + "\t" +
                                                lats[Convert.ToInt32(pi[1])].ToString().Replace(',', '.') + "\t" +
                                                value.ToString().Replace(',', '.')
                                                );
                                            count++;
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
                        }
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
                // file deletion
                try
                {
                    System.IO.File.Delete(batfilename);
                }
                catch (Exception ex) { }
                foreach (HttpPostedFileBase file in Files)
                {
                    string path_filename = Path.Combine(Server.MapPath(path), Path.GetFileName(file.FileName));
                    string filenameout = Path.Combine(Server.MapPath(path), Path.GetFileName(Path.ChangeExtension(Path.GetFileNameWithoutExtension(file.FileName) + "_out", "txt")));
                    try
                    {
                        System.IO.File.Delete(path_filename);
                    }
                    catch (Exception ex) { }
                    try
                    {
                        System.IO.File.Delete(Path.ChangeExtension(path_filename, "txt"));
                    }
                    catch (Exception ex) { }
                    try
                    {
                        System.IO.File.Delete(filenameout);
                    }
                    catch (Exception ex) { }
                }
            }

            TimeSpan time = DateTime.Now - start;
            report += "<br/>Time: " + time.ToString() + "<br/>Count: " + count.ToString();
            ViewBag.Report = report;
            return View();
        }
    }
}