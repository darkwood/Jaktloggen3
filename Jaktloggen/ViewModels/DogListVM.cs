using Jaktloggen.Models;

using MvvmHelpers;

namespace Jaktloggen.ViewModels
{
    
    public class DogListVM
    {
        public ObservableRangeCollection<Dog> ItemCollection { get; set; }
        public DogListVM()
        {
            ItemCollection = new ObservableRangeCollection<Dog>();
        }

        public void BindData()
        {
            ItemCollection.ReplaceRange(App.Database.GetDogs());
        }
    }
}
