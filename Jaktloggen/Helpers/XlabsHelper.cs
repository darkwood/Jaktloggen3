using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Jaktloggen.Interfaces;
using Jaktloggen.IO;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using Xamarin.Forms;

namespace Jaktloggen.Helpers
{
    
    public static class PositionHelper
    {

		public static bool IsLocationAvailable()
		{
			if (!CrossGeolocator.IsSupported)
				return false;

			return CrossGeolocator.Current.IsGeolocationAvailable;
		}

		public static async Task<Position> GetCurrentLocation()
		{
			Position position = null;
			try
			{
				var locator = CrossGeolocator.Current;
				locator.DesiredAccuracy = 1;

				position = await locator.GetLastKnownLocationAsync();

				if (position != null)
				{
					//got a cahched position, so let's use it.
                    return position;
				}

				if (!locator.IsGeolocationAvailable || !locator.IsGeolocationEnabled)
				{
					//not available or enabled
					return null;
				}

				position = await locator.GetPositionAsync(TimeSpan.FromSeconds(20), null, true);

			}
			catch (Exception ex)
			{
				//Display error as we have timed out or can't get location.
                Debug.WriteLine(ex.Message);
			}

			if (position == null)
				return null;

			var output = string.Format("Time: {0} \nLat: {1} \nLong: {2} \nAltitude: {3} \nAltitude Accuracy: {4} \nAccuracy: {5} \nHeading: {6} \nSpeed: {7}",
				position.Timestamp, position.Latitude, position.Longitude,
				position.Altitude, position.AltitudeAccuracy, position.Accuracy, position.Heading, position.Speed);

			Debug.WriteLine(output);
            return null;
		}


        public static async Task<Address> GetLocationNameForPosition(Position position)
        {
            if(!IsLocationAvailable()){
                return null;
            }

			var possibleAddresses = await CrossGeolocator.Current.GetAddressesForPositionAsync(position);

			if (possibleAddresses.Any())
			{
				var address = possibleAddresses.First();
				return address;
			}
            return null;
        }
    }

    public class MediaFile
    {
        public MediaFile(string path)
        {
            Path = path;
        }
        public string Path { get; set; }
    }
}
