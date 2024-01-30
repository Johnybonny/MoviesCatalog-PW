using Pasternak.MoviesCatalog.Core;
using Pasternak.MoviesCatalog.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pasternak.MoviesCatalog.DAODB.BO
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
