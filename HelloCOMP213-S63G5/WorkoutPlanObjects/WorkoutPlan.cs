using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkoutPlanObjects
{
    public class WorkoutPlan
    {
        private List<WorkoutSet> _workoutSet;

        public List<WorkoutSet> WorkoutSet
        {
            get { return _workoutSet; }
            set { _workoutSet = value; }
        }
        public int test { get; set; }

        public WorkoutPlan()
        {
            _workoutSet = new List<WorkoutSet>();
        }


    }
}
