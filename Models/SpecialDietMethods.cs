using System;
using System.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;

namespace LunchGuide.Models
{
    public class SpecialDietMethods
    {
        public SpecialDietMethods() { }

        public List<SpecialDietModel> GetSpecialDietList(out string errormsg)
        {
            //Skapa SqlConnection
            SqlConnection dbConnection = new SqlConnection();

            //Koppling mot Sql server
            dbConnection.ConnectionString = @"Data Source =.\sqlexpress; Initial Catalog = test; Integrated Security = True";

            String sqlstring = "SELECT * FROM Tbl_SpecialDiet";

            SqlCommand dbCommand = new SqlCommand(sqlstring, dbConnection);

            //Skapa en adapter
            SqlDataAdapter myAdapter = new SqlDataAdapter(dbCommand);
            DataSet myDS = new DataSet();
            List<SpecialDietModel> sdList = new List<SpecialDietModel>();

            try
            {
                dbConnection.Open();
                myAdapter.Fill(myDS, "SpecialDietTable");

                int i = 0;
                int count = 0;
                count = myDS.Tables["SpecialDietTable"].Rows.Count;

                if (count > 0)
                {
                    while (i < count)
                    {
                        SpecialDietModel sdMo = new SpecialDietModel();
                        sdMo.Id = Convert.ToInt16(myDS.Tables["SpecialDietTable"].Rows[i]["SD_Id"]);
                        sdMo.Type = myDS.Tables["SpecialDietTable"].Rows[i]["SD_Type"].ToString();

                        sdList.Add(sdMo);
                        i++;
                    }

                    errormsg = "";
                    return sdList;
                }
                else
                {
                    errormsg = "Det går inte att hämta information om allergier";
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