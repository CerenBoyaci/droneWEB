using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace droneWEB.Core.Dto.Requests
{
    public class GirisIstek
    {
        public string Eposta { get; set; } = null!;
        public string Sifre { get; set; } = null!;
    }

}
