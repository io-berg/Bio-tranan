namespace WebApp.Models.ApiModels
{
    public class ApiRoot
    {
        public string searchType { get; set; }
        public string expression { get; set; }
        public List<Result> Results { get; set; }
        public string errorMessage { get; set; }
    }
}