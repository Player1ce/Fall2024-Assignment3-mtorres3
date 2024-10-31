using System.ComponentModel.DataAnnotations;
using VaderSharp2;

namespace Fall2024_Assignment3_mtorres3.Models;

public abstract class SentimentAnalyzedText
{
    [Key]
    public int ID { get; set; }
    public string Text { get; set; }
    public string Sentiment { get; set; }
}