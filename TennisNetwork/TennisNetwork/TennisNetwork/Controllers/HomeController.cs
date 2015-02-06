using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TennisNetwork.Models;
using System.Data.Entity;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using TennisNetwork.Data;
using Microsoft.Web.Mvc;

namespace TennisNetwork.Controllers
{
    public class HomeController : BaseController
    {
        public const int UsersPageSize = 5;

        public HomeController(IUowData data)
            : base(data)
        {

        }

        public ActionResult Index()
        {
            ViewBag.UserLevelId = new SelectList(this.Data.UserLevels.All().ToList(), "Id", "Level");

            return View();
        }

        public ActionResult Search(SearchUserViewModel model, int? pageNumber)
        {
            var page = pageNumber.GetValueOrDefault(1);
            ViewBag.UserLevelId = new SelectList(this.Data.UserLevels.All().ToList(), "Id", "Level", model.UserLevelId);
            ViewBag.SelectedPage = page;

            if (ModelState.IsValid)
            {
                var currenlyLoggedUser = string.IsNullOrEmpty(this.UserId) ? 0 : 1;
                
                var users = this.GetUsers(model, page).Select(UserViewModel.FromUsers).ToList();
                var usersCount = this.GetUsers(model, 1, int.MaxValue).Count();
                ViewBag.UserPages = Math.Ceiling((decimal)usersCount / UsersPageSize);

                var searchResultViewModel = new SearchResultViewModel { SearchUserViewModel = model, UserViewModel = users };
                
                return View("SearchResultTennisPlayer", searchResultViewModel);
            }
            
            return View("Index", model);
        }

        [AjaxOnly]
        public ActionResult UserCalendar(int? pageNumber, string selectedUserId)
        {
            var page = pageNumber.GetValueOrDefault(1);


            var userId = selectedUserId;
            var events = this.Data.Events.All()
                                         .Where(e => e.Users.Any(u => u.Id == userId))
                                         .Where(e => !e.Taken)
                                         .Where(e => e.Users.Any(u => u.Id != this.UserId))
                                         .Select(CreateEventViewModel.FromEvent)
                                         .OrderByDescending(e => e.StartDate)
                                         .Skip((page - 1) * EventsPageSize).Take(EventsPageSize);

            ViewBag.PageNumber = page;
            ViewBag.Pages = Math.Ceiling((double)this.Data.Events.All()
                                                                 .Where(e => e.Users.Any(u => u.Id == userId))
                                                                 .Where(e => !e.Taken)
                                                                 .Count() / EventsPageSize);
            ViewBag.Events = this.Data.Events.All()
                                             .Where(e => e.Users.Any(u => u.Id == userId))
                                             .Where(e => !e.Taken)
                                             .Count();

            return PartialView(events.ToList());
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        private IQueryable<ApplicationUser> GetUsers(SearchUserViewModel model, int pageNumber, int usersPageSize = UsersPageSize)
        {
            return this.Data.Users.All()
                                  .Where(u => !model.UserLevelId.HasValue || u.UserLevelId == model.UserLevelId)
                                  .Where(u => model.Gender == 0 || u.Gender == model.Gender)
                                  .Where(u => model.Country == null || u.Addresses.Any(a => a.Country == model.Country))
                                  .Where(u => model.Town == null || u.Addresses.Any(a => a.Town == model.Town))
                                  .Where(u => model.State == null || u.Addresses.Any(a => a.State == model.State))
                                  .Where(u => u.Id != this.UserId)
                                  .OrderBy(u => u.Id)
                                  .Skip((pageNumber - 1) * usersPageSize)
                                  .Take(usersPageSize);
        }
    }
}