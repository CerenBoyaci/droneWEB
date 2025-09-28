using droneWEB.Core.Dtos.Request;
using droneWEB.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace droneWEB.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HaritalamaController : ControllerBase
    {
        private readonly HaritalamaServisi _servis;
        public HaritalamaController(HaritalamaServisi servis)
        {
            _servis = servis;
        }

        [HttpGet("tum-isaretciler")]
        public async Task<IActionResult> TumIsaretciler()
        {
            var isaretciler = await _servis.TumunuGetirAsync();
            return Ok(isaretciler);
        }

        [HttpPost("olustur")]
        public async Task<IActionResult> Olustur([FromBody] IsaretciOlusturRequest request)
        {
            var isaretci = await _servis.OlusturAsync(request);
            return Ok(isaretci);
        }

        [HttpPut("guncelle")]
        public async Task<IActionResult> Guncelle([FromBody] IsaretciGuncelleRequest request)
        {
            var isaretci = await _servis.GuncelleAsync(request);
            return Ok(isaretci);
        }
    }
}
