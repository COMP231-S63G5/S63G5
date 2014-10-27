using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;


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
        public List<string> getStrokes()
        {
            List<string> strokes = new List<string>();

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
                    strokes.Add(reader["Name"].ToString());
                }

                conn.Close();
            }
            catch (Exception e)
            {
                strokes.Add(e.Message);
            }
            finally
            {
                
            }

            return strokes;
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
