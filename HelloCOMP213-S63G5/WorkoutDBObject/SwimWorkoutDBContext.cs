using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using WorkoutPlanObjects;
using System.Collections;

using Microsoft.ApplicationBlocks.Data;

namespace WorkoutDBObject
{
    static public class SwimWorkoutDBContext
    {
        //private SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SwimDBConnectionString"].ConnectionString);
        private static String connStr = ConfigurationManager.ConnectionStrings["SwimDBConnectionString"].ConnectionString;

        #region Get Workout Plan by ID
        /// <summary>
        /// query workout plan, and plan set
        /// </summary>
        /// <param name="planid">plan table id</param>
        /// <returns>a plan object</returns>
        public static WorkoutPlanObject getWorkoutPlan(int planid)
        {
            string planName = "";
            DateTime planDate = new DateTime();
            List<WorkoutSetObject> setList = new List<WorkoutSetObject>();

            SqlConnection conn = new SqlConnection(connStr);

            using (SqlTransaction trans = conn.BeginTransaction())
            {
                try
                {
                    WorkoutSetObject temp_set;
                    SqlDataReader reader = SqlHelper.ExecuteReader(trans, "getworkoutplan", planid);

                    if (reader.Read())
                    {   // read from tbl_workoutplan
                        planName = reader.GetString(0);
                        planDate = reader.GetDateTime(2);
                    }
                    if (reader.NextResult())
                    {   // read from tbl_set
                        while (reader.Read())
                        {
                            temp_set = new WorkoutSetObject(
                                reader.GetInt32(0),     // tblid
                                reader.GetString(1),    // set type
                                reader.GetInt16(2),     // repeat
                                reader.GetInt16(3),    // distance
                                reader.GetString(4),    // stroke
                                reader.GetString(5),    // pace
                                reader.GetString(6),    // rest
                                reader.GetString(7),    // description
                                reader.GetString(8),    // energyname
                                reader.GetInt16(9),     // total distance
                                reader.GetInt16(10),    // order id
                                reader.GetInt16(11));    // parent id
                            setList.Add(temp_set);
                        }
                    }
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    Console.WriteLine(ex.Message);
                }
                finally
                { 
                    conn = null;
                }
            }
            WorkoutPlanObject plan = new WorkoutPlanObject(planid,planName,planDate,setList);
            return plan;
        }
        #endregion

        #region Insert Workout Plan
        /// <summary>
        /// insert plan and set
        /// </summary>
        /// <param name="plan">workout plan object</param>
        /// <returns>table plan id</returns>
        public static long insertWorkoutPlan(WorkoutPlanObject plan)
        {
            long planid = 0;
            SqlConnection conn = new SqlConnection(connStr);

            using (SqlTransaction trans = conn.BeginTransaction())
            {
                try
                {   // insert and retrieve plan id
                    WorkoutSetObject temp_set;
                    planid = long.Parse(SqlHelper.ExecuteScalar(trans, "addWorkOutPlan", plan.PlanDate, plan.PlanName, plan.TotalDistance, plan.TotalDuration).ToString());
                    plan.tblID = planid;

                    for (int i = 1; i <= plan.SubSetHashTable.Count;i++ )
                    {
                        temp_set = (WorkoutSetObject) plan.SubSetHashTable[i];
                        temp_set.tblID = int.Parse(SqlHelper.ExecuteScalar(trans, "insertWorkoutSet", 
                            temp_set.SetType.ToString(),
                            planid,
                            temp_set.Stroke,
                            temp_set.Pace,
                            temp_set.Rest,
                            temp_set.Duration,
                            temp_set.Distance,
                            temp_set.Description,
                            temp_set.EnergyGroupName,
                            temp_set.TotalDistance,
                            temp_set.OrderID,
                            temp_set.ParentID
                            ).ToString());
                    }
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                }
                finally
                {
                    conn = null;
                }
            }
            return planid;
        }
        #endregion

        #region Update Workout Plan
        /// <summary>
        /// update workout plan and its sets
        /// </summary>
        /// <param name="plan">workout plan object</param>
        /// <returns> 0 means fail</returns>
        public static int updateWorkoutPlan(WorkoutPlanObject plan)
        {
            int result = 0;
            SqlConnection conn = new SqlConnection(connStr);

            using (SqlTransaction trans = conn.BeginTransaction())
            {
                try
                {
                    SqlHelper.ExecuteNonQuery(trans, "updateWorkOutPlan", plan.tblID,plan.PlanDate,plan.PlanName,plan.TotalDistance,plan.TotalDuration);
                    SqlHelper.ExecuteNonQuery(trans, CommandType.Text, "delete * from tbl_set where planID=" + plan.tblID);

                    WorkoutSetObject temp_set;
                    for (int i = 1; i <= plan.SubSetHashTable.Count; i++)
                    {
                        temp_set = (WorkoutSetObject)plan.SubSetHashTable[i];
                        temp_set.tblID = int.Parse(SqlHelper.ExecuteScalar(trans, "insertWorkoutSet",
                            temp_set.SetType.ToString(),
                            plan.tblID,
                            temp_set.Stroke,
                            temp_set.Pace,
                            temp_set.Rest,
                            temp_set.Duration,
                            temp_set.Distance,
                            temp_set.Description,
                            temp_set.EnergyGroupName,
                            temp_set.TotalDistance,
                            temp_set.OrderID,
                            temp_set.ParentID
                            ).ToString());
                    }
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                }
            }

            return result;
        }
        #endregion

        #region deleteWorkoutPlan
        public static int deleteWorkoutPlan(WorkoutPlanObject plan)
        {
            int result = 0;
            SqlConnection conn = new SqlConnection(connStr);
            using (SqlTransaction trans = conn.BeginTransaction())
            {
                try
                {
                    result = SqlHelper.ExecuteNonQuery(trans, "deleteworkoutplan", plan.tblID);
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                }
            }
            return result;
        }
        #endregion
    }


    #region Comments
    /*    public class SwimWorkoutDBContext
    {
        private SqlConnection conn;
        private SqlCommand cmd;
        private SqlDataReader reader;
        

        public SwimWorkoutDBContext()
        {
           // conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SwimDBConnectionString"].ConnectionString);
        }

        // Gets list of strokes
        public List<WorkoutStroke> getStrokes()
        {
            List<WorkoutStroke> strokes = new List<WorkoutStroke>();

            try
            {
                string connString = ConfigurationManager.ConnectionStrings["SwimDBConnectionString"].ConnectionString;
                conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SwimDBConnectionString"].ConnectionString);
                conn.Open();
                
                cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "getStrokes";
                cmd.CommandType = CommandType.StoredProcedure;
                reader = cmd.ExecuteReader();
                
                while(reader.Read())
                {
                    strokes.Add(new WorkoutStroke(int.Parse(reader["id"].ToString()), 
                                                    reader["Name"].ToString(), 
                                                    reader["Description"].ToString()));
                }

                conn.Close();
            }
            catch (Exception e)
            {
            }
            finally
            {
                
            }

            return strokes;
        }

        // Return Stroke Object of specific ID
        public WorkoutStroke getStrokes(int strokeId)
        {
            WorkoutStroke stroke;

            try
            {
                string connString = ConfigurationManager.ConnectionStrings["SwimDBConnectionString"].ConnectionString;
                conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SwimDBConnectionString"].ConnectionString);
                conn.Open();

                cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "getStroke";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@id", strokeId);
                reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    stroke = new WorkoutStroke(int.Parse(reader["id"].ToString()), reader["Name"].ToString(), reader["Description"].ToString());
                }
                else
                {
                    throw new DataException("multiple rows returned from query");
                }

                conn.Close();

                return stroke;
            }
            catch (Exception e)
            {
                return null;
            }

        }

        // Inserts new Plan
        public int insertWorkoutPlan(String date)
        {
            try
            {
                string connString = ConfigurationManager.ConnectionStrings["SwimDBConnectionString"].ConnectionString;
                conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SwimDBConnectionString"].ConnectionString);
                conn.Open();

                cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "insertWorkoutPlan";
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter parm = cmd.Parameters.Add("@date", SqlDbType.Date);
                parm.Value = Convert.ToDateTime(date);
                
                int planId = (int)cmd.ExecuteScalar();

                conn.Close();

                return planId;
            }
            catch (Exception e)
            {
                return -1;
            }
        }

        // Inserts new set
        public int insertWorkoutSet(WorkoutSet set)
        {
            try
            {
                string connString = ConfigurationManager.ConnectionStrings["SwimDBConnectionString"].ConnectionString;
                conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SwimDBConnectionString"].ConnectionString);
                conn.Open();

                cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "insertWorkoutSet";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@strokeID", set.Stroke.ID);
                cmd.Parameters.Add("@repeats", set.Repeats);
                cmd.Parameters.Add("@distance", set.WorkoutSetDistance);
                cmd.Parameters.Add("@description", set.Description);
                cmd.Parameters.Add("@paceTime", set.PaceTime);
                cmd.Parameters.Add("@restPeriod", set.RestPeriod);
                cmd.Parameters.Add("@E1", set.E1);
                cmd.Parameters.Add("@E2", set.E2);
                cmd.Parameters.Add("@E3", set.E3);
                cmd.Parameters.Add("@S1", set.S1);
                cmd.Parameters.Add("@S2", set.S2);
                cmd.Parameters.Add("@S3", set.S3);
                cmd.Parameters.Add("@REC", set.REC);
                cmd.Parameters.Add("@duration", set.SingleDuration);

                int setId = (int)cmd.ExecuteScalar();

                conn.Close();

                return setId;
            }
            catch (Exception e)
            {
                return -1;
            }
        }

        // Inserts workout member
        public int insertWorkoutPlanMember(int parentId, int childId, int memberOrder)
        {
            try
            {
                string connString = ConfigurationManager.ConnectionStrings["SwimDBConnectionString"].ConnectionString;
                conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SwimDBConnectionString"].ConnectionString);
                conn.Open();

                cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "insertWorkoutMember";
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.Add("@parentId", parentId);
                cmd.Parameters.Add("@childId", childId);
                cmd.Parameters.Add("@memberOrder", memberOrder);

                cmd.ExecuteNonQuery();

               // int workoutMemberId = (int)cmd.ExecuteScalar();

                conn.Close();
                return 1;
                //return workoutMemberId;
            }
            catch (Exception e)
            {
                return -1;
            }
        }

        //This method returns List Of Existing WorkOut Plan Ids From Database
        public List<int> getWorkOutPlanIds() {

            List<int> listOfWorkOutPlanIds = new List<int>();//List to hold returned workout plan ids from stored procedure

            try
            {
                string connString = ConfigurationManager.ConnectionStrings["SwimDBConnectionString"].ConnectionString;
                conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SwimDBConnectionString"].ConnectionString);
                conn.Open();

                cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "getWorkOutPlanIDs";
                cmd.CommandType = CommandType.StoredProcedure;
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    //adding ids to list after converting it to Int
                    listOfWorkOutPlanIds.Add(Convert.ToInt32(reader["ID"]));
                }

                conn.Close();
            }
            catch (Exception)
            {
                //ADDING -1 to List Of returned ids if there is any error
                listOfWorkOutPlanIds.Add(-1);
            }
           

            return listOfWorkOutPlanIds;
        }//end of getWorkOutPlanIds method


        public WorkoutPlan getWorkOutPlan(int id)
        {
            WorkoutPlan workoutplan = new WorkoutPlan();
            List<WorkoutSet> listOfSets = new List<WorkoutSet>();
            try{
                conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SwimDBConnectionString"].ConnectionString);
                
                
                conn.Open();

                cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "getworkoutplan";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@workoutplanID", id);
                reader = cmd.ExecuteReader();


                workoutplan.ID = id;
                
                

                while (reader.Read())
                {
                    if (workoutplan.Date==null || workoutplan.Date=="")
                    {
                        workoutplan.Date = reader["planDate"].ToString();
                    }
                    WorkoutSet WOSet = new WorkoutSet();
                    WorkoutStroke stroke = new WorkoutStroke();

                    WOSet.ID=Convert.ToInt32(reader["Set_Id"]);
                    stroke.ID=Convert.ToInt32(reader["Stroke_Id"]);
                    stroke.Name=reader["Name"].ToString();
                    stroke.Description= reader["Stroke_Desc"].ToString();
                    //listOfValues.Add("Member_Id", reader["Member_Id"].ToString());
                    WOSet.OrderNum=Convert.ToInt32(reader["memberOrder"]);
                  //  listOfValues.Add("planDate",reader["planDate"].ToString());
                    WOSet.Repeats=Convert.ToInt32(reader["repeats"]);
                    WOSet.WorkoutSetDistance=Convert.ToInt32(reader["distance"]);
                    WOSet.Description=reader["Set_Desc"].ToString();
                    WOSet.PaceTime = Convert.ToInt32(reader["paceTime"]);
                    WOSet.RestPeriod=Convert.ToInt32(reader["restPeriod"]);
                    WOSet.Stroke = stroke;
                    WOSet.E1 =Convert.ToInt32(reader["E1"]);
                    WOSet.E2 = Convert.ToInt32(reader["E2"]);
                    WOSet.E3 = Convert.ToInt32(reader["E3"]);
                    WOSet.S1 = Convert.ToInt32(reader["S1"]);
                    WOSet.S2 = Convert.ToInt32(reader["S2"]);
                    WOSet.S3 = Convert.ToInt32(reader["S3"]);
                    WOSet.REC = Convert.ToInt32(reader["REC"]);
                    WOSet.SingleDuration = Convert.ToInt32(reader["duration"]);
                   

                    listOfSets.Add(WOSet);
                }

                workoutplan.WorkoutSet = listOfSets;
                conn.Close();   
              }
              catch (Exception)
              {
                   
                //   List<string> listOfValues = new List<string>();
                //    listOfValues.Add("Error");
                //    listOfSets.Add();
                  return null;
               }

            return workoutplan;
        
        }

        public Boolean deleteWorkOutPlan(int id)
        {

            try
            {
                conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SwimDBConnectionString"].ConnectionString);
                
                conn.Open();

                cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "deleteworkoutplan";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@workoutplanID", id);
                cmd.ExecuteReader();
                
                conn.Close();

                return true;

            }
            catch (Exception)
            {

                //   List<string> listOfValues = new List<string>();
                //    listOfValues.Add("Error");
                //    listOfSets.Add();
                return false;
            }
            
        }


    }
    */
    #endregion

}
