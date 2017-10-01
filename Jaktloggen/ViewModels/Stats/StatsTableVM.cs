using System.Linq;
using Jaktloggen.Models;
using MvvmHelpers;


namespace Jaktloggen.ViewModels.Stats
{
    
    public class StatsTableVM
    {
        public ObservableRangeCollection<StatsTableItem> ItemCollection { get; set; }

        public StatsTableVM()
        {
            ItemCollection = new ObservableRangeCollection<StatsTableItem>();

            var loggs = App.Database.GetLoggs();
            var jegere = App.Database.GetJegere();

            foreach (var jeger in jegere)
            {
                //var statsTableItem = new StatsTableItem();
                //statsTableItem.Jeger = jeger.Navn;
                //var loggsWithJeger = loggs.Where(l => l.JegerId == jeger.ID);
                //statsTableItem.Sett = loggsWithJeger.Sum(s => s.Sett);
                //statsTableItem.Skudd = loggsWithJeger.Sum(s => s.Skudd);
                //statsTableItem.Treff = loggsWithJeger.Sum(s => s.Treff);

            }
        }
    }
}
