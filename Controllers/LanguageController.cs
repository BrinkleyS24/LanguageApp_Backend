using Microsoft.AspNetCore.Mvc;
using MyProject.Models;
using MyProject.Services;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;


namespace MyProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LanguageConversionController : ControllerBase
    {
        private readonly ITranslationService _translationService;

        public LanguageConversionController(ITranslationService translationService)
        {
            _translationService = translationService;
        }

        [HttpPost("detect")]
        public async Task<IActionResult> DetectLanguage([FromBody] DetectLanguageRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.InputText))
            {
                return BadRequest("InputText cannot be null or empty.");
            }

            var result = await _translationService.DetectLanguageAsync(request.InputText);
            if (result == null)
            {
                return StatusCode(500, "Error detecting language.");
            }

            try
            {

                // Parse the JSON response
                var parsedResult = JObject.Parse(result);
                
                var language = parsedResult["Language"]?.ToString();
                var confidence = parsedResult["Confidence"]?.ToObject<double>();

                if (!string.IsNullOrEmpty(language) && confidence.HasValue)
                {
                    var detectedLanguage = new { Language = language, Confidence = confidence };
                    return Ok(detectedLanguage);
                }
                else
                {
                    System.Console.WriteLine("[ERROR] Language or confidence not found in the detection.");
                    return StatusCode(500, "Error parsing detection response.");
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"[ERROR] Error parsing detection response: {ex.Message}");
                return StatusCode(500, "Error parsing detection response.");
            }
        }





        [HttpGet("languages")]
        public async Task<IActionResult> GetLanguages()
        {
            var result = await _translationService.GetLanguagesAsync();
            if (result == null)
            {
                return StatusCode(500, "Error retrieving languages.");
            }

            try
            {
                // Parse the JSON response
                var parsedResult = JObject.Parse(result);

                // Check if the "languages" property exists directly in the response
                if (parsedResult["languages"] != null)
                {
                    // Retrieve the languages array
                    var languagesArray = parsedResult["languages"];
                    if (languagesArray is JArray array)
                    {
                        List<object> languagesList = new List<object>();

                        foreach (var language in array)
                        {
                            var languageCode = language["language"]?.ToString();
                            var languageName = language["name"]?.ToString();
                            if (!string.IsNullOrEmpty(languageCode) && !string.IsNullOrEmpty(languageName))
                            {
                                languagesList.Add(new { Language = languageCode, Name = languageName });
                            }
                        }

                        return Ok(languagesList);
                    }
                    else
                    {
                        System.Console.WriteLine("[ERROR] 'languages' property is not an array.");
                        return StatusCode(500, "Error parsing languages response.");
                    }
                }
                else
                {
                    System.Console.WriteLine("[ERROR] 'languages' property not found in the response.");
                    return StatusCode(500, "Error parsing languages response.");
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"[ERROR] Error parsing languages response: {ex.Message}");
                return StatusCode(500, "Error parsing languages response.");
            }
        }



        [HttpPost("translate")]
        public async Task<IActionResult> TranslateText([FromBody] LanguageConversionRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.InputText) ||
                string.IsNullOrWhiteSpace(request.SourceLanguage) ||
                string.IsNullOrWhiteSpace(request.TargetLanguage))
            {
                return BadRequest("InputText, SourceLanguage, and TargetLanguage cannot be null or empty.");
            }

            var result = await _translationService.TranslateAsync(request.InputText, request.SourceLanguage, request.TargetLanguage);
            if (result == null)
            {
                return StatusCode(500, "Error translating text.");
            }

            return Ok(new { TranslatedText = result });
        }
    }
}
