using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;

using Xamarin.Forms;

namespace Jaktloggen.Models
{
    
    public class Jeger : EntityBase
    {
        public string Fornavn { get; set; }
        public string Etternavn { get; set; }
        public string Email { get; set; } 
        public string Phone { get; set; }
        public string ImagePath { get; set; }
        public bool IsMe { get; set; }
        [XmlIgnore, JsonIgnore]
        public bool Selected { get; set; }

        [XmlIgnore] [JsonIgnore]
        public string VisningsNavn
        {
            get
            {
                if (Fornavn == null && Etternavn == null)
                {
                    return "Velg jeger";
                }
                return Fornavn;
            }
        }
        [XmlIgnore] [JsonIgnore]
        public string Navn => Fornavn + " " + Etternavn;

        [XmlIgnore] [JsonIgnore]
        public ImageSource IconSource
        {
            get
            {
                var icon = Selected ? "starred.png" : "starred_not.png";
                return ImageSource.FromFile(icon);
            }
        }

        [XmlIgnore] [JsonIgnore]
        public ImageSource Image
        {
            get
            {
                return ImageSource.FromFile(string.IsNullOrWhiteSpace(ImagePath) ? "placeholder_hunter.jpg" : ImagePath);
            }
            
        }
    }
}
