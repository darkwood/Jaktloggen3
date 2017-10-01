using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;
using Newtonsoft.Json;

using Xamarin.Forms;

namespace Jaktloggen.Models
{
    
    public class Art : EntityBase
    {
        public string Navn { get; set; }
        public string Wikinavn { get; set; }
        public int GroupId { get; set; }
        public string ImagePath { get; set; }

        [XmlIgnore, JsonIgnore]
        public bool Selected { get; set; }

        [XmlIgnore, JsonIgnore]
        public ImageSource IconSource {
            get
            {
                var icon = Selected ? "starred.png" : "starred_not.png";
                return ImageSource.FromFile(icon);
            }
        }
        
        [XmlIgnore, JsonIgnore]
        public ImageSource Image
        {
            get
            {
                return ImageSource.FromFile(string.IsNullOrWhiteSpace(ImagePath) ? "placeholder_art.png" : ImagePath);
            }
        }
    }
}
