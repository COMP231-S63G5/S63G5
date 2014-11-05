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
        private string _description = "";

        //Energy Fields
        private int _E1;
        private int _E2;
        private int _E3;
        private int _S1;
        private int _S2;
        private int _S3;
        private int _REC;

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

        //Energy Field Properties
        public int E1
        {
            get { return _E1; }
            set { _E1 = value; }
        }
        public int E2
        {
            get { return _E2; }
            set { _E2 = value; }
        }
        public int E3
        {
            get { return _E3; }
            set { _E3 = value; }
        }
        public int S1
        {
            get { return _S1; }
            set { _S1 = value; }
        }
        public int S2
        {
            get { return _S2; }
            set { _S2 = value; }
        }
        public int S3
        {
            get { return _S3; }
            set { _S3 = value; }
        }
        public int REC
        {
            get { return _REC; }
            set { _REC = value; }
        }

        public WorkoutSet(  int repeats, 
                            int workoutsetdistance, 
                            int duration,  
                            int orderNum, 
                            WorkoutStroke stroke, 
                            int pacetime = 0, 
                            int restperiod = 0, 
                            int E1 = 0, 
                            int E2 = 0, 
                            int E3 = 0, 
                            int S1 = 0, 
                            int S2 = 0, 
                            int S3 = 0,
                            int REC = 0)
        {
            Repeats = repeats;
            WorkoutSetDistance = workoutsetdistance;           
            SingleDuration = duration;
            RestPeriod = restperiod;
            OrderNum = orderNum;
            Stroke = stroke;
            
            E1 = this.E1;
            E2 = this.E2;
            E3 = this.E3;
            S1 = this.S1;
            S2 = this.S2;
            S3 = this.S3;
            REC = this.REC;
        }

        public WorkoutSet(int orderNum)
        {
            OrderNum = orderNum;
            Stroke = new WorkoutStroke(-1, "Default", "Default");
        }


        public WorkoutSet()
        {

        }
    }
}