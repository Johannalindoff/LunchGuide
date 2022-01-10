using System;
using System.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;

namespace LunchGuide.Models
{
    public class UniversityMethods
    {
        public UniversityMethods() { }

        public List<UniversityModel> GetUniversity(out string errormsg)
        {
            //Skapa SqlConnection
            SqlConnection dbConnection = new SqlConnection();


            //Koppling mot Sql server
            dbConnection.ConnectionString = @"Data Source =.\sqlexpress; Initial Catalog = test; Integrated Security = True";

            //sql string för att hämta alla Uni
            String sqlstring = "Select * From Tbl_University";
            SqlCommand dbCommand = new SqlCommand(sqlstring, dbConnection);


            //Skapa en adapter 
            SqlDataAdapter myAdapter = new SqlDataAdapter(dbCommand);
            DataSet myDS = new DataSet();
            List<UniversityModel> ListOfUni = new List<UniversityModel>();

            try
            {
                dbConnection.Open();

                //Fyller dataset med data i en tabell med ett annat 
                myAdapter.Fill(myDS, "UniTable");
                int count = 0;
                int i = 0;

                count = myDS.Tables["UniTable"].Rows.Count;
                if (count > 0)
                {
                    while (i < count)
                    {
                        UniversityModel UnMo = new UniversityModel();
                        UnMo.Uni_Name = myDS.Tables["UniTable"].Rows[i]["Uni_Name"].ToString();
                        UnMo.Uni_Id = Convert.ToInt16(myDS.Tables["UniTable"].Rows[i]["Uni_Id"]);

                        i++;
                        ListOfUni.Add(UnMo);
                    }
                    errormsg = "";
                    return ListOfUni;
                }
                else
                {
                    errormsg = "Det hämtas ingen todo";
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
