namespace MovieTheaterCore.Models
{
    public class Actor
    {
        public long Id { get; private set; }
        public string Name { get; set; }
        public long MovieId { get; set; }
    }
}