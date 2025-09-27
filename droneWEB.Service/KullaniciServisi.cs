using droneWEB.Core.Dto.Requests;
using droneWEB.Core.Dto.Responses;
using droneWEB.Core.Dtos;
using droneWEB.Data.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace droneWEB.Service
{
    public class KullaniciServisi
    {
        private readonly KullaniciRepository _repo;
        private readonly IConfiguration _config;

        public KullaniciServisi(KullaniciRepository repo, IConfiguration config)
        {
            _repo = repo;
            _config = config;
        }

        public async Task<KayitYanit> KayitOl(KayitIstek istek)
        {
            if (istek == null) throw new Exception("İstek boş olamaz.");

            if (await _repo.GetirEpostaIleAsync(istek.Eposta) != null)
                throw new Exception("Bu eposta zaten kayıtlı.");

            if (await _repo.GetirTcIleAsync(istek.TcKimlikNo) != null)
                throw new Exception("Bu TC kimlik numarası zaten kayıtlı.");

            using var hmac = new HMACSHA512();
            var kullanici = new Kullanici
            {
                Ad = istek.Ad,
                Soyad = istek.Soyad,
                Eposta = istek.Eposta,
                TcKimlikNo = istek.TcKimlikNo,
                Telefon = istek.Telefon,
                SifreHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(istek.Sifre)),
                SifreSalt = hmac.Key,
                KayitTarihi = DateTime.UtcNow
            };

            kullanici.Id = await _repo.EkleAsync(kullanici);

            return new KayitYanit
            {
                Id = kullanici.Id,
                Ad = kullanici.Ad,
                Soyad = kullanici.Soyad,
                Eposta = kullanici.Eposta,
                Mesaj = "Kayıt başarılı."
            };
        }

        public async Task<GirisYanıt?> GirisYap(GirisIstek istek)
        {
            var kullanici = await _repo.GetirEpostaIleAsync(istek.Eposta);
            if (kullanici == null) return null;

            using var hmac = new HMACSHA512(kullanici.SifreSalt);
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(istek.Sifre));

            if (!hash.SequenceEqual(kullanici.SifreHash))
                return null;

            var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"] ?? throw new Exception("JWT key bulunamadı"));
            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, kullanici.Id.ToString()),
                    new Claim(ClaimTypes.Email, kullanici.Eposta),
                    new Claim(ClaimTypes.Name, kullanici.Ad + " " + kullanici.Soyad)
                }),
                Expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_config["Jwt:ExpireMinutes"] ?? "60")),
                Issuer = _config["Jwt:Issuer"],
                Audience = _config["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new GirisYanıt
            {
                Id = kullanici.Id,
                Token = tokenHandler.WriteToken(token),
                AdSoyad = kullanici.Ad + " " + kullanici.Soyad,
                Eposta = kullanici.Eposta,
                TokenSonGecerlilik = tokenDescriptor.Expires ?? DateTime.UtcNow
            };
        }

        public async Task<Kullanici?> GetirEpostaIleAsync(string eposta)
        {
            return await _repo.GetirEpostaIleAsync(eposta);
        }

        public async Task<Kullanici?> GetirTcIleAsync(string tcKimlikNo)
        {
            return await _repo.GetirTcIleAsync(tcKimlikNo);
        }

        public async Task YetkiAta(YetkiIstek istek)
        {
            await _repo.YetkiAtaAsync(istek.KullaniciId, istek.RolId);
        }

        public async Task YetkiAtaCoklu(int kullaniciId, List<int> rolIdListesi)
        {
            await _repo.YetkiAtaAsync(kullaniciId, rolIdListesi);
        }

        public async Task<List<RolItem>> TumRolleriGetir()
        {
            return await _repo.GetAllAsync();
        }



    }
}
