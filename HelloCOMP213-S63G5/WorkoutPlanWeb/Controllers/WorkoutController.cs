﻿using System;
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
            List<Dictionary<string, string>> listOfSets = workoutbll.getWorkPlanDetails(id);

            WorkoutPlan wp1 = new WorkoutPlan();
            if (listOfSets.Count > 0)
            {
                wp1.Date = listOfSets[0]["planDate"];
                wp1.ID = Convert.ToInt32(listOfSets[0]["WorkOutPlan_ID"]);
            }
               List<WorkoutSet> wp1_sets=new List<WorkoutSet>();

                foreach(Dictionary<string,string> set in listOfSets){
                    WorkoutSet setObj = new WorkoutSet();
                    setObj.ID = Convert.ToInt32(set["Set_Id"]);
                    setObj.OrderNum = Convert.ToInt32(set["memberOrder"]);
                    setObj.PaceTime = Convert.ToInt32(set["paceTime"]);
                    setObj.Repeats = Convert.ToInt32(set["repeats"]);
                    setObj.RestPeriod = Convert.ToInt32(set["restPeriod"]);
                    setObj.Description = set["Set_Desc"];
                    setObj.WorkoutSetDistance = Convert.ToInt32(set["distance"]);

                    int strokeID = Convert.ToInt32(set["Stroke_Id"]);
                    string stroke_name = set["Name"];
                    string stroke_desc = set["Stroke_Desc"];
                    WorkoutStroke stroke = new WorkoutStroke(strokeID,stroke_name,stroke_desc);

                    setObj.Stroke = stroke;

                    wp1_sets.Add(setObj);
                }

                wp1.WorkoutSet = wp1_sets;


            return View(wp1);
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

        public ActionResult PrintWorkoutPlan(int id)
        {
            WorkOutPlan_BLL workoutbll = new WorkOutPlan_BLL();
            WorkoutPlan workoutPlan = new WorkoutPlan();
            //workoutPlan = (WorkoutPlan)workoutbll.getWorkPlanDetails(id);       --Will be uncommented once the getWorkPlanDetails returns the correct type(WorkoutPlan).

            if (workoutPlan != null)
            {
                PrintToPDF printToPdf = new PrintToPDF(workoutPlan);
            }
            else
            {
                //TO-DO: return error
            }
            

            return View();
        }


        public ActionResult SaveWorkoutPlan()
        {
            WorkOutPlan_BLL plan_dll = new WorkOutPlan_BLL();
            WorkoutPlan workoutPlan = new WorkoutPlan();
            workoutPlan.WorkoutSet = Session["WorkoutSetList"] as List<WorkoutSet>;
            workoutPlan.Date = Session["WorkoutPlanDate"] as String;
            plan_dll.insertWorkoutPlan(workoutPlan);        
            Session.Clear();   
            
            
            return JavaScript(String.Format("window.location = '{0}'", Url.Action("Index","Home")));
        }

        public ActionResult workoutAction(WorkoutPlan workoutPlan, string WorkoutPlanDate,int deleteId = -1,string command = "")
        {
            Strokes_BLL strokes = new Strokes_BLL();
            Session["strokes"] = strokes.getStrokes();                    
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
                return RedirectToAction("deleteSet", "Workout", new { index = deleteId});
            }
            else
            {
                return View(); //TO-DO: 
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
