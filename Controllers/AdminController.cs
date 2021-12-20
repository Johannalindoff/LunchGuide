using Microsoft.AspNetCore.Mvc;
using LunchGuide.Models;
using User.Models; // Är detta visual studios egna?
using Microsoft.AspNetCore.Http;

namespace LunchGuide.Controllers
{
    public class AdminController : Controller
    {
        public ActionResult Login()
        {
            return View();
        }
    }
}
