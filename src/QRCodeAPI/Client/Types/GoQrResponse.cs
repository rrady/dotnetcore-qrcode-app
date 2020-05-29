using System.Text.Json.Serialization;

namespace QRCodeAPI.Client.Types
{
    public class GoQrResponse
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }
        [JsonPropertyName("symbol")]
        public Symbol[] Symbol { get; set; }
    }
}