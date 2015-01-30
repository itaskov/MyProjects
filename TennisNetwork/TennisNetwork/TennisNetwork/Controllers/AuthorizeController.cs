using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using TennisNetwork.Data;

namespace TennisNetwork.Controllers
{
    [Authorize]
    public class AuthorizeController : BaseController
    {
        public AuthorizeController(IUowData data)
            : base(data)
        {

        }
    }
}