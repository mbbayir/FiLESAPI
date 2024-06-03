using Microsoft.AspNetCore.Mvc;

namespace FİLESAPI.UI.Controllers
{
    public class FilesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
