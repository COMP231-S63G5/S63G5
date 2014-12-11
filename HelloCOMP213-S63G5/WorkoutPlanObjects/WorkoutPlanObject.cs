using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkoutPlanObjects;


namespace WorkoutPlanObjects
{
    public class WorkoutPlanObject
    {
        /// <summary>
        /// table id
        /// </summary>
        public int tblID;
        public DateTime PlanDate;
        public string PlanName;
        public Hashtable SubSetHashTable;
        public List<WorkoutSetObject> SubSetList;

        public int TotalDistance
        {
            get
            {
                int result = 0;
                if (SubSetHashTable.Count > 0)
                {
                    WorkoutSetObject set;
                    for (int i = 1; i <= SubSetHashTable.Count; i++)
                    {
                        set = (WorkoutSetObject)SubSetHashTable[i];
                        if (set.SetType == EnumWorkoutSetType.Set)
                        {
                            result = result + set.TotalRepeats * set.Distance;
                        }
                    }
                }
                return result;
            }
        }

        public string TotalDuration
        {
            get
            {
                int min = 0, sec = 0, m = 0, s = 0;
                if (SubSetHashTable.Count > 0)
                {
                    WorkoutSetObject set;
                    for (int i = 1; i <= SubSetHashTable.Count; i++)
                    {
                        set = (WorkoutSetObject)SubSetHashTable[i];
                        if (set.SetType == EnumWorkoutSetType.Set)
                        {
                            string[] t = set.Pace.Split(':');
                            m = Int32.Parse(t[0]);
                            s = Int32.Parse(t[1]);
                            min = min + m * set.TotalRepeats;
                            sec = sec + s * set.TotalRepeats;
                        }
                    }
                }
                min = min + sec / 60;
                sec = sec % 60;
                return min.ToString() + ":" + sec.ToString().PadLeft(2, '0');
            }
        }

        public string EnergyPercentage(string energyType)
        {
            int energyTotal = 0;
            WorkoutSetObject set;
            for (int i = 1; i <= SubSetHashTable.Count; i++)
            {
                set = (WorkoutSetObject)SubSetHashTable[i];
                if (set.SetType == EnumWorkoutSetType.Set)
                {
                    if (set.EnergyGroupName.Equals(energyType))
                    {
                        energyTotal += set.Distance * set.TotalRepeats;
                    }
                }
            }

            double percent = (double)energyTotal / (double)TotalDistance;
            percent *= 100;

            return  Math.Round(percent) + "%"; 
        }
        public WorkoutPlanObject(string name, DateTime date)
        {
            PlanName = name;
            PlanDate = date;
            tblID = 0;
            SubSetHashTable = new Hashtable();
            SubSetList = new List<WorkoutSetObject>();
        }

        public WorkoutPlanObject(DateTime date)
        {
            PlanDate = date;
            SubSetHashTable = new Hashtable();
            SubSetList = new List<WorkoutSetObject>();
        }
        public WorkoutPlanObject(int tblid, string name, DateTime date, List<WorkoutSetObject> workoutSets)
        {
            tblID = tblid;
            PlanName = name;
            PlanDate = date;
            SubSetHashTable = new Hashtable();
            SubSetList = new List<WorkoutSetObject>();
            var sets = from s in workoutSets
                       orderby s.OrderID
                       select s;
            foreach (var set in sets)
            {
                SubSetHashTable.Add(set.OrderID, set);
                set.workoutPlan = this;
                if (set.SetType == EnumWorkoutSetType.Section)
                {
                    set.SubSetList = new List<WorkoutSetObject>();
                    SubSetList.Add(set);    // add section
                }
                else if (set.SetType == EnumWorkoutSetType.Group)
                {   // set.ParentID must less than set.orderID, and exists in SubSetHashTable
                    set.SubSetList = new List<WorkoutSetObject>();
                    WorkoutSetObject parent_set = (WorkoutSetObject)SubSetHashTable[set.ParentID];
                    parent_set.SubSetList.Add(set); // add group to its parent node
                    parent_set.Repeats = parent_set.Repeats;    // trigger setter
                }
                else
                {   // set.ParentID must less than set.orderID, and exists in SubSetHashTable
                    WorkoutSetObject parent_set = (WorkoutSetObject)SubSetHashTable[set.ParentID];
                    parent_set.SubSetList.Add(set); // add set to its parent node
                    parent_set.Repeats = parent_set.Repeats;    // trigger setter
                }
            }
        }

        /// <summary>
        /// add a section to a plan. section is 1st level of a plan
        /// </summary>
        /// <param name="workoutSet">workout section object</param>
        /// <param name="position">the position where new section will be add after at</param>
        public void addWorkoutSection(WorkoutSetObject workoutSet, int position)
        {
            if (workoutSet.SetType != EnumWorkoutSetType.Section)
            {
                throw new Exception("Cannot add " + workoutSet.SetType.ToString() + " to a plan.");
            }
            else if (position == 0)
            {
                workoutSet.OrderID = 1;
                workoutSet.ParentID = 0;
                SubSetList.Add(workoutSet); // add section to tree
                if (SubSetHashTable.Count >0)
                {
                    WorkoutSetObject temp_set = (WorkoutSetObject)SubSetHashTable[position];
                    SubSetHashTable.Add(SubSetHashTable.Count + 1, null); 
                    for (int i = SubSetHashTable.Count -1; i > position; i--)
                    {
                        temp_set = (WorkoutSetObject)SubSetHashTable[i];    // get 2nd set
                        temp_set.OrderID++; // increase orderid
                        temp_set.ParentID++;    // increase parentid

                        SubSetHashTable[i + 1] = temp_set;  // move 5th to 6th in hashtable
                    }
                }
                SubSetHashTable[1]= workoutSet; // add to table, easy searching
                workoutSet.workoutPlan = this;
            }
            else if (position > 0)
            {
                WorkoutSetObject temp_set = (WorkoutSetObject)SubSetHashTable[position];
                //if (temp_set.SetType != EnumWorkoutSetType.Section)
                //{
                //    throw new Exception("Cannot add section after " + temp_set.SetType.ToString() +".");
                //}
                //else
                //{
                position = temp_set.LastChildSetOrderID;
                if (position == SubSetHashTable.Count)
                {   // append section after last section
                    workoutSet.OrderID = position + 1;
                    workoutSet.ParentID = 0;
                    SubSetList.Add(workoutSet);
                    SubSetHashTable.Add(position + 1, workoutSet);
                    workoutSet.workoutPlan = this;
                }
                else
                {   // insert in the middle
                    SubSetHashTable.Add(SubSetHashTable.Count + 1, null);   // add a null at last
                    // if count=5, insert position after 2, then loop from 5 to 3
                    for (int i = SubSetHashTable.Count -1; i > position; i--)
                    {
                        temp_set = (WorkoutSetObject)SubSetHashTable[i];    // get 5th set
                        temp_set.OrderID++; // assign order id to 6
                        if (temp_set.ParentID > position)
                        {
                            temp_set.ParentID++;    // if parent id after 2, parent also move one position forward.
                        }
                        SubSetHashTable[i + 1] = temp_set;  // move 5th to 6th in hashtable
                    }
                    // now item orderid 3-5 become orderid 4-6. can insert to order id=3
                    workoutSet.OrderID = position + 1;
                    SubSetHashTable[position + 1] = workoutSet;  // put new set at 3rd position
                    SubSetList.Add(workoutSet);
                    workoutSet.workoutPlan = this;
                }
                //}
            }
        }
        public void addWorkoutSet(WorkoutSetObject workoutSet, int parentid, int position)
        {
            WorkoutSetObject parent_set = (WorkoutSetObject)SubSetHashTable[parentid];
            
            WorkoutSetObject t_set = (WorkoutSetObject)SubSetHashTable[position];
            position = t_set.LastChildSetOrderID;

            if (parent_set.SetType == EnumWorkoutSetType.Set)
            {
                throw new Exception("Cannot add set/group to a set.");
            }
            else if (position == SubSetHashTable.Count)
            {   // add to last of last section
                workoutSet.OrderID = position + 1;
                workoutSet.ParentID = parent_set.OrderID;
                parent_set.SubSetList.Add(workoutSet);
                SubSetHashTable.Add(position + 1, workoutSet);
            }
            else
            {
                WorkoutSetObject temp_set;
                SubSetHashTable.Add(SubSetHashTable.Count + 1, null);   // add null to last index
                // if insert a set after 3rd set (total 5 set), move 5th to 6th, 4th to 5th
                for (int i = SubSetHashTable.Count - 1; i > position; i--)
                {
                    temp_set = (WorkoutSetObject)SubSetHashTable[i];    // get 5th set
                    temp_set.OrderID++; // assign order id to 6
                    if (temp_set.ParentID > position)
                    {
                        temp_set.ParentID++;    // if parent id after 3, parent also move one position forward.
                    }
                    SubSetHashTable[i + 1] = temp_set;  // move 5th to 6th in hash table,
                }
                workoutSet.OrderID = position + 1;  // assign order id to 4
                workoutSet.ParentID = parent_set.OrderID;
                SubSetHashTable[position + 1] = workoutSet; // insert new set to 4th
                parent_set.SubSetList.Add(workoutSet);
                workoutSet.workoutPlan = this;   // assign root node
            }
            parent_set.Repeats = parent_set.Repeats;    // trigger setter   //workoutSet.TotalRepeats = workoutSet.Repeats * parent_set.TotalRepeats;
        }
        public void addWorkoutGroup(WorkoutSetObject workoutSet, int parentid, int position)
        {
            addWorkoutSet(workoutSet, parentid, position);
        }

        /// <summary>
        /// remove workout set/group/section
        /// section and group must contains 0 sub item
        /// </summary>
        /// <param name="orderID">order ID of the workout set/group/section</param>
        public void remove(int orderID)
        {
            WorkoutSetObject removeSet = (WorkoutSetObject)SubSetHashTable[orderID];
            if (removeSet.SetType == EnumWorkoutSetType.Section && removeSet.SubSetList.Count > 0)
            {
                throw new Exception("Section contains sub itmes, please remove them first.");
            }
            else if (removeSet.SetType == EnumWorkoutSetType.Group && removeSet.SubSetList.Count > 0)
            {
                throw new Exception("Group contains sub itmes, please remove them first.");
            }
            else if (orderID == SubSetHashTable.Count)
            {   // last and empty section
                SubSetHashTable.Remove(SubSetHashTable.Count);  // remove last item from hashtable
                SubSetList.Remove(removeSet);
            }
            else
            {   // remove from middle
                // if remove 3rd one from a list of 5, move 4th to 3rd, then 5th to 4th
                WorkoutSetObject temp_set;
                for (int i = orderID; i < SubSetHashTable.Count; i++)
                {
                    temp_set = (WorkoutSetObject)SubSetHashTable[i + 1];   // get 4th set
                    temp_set.OrderID = i;   // 4 to 3, 5 to 4
                    if (temp_set.ParentID > orderID)
                    {
                        temp_set.ParentID--;
                    }
                    SubSetHashTable[i] = temp_set; //assign to 3rd set
                }
                SubSetHashTable.Remove(SubSetHashTable.Count);  // remove last set
                if (removeSet.ParentID == 0)
                {
                    SubSetList.Remove(removeSet);   // remove from plan subsetlist
                }
                else
                {   // remove from parent node's subsetlist
                    ((WorkoutSetObject)SubSetHashTable[removeSet.ParentID]).SubSetList.Remove(removeSet);
                }
            }
        }
        public string getConsoleString()
        {
            return string.Format("Team: {0}  -  Date: {1}  \n\n", PlanName, PlanDate) +
                string.Join("", SubSetList.Select(s => s.getConsoleString()).ToArray()) + "\n" +
                string.Format("Total Duration: {0,10}\n", TotalDuration) +
                string.Format("Total Distance: {0,10}", TotalDistance);
        }

        //public void removeWorkoutSection(WorkoutSetObject workoutSet)
        //{
        //    if (workoutSet.SetType!= EnumWorkoutSetType.Section)
        //    {
        //        throw new Exception("Cannot remove " + workoutSet.SetType.ToString() + " from a plan.");
        //    }
        //    else if (workoutSet.SubSetList.Count>0)
        //    {
        //        throw new Exception("Section contains sub itmes, please remove them first.");
        //    }
        //    else  if (workoutSet.OrderID == SubSetHashTable.Count )
        //    {   // last and empty section
        //        SubSetHashTable.Remove(SubSetHashTable.Count);  // remove last item from hashtable
        //        SubSetList.Remove(workoutSet);
        //    }
        //    else
        //    {   // remove from middle
        //        // if remove 3rd one from a list of 5, move 4th to 3rd, then 5th to 4th
        //        WorkoutSetObject temp_set;
        //        for(int i= workoutSet.OrderID;i<SubSetHashTable.Count;i++)
        //        {
        //            temp_set = (WorkoutSetObject) SubSetHashTable[i + 1];   // get 4th set
        //            temp_set.OrderID = i;   // 4 to 3, 5 to 4
        //            if (temp_set.ParentID > workoutSet.OrderID)
        //            {
        //                temp_set.ParentID--;    
        //            }
        //            SubSetHashTable[i] = temp_set; //assign to 3rd set
        //        }
        //        SubSetHashTable.Remove(SubSetHashTable.Count);  // remove last set
        //        SubSetList.Remove(workoutSet);
        //    }
        //}
    }
}

