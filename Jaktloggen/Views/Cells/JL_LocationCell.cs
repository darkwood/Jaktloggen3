using System;
using Xamarin.Forms;

namespace Jaktloggen.Views.Cells
{
    public class JL_LocationCell : ViewCell
    {
        public Label StatusLabel { get; private set; }
        public ActivityIndicator ActivityIndicator{ get; set; }
        public Image PinImage { get; private set; }

        public JL_LocationCell(string label, EventHandler onTapped = null)
        {
            StatusLabel = new Label()
            {
                Text = "",
                HorizontalOptions = LayoutOptions.End,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };

            ActivityIndicator = new ActivityIndicator()
            {
                IsRunning = false,
                HorizontalOptions = LayoutOptions.End
            };

            PinImage = new Image()
            {
                Source = ImageSource.FromFile("icons/darkpin.png"),
                IsVisible = false
            };

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
						LineBreakMode = LineBreakMode.NoWrap
					},
					StatusLabel,
					ActivityIndicator,
                }
            };
            if (onTapped != null)
            {
                Tapped += onTapped;
            }
        }

		public void StartLoadingPosition(string statusText)
		{
			StatusLabel.Text = statusText;
            ActivityIndicator.IsRunning = true;
            ActivityIndicator.IsVisible = true;
            PinImage.IsVisible = false;
		}

		public void LoadingPositionCompleted(string statusText, bool success)
		{
			StatusLabel.Text = statusText;
            ActivityIndicator.IsRunning = false;
            ActivityIndicator.IsVisible = false;
            PinImage.IsVisible = success;
		}
    }
}
