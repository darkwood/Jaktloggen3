using System;
using System.Linq;
using System.Threading.Tasks;
using Jaktloggen.Models;
using Jaktloggen.ViewModels;
using Jaktloggen.Views.Cells;
using Jaktloggen.Views.Extended;

using Xamarin.Forms;

namespace Jaktloggen.Views
{
    public class JaktListPage : Base.ContentPageJL
    {
        private JaktListVM VM;
        public JaktListPage()
        {
            Title = "Jaktloggen";
            BindingContext = VM = new JaktListVM();


			MessagingCenter.Subscribe<Models.EntityBase>(this, "DeleteEntity", async (item) =>
			{
				var i = item as Jakt;
				if (i != null)
				{
					var ok = await DisplayAlert("Bekreft sletting", "Jakt og alle loggføringer blir slettet.", "Slett", "Avbryt");
					if (ok)
					{
						VM.Delete(i);
					}
				}
			});
        }

        private async void CreateNewItem()
        {
            var jakt = VM.CreateJakt();
            await Navigation.PushAsync(new LoggListPage(jakt));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Init();

        }

        private void Init()
        {
            VM.BindData();

            ListView lv = new ListView();
            lv.RowHeight = 50;
            lv.HorizontalOptions = LayoutOptions.FillAndExpand;
            lv.VerticalOptions = LayoutOptions.FillAndExpand;
            lv.SetBinding(ListView.ItemsSourceProperty, new Binding("GroupedItems"));
            lv.IsGroupingEnabled = true;
            lv.GroupDisplayBinding = new Binding("Name");
            lv.GroupShortNameBinding = new Binding("ShortName");
            lv.ItemSelected += (sender, e) =>
            {
                if (e.SelectedItem != null)
                {
                    Navigation.PushAsync(new LoggListPage((Jakt)e.SelectedItem));
                    ((ListView)sender).SelectedItem = null;
                }
            }; //Remember to remove this event handler on dispoing of the page;
            DataTemplate dt = new DataTemplate(typeof(CircleImageCell));
            dt.SetBinding(CircleImageCell.TextProperty, "Title");
            dt.SetBinding(CircleImageCell.DetailProperty, "Details");
            dt.SetBinding(CircleImageCell.ImageSourceProperty, "Image");

            lv.ItemTemplate = dt;
			lv.IsPullToRefreshEnabled = true;
			lv.Refreshing += async delegate {
                Init();
                lv.EndRefresh();
            };;
            if (VM.GroupedItems.Any())
            {
                if(!ToolbarItems.Any())
                {
                    ToolbarItems.Add(new ToolbarItem("Ny jakt", null, CreateNewItem, ToolbarItemOrder.Primary));
                }

                Content = lv;
            }
            else
            {
                var btn = new PrimaryButton()
                {
                    Text = "Opprett første jakt"
                };
                btn.Clicked += delegate (object sender, EventArgs args) { CreateNewItem(); };

                var myImage = new CircleImage()
                {
                    Source = FileImageSource.FromFile("placeholder_hunt.jpg"),
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    Aspect = Aspect.AspectFill,
                };
                
                Content = new StackLayout()
                {
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    Children =
                    {
                        myImage,
                        btn
                    }
                };
            }
        }
	
    }
}
