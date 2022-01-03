using System;
using System.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;

namespace LunchGuide.Models
{
    public class UserMethods
    {
        public UserMethods()
        {
        }

        public UserModel VerifyUser (UserModel um, out string errormsg)
        {
            //Skapa SqlConnection
            SqlConnection dbConnection = new SqlConnection();

            //Koppling mot Sql server
            dbConnection.ConnectionString = @"Data Source =.\sqlexpress; Initial Catalog = test; Integrated Security = True";

            //sqlstring och kolla om användaren existerar
            String sqlstring = "SELECT * FROM Tbl_User WHERE Us_Username = @username AND Us_Password = @password";

            SqlCommand dbCommand = new SqlCommand(sqlstring, dbConnection);

            dbCommand.Parameters.Add("username", SqlDbType.NVarChar, 30).Value = um.Username;
            dbCommand.Parameters.Add("password", SqlDbType.NVarChar, 30).Value = um.Password;

            //Skapa en adapter
            SqlDataAdapter myAdapter = new SqlDataAdapter(dbCommand);
            DataSet myDS = new DataSet();

            try
            {
                dbConnection.Open();
                myAdapter.Fill(myDS, "UserTable");
               
                int count = 0;
                count = myDS.Tables["UserTable"].Rows.Count;

                if (count > 0)
                {
                    // En ny modell skapas för att sedan returneras
                    UserModel NewUm = new UserModel();
                    NewUm.Id = Convert.ToInt16(myDS.Tables["UserTable"].Rows[0]["Us_Id"]);
                    NewUm.Username = myDS.Tables["UserTable"].Rows[0]["Us_Username"].ToString();
                    NewUm.Password = myDS.Tables["UserTable"].Rows[0]["Us_Password"].ToString();
                    NewUm.Restaurant = Convert.ToInt16(myDS.Tables["UserTable"].Rows[0]["Us_Restaurant"]);

                    errormsg = "";
                    return NewUm;
                }
                else
                {
                    errormsg = "Användarnamn eller lösenord är felaktigt";
                    return (null);
                }
            }
            catch (Exception e)
            {
                errormsg = e.Message;
                return (null);
            }
            finally
            {
                dbConnection.Close();
            }

        }
    }
}
