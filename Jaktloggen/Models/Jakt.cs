using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml.Serialization;
using Newtonsoft.Json;

using Xamarin.Forms;

namespace Jaktloggen.Models
{
    using System.Linq;

    
    public class Jakt : EntityBase, IPosition
    {
        public string Sted { get; set; }
        public DateTime DatoFra { get; set; }
        public DateTime DatoTil  { get; set; }
        public List<int> JegerIds { get; set; }
        public List<int> DogIds { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string ImagePath { get; set; }
        public string Notes { get; set; }
        

        public Jakt()
        {
            JegerIds = new List<int>();
            DogIds = new List<int>();
            DatoFra = DateTime.Now.Date;
            DatoTil = DateTime.Now.Date;
        }
            

        [XmlIgnore] [JsonIgnore]
        public string DatoFraTil {
            get
            {
                if (DatoFra.Date == DatoTil.Date)
                {
                    return DatoFra.ToString("dd MMMM", new CultureInfo("nb-NO"));
                }

                return DatoFra.ToString("dd", new CultureInfo("nb-NO")) + " - " + DatoTil.ToString("dd MMMM", new CultureInfo("nb-NO"));
            }
        }
        [XmlIgnore] [JsonIgnore]
        public string Title
        {
            get
            {
                return string.IsNullOrWhiteSpace(Sted) ? GetJaktFromSeason() : Sted;

            }
        }

        private string GetJaktFromSeason()
        {
            var m = DatoFra.Month;
            if (m == 9 || m == 10) { return "Høstjakt"; }
            if (m == 11 || m == 12 || m == 1 || m == 2) { return "Vinterjakt"; }
            if (m == 3 || m == 4 || m == 5) { return "Vårjakt"; }
            return "Sommerjakt";
        }

        [XmlIgnore] [JsonIgnore]
        public string Details
        {
            get { return DatoFraTil; }
        }

        [XmlIgnore] [JsonIgnore]
        public string Position
        {
            get { return string.IsNullOrWhiteSpace(Latitude) ? "Ikke satt" : "Vis posisjon"; }
        }
        
        [XmlIgnore] [JsonIgnore]
        public ImageSource Image
        {
            get
            {
                return ImageSource.FromFile(string.IsNullOrWhiteSpace(ImagePath) ? "placeholder_hunt.jpg" : ImagePath);
            }
        }

        public string JegereInJakt
        {
            get
            {
                if (JegerIds.Any())
                {
                    var jegere = App.Database.GetJegere().Where(j => JegerIds.Contains(j.ID));
                    return string.Join(", ", jegere.Select(s => s.Fornavn));
                }
                return string.Empty;
            }
        }

        public string DogsInJakt
        {
            get
            {
                if (DogIds.Any())
                {
                    var dogs = App.Database.GetDogs().Where(j => DogIds.Contains(j.ID));
                    return string.Join(", ", dogs.Select(s => s.Navn));
                }
                return string.Empty;
            }
        }
    }
}
