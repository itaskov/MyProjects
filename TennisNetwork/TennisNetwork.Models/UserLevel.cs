using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace TennisNetwork.Models
{
    public class UserLevel
    {
        private ICollection<ApplicationUser> users;

        public UserLevel()
        {
            this.users = new HashSet<ApplicationUser>();
        }

        public virtual ICollection<ApplicationUser> Users
        {
            get { return users; }
            set { users = value; }
        }
        
        [Key]
        public int Id { get; set; }

        [Required]
        public string Level { get; set; }

        [MaxLength]
        public string Description { get; set; }
    }
}
