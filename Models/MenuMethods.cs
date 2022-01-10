using System;
using System.Data;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;

namespace LunchGuide.Models
{
    public class MenuMethods
    {
        public MenuMethods() { }



        public List<MenuModel> GetMenus(int universityId, out string errormsg)
        {
            //Skapa SqlConnection
            SqlConnection dbConnection = new SqlConnection();

            //Koppling mot Sql server
            dbConnection.ConnectionString = @"Data Source =.\sqlexpress; Initial Catalog = test; Integrated Security = True";

            //sqlstring, hämta information om lunchmeny tillhörande relevant restaurang
            String sqlstring = "SELECT re.Re_Name, re.Re_Id, dm.DM_Id,dmhd.DMHD_DI_Id, di.Di_Name FROM Tbl_Restaurant as re INNER JOIN Tbl_DailyMenu as dm ON dm.DM_Restaurant = re.Re_Id AND convert(varchar(10), DM_Date, 102) = convert(varchar(10), getdate(), 102) INNER JOIN Tbl_DailyMenuHasDish as dmhd ON dm.DM_Id = dmhd.DMHD_DM_Id INNER JOIN Tbl_Dish as di ON dmhd.DMHD_Di_Id = di.Di_Id WHERE re.Re_University = @universityId;";

            SqlCommand dbCommand = new SqlCommand(sqlstring, dbConnection);

            dbCommand.Parameters.Add("universityId", SqlDbType.NVarChar, 30).Value = universityId;

            //Skapa en adapter
            SqlDataAdapter myAdapter = new SqlDataAdapter(dbCommand);
            DataSet myDS = new DataSet();
            List<MenuModel> MenuList = new List<MenuModel>();

            try

            {
                dbConnection.Open();
                myAdapter.Fill(myDS, "MenuTable");
                int i = 0;
                int count = 0;
                count = myDS.Tables["MenuTable"].Rows.Count;

                if (count > 0)
                {
                    while (i < count)
                    {
                        MenuModel MeMo = new MenuModel();

                        //Id på restaurangen
                        MeMo.Id = Convert.ToInt16(myDS.Tables["MenuTable"].Rows[i]["Re_Id"]);
                        MeMo.Restaurant = myDS.Tables["MenuTable"].Rows[i]["Re_Name"].ToString();
                        MeMo.Dish = myDS.Tables["MenuTable"].Rows[i]["Di_Name"].ToString();

                        MenuList.Add(MeMo);
                        i++;

                    }

                    errormsg = "";
                    return MenuList;

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





        ////Hämta alla restauranger med namn och id och lägg i en lista 
        //public List<MenuModel> GetRestaurant(int universityId, out string errormsg2)
        //{
        //    //Skapa SqlConnection
        //    SqlConnection dbConnection = new SqlConnection();

        //    //Koppling mot Sql server
        //    dbConnection.ConnectionString = @"Data Source =.\sqlexpress; Initial Catalog = test; Integrated Security = True";

        //    //sqlstring, hämta information om lunchmeny tillhörande relevant restaurang
        //    String sqlstring = "SELECT * FROM Tbl_Restaurant WHERE Re_University = @universityId;";

        //    SqlCommand dbCommand = new SqlCommand(sqlstring, dbConnection);

        //    dbCommand.Parameters.Add("universityId", SqlDbType.NVarChar, 30).Value = universityId;

        //    //Skapa en adapter
        //    SqlDataAdapter myAdapter = new SqlDataAdapter(dbCommand);
        //    DataSet myDS = new DataSet();
        //    List<ViewMenuModel> RestaurantList = new List<ViewMenuModel>();

        //    try

        //    {
        //        dbConnection.Open();
        //        myAdapter.Fill(myDS, "MenuTable");
        //        int i = 0;
        //        int count = 0;
        //        count = myDS.Tables["MenuTable"].Rows.Count;

        //        if (count > 0)
        //        {
        //            while (i < count)
        //            {
        //                ViewMenuModel ViMeMo = new ViewMenuModel();

        //                //Id på restaurangen
        //                ViMeMo.Restaurant_Id = Convert.ToInt16(myDS.Tables["MenuTable"].Rows[i]["Re_Id"]);
        //                ViMeMo.Restaurant_Name = myDS.Tables["MenuTable"].Rows[i]["Re_Name"].ToString();


        //                RestaurantList.Add(ViMeMo);
        //                i++;

        //            }

        //            errormsg = "";
        //            return RestaurantList;

        //        }
        //        else
        //        {
        //            errormsg = "Det går inte att hämta information om restaurangen";
        //            return (null);
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        errormsg = e.Message;
        //        return null;
        //    }
        //    finally
        //    {
        //        dbConnection.Close();
        //    }
        //}


        //public List<ViewMenuModel> GetDailyMenu(int restaurantId, out string errormsg)
        //{
        //    //Skapa SqlConnection
        //    SqlConnection dbConnection = new SqlConnection();

        //    //Koppling mot Sql server
        //    dbConnection.ConnectionString = @"Data Source =.\sqlexpress; Initial Catalog = test; Integrated Security = True";

        //    //sqlstring, hämta information om lunchmeny tillhörande relevant restaurang
        //    String sqlstring = "SELECT dm.DM_Restaurant, dm.DM_Date, d.Di_Id, d.Di_Name, sd.SD_Type FROM Tbl_DailyMenu as dm INNER JOIN Tbl_DailyMenuHasDish as dmhd ON dm.DM_Id = dmhd.DMHD_DM_Id INNER JOIN Tbl_Dish as d ON dmhd.DMHD_Di_Id = d.Di_Id INNER JOIN Tbl_DishHasSpecialDiet as hsd ON d.Di_Id = hsd.HSD_Di_Id INNER JOIN Tbl_SpecialDiet sd ON sd.SD_Id = hsd.HSD_SD_Id WHERE dm.DM_Restaurant = @id ORDER BY d.Di_Id ASC";

        //    SqlCommand dbCommand = new SqlCommand(sqlstring, dbConnection);

        //    dbCommand.Parameters.Add("id", SqlDbType.NVarChar, 30).Value = restaurantId;

        //    //Skapa en adapter
        //    SqlDataAdapter myAdapter = new SqlDataAdapter(dbCommand);
        //    DataSet myDS = new DataSet();
        //    List<ViewMenuModel> DailyMenuList = new List<ViewMenuModel>();

        //    try
        //    {
        //        dbConnection.Open();
        //        myAdapter.Fill(myDS, "MenuTable");

        //        int i = 0;
        //        int count = 0;
        //        count = myDS.Tables["MenuTable"].Rows.Count;

        //        if (count > 0)
        //        {
        //            // Gå igenom alla rader som ska läggas till
        //            while (i < count)
        //            {
        //                // Skapa en ViewMenumodel och lägg till det nuvarande id:et
        //                ViewMenuModel ViMeMo = new ViewMenuModel();

        //                ViMeMo.Date = Convert.ToDateTime(myDS.Tables["MenuTable"].Rows[i]["DM_Date"]);


        //                // Kolla om Dishlistan redan innehåller det nuvarande id:et
        //                if (DailyMenuList.Any(d => d.Id == ViMeMo.Restaurant_Id))
        //                {
        //                    // Kolla vart i Dishlistan det id:et finns
        //                    int listIndex = DailyMenuList.FindIndex(d => d.Id == ViMeMo.Restarant_Id);

        //                    if (DailyMenuList[listIndex].Date == dm.Date)
        //                    {
        //                        // Om id:et har en lista som inte innehåller nuvarande allergi, lägg till det
        //                        if (!(DailyMenuList[listIndex].SpecialDiet.Contains(myDS.Tables["MenuTable"].Rows[i]["SD_Type"].ToString())))
        //                        {
        //                            DailyMenuList[listIndex].SpecialDiet.Add(myDS.Tables["MenuTable"].Rows[i]["SD_Type"].ToString() + " ");
        //                        }
        //                    }
        //                }
        //                // Om Dishlistan inte innehåller det nuvarande id:et, lägg till maträtten
        //                else
        //                {
        //                    dm.Dish = myDS.Tables["DishTable"].Rows[i]["Di_Name"].ToString();
        //                    dm.SpecialDiet = new List<String>();
        //                    dm.SpecialDiet.Add(myDS.Tables["DishTable"].Rows[i]["SD_Type"].ToString() + " ");

        //                    DishList.Add(dm);
        //                }

        //                // Kolla nästa rad
        //                i++;
        //            }
        //            errormsg = "";
        //            return DishList;
        //        }
        //        else
        //        {
        //            errormsg = "Det går inte att hämta information om rätterna";
        //            return (null);
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        errormsg = e.Message;
        //        return null;
        //    }
        //    finally
        //    {
        //        dbConnection.Close();
        //    }
        //}


        public int[] GetIdOfRestaurants(int universityId, out string errormsg4)
        {
            //Skapa SqlConnection
            SqlConnection dbConnection = new SqlConnection();

            //Koppling mot Sql server
            dbConnection.ConnectionString = @"Data Source =.\sqlexpress; Initial Catalog = test; Integrated Security = True";

            String sqlstring = "SELECT Re_Id FROM Tbl_Restaurant WHERE Re_University = @universityId";

            SqlCommand dbCommand = new SqlCommand(sqlstring, dbConnection);

            dbCommand.Parameters.Add("universityId", SqlDbType.NVarChar, 30).Value = universityId;



            SqlDataReader reader = null;
            int antal = 0;
            int i = 0;


            //Skapa en adapter
            SqlDataAdapter myAdapter = new SqlDataAdapter(dbCommand);
            DataSet myDS = new DataSet();

            try
            {
                dbConnection.Open();
                myAdapter.Fill(myDS, "RestaurantTable");

                int count = 0;
                count = myDS.Tables["RestaurantTable"].Rows.Count;
                int[] idArray = new int[count];

                if (count > 0)
                {
                    while (i < count)
                    {

                        idArray[i] = Convert.ToInt16(myDS.Tables["RestaurantTable"].Rows[i]["Re_Id"]);

                        i++;
                    }




                    errormsg4 = "";
                    return idArray;
                }
                else
                {
                    errormsg4 = "Det går inte att hämta information om restaurangen";
                    return (null);
                }

            }
            catch (Exception e)
            {
                errormsg4 = e.Message;
                return null;
            }
            finally
            {
                dbConnection.Close();
            }
        }



        public RestaurantModel GetRestaurantInfo(int id, out string errormsg3)
        {
            //Skapa SqlConnection
            SqlConnection dbConnection = new SqlConnection();

            //Koppling mot Sql server
            dbConnection.ConnectionString = @"Data Source =.\sqlexpress; Initial Catalog = test; Integrated Security = True";

            String sqlstring = "SELECT * FROM Tbl_Restaurant WHERE Re_Id = @id";

            SqlCommand dbCommand = new SqlCommand(sqlstring, dbConnection);

            dbCommand.Parameters.Add("id", SqlDbType.NVarChar, 30).Value = id;

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
                    ReM.Price = Convert.ToInt16(myDS.Tables["RestaurantTable"].Rows[0]["Re_Price"]);


                    errormsg3 = "";
                    return ReM;
                }
                else
                {
                    errormsg3 = "Det går inte att hämta information om restaurangen";
                    return (null);
                }
            }
            catch (Exception e)
            {
                errormsg3 = e.Message;
                return null;
            }
            finally
            {
                dbConnection.Close();
            }
        }
    }
}
