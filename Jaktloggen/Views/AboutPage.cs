using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using Jaktloggen.Models;
using Jaktloggen.ViewModels;
using Jaktloggen.Views.Base;
using Jaktloggen.Views.Cells;
using Jaktloggen.Views.Input;
using Xamarin.Forms;

namespace Jaktloggen.Views
{
    public class AboutPage : Base.ContentPageJL
    {
        public AboutPage()
        {

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Init();
        }

        private void Init()
        {
            Title = "Om Jaktloggen";
            
            Content = new StackLayout()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Children =
                {
                    new Label()
                    {
                        Text = @"Jaktloggen ønsker å ta vare på gode jaktopplevelser gjennom bilder, statistikk og posisjonering.",
                        Margin = 10,
                    },
                    new Label
                    {
                        Text = "Tilbakemelding",
                        FontSize = 20,
                    },
                    new Editor()
                    {
                        HeightRequest = 100
                    },
                    new Entry()
                    {
                        Placeholder = "Ditt navn"
                    },
                    new Entry()
                    {
                        Placeholder = "Din e-post",
                        Keyboard = Keyboard.Email,
                    },
                }
            };
        }
    }
}
