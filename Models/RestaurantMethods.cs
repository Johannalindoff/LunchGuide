using System;
using System.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;

namespace LunchGuide.Models
{
    public class RestaurantMethods
    {
        public RestaurantMethods() { }

        public RestaurantModel GetRestaurantInfo(int restaurantId, out string errormsg)
        {
            //Skapa SqlConnection
            SqlConnection dbConnection = new SqlConnection();

            //Koppling mot Sql server
            dbConnection.ConnectionString = @"Data Source =.\sqlexpress; Initial Catalog = test; Integrated Security = True";

            //sqlstring och kolla om användaren existerar
            String sqlstring = "SELECT * FROM Tbl_Restaurant WHERE Re_Id = @id";

            SqlCommand dbCommand = new SqlCommand(sqlstring, dbConnection);

            dbCommand.Parameters.Add("id", SqlDbType.NVarChar, 30).Value = restaurantId;

            //Skapa en adapter
            SqlDataAdapter myAdapter = new SqlDataAdapter(dbCommand);
            DataSet myDS = new DataSet();

            try
            {
                dbConnection.Open();
                myAdapter.Fill(myDS, "RestaurantTable");

                int count = 0;
                count = myDS.Tables["RestaurantTable"].Rows.Count;

                if (count > 0)
                {
                    RestaurantModel ReM = new RestaurantModel();
                    ReM.Id = Convert.ToInt16(myDS.Tables["RestaurantTable"].Rows[0]["Re_Id"]);
                    ReM.Name = myDS.Tables["RestaurantTable"].Rows[0]["Re_Name"].ToString();
                    ReM.Address = myDS.Tables["RestaurantTable"].Rows[0]["Re_Address"].ToString();
                    ReM.Phone = myDS.Tables["RestaurantTable"].Rows[0]["Re_Phone"].ToString();
                    ReM.Website = myDS.Tables["RestaurantTable"].Rows[0]["Re_Website"].ToString();
                    ReM.OpeningHours = myDS.Tables["RestaurantTable"].Rows[0]["Re_OpeningHours"].ToString();
                    ReM.Price = Convert.ToInt16(myDS.Tables["RestaurantTable"].Rows[0]["Re_Price"]);
                    ReM.University = Convert.ToInt16(myDS.Tables["RestaurantTable"].Rows[0]["Re_University"]);

                    errormsg = "";
                    return ReM;
                }
                else
                {
                    errormsg = "Det går inte att hämta information om restaurangen";
                    return (null);
                }
            }
            catch (Exception e)
            {
                errormsg = e.Message;
                return null;
            }
            finally
            {
                dbConnection.Close();
            }

        }
    }

    
}
