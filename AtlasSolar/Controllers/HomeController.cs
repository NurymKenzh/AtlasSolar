using AtlasSolar.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace AtlasSolar.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //// если пользователь авторизован, то создаются роли "Admin" и "Moderator", текущий пользователь добавляется к роли "Admin"
            //// должно отработать хотя бы один раз
            //if (User.Identity.IsAuthenticated)
            //{
            //    ApplicationDbContext context = new ApplicationDbContext();
            //    var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            //    var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            //    if (!roleManager.RoleExists("Admin"))
            //    {
            //        var role = new IdentityRole();
            //        role.Name = "Admin";
            //        roleManager.Create(role);
            //        UserManager.AddToRole(User.Identity.GetUserId(), "Admin");
            //    }
            //    if (!roleManager.RoleExists("Moderator"))
            //    {
            //        var role = new IdentityRole();
            //        role.Name = "Moderator";
            //        roleManager.Create(role);
            //    }
            //}

            List<string> news_links = new List<string>();
            List<string> news_link_texts = new List<string>();
            List<string> news_texts = new List<string>();
            string news_link_main = "http://energo.gov.kz/index.php?id=40";
            try
            {
                HtmlAgilityPack.HtmlDocument html = new HtmlAgilityPack.HtmlDocument();
                HtmlAgilityPack.HtmlNode root = html.DocumentNode;
                string langprefix = "en.";
                string langid = "42";
                HttpCookie cookie = Request.Cookies["Language"];
                if (cookie != null)
                {
                    if (cookie.Value != null)
                    {
                        if (cookie.Value == "en")
                        {
                            langprefix = "en.";
                            langid = "42";
                        }
                        if (cookie.Value == "kk")
                        {
                            langprefix = "kz.";
                            langid = "41";
                        }
                        if (cookie.Value == "ru")
                        {
                            langprefix = "";
                            langid = "40";
                        }
                    }
                }
                string url = "http://" + langprefix + "energo.gov.kz/index.php?id=" + langid;
                news_link_main = url;
                html = new HtmlAgilityPack.HtmlDocument();
                //html.LoadHtml(new WebClient().DownloadString(url));
                //html.Load(new WebClient().DownloadString(url), Encoding.UTF8);
                using (var client = new WebClient())
                {
                    using (var stream = client.OpenRead(url))
                    {
                        var reader = new StreamReader(stream, Encoding.UTF8);
                        var html_ = reader.ReadToEnd();
                        html.LoadHtml(html_);
                    }
                }

                root = html.DocumentNode;
                bool begin = false;

                string news_link = "",
                    news_link_text = "",
                    news_text = "";
                int max_news_count = 5;
                
                foreach (HtmlAgilityPack.HtmlNode node in root.Descendants())
                {
                    if(begin)
                    {
                        if (node.Name == "a")
                        {
                            news_link = node.GetAttributeValue("href", "").Trim();
                        }
                        if (node.GetAttributeValue("class", "").Trim() == "mp_news_block_title")
                        {
                            news_link_text = node.InnerHtml.Trim();
                            news_link_text = news_link_text.Replace("<h2>", "");
                            news_link_text = news_link_text.Replace("</h2>", "");
                        }
                        if (node.GetAttributeValue("class", "").Trim() == "mp_news_block_excerp")
                        {
                            news_text = node.InnerHtml.Trim();
                        }
                        if(news_link!="" && news_link_text!="" && news_text !="")
                        {
                            news_links.Add(news_link);
                            news_link_texts.Add(news_link_text);
                            news_texts.Add(news_text);
                            news_link = "";
                            news_link_text = "";
                            news_text = "";
                            if(news_links.Count >= max_news_count)
                            {
                                break;
                            }
                        }
                    }
                    if(node.GetAttributeValue("class", "").Trim() == "news_items")
                    {
                        begin = true;
                    }
                }
            }
            catch
            {

            }
            
            ViewBag.NewsLinks = news_links;
            ViewBag.NewsLinkTexts = news_link_texts;
            ViewBag.NewsTexts = news_texts;
            ViewBag.NewsLink = news_link_main;

            return View();
            //return RedirectToAction("Demo", "Maps");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}