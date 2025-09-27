using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace droneWEB.Core.Dto.Requests
{
    public class RolAtaIstek
    {
        public int KullaniciId { get; set; }
        public List<int> RolIds { get; set; } = new();
    }
}
