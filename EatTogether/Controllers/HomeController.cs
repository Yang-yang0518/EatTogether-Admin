using System.Diagnostics;
using EatTogether.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EatTogether.Controllers
{
    //[Authorize]
    public class HomeController : Controller
    {
        //private readonly ILogger<HomeController> _logger;

        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}

        // GET /Home/Index
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

    }
}
