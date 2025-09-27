using droneWEB.Core.Dto.Requests;
using droneWEB.Core.Dto.Responses;
using droneWEB.Core.Dtos;
using droneWEB.Service;
using Microsoft.AspNetCore.Mvc;

namespace droneWEB.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KullaniciController : ControllerBase
    {
        private readonly KullaniciServisi _servis;

        public KullaniciController(KullaniciServisi servis)
        {
            _servis = servis;
        }

        [HttpPost("kayit-ol")]
        public async Task<IActionResult> KayitOl([FromBody] KayitIstek istek)
        {
            try
            {
                var sonuc = await _servis.KayitOl(istek);
                return Ok(sonuc);
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { Mesaj = ex.Message });
            }
        }

        [HttpPost("giris-yap")]
        public async Task<IActionResult> GirisYap([FromBody] GirisIstek istek)
        {
            var token = await _servis.GirisYap(istek);
            if (token == null)
                return Unauthorized(new { Mesaj = "Eposta veya şifre hatalı" });

            return Ok(token);
        }

        [HttpGet("ara")]
        public async Task<IActionResult> KullaniciAra([FromQuery] string? eposta, [FromQuery] string? tcKimlikNo)
        {
            Kullanici? kullanici = null;

            if (!string.IsNullOrEmpty(eposta))
                kullanici = await _servis.GetirEpostaIleAsync(eposta);
            else if (!string.IsNullOrEmpty(tcKimlikNo))
                kullanici = await _servis.GetirTcIleAsync(tcKimlikNo);
            else
                return BadRequest(new { Mesaj = "Eposta veya TC Kimlik No belirtilmeli" });

            if (kullanici == null)
                return NotFound(new { Mesaj = "Kullanıcı bulunamadı" });

            return Ok(new
            {
                kullanici.Id,
                kullanici.Ad,
                kullanici.Soyad,
                kullanici.Eposta,
                kullanici.TcKimlikNo,
                kullanici.Telefon,
                kullanici.KayitTarihi
            });
        }

        [HttpPost("yetki-ata")]
        public async Task<IActionResult> YetkiAta([FromBody] YetkiIstek istek)
        {
            try
            {
                await _servis.YetkiAta(istek);
                return Ok(new { Mesaj = "Yetki atandı" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Mesaj = ex.Message });
            }
        }

        [HttpPost("yetki-ata-toplu")]
        public async Task<IActionResult> YetkiAtaToplu([FromQuery] int kullaniciId, [FromBody] List<int> rolIdListesi)
        {
            try
            {
                await _servis.YetkiAtaCoklu(kullaniciId, rolIdListesi);
                return Ok(new { Mesaj = "Yetkiler atandı" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Mesaj = ex.Message });
            }
        }
    }
}
