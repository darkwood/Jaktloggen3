using System;
using System.Collections.Generic;
using System.Linq;

using Jaktloggen.Models;
using Jaktloggen.ViewModels;

using Xamarin.Forms;

namespace Jaktloggen.Views
{
    public class JegerSelectorPage : Base.ContentPageJL
    {
        private JegerSelectorVM VM;

        public JegerSelectorPage(int jaktId, List<int> jegerIds, Logg currentLogg = null)
        {
            BindingContext = VM = new JegerSelectorVM(jaktId, jegerIds, currentLogg);
            ToolbarItems.Add(new ToolbarItem("+", "add.png", () =>
            {
                Navigation.PushAsync(new JegerPage(new Jeger()), true);
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
            if (VM.Jegere.Any())
            {
                if (VM.CurrentLogg != null && VM.CurrentLogg.JegerId > 0)
                {
                    var btnClear = new Button();
                    btnClear.Text = "Fjern valgt jeger";
                    btnClear.Clicked += delegate (object sender, EventArgs args)
                    {
                        VM.RemoveJeger(VM.CurrentLogg.Jeger);
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
                    Text = "Opprett første jeger",
                };
                btn.Clicked += delegate (object sender, EventArgs args)
                {
                    Navigation.PushAsync(new JegerPage(new Jeger()), true);
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
                var selectedJeger = ((Jeger)e.SelectedItem);

                if (VM.CurrentLogg == null) // multiple picker
                {
                    VM.UpdateJegerIds(selectedJeger);
                    VM.BindData();
                }
                else
                {
                    VM.AddJeger(selectedJeger);
                    await Navigation.PopAsync(true);
                }

                
                ((ListView)sender).SelectedItem = null;
            }
        }
    }
}
