using System.Diagnostics;
using Xamarin.Forms;

namespace Jaktloggen.Views.Cells
{
    public class CircleImageCell : ViewCell
    {
        private Label TitleLabel = new Label();
        private Label DetailsLabel = new Label();
        private Image CircleImage;
        private Image SecondaryImage { get; set; }
        public static readonly BindableProperty TextProperty =
        BindableProperty.Create("Text", typeof(string), typeof(CircleImageCell), "");
        public static readonly BindableProperty DetailProperty =
       BindableProperty.Create("Detail", typeof(string), typeof(CircleImageCell), "");
        public static readonly BindableProperty ImageSourceProperty =
        BindableProperty.Create("ImageSource", typeof(ImageSource), typeof(CircleImageCell));
        public static readonly BindableProperty SecondaryImageSourceProperty =
        BindableProperty.Create("SecondaryImageSource", typeof(ImageSource), typeof(CircleImageCell));

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }
        public string Detail
        {
            get { return (string)GetValue(DetailProperty); }
            set { SetValue(DetailProperty, value); }
        }
        public ImageSource ImageSource
        {
            get { return (ImageSource)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }
        public ImageSource SecondaryImageSource
        {
            get { return (ImageSource)GetValue(SecondaryImageSourceProperty); }
            set { SetValue(SecondaryImageSourceProperty, value); }
        }
        public CircleImageCell()
        {
            CircleImage = new Image()
            {

                HorizontalOptions = LayoutOptions.Start,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                //BorderThickness = 1,
                //BorderColor = Color.White,
                HeightRequest = 40,
                WidthRequest = 40,
                Aspect = Aspect.AspectFill
            };
            SecondaryImage = new Image()
            {

                HorizontalOptions = LayoutOptions.End,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HeightRequest = 40,
                WidthRequest = 40,
                Aspect = Aspect.AspectFill
            };


            TitleLabel.VerticalOptions = LayoutOptions.CenterAndExpand;
            TitleLabel.HorizontalOptions = LayoutOptions.StartAndExpand;

            DetailsLabel.FontSize = 10;
            DetailsLabel.HorizontalOptions = LayoutOptions.EndAndExpand;
            DetailsLabel.VerticalOptions = LayoutOptions.CenterAndExpand;
            

            var stackLayout = new StackLayout()
            {
                Orientation = StackOrientation.Horizontal,
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Padding = 10,
            };
            
            stackLayout.Children.Add(CircleImage);
            stackLayout.Children.Add(TitleLabel);
            stackLayout.Children.Add(DetailsLabel);
			//stackLayout.Children.Add(SecondaryImage);


            View = stackLayout;
        }
        protected override void OnAppearing()
        {

			var deleteAction = new MenuItem { Text = "Delete", IsDestructive = true }; // red background
			deleteAction.SetBinding(MenuItem.CommandParameterProperty, new Binding("."));
			deleteAction.Clicked += async (sender, e) => {
				var mi = ((MenuItem)sender);
                MessagingCenter.Send<Models.EntityBase>(mi.CommandParameter as Models.EntityBase, "DeleteEntity");
			};
			ContextActions.Add(deleteAction);

            base.OnAppearing();

        }
        

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (BindingContext != null)
            {
                TitleLabel.Text = Text;
                DetailsLabel.Text = Detail;
                CircleImage.Source = ImageSource;
                CircleImage.IsVisible = ImageSource != null;
                //SecondaryImage.Source = SecondaryImageSource;
            }
        }
    }
}
