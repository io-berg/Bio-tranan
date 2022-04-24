using System.ComponentModel.DataAnnotations;

namespace MovieTheaterCore.Models
{
    public class Review
    {
        public long Id { get; set; }
        [Required]
        public long MovieId { get; set; }
        [MaxLength(100)]
        public string ReviewerName { get; set; }
        [Range(1, 5)]
        public int Score { get; set; }
        [MaxLength(2000)]
        public string ReviewContent { get; set; }
    }
}