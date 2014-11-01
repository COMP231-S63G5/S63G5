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
                parm.Value = new DateTime(int.Parse(date.Substring(6,4)),int.Parse(date.Substring(0,2)),int.Parse(date.Substring(3,2)));
                
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

                int workoutMemberId = (int)cmd.ExecuteScalar();

                conn.Close();

                return workoutMemberId;
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


        public List<Dictionary<string, string>> getWorkOutPlan(int id)
        {

            List<Dictionary<string, string>> listOfSets = new List<Dictionary<string, string>>();
            try{
                conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SwimDBConnectionString"].ConnectionString);
                
                
                conn.Open();

                cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "getworkoutplan";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@workoutplanID", id);
                reader = cmd.ExecuteReader();
            
                while (reader.Read())
                {

                    Dictionary<string, string> listOfValues = new Dictionary<string,string>();
                    listOfValues.Add("WorkOutPlan_ID", reader["WorkOutPlan_ID"].ToString());
                    listOfValues.Add("Set_Id",reader["Set_Id"].ToString());
                    listOfValues.Add("Stroke_Id",reader["Stroke_Id"].ToString());
                    listOfValues.Add("Name",reader["Name"].ToString());
                    listOfValues.Add("Stroke_Desc", reader["Stroke_Desc"].ToString());
                    listOfValues.Add("Member_Id", reader["Member_Id"].ToString());
                    listOfValues.Add("memberOrder", reader["memberOrder"].ToString());
                    listOfValues.Add("planDate",reader["planDate"].ToString());
                    listOfValues.Add("repeats",reader["repeats"].ToString());
                    listOfValues.Add("distance",reader["distance"].ToString());
                    listOfValues.Add("Set_Desc",reader["Set_Desc"].ToString());
                    listOfValues.Add("paceTime",reader["paceTime"].ToString());
                    listOfValues.Add("restPeriod",reader["restPeriod"].ToString());
                    listOfSets.Add(listOfValues);
                }

                conn.Close();   
              }
              catch (Exception)
              {
                   
                //   List<string> listOfValues = new List<string>();
                //    listOfValues.Add("Error");
                //    listOfSets.Add();
                  return null;
               }

            return listOfSets;
        
        }

    }
}
