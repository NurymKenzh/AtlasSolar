using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AtlasSolar.Models
{
    [Table("PVSystemMaterials", Schema = "public")]
    public class PVSystemMaterial
    {
        public int Id { get; set; }

        [Display(ResourceType = typeof(Resources.Common), Name = "NameEN")]
        public string NameEN { get; set; }

        [Display(ResourceType = typeof(Resources.Common), Name = "NameKZ")]
        public string NameKZ { get; set; }

        [Display(ResourceType = typeof(Resources.Common), Name = "NameRU")]
        public string NameRU { get; set; }

        [Display(ResourceType = typeof(Resources.Common), Name = "Efficiency")]
        [DisplayFormat(DataFormatString = "{0:G}", ApplyFormatInEditMode = true)]
        public decimal Efficiency { get; set; }

        // Nominal solar cell operating temperature NOCT
        [Display(ResourceType = typeof(Resources.Common), Name = "RatedOperatingTemperature")]
        public int RatedOperatingTemperature { get; set; }

        // Photographic Panel Temperature Power Factor
        [Display(ResourceType = typeof(Resources.Common), Name = "ThermalPowerFactor")]
        [DisplayFormat(DataFormatString = "{0:G}", ApplyFormatInEditMode = true)]
        public decimal ThermalPowerFactor { get; set; }

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