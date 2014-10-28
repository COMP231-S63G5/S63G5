using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

using WorkoutPlanObjects;


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
            finally
            {

            }

            return listOfWorkOutPlanIds;
        }//end of getWorkOutPlanIds method



    }
}
