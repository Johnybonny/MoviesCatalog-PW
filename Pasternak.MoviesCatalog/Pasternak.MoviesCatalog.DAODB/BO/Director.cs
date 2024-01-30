using Pasternak.MoviesCatalog.Interfaces;

namespace Pasternak.MoviesCatalog.DAODB.BO
{
    public class Director : IDirector
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Nationality { get; set; }
        public List<IMovie> Movies { get; set; }
    }
}
