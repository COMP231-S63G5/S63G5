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
        

        [HttpGet]
        public ActionResult ViewWorkOutPlan(int id) {

            WorkOutPlan_BLL workoutbll = new WorkOutPlan_BLL();

            WorkoutPlan wp = workoutbll.getWorkPlanDetails(id);
            return View(wp);
        }
        /*
        [HttpGet]
        public ActionResult AddNewPlan()
        {
            Strokes_BLL strokes = new Strokes_BLL();
            ViewBag.strokes = strokes.getStrokes();
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
        */
        [HttpGet]
        public ActionResult AddNewWorkoutPlan()
        {
            if (Session["WorkoutSetList"] == null)
            { 
                Session["WorkoutSetList"] = new List<WorkoutSet>();
            }
            Strokes_BLL strokes = new Strokes_BLL();
            ViewBag.strokes = strokes.getStrokes();
            return View();
        }

        public ActionResult PrintWorkoutPlan(int planId)
        {
            WorkOutPlan_BLL workoutbll = new WorkOutPlan_BLL();
            WorkoutPlan workoutPlan = new WorkoutPlan();
            workoutPlan = workoutbll.getWorkPlanDetails(planId);   

            if (workoutPlan != null)
            {
                PrintToPDF printToPdf = new PrintToPDF();
                printToPdf.PrintWorkoutPlan(workoutPlan);
            }
            else
            {
                //TO-DO: return error
            }
            

            return View();
        }

        public ActionResult EditWorkoutPlan(int planId)
        {
            WorkOutPlan_BLL workoutbll = new WorkOutPlan_BLL();
            WorkoutPlan workoutPlan = workoutbll.getWorkPlanDetails(planId);
            Session["WorkoutSetList"] = workoutPlan.WorkoutSet;
            Session["WorkoutPlanId"] = planId;
            return View(workoutPlan);
        }


        public ActionResult SaveWorkoutPlan()
        {
            WorkOutPlan_BLL plan_dll = new WorkOutPlan_BLL();
            WorkoutPlan workoutPlan = new WorkoutPlan();
            workoutPlan.WorkoutSet = Session["WorkoutSetList"] as List<WorkoutSet>;
            workoutPlan.Date = Session["WorkoutPlanDate"] as String;
            plan_dll.insertWorkoutPlan(workoutPlan);
            Session["WorkoutSetList"] = null;  
            
            
            return JavaScript(String.Format("window.location = '{0}'", Url.Action("Index","Home")));
        }

        public ActionResult UpdateWorkoutPlan()
        {
            WorkOutPlan_BLL plan_dll = new WorkOutPlan_BLL();
            WorkoutPlan workoutPlan = new WorkoutPlan();
            workoutPlan.WorkoutSet = Session["WorkoutSetList"] as List<WorkoutSet>;
            workoutPlan.Date = Session["WorkoutPlanDate"] as String;
            workoutPlan.ID = (int)Session["WorkoutPlanId"];

            
            if (plan_dll.deleteWorkoutPlan(workoutPlan.ID)) 
            {
                plan_dll.insertWorkoutPlan(workoutPlan);
                Session["WorkoutSetList"] = null;   
            }
            
            return JavaScript(String.Format("window.location = '{0}'", Url.Action("Index", "Home")));
        }

        public ActionResult workoutAction(WorkoutPlan workoutPlan, string WorkoutPlanDate,int Id = -1,string command = "")
        {
            if (Session["strokes"] == null)
            {
                Strokes_BLL strokes = new Strokes_BLL();
                Session["strokes"] = strokes.getStrokes();          
            }
                     
            Session["WorkoutPlanDate"] = WorkoutPlanDate;
            List<WorkoutSet> _workoutSets = Session["WorkoutSetList"] as List<WorkoutSet>;
            _workoutSets = workoutPlan.WorkoutSet;
            Session["WorkoutSetList"] = _workoutSets;
            if (command == "Save Plan")
            {           
                return RedirectToAction("SaveWorkoutPlan","Workout");
            }
            else if(command == "Create New Set")
            {
                return RedirectToAction("createSet", "Workout");
            }
            else if(command == "delete")
            {
                return RedirectToAction("deleteSet", "Workout", new { index = Id});
            }
            else if (command == "Print Plan")
            {
                return RedirectToAction("PrintWorkoutPlan", "Workout", new { planId = Id });
            }
            else if (command == "Edit Plan")
            {
                return RedirectToAction("EditWorkoutPlan", "Workout", new { planId = Id });
            }
            else if (command == "Update Plan")
            {
                return RedirectToAction("UpdateWorkoutPlan", "Workout"); 
            }
            else
            {
                return View(); //TO-DO: return error message
            }
        }
        public PartialViewResult createSet()
        {
             List<WorkoutSet> _workoutSets = Session["WorkoutSetList"] as List<WorkoutSet>;
             _workoutSets.Add(new WorkoutSet(_workoutSets.Count + 1));
             Session["WorkoutSetList"] = _workoutSets;

             return PartialView("WorkoutSetList", _workoutSets);   
        }
        



        [HttpPost]
        public ActionResult editSet(FormCollection form)  //Code not relevant now, as now there's a better way to update the model. 
        {
            Strokes_BLL strokes = new Strokes_BLL();
            WorkoutSet workoutSet = new WorkoutSet();
            
            workoutSet.Repeats = int.Parse(form["item.Repeats"]);
            workoutSet.ID = int.Parse(form["item.ID"]); 
            workoutSet.WorkoutSetDistance = int.Parse(form["item.WorkoutSetDistance"]);
            workoutSet.SingleDuration = int.Parse(form["item.SingleDuration"]);
            workoutSet.OrderNum = int.Parse(form["item.OrderNum"]);
            workoutSet.PaceTime = int.Parse(form["item.PaceTime"]);
            workoutSet.Description = form["item.Description"];
            workoutSet.Stroke = strokes.getStrokes(int.Parse(form["strokeSelect"]));

            List<WorkoutSet> _workoutSets = Session["WorkoutSetList"] as List<WorkoutSet>;
            _workoutSets[workoutSet.OrderNum - 1] = workoutSet;

           // _workoutSets.Add(new WorkoutSet(_workoutSets.Count + 1));

            Session["WorkoutSetList"] = _workoutSets;

            return Json(new { success = true });
        }

        // Function to remove a set from the WorkoutSetList Session Variable
        
        public PartialViewResult deleteSet(int index)
        {
            
         //   index = index - 1;
            List<WorkoutSet> _workoutSets = Session["WorkoutSetList"] as List<WorkoutSet>;

            _workoutSets.RemoveAt(index - 1);

            // This will reorder the workout sets in the list
            for (int i = index - 1; i < _workoutSets.Count; i++)
            {
                _workoutSets[i].OrderNum = i + 1;
            }
            
            Session["WorkoutSetList"] = _workoutSets;

            //Strokes_BLL strokes = new Strokes_BLL();
            //Session["strokes"] = strokes.getStrokes();  TODO- Look into strokes
            
            return PartialView("WorkoutSetList", _workoutSets);
        }


    }
}
