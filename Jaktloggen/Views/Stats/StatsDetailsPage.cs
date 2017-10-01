using Jaktloggen.Models;
using Jaktloggen.ViewModels.Stats;
using Jaktloggen.Views.Cells;

using Xamarin.Forms;

namespace Jaktloggen.Views.Stats
{
    public class StatsDetailsPage : Base.ContentPageJL
    {
        public StatsDetailsVM VM;
        public StatsDetailsPage(StatItem item)
        {
            BindingContext = VM = new StatsDetailsVM(item);
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
            lv.SetBinding(ListView.ItemsSourceProperty, new Binding("ItemCollection"));
            lv.ItemSelected += (sender, e) =>
            {
                if (e.SelectedItem != null)
                {
                    var statItem = ((StatItem) e.SelectedItem);
                    DisplayAlert(statItem.Title, statItem.Details, "OK");
                    ((ListView)sender).SelectedItem = null;
                }
            };
            DataTemplate dt = new DataTemplate(typeof(CircleImageCell));
            dt.SetBinding(CircleImageCell.TextProperty, "Title");
            dt.SetBinding(CircleImageCell.DetailProperty, "Details");
            lv.ItemTemplate = dt;

            var filterView = new StackLayout()
                             {
                                 Orientation = StackOrientation.Horizontal,
                                 Padding = 5
                             };
            var dateFrom = new DatePicker();
            var dateTo = new DatePicker();
            dateFrom.SetBinding(DatePicker.DateProperty, "DateFrom");
            dateTo.SetBinding(DatePicker.DateProperty, "DateTo");
            dateFrom.DateSelected += OnDateSelected;
            dateTo.DateSelected += OnDateSelected;

            Content = lv;
        }

        private void OnDateSelected(object sender, DateChangedEventArgs dateChangedEventArgs)
        {

        }
    }
}
