using droneWEB.Core.Dtos.Request;
using droneWEB.Core.Dtos.Response;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;


namespace droneWEB.Data.Repository
{
    public class HaritalamaRepository
    {
        private readonly string _connectionString;

        public HaritalamaRepository(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection");
        }

        public async Task<List<IsaretciResponse>> TumunuGetirAsync()
        {
            var sql = "SELECT Id, Baslik, Enlem, Boylam FROM dt_haritalama";
            using var baglanti = new SqlConnection(_connectionString);
            var result = await baglanti.QueryAsync<IsaretciResponse>(sql);
            return result.ToList();
        }

        public async Task<IsaretciResponse> OlusturAsync(IsaretciOlusturRequest request)
        {
            var sql = @"INSERT INTO dt_haritalama (Baslik, Enlem, Boylam)
                        VALUES (@Baslik, @Enlem, @Boylam);
                        SELECT CAST(SCOPE_IDENTITY() as int);";

            using var baglanti = new SqlConnection(_connectionString);
            var id = await baglanti.ExecuteScalarAsync<int>(sql, request);

            return new IsaretciResponse
            {
                Id = id,
                Baslik = request.Baslik,
                Enlem = request.Enlem,
                Boylam = request.Boylam
            };
        }

        public async Task<IsaretciResponse> GuncelleAsync(IsaretciGuncelleRequest request)
        {
            var sql = @"UPDATE dt_haritalama
                        SET Baslik = @Baslik, Enlem = @Enlem, Boylam = @Boylam
                        WHERE Id = @Id";

            using var baglanti = new SqlConnection(_connectionString);
            await baglanti.ExecuteAsync(sql, request);

            return new IsaretciResponse
            {
                Id = request.Id,
                Baslik = request.Baslik,
                Enlem = request.Enlem,
                Boylam = request.Boylam
            };
        }
    }
}
