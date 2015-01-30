using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TennisNetwork.Controllers
{
    [Authorize(Roles = "Administrators")]
    public class AdminController : BaseController
    {
        
    }
}