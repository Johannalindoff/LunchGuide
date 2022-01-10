using System;
using System.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;

namespace LunchGuide.Models
{
    public class DishMethods
    {
        public DishMethods() { }

        public List<DishModel> GetListOfDishes(int restaurantId, out string errormsg)
        {
            //Skapa SqlConnection
            SqlConnection dbConnection = new SqlConnection();

            //Koppling mot Sql server
            dbConnection.ConnectionString = @"Data Source =.\sqlexpress; Initial Catalog = test; Integrated Security = True";

            //sqlstring, hämta information om lunchmeny tillhörande relevant restaurang
            String sqlstring = "SELECT dm.DM_Restaurant, dm.DM_Date, d.Di_Id, d.Di_Name, sd.SD_Type FROM Tbl_DailyMenu as dm INNER JOIN Tbl_DailyMenuHasDish as dmhd ON dm.DM_Id = dmhd.DMHD_DM_Id INNER JOIN Tbl_Dish as d ON dmhd.DMHD_Di_Id = d.Di_Id INNER JOIN Tbl_DishHasSpecialDiet as hsd ON d.Di_Id = hsd.HSD_Di_Id INNER JOIN Tbl_SpecialDiet sd ON sd.SD_Id = hsd.HSD_SD_Id WHERE dm.DM_Restaurant = @id ORDER BY d.Di_Id ASC";

            SqlCommand dbCommand = new SqlCommand(sqlstring, dbConnection);

            dbCommand.Parameters.Add("id", SqlDbType.NVarChar, 30).Value = restaurantId;

            //Skapa en adapter
            SqlDataAdapter myAdapter = new SqlDataAdapter(dbCommand);
            DataSet myDS = new DataSet();
            List<DishModel> DishList = new List<DishModel>();

            try
            {
                dbConnection.Open();
                myAdapter.Fill(myDS, "DishTable");

                int i = 0;
                int count = 0;
                count = myDS.Tables["DishTable"].Rows.Count;

                if (count > 0)
                {
                    // Gå igenom alla rader som ska läggas till
                    while (i < count)
                    {
                        // Skapa en Dishmodel och lägg till det nuvarande id:et
                        DishModel dm = new DishModel();
                        dm.Id = Convert.ToInt16(myDS.Tables["DishTable"].Rows[i]["Di_Id"]);
                        dm.Date = Convert.ToDateTime(myDS.Tables["DishTable"].Rows[i]["DM_Date"]);


                        // Kolla om Dishlistan redan innehåller det nuvarande id:et
                        if (DishList.Any(d => d.Id == dm.Id))
                        {
                            // Kolla vart i Dishlistan det id:et finns
                            int listIndex = DishList.FindIndex(d => d.Id == dm.Id);

                            if (DishList[listIndex].Date == dm.Date)
                            {
                                // Om id:et har en lista som inte innehåller nuvarande allergi, lägg till det
                                if (!(DishList[listIndex].SpecialDietString.Contains(myDS.Tables["DishTable"].Rows[i]["SD_Type"].ToString())))
                                {
                                    DishList[listIndex].SpecialDietString.Add(myDS.Tables["DishTable"].Rows[i]["SD_Type"].ToString() + " ");
                                }
                            }    
                        }
                        // Om Dishlistan inte innehåller det nuvarande id:et, lägg till maträtten
                        else
                        {
                            dm.Dish = myDS.Tables["DishTable"].Rows[i]["Di_Name"].ToString();
                            dm.SpecialDietString = new List<String>();
                            dm.SpecialDietString.Add(myDS.Tables["DishTable"].Rows[i]["SD_Type"].ToString() + " ");

                            DishList.Add(dm);
                        }

                        // Kolla nästa rad
                        i++;
                    }
                    errormsg = "";
                    return DishList;
                }
                else
                {
                    errormsg = "Det går inte att hämta information om rätterna";
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


        public int addDish(UserModel um, DishModel DiMo, out string errormsg)
        {

            // Börja med att lägga till rätten i dish-tabellen
            SqlConnection dbConnection = new SqlConnection();
            dbConnection.ConnectionString = @"Data Source =.\sqlexpress; Initial Catalog = test; Integrated Security = True";
            String sqlstring = "INSERT INTO [Tbl_Dish]([Di_Name]) VALUES(@dishname)";
            SqlCommand dbCommand = new SqlCommand(sqlstring, dbConnection);
            dbCommand.Parameters.Add("dishname", SqlDbType.NVarChar, 30).Value = DiMo.Dish;

            try
            {
                dbConnection.Open();
                int i = 0;
                i = dbCommand.ExecuteNonQuery();
                if (i == 1)
                {
                    errormsg = "";
                }
                else
                {
                    errormsg = "Det går inte att lägga till maträtten i dishtabellen";
                    return (i);
                }
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

            // Lägg till en dailymenu
            sqlstring = "INSERT INTO [Tbl_DailyMenu] ([DM_Date], [DM_Restaurant]) VALUES(@date, @restaurant)";
            dbCommand = new SqlCommand(sqlstring, dbConnection);
            dbCommand.Parameters.Add("date", SqlDbType.DateTime).Value = DiMo.Date;
            dbCommand.Parameters.Add("restaurant", SqlDbType.Int).Value = um.Restaurant;

            try
            {
                dbConnection.Open();
                int i = 0;
                i = dbCommand.ExecuteNonQuery();
                if (i == 1)
                {
                    errormsg = "";
                }
                else
                {
                    errormsg = "Det går inte att lägga till en dailymenu i tabellen";
                    return (i);
                }
                
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

            // Loopa igenom rättens allergier
            int x = 0;
            List<int> listOfSD = DiMo.SpecialDietInt;
            int test = listOfSD[x];
            while (x < listOfSD.Count)
            {
                // Lägg till maträttens allergier
                sqlstring = "INSERT INTO [Tbl_DishHasSpecialDiet] ([HSD_Di_Id], [HSD_SD_Id]) VALUES((SELECT Di_Id FROM[Tbl_Dish] WHERE Di_Name = @dishname), @sdid)";
                dbCommand = new SqlCommand(sqlstring, dbConnection);
                dbCommand.Parameters.Add("dishname", SqlDbType.NVarChar, 30).Value = DiMo.Dish;
                dbCommand.Parameters.Add("sdid", SqlDbType.Int).Value = listOfSD[x];

                try
                {
                    dbConnection.Open();
                    int i = 0;
                    i = dbCommand.ExecuteNonQuery();
                    if (i == 1)
                    {
                        errormsg = "";
                    }
                    else
                    {
                        errormsg = "Det går inte att lägga till allergin till maträtten";
                        return (i);
                    }

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

                x++;
            }

            
            // Lägg till att dailymenu har maträtten
            sqlstring = "INSERT INTO [Tbl_DailyMenuHasDish] ([DMHD_DM_Id], [DMHD_Di_Id]) VALUES ((SELECT TOP 1 DM_Id FROM[Tbl_DailyMenu] ORDER BY DM_Id DESC), (SELECT TOP 1 Di_Id FROM[Tbl_Dish] ORDER BY Di_Id DESC))";
            dbCommand = new SqlCommand(sqlstring, dbConnection);

            try
            {
                dbConnection.Open();
                int i = 0;
                i = dbCommand.ExecuteNonQuery();
                if (i == 1)
                {
                    errormsg = "";
                    return (i);
                }
                else
                {
                    errormsg = "Det går inte att lägga till allergin till maträtten";
                    return (i);
                }

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