using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace droneWEB.Core.Dtos.Response
{
    public class IsaretciResponse
    {
        public int Id { get; set; }
        public string Baslik { get; set; } = null!;
        public double Enlem { get; set; }
        public double Boylam { get; set; }
    }
}
