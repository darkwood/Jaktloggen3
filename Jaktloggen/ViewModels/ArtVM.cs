using System.Threading.Tasks;
using Jaktloggen.Helpers;
using Jaktloggen.Models;
using MvvmHelpers;

namespace Jaktloggen.ViewModels
{
    
    public class ArtVM : ObservableObject
    {
        public bool ArtExists => CurrentArt.ID > 0;

        public Art CurrentArt { get; set; }

        public ArtVM(Art currentArt)
        {
            if (currentArt.ID == 0)
            {
                currentArt.GroupId = 100;
                currentArt.ID = App.Database.SaveArt(currentArt);
            }
            CurrentArt = currentArt;
        }

        public void BindData()
        {
            var id = CurrentArt.ID;
            CurrentArt = null;
            CurrentArt = App.Database.GetArt(id);
        }

        public void Save()
        {
            App.Database.SaveArt(CurrentArt);
        }

        public void Delete()
        {
            App.Database.DeleteArt(CurrentArt);
        }
    }
}
