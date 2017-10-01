using System.Collections.Generic;
using System.Threading.Tasks;
using Jaktloggen.Helpers;
using Jaktloggen.Models;

using MvvmHelpers;

namespace Jaktloggen.ViewModels
{
    public class LoggVM :ObservableObject
    {
        public Logg CurrentLogg { get; set; }
        public IEnumerable<Logg> AllLogs { get; private set; }
        public bool IsNew { get; set; }
        public bool IsLoadingPosition { get; set; }
        public LoggVM(Logg currentLogg)
        {
            if (currentLogg.ID == 0)
            {
                IsNew = true;
                currentLogg.ID = App.Database.SaveLogg(currentLogg);
            }
            CurrentLogg = currentLogg;
        }

        public async void BindData()
        {
            AllLogs = App.Database.GetLoggs();
            CurrentLogg = App.Database.GetLogg(CurrentLogg.ID);
            if (IsNew)
            {
                //await TryGetPosition();
            }
        }

        private async Task TryGetPosition()
        {
            ToggleLoadPosition();

            var position = await PositionHelper.GetCurrentLocation();
            if (position != null)
            {
                CurrentLogg.Latitude = position.Latitude.ToString();
                CurrentLogg.Longitude = position.Longitude.ToString();

                Save();
            }

            ToggleLoadPosition();
        }

        private void ToggleLoadPosition()
        {
            IsLoadingPosition = !IsLoadingPosition;
        }

        public void Save()
        {
            App.Database.SaveLogg(CurrentLogg);
        }
        public void Delete()
        {
            App.Database.DeleteLog(CurrentLogg);
        }
    }
}
