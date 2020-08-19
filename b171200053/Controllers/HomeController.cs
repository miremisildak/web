using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace b171200053.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            TheStory.BusinessLayer.Test test = new TheStory.BusinessLayer.Test();

            return View();
        }
    }
}