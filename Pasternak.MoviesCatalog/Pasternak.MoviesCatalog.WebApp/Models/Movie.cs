using Pasternak.MoviesCatalog.Core;
using Pasternak.MoviesCatalog.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Pasternak.MoviesCatalog.WebApp.Models
{
    public class Movie: IMovie
    {
        public string ID { get; set; }
        public string Title { get; set; }
        public int ReleaseYear { get; set; }
        public Genre MovieGenre { get; set; }
        [Display(Name = "Director")]
        public IDirector Director { get; set; }
    }
}
