using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Jaktloggen.Views
{
    public class SingleItemPicker : Base.ContentPageJL
    {
        public List<string> ItemCollection;
        private Action<SingleItemPicker> _callback;
        public string SelectedItem = string.Empty;
        public SingleItemPicker(string title, List<string> itemCollection, Action<SingleItemPicker> callback )
        {
            Title = title;
            BindingContext = this;
            ItemCollection = itemCollection;
            _callback = callback;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Init();
        }

        public void Init()
        {
            ListView lv = new ListView();
            lv.HorizontalOptions = LayoutOptions.FillAndExpand;
            lv.VerticalOptions = LayoutOptions.FillAndExpand;
            lv.ItemsSource = ItemCollection;
            lv.ItemSelected += (sender, e) =>
            {
                if (e.SelectedItem != null)
                {
                    SelectedItem = e.SelectedItem as string;
                    _callback(this);
                    ((ListView)sender).SelectedItem = null;
                    Navigation.PopAsync();
                }
            }; //Remember to remove this event handler on dispoing of the page;
            Content = lv;
        }
    }
}
