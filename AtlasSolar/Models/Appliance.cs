using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AtlasSolar.Models
{
    [Table("Appliances", Schema = "public")]
    public class Appliance
    {
        public int Id { get; set; }

        [Display(ResourceType = typeof(Resources.Common), Name = "ApplianceType")]
        public ApplianceType ApplianceType { get; set; }

        [Display(ResourceType = typeof(Resources.Common), Name = "ApplianceType")]
        public int ApplianceTypeId { get; set; }

        [Display(ResourceType = typeof(Resources.Common), Name = "NameEN")]
        public string NameEN { get; set; }

        [Display(ResourceType = typeof(Resources.Common), Name = "NameKZ")]
        public string NameKZ { get; set; }

        [Display(ResourceType = typeof(Resources.Common), Name = "NameRU")]
        public string NameRU { get; set; }

        [Display(ResourceType = typeof(Resources.Common), Name = "Power")]
        public int? Power { get; set; }


        [Display(ResourceType = typeof(Resources.Common), Name = "Name")]
        public string Name
        {
            get
            {
                return ApplianceType.Name;
            }
        }

        [Display(ResourceType = typeof(Resources.Common), Name = "Name")]
        public string NamePower
        {
            get
            {
                return ApplianceType.Name + (Power != null ? $" ({Power.ToString()})" : "");
            }
        }
    }
}