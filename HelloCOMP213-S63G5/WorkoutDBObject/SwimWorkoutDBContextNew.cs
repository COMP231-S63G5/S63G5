using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkoutPlanObjects;

namespace WorkoutDBObject
{
    public class SwimWorkoutDBContextNew
    {
        private SqlConnection conn;
        private SqlCommand cmd;
        private SqlDataReader reader;
        public string connString = "Data Source=Hiren; Initial Catalog=SwimWorkoutsDB; Integrated Security=True; User Id=coach; Password=swimdb;";

        //This method returns Unique Strokes existing in Set table
        public List<string> getStrokes() {
            List<string> listOfStrokes = new List<string>();

            try
            {
                string connString ="Data Source=Hiren; Initial Catalog=SwimWorkoutsDB; Integrated Security=True; User Id=coach; Password=swimdb;";
             //   conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SwimDBConnectionString"].ConnectionString);
               // conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SwimDBConnectionString"].ConnectionString);
                conn = new SqlConnection(connString);
                conn.Open();

                cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "getStrokes";
                cmd.CommandType = CommandType.StoredProcedure;
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    listOfStrokes.Add(reader[0].ToString());
                }

                conn.Close();
            }
            catch (Exception)
            {
                listOfStrokes.Add("Error Occured");
               // listOfStrokes.Add(e.);
            }
         
            return listOfStrokes;        
        }


         //This method returns List Of Existing WorkOut Plan Ids From Database
        public List<int> getWorkOutPlanIds()
        {

            List<int> listOfWorkOutPlanIds = new List<int>();//List to hold returned workout plan ids from stored procedure

            try
            {
                //string connString = ConfigurationManager.ConnectionStrings["SwimDBConnectionString"].ConnectionString;
               // conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SwimDBConnectionString"].ConnectionString);
                conn = new SqlConnection(connString);
                conn.Open();

                cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "getWorkOutPlanIDs";
                cmd.CommandType = CommandType.StoredProcedure;
                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    //adding ids to list after converting it to Int
                    listOfWorkOutPlanIds.Add(Convert.ToInt32(reader["planID"]));
                }

                conn.Close();
            }
            catch (Exception e)
            {
                //ADDING -1 to List Of returned ids if there is any error
                 //listOfWorkOutPlanIds.Add(-1);
                throw new Exception(e.Message);
            }
            return listOfWorkOutPlanIds;
        }



        public string insertWorkoutPlan(String date,int totalDistance,string totalDuration)
        {
            try
            {
               // string connString = ConfigurationManager.ConnectionStrings["SwimDBConnectionString"].ConnectionString;
              //  conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SwimDBConnectionString"].ConnectionString);
                conn = new SqlConnection(connString);
                conn.Open();

                cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "addWorkOutPlan";
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter parm = cmd.Parameters.Add("@plandate", SqlDbType.Date);
                parm.Value = Convert.ToDateTime(date);
                cmd.Parameters.AddWithValue("@ttl_distance", totalDistance);
                cmd.Parameters.AddWithValue("@ttl_duration", totalDuration);         

                int planId = (int)cmd.ExecuteScalar();

                conn.Close();

                return planId.ToString();
            }
            catch (Exception e)
            {
                return e.Message.ToString();
            }
        }


        public WorkoutPlanObject getWorkOutPlan(int id)
        {
            WorkoutPlanObject newplan;
            List<WorkoutSetObject> listOfSets = new List<WorkoutSetObject>();
            int workoutplanID=0;
            int ttlplandis=0;
            int ttlplandur=0;
            DateTime plandate=Convert.ToDateTime(1000-10-10);

            try
            {
                conn = new SqlConnection(connString);
                conn.Open();
                cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "getworkoutplan";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@workoutplanID", id);
                reader = cmd.ExecuteReader();
 

                while (reader.Read())
                {
                    if(workoutplanID==null){
                        workoutplanID = Convert.ToInt32(reader["planID"]);
                    }
                    if(ttlplandis==null){
                        ttlplandis = Convert.ToInt32(reader["totalPlanDistance"]);
                    }
                    if (ttlplandur == null)
                    {
                        ttlplandur = Convert.ToInt32(reader["totalDuration"]);
                    }
                    if(plandate==null){
                        plandate =Convert.ToDateTime(reader["planDate"]);
                    }


                    WorkoutSetObject newset = new WorkoutSetObject(
                        Convert.ToInt32(reader["setID"]),
                        reader["setType"].ToString(),
                        Convert.ToInt32(reader["repeats"]),
                        Convert.ToInt32(reader["distance"]),
                        reader["stroke"].ToString(),
                        reader["pace"].ToString(),
                        reader["rest"].ToString(),
                        reader["duration"].ToString(),
                        reader["description"].ToString(),
                        reader["energyName"].ToString(),
                        Convert.ToInt32(reader["totalDistance"]),
                        Convert.ToInt32(reader["orderID"]),
                        Convert.ToInt32(reader["parentID"])
                        );

                    listOfSets.Add(newset);
                }

                newplan = new WorkoutPlanObject(workoutplanID, "", plandate, listOfSets);
                
                conn.Close();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            return newplan;

        }

        public Boolean deleteWorkOutPlan(int id)
        {
            try
            {   
               // conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SwimDBConnectionString"].ConnectionString);
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
                return false;
            }

        }
    }
}
