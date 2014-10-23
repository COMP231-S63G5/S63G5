using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkoutPlanObjects
{
    class WorkoutStroke
    {
        private int _id;
        private string _name;
        private string _description;

        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }

        public WorkoutStroke(int id, string name, string description)
        {
            ID = id;
            Name = name;
            Description = description;
        }
    }
}
