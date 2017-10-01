using System;
using System.Collections.Generic;
using System.Linq;

using Jaktloggen.Models;

using MvvmHelpers;

namespace Jaktloggen.ViewModels
{
    public class JegerSelectorGroup : ObservableRangeCollection<Jeger>
    {
        public String Name { get; private set; }
        public String ShortName { get; private set; }

        public JegerSelectorGroup(String Name, String ShortName)
        {
            this.Name = Name;
            this.ShortName = ShortName;
        }
    }

    
    public class JegerSelectorVM : ObservableObject
    {
        public int JaktId { get; set; }
        public List<int> JegerIds { get; set; }
        public Logg CurrentLogg { get; set; }
        public ObservableRangeCollection<JegerSelectorGroup> GroupedItems { get; set; }
        public List<Jeger> Jegere = new List<Jeger>();
        public JegerSelectorVM(int jaktId, List<int> jegerIds, Logg currentLogg = null)
        {
            JaktId = jaktId;
            JegerIds = jegerIds;
            CurrentLogg = currentLogg;
            GroupedItems = new ObservableRangeCollection<JegerSelectorGroup>();
        }

        public void BindData()
        {
            GroupedItems.Clear();
            Jegere = App.Database.GetJegere().ToList();

            foreach (var jeger in Jegere)
            {
                if (CurrentLogg != null)
                {
                    jeger.Selected = jeger.ID == CurrentLogg.JegerId;
                }
                else
                {
                    jeger.Selected = JegerIds.Contains(jeger.ID);
                }
            }

            var currentJegereHeader = "Jegere fra denne jakta";
            var otherJegereHeader = "Flere jegere";
            var allJegereHeader = "Velg jeger";

            var jegereInJakt = Jegere.Where(j => JegerIds.Contains(j.ID));
            if (jegereInJakt.Any()){
                var jegereInJaktGroup = new JegerSelectorGroup(currentJegereHeader, "");
                jegereInJaktGroup.AddRange(jegereInJakt);
                GroupedItems.Add(jegereInJaktGroup);
            }

            var otherJegerList = Jegere.Where(j => !JegerIds.Contains(j.ID));
            if (otherJegerList.Any())
            {
                var otherJegere = new JegerSelectorGroup(jegereInJakt.Any() ? otherJegereHeader : allJegereHeader, "");
                otherJegere.AddRange(otherJegerList);
                GroupedItems.Add(otherJegere);
            }
            
        }
        
        public void AddJeger(Jeger selectedJeger)
        {
            if (!JegerIds.Contains(selectedJeger.ID))
            {
                JegerIds = App.Database.AddJegerToJakt(JaktId, selectedJeger.ID);
            }

            if (CurrentLogg != null)
            {
                CurrentLogg.JegerId = selectedJeger.ID;
                App.Database.SaveLogg(CurrentLogg);
            }
        }

        public void RemoveJeger(Jeger selectedJeger)
        {
            selectedJeger.Selected = false;

            if (CurrentLogg != null)
            {
                CurrentLogg.Jeger = new Jeger();
                App.Database.SaveLogg(CurrentLogg);
            }

            if (JegerIds.Contains(selectedJeger.ID))
            {
                JegerIds = App.Database.RemoveJegerFromJakt(JaktId, selectedJeger.ID);
            }
        }

        public void UpdateJegerIds(Jeger selectedJeger)
        {
            if (JegerIds.Contains(selectedJeger.ID))
            {
                RemoveJeger(selectedJeger);
            }
            else
            {
                AddJeger(selectedJeger);
            }
        }
    }
}
