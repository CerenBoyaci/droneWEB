using Dapper;
using droneWEB.Core.Dto;
using droneWEB.Core.Dtos;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

namespace droneWEB.Data.Repository
{
    public class KullaniciRepository
    {
        private readonly string _connectionString;

        public KullaniciRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection");
        }

        public async Task<int> EkleAsync(Kullanici kullanici)
        {
            var sql = @"
                INSERT INTO dt_kullanici 
                (Ad, Soyad, Eposta, TcKimlikNo, Telefon, SifreHash, SifreSalt, KayitTarihi) 
                VALUES (@Ad, @Soyad, @Eposta, @TcKimlikNo, @Telefon, @SifreHash, @SifreSalt, @KayitTarihi); 
                SELECT CAST(SCOPE_IDENTITY() as int);";

            using var baglanti = new SqlConnection(_connectionString);
            return await baglanti.ExecuteScalarAsync<int>(sql, kullanici);
        }

        public async Task<Kullanici?> GetirEpostaIleAsync(string eposta)
        {
            var sql = "SELECT * FROM dt_kullanici WHERE Eposta = @Eposta";
            using var baglanti = new SqlConnection(_connectionString);
            return await baglanti.QueryFirstOrDefaultAsync<Kullanici>(sql, new { Eposta = eposta });
        }

        public async Task<Kullanici?> GetirTcIleAsync(string tcKimlikNo)
        {
            var sql = "SELECT * FROM dt_kullanici WHERE TcKimlikNo = @TcKimlikNo";
            using var baglanti = new SqlConnection(_connectionString);
            return await baglanti.QueryFirstOrDefaultAsync<Kullanici>(sql, new { TcKimlikNo = tcKimlikNo });
        }

        public async Task YetkiAtaAsync(int kullaniciId, int rolId)
        {
            var sql = "INSERT INTO dt_yetki (KullaniciId, RolId) VALUES (@KullaniciId, @RolId)";
            using var baglanti = new SqlConnection(_connectionString);
            await baglanti.ExecuteAsync(sql, new { KullaniciId = kullaniciId, RolId = rolId });
        }

        public async Task YetkiAtaAsync(int kullaniciId, List<int> rolIdListesi)
        {
            using var baglanti = new SqlConnection(_connectionString);
            foreach (var rolId in rolIdListesi)
            {
                var sql = "INSERT INTO dt_yetki (KullaniciId, RolId) VALUES (@KullaniciId, @RolId)";
                await baglanti.ExecuteAsync(sql, new { KullaniciId = kullaniciId, RolId = rolId });
            }
        }

    }
}
