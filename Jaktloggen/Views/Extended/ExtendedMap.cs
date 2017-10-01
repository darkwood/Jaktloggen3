using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.Maps;

namespace Jaktloggen.Views.Extended
{
    public class ExtendedMap : Map
    {
        public event EventHandler<TapEventArgs> Tap;
        public ExtendedMap()
        {

        }

        public ExtendedMap(MapSpan region) : base(region)
        {

        }

        public void OnTap(Position coordinate)
        {
            OnTap(new TapEventArgs { Position = coordinate });
        }

        protected virtual void OnTap(TapEventArgs e)
        {
            var handler = Tap;
            if (handler != null) handler(this, e);
        }
    }

    public class TapEventArgs : EventArgs
    {
        public Position Position { get; set; }
    }
}
