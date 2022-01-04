using Microsoft.AspNetCore.Mvc;
using LunchGuide.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace LunchGuide.Controllers
{
    public class StudentController : Controller
    {


        [HttpGet]
        public ActionResult Start()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Menu()
        {
            return View();
        }
    }
}
  

        