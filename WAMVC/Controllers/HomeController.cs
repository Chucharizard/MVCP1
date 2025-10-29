using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WAMVC.Models;
using Microsoft.AspNetCore.Authorization;

namespace WAMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            HomeModel Mode = new HomeModel();
            Mode.Mensaje = "Pensamientos";
            Mode.Destinatario = "Panaderos";
            return View(Mode);
        }

        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [AllowAnonymous]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
