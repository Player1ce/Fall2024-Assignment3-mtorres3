using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fall2024_Assignment3_mtorres3.Models;

public class MovieActor
{
    [Key]
    public int ID { get; set; }

    [ForeignKey("Movie")]
    public int MovieID { get; set; }
    public Movie? Movie { get; set; }

    [ForeignKey("Actor")]
    public int ActorID { get; set; }
    public Actor? Actor { get; set; }

}