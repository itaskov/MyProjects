using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace TennisNetwork.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        private ICollection<Event> events;

        private ICollection<Address> addresses;
        
        public ApplicationUser()
        {
            this.events = new HashSet<Event>();
            this.addresses = new HashSet<Address>();
        }

        public virtual ICollection<Event> Events
        {
            get { return events; }
            set { events = value; }
        }
        public virtual ICollection<Address> Addresses
        {
            get { return addresses; }
            set { addresses = value; }
        }

        [Required]
        public int UserLevelId { get; set; }

        public virtual UserLevel Level { get; set; }

        [Required]
        public Gender Gender { get; set; }

        [Required]
        [MaxLength(20)]
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string ImageURL { get; set; }
    }
}
