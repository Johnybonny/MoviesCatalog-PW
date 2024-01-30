namespace Pasternak.MoviesCatalog.Interfaces
{
    public interface IDirector
    {
        string ID { get; set; }
        string Name { get; set; }
        int Age { get; set; }
        string Nationality { get; set; }
        List<IMovie> Movies { get; set; }
    }
}
