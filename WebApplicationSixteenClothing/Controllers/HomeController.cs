using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;


namespace WebApplicationSixteenClothing.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
