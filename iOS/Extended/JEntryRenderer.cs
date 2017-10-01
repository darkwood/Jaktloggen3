using Jaktloggen.iOS.Extended;
using Jaktloggen.Views.Extended;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(JEntry), typeof(JEntryRenderer))]
namespace Jaktloggen.iOS.Extended
{
    using System;

    using ObjCRuntime;

    using UIKit;

    public class JEntryRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.EditingDidBegin += (object sender, EventArgs eIos) => {
                    Control.PerformSelector(new Selector("selectAll"), null, 0.0f);
                };
            }
        }
    }
}