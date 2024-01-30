using Pasternak.MoviesCatalog.Core;

namespace Pasternak.MoviesCatalog.Interfaces
{
    public interface IMovie
    {
        string ID { get; set; }
        string Title { get; set; }
        int ReleaseYear { get; set; }
        Genre MovieGenre { get; set; }
        IDirector Director { get; set; }
    }
}
