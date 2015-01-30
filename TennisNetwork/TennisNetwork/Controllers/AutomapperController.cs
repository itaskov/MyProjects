using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TennisNetwork.Models;
using AutoMapper.QueryableExtensions;

namespace TennisNetwork.Controllers
{
    public class AutomapperController : BaseController
    {
        public ActionResult Index()
        {
            var events = this.Data.Events.All().Project().To<AutomapperEventViewModel>();

            return View(events.ToList());
        }
    }
}