using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace AtlasSolar.Models
{
    public class IndexViewModel
    {
        public bool HasPassword { get; set; }
        public IList<UserLoginInfo> Logins { get; set; }
        public string PhoneNumber { get; set; }
        public bool TwoFactor { get; set; }
        public bool BrowserRemembered { get; set; }
    }

    public class ManageLoginsViewModel
    {
        public IList<UserLoginInfo> CurrentLogins { get; set; }
        public IList<AuthenticationDescription> OtherLogins { get; set; }
    }

    public class FactorViewModel
    {
        public string Purpose { get; set; }
    }

    public class SetPasswordViewModel
    {
        [Required]
        [StringLength(100, ErrorMessageResourceType = typeof(Resources.Errors), ErrorMessageResourceName = "TheMustBeAtLeastCharactersLong", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(Resources.Common), Name = "NewPassword")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(Resources.Common), Name = "ConfirmNewPassword")]
        [Compare("NewPassword", ErrorMessageResourceType = typeof(Resources.Errors), ErrorMessageResourceName = "TheNewPasswordAndConfirmationPasswordDoNotMatch")]
        public string ConfirmPassword { get; set; }
    }

    public class ChangePasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(Resources.Common), Name = "CurrentPassword")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessageResourceType = typeof(Resources.Errors), ErrorMessageResourceName = "TheMustBeAtLeastCharactersLong", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(Resources.Common), Name = "NewPassword")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(Resources.Common), Name = "ConfirmNewPassword")]
        [Compare("NewPassword", ErrorMessageResourceType = typeof(Resources.Errors), ErrorMessageResourceName = "TheNewPasswordAndConfirmationPasswordDoNotMatch")]
        public string ConfirmPassword { get; set; }
    }

    public class AddPhoneNumberViewModel
    {
        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string Number { get; set; }
    }

    public class VerifyPhoneNumberViewModel
    {
        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
    }

    public class ConfigureTwoFactorViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
    }
}