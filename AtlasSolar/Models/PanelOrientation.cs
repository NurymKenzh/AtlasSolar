using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AtlasSolar.Models
{
    [Table("PanelOrientations", Schema = "public")]
    public class PanelOrientation
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

        [Display(ResourceType = typeof(Resources.Common), Name = "Name")]
        public string Name
        {
            get
            {
                if (HttpContext.Current != null)
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
                        else
                        {
                            return NameEN;
                        }
                    }
                    else
                    {
                        return NameEN;
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