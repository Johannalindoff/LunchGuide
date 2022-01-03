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

        public int VerifyUser (UserModel um, out string errormsg)
        {
            //Skapa SqlConnection
            SqlConnection dbConnection = new SqlConnection();

            //Koppling mot Sql server
            dbConnection.ConnectionString = @"Data Source =.\sqlexpress; Initial Catalog = test; Integrated Security = True";

            //sqlstring och kolla om användaren existerar
            String sqlstring = "SELECT Us_Id FROM Tbl_User WHERE Us_Username = @username AND Us_Password = @password";

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
                int exist = 0;
                int count = 0;
                
                count = myDS.Tables["UserTable"].Rows.Count;
                if (count > 0)
                {
                    exist = 1;
                    errormsg = "";
                }
                else
                {
                    errormsg = "Användarnamn eller lösenord är felaktigt";
                }
                return (exist);

            }
            catch (Exception e)
            {
                errormsg = e.Message;
                return 0;
            }
            finally
            {
                dbConnection.Close();
            }

        }
    }
}
