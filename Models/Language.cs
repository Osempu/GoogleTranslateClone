using System.Text.Json.Serialization;

namespace Google_Translate_Clone.Models
{
    public class Language
    {
        [JsonPropertyName("language")]
        public string language { get; set; }

        [JsonPropertyName("name")]
        public string name { get; set; }
    }
}