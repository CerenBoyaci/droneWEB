using Microsoft.AspNetCore.Mvc;

namespace droneWEB.WebUI.Controllers
{
    public class HaritaController : Controller
    {
        public IActionResult Harita()
        {
            return View();
        }
    }
}
