
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkoutPlanObjects
{
    public abstract class IWorkoutSet
    {
        private int _id;
        private int _strokeId;
        private int _repeats;
        private int _workoutSetDistance; //The distance for each workout set
        private int _duration; //time to complete of each workout set


        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }

        public int StrokeId
        {
            get { return _strokeId; }
            set { _strokeId = value; }
        }


        public int Repeats
        { 
            get { return _repeats; }
            set { _repeats = value; }
        }

        public int WorkoutSetDistance
         {
             get { return _workoutSetDistance; }
             set { _workoutSetDistance = value; }
         }

        //Method is virtual because single duration calculations is different when calculating single duration for one workout set vs single duration for one workout group.
        virtual public int SingleDuration
        { 
            get { return _duration; }
            set { _duration = value; }
        }

        //Method is virtual because Total Duration calculations is different when calculating for one workoutset vs total duration for one workout group.
        virtual public int TotalDuration
        { 
            get { return _duration * Repeats; }
        }

    }
}

