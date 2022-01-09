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
                ReMo = ReMe.GetRestaurantInfo( NewUm.Restaurant, out error);

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
        public ActionResult AddDailyMenu(FormCollection formAnswer)
        {
            // inparametrar: string Type, DishModel dimo
            //int i = Convert.ToInt32(Type);
            //ViewData["Type"] = i;

            string[] checkedItems;
            List<String> itemString = new List<String>();
            string items = formAnswer["CategoryIds"];

            if (items != null)
            {
                checkedItems = items.Split(new[]
                {
                    ","
                }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < checkedItems.Length; i++)
                {
                    itemString.Add(checkedItems[i]);
                }
            }

            // Skapa en ny dish-modell utifrån formulärsvaren
            DishModel DiMo = new DishModel();
            DiMo.Dish = formAnswer["Dish"];
            DiMo.SpecialDiet = new List<string>();
            // Kan man skriva såhär till en lista? ELler måste man lägga till objekt för objekt?
            DiMo.SpecialDiet = itemString;
            DiMo.Date = Convert.ToDateTime(formAnswer["Date"]);

            // Kalla på dish-metoden som lägger till den nya måltiden i databasen
            DishMethods DiMe = new DishMethods();



            return View();
        }
    }
}
