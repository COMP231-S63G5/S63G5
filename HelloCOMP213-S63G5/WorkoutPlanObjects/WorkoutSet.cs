using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkoutPlanObjects
{
    class WorkoutSet : IWorkoutSet
    {
        private int _breakTime;

        public int BreakTime
        {
            get { return _breakTime; }
            set { _breakTime = value; }
        }


        public WorkoutSet(int repeats, int workoutsetdistance, string workoutsetdetails, int duration, int breaktime = 0)
        {
            Repeats = repeats;
            WorkoutSetDistance = workoutsetdistance;
            WorkoutSetDetails = workoutsetdetails;
            SingleDuration = duration;
            BreakTime = breaktime;
        }
    }
}