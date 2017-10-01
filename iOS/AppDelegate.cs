using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;
using Xamarin;

namespace Jaktloggen.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();
            FormsMaps.Init();
            LoadApplication(new App());

            UITabBar.Appearance.SelectedImageTintColor = UIColor.FromRGB(64, 97, 62);

            return base.FinishedLaunching(app, options);
        }
    }
}
    