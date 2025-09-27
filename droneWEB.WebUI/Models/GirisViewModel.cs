using System.ComponentModel.DataAnnotations;

namespace droneWEB.WebUI.Models
{
    public class GirisViewModel
    {
        [Required, EmailAddress]
        public string Eposta { get; set; } = null!;

        [Required, DataType(DataType.Password)]
        public string Sifre { get; set; } = null!;
    }
}
