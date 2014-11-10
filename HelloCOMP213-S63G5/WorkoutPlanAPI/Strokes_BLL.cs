using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkoutDBObject;
using WorkoutPlanObjects;

namespace WorkoutPlanAPI
{
    /*
     * This class will contain all functions involving updating/deletion/retrieval of strokes  
     */
    public class Strokes_BLL
    {
        public List<WorkoutStroke> getStrokes()
        {
            SwimWorkoutDBContext swimDB = new SwimWorkoutDBContext();

            return swimDB.getStrokes();
        }

        public WorkoutStroke getStrokes(int strokeID)
        {
            SwimWorkoutDBContext swimDB = new SwimWorkoutDBContext();

            return swimDB.getStrokes(strokeID);
        }


     
       
    }
}
