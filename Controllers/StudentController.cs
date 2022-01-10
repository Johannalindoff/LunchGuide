using Microsoft.AspNetCore.Mvc;
using LunchGuide.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace LunchGuide.Controllers
{
    public class StudentController : Controller
    {


        [HttpGet]
        public IActionResult Start()
        {
            List<UniversityModel> ListOfUni = new List<UniversityModel>();
            UniversityMethods UnMe = new UniversityMethods();
            UniversityModel UnMo = new UniversityModel();
            string error = "";
            ListOfUni = UnMe.GetUniversity(out error);
            ViewBag.error = error;

            // Skapa en sessionsvariabel som heter uni som kan hämtas senare
            string s = JsonConvert.SerializeObject(ListOfUni);
            HttpContext.Session.SetString("session", s);

            return View(ListOfUni);


        }


        [HttpGet]
        public IActionResult Menu(int universityId)
        {


            MenuMethods MeMe = new MenuMethods();

            //Delkarera en lista med vymodeller
            List<ViewMenuModel> MenuModelList = new List<ViewMenuModel>();

            //Metoden ska hämta en array med idn

            int[] idArray = MeMe.GetIdOfRestaurants(universityId, out string errormsg4);

            for (int i = 0; i < idArray.Length; i++)
            {
                ViewMenuModel myModel = new ViewMenuModel();
                RestaurantModel ReMo = new RestaurantModel();
                DishMethods DiMe = new DishMethods();

                ReMo = MeMe.GetRestaurantInfo(idArray[i], out string errormsg3);
                myModel.Restaurant = ReMo;

                myModel.DishList = DiMe.GetListOfDishes(idArray[i], out string errormsg);

                MenuModelList.Add(myModel);

            }

            AllMenuViewModel AlMeViMo = new AllMenuViewModel();
            // kanske att den här listan måste deklareras först?
            AlMeViMo.listOfMenus = MenuModelList;

            // Vi vill returnera en lista fylld med modeller av (adminoverviewmodel) dvs viewmenumodel
            ViewBag.mml = MenuModelList;
            ViewData["Lista"] = MenuModelList;
            return View(AlMeViMo);

        }
    }
}




