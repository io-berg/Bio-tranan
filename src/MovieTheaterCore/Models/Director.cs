namespace MovieTheaterCore.Models
{
    public class Director
    {
        public long Id { get; private set; }
        public string Name { get; set; }
        public long MovieId { get; set; }
    }
}