using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WorkoutPlanAPI;
using WorkoutPlanObjects;

namespace WorkoutPlanWeb.Controllers
{
    public class WorkoutController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public ActionResult AddNewPlan()
        {
            Strokes_BLL strokes = new Strokes_BLL();
            ViewBag.strokes = strokes.getStrokeNames();
            WorkoutPlan workoutPlan = new WorkoutPlan();
            workoutPlan.WorkoutSet = new List<WorkoutSet>();
            return View(workoutPlan);
        }

        [HttpPost]
        public ActionResult AddNewPlan(WorkoutPlan workoutPlan)
        {
            return View();
        }


    }
}
