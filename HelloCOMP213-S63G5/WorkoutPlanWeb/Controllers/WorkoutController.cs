using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WorkoutPlanWeb.Controllers
{
    public class WorkoutController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddNewPlan()
        {            
            return View();
        }

    }
}
