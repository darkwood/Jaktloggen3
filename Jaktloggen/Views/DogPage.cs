using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using Jaktloggen.Models;
using Jaktloggen.ViewModels;
using Jaktloggen.Views.Base;
using Jaktloggen.Views.Cells;
using Jaktloggen.Views.Input;

using Xamarin.Forms;

namespace Jaktloggen.Views
{
    public class DogPage : Base.ContentPageJL
    {
        private DogVM VM;
        public DogPage(Dog dog)
        {
            BindingContext = VM = new DogVM(dog);
            Init();
        }

        private void Init()
        {
            Title = VM.CurrentDog.Navn;

            var tableSection = new TableSection();
            tableSection.Add(new JL_EntryCell("Navn", VM.CurrentDog.Navn, "CurrentDog.Navn", EntryComplete));
            tableSection.Add(new JL_EntryCell("Rase", VM.CurrentDog.Rase, "CurrentDog.Rase", EntryComplete));
            tableSection.Add(new JL_EntryCell("Lisensnummer", VM.CurrentDog.Lisensnummer, "CurrentDog.Lisensnummer",
                EntryComplete));
            tableSection.Add(new JL_ImageCell("Bilde", VM.CurrentDog.Image, ImageCell_OnTapped));


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
            await Navigation.PushAsync(new MediaPage($"dog_{VM.CurrentDog.ID}.jpg", VM.CurrentDog.ImagePath, delegate (MediaPage mediaPage)
            {
                VM.CurrentDog.ImagePath = mediaPage.ImagePath;
                VM.Save();
                Init();
            }), true);
        }
        private async void ButtonDelete_OnClicked(object sender, EventArgs e)
        {
            var ok = await DisplayAlert("Bekreft sletting", "Hund blir permanent slettet og fjernet fra alle loggføringer.", "Slett", "Avbryt");
            if (ok)
            {
                VM.Delete();
                await Navigation.PopAsync(true);
            }
        }
    }
}
