using FİLESAPI.UI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FİLESAPI.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;
        public HomeController(ILogger<HomeController> logger , IConfiguration configuration)
        {   
            _logger = logger;
            _configuration = configuration;
        }


        public IActionResult Index()
        {
            return View();
        }


        [Route("Folders")]
        public IActionResult Folders()
        {
            string ApiBaseUrl = _configuration["ApiBaseUrl"]!;
            ViewBag.ApiBaseUrl = ApiBaseUrl;
            return View();
        }

        [Route("Files/{id}")]
        public IActionResult Files( int id)
        {
            string ApiBaseUrl = _configuration["ApiBaseUrl"]!;
            ViewBag.ApiBaseUrl = ApiBaseUrl;

            return View();
        }

        [Route("Login")]
        public IActionResult Login()
        {
            string ApiBaseUrl = _configuration["ApiBaseUrl"];
            ViewBag.ApiBaseUrl = ApiBaseUrl;
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
