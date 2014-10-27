using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkoutPlanObjects
{
    public class WorkoutSet : IWorkoutSet
    {

        private int _restPeriod;
        private int _paceTime;
        private string _description;

        public string Description 
        {
            get { return _description; }
            set { _description = value; }
        }

        public int PaceTime 
        { 
            get { return _paceTime;} 
            set { _paceTime = value;} 
        }

        public int RestPeriod
        {
            get { return _restPeriod; }
            set { _restPeriod = value; }
        }


        public WorkoutSet(int repeats, int workoutsetdistance, int duration,  int orderNum, int pacetime = 0, int restperiod = 0)
        {
            Repeats = repeats;
            WorkoutSetDistance = workoutsetdistance;           
            SingleDuration = duration;
            RestPeriod = restperiod;
            OrderNum = orderNum;
        }

        public WorkoutSet(int orderNum)
        {
            OrderNum = orderNum;
        }
        public WorkoutSet()
        {

        }
    }
}