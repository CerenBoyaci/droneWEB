using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace droneWEB.Core.Dto.Responses
{
    public class KayitYanit
    {
        public int Id { get; set; }
        public string Ad { get; set; } = null!;
        public string Soyad { get; set; } = null!;
        public string Eposta { get; set; } = null!;
        public string Mesaj { get; set; } = "Kayıt başarılı";
    }

}
