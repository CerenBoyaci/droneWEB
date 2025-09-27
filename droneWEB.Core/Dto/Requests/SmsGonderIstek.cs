using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace droneWEB.Core.Dto.Requests
{
    public class SmsGonderIstek
    {
        public string To { get; set; } = null!;
        public string Mesaj { get; set; } = null!;
    }

}
