using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TennisNetwork.Models
{
    public class Event
    {
        private ICollection<ApplicationUser> users;

        public Event()
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
        public DateTime StartDateTime { get; set; }

        [Required]
        public DateTime EndDateTime { get; set; }

        public EventType EventType { get; set; }

        [Required]
        public string AuthorId { get; set; }

        [Required]
        public bool Taken { get; set; }
    }
}
