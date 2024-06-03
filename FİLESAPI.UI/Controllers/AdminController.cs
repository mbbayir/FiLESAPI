using Microsoft.AspNetCore.Mvc;

namespace FİLESAPI.UI.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
