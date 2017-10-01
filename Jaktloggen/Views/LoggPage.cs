using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Jaktloggen.Helpers;
using Jaktloggen.Models;
using Jaktloggen.ViewModels;
using Jaktloggen.Views.Base;
using Jaktloggen.Views.Cells;
using Jaktloggen.Views.Input;
using Xamarin.Forms;

namespace Jaktloggen.Views
{
    public class LoggPage : Base.ContentPageJL
    {
        private LoggVM VM;
        private ActivityIndicator PositionActivityIndicator;

        public JL_LocationCell PositionCell { get; private set; }

        public LoggPage(Logg logg)
        {
            PositionActivityIndicator = new ActivityIndicator();
            BindingContext = VM = new LoggVM(logg);
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            Init();

            if(VM.IsNew)
            {
                TryGetPosition();
            }
            else if(!string.IsNullOrWhiteSpace(VM.CurrentLogg.Longitude)){
                PositionCell.LoadingPositionCompleted("Vis i kart", true);
            }
            else{
                PositionCell.LoadingPositionCompleted("Trykk for å velge posisjon", false);
            }
        }

        public async void TryGetPosition()
        {
            PositionCell.StartLoadingPosition("Henter posisjon...");
			var position = await PositionHelper.GetCurrentLocation();
			if (position != null)
			{
                VM.CurrentLogg.Latitude = position.Latitude.ToString();
				VM.CurrentLogg.Longitude = position.Longitude.ToString();
                PositionCell.LoadingPositionCompleted("Vis i kart", true);
            }
            else {
                PositionCell.LoadingPositionCompleted("Fant ikke posisjon. Trykk for å prøve igjen.", false);

			}
        }

        public void Init()
        {
            VM.BindData();
            Title = VM.CurrentLogg.Title;

            var tableSection = new TableSection();
            PositionCell = new JL_LocationCell("Posisjon", 
                                               delegate (object sender, EventArgs args) {
				   Navigation.PushModalAsync(new PositionPage(VM.CurrentLogg, delegate (PositionPage page)
				   {
					   VM.CurrentLogg.Latitude = page.VM.LatitudeString;
					   VM.CurrentLogg.Longitude = page.VM.LongitudeString;
					   VM.Save();
                }));
            });
            tableSection.Add(PositionCell);
            
            tableSection.Add(new JL_TextCell("Art", VM.CurrentLogg.Art.Navn, delegate (object sender, EventArgs args)
            {
                Navigation.PushAsync(new ArtSelectorPage(VM.CurrentLogg));
            }));
            tableSection.Add(CreateNumericTextCell("Antall treff", VM.CurrentLogg.Treff.ToString(), delegate (EntryPage entryPage)
            {
                VM.CurrentLogg.Treff = GetNumericValue(entryPage.Value);
                if (VM.CurrentLogg.Treff > VM.CurrentLogg.Skudd)
                {
                    VM.CurrentLogg.Skudd = VM.CurrentLogg.Treff;
                }
                if (VM.CurrentLogg.Skudd > VM.CurrentLogg.Sett)
                {
                    VM.CurrentLogg.Sett = VM.CurrentLogg.Skudd;
                }

                VM.Save();
            }));
            tableSection.Add(CreateNumericTextCell("Antall skudd", VM.CurrentLogg.Skudd.ToString(), delegate (EntryPage entryPage)
            {
                VM.CurrentLogg.Skudd = GetNumericValue(entryPage.Value);
                if (VM.CurrentLogg.Skudd > VM.CurrentLogg.Sett)
                {
                    VM.CurrentLogg.Sett = VM.CurrentLogg.Skudd;
                }
                VM.Save();
            }));
            tableSection.Add(CreateNumericTextCell("Antall observert", VM.CurrentLogg.Sett.ToString(), delegate (EntryPage entryPage)
            {
                VM.CurrentLogg.Sett = GetNumericValue(entryPage.Value);
                VM.Save();
            }));

            tableSection.Add(new JL_TextCell("Jeger", VM.CurrentLogg.Jeger.Navn, delegate (object sender, EventArgs args)
            {
                var jakt = App.Database.GetJakt(VM.CurrentLogg.JaktId);
                Navigation.PushAsync(new JegerSelectorPage(jakt.ID, jakt.JegerIds, VM.CurrentLogg));
            }));
            tableSection.Add(new JL_TextCell("Hund", VM.CurrentLogg.Dog.Navn, delegate (object sender, EventArgs args)
            {
                var jakt = App.Database.GetJakt(VM.CurrentLogg.JaktId);
                Navigation.PushAsync(new DogSelectorPage(jakt.ID, jakt.DogIds, VM.CurrentLogg));
            }));
            tableSection.Add(new JL_ImageCell("Bilde", VM.CurrentLogg.Image, ImageCell_OnTapped));
            tableSection.Add(new JL_TextCell("Tidspunkt", VM.CurrentLogg.DateFormatted,
                delegate (object sender, EventArgs args)
                {
                    Navigation.PushAsync(new DatePage("Tidspunkt", VM.CurrentLogg.Dato, callback: DateSelected, useTime: true));
                }));


            var customFieldsSection = new TableSection("Ekstra felter");
            foreach (var item in App.Database.GetSelectedLoggTyper())
            {
                switch (item.Key)
                {
                    case "Gender":
                        customFieldsSection.Add(new JL_TextCell(item.Navn, VM.CurrentLogg.Gender,
                            delegate (object sender, EventArgs args)
                            {
                                var items = new List<string>();
                                items.Add("Hannkjønn");
                                items.Add("Hunnkjønn");
                                Navigation.PushAsync(new SingleItemPicker("Velg kjønn", items,
                                    delegate (SingleItemPicker picker)
                                    {
                                        VM.CurrentLogg.Gender = picker.SelectedItem;
                                        VM.Save();
                                    }));

                            }));
                        break;
                    case "Age":
                        customFieldsSection.Add(new JL_TextCell(item.Navn, VM.CurrentLogg.Age,
                            delegate (object sender, EventArgs args)
                            {
                                var items = new List<string>();
                                items.Add("Kalv");
                                items.Add("1 1/2 år");
                                items.Add("2 1/2 år og eldre");
                                Navigation.PushAsync(new SingleItemPicker("Alder på storvilt", items,
                                    delegate (SingleItemPicker picker)
                                    {
                                        VM.CurrentLogg.Age = picker.SelectedItem;
                                        VM.Save();
                                    }));

                            }));
                        break;
                    case "Tags":
                        customFieldsSection.Add(new JL_EntryCell(item.Navn, VM.CurrentLogg.Tags.ToString(), "CurrentLogg.Tags", EntryComplete) { Keyboard = Keyboard.Numeric });
                        break;
                    case "Weight":
                        customFieldsSection.Add(new JL_EntryCell(item.Navn, VM.CurrentLogg.Weight.ToString(), "CurrentLogg.Weight", EntryComplete) { Keyboard = Keyboard.Numeric });
                        break;
                    case "ButchWeight":
                        customFieldsSection.Add(new JL_EntryCell(item.Navn, VM.CurrentLogg.ButchWeight.ToString(), "CurrentLogg.ButchWeight", EntryComplete) { Keyboard = Keyboard.Numeric });
                        break;
                    case "WeaponType":
                        customFieldsSection.Add(new JL_TextCell(item.Navn, VM.CurrentLogg.WeaponType,
                            delegate (object sender, EventArgs args)
                            {
                                Navigation.PushAsync(
                                    new EntryPage(
                                        item.Navn,
                                        VM.CurrentLogg.WeaponType)
                                    {
                                        Callback = delegate (EntryPage entryPage)
                                        {
                                            VM.CurrentLogg.WeaponType = entryPage.Value;
                                            VM.Save();
                                        },
                                        AutoCompleteEntries = VM.AllLogs.Select(l => l.WeaponType).Where(a => !a.Equals(VM.CurrentLogg.WeaponType, StringComparison.CurrentCultureIgnoreCase))
                                    }
                                );
                            }));
                        break;
                    case "Weather":
                        customFieldsSection.Add(new JL_EntryCell(item.Navn, VM.CurrentLogg.Weather, "CurrentLogg.Weather", EntryComplete));
                        break;
                }
            }

            Content = new TableViewJL
            {
                HasUnevenRows = true,
                Root = new TableRoot
                {
                    tableSection,
                    customFieldsSection,
                    new TableSection()
                    {
                        new JL_ButtonCell("Slett logg", ButtonDelete_OnClicked)
                    }
                }
            };
        }

        private static int GetNumericValue(string value)
        {
            var i = 0;
            if (int.TryParse(value, out i))
            {
                return i;
            }
            return 0;
        }

        private JL_TextCell CreateNumericTextCell(string label, string value, Action<EntryPage> callback)
        {
            return new JL_TextCell(label, value, delegate (object sender, EventArgs args)
            {
                Navigation.PushAsync(
                    new EntryPage(
                        label,
                        value,
                        true
                        )
                    {
                        Callback = callback
                    }
                    );
            });
        }

        private void DateSelected(DatePage datePage)
        {
            VM.CurrentLogg.Dato = datePage.DateFrom;
            VM.Save();
        }

        private void EntryComplete(object sender, EventArgs e)
        {
            VM.Save();
        }

        private async void ButtonDelete_OnClicked(object sender, EventArgs e)
        {
            var ok = await DisplayAlert("Bekreft sletting", "Loggføringen blir fjernet permanent.", "Slett", "Avbryt");
            if (ok)
            {
                VM.Delete();
                await Navigation.PopAsync(true);
            }
        }
        private async void ImageCell_OnTapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new MediaPage($"logg_{VM.CurrentLogg.ID}.jpg", VM.CurrentLogg.ImagePath, delegate (MediaPage mediaPage)
            {
                VM.CurrentLogg.ImagePath = mediaPage.ImagePath;
                VM.Save();
                Init();
            }), true);
        }
    }
}
