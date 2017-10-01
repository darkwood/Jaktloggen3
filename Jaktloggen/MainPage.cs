using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using Jaktloggen.Views;
using Xamarin.Forms;

namespace Jaktloggen
{
    public class MainPage : TabbedPage
    {
        public MainPage()
        {
            Children.Add(new NavigationPage(new JaktListPage()) { Title = "Jaktloggen", Icon = "tabicons/hunts.png" });
            Children.Add(new NavigationPage(new StatsListPage()) { Title = "Statistikk", Icon = "tabicons/stats.png" });
            Children.Add(new NavigationPage(new LoggTypeListPage()) { Title = "Flere felter", Icon = "tabicons/fields.png" });
            Children.Add(new NavigationPage(new ArtListPage()) { Title = "Arter", Icon = "tabicons/species.png" });
            Children.Add(new NavigationPage(new JegerListPage()) { Title = "Jegere", Icon = "tabicons/hunters.png" });
            Children.Add(new NavigationPage(new DogListPage()) { Title = "Hunder", Icon = "tabicons/dog.png" });
            Children.Add(new NavigationPage(new SettingsPage()) { Title = "Innstillinger" });
        }
    }
}
