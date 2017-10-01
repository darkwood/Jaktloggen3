using System;
using System.Linq;

using Jaktloggen.Models;
using Jaktloggen.ViewModels;
using Jaktloggen.Views.Cells;
using Jaktloggen.Views.Extended;

using Xamarin.Forms;

namespace Jaktloggen.Views
{
    public class JegerListPage : Base.ContentPageJL
    {
        private JegerListVM VM;
        public JegerListPage()
        {
            BindingContext = VM = new JegerListVM();
            ToolbarItems.Add(new ToolbarItem("+", "add.png", () =>
            {
                Navigation.PushAsync(new JegerPage(new Jeger()), true);
            }, ToolbarItemOrder.Primary));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Init();
        }

        public void Init()
        {
            VM.BindData();

            ListView lv = new ListView();
            lv.HorizontalOptions = LayoutOptions.FillAndExpand;
            lv.VerticalOptions = LayoutOptions.FillAndExpand;
            lv.SetBinding(ListView.ItemsSourceProperty, new Binding("ItemCollection"));
            lv.ItemSelected += (sender, e) =>
            {
                if (e.SelectedItem != null)
                {
                    Navigation.PushAsync(new JegerPage((Jeger)e.SelectedItem), true);
                    ((ListView)sender).SelectedItem = null;
                }
            }; //Remember to remove this event handler on dispoing of the page;
            DataTemplate dt = new DataTemplate(typeof(CircleImageCell));
            dt.SetBinding(CircleImageCell.TextProperty, "Navn");
            dt.SetBinding(CircleImageCell.ImageSourceProperty, "Image");
            lv.ItemTemplate = dt;
            if (VM.ItemCollection.Any())
            {
                Content = lv;
            }
            else
            {
                var btn = new Button()
                {
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                    Text = "Opprett første jeger",
                    BackgroundColor = Color.FromHex("#74B058")
                };
                btn.Clicked += delegate(object sender, EventArgs args)
                {
                    Navigation.PushAsync(new JegerPage(new Jeger()), true);
                };
                var myImage = new CircleImage()
                {
                    Source = FileImageSource.FromFile("placeholder_hunter.jpg"),
                    BorderThickness = 2,
                    BorderColor = Color.White,
                    Aspect = Aspect.AspectFill,
                    Margin = 20
                };

                Content = new StackLayout()
                {
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    HorizontalOptions = LayoutOptions.Center,
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
