using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using Jaktloggen.Models;
using Jaktloggen.Views.Extended;
using MvvmHelpers;

using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace Jaktloggen.Views.Input
{
    
    public class PositionPageVM
    {
        public Position Position { get; set; }
        public string Status { get; set; } = "Sett posisjon";

        public string LatitudeString
        {
            get
            {
                return Position.Latitude != 0.0f ? Position.Latitude.ToString() : string.Empty;
            }
        }

        public string LongitudeString
        {
            get
            {
                return Position.Longitude != 0.0f ? Position.Longitude.ToString() : string.Empty;
            }
        }
    }
    public class PositionPage : Base.ContentPageJL
    {
        private Action<PositionPage> _callback;

        private IPosition _page;
        private bool PositionIsSet { get; set; }

        public ExtendedMap CurrentMap { get; set; }
        public PositionPageVM VM { get; set; }
        public PositionPage(IPosition page, Action<PositionPage> callback)
        {
            _page = page;
            BindingContext = VM = new PositionPageVM();

            double lat, lon;
            if (double.TryParse(page.Latitude, out lat) && double.TryParse(page.Longitude, out lon))
            {
                PositionIsSet = true;
                VM.Position = new Position(lat, lon);
            }
            else
            {
                MoveToCurrentPosition();
            }
            _callback = callback;
            InitMap();
        }

        

        private async void MoveToCurrentPosition()
        {
            VM.Status = "Henter GPS-posisjon...";
            var pos = await Helpers.PositionHelper.GetCurrentLocation();
            VM.Position = new Position(pos.Latitude, pos.Longitude);
            VM.Status = "Posisjon funnet";
            SetPinAtPosition();
        }

        private void InitMap()
        {
            
            CurrentMap = new ExtendedMap()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand
            };

            SetPinAtPosition();
            CurrentMap.Tap += MapOnTap;
            CurrentMap.IsShowingUser = true;

            var btnSave = new Button() { Text = "Lagre" };
            btnSave.Clicked += delegate (object sender, EventArgs args)
            {
                _callback(this);
                Navigation.PopModalAsync(true);
            };
            var btnCancel = new Button() { Text = "Avbryt" };
            btnCancel.Clicked += delegate (object sender, EventArgs args)
            {
                Navigation.PopModalAsync(true);
            };

            var btnRemove = new Button() { Text = "Slett" };
            btnRemove.Clicked += delegate (object sender, EventArgs args)
            {
                VM.Position = new Position();
                _callback(this);
                Navigation.PopModalAsync(true);
            };

            var lblStatus = new Label()
            {
                FontSize = 12,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };
            lblStatus.SetBinding(Label.TextProperty, "Status");
            var stackLayout = new StackLayout()
            {
                Children =
                {
                    CurrentMap,
                    new StackLayout()
                    {
                        Orientation = StackOrientation.Horizontal,
                        Children =
                        {
                            btnCancel,
                            btnRemove,
                            lblStatus,
                            btnSave
                        }
                    }
                }
            };

            Content = stackLayout;

        }
        
        private async void MapOnTap(object sender, TapEventArgs tapEventArgs)
        {
            VM.Position = tapEventArgs.Position;
            SetPinAtPosition();
        }

        //todo: make MapOnZoom event and store radius value (distance)

        private void SetPinAtPosition()
        {
            if (CurrentMap.Pins.Count > 0)
            {
                VM.Status = "Ny posisjon satt";
                CurrentMap.Pins.Clear();
            }
            
            CurrentMap.Pins.Add(CreatePin());

            CurrentMap.MoveToRegion(
                MapSpan.FromCenterAndRadius(
                    VM.Position,
                    Distance.FromKilometers(2)
                )
            );
        }

        private Pin CreatePin()
        {
            return new Pin
            {
                Type = PinType.Place,
                Position = VM.Position,
                Label = "Valgt posisjon"
            };
        }
    }
}
