using System;
using System.Linq;
using System.Threading.Tasks;

using Jaktloggen.Helpers;
using Jaktloggen.Models;
using Jaktloggen.ViewModels;
using Jaktloggen.Views.Cells;
using Jaktloggen.Views.Extended;

using Xamarin.Forms;

namespace Jaktloggen.Views
{
    public class LoggListPage : Base.ContentPageJL
    {
        private JaktVM VM;
        private ActivityIndicator PositionActivityIndicator;

        public Label TitleLabel { get; private set; }

        public LoggListPage(Jakt jakt)
        {
            Title = "Loggføringer";
            PositionActivityIndicator = new ActivityIndicator();
            BindingContext = VM = new JaktVM(jakt);

			MessagingCenter.Subscribe<Models.EntityBase>(this, "DeleteEntity", async (item) =>
			{
				var i = item as Logg;
				if (i != null)
				{
					var ok = await DisplayAlert("Bekreft sletting", "Loggføringen blir fjernet permanent.", "Slett", "Avbryt");
					if (ok)
					{
						VM.Delete(i);
					}

				}
			});
        }


        protected override async void OnAppearing()
        {
            base.OnAppearing();
            Init();
			
            if (VM.IsNew)
			{
				ToggleLoadPosition();
				await TryGetPosition();
				ToggleLoadPosition();
			}


			

        }

        private void ToggleLoadPosition()
        {
            VM.IsLoadingPosition = !VM.IsLoadingPosition;
            PositionActivityIndicator.IsRunning = VM.IsLoadingPosition;
        }


        private async Task TryGetPosition()
        {

            var acceptUseGps = true; //await DisplayAlert("Hent posisjon", "Ønsker du å hente posisjon og sted fra GPS?", "Ja", "Nei");
            if (acceptUseGps)
            {
                

                var position = await PositionHelper.GetCurrentLocation();
                if (position != null)
                {
                    VM.CurrentJakt.Latitude = position.Latitude.ToString();
                    VM.CurrentJakt.Longitude = position.Longitude.ToString();

                    var address = await PositionHelper.GetLocationNameForPosition(position);

                    if (address != null && address.FeatureName != null)
                    {
                        TitleLabel.Text = VM.CurrentJakt.Sted = address.Locality;
                    }

                    VM.Save();

                }
            }
        }
        
        public void Init()
        {
            VM.BindData();

            var dateLabel = new Label() {FontSize = 12};
            TitleLabel = new Label() { FontSize = 16 };
            
            var circleImage = new CircleImage()
            {
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.Start,
                BorderThickness = 2,
                BorderColor = Color.White,
                HeightRequest = 170,
                WidthRequest = 170,
                Aspect = Aspect.AspectFill
            };

            var headerTextLayout = new StackLayout()
            {
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Children = { dateLabel, TitleLabel }
            };

            dateLabel.SetBinding(Label.TextProperty, new Binding("CurrentJakt.DatoFraTil"));
            circleImage.SetBinding(CircleImage.SourceProperty, new Binding("CurrentJakt.Image"));
            TitleLabel.SetBinding(Label.TextProperty, new Binding("CurrentJakt.Title"));

			headerTextLayout.SetBinding(StackLayout.IsVisibleProperty, new Binding("IsLoadingPosition", converter: new InverseBooleanConverter()));
            PositionActivityIndicator.SetBinding(ActivityIndicator.IsVisibleProperty, new Binding("IsLoadingPosition"));
            

            var jaktSummary = new StackLayout()
            {
                Orientation = StackOrientation.Vertical,

                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Padding = 5,
                GestureRecognizers = {
                        new TapGestureRecognizer {
                                Command = new Command (()=>Navigation.PushAsync(new JaktPage(VM.CurrentJakt))),
                        },
                },
                Children =
                {
                    circleImage,
                    headerTextLayout,
                    PositionActivityIndicator,
                }
            };
            ListView lv = new ListView();
            lv.HorizontalOptions = LayoutOptions.FillAndExpand;
            lv.VerticalOptions = LayoutOptions.FillAndExpand;
            lv.SetBinding(ListView.ItemsSourceProperty, new Binding("ItemCollection"));
            lv.ItemSelected += (sender, e) =>
            {
                if (e.SelectedItem != null)
                {
                    Navigation.PushAsync(new LoggPage((Logg)e.SelectedItem), true);
                    ((ListView)sender).SelectedItem = null;
                }
            }; //Remember to remove this event handler on dispoing of the page;
            DataTemplate dt = new DataTemplate(typeof(CircleImageCell));
            dt.SetBinding(CircleImageCell.TextProperty, new Binding("Title"));
            dt.SetBinding(CircleImageCell.DetailProperty, new Binding("Details"));
            dt.SetBinding(CircleImageCell.ImageSourceProperty, new Binding("Image"));
            lv.ItemTemplate = dt;

            if (VM.ItemCollection.Any())
            {
                if (!ToolbarItems.Any())
                {
                    ToolbarItems.Add(new ToolbarItem("Ny hendelse", null, CreateNewLog, ToolbarItemOrder.Primary));
                }

                Content = new StackLayout()
                {
                    Padding = 5,
                    Children =
                    {
                        jaktSummary,
                        lv
                    }
                };
            }
            else
            {
                var btn = new PrimaryButton()
                {
                    Text = "Opprett første hendelse",
                    VerticalOptions = LayoutOptions.Fill
                };
                btn.Clicked += delegate {
                    CreateNewLog();
                };

                Content = new StackLayout()
                {
                    VerticalOptions = LayoutOptions.Center,
                    Children =
                    {
                        jaktSummary,
                        btn
                    }
                };
            }
        }

        private async void CreateNewLog()
        {
			var logg = VM.CreateLogg();
			await Navigation.PushAsync(new LoggPage(logg), true);
        }
    }
}
