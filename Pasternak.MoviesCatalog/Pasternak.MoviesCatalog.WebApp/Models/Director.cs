using Microsoft.EntityFrameworkCore;
using Pasternak.MoviesCatalog.Interfaces;

namespace Pasternak.MoviesCatalog.WebApp.Models
{
    [Index(nameof(Name), IsUnique = true)]
    public class Director: IDirector
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Nationality { get; set; }
        public List<IMovie> Movies { get; set; }
    }
}
