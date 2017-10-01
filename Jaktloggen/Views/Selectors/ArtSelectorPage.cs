using System;

using Jaktloggen.Models;
using Jaktloggen.ViewModels;

using Xamarin.Forms;

namespace Jaktloggen.Views
{
    public class ArtSelectorPage : Base.ContentPageJL
    {
        private ArtSelectorVM VM;
        public ArtSelectorPage(Logg currentLogg)
        {
            BindingContext = VM = new ArtSelectorVM(currentLogg);
            ToolbarItems.Add(new ToolbarItem("+", "add.png", () =>
            {
                Navigation.PushAsync(new ArtPage(new Art()), true);
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
            lv.SetBinding(ListView.ItemsSourceProperty, new Binding("GroupedItems"));
            lv.IsGroupingEnabled = true;
            lv.GroupDisplayBinding = new Binding("Name");
            lv.GroupShortNameBinding = new Binding("ShortName");
            lv.ItemSelected += (sender, e) =>
            {
                if (e.SelectedItem != null)
                {
                    var selectedArt = ((Art)e.SelectedItem);

                    VM.SetArt(selectedArt);

                    Navigation.PopAsync(true);
                    ((ListView)sender).SelectedItem = null;
                }
            };
            DataTemplate dt = new DataTemplate(typeof(ImageCell));
            dt.SetBinding(ImageCell.TextProperty, "Navn");
            dt.SetBinding(ImageCell.ImageSourceProperty, "IconSource");
            lv.ItemTemplate = dt;


            if (VM.CurrentLogg.ArtId > 0)
            {
                var btnClear = new Button();
                btnClear.Text = "Fjern valg";
                btnClear.Clicked += delegate(object sender, EventArgs args)
                {
                    VM.RemoveArt();
                    Navigation.PopAsync(true);  
                };
                Content = new StackLayout()
                {
                    Children =
                    {
                        btnClear,
                        lv
                    }
                };
            }
            else
            {
                Content = lv;
            }
            
        }
    }
}
