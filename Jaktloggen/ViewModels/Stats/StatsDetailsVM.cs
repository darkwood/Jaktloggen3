using System;

using Jaktloggen.Models;

using MvvmHelpers;

namespace Jaktloggen.ViewModels.Stats
{
    
    public class StatsDetailsVM : ObservableObject
    {
        public StatItem StatItem { get; set; }
        public ObservableRangeCollection<StatItem> ItemCollection { get; set; }
        public DateTime DateFrom { get; set; } = DateTime.Now.AddYears(-1);
        public DateTime DateTo { get; set; } = DateTime.Now;

        public StatsDetailsVM(StatItem statItem)
        {
            StatItem = statItem;
            ItemCollection = new ObservableRangeCollection<StatItem>();
            if (statItem.Items != null)
            {
                ItemCollection.AddRange(statItem.Items);
            }
        }
    }
}
