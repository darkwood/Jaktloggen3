using System;

using Jaktloggen.Views.Extended;

using Xamarin.Forms;

namespace Jaktloggen.Views.Cells
{
    public class JL_ImageCell : ViewCell
    {
        public JL_ImageCell(string text, ImageSource image, EventHandler onTapped = null)
        {   
            View = new StackLayout()
            {
                Orientation = StackOrientation.Horizontal,
                Padding = 10,
                HeightRequest = 40,
                Children =
                {
                    new Label()
                    {
                        Text = text,
                        VerticalOptions = LayoutOptions.CenterAndExpand,
                        HorizontalOptions = LayoutOptions.StartAndExpand
                    },
                    new CircleImage()
                    {
                        Source = image,
                        HorizontalOptions = LayoutOptions.End,
                        VerticalOptions = LayoutOptions.CenterAndExpand,
                        BorderThickness = 1,
                        BorderColor = Color.White,
                        HeightRequest = 40,
                        WidthRequest = 40,
                        Aspect = Aspect.AspectFill
                    }
                }
            };

            if (onTapped != null)
            {
                Tapped += onTapped;
            }
        }
    }
}
