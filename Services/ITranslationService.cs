using System.Threading.Tasks;

namespace MyProject.Services
{
    public interface ITranslationService
    {
        Task<string?> DetectLanguageAsync(string text);
        Task<string?> GetLanguagesAsync();
        Task<string?> TranslateAsync(string text, string sourceLanguage, string targetLanguage);
    }
}
