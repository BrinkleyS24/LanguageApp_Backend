using RestSharp;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;


namespace MyProject.Services
{
    public class TranslationService : ITranslationService
    {
        private readonly RestClient _client;

        public TranslationService()
        {
            _client = new RestClient("https://deep-translate1.p.rapidapi.com");
            _client.AddDefaultHeader("x-rapidapi-key", "0be3b540a1msh34ab91e9a1fed08p179c4bjsn32fec098fe80");
            _client.AddDefaultHeader("x-rapidapi-host", "deep-translate1.p.rapidapi.com");
        }

        public async Task<string?> DetectLanguageAsync(string text)
        {
            try
            {
                var request = new RestRequest("/language/translate/v2/detect", Method.Post);
                request.AddHeader("Content-Type", "application/json");
                request.AddParameter("application/json", JsonConvert.SerializeObject(new { q = text }), ParameterType.RequestBody);

                var response = await _client.ExecuteAsync(request);

                Console.WriteLine($"Response Content: {response.Content}");

                if (response.IsSuccessful && !string.IsNullOrEmpty(response.Content))
                {
                    var content = JObject.Parse(response.Content);
                    var data = content["data"];

                    if (data != null && data.HasValues)
                    {
                        var detections = data["detections"];

                        if (detections != null && detections.HasValues)
                        {
                            // Assuming we only have one detection
                            var detection = detections[0];
                            var language = detection?["language"]?.ToString() ?? "";
                            var confidence = detection?["confidence"]?.ToObject<double>();

                            if (!string.IsNullOrEmpty(language) && confidence.HasValue)
                            {
                                var detectedLanguage = new { Language = language, Confidence = confidence };
                                return JsonConvert.SerializeObject(detectedLanguage);
                            }
                            else
                            {
                                throw new Exception("Language detection not available.");
                            }
                        }
                        else
                        {
                            throw new Exception("Detections array is empty or not found.");
                        }
                    }
                    else
                    {
                        throw new Exception("Data not found in the response.");
                    }
                }
                else
                {
                    throw new Exception("Error detecting language.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error detecting language: {ex.Message}");
                return null;
            }
        }



        public async Task<string?> GetLanguagesAsync()
        {
            var request = new RestRequest("/language/translate/v2/languages", Method.Get);
            var response = await _client.ExecuteAsync(request);
            if (response.IsSuccessful && !string.IsNullOrEmpty(response.Content))
            {
                try
                {
                    var content = JObject.Parse(response.Content);
                    if (content["languages"] != null)
                    {
                        var languagesArray = content["languages"];
                        if (languagesArray is JArray array)
                        {
                            List<string> languageNames = new List<string>();

                            foreach (var language in array)
                            {
                                var languageName = language["name"]?.ToString();
                                if (!string.IsNullOrEmpty(languageName))
                                {
                                    languageNames.Add(languageName);
                                }
                            }

                            return response.Content;
                        }
                        else
                        {
                            System.Console.WriteLine("[ERROR] 'languages' property is not an array.");
                        }
                    }
                    else
                    {
                        System.Console.WriteLine("[ERROR] 'languages' property not found in the response.");
                    }
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine($"[ERROR] Error parsing languages response: {ex.Message}");
                }
            }
            return null;
        }



        public async Task<string?> TranslateAsync(string text, string sourceLanguage, string targetLanguage)
        {
            var request = new RestRequest("/language/translate/v2", Method.Post);
            request.AddHeader("Content-Type", "application/json");
            request.AddJsonBody(new { q = text, source = sourceLanguage, target = targetLanguage });

            var response = await _client.ExecuteAsync(request);
            if (response.IsSuccessful && !string.IsNullOrEmpty(response.Content))
            {
                try
                {
                    var content = JObject.Parse(response.Content);

                    if (content["data"] != null)
                    {
                        var translations = content["data"]?["translations"];
                        if (translations != null && translations.Type == JTokenType.Object)
                        {
                            var translatedText = translations["translatedText"];
                            if (translatedText != null)
                            {
                                return translatedText.ToString();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Log or handle the exception appropriately
                    System.Console.WriteLine($"[ERROR] Error parsing JSON response: {ex.Message}");
                }
            }
            return null;
        }
    }
}
