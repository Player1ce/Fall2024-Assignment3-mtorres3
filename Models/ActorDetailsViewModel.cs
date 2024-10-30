namespace Fall2024_Assignment3_mtorres3.Models;

public class ActorDetailsViewModel(Actor actor, IEnumerable<Movie> movie)
{
    public Actor Actor { get; set; } = actor;

    public IEnumerable<Movie> Movies { get; set; } = movie;
}