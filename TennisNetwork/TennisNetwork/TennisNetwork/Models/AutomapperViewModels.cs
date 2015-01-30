using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using TennisNetwork.Controllers;

namespace TennisNetwork.Models
{
    public class AutomapperEventViewModel
    {
        //[Key]
        //public int Id { get; set; }

        [Required]
        public DateTime StartDateTime { get; set; }

        [Required]
        public DateTime EndDateTime { get; set; }

        public DateTime EndDate { get; set; }

        public EventType EventType { get; set; }

        [Required]
        public string AuthorId { get; set; }

        [Required]
        public bool Taken { get; set; }
    }
}