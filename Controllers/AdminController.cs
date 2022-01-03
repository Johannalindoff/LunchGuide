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

            // Borde kontrolleras att detta finns i databasen innan man returnerar den här vyn
            // Om det inte finns borde login vyn returneras igen med ett errormsg
            int i = 0;
            string error = "";
            
            UserMethods ume = new UserMethods();
            i = ume.VerifyUser(um, out error);

            ViewBag.error = error;
            ViewBag.exists = i;


            // Här skapas sessionsvariabel som heter user som kan hämtas senare
            string s = JsonConvert.SerializeObject(um);
            HttpContext.Session.SetString("usersession", s);

            // Skapar viewbag
            ViewBag.name = um.Username;

            // Hämta data tillhörande användaren från databasen med hjälp av nån metod och skriv ut

            return View();
        }
    }
}
