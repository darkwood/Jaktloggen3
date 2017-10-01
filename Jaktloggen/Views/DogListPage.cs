using System.Linq;

using Jaktloggen.Models;
using Jaktloggen.ViewModels;
using Jaktloggen.Views.Cells;
using Jaktloggen.Views.Extended;

using Xamarin.Forms;

namespace Jaktloggen.Views
{
    public class DogListPage : Base.ContentPageJL
    {
        private DogListVM VM;
        public DogListPage()
        {
            BindingContext = VM = new DogListVM();
            ToolbarItems.Add(new ToolbarItem("+", "add.png", () =>
            {
                CreateNewItem();
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
                    Navigation.PushAsync(new DogPage((Dog)e.SelectedItem), true);
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
                    Text = "Opprett første hund",
                };
                btn.Clicked += delegate     { CreateNewItem(); };

                var myImage = new CircleImage()
                {
                    Source = FileImageSource.FromFile("placeholder_dog.jpg"),
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

        private void CreateNewItem()
        {
            Navigation.PushAsync(new DogPage(new Dog()), true);
        }
    }
}
