using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AtlasSolar.Models
{
    [Table("Feedbacks", Schema = "public")]
    public class Feedback
    {
        public int Id { get; set; }

        [Display(ResourceType = typeof(Resources.Common), Name = "DateTime")]
        public DateTime DateTime { get; set; }

        [Display(ResourceType = typeof(Resources.Common), Name = "YourName")]
        public string Name { get; set; }

        [Display(ResourceType = typeof(Resources.Common), Name = "Email")]
        public string Email { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(ResourceType = typeof(Resources.Common), Name = "Text")]
        public string Text { get; set; }
    }
}