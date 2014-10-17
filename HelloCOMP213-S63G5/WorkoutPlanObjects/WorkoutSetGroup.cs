﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkoutPlanObjects
{
    class WorkoutSetGroup : IWorkoutSet
    {
        private List<IWorkoutSet> _workoutSet;


        public WorkoutSetGroup()
        {
            Repeats = 1; //default repeat is 1
            WorkoutSets = new List<IWorkoutSet>();
            WorkoutSetDetails = "";

        }

        public WorkoutSetGroup(int repeats, List<IWorkoutSet> workoutset)
        {
            Repeats = repeats;
            WorkoutSets = new List<IWorkoutSet>();
            WorkoutSets = workoutset;
            WorkoutSetDetails = "";

        }

        //contains a list of all workoutsets 
        public List<IWorkoutSet> WorkoutSets
        {
            get
            {
                return _workoutSet;
            }
            set
            {
                _workoutSet = value;
            }
        }

        //single duration refers to the sum of all durations within the workout set group. 
        public override int SingleDuration
        {
            get
            {
                return WorkoutSets.Sum(i => i.SingleDuration);
            }
            
        }

        //total duration is the sum of all workout sets in the group times the number of repeats. 
        public override int TotalDuration
        {
            get
            {
                return WorkoutSets.Sum(i => i.TotalDuration) * Repeats;
            }
        }

        public void addWorkoutSet(IWorkoutSet workoutset) //can contain additional workoutset groups or workout sets, hence IWorkoutSet class. 
        {
            WorkoutSets.Add(workoutset);
        }




       
    }
}

