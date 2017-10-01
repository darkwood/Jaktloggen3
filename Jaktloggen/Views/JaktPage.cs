using System;
using System.Linq;

using Jaktloggen.Models;
using Jaktloggen.ViewModels;
using Jaktloggen.Views.Base;
using Jaktloggen.Views.Cells;
using Jaktloggen.Views.Input;
using Xamarin.Forms;

namespace Jaktloggen.Views
{
    public class JaktPage : Base.ContentPageJL
    {
        private JaktVM VM;
        public JaktPage(Jakt jakt)
        {
            BindingContext = VM = new JaktVM(jakt);
        }

        protected override void OnAppearing()
        {
            Title = "Rediger jakt";
            base.OnAppearing();
            Init();
        }

        public void Init()
        {
            VM.BindData();
            
            
            var tableSection = new TableSection("");
            tableSection.Add(new JL_TextCell("Sted", VM.CurrentJakt.Title, StedCell_OnTapped));
            tableSection.Add(new JL_ImageCell("Jaktbilde", VM.CurrentJakt.Image, ImageCell_OnTapped));
            tableSection.Add(new JL_TextCell("Dato", VM.CurrentJakt.DatoFraTil, DateCell_OnTapped));
            tableSection.Add(new JL_TextCell("Posisjon", VM.CurrentJakt.Position, Posisjon_OnTapped));
            tableSection.Add(new JL_TextCell("Jegere på jaktlaget", VM.CurrentJakt.JegereInJakt, Jegere_OnTapped));
            tableSection.Add(new JL_TextCell("Hunder på jaktlaget", VM.CurrentJakt.DogsInJakt, Dogs_OnTapped));
            tableSection.Add(new JL_TextCell("Notater", VM.CurrentJakt.Notes, NoteCell_OnTapped));
            tableSection.Add(new JL_TextCell("Se alle loggføringer på kartet", ">>", ViewLogsOnMap_OnTapped));

            Content = new TableViewJL
            {
                HasUnevenRows = true,
                Root = new TableRoot
                {
                    tableSection,
                    new TableSection()
                    {
                        new JL_ButtonCell("Slett jakt", ButtonDelete_OnClicked)
                    },
                },
            };
        }

        private void Dogs_OnTapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new DogSelectorPage(VM.CurrentJakt.ID, VM.CurrentJakt.DogIds));
        }

        private void Jegere_OnTapped(object sender, EventArgs e)
        {
            Navigation.PushAsync(new JegerSelectorPage(VM.CurrentJakt.ID, VM.CurrentJakt.JegerIds));
        }

        private async void ViewLogsOnMap_OnTapped(object sender, EventArgs eventArgs)
        {
            await Navigation.PushAsync(new PositionLogsPage(VM.ItemCollection));
        }

        private async void Posisjon_OnTapped(object sender, EventArgs eventArgs)
        {
            await Navigation.PushModalAsync(
                new PositionPage(VM.CurrentJakt, delegate(PositionPage page)
                {
                    VM.CurrentJakt.Latitude = page.VM.LatitudeString;
                    VM.CurrentJakt.Longitude = page.VM.LongitudeString;
                    VM.Save();
                }));
        }

        private async void StedCell_OnTapped(object sender = null, EventArgs e = null)
        {
            var autoEntries = VM.AllJaktNames.Where(a => !a.Equals(VM.CurrentJakt.Sted, StringComparison.CurrentCultureIgnoreCase));
            await Navigation.PushAsync(
                new EntryPage(
                    "Sted", 
                    VM.CurrentJakt.Sted,
                    autoCompleteEntries: autoEntries)
                    {
                        Callback = delegate (EntryPage entryPage)
                        {
                            VM.CurrentJakt.Sted = entryPage.Value;
                            VM.Save();
                        }
                    });

        }

        private async void NoteCell_OnTapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new EntryPage("Notater", VM.CurrentJakt.Notes)
                
            {
                Callback = delegate (EntryPage entryPage)
                {
                    VM.CurrentJakt.Notes = entryPage.Value;
                    VM.Save();
                },
                Multiline = true
            });
        }
        private async void ImageCell_OnTapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MediaPage($"jakt_{VM.CurrentJakt.ID}.jpg", VM.CurrentJakt.ImagePath, delegate (MediaPage mediaPage)
            {
                VM.CurrentJakt.ImagePath = mediaPage.ImagePath;
                VM.Save();
                Init();
            }), true);
            
        }

        private async void DateCell_OnTapped(object sender, EventArgs e)
        {
            await
                Navigation.PushAsync(new DatePage("Velg dato fra - til", VM.CurrentJakt.DatoFra, VM.CurrentJakt.DatoTil,
                    delegate(DatePage datePage)
                    {
                        VM.CurrentJakt.DatoFra = datePage.DateFrom;
                        VM.CurrentJakt.DatoTil = datePage.DateTo;
                        VM.Save();
                    }));
        }

        private async void ButtonDelete_OnClicked(object sender, EventArgs e)
        {
            var ok = await DisplayAlert("Bekreft sletting", "Jakt og alle loggføringer blir slettet.", "Slett", "Avbryt");
            if (ok)
            {
                VM.Delete();
                await Navigation.PopToRootAsync();
            }
        }
    }
}
