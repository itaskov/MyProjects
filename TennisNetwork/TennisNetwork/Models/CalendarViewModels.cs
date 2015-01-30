using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using TennisNetwork.Controllers;

namespace TennisNetwork.Models
{
    public class CreateEventViewModel
    {
        public static Expression<Func<Event, CreateEventViewModel>> FromEvent
        {
            get
            {
                return ev => new CreateEventViewModel
                {
                    EndDate = ev.EndDateTime,
                    EndTime = ev.EndDateTime,
                    EventType = ev.EventType,
                    Id = ev.Id,
                    StartDate = ev.StartDateTime,
                    StartTime = ev.StartDateTime,
                    UserId = ev.AuthorId
                };
            }
        }

        public static Event ToEvent(CreateEventViewModel model, BaseController controller)
        {
            return new Event
            {
                AuthorId = model.UserId,
                EndDateTime = new DateTime(model.EndDate.Value.Year,
                                           model.EndDate.Value.Month,
                                           model.EndDate.Value.Day,
                                           model.EndTime.Hour,
                                           model.EndTime.Minute,
                                           second: 0),
                EventType = model.EventType,
                StartDateTime = new DateTime(model.StartDate.Value.Year,
                                           model.StartDate.Value.Month,
                                           model.StartDate.Value.Day,
                                           model.StartTime.Hour,
                                           model.StartTime.Minute,
                                           second: 0),
                Users = new List<ApplicationUser> { controller.Data.Users.All().Where(u => u.Id == controller.UserId).First() }
            };
        }
        
        [Required]
        public int Id { get; set; }
        
        [Display(Name = "Event")]
        public EventType EventType { get; set; }

        [Required]
        [Display(Name = "Start date")]
        [DataType(DataType.Date)]
        public DateTime? StartDate { get; set; }

        [Required]
        [Display(Name = "End date")]
        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }

        [Required]
        [DataType(DataType.Time)]
        [Display(Name = "Start time")]
        //[DisplayFormat(DataFormatString = "{0:HH\\:mm}")]
        public DateTime StartTime { get; set; }

        [Required]
        [DataType(DataType.Time)]
        [Display(Name = "End time")]
        //[DisplayFormat(DataFormatString ="{0:HH\\:mm}")]
        public DateTime EndTime { get; set; }

        [Required]
        [ScaffoldColumn(false)]
        public string UserId { get; set; }
    }
}