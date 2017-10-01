using System;
using System.Collections.Generic;
using System.Linq;

using Jaktloggen.Models;
using Jaktloggen.ViewModels;

using Xamarin.Forms;

namespace Jaktloggen.Views
{
    public class DogSelectorPage : Base.ContentPageJL
    {
        private DogSelectorVM VM;

        public DogSelectorPage(int jaktId, List<int> dogIds, Logg currentLogg = null)
        {
            BindingContext = VM = new DogSelectorVM(jaktId, dogIds, currentLogg);
            ToolbarItems.Add(new ToolbarItem("+", "add.png", () =>
            {
                Navigation.PushAsync(new DogPage(new Dog()), true);
            }, ToolbarItemOrder.Primary));
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            Init();
        }

        public void Init()
        {
            VM.BindData();

            ListView lv = new ListView(ListViewCachingStrategy.RecycleElement);

            lv.HorizontalOptions = LayoutOptions.FillAndExpand;
            lv.VerticalOptions = LayoutOptions.FillAndExpand;
            lv.SetBinding(ListView.ItemsSourceProperty, new Binding("GroupedItems"));
            lv.IsGroupingEnabled = true;
            lv.GroupDisplayBinding = new Binding("Name");
            lv.GroupShortNameBinding = new Binding("ShortName");
            lv.ItemSelected += OnLvOnItemSelected;
            DataTemplate dt = new DataTemplate(typeof(ImageCell));
            dt.SetBinding(ImageCell.TextProperty, "Navn");
            dt.SetBinding(ImageCell.ImageSourceProperty, "IconSource");
            lv.ItemTemplate = dt;
            if (VM.Dogs.Any())
            {
                if (VM.CurrentLogg != null && VM.CurrentLogg.DogId > 0)
                {
                    var btnClear = new Button();
                    btnClear.Text = "Fjern valgt hund";
                    btnClear.Clicked += delegate (object sender, EventArgs args)
                    {
                        VM.RemoveDog(VM.CurrentLogg.Dog);
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
            else
            {
                var btn = new Button()
                {
                    Text = "Opprett første dog",
                };
                btn.Clicked += delegate (object sender, EventArgs args)
                {
                    Navigation.PushAsync(new DogPage(new Dog()), true);
                };
                Content = new StackLayout()
                {
                    Children =
                    {
                        btn
                    }
                };
            }
        }

        private async void OnLvOnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                var selectedDog = ((Dog)e.SelectedItem);

                if (VM.CurrentLogg == null) // multiple picker
                {
                    VM.UpdateDogIds(selectedDog);
                    VM.BindData();
                }
                else
                {
                    VM.AddDog(selectedDog);
                    await Navigation.PopAsync(true);
                }


                ((ListView)sender).SelectedItem = null;
            }
        }
    }
}
