using System;
using System.Linq;

using Jaktloggen.Models;

using MvvmHelpers;

namespace Jaktloggen.ViewModels
{

    public class ArtGrouping : ObservableRangeCollection<Art>
    {
        public String Name { get; private set; }
        public String ShortName { get; private set; }

        public ArtGrouping(String Name, String ShortName)
        {
            this.Name = Name;
            this.ShortName = ShortName;
        }
    }
    
    public class ArtListVM
    {
        public ObservableRangeCollection<ArtGrouping> GroupedItems { get; set; }
        public ArtListVM()
        {
            GroupedItems = new ObservableRangeCollection<ArtGrouping>();
        }

        public void BindData()
        {
            GroupedItems = new ObservableRangeCollection<ArtGrouping>();

            var artGroups = App.Database.GetArtGroups();
            var arter = App.Database.GetArter();
            var selectedArtIds = App.Database.GetSelectedArter().Select(s => s.ID);
            foreach (var g in artGroups)
            {
                var arterInGroup = arter.Where(a => a.GroupId == g.ID);

                if (arterInGroup.Any())
                {
                    var ag = new ArtGrouping(g.Navn, "");

                    foreach (var art in arterInGroup)
                    {
                        art.Selected = selectedArtIds.Any(s => s == art.ID);
                        ag.Add(art);
                    }

                    GroupedItems.Add(ag);
                }
            }
        }

        public void ArtSelected(Art art)
        {
            art.Selected = !art.Selected;

            if (art.Selected)
            {
                App.Database.AddSelectedArt(art);
            }
            else
            {
                App.Database.RemoveSelectedArt(art);
            }

            BindData();
        }
    }
}
