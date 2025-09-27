using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using droneWEB.Core.Dto.Requests;

namespace droneWEB.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SmsController : ControllerBase
    {
        private readonly SmsServisi _smsServisi;

        public SmsController(SmsServisi smsServisi)
        {
            _smsServisi = smsServisi;
        }

        [HttpPost("gonder")]
        public async Task<IActionResult> Gonder([FromBody] SmsGonderIstek istek)
        {
            if (string.IsNullOrEmpty(istek.To) || string.IsNullOrEmpty(istek.Mesaj))
                return BadRequest("Numara ve mesaj zorunludur.");

            await _smsServisi.SendSmsAsync(istek.To, istek.Mesaj);

            return Ok(new { message = "SMS gönderildi" });
        }
    }
}


