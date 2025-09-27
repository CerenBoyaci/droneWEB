using droneWEB.WebUI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;

namespace droneWEB.WebUI.Controllers
{
    public class KullaniciController : Controller
    {
        private readonly HttpClient _httpClient;

        public KullaniciController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7130/api/"); // API base URL
        }

        [HttpGet]
        public IActionResult Kayit()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Kayit(KayitViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            string telefon = $"+90{model.Telefon.Replace(" ", "")}";

            var response = await _httpClient.PostAsJsonAsync("Kullanici/kayit-ol", model);

            if (response.IsSuccessStatusCode)
            {
                ViewBag.Mesaj = "Kayıt başarılı!";
                return View();
            }

            var hata = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError("", hata);
            return View(model);
        }
        [HttpGet]
        public IActionResult Giris()
        {
            return View();
        }

        /*  [HttpPost]
          public async Task<IActionResult> Giris(GirisViewModel model)
          {
              if (!ModelState.IsValid)
                  return View(model);



              var response = await _httpClient.PostAsJsonAsync("Kullanici/giris-yap", model);

              if (response.IsSuccessStatusCode)
              {
                  var tokenResponse = await response.Content.ReadFromJsonAsync<GirisResponse>();
                  if (tokenResponse != null)
                  {
                      HttpContext.Session.SetString("Token", tokenResponse.Token);
                      return RedirectToAction("Index", "Home");
                  }
              }

              var hata = await response.Content.ReadAsStringAsync();
              ModelState.AddModelError("", hata);
              return View(model);
          }*/

        [HttpPost]
        public async Task<IActionResult> Giris(GirisViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var response = await _httpClient.PostAsJsonAsync("Kullanici/giris-yap", model);

            if (response.IsSuccessStatusCode)
            {
                var tokenResponse = await response.Content.ReadFromJsonAsync<GirisResponse>();
                if (tokenResponse != null)
                {
                   
                    HttpContext.Session.SetString("KullaniciId", tokenResponse.Id.ToString()); // artık Id mevcut
                    HttpContext.Session.SetString("AdSoyad", tokenResponse.AdSoyad);
                    HttpContext.Session.SetString("Token", tokenResponse.Token);

                    return RedirectToAction("Index", "Home");
                }
            }

            var hata = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError("", hata);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> YetkiAta(int kullaniciId)
        {
            var model = new YetkiViewModel { KullaniciId = kullaniciId };

            // Roller API'den çekiliyor
            var rollerResponse = await _httpClient.GetFromJsonAsync<List<RolItem>>("api/rol/liste");
            if (rollerResponse != null)
                model.RolSecenekleri = rollerResponse;

            return View(model);
        }

        // Form submit
        [HttpPost]
        public async Task<IActionResult> YetkiAta(YetkiViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var response = await _httpClient.PostAsJsonAsync(
                $"api/kullanici/yetki-ata-toplu?kullaniciId={model.KullaniciId}",
                model.RolIdListesi
            );

            if (response.IsSuccessStatusCode)
            {
                TempData["Basarili"] = "Yetkiler başarıyla atandı.";
                return RedirectToAction("Index"); // veya kullanıcı listesi
            }

            TempData["Hata"] = "Yetki atama başarısız oldu.";
            return View(model);
        }



    }
}
