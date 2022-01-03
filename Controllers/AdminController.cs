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

                // Hämta relevant information till användaren
                RestaurantModel ReMo = new RestaurantModel();
                RestaurantMethods ReMe = new RestaurantMethods();
                ReMo = ReMe.GetRestaurantInfo( NewUm.Restaurant, out error);


                return View(ReMo);
            }
            else
            {
                ViewBag.error = error;
                return View("Login");
            }
        }
    }
}
