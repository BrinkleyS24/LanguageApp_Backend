namespace MyProject.Models
{
    public class LanguageConversionRequest
    {
        public string? InputText { get; set; }
        public string? SourceLanguage { get; set; }
        public string? TargetLanguage { get; set; }
    }

    public class LanguageConversionResponse
    {
        public string? TranslatedText { get; set; }
    }

}
