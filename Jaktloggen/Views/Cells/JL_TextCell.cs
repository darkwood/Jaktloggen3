using System;

using Xamarin.Forms;

namespace Jaktloggen.Views.Cells
{
    public class JL_TextCell : ViewCell
    {
        string text;

        public string Text
        {
            get
            {
                return text;
            }

            set
            {
                text = value;
                _valueLabel.Text = text;
            }
        }

        private Label _valueLabel;

        public JL_TextCell(string label, string text, EventHandler onTapped = null)
        {
			_valueLabel = new Label()
			{
				HorizontalOptions = LayoutOptions.End,
				VerticalOptions = LayoutOptions.CenterAndExpand
			};

            Text = text;

            View = new StackLayout()
            {
                Orientation = StackOrientation.Horizontal,
                Padding = 10,
                Children =
                {
                    new Label()
                    {
                        Text = label,
                        VerticalOptions = LayoutOptions.CenterAndExpand,
                        HorizontalOptions = LayoutOptions.StartAndExpand,
                        LineBreakMode = LineBreakMode.NoWrap,
                    },
                    _valueLabel
                }
            };
            if (onTapped != null)
            {
                Tapped += onTapped;
            }
        }
    }
}
