using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TennisNetwork.Models;
using Microsoft.AspNet.Identity;
using System.Net;
using System.Data.Entity;
using TennisNetwork.Data;

namespace TennisNetwork.Controllers
{
    public class EventsController : AuthorizeController
    {
        public EventsController()
            : this(new UowData())
        {

        }
        
        public EventsController(IUowData data)
            : base(data)
        {

        }

        // GET: Events
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult UserCalendar(int? pageNumber, string selectedUserId)
        {
            var page = pageNumber.GetValueOrDefault(1);

            var userId = string.IsNullOrWhiteSpace(selectedUserId) ? this.UserId : selectedUserId;
            var events = this.Data.Events.All()
                                         .Where(e => e.Users.Any(u => u.Id == userId))
                                         .Select(CreateEventViewModel.FromEvent)
                                         .OrderByDescending(e => e.StartDate)
                                         .Skip((page - 1) * EventsPageSize).Take(EventsPageSize).ToList();

            ViewBag.PageNumber = page;
            ViewBag.Pages = Math.Ceiling((double)this.Data.Events.All().Where(e => e.Users.Any(u => u.Id == userId)).Count() / EventsPageSize);
            ViewBag.Events = this.Data.Events.All().Where(e => e.Users.Any(u => u.Id == userId)).Count();
            
            return PartialView("UserCalendar", events.ToList());
        }

        public ActionResult Create(int? pageNumber)
        {
            ViewBag.UserId = this.UserId;
            ViewBag.PageNumber = pageNumber;

            return PartialView("_CreateEvent");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateEventViewModel model)
        {
            if (ModelState.IsValid)
            {
                var ev = CreateEventViewModel.ToEvent(model, this);
                this.Data.Events.Add(ev);
                this.Data.SaveChanges();
                
                string url = Url.Action("UserCalendar", "Events");
                return Json(new { success = true, url = url });
            }

            return PartialView("_CreateEvent", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Join(int eventId)
        {
            var ev = this.Data.Events.GetById(eventId);
            if (ev.Taken)
            {
                return Json(new { taken = true });
            }
            else
            {
                //ev.Taken = true;
                var eventType = Activator.CreateInstance(null, "TennisNetwork.Models." + ev.EventType.ToString());
                var manager = new EventManager(this.Data, eventId, (IEvent)eventType.Unwrap());
                ev.Taken = manager.IsEventTaken();

                ev.Users.Add(this.Data.Users.All().First(u => u.Id == this.UserId));
                this.Data.SaveChanges();

                string url = Url.Action("Index", "Events");
                return Json(new { taken = false, url = url });
            }
        }

        public ActionResult Edit(int? id, int? pageNumber)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var ev = GetAllEvents().Where(e => e.Id == id).Select(CreateEventViewModel.FromEvent).FirstOrDefault();
            if (ev == null)
            {
                return HttpNotFound();
            }

            ViewBag.PageNumber = pageNumber;

            return PartialView("_EditEvent", ev);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(CreateEventViewModel model, int? pageNumber)
        {
            if (ModelState.IsValid)
            {
                //var ev = this.Data.Events.GetById(model.Id);
                var ev = this.Data.Events.All().Where(e => e.Id == model.Id && e.AuthorId == this.UserId).FirstOrDefault();
                ev.EndDateTime = new DateTime(model.EndDate.Value.Year,
                                              model.EndDate.Value.Month,
                                              model.EndDate.Value.Day,
                                              model.EndTime.Hour,
                                              model.EndTime.Minute, 
                                              0);
                ev.EventType = model.EventType;
                ev.StartDateTime = new DateTime(model.StartDate.Value.Year,
                                              model.StartDate.Value.Month,
                                              model.StartDate.Value.Day,
                                              model.StartTime.Hour,
                                              model.StartTime.Minute, 
                                              0);

                this.Data.Events.Update(ev);
                await this.Data.SaveChangesAsync();

                string url = Url.Action("UserCalendar", "Events", new { pageNumber = pageNumber });
                return Json(new { success = true, url = url });
            }

            return PartialView("_EditEvent", model);
        }

        public ActionResult Delete(int? id, int? pageNumber)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var ev = GetAllEvents().Where(e => e.Id == id).Select(CreateEventViewModel.FromEvent).FirstOrDefault();
            if (ev == null)
            {
                return HttpNotFound();
            }

            ViewBag.PageNumber = pageNumber;

            return PartialView("_DeleteEvent", ev);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, int pageNumber)
        {
            var userId = this.UserId;
            
            var ev = this.Data.Events.All().Where(e => e.Id == id && e.Users.Any(u => u.Id == userId)).FirstOrDefault();
            if (ev == null)
            {
                return HttpNotFound();
            }

            var user = this.Data.Users.All().Where(u => u.Id == userId).FirstOrDefault();
            ev.Users.Remove(user);
            this.Data.SaveChanges();

            string url = Url.Action("UserCalendar", "Events", new { pageNumber = pageNumber });
            return Json(new { success = true, url = url });
        }

        private IQueryable<Event> GetAllEvents()
        {
            return this.Data.Events.All();
        }
    }
}