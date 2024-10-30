using System.ComponentModel.DataAnnotations;

namespace Fall2024_Assignment3_mtorres3.Models
{
    public class Movie
    {
        [Key]
        public int ID { get; set; }
        public string Title { get; set; }
        public string Genre { get; set; }
        public int ReleaseYear { get; set; }
        public string ImdbHyperlink { get; set; }
        public byte[]? Poster { get; set; }

        public IEnumerable<MovieReview> Reviews { get; set; } = new List<MovieReview>();
    }
}
