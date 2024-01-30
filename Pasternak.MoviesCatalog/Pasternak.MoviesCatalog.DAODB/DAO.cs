using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Pasternak.MoviesCatalog.Core;
using Pasternak.MoviesCatalog.DAODB.BO;
using Pasternak.MoviesCatalog.Interfaces;

namespace Pasternak.MoviesCatalog.DAODB
{
    public class DAO : DbContext, IDAO
    {
        private IConfiguration _configuration;

        public DAO(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string currentDirectory = AppContext.BaseDirectory;

            while (currentDirectory != null)
            {
                if (Directory.GetFiles(currentDirectory, "*.csproj").Length > 0 ||
                    Directory.GetFiles(currentDirectory, "*.sln").Length > 0)
                {
                    break;
                }
                currentDirectory = Path.GetDirectoryName(currentDirectory);
            }

            string dbPath = Path.Combine(currentDirectory, "movies.db");

            optionsBuilder.UseSqlite($"Data Source={dbPath}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Director>()
                .HasMany(p => (ICollection<Movie>)p.Movies)
                .WithOne(b => (Director)b.Director);
            base.OnModelCreating(modelBuilder);
        }

        public virtual DbSet<Director> Directors { get; set; }
        public virtual DbSet<Movie> Movies { get; set; }

        public IMovie CreateNewMovie(string title, int releaseYear, Genre genre, string directorID)
        {
            Director? director = Directors.Find(directorID);
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
            Movies.Add(movie);
            SaveChanges();
            return movie;
        }

        public IDirector CreateNewDirector(string name, int age, string nationality)
        {
            Director director = new Director
            {
                ID = Guid.NewGuid().ToString(),
                Name = name,
                Age = age,
                Nationality = nationality,
                Movies = new List<IMovie>()
            };
            Directors.Add(director);
            SaveChanges();
            return director;
        }

        public IEnumerable<IMovie> GetAllMovies()
        {
            return Movies.Include(b => b.Director).ToList();
        }

        public IEnumerable<IDirector> GetAllDirectors()
        {
            return Directors.ToList();
        }

        public void EditDirector(string id, string name, int age, string nationality)
        {
            Director? director = Directors.Find(id);
            if (director != null)
            {
                director.Name = name;
                director.Age = age;
                director.Nationality = nationality;
                SaveChanges();
            }
        }

        public void DeleteDirector(string id)
        {
            Director? director = Directors.Find(id);
            if (director != null)
            {
                Directors.Remove(director);
                SaveChanges();
            }
        }

        public void EditMovie(string id, string title, int releaseYear, Genre genre, string directorID)
        {
            Movie? movie = Movies.Find(id);
            if (movie != null)
            {
                Director? director = Directors.Find(directorID);
                if (director == null)
                {
                    throw new ArgumentException("Director not found");
                }
                movie.Title = title;
                movie.ReleaseYear = releaseYear;
                movie.MovieGenre = genre;
                movie.Director = director;
                SaveChanges();
            }
        }

        public void DeleteMovie(string id)
        {
            Movie? movie = Movies.Find(id);
            if (movie != null)
            {
                Movies.Remove(movie);
                SaveChanges();
            }
        }

        public IMovie? GetMovie(string id)
        {
            return Movies.Find(id);
        }

        public IDirector? GetDirector(string id)
        {
            return Directors.Include(p => p.Movies).FirstOrDefault(p => p.ID == id);
        }
    }
}
