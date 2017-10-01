using System;

using Xamarin.Forms;

namespace Jaktloggen.Views.Cells
{
    public class JL_ButtonCell : ViewCell
    {
        public JL_ButtonCell(string text, EventHandler onTapped = null)
        {
            var btn = new Button()
            {
                Text = text
            };
            
            btn.Clicked += onTapped;

            View = btn;

        }
    }
}
