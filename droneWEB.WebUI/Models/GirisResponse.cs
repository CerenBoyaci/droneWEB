public class GirisResponse
{
    public int Id { get; set; }
    public string Token { get; set; } = null!;
    public string AdSoyad { get; set; } = null!;
    public string Eposta { get; set; } = null!;
    public DateTime TokenSonGecerlilik { get; set; }
}

