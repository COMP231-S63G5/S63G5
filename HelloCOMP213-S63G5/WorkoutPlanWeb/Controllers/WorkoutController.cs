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
            //TO-DO: Need to save the workoutplan to db.

            return RedirectToAction("Index","Home"); //TO-DO: Need to redirect to view workout plan page. Currently redirecting to the Home page after submit.
        }

        public ActionResult ViewWorkoutPlan()
        {
            return View();
        }

        public ActionResult AddNewWorkoutPlan()
        {
            if (Session["WorkoutSetList"] == null)
            { 
                Session["WorkoutSetList"] = new List<WorkoutSet>();
            }
            return View();
        }


        public PartialViewResult createSet()
        {
            
            List<WorkoutSet> _workoutSets = Session["WorkoutSetList"] as List<WorkoutSet>;
            _workoutSets.Add(new WorkoutSet(_workoutSets.Count + 1));

            Session["WorkoutSetList"] = _workoutSets;

            return PartialView("WorkoutSetList", _workoutSets);
        }

        [HttpPost]
        public ActionResult editSet(FormCollection form)
        {

            WorkoutSet workoutSet = new WorkoutSet();
            
            workoutSet.Repeats = int.Parse(form["item.Repeats"]);
            workoutSet.ID = int.Parse(form["item.ID"]); 
            workoutSet.WorkoutSetDistance = int.Parse(form["item.WorkoutSetDistance"]);
            workoutSet.SingleDuration = int.Parse(form["item.SingleDuration"]);
            workoutSet.OrderNum = int.Parse(form["item.OrderNum"]);
            workoutSet.PaceTime = int.Parse(form["item.PaceTime"]);
            workoutSet.Description = form["item.Description"];

            List<WorkoutSet> _workoutSets = Session["WorkoutSetList"] as List<WorkoutSet>;
            _workoutSets[workoutSet.OrderNum - 1] = workoutSet;

           // _workoutSets.Add(new WorkoutSet(_workoutSets.Count + 1));

            Session["WorkoutSetList"] = _workoutSets;

            return Json(new { success = true });
        }

        public ActionResult Partial1()
        {
            return View();
        }

    }
}
