using System.Globalization;
using System.Text.Json;
using System.Text.Json.Nodes;
using Google_Translate_Clone.Models;
using Microsoft.AspNetCore.WebUtilities;

namespace Google_Translate_Clone.Client
{
    public class GoogleTranslateClient
    {
        private const string baseAddress = "https://translation.googleapis.com/language/translate/v2";
        private readonly HttpClient client;

        public GoogleTranslateClient(HttpClient client)
        {
            this.client = client;
        }

        public async Task<Dictionary<string,Language>> GetSupportedLanguages()
        {
            var queryParams = new Dictionary<string, string>()
            {
                ["key"] = "AIzaSyBeAFAjGGV9y48YVSCMvKWMFRVWoUlhOZo",
                ["target"] = "en"
            };

            var uri = QueryHelpers.AddQueryString($"{baseAddress}/languages", queryParams);

            var response = await client.GetAsync(uri);
            string content = await response.Content.ReadAsStringAsync();

            var data = JsonObject.Parse(content);
            var languagesNode = data["data"]["languages"];
            var languages = JsonSerializer.Deserialize<List<Language>>(languagesNode)
                                .ToDictionary(x => x.language);
            return languages;
        }

        public async Task<Language> DetectLanguage(string input)
        {
            var queryParams = new Dictionary<string, string>()
            {
                ["key"] = "AIzaSyBeAFAjGGV9y48YVSCMvKWMFRVWoUlhOZo",
                ["q"] = input
            };

            var uri = QueryHelpers.AddQueryString($"{baseAddress}/detect", queryParams);

            var response = await client.PostAsync(uri, null);
            var content = await response.Content.ReadAsStringAsync();

            JsonNode data = JsonNode.Parse(content);
            JsonNode languageNode = data["data"]["detections"][0][0];
            Language language = JsonSerializer.Deserialize<Language>(languageNode);

            return language;
        }

        public async Task<TranslatedText> Translate(string input, string source, string target)
        {
            var queryParams = new Dictionary<string, string>()
            {
                ["key"] = "AIzaSyBeAFAjGGV9y48YVSCMvKWMFRVWoUlhOZo",
                ["q"] = input,
                ["target"] = target,
                ["source"] = source
            };

            var uri = QueryHelpers.AddQueryString(baseAddress, queryParams);

            var response = await client.PostAsync(uri, null);
            var content = await response.Content.ReadAsStringAsync();

            JsonNode data = JsonNode.Parse(content);
            JsonNode translateNode = data["data"]["translations"][0];
            TranslatedText translation = JsonSerializer.Deserialize<TranslatedText>(translateNode);

            return translation;
        }
    }
}