using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.iOS;
using Xamarin.Forms.Platform.iOS;
using Jaktloggen.iOS.Extended;
using Jaktloggen.Views.Extended;
using MapKit;

[assembly: ExportRenderer(typeof(ExtendedMap), typeof(ExtendedMapRenderer))]

namespace Jaktloggen.iOS.Extended
{
    public class ExtendedMapRenderer : MapRenderer
    {
        private readonly UITapGestureRecognizer _tapRecogniser;

        public ExtendedMapRenderer()
        {
            _tapRecogniser = new UITapGestureRecognizer(OnTap)
            {
                NumberOfTapsRequired = 1,
                NumberOfTouchesRequired = 1
            };
        }

        private void OnTap(UITapGestureRecognizer recognizer)
        {
            var cgPoint = recognizer.LocationInView(Control);
            var location = ((MKMapView)Control).ConvertPoint(cgPoint, Control);
            ((ExtendedMap)Element).OnTap(new Position(location.Latitude, location.Longitude));
        }

        protected override void OnElementChanged(ElementChangedEventArgs<View> e)
        {
            if (Control != null)
                Control.RemoveGestureRecognizer(_tapRecogniser);
            base.OnElementChanged(e);
            if (Control != null)
                Control.AddGestureRecognizer(_tapRecogniser);
        }
    }
}