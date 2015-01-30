using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace TennisNetwork.Models
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
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class ChangePasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
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

    public class ChangeAddressViewModel
    {
        public static Expression<Func<Address, ChangeAddressViewModel>> FromAddress
        {
            get
            {
                return address => new ChangeAddressViewModel
                {
                    Id = address.Id,
                    Town = address.Town,
                    State = address.State,
                    Country = address.Country
                };
            }
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "The field '{0}' should be between {2} and {1} characters.")]
        public string Town { get; set; }

        [MaxLength(20)]
        public string State { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "The field '{0}' should be between {2} and {1} characters.")]
        public string Country { get; set; }
    }

    public class ChangeUserDataViewModel
    {
        public static Expression<Func<ApplicationUser, ChangeUserDataViewModel>> FromUser
        {
            get
            {
                return user => new ChangeUserDataViewModel
                {
                    Id = user.Id,
                    Email = user.Email,
                    UserLevelId = user.UserLevelId,
                    Gender = user.Gender,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    ImageURL = user.ImageURL
                };
            }
        }

        [Required]
        public string Id { get; set; }
        
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Level")]
        public int UserLevelId { get; set; }

        public virtual UserLevel Level { get; set; }

        [Required]
        [EnumShouldNotContainEmptyString]
        public Gender Gender { get; set; }

        [Required]
        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Display(Name = "Photo")]
        public string ImageURL { get; set; }
    }
}