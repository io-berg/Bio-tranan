namespace WebApp.Models
{
    public class MovieViewModel
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Genre { get; set; }
        public int Runtime { get; set; }
        public int ReleaseYear { get; set; }
        public int AgeRating { get; set; }
        public string SpokenLanguage { get; set; }
        public string TextLanguage { get; set; }
        public List<String> Directors { get; set; }
        public List<String> Actors { get; set; }
    }
}