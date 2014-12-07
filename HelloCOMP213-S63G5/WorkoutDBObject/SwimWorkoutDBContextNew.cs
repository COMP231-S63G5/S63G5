using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkoutDBObject
{
    public class SwimWorkoutDBContextNew
    {
        private SqlConnection conn;
        private SqlCommand cmd;
        private SqlDataReader reader;

        //This method returns Unique Strokes existing in Set table
        public List<string> getStrokes() {
            List<string> listOfStrokes = new List<string>();

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
        }



        public string insertWorkoutPlan(String date,int totalDistance,string totalDuration)
        {
            try
            {
                string connString = ConfigurationManager.ConnectionStrings["SwimDBConnectionString"].ConnectionString;
                conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SwimDBConnectionString"].ConnectionString);
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
                return false;
            }

        }
    }
}
