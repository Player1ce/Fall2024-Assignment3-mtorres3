namespace Fall2024_Assignment3_mtorres3.Models;

public class MovieDetailsViewModel(Movie movie, IEnumerable<Actor> actors)
{
    public Movie Movie { get; set; } = movie;

    public IEnumerable<Actor> Actors { get; set; } = actors;
}