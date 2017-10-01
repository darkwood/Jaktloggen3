using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

using Xamarin.Forms;

namespace Jaktloggen.Views.Input
{
    public class DatePage : Base.ContentPageJL
    {
        private bool _useTime;
        public DateTime DateFrom;
        public DateTime DateTo;
        public DatePicker datePickerFrom = new DatePicker();
        public DatePicker datePickerTo = new DatePicker();
        public TimePicker timePickerFrom = new TimePicker();
        public TimePicker timePickerTo = new TimePicker();
        private Action<DatePage> _callback;
        public DatePage(string title, DateTime dateFrom, DateTime? dateTo = null, Action<DatePage> callback = null, bool useTime = false)
        {
            Title = title;
            ToolbarItems.Add(new ToolbarItem("Ferdig", null, () =>
            {
                Navigation.PopAsync(true);
            }, ToolbarItemOrder.Default));

            _useTime = useTime;
            _callback = callback;

            DateFrom = dateFrom;

            Init(dateFrom, dateTo);
        }

        private void Init(DateTime dateFrom, DateTime? dateTo)
        {
            var stackLayout = new StackLayout();

            datePickerFrom.Date = dateFrom;
            datePickerFrom.DateSelected += DateFromSelected;
            stackLayout.Children.Add(CreateDateField(dateTo == null ? "Dato" : "Fra dato", datePickerFrom));
            if (_useTime)
            {
                timePickerFrom.Time = dateFrom.TimeOfDay;
                stackLayout.Children.Add(CreateDateField("Tidspunkt", timePickerFrom));
            }

            if (dateTo.HasValue)
            {
                DateTo = dateTo.Value;
                datePickerTo.Date = dateTo.Value;
                datePickerTo.DateSelected += DateToSelected;
                stackLayout.Children.Add(CreateDateField("Til dato", datePickerTo));
                if (_useTime)
                {
                    timePickerTo.Time = dateFrom.TimeOfDay;
                    stackLayout.Children.Add(CreateDateField("Tidspunkt", timePickerTo));
                }
            }

            Content = stackLayout;
        }

        private StackLayout CreateDateField(string label, View datePicker)
        {
            datePicker.HorizontalOptions = LayoutOptions.EndAndExpand;
            return new StackLayout()
            {
                Orientation = StackOrientation.Horizontal,
                Padding = 10,
                Children =
                {
                    new Label { Text = label, VerticalOptions = LayoutOptions.Center},
                    datePicker,
                }
            };
        }

        private void DateFromSelected(object sender, EventArgs eventArgs)
        {
            DateFrom = _useTime ? datePickerFrom.Date + timePickerFrom.Time : datePickerFrom.Date;

            if (DateFrom > DateTo)
            {
                DateTo = DateFrom;
                datePickerTo.Date = DateTo;
            }
            _callback(this);
        }
        private void DateToSelected(object sender, EventArgs eventArgs)
        {
            DateTo = _useTime ? datePickerTo.Date + timePickerTo.Time : datePickerTo.Date;
            
            if (DateTo < DateFrom)
            {
                DateFrom = DateTo;
                datePickerFrom.Date = DateFrom;
            }
            
            _callback(this);
        }
    }
}
