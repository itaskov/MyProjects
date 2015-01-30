using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace TennisNetwork.Models
{
    public class SearchUserViewModel
    {
        [Display(Name = "Level")]
        public int? UserLevelId { get; set; }

        public virtual UserLevel UserLevel { get; set; }
        
        public IEnumerable<SelectListItem> Levels { get; set; }

        public Gender Gender { get; set; }

        [StringLength(20, ErrorMessage = "The {0} must not be more than {1} characters long.")]
        public string Country { get; set; }

        public string Town { get; set; }

        public string State { get; set; }
    }

    public class UserViewModel
    {
        public static Expression<Func<ApplicationUser, UserViewModel>> FromUsers
        {
            get
            {
                return user => new UserViewModel
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Level = user.Level,
                    Country = user.Addresses.FirstOrDefault().Country,
                    Town = user.Addresses.FirstOrDefault().Town
                };
            }
        }
        
        [Key]
        [ScaffoldColumn(false)]
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        [Display(Name = "Level")]
        public int UserLevelId { get; set; }

        public virtual UserLevel Level { get; set; }

        public string Country { get; set; }

        public string Town { get; set; }

        public string State { get; set; }

        /// <summary>
        /// For pagination.
        /// </summary>
        [ScaffoldColumn(false)]
        public SearchUserViewModel SearchUserViewModel;
    }

    public class SearchResultViewModel
    {
        public SearchUserViewModel SearchUserViewModel { get; set; }

        public IEnumerable<UserViewModel> UserViewModel { get; set; }
    }
}