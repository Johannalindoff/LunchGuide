using Microsoft.AspNetCore.Mvc;
using LunchGuide.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace LunchGuide.Controllers
{
    public class AdminController : Controller
    {
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public IActionResult AdminOverview(IFormCollection formAnswer)
        {
            // Skapa user model objekt från indatat i inloggningsfönstret
            UserModel um = new UserModel();
            um.Username = formAnswer["Username"];
            um.Password = formAnswer["Password"];

            // Kontrollera att användaren existerar med hjälp av metoden VerifyUser
            int i = 0;
            string error = "";
            UserMethods ume = new UserMethods();
            UserModel NewUm = new UserModel();
            NewUm = ume.VerifyUser(um, out error);

            // Om användaren existerar är den välkommen till vyn AdminOverview
            if (NewUm != null)
            {
                // Skriver ut användarnamnet till vyn
                ViewBag.name = NewUm.Username;

                // Skapa en sessionsvariabel som heter user som kan hämtas senare
                string s = JsonConvert.SerializeObject(NewUm);
                HttpContext.Session.SetString("usersession", s);

                // Skapa restaurangmodel
                RestaurantModel ReMo = new RestaurantModel();
                RestaurantMethods ReMe = new RestaurantMethods();
                ReMo = ReMe.GetRestaurantInfo(NewUm.Restaurant, out error);

                // Lägg till restaurangmodellen i vymodelllen
                AdminOverviewModel myModel = new AdminOverviewModel();
                myModel.RestaurantInfo = ReMo;

                // Lägg till lista av dishinfo i vymodellen
                DishMethods DiMe = new DishMethods();
                myModel.DishInfo = DiMe.GetListOfDishes(NewUm.Restaurant, out error);

                // Returnera vymodellen
                return View(myModel);
            }
            else
            {
                ViewBag.error = error;
                return View("Login");
            }
        }


        [HttpGet]
        public IActionResult AdminOverview()
        {

            // Hämta sessionsvariabeln
            UserModel um = new UserModel();
            string s = HttpContext.Session.GetString("usersession");
            um = JsonConvert.DeserializeObject<UserModel>(s);

            string error = "";

            // Om användaren existerar är den välkommen till vyn AdminOverview
            if (um != null)
            {
                // Skriver ut användarnamnet till vyn
                ViewBag.name = um.Username;

                // Skapa restaurangmodel
                RestaurantModel ReMo = new RestaurantModel();
                RestaurantMethods ReMe = new RestaurantMethods();
                ReMo = ReMe.GetRestaurantInfo(um.Restaurant, out error);

                // Lägg till restaurangmodellen i vymodelllen
                AdminOverviewModel myModel = new AdminOverviewModel();
                myModel.RestaurantInfo = ReMo;

                // Lägg till lista av dishinfo i vymodellen
                DishMethods DiMe = new DishMethods();
                myModel.DishInfo = DiMe.GetListOfDishes(um.Restaurant, out error);

                // Returnera vymodellen
                return View(myModel);
            }
            else
            {
                ViewBag.error = error;
                return View("Login");
            }
        }


        [HttpGet]
        public ActionResult AddDailyMenu()
        {
            // Hämta sessionsvariabeln
            UserModel um = new UserModel();
            string s = HttpContext.Session.GetString("usersession");
            um = JsonConvert.DeserializeObject<UserModel>(s);

            // Skriver ut användarnamnet till vyn
            ViewBag.name = um.Username;

            // Skriver ut specialdiets till checkboxarna
            DishMethods DiMe = new DishMethods();
            DishModel DiMo = new DishModel();
            DiMo.AviableSD = DiMe.GetSpecialDietList(out string error2);

            return View(DiMo);
        }

        [HttpPost]
        public ActionResult AddDailyMenu(IFormCollection formAnswer)
        {

            // Hämta sessionsvariabeln
            UserModel um = new UserModel();
            string s = HttpContext.Session.GetString("usersession");
            um = JsonConvert.DeserializeObject<UserModel>(s);

            // Skriver ut användarnamnet till vyn
            ViewBag.name = um.Username;

            // Skriver ut specialdiets till checkboxarna
            DishMethods DiMe = new DishMethods();
            DishModel DiMo = new DishModel();
            DiMo.AviableSD = DiMe.GetSpecialDietList(out string error2);

            // Samlar ihop enkätsvar
            string items = formAnswer["CategoryIds"];
            string[] checkedItemsString;
            List<int> checkedItemsInt = new List<int>();

            // De ibockade rutorna kommer som en sträng, dela upp den om omvandla till ints
            if (items != null)
            {
                checkedItemsString = items.Split(new[]
                {
                    ","
                }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < checkedItemsString.Length; i++)
                {
                    checkedItemsInt.Add(Convert.ToInt16(checkedItemsString[i]));
                }
            }

            // Skapa en ny dish-modell utifrån formulärsvaren
            DishModel DiMo2 = new DishModel();
            DiMo2.Dish = formAnswer["Dish"];
            DiMo2.Date = Convert.ToDateTime(formAnswer["Date"]);
            DiMo2.SpecialDietInt = new List<int>();
            DiMo2.SpecialDietInt = checkedItemsInt;

            // Kalla på dish-metoden som lägger till den nya måltiden i databasen
            DishMethods DiMe2 = new DishMethods();
            int addSucceeded = 0;
            addSucceeded = DiMe2.addDish(um, DiMo2, out string error);

            // Skriv ut till vyn om det lyckades eller inte
            ViewBag.antal = addSucceeded;
            ViewBag.error = error;
            ViewBag.dish = DiMo2.Dish;

            return View(DiMo);
        }
    }
}