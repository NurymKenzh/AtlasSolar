using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AtlasSolar.Models
{
    [Table("Options", Schema = "public")]
    public class Option
    {
        public int Id { get; set; }

        [Display(ResourceType = typeof(Resources.Common), Name = "Code")]
        public string Code { get; set; }

        [Display(ResourceType = typeof(Resources.Common), Name = "DescriptionKZ")]
        public string DescriptionKZ { get; set; }

        [Display(ResourceType = typeof(Resources.Common), Name = "DescriptionRU")]
        public string DescriptionRU { get; set; }

        [Display(ResourceType = typeof(Resources.Common), Name = "Value")]
        public string Value { get; set; }
    }
}