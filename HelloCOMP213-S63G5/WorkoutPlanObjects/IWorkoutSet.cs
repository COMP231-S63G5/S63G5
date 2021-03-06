﻿
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
        private WorkoutStroke _stroke;
        private int _repeats;
        private int _workoutSetDistance; //The distance for each workout set
        private int _duration; //time to complete of each workout set
        private int _orderNum; // The order the object shows on the plan


        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }

        public WorkoutStroke Stroke
        {
            get { return _stroke; }
            set { _stroke = value; }
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

        public int OrderNum
        {
            get { return _orderNum; }
            set { _orderNum = value; }
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

