using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using QRCodeAPI.Models;
using QRCodeAPI.Client.Types;

namespace QRCodeAPI.Client
{
    public class GoQrClient : IQrClient
    {
        private const string TAG = "[GoQrClient]";
        private const string API_URL = "http://api.qrserver.com/v1";
        
        private readonly HttpClient _client;
        private readonly ILogger<GoQrClient> _logger;

        public GoQrClient(HttpClient client, ILogger<GoQrClient> logger)
        {
            _client = client;
            _logger = logger;
        }

        public async Task<QrResponseModel> PostQrAsync(QrFileModel file)
        {
            _logger.LogTrace($"{TAG} PostQrAsync called with file: {file.Name}");
            var byteContent = new ByteArrayContent(file.Content);
            var content = new MultipartFormDataContent {{byteContent, "file", file.Name}};
            _logger.LogTrace($"{TAG} Calling api at address: '{API_URL}/read-qr-code/'");
            var response = await _client.PostAsync($"{API_URL}/read-qr-code/", content);
            _logger.LogDebug($"{TAG} Received response status code: {response.StatusCode}, reason phrase: {response.ReasonPhrase}");
            response.EnsureSuccessStatusCode();            
            
            var json = await response.Content.ReadAsStringAsync();
            var goQrResponse = JsonSerializer.Deserialize<GoQrResponse[]>(json);
            return ParseGoQrResponse(goQrResponse);
        }

        private QrResponseModel ParseGoQrResponse(GoQrResponse[] response)
        {
            _logger.LogTrace($"{TAG} Parsing response from api.");
            var model = response.FirstOrDefault();
            var symbol = model?.Symbol.FirstOrDefault();
            
            return new QrResponseModel
            {
                Data = symbol?.Data,
                Error = symbol?.Error
            };
        }
    }
}