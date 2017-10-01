using Jaktloggen.Models;
using Jaktloggen.ViewModels;
using Jaktloggen.Views.Cells;
using Jaktloggen.Views.Stats;

using Xamarin.Forms;

namespace Jaktloggen.Views
{
    public class StatsListPage : Base.ContentPageJL
    {
        private StatsListVM VM;
        public StatsListPage()
        {
            Title = "Statistikk";
            BindingContext = VM = new StatsListVM();
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
                    var statItem = (StatItem) e.SelectedItem;
                    if (statItem.Items != null)
                    {
                        Navigation.PushAsync(new StatsDetailsPage(statItem), true);
                    }

                    ((ListView)sender).SelectedItem = null;
                }
            }; //Remember to remove this event handler on dispoing of the page;
            DataTemplate dt = new DataTemplate(typeof(CircleImageCell));
            dt.SetBinding(CircleImageCell.TextProperty, "Title");
            dt.SetBinding(CircleImageCell.DetailProperty, "Details");
            dt.SetBinding(CircleImageCell.ImageSourceProperty, "Image");
            lv.ItemTemplate = dt;
            Content = lv;
        }
    }
}
