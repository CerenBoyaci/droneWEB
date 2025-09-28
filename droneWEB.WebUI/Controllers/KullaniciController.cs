using droneWEB.WebUI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        public async Task<IActionResult> YetkiAta()
        {
            var model = new YetkiViewModel();

            // Roller
            var roller = await _httpClient.GetFromJsonAsync<List<RolItem>>("https://localhost:7130/api/Kullanici/liste");
            if (roller != null)
                model.RolSecenekleri = roller
                    .Select(r => new SelectListItem { Value = r.Id.ToString(), Text = r.Ad })
                    .ToList();

            // Kullanıcılar
            var kullanicilar = await _httpClient.GetFromJsonAsync<List<KullaniciItem>>("https://localhost:7130/api/Kullanici/kullanicilar");
            if (kullanicilar != null)
                model.KullaniciSecenekleri = kullanicilar
                    .Select(k => new SelectListItem { Value = k.Id.ToString(), Text = k.Ad + " " + k.Soyad })
                    .ToList();

            return View(model);
        }





        // Form submit
        [HttpPost]
        public async Task<IActionResult> YetkiAta(YetkiViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var response = await _httpClient.PostAsJsonAsync(
                $"https://localhost:7130/api/Kullanici/yetki-ata-toplu?kullaniciId={model.KullaniciId}",
                model.RolIdListesi
            );

            if (response.IsSuccessStatusCode)
            {
                TempData["Basarili"] = "Yetkiler başarıyla atandı.";

                // API'den tekrar seçenekleri çekelim ki sayfa yenilendiğinde dropdown dolu olsun
                var roller = await _httpClient.GetFromJsonAsync<List<RolItem>>("https://localhost:7130/api/Kullanici/liste");
                if (roller != null)
                    model.RolSecenekleri = roller
                        .Select(r => new SelectListItem { Value = r.Id.ToString(), Text = r.Ad })
                        .ToList();

                var kullanicilar = await _httpClient.GetFromJsonAsync<List<KullaniciItem>>("https://localhost:7130/api/Kullanici/kullanicilar");
                if (kullanicilar != null)
                    model.KullaniciSecenekleri = kullanicilar
                        .Select(k => new SelectListItem { Value = k.Id.ToString(), Text = k.Ad + " " + k.Soyad })
                        .ToList();

                return View(model);
            }

            TempData["Hata"] = "Yetki atama başarısız oldu.";
            return View(model);
        }





    }
}
