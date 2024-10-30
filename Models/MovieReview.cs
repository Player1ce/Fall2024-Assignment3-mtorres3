using System.ComponentModel.DataAnnotations.Schema;

namespace Fall2024_Assignment3_mtorres3.Models
{
    public class MovieReview : SentimentAnalyzedText
    {
        [ForeignKey("Movie")]
        public int? MovieID { get; set; }
        public Movie? Movie { get; set; }
    }
}
