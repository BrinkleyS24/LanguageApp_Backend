namespace MyProject.Models
{
    public class Languages
    {
        public string? Language { get; set; }
        public string? Name { get; set; }
    }

    public class GetLanguagesResponse
    {
        public List<Languages>? Languages { get; set; }
    }



}
