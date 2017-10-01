using Jaktloggen.Models;

using MvvmHelpers;

namespace Jaktloggen.ViewModels
{
    public class JegerListVM
    {
        public ObservableRangeCollection<Jeger> ItemCollection { get; set; }
        public JegerListVM()
        {
            ItemCollection = new ObservableRangeCollection<Jeger>();
        }

        public void BindData()
        {
            ItemCollection.ReplaceRange(App.Database.GetJegere());
        }
    }
}
