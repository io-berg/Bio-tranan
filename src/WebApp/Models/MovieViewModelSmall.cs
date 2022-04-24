namespace WebApp.Models
{
    public class MovieViewModelSmall
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public int Runtime { get; set; }
        public int ReleaseYear { get; set; }
        public int AgeRating { get; set; }
        public string SpokenLanguage { get; set; }
        public string TextLanguage { get; set; }
    }
}