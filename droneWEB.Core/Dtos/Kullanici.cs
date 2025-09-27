using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace droneWEB.Core.Dtos
{
    public class Kullanici
    {
        public int Id { get; set; }
        public string Ad { get; set; } = null!;
        public string Soyad { get; set; } = null!;
        public string Eposta { get; set; } = null!;
        public string TcKimlikNo { get; set; } = null!;
        public string Telefon { get; set; } = null!;
        public byte[] SifreHash { get; set; } = null!;
        public byte[] SifreSalt { get; set; } = null!;
        public DateTime KayitTarihi { get; set; }
    }
}

