using System;
using System.Threading.Tasks;

using Jaktloggen.Helpers;
using Jaktloggen.Models;

using MvvmHelpers;

namespace Jaktloggen.ViewModels
{
    public class JegerVM :ObservableObject
    {
        public bool JegerExists => CurrentJeger.ID > 0;

        public Jeger CurrentJeger { get; set; }

        public JegerVM(Jeger currentJeger)
        {
            if (currentJeger.ID == 0)
            {
                currentJeger.ID = App.Database.SaveJeger(currentJeger);
            }
            CurrentJeger = currentJeger;
        }

        public void Init()
        {
            var id = CurrentJeger?.ID ?? 0;
            CurrentJeger = null;
            CurrentJeger = App.Database.GetJeger(id);
        }

        public int Save()
        {
            return App.Database.SaveJeger(CurrentJeger);
        }

        public void Delete()
        {
            App.Database.DeleteJeger(CurrentJeger);
        }
    }
}
