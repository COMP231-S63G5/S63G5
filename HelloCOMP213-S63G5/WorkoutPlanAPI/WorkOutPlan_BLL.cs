using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkoutDBObject;
using WorkoutPlanObjects;

namespace WorkoutPlanAPI
{
    public static class WorkOutPlan_BLL
    {
        public static long insertWorkoutPlan(WorkoutPlanObject plan)
        {
            return WorkoutDBObject.SwimWorkoutDBContext.insertWorkoutPlan(plan);
        }
        public static WorkoutPlanObject getWorkoutPlan(int planid)
        {
            return WorkoutDBObject.SwimWorkoutDBContext.getWorkoutPlan(planid);
        }
        public static int updateWorkoutPlan(WorkoutPlanObject plan)
        {
            return WorkoutDBObject.SwimWorkoutDBContext.updateWorkoutPlan(plan);
        }
        public static int deleteWorkoutPlan(WorkoutPlanObject plan)
        {
            return WorkoutDBObject.SwimWorkoutDBContext.deleteWorkoutPlan(plan);
        }
        public static List<int> getWorkOutPlanIDsList()
        {
            return WorkoutDBObject.SwimWorkoutDBContext.getWorkOutPlanIds();
        }
    }
}

#region comment
/*    
        
        //This method Would call ListOfWorkOutPlanIDs method to get list of 
        //existing workoutPlan ids from Business Layer.
        public List<int> getWorkOutPlanIDsList()
        {
            SwimWorkoutDBContext swimDB2 = new SwimWorkoutDBContext();
            List<int> listOfWorkOutPlanIDs = swimDB2.getWorkOutPlanIds();
            return listOfWorkOutPlanIDs;
        }
        public WorkoutPlan getWorkPlanDetails(int id)
        {
            SwimWorkoutDBContext swimDb = new SwimWorkoutDBContext();
            WorkoutPlan woplan = swimDb.getWorkOutPlan(id);
            return woplan;
        }

        // Method to insert workout plan to DB
        public Boolean insertWorkoutPlan(WorkoutPlan plan )
        {
            SwimWorkoutDBContext swimDB = new SwimWorkoutDBContext();
            List<int> setIds;

            // insert plan
            plan.ID = swimDB.insertWorkoutPlan(plan.Date);

            // insert sets
            setIds = insertWorkoutSets(plan.WorkoutSet);

            // insert plan members
            insertWorkoutPlanMembers(plan.ID, setIds);

            return true;
        }

        // Method to insert all sets
        public List<int> insertWorkoutSets(List<WorkoutSet> sets)
        {
            List<int> setIds = new List<int>();
            SwimWorkoutDBContext swimDB = new SwimWorkoutDBContext();

            foreach (WorkoutSet set in sets)
            {
                setIds.Add(swimDB.insertWorkoutSet(set));
            }

            return setIds;
        }

        // Method to insert all plan members
        public void insertWorkoutPlanMembers( int planId, List<int> sets)
        {
            SwimWorkoutDBContext swimDB = new SwimWorkoutDBContext();

            for (int i = 1; i<=sets.Count; i++)
            {
                swimDB.insertWorkoutPlanMember(planId, sets[i-1], i);
            }

        }

        // Method to insert all plan members
        public Boolean deleteWorkoutPlan(int planId)
        {
            SwimWorkoutDBContext swimDB = new SwimWorkoutDBContext();

            return swimDB.deleteWorkOutPlan(planId);

        }
*/
#endregion