using System.Threading.Tasks;

using Jaktloggen.Helpers;
using Jaktloggen.Models;

using MvvmHelpers;

namespace Jaktloggen.ViewModels
{
    
    public class DogVM : ObservableObject
    {

        public bool DogExists => CurrentDog.ID > 0;

        public Dog CurrentDog { get; set; }

        public DogVM(Dog currentDog)
        {
            if (currentDog.ID == 0)
            {
                currentDog.ID = App.Database.SaveDog(currentDog);
            }
            CurrentDog = currentDog;
        }

        public void BindData()
        {
            var id = CurrentDog.ID;
            CurrentDog = null;
            CurrentDog = App.Database.GetDog(id);
        }

        public void Save()
        {
            App.Database.SaveDog(CurrentDog);
        }

        public void Delete()
        {
            App.Database.DeleteDog(CurrentDog);
        }
    }
}
