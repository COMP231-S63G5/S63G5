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


        public WorkoutSet(int repeats, string workoutSetName, int duration)
        {
            Repeats = repeats;
            WorkoutSetName = workoutSetName;
            SingleDuration = duration;
        }
    }
}
