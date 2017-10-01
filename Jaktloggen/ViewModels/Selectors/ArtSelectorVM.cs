using System;
using System.Linq;

using Jaktloggen.Models;

using MvvmHelpers;

namespace Jaktloggen.ViewModels
{
    public class ArtSelectorGroup : ObservableRangeCollection<Art>
    {
        public String Name { get; private set; }
        public String ShortName { get; private set; }

        public ArtSelectorGroup(String Name, String ShortName)
        {
            this.Name = Name;
            this.ShortName = ShortName;
        }

        // Whatever other properties
    }

    
    public class ArtSelectorVM : ObservableObject
    {
        public Logg CurrentLogg { get; set; }
        public ObservableRangeCollection<ArtSelectorGroup> GroupedItems { get; set; } = new ObservableRangeCollection<ArtSelectorGroup>();
        public ArtSelectorVM(Logg currentLogg)
        {
            CurrentLogg = currentLogg;
        }

        public void BindData()
        {
            GroupedItems = new ObservableRangeCollection<ArtSelectorGroup>();

            var artGroups = App.Database.GetArtGroups();
            var arter = App.Database.GetArter();

            var arterInJakt = new ArtSelectorGroup("Mine favoritter", "");
            foreach (var art in arter.Where(j => j.Selected))
            {
                //art.Selected = CurrentLogg.ArtId == art.ID; //Remove selected for all but the picked art
                arterInJakt.Add(art);
            }
            if (arterInJakt.Count > 0)
            {
                GroupedItems.Add(arterInJakt);
            }

            foreach (var g in artGroups)
            {
                var arterInGroup = arter.Where(a => a.GroupId == g.ID);

                if (arterInGroup.Any())
                {
                    var ag = new ArtSelectorGroup(g.Navn, "");

                    foreach (var art in arterInGroup)
                    {
                        if (arterInJakt.All(a => a.ID != art.ID))
                        {
                            ag.Add(art);
                        }
                    }

                    if (ag.Count > 0)
                    {
                        GroupedItems.Add(ag);
                    }
                }
            }
        }

        public void SetArt(Art selectedArt)
        {
            CurrentLogg.Art = selectedArt;
            App.Database.AddSelectedArt(selectedArt);
            App.Database.SaveLogg(CurrentLogg);
        }


        public void RemoveArt()
        {
            CurrentLogg.Art = new Art();
            App.Database.SaveLogg(CurrentLogg);
        }
        
    }
}
