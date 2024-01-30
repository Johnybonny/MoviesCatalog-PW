using Pasternak.MoviesCatalog.Core;

namespace Pasternak.MoviesCatalog.Interfaces
{
    public interface IDAO
    {
        IEnumerable<IDirector> GetAllDirectors();
        IEnumerable<IMovie> GetAllMovies();
        IDirector CreateNewDirector(string name, int age, string nationality);
        IMovie CreateNewMovie(string title, int releaseYear, Genre genre, string directorID);
        void EditDirector(string id, string name, int age, string nationality);
        void EditMovie(string id, string title, int releaseYear, Genre genre, string directorID);
        void DeleteDirector(string id);
        void DeleteMovie(string id);
        IDirector? GetDirector(string id);
        IMovie? GetMovie(string id);
        
    }
}
