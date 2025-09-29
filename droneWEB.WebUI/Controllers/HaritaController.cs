using Microsoft.AspNetCore.Mvc;

namespace droneWEB.WebUI.Controllers
{
    public class HaritaController : BaseController
    {
        [HttpGet]
        public IActionResult Harita()
        {
            return View();
        }

        [HttpGet]
        public IActionResult RotaHesaplama()
        {
            return View();
        }
    }
}
