using System.Text.Json.Serialization;

namespace QRCodeAPI.Client.Types
{
    public class Symbol
    {
        [JsonPropertyName("seq")]
        public int Seq { get; set; }
        [JsonPropertyName("data")]
        public string Data { get; set; }
        [JsonPropertyName("error")]
        public string Error { get; set; }
    }
}