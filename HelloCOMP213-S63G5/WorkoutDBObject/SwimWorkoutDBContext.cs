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


namespace WorkoutDBObject
{
    public class SwimWorkoutDBContext
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
}
