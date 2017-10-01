using System;
using System.Threading.Tasks;

using MvvmHelpers;

namespace Jaktloggen.ViewModels
{
    public class SettingsVM : ObservableObject
    {
        public string Title = "Verktøy";

        public Task Export()
        {
            throw new NotImplementedException();
            //File.Save(App.Database.GetJakts(), "jakt.xml");
        }

        public Task Import()
        {
            throw new NotImplementedException();
        }
    }
}
