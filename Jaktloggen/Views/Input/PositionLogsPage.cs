using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using Jaktloggen.Helpers;
using Jaktloggen.Models;
using Jaktloggen.Views.Extended;
using MvvmHelpers;

using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace Jaktloggen.Views.Input
{
    public class PositionLogsPage : Base.ContentPageJL
    {
        public ExtendedMap CurrentMap { get; set; }
        public IEnumerable<Logg> LoggCollection { get; set; }
        public PositionLogsPage(IEnumerable<Logg> itemCollection)
        {
            LoggCollection = itemCollection;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            InitMap();
        }

        private void InitMap()
        {
            CurrentMap = new ExtendedMap()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand
            };
            CurrentMap.IsShowingUser = true;

            SetPinsAtPositions();
            
            Content = CurrentMap;
        }

        private void SetPinsAtPositions()
        {
            if (CurrentMap.Pins.Count > 0)
            {
                CurrentMap.Pins.Clear();
            }
            foreach (var logg in LoggCollection)
            {
                var pin = CreatePin(logg);
                if (pin != null)
                {
                    CurrentMap.Pins.Add(pin);
                }
                
            }

            SetMapCenterAndRadius();
        }

        private void SetMapCenterAndRadius()
        {
            var latitudes = new List<double>();
            var longitudes = new List<double>();
            
            if (CurrentMap.Pins != null)
            {
                foreach (Pin aPin in CurrentMap.Pins)
                {
                    latitudes.Add(aPin.Position.Latitude);
                    longitudes.Add(aPin.Position.Longitude);
                }
            }

            if (latitudes.Any() && longitudes.Any())
            {
                double lowestLat = latitudes.Min();
                double highestLat = latitudes.Max();
                double lowestLong = longitudes.Min();
                double highestLong = longitudes.Max();
                double finalLat = (lowestLat + highestLat) / 2;
                double finalLong = (lowestLong + highestLong) / 2;
                double distance = MapHelper.GeoCodeCalc.CalcDistance(lowestLat, lowestLong, highestLat, highestLong, MapHelper.GeoCodeCalcMeasurement.Kilometers);

                CurrentMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(finalLat, finalLong), Distance.FromKilometers(distance)));
            }
        }

        private Pin CreatePin(Logg logg)
        {
            double lat, lon;
            if (double.TryParse(logg.Latitude, out lat) && double.TryParse(logg.Longitude, out lon))
            {
                var position = new Position(lat, lon);
                return new Pin
                {
                    Type = PinType.Generic,
                    Position = position,
                    Label = logg.FangstText
                };
            }
            return null;
        }
    }
}
