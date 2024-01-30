using Microsoft.EntityFrameworkCore;
using Pasternak.MoviesCatalog.WebApp.Models;
using System.Net.NetworkInformation;

namespace Pasternak.MoviesCatalog.WebApp
{
    public class DataContext: DbContext
    {
        private IConfiguration _configuration { get; set; }
        public DataContext(DbContextOptions<DataContext> options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(_configuration.GetConnectionString("Sqlite"));
        }

        public virtual DbSet<Movie> Movies { get; set; }
        public virtual DbSet<Director> Directors { get; set; }
    }
}
