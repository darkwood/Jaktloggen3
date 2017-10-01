using System;
using System.Linq;

using Jaktloggen.Models;
using MvvmHelpers;

namespace Jaktloggen.ViewModels
{
    public class JaktGroup : ObservableRangeCollection<Jakt>
    {
        public String Name { get; private set; }
        public String ShortName { get; private set; }

        public JaktGroup(String Name, String ShortName)
        {
            this.Name = Name;
            this.ShortName = ShortName;
            this.CollectionChanged += (sender, e) => {
                //do
            };        
        }
    }
    
    public class JaktListVM : ObservableObject
    {
        public ObservableRangeCollection<JaktGroup> GroupedItems { get; set; }

        internal void Delete(Jakt item)
        {
            
            App.Database.DeleteJakt(item);
            var group = GroupedItems.FirstOrDefault(g => g.Any(x => x.ID == item.ID));
            group.Remove(item);
            //OnPropertyChanged("GroupedItems");
        }

        public JaktListVM()
        {
            GroupedItems = new ObservableRangeCollection<JaktGroup>();

        }
        public void BindData()
        {
            GroupedItems.Clear();

            var groups = App.Database.GetJakts().GroupBy(g => g.DatoFra.Year).OrderByDescending(o => o.Key);
            foreach (var g in groups)
            {
                var jg = new JaktGroup(g.Key.ToString(), "");
                jg.AddRange(g.ToList());
                GroupedItems.Add(jg);
            }
        }
        public Jakt CreateJakt()
        {
            var jakt = new Jakt();
            return jakt;
        }
    }
}
