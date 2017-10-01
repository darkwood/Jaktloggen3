using Jaktloggen.Models;
using Jaktloggen.ViewModels;

using Xamarin.Forms;

namespace Jaktloggen.Views
{
    public class LoggTypeListPage : Base.ContentPageJL
    {
        private LoggTypeListVM VM;
        public LoggTypeListPage()
        {
            BindingContext = VM = new LoggTypeListVM();
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
                    var loggType = (LoggType)e.SelectedItem;
                    VM.LoggTypeSelected(loggType);
                    ((ListView)sender).SelectedItem = null;
                }
            }; //Remember to remove this event handler on dispoing of the page;
            DataTemplate dt = new DataTemplate(typeof(ImageCell));
            dt.SetBinding(ImageCell.TextProperty, "Navn");
            dt.SetBinding(ImageCell.ImageSourceProperty, "IconSource");
            lv.ItemTemplate = dt;
            Content = lv;
        }
    }
}
