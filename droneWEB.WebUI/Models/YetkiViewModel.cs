namespace droneWEB.WebUI.Models
{
    public class YetkiViewModel
    {
        public int KullaniciId { get; set; }
        public List<int> RolIdListesi { get; set; } = new List<int>();
        public List<RolItem> RolSecenekleri { get; set; } = new List<RolItem>();
    }
}
