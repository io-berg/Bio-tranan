using System.ComponentModel.DataAnnotations;

namespace MovieTheaterCore.Models
{
    public class ReviewCreationModel
    {
        public long MovieId { get; set; }
        public string ReviewerName { get; set; }
        public int Score { get; set; }
        public string ReviewContent { get; set; }
        public string ReservationCode { get; set; }
    }
}