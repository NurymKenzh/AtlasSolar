using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AtlasSolar.Models
{
    [Table("Provinces", Schema = "public")]
    public class Province
    {
        public int Id { get; set; }

        [Display(ResourceType = typeof(Resources.Common), Name = "Code")]
        public string Code { get; set; }

        [Display(ResourceType = typeof(Resources.Common), Name = "NameEN")]
        public string NameEN { get; set; }

        [Display(ResourceType = typeof(Resources.Common), Name = "NameKZ")]
        public string NameKZ { get; set; }

        [Display(ResourceType = typeof(Resources.Common), Name = "NameRU")]
        public string NameRU { get; set; }
        
        public decimal? Max_auto_dist { get; set; }
        public decimal? Min_auto_dist { get; set; }

        public decimal? Max_lep_dist { get; set; }
        public decimal? Min_lep_dist { get; set; }

        public decimal? Max_np_dist { get; set; }
        public decimal? Min_np_dist { get; set; }

        public decimal? Max_slope_srtm { get; set; }
        public decimal? Min_slope_srtm { get; set; }

        public decimal? Max_srtm { get; set; }
        public decimal? Min_srtm { get; set; }

        public decimal? Max_swvdwnyear { get; set; }
        public decimal? Min_swvdwnyear { get; set; }

        public decimal? Max_longitude { get; set; }
        public decimal? Min_longitude { get; set; }

        public decimal? Max_latitude { get; set; }
        public decimal? Min_latitude { get; set; }

        [Display(ResourceType = typeof(Resources.Common), Name = "Name")]
        public string Name
        {
            get
            {
                HttpCookie cookie = HttpContext.Current.Request.Cookies["Language"];
                if (cookie != null)
                {
                    if (cookie.Value != null)
                    {
                        if (cookie.Value == "en")
                        {
                            return NameEN;
                        }
                        if (cookie.Value == "kk")
                        {
                            return NameKZ;
                        }
                        if (cookie.Value == "ru")
                        {
                            return NameRU;
                        }
                    }
                }
                else
                {
                    return NameEN;
                }
                return NameEN;
            }
        }
    }
}