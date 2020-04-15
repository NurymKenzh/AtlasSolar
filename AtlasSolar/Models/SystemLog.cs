using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AtlasSolar.Models
{
    [Table("SystemLogs", Schema = "public")]
    public class SystemLog
    {
        public int Id { get; set; }

        [Display(ResourceType = typeof(Resources.Common), Name = "Time")]
        public DateTime DateTime { get; set; }

        [Display(ResourceType = typeof(Resources.Common), Name = "UserId")]
        public string UserId { get; set; }

        [Display(ResourceType = typeof(Resources.Common), Name = "User")]
        public string User { get; set; }

        [Display(ResourceType = typeof(Resources.Common), Name = "Operation")]
        public string Operation { get; set; }

        [Display(ResourceType = typeof(Resources.Common), Name = "Action")]
        public string Action { get; set; }

        [Display(ResourceType = typeof(Resources.Common), Name = "Comment")]
        public string Comment { get; set; }

        [Display(ResourceType = typeof(Resources.Common), Name = "Errors")]
        public string Errors { get; set; }

        protected static ApplicationDbContext ApplicationDbContext { get; set; }
        protected static UserManager<ApplicationUser> UserManager { get; set; }

        public static void New(string UserId, string User, string OperationName, string Comment, string Errors, bool Start = true)
        {
            NpgsqlContext db = new NpgsqlContext();

            SystemLog systemlog = new SystemLog()
            {
                Action = Start ? Properties.Settings.Default.SystemLogActionStart : Properties.Settings.Default.SystemLogActionFinish,
                DateTime = DateTime.Now,
                Operation = OperationName,
                User = User,
                UserId = UserId,
                Comment = Comment,
                Errors = Errors
            };

            db.SystemLogs.Add(systemlog);
            db.SaveChanges();
        }

        public static void New(string OperationName, string Comment, string Errors, bool Start = true)
        {
            NpgsqlContext db = new NpgsqlContext();
            ApplicationDbContext = new ApplicationDbContext();
            UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(ApplicationDbContext));
            ApplicationUser user = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(HttpContext.Current.User.Identity.GetUserId());

            SystemLog systemlog = new SystemLog()
            {
                Action = Start ? Properties.Settings.Default.SystemLogActionStart : Properties.Settings.Default.SystemLogActionFinish,
                DateTime = DateTime.Now,
                Operation = OperationName,
                User = user.Email,
                UserId = user.Id,
                Comment = Comment,
                Errors = Errors
            };

            db.SystemLogs.Add(systemlog);
            db.SaveChanges();
        }

        public static void New(string OperationName, bool Start = true)
        {
            NpgsqlContext db = new NpgsqlContext();
            ApplicationDbContext = new ApplicationDbContext();
            UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(ApplicationDbContext));
            ApplicationUser user = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(HttpContext.Current.User.Identity.GetUserId());

            SystemLog systemlog = new SystemLog()
            {
                Action = Start ? Properties.Settings.Default.SystemLogActionStart : Properties.Settings.Default.SystemLogActionFinish,
                DateTime = DateTime.Now,
                Operation = OperationName,
                User = user.Email,
                UserId = user.Id
            };

            db.SystemLogs.Add(systemlog);
            db.SaveChanges();
        }
    }
}