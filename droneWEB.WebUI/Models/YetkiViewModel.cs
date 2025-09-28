using Microsoft.AspNetCore.Mvc.Rendering;

namespace droneWEB.WebUI.Models
{
    public class YetkiViewModel
    {
        public int KullaniciId { get; set; }  // Dropdown’dan seçilen kullanıcı
        public List<int> RolIdListesi { get; set; } = new List<int>();  // Çoklu rol seçimi
        public List<SelectListItem> KullaniciSecenekleri { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> RolSecenekleri { get; set; } = new List<SelectListItem>();
    }

}
