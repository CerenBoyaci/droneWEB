using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace droneWEB.Core.Dto.Responses
{
    public class GirisYanıt
    {
        public int Id { get; set; }
        public string Token { get; set; } = null!;
        public string AdSoyad { get; set; } = null!;
        public string Eposta { get; set; } = null!;
        public DateTime TokenSonGecerlilik { get; set; }
        //public List<string> Roller { get; set; } = new();
    }

}
