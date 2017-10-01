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
    public class JegerPage : Base.ContentPageJL
    {
        private JegerVM VM;
        public JegerPage(Jeger jeger)
        {
            BindingContext = VM = new JegerVM(jeger);
            Init();
        }

        private void Init()
        {
            Title = VM.CurrentJeger.Navn;

            var tableSection = new TableSection();
            tableSection.Add(new JL_EntryCell("Fornavn", VM.CurrentJeger.Fornavn, "CurrentJeger.Fornavn", EntryComplete));
            tableSection.Add(new JL_EntryCell("Etternavn", VM.CurrentJeger.Etternavn, "CurrentJeger.Etternavn", EntryComplete));
            tableSection.Add(new JL_EntryCell("E-post", VM.CurrentJeger.Email, "CurrentJeger.Email", EntryComplete)
            {
                Keyboard = Keyboard.Email
            });
            tableSection.Add(new JL_EntryCell("Mobil", VM.CurrentJeger.Phone, "CurrentJeger.Phone", EntryComplete)
            {
                Keyboard = Keyboard.Telephone
            });
            tableSection.Add(new JL_ImageCell("Bilde", VM.CurrentJeger.Image, ImageCell_OnTapped));


            Content = new TableViewJL
            {
                HasUnevenRows = true,
                Root = new TableRoot
                {
                    tableSection,
                    new TableSection()
                    {
                        new JL_ButtonCell("Slett jeger", ButtonDelete_OnClicked)
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
            await Navigation.PushAsync(new MediaPage($"dog_{VM.CurrentJeger.ID}.jpg", VM.CurrentJeger.ImagePath, delegate (MediaPage mediaPage)
            {
                VM.CurrentJeger.ImagePath = mediaPage.ImagePath;
                VM.Save();
                Init();
            }), true);
        }
        private async void ButtonDelete_OnClicked(object sender, EventArgs e)
        {
            var ok = await DisplayAlert("Bekreft sletting", "Jeger blir permanent slettet og fjernet fra alle loggføringer.", "Slett", "Avbryt");
            if (ok)
            {
                VM.Delete();
                await Navigation.PopAsync(true);
            }
        }
    }
}
