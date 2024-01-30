using Pasternak.MoviesCatalog.Core;
using Pasternak.MoviesCatalog.Interfaces;

namespace Pasternak.MoviesCatalog.DAOMock2.BO
{
    public class Movie : IMovie
    {
        public string ID { get; set; }
        public string Title { get; set; }
        public int ReleaseYear { get; set; }
        public Genre MovieGenre { get; set; }
        public IDirector Director { get; set; }
    }
}
