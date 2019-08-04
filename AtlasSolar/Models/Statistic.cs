using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AtlasSolar.Models
{
    [Table("Statistics", Schema = "public")]
    public class Statistic
    {
        public int Id { get; set; }

        [Display(ResourceType = typeof(Resources.Common), Name = "Time")]
        public DateTime DateTime { get; set; }

        [Display(ResourceType = typeof(Resources.Common), Name = "Source")]
        public MeteoDataSource MeteoDataSource { get; set; }

        [Display(ResourceType = typeof(Resources.Common), Name = "Source")]
        public int MeteoDataSourceId { get; set; }

        [Display(ResourceType = typeof(Resources.Common), Name = "Periodicity")]
        public MeteoDataPeriodicity MeteoDataPeriodicity { get; set; }

        [Display(ResourceType = typeof(Resources.Common), Name = "Periodicity")]
        public int MeteoDataPeriodicityId { get; set; }

        [Display(ResourceType = typeof(Resources.Common), Name = "MeteoDataTypesCount")]
        public int MeteoDataTypesCount { get; set; }

        [Display(ResourceType = typeof(Resources.Common), Name = "DataCount")]
        public long DataCount { get; set; }
    }
}