using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WorkoutPlanAPI;

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
            Strokes_BLL strokes = new Strokes_BLL();
            ViewBag.strokes = strokes.getStrokeNames();
            return View();
        }

        public ActionResult ViewWorkoutPlan()
        {
            return View();
        }

    }
}
