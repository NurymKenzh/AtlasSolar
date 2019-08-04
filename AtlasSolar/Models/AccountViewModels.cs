using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AtlasSolar.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(ResourceType = typeof(Resources.Common), Name = "Email")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(ResourceType = typeof(Resources.Common), Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(ResourceType = typeof(Resources.Common), Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(Resources.Common), Name = "Password")]
        public string Password { get; set; }
        
        [Display(ResourceType = typeof(Resources.Common), Name = "RememberMe")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(ResourceType = typeof(Resources.Common), Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessageResourceType = typeof(Resources.Errors), ErrorMessageResourceName = "TheMustBeAtLeastCharactersLong", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(Resources.Common), Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(Resources.Common), Name = "ConfirmPassword")]
        [Compare("Password", ErrorMessageResourceType = typeof(Resources.Errors), ErrorMessageResourceName = "ThePasswordAndConfirmationPasswordDoNotMatch")]
        public string ConfirmPassword { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(ResourceType = typeof(Resources.Common), Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessageResourceType = typeof(Resources.Errors), ErrorMessageResourceName = "TheMustBeAtLeastCharactersLong", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(Resources.Common), Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(Resources.Common), Name = "ConfirmPassword")]
        [Compare("Password", ErrorMessageResourceType = typeof(Resources.Errors), ErrorMessageResourceName = "ThePasswordAndConfirmationPasswordDoNotMatch")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(ResourceType = typeof(Resources.Common), Name = "Email")]
        public string Email { get; set; }
    }
}
