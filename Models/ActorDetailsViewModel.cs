namespace Fall2024_Assignment3_mtorres3.Models;

public class ActorDetailsViewModel(Actor actor, IEnumerable<Actor> actors)
{
    public Actor Actor { get; set; } = actor;

    public IEnumerable<Actor> Actors { get; set; } = actors;
}