using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TennisNetwork.Data;
using TennisNetwork.Models;
using Microsoft.AspNet.Identity;

namespace TennisNetwork.Controllers
{
    public class BaseController : Controller
    {
        public const string VIRTUAL_PATH = "~/Photos/";

        public const int EventsPageSize = 4;

        public string UserId
        {
            get
            {
                string userId;
                try
                {
                    userId = User.Identity.GetUserId();
                }
                catch (NullReferenceException)
                {
                    userId = null;
                }

                return userId;
            }
        }

        public IUowData Data { get; private set; }

        public BaseController()
            : this(new UowData())
        {

        }

        public BaseController(IUowData data)
        {
            this.Data = data;
        }
    }
}