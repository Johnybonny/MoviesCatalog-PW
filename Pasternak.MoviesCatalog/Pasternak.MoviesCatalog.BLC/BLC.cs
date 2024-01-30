using Pasternak.MoviesCatalog.Interfaces;
using System.Configuration;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using Pasternak.MoviesCatalog.Core;

namespace Pasternak.MoviesCatalog.BLC
{
    public class BLC
    {
        private IDAO dao;

        public BLC(IConfiguration configuration)
        {

            string libraryName = System.Configuration.ConfigurationManager.AppSettings["DAOLibraryName"]!;
            string path = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string libraryPath = string.Join("\\", path, libraryName);
            Assembly assembly = Assembly.UnsafeLoadFrom(libraryPath);
            Type? typeToCreate = null;

            foreach (Type type in assembly.GetTypes())
            {
                if (type.IsAssignableTo(typeof(IDAO)))
                {
                    typeToCreate = type;
                    break;
                }
            }
            ConstructorInfo? constructor = typeToCreate!.GetConstructor([typeof(IConfiguration)]);
            if (constructor != null)
            {
                dao = (IDAO)constructor.Invoke(new object[] { configuration })!;
            }
            else
            {
                dao = (IDAO)Activator.CreateInstance(typeToCreate!, null)!;
            }
        }

        public BLC(IDAO dao)
        {
            this.dao = dao;
        }

        public IEnumerable<IMovie> GetMovies()
        {
            return dao.GetAllMovies();
        }
        public IEnumerable<IDirector> GetDirectors()
        {
            return dao.GetAllDirectors();
        }
        public IMovie? GetMovie(string id)
        {
            return dao.GetMovie(id);
        }
        public IDirector? GetDirector(string id)
        {
            return dao.GetDirector(id);
        }
        public IMovie CreateNewMovie(string title, int releaseYear, Genre genre, string directorID)
        {
            return dao.CreateNewMovie(title, releaseYear, genre, directorID);
        }
        public IDirector CreateNewDirector(string name, int age, string nationality)
        {
            return dao.CreateNewDirector(name, age, nationality);
        }
        public void EditMovie(string id, string title, int releaseYear, Genre genre, string directorID)
        {
            dao.EditMovie(id, title, releaseYear, genre, directorID);
        }
        public void EditDirector(string id, string name, int age, string nationality)
        {
            dao.EditDirector(id, name, age, nationality);
        }
        public void DeleteMovie(string id)
        {
            dao.DeleteMovie(id);
        }
        public void DeleteDirector(string id)
        {
            dao.DeleteDirector(id);
        }
    }
}
