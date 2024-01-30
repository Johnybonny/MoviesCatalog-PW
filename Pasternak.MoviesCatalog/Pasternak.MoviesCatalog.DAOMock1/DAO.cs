using Pasternak.MoviesCatalog.Core;
using Pasternak.MoviesCatalog.DAOMock1.BO;
using Pasternak.MoviesCatalog.Interfaces;

namespace Pasternak.MoviesCatalog.DAOMock1
{
    public class DAO : IDAO
    {
        private List<IDirector> directors;
        private List<IMovie> movies;

        public DAO()
        {
            directors =
            [
                new Director() {ID = Guid.NewGuid().ToString(), Name = "Christopher Nolan", Age = 53, Nationality = "British-American", Movies = new List<IMovie>()},
                new Director() {ID = Guid.NewGuid().ToString(), Name = "Quentin Tarantino", Age = 60, Nationality = "American", Movies = new List<IMovie>()}
            ];

            movies =
            [
                new Movie()
                {
                    ID = Guid.NewGuid().ToString(),
                    Title = "Oppenheimer",
                    ReleaseYear = 2023,
                    MovieGenre = Core.Genre.Drama,
                    Director = directors[0]
                },
                new Movie()
                {
                    ID = Guid.NewGuid().ToString(),
                    Title = "Interstellar",
                    ReleaseYear = 2014,
                    MovieGenre = Core.Genre.Drama,
                    Director = directors[0]
                },
                new Movie()
                {
                    ID = Guid.NewGuid().ToString(),
                    Title = "Pulp Fiction",
                    ReleaseYear = 1994,
                    MovieGenre = Core.Genre.Crime,
                    Director = directors[1]
                },
            ];

            directors[0].Movies.Add(movies[0]);
            directors[0].Movies.Add(movies[1]);
            directors[1].Movies.Add(movies[2]);
        }

        public IDirector CreateNewDirector(string name, int age, string nationality)
        {
            var director = new Director
            {
                ID = Guid.NewGuid().ToString(),
                Name = name,
                Age = age,
                Nationality = nationality,
                Movies = new List<IMovie>()
            };
            directors.Add(director);
            return director;
        }

        public IMovie CreateNewMovie(string title, int releaseYear, Genre genre, string directorID)
        {

            Director? director = (Director?)GetDirector(directorID);
            if (director == null)
            {
                throw new ArgumentException("Director not found");
            }
            Movie movie = new Movie
            {
                ID = Guid.NewGuid().ToString(),
                Title = title,
                ReleaseYear = releaseYear,
                MovieGenre = genre,
                Director = director
            };
            movies.Add(movie);
            director.Movies.Add(movie);
            return movie;
        }

        public IEnumerable<IDirector> GetAllDirectors()
        {
            return directors;
        }

        public IEnumerable<IMovie> GetAllMovies()
        {
            return movies;
        }

        public void EditDirector(string id, string name, int age, string nationality)
        {
            foreach (Director director in directors)
            {
                if (director.ID == id)
                { 
                    director.Name = name;
                    director.Age = age;
                    director.Nationality = nationality;
                    break;
                }
            }
        }

        public void EditMovie(string id, string title, int releaseYear, Genre genre, string directorID)
        {
            foreach (Movie movie in movies)
            {
                if (movie.ID == id)
                {
                    Director? director = (Director?)GetDirector(directorID);
                    if (director == null)
                    {
                        throw new ArgumentException("Director not found");
                    }
                    movie.Title = title;
                    movie.ReleaseYear = releaseYear;
                    movie.MovieGenre = genre;
                    movie.Director = director;
                }
            }
        }

        public void DeleteDirector(string id)
        {
            Director currentDirector = null;
            foreach (Director director in directors)
            {
                if (director.ID == id)
                {
                    currentDirector = director;
                    break;
                }
            }
            if (currentDirector != null)
            {
                List<Movie> moviesCopy = new List<Movie>(
                    currentDirector.Movies.Cast<Movie>()
                );
                foreach (Movie movie in moviesCopy)
                {
                    movies.Remove(movie);
                }
                directors.Remove(currentDirector);
            }
        }

        public void DeleteMovie(string id)
        {
            Movie currentMovie = null;
            foreach (Movie movie in movies)
            {
                if (movie.ID == id)
                {
                    currentMovie = movie;
                    break;
                }
            }
            if (currentMovie != null)
            {
                movies.Remove(currentMovie);
                GetDirector(currentMovie.Director.ID).Movies.Remove(currentMovie);
            }
        }

        public IDirector? GetDirector(string id)
        {
            foreach (Director director in directors)
            {
                if (director.ID == id)
                {
                    return director;
                }
            }
            return null;
        }

        public IMovie? GetMovie(string id)
        {
            foreach (Movie movie in movies)
            {
                if (movie.ID == id)
                {
                    return movie;
                }
            }
            return null;
        }
    }
}
