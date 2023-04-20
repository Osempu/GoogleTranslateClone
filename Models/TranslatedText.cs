using System.Text.Json.Serialization;

namespace Google_Translate_Clone.Models
{
    public class TranslatedText
    {
        [JsonPropertyName("translatedText")]
        public string translatedText { get; set; }
    }
}