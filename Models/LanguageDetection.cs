namespace MyProject.Models
{
    public class DetectLanguageRequest
    {
        public string? InputText { get; set; }
    }

    public class DetectLanguageResponse
{
    public string? Language { get; set; }
}

}
