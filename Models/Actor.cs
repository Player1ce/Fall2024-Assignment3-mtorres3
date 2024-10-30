using System.ComponentModel.DataAnnotations;

namespace Fall2024_Assignment3_mtorres3.Models;

public class Actor
{
    [Key]
    public int ID { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
    public string IMDBHyperlink { get; set; }
    public byte[]? Photo { get; set; }

    public IEnumerable<ActorTweet> Tweets { get; set; } = new List<ActorTweet>();
}