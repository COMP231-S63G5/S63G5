using System;
using System.Collections.Generic;
using System.Collections;
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
        public ActionResult ViewWorkOutPlan(int id)
        {

            WorkoutPlanObject wp = WorkOutPlan_BLL.getWorkoutPlan(id);

            return View(wp);
        }

        [HttpGet]
        public ActionResult AddNewWorkoutPlan()
        {
            if (Session["WorkoutSetList"] == null)
            { 
                Session["WorkoutSetList"] = new List<WorkoutSetObject>();
            }

            if (Session["wp"] == null)
            {
                WorkoutPlanObject plan = new WorkoutPlanObject("Swim plan UI", DateTime.Now);

               /* WorkoutSetObject set1 = new WorkoutSetObject("Warm up");
                plan.addWorkoutSection(set1, 0);

                WorkoutSetObject set2 = new WorkoutSetObject(4, 50, "free", "1:00", "description 1", "E1", 200);
                plan.addWorkoutSet(set2, 1, 1);

                WorkoutSetObject set3 = new WorkoutSetObject("Main");
                plan.addWorkoutSection(set3, 2);

                WorkoutSetObject set4 = new WorkoutSetObject(1, 100, "back", "1:30", "description 2", "E2", 100);
                plan.addWorkoutSet(set4, 3, 3);

                WorkoutSetObject set5 = new WorkoutSetObject(4);
                plan.addWorkoutGroup(set5, 3, 4);

                WorkoutSetObject set6 = new WorkoutSetObject(2);
                plan.addWorkoutGroup(set6, 5, 5);

                WorkoutSetObject set7 = new WorkoutSetObject(1, 50, "fly", "1:00", "description 3", "E3", 400);
                plan.addWorkoutSet(set7, 6, 6);

                WorkoutSetObject set8 = new WorkoutSetObject("Warm down");
                plan.addWorkoutSection(set8, 7);

                WorkoutSetObject set9 = new WorkoutSetObject(4, 100, "free", "2:00", "description 4", "S1", 400);
                plan.addWorkoutSet(set9, 8, 8);

                //Console.WriteLine("Printing workout plan object generated from UI.\n"); 
                //Console.WriteLine(plan.getConsoleString());
                //Console.ReadLine();

                List<WorkoutSetObject> setDB = new List<WorkoutSetObject>(){
                    new WorkoutSetObject(14,"Section",1,0,"","","","Warm up","",0,1,0),
                    new WorkoutSetObject(15,"Set",4,50,"Free","1:00","4:00","description 1","E1",200,2,1),
                    new WorkoutSetObject(16,"Section",1,0,"","","","Main","",0,3,0),
                    new WorkoutSetObject(17,"Set",1,100,"Back","1:30","description 2","E2","100",0,4,3),
                    new WorkoutSetObject(18,"Group",4,0,"","","","","",0,5,3),
                    new WorkoutSetObject(19,"Group",2,0,"","","","","",0,6,5),
                    new WorkoutSetObject(20,"Set",1,50,"Fly","1:00","8:00","description 3","E3",400,7,6),
                    new WorkoutSetObject(21,"Section",1,0,"","","","Warm down","",0,8,0),
                    new WorkoutSetObject(22,"Set",4,100,"Free","2:00","8:00","description 4","S1",400,9,8),
                    new WorkoutSetObject(23,"Group",1,0,"","","","","",0,10,8),
                    new WorkoutSetObject(23,"Group",1,0,"","","","","",0,11,10),
                    new WorkoutSetObject(23,"Group",1,0,"","","","","",0,12,10)
                };

                WorkoutPlanObject plan2 = new WorkoutPlanObject(1, "Swim plan DB", DateTime.Now, setDB);*/
                Session["wp"] = plan;

            }
            WorkoutPlanObject wp1 = Session["wp"] as WorkoutPlanObject;
            
            Session["wp"] = wp1;
            
            Session["WorkoutSetList"] = wp1.SubSetList;
            //WorkoutSetObject ws = new WorkoutSetObject("home");
            ViewBag.indent = 0;
            return View(wp1);
        }

        //public ActionResult PrintWorkoutPlan(int planId)
        //{
        //    WorkOutPlan_BLL workoutbll = new WorkOutPlan_BLL();
        //    WorkoutPlan workoutPlan = new WorkoutPlan();
        //    workoutPlan = workoutbll.getWorkPlanDetails(planId);   

        //    if (workoutPlan != null)
        //    {
        //        PrintToPDF printToPdf = new PrintToPDF();
        //        printToPdf.PrintWorkoutPlan(workoutPlan);
        //    }
        //    else
        //    {
        //        //TO-DO: return error
        //    }
            

        //    return View();
        //}

        //public ActionResult EditWorkoutPlan(int planId)
        //{
        //    WorkOutPlan_BLL workoutbll = new WorkOutPlan_BLL();
        //    WorkoutPlan workoutPlan = workoutbll.getWorkPlanDetails(planId);
        //    Session["WorkoutSetList"] = workoutPlan.WorkoutSet;
        //    Session["WorkoutPlanId"] = planId;
        //    return View(workoutPlan);
        //}


        //public ActionResult SaveWorkoutPlan()
        //{
        //    WorkOutPlan_BLL plan_dll = new WorkOutPlan_BLL();
        //    WorkoutPlan workoutPlan = new WorkoutPlan();
        //    workoutPlan.WorkoutSet = Session["WorkoutSetList"] as List<WorkoutSet>;
        //    workoutPlan.Date = Session["WorkoutPlanDate"] as String;
        //    plan_dll.insertWorkoutPlan(workoutPlan);
        //    Session["WorkoutSetList"] = null;  
            
            
        //    return JavaScript(String.Format("window.location = '{0}'", Url.Action("Index","Home")));
        //}

        //public ActionResult UpdateWorkoutPlan()
        //{
        //    WorkOutPlan_BLL plan_dll = new WorkOutPlan_BLL();
        //    WorkoutPlan workoutPlan = new WorkoutPlan();
        //    workoutPlan.WorkoutSet = Session["WorkoutSetList"] as List<WorkoutSet>;
        //    workoutPlan.Date = Session["WorkoutPlanDate"] as String;
        //    workoutPlan.ID = (int)Session["WorkoutPlanId"];

            
        //    if (plan_dll.deleteWorkoutPlan(workoutPlan.ID)) 
        //    {
        //        plan_dll.insertWorkoutPlan(workoutPlan);
        //        Session["WorkoutSetList"] = null;   
        //    }
            
        //    return JavaScript(String.Format("window.location = '{0}'", Url.Action("Index", "Home")));
        //}
        public ActionResult editSection(string sectionName, string command, string orderId)
        {
            WorkoutPlanObject wp = Session["wp"] as WorkoutPlanObject;
            
                WorkoutSetObject ws = (WorkoutSetObject)wp.SubSetHashTable[int.Parse(orderId)];
                if (command == "Update Section")
                {
                    if (sectionName == "")
                    {
                        ModelState.AddModelError("errorMessage", "Section name cannot be empty");
                        ModelState.AddModelError("editSection", "Section name cannot be empty");
                    }
                    else
                    {
                        ws.Description = sectionName;
                        Session["wp"] = wp;
                        Session["WorkoutSetList"] = wp.SubSetList;
                    }

                }
                else if (command == "Delete Section")
                {
                    if (ws.SubSetList.Count > 0) //a count of more than 0 indicates the section is not empty. 
                    {
                        ModelState.AddModelError("errorMessage", "Section must be empty before it can be deleted ");
                        ModelState.AddModelError("editSection", "Section must be empty before it can be deleted ");
                    }
                    else
                    {
                        wp.remove(int.Parse(orderId));
                        Session["wp"] = wp;
                        Session["WorkoutSetList"] = wp.SubSetList;
                    }
                }
            
            return View("AddNewWorkoutPlan", wp);
        }

        public ActionResult addSection(string sectionName, string selectPosition)
        {
            WorkoutPlanObject wp = Session["wp"] as WorkoutPlanObject;
            int pos;
            if (sectionName == "")
            {
                ModelState.AddModelError("errorMessage", "Section name cannot be empty");
                ModelState.AddModelError("addSectionName", "Section name cannot be empty");
            }
            else if(selectPosition == "")
	        {
                ModelState.AddModelError("errorMessage", "Position cannot be empty");
                ModelState.AddModelError("addSectionPosition", "Position cannot be empty");
	        }
            else if (int.TryParse(selectPosition,out pos) == false)
            {
                ModelState.AddModelError("errorMessage", "Position must be a number");
                ModelState.AddModelError("addSectionPosition", "Position must be a number");
            }
            else
            {
                WorkoutSetObject ws = new WorkoutSetObject(sectionName);
                wp.addWorkoutSection(ws, int.Parse(selectPosition));
                Session["WorkoutSetList"] = wp.SubSetList;
                Session["wp"] = wp;
            }
            return View("AddNewWorkoutPlan", wp);
        }

        public ActionResult addGroup(string repeats, string addGroupParentId, string position)
        {
            WorkoutPlanObject wp = Session["wp"] as WorkoutPlanObject;
            int rep;
            int pos; 
            if (repeats == "")
            {
                ModelState.AddModelError("errorMessage", "Repeats cannot be empty");
                ModelState.AddModelError("addGroupRepeat", "Repeats cannot be empty");
            }
            else if (int.TryParse(repeats,out rep) == false)
            {
                ModelState.AddModelError("errorMessage", "Repeats must be a number");
                ModelState.AddModelError("addGroupRepeat", "Repeats must be a number");
            }
            else if (position=="")
            {
                ModelState.AddModelError("addGroupLocation", "Group position cannot be empty");
                ModelState.AddModelError("errorMessage", "Group position cannot be empty");
            }
            else if (int.TryParse(position, out pos) == false)
            {
                ModelState.AddModelError("addGroupLocation", "Group position must be a number");
                ModelState.AddModelError("errorMessage", "Group position must be a number");
            }
            else
            {
                WorkoutSetObject ws = new WorkoutSetObject(rep);
                wp.addWorkoutGroup(ws, int.Parse(addGroupParentId), pos);
                Session["wp"] = wp;
                Session["WorkoutSetList"] = wp.SubSetList;
            }
            return View("AddNewWorkoutPlan", wp);
        }

        public ActionResult addSet(string repeat, string distance, string stroke,string typeDuration, string type, string duration, string description, string totalDistance, string energyGroup, string energyAmount, string position, string addSetParentId)
        {
            WorkoutPlanObject wp = Session["wp"] as WorkoutPlanObject;
           
            WorkoutSetObject ws = new WorkoutSetObject(int.Parse(repeat), int.Parse(distance), stroke, typeDuration, description, energyGroup, int.Parse(totalDistance));
            if (type == "Rest")
            {
                ws.Rest = typeDuration;
                ws.Pace = "";
            }
            else if (type == "Pace")
            {
                ws.Pace = typeDuration;
                ws.Rest = "";
            }
            else
            {
                ws.Rest = "";
                ws.Pace = "";
            }
            wp.addWorkoutSet(ws, int.Parse(addSetParentId), int.Parse(position));
            Session["wp"] = wp;
            Session["WorkoutSetList"] = wp.SubSetList;

            return View("AddNewWorkoutPlan", wp);
        }

        public ActionResult editGroup(string repeats, string groupOrderId, string command)
        {
            WorkoutPlanObject wp = Session["wp"] as WorkoutPlanObject;
            int rep;

            WorkoutSetObject ws = (WorkoutSetObject)wp.SubSetHashTable[int.Parse(groupOrderId)];
            if (command == "Update Group")
            {
                if (repeats == "")
                {
                    ModelState.AddModelError("errorMessage", "Repeats cannot be empty");
                    ModelState.AddModelError("editGroupRepeat", "Repeats cannot be empty");
                }
                else if (int.TryParse(repeats, out rep) == false)
                {
                    ModelState.AddModelError("errorMessage", "Repeats must be a number");
                    ModelState.AddModelError("editGroupRepeat", "Repeats must be a number");
                }
                else
                {
                    ws.Repeats = rep;
                    Session["wp"] = wp;
                    Session["WorkoutSetList"] = wp.SubSetList;
                }

            }
            else if (command == "Delete Group")
            {
                if (ws.SubSetList.Count > 0) //a count of more than 0 indicates the group is not empty. 
                {
                    ModelState.AddModelError("errorMessage", "Section must be empty before it can be deleted ");
                    ModelState.AddModelError("editGroupRepeat", "Section must be empty before it can be deleted ");
                }
                else
                {
                    wp.remove(int.Parse(groupOrderId));
                    Session["wp"] = wp;
                    Session["WorkoutSetList"] = wp.SubSetList;
                }
            }

            
            return View("AddNewWorkoutPlan", wp);
        }

        public ActionResult editSet(string repeat, string distance, string stroke, string type, string typeDuration, string description, string totalDistance, string energyGroup, string energyAmount, string editSetOrderId,string command)
        {     
            WorkoutPlanObject wp = Session["wp"] as WorkoutPlanObject;
            WorkoutSetObject ws = (WorkoutSetObject)wp.SubSetHashTable[int.Parse(editSetOrderId)];
            if (command == "Delete Set")
            {
                wp.remove(int.Parse(editSetOrderId));
            }
            else
            {
                ws.Repeats = int.Parse(repeat);
                ws.Distance = int.Parse(distance);
                ws.Stroke = stroke;
                if (type == "Pace")
                {
                    ws.Pace = typeDuration;
                    ws.Rest = ""; // Only pace or rest can be in use
                }
                else if (type == "Rest")
                {
                    ws.Rest = typeDuration;
                    ws.Pace = "";
                }
                else //some workoutsets do not require rest or pace
                {
                    ws.Rest = "";
                    ws.Pace = "";
                }
                //ws.Duration = ws.Repeats ;
                ws.Description = description;
                ws.EnergyGroupName = energyGroup;
                ws.TotalDistance = int.Parse(totalDistance);
            }
            Session["wp"] = wp;
            Session["WorkoutSetList"] = wp.SubSetList;
            return RedirectToAction("AddNewWorkoutPlan", "Workout");
        }


        public ActionResult savePlan()
        {
            WorkoutPlanObject wp = Session["wp"] as WorkoutPlanObject;
            WorkOutPlan_BLL.insertWorkoutPlan(wp);//where idb should work fine the connection
            Session["wp"] = null;
            return RedirectToAction("Index", "Home");

        }



        
        public ActionResult deletePlan(string tblID)
        {
            if (int.Parse(tblID) != 0)
            {
                WorkoutPlanObject wp = Session["wp"] as WorkoutPlanObject;
                WorkOutPlan_BLL.deleteWorkoutPlan(wp);
                //TO-DO: Delete the plan using the tblID of the plan
            }

            Session["wp"] = null;
            return RedirectToAction("Index", "Home");
        }

        public ActionResult updatePlanDate(DateTime date)
        {
            WorkoutPlanObject wp = Session["wp"] as WorkoutPlanObject;
            wp.PlanDate = date;
            Session["wp"] = wp;
            return RedirectToAction("AddNewWorkoutPlan", "Workout");
        }

        public JsonResult getSubSetPosition(string orderId)
        {
            WorkoutPlanObject wp = Session["wp"] as WorkoutPlanObject;
            WorkoutSetObject ws = (WorkoutSetObject)wp.SubSetHashTable[int.Parse(orderId)];
            List<int> ids = ws.getSubsetPositions();
            return Json(ids);
        }


        //public ActionResult workoutAction(WorkoutPlan workoutPlan, string WorkoutPlanDate,int Id = -1,string command = "")
        //{
        //    if (Session["strokes"] == null)
        //    {
        //        Strokes_BLL strokes = new Strokes_BLL();
        //        Session["strokes"] = strokes.getStrokes();          
        //    }
                     
        //    Session["WorkoutPlanDate"] = WorkoutPlanDate;
        //    List<WorkoutSet> _workoutSets = Session["WorkoutSetList"] as List<WorkoutSet>;
        //    _workoutSets = workoutPlan.WorkoutSet;
        //    Session["WorkoutSetList"] = _workoutSets;
        //    if (command == "Save Plan")
        //    {           
        //        return RedirectToAction("SaveWorkoutPlan","Workout");
        //    }
        //    else if(command == "Create New Set")
        //    {
        //        return RedirectToAction("createSet", "Workout");
        //    }
        //    else if(command == "delete")
        //    {
        //        return RedirectToAction("deleteSet", "Workout", new { index = Id});
        //    }
        //    else if (command == "Print Plan")
        //    {
        //        return RedirectToAction("PrintWorkoutPlan", "Workout", new { planId = Id });
        //    }
        //    else if (command == "Edit Plan")
        //    {
        //        return RedirectToAction("EditWorkoutPlan", "Workout", new { planId = Id });
        //    }
        //    else if (command == "Update Plan")
        //    {
        //        return RedirectToAction("UpdateWorkoutPlan", "Workout"); 
        //    }
        //    else
        //    {
        //        return View(); //TO-DO: return error message
        //    }
        //}
        //public PartialViewResult createSet()
        //{
        //     List<WorkoutSet> _workoutSets = Session["WorkoutSetList"] as List<WorkoutSet>;
        //     _workoutSets.Add(new WorkoutSet(_workoutSets.Count + 1));
        //     Session["WorkoutSetList"] = _workoutSets;

        //     return PartialView("WorkoutSetList", _workoutSets);   
        //}
        



        //[HttpPost]
        //public ActionResult editSet(FormCollection form)  //Code not relevant now, as now there's a better way to update the model. 
        //{
        //    Strokes_BLL strokes = new Strokes_BLL();
        //    WorkoutSet workoutSet = new WorkoutSet();
            
        //    workoutSet.Repeats = int.Parse(form["item.Repeats"]);
        //    workoutSet.ID = int.Parse(form["item.ID"]); 
        //    workoutSet.WorkoutSetDistance = int.Parse(form["item.WorkoutSetDistance"]);
        //    workoutSet.SingleDuration = int.Parse(form["item.SingleDuration"]);
        //    workoutSet.OrderNum = int.Parse(form["item.OrderNum"]);
        //    workoutSet.PaceTime = int.Parse(form["item.PaceTime"]);
        //    workoutSet.Description = form["item.Description"];
        //    workoutSet.Stroke = strokes.getStrokes(int.Parse(form["strokeSelect"]));

        //    List<WorkoutSet> _workoutSets = Session["WorkoutSetList"] as List<WorkoutSet>;
        //    _workoutSets[workoutSet.OrderNum - 1] = workoutSet;

        //   // _workoutSets.Add(new WorkoutSet(_workoutSets.Count + 1));

        //    Session["WorkoutSetList"] = _workoutSets;

        //    return Json(new { success = true });
        //}

        // Function to remove a set from the WorkoutSetList Session Variable
        
        //public PartialViewResult deleteSet(int index)
        //{
            
        // //   index = index - 1;
        //    List<WorkoutSet> _workoutSets = Session["WorkoutSetList"] as List<WorkoutSet>;

        //    _workoutSets.RemoveAt(index - 1);

        //    // This will reorder the workout sets in the list
        //    for (int i = index - 1; i < _workoutSets.Count; i++)
        //    {
        //        _workoutSets[i].OrderNum = i + 1;
        //    }
            
        //    Session["WorkoutSetList"] = _workoutSets;

        //    //Strokes_BLL strokes = new Strokes_BLL();
        //    //Session["strokes"] = strokes.getStrokes();  TODO- Look into strokes
            
        //    return PartialView("WorkoutSetList", _workoutSets);
        //}


    }
}
