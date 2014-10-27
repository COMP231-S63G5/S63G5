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
        private String _date;

        public List<WorkoutSet> WorkoutSet
        {
            get { return _workoutSet; }
            set { _workoutSet = value; }
        }
        public String Date {
            get { return _date; }
            set { _date = value; }
        }

        public WorkoutPlan()
        {
            _workoutSet = new List<WorkoutSet>();
        }

    }
}
