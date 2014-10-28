using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkoutDBObject;

namespace WorkoutPlanAPI
{
    public class WorkOutPlan_DLL
    {
        //This method Would call ListOfWorkOutPlanIDs method to get list of 
        //existing workoutPlan ids from Business Layer.
        public List<int> getWorkOutPlanIDsList()
        {
            SwimWorkoutDBContext swimDB2 = new SwimWorkoutDBContext();
            List<int> listOfWorkOutPlanIDs = swimDB2.getWorkOutPlanIds();
            return listOfWorkOutPlanIDs;
        }
    }
}
