using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AtlasSolar.Models
{
    [Table("MeteoData", Schema = "public")]
    public class MeteoData
    {
        public long Id { get; set; }

        [Display(ResourceType = typeof(Resources.Common), Name = "MeteoDataType")]
        public MeteoDataType MeteoDataType { get; set; }

        [Display(ResourceType = typeof(Resources.Common), Name = "MeteoDataType")]
        public int MeteoDataTypeId { get; set; }

        [Display(ResourceType = typeof(Resources.Common), Name = "Year")]
        public int? Year { get; set; }

        [Display(ResourceType = typeof(Resources.Common), Name = "Month")]
        public int? Month { get; set; }

        [Display(ResourceType = typeof(Resources.Common), Name = "Day")]
        public int? Day { get; set; }

        //[Display(ResourceType = typeof(Resources.Common), Name = "Hour")]
        //public int? Hour { get; set; }

        [Display(ResourceType = typeof(Resources.Common), Name = "Longitude")]
        public decimal Longitude { get; set; }

        [Display(ResourceType = typeof(Resources.Common), Name = "Latitude")]
        public decimal Latitude { get; set; }

        [Display(ResourceType = typeof(Resources.Common), Name = "Value")]
        public decimal? Value { get; set; }
    }
}