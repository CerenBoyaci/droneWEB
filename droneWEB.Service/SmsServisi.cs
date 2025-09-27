using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

public class SmsServisi
{
    private readonly HttpClient _http;
    private readonly string _baseUrl;
    private readonly string _apiKey;
    private readonly string _from;

    public SmsServisi(HttpClient http, IConfiguration config)
    {
        _http = http;
        _baseUrl = config["Infobip:BaseUrl"];
        _apiKey = config["Infobip:ApiKey"];
        _from = config["Infobip:From"] ?? "TESTSMS";
    }

    public async Task SendSmsAsync(string to, string message)
    {
        var payload = new
        {
            messages = new[]
            {
                new {
                    from = _from,
                    destinations = new [] { new { to = to } },
                    content = new { text = message }
                }
            }
        };

        var json = JsonSerializer.Serialize(payload);
        var request = new HttpRequestMessage(HttpMethod.Post, $"{_baseUrl}/sms/2/text/advanced");
        request.Content = new StringContent(json, Encoding.UTF8, "application/json");
        request.Headers.Authorization = new AuthenticationHeaderValue("App", _apiKey);

        var resp = await _http.SendAsync(request);
        resp.EnsureSuccessStatusCode();
        var respBody = await resp.Content.ReadAsStringAsync();
        Console.WriteLine("Infobip HTTP SMS yanıt: " + respBody);
    }
}
