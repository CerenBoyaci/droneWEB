using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace droneWEB.Core.Dto.Requests
{
    public class KayitIstek
    {
        public string Ad { get; set; } = null!;
        public string Soyad { get; set; } = null!;
        public string Eposta { get; set; } = null!;
        public string TcKimlikNo { get; set; } = null!;
        public string Telefon { get; set; } = null!;
        public string Sifre { get; set; } = null!;
       
    }

}
