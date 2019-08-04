using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.AccessControl;
using System.Web;

namespace AtlasSolar.Models
{
    [Table("SPP", Schema = "public")]
    public class SPP
    {
        public int Id { get; set; }

        [Display(ResourceType = typeof(Resources.Common), Name = "PanelsCount")]
        public int? Count { get; set; }

        [Display(ResourceType = typeof(Resources.Common), Name = "PowerMW")]
        public decimal? Power { get; set; }

        [Display(ResourceType = typeof(Resources.Common), Name = "Cost")]
        public string Cost { get; set; }

        [Display(ResourceType = typeof(Resources.Common), Name = "CommissioningYear")]
        public int? Startup { get; set; }

        [Display(ResourceType = typeof(Resources.Common), Name = "Link")]
        public string Link { get; set; }

        [Display(ResourceType = typeof(Resources.Common), Name = "Customer")]
        public string Customer { get; set; }

        [Display(ResourceType = typeof(Resources.Common), Name = "Investor")]
        public string Investor { get; set; }

        [Display(ResourceType = typeof(Resources.Common), Name = "Executor")]
        public string Executor { get; set; }

        [Display(ResourceType = typeof(Resources.Common), Name = "Name")]
        public string Name { get; set; }

        [Display(ResourceType = typeof(Resources.Common), Name = "CapacityFactor")]
        public decimal? CapacityFactor { get; set; }

        [Display(ResourceType = typeof(Resources.Common), Name = "SPPStatus")]
        public SPPStatus SPPStatus { get; set; }

        [Display(ResourceType = typeof(Resources.Common), Name = "SPPStatus")]
        public int? SPPStatusId { get; set; }

        [Display(ResourceType = typeof(Resources.Common), Name = "SPPPurpose")]
        public SPPPurpose SPPPurpose { get; set; }

        [Display(ResourceType = typeof(Resources.Common), Name = "SPPPurpose")]
        public int? SPPPurposeId { get; set; }

        [Display(ResourceType = typeof(Resources.Common), Name = "PanelOrientation")]
        public PanelOrientation PanelOrientation { get; set; }

        [Display(ResourceType = typeof(Resources.Common), Name = "PanelOrientation")]
        public int? PanelOrientationId { get; set; }

        [Display(ResourceType = typeof(Resources.Common), Name = "Photo")]
        public byte[] Photo { get; set; }

        [Display(ResourceType = typeof(Resources.Common), Name = "Coordinates")]
        public string Coordinates { get; set; }
    }
}