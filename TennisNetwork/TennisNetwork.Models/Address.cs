using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TennisNetwork.Models
{
    public class Address
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string Town { get; set; }

        [MaxLength(20)]
        public string State { get; set; }

        [Required]
        [StringLength(20)]
        public string Country { get; set; }

        //[Required] If enable require to create AddressViewModel, because ModelSate error for userId is generated in '/Account/Register' before the user is created!
        [Display(Name = "User")]
        public string ApplicationUserId { get; set; }

        public ApplicationUser User { get; set; }
    }
}
