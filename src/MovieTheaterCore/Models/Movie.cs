using System.ComponentModel.DataAnnotations;

namespace MovieTheaterCore.Models;

public class Movie
{
    [Key]
    [Required]
    public long Id { get; private set; }
    public string Title { get; set; }
    public string Genre { get; set; }
    public List<Director> Directors { get; set; }
    public List<Actor> Actors { get; set; }
    public string Description { get; set; }
    public string SpokenLanguage { get; set; }
    public string TextLanguage { get; set; }
    public int Runtime { get; set; }
    public int ReleaseYear { get; set; }
    public int AgeRating { get; set; }
    public int AllowedViewings { get; set; }
}
