using System.ComponentModel.DataAnnotations.Schema;

namespace Fall2024_Assignment3_mtorres3.Models;

public class ActorTweet : SentimentAnalyzedText
{
    [ForeignKey("Actor")]
    public int ActorID { get; set; }
    public Actor Actor { get; set; }
}