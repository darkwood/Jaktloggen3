using System;

using Jaktloggen.Models;
using Jaktloggen.ViewModels;
using Jaktloggen.Views.Base;
using Jaktloggen.Views.Cells;
using Jaktloggen.Views.Input;

using Xamarin.Forms;

namespace Jaktloggen.Views
{
    public class ArtPage : Base.ContentPageJL
    {
        private ArtVM VM;
        public ArtPage(Art art)
        {
            BindingContext = VM = new ArtVM(art);
            Init();
        }
        private void Init()
        {
            Title = VM.CurrentArt.Navn;

            var tableSection = new TableSection();
            tableSection.Add(new JL_EntryCell("Navn", VM.CurrentArt.Navn, "CurrentArt.Navn", EntryComplete));
            tableSection.Add(new JL_ImageCell("Bilde", VM.CurrentArt.Image, ImageCell_OnTapped));


            Content = new TableViewJL
            {
                HasUnevenRows = true,
                Root = new TableRoot
                {
                    tableSection,
                    new TableSection()
                    {
                        new JL_ButtonCell("Slett", ButtonDelete_OnClicked)
                    },
                },
            };
        }
        private void EntryComplete(object sender, EventArgs e)
        {
            VM.Save();
        }
        private async void ImageCell_OnTapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MediaPage($"art_{VM.CurrentArt.ID}.jpg", VM.CurrentArt.ImagePath, delegate (MediaPage mediaPage)
            {
                VM.CurrentArt.ImagePath = mediaPage.ImagePath;
                VM.Save();
                Init();
            }), true);
        }
        
        private async void ButtonDelete_OnClicked(object sender, EventArgs e)
        {
            var ok = await DisplayAlert("Bekreft sletting", "Arten blir permanent slettet og fjernet fra alle loggføringer.", "Slett", "Avbryt");
            if (ok)
            {
                VM.Delete();
                await Navigation.PopAsync(true);
            }
        }
    }
}
