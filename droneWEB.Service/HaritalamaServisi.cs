using droneWEB.Core.Dtos.Request;
using droneWEB.Core.Dtos.Response;
using droneWEB.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace droneWEB.Service
{
    public class HaritalamaServisi
    {
        private readonly HaritalamaRepository _repository;
        public HaritalamaServisi(HaritalamaRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<IsaretciResponse>> TumunuGetirAsync()
        {
            return await _repository.TumunuGetirAsync();
        }

        public async Task<IsaretciResponse> OlusturAsync(IsaretciOlusturRequest request)
        {
            return await _repository.OlusturAsync(request);
        }

        public async Task<IsaretciResponse> GuncelleAsync(IsaretciGuncelleRequest request)
        {
            return await _repository.GuncelleAsync(request);
        }
    }
}
