using CommunityToolkit.Mvvm.ComponentModel;
using Pasternak.MoviesCatalog.Interfaces;

namespace Pasternak.MoviesCatalog.MAUI.ViewModels
{
    public partial class DirectorViewModel : ObservableObject, IDirector, ICloneable
    {
        [ObservableProperty]
        private string iD;

        [ObservableProperty]
        private string name;

        [ObservableProperty]
        private int age;

        [ObservableProperty]
        private string nationality;

        [ObservableProperty]
        private List<IMovie> movies;

        public DirectorViewModel(IDirector director)
        {
            ID = director.ID;
            Name = director.Name;
            Age = director.Age;
            Nationality = director.Nationality;
            Movies = director.Movies;
        }

        public DirectorViewModel()
        {

        }

        public object Clone()
        {
            return new DirectorViewModel(this);
        }

        public override bool Equals(object obj)
        {
            if (obj is DirectorViewModel other)
            {
                return ID == other.ID;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return ID.GetHashCode();
        }
    }

}
