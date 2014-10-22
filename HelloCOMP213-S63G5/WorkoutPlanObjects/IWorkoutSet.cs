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
        private int _repeats;
        private string _workoutSetDetails; //details of each workout
        private int _workoutSetDistance; //The distance for each workout set
        private int _duration; //time to complete of each workout set
        
        public int Repeats { 
            get
            {
                return _repeats;
            }
            set
            {
                _repeats = value;
            }
        }

         public string WorkoutSetDetails
        {
            get
            {
                return _workoutSetDetails;
            }
            set
            {
                _workoutSetDetails = value;
            }
        }

        public int WorkoutSetDistance
         {
             get
             {
                 return _workoutSetDistance;
             }
             set
             {
                 _workoutSetDistance = value;
             }
         }



        //Method is virtual because single duration calculations is different when calculating single duration for one workout set vs single duration for one workout group.
        virtual public int SingleDuration
        {
            get
            {
                return _duration;
            }
            set
            {
                _duration = value;
            }
        }

        //Method is virtual because Total Duration calculations is different when calculating for one workoutset vs total duration for one workout group.
        virtual public int TotalDuration
        {
            get
            {
                return _duration * Repeats;
            }
        }

    }
}
