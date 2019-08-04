using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AtlasSolar.Models
{
    public class UserViewModel
    {
        public string Id { get; set; }

        [EmailAddress(ErrorMessageResourceType = typeof(Resources.Common), ErrorMessageResourceName = "TheFieldIsNotAValidEmailAddress")]
        [Display(ResourceType = typeof(Resources.Common), Name = "Email")]
        public string Email { get; set; }

        [Display(ResourceType = typeof(Resources.Common), Name = "Roles")]
        public IList<string> Roles { get; set; }
    }
}