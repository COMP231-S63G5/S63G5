using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkoutPlanObjects;

namespace WorkoutPlanObjects
{
    public class WorkoutSetObject
    {
        /// <summary>
        /// table id
        /// </summary>
        public int tblID;
        public string Description;
        public int Distance;
        public string Duration;
        public string EnergyGroupName;
        public int EnergyGroupAmount;
        public int OrderID;
        public string Pace;
        public int ParentID;
        public string Rest;
        public EnumWorkoutSetType SetType;
        public string Stroke;
        public List<WorkoutSetObject> SubSetList;
        internal WorkoutPlanObject workoutPlan;   // root node
        internal int TotalRepeats;

        private int _repeats;
        public int Repeats
        {
            get { return _repeats; }
            set
            {
                TotalRepeats = TotalRepeats / _repeats * value;
                _repeats = value;
                if (SetType == EnumWorkoutSetType.Group || SetType == EnumWorkoutSetType.Section)
                {
                    if (SubSetList.Count > 0)
                    {
                        foreach (WorkoutSetObject set in SubSetList)
                        {
                            set.TotalRepeats = set.Repeats * TotalRepeats;  // get total repeats from parent
                            set.Repeats = set.Repeats;  // trigger setter of Repeats of child's child's... node
                        }
                    }
                }
            }
        }


        /// <summary>
        /// create a section
        /// </summary>
        /// <param name="sectionName">section name</param>
        public WorkoutSetObject(string sectionName)
        {
            SetType = EnumWorkoutSetType.Section;
            Description = sectionName;
            SubSetList = new List<WorkoutSetObject>();
            TotalRepeats = 1;
            ParentID = 0;
            _repeats = 1;
            Repeats = 1;
        }

        /// <summary>
        /// create a group
        /// </summary>
        /// <param name="repeats">repeats of a group</param>
        public WorkoutSetObject(int repeats)
        {
            SetType = EnumWorkoutSetType.Group;
            SubSetList = new List<WorkoutSetObject>();
            TotalRepeats = 1;
            _repeats = 1;
            Repeats = repeats;
        }


        /// <summary>
        /// create a set
        /// </summary>
        /// <param name="repeats">repeats</param>
        /// <param name="distance">distance in meter</param>
        /// <param name="stroke">stroke: free, back, fly, breast, medley</param>
        /// <param name="duration">1:20</param>
        /// <param name="description">description</param>
        /// <param name="energyname">E1 E2 E3 S1 S2 S3</param>
        /// <param name="energyamount">100,200</param>
        public WorkoutSetObject(int repeats, int distance, string stroke, string pace, string description, string energyname, int energyamount)
        {
            SetType = EnumWorkoutSetType.Set;
            Distance = distance;
            Stroke = stroke;
            Pace = pace;
            Description = description;
            EnergyGroupName = energyname;
            EnergyGroupAmount = energyamount;
            TotalRepeats = 1;
            _repeats = 1;
            Repeats = repeats;
        }

        public WorkoutSetObject(int tblid, string setType, int repeats, int distance, string stroke, string pace, string rest, string duration, string description, string energyname, int energyamount, int orderid, int parentid)
        {
            try
            {
                tblID = tblid;
                if (setType == "Section")
                {
                    SetType = EnumWorkoutSetType.Section;
                    SubSetList = new List<WorkoutSetObject>();
                }
                else if (setType == "Group")
                {
                    SetType = EnumWorkoutSetType.Group;
                    SubSetList = new List<WorkoutSetObject>();
                }
                else if (setType == "Set")
                {
                    SetType = EnumWorkoutSetType.Set;
                }
                TotalRepeats = 1;
                _repeats = 1;
                Repeats = repeats;
                Distance = distance;
                Stroke = stroke;
                Pace = pace;
                Rest = rest;
                Duration = duration;
                Description = description;
                EnergyGroupName = energyname;
                EnergyGroupAmount = energyamount;
                OrderID = orderid;
                ParentID = parentid;
            }
            catch
            {
                throw new Exception("Error loading workout set from Database. set id:" + tblid);
            }
        }

        public string getConsoleString()
        {
            if (SetType == EnumWorkoutSetType.Section)
            {
                return string.Format("{0,2}    {1}\n", OrderID, Description) +
                    string.Join("", SubSetList.Select(s => s.getConsoleString()).ToArray());
            }
            else if (SetType == EnumWorkoutSetType.Group)
            {
                return string.Format("{0,2}  {1,5} X  [\n", OrderID, Repeats) +
                    string.Join("", SubSetList.Select(s => s.getConsoleString()).ToArray()) +
                "             ]\n";
            }
            else
            {   // SetType is Set
                return string.Format("{0,2}      {1,4} X {2,-4} {3,-6} on {4,5} ({5})   {6:5} {7}\n", OrderID, Repeats, Distance, Stroke, Pace, Description, EnergyGroupName, EnergyGroupAmount);
            }
        }

        /// <summary>
        /// return order id of last child
        /// </summary>
        public int LastChildSetOrderID
        {
            get
            {
                if (SetType == EnumWorkoutSetType.Set)
                {
                    return OrderID;
                }
                else
                {
                    return SubSetList.Max(s => s.LastChildSetOrderID);
                }
            }
        }

        public List<int> getSubsetPositions()
        {
            List<int> posList = new List<int>();
            posList.Add(OrderID);   // add node itself
            if (SubSetList.Count > 0)
            {
                var sub = (from s in SubSetList
                           orderby s.OrderID
                           select s).ToList();  // sort subsets
                foreach (WorkoutSetObject s in sub)
                {
                    posList.Add(s.OrderID); // add to list
                }
            }
            return posList;
        }

        //public void removeWorkoutSet(WorkoutSetObject workoutSet)
        //{
        //    if (workoutSet.SetType != EnumWorkoutSetType.Set)
        //    {
        //        throw new Exception("Cannot remove section/group.");
        //    }
        //    else if (workoutSet.OrderID == workoutPlan.SubSetHashTable.Count)
        //    {   // last set in last group or section
        //        workoutPlan.SubSetHashTable.Remove(workoutPlan.SubSetHashTable.Count);  // remove last item from hashtable
        //        SubSetList.Remove(workoutSet);
        //    }
        //    else
        //    {   // remove from middle
        //        // if remove 3rd one from a list of 5, move 4th to 3rd, then 5th to 4th
        //        WorkoutSetObject temp_set;
        //        for (int i = workoutSet.OrderID; i < workoutPlan.SubSetHashTable.Count; i++)
        //        {
        //            temp_set = (WorkoutSetObject)workoutPlan.SubSetHashTable[i + 1];   // get 4th set
        //            temp_set.OrderID = i;   // 4 to 3, 5 to 4
        //            if (temp_set.ParentID > workoutSet.OrderID)
        //            {
        //                temp_set.ParentID--;
        //            }
        //            workoutPlan.SubSetHashTable[i] = temp_set; //assign to 3rd set
        //        }
        //        workoutPlan.SubSetHashTable.Remove(workoutPlan.SubSetHashTable.Count);  // remove last set
        //        SubSetList.Remove(workoutSet);
        //    }
        //}
        //public void removeWorkoutGroup(WorkoutSetObject workoutSet)
        //{
        //    if (workoutSet.SubSetList.Count>0)
        //    {
        //        throw new Exception("Section/Group contains sub itmes, please remove them first.");
        //    }
        //    else if (workoutSet.OrderID == workoutPlan.SubSetHashTable.Count)
        //    {   // last set in last group or section
        //        workoutPlan.SubSetHashTable.Remove(workoutPlan.SubSetHashTable.Count);  // remove last item from hashtable
        //        SubSetList.Remove(workoutSet);
        //    }
        //    else
        //    {   // remove from middle
        //        // if remove 3rd one from a list of 5, move 4th to 3rd, then 5th to 4th
        //        WorkoutSetObject temp_set;
        //        for (int i = workoutSet.OrderID; i < workoutPlan.SubSetHashTable.Count; i++)
        //        {
        //            temp_set = (WorkoutSetObject)workoutPlan.SubSetHashTable[i + 1];   // get 4th set
        //            temp_set.OrderID = i;   // 4 to 3, 5 to 4
        //            if (temp_set.ParentID > workoutSet.OrderID)
        //            {
        //                temp_set.ParentID--;
        //            }
        //            workoutPlan.SubSetHashTable[i] = temp_set; //assign to 3rd set
        //        }
        //        workoutPlan.SubSetHashTable.Remove(workoutPlan.SubSetHashTable.Count);  // remove last set
        //        SubSetList.Remove(workoutSet);
        //    }
        //}
    }

}
