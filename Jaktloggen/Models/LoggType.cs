using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;

using Xamarin.Forms;

namespace Jaktloggen.Models
{
    
    public class LoggType : EntityBase
    {
        public string Key { get; set; }
        public string Navn { get; set; }
        public string Beskrivelse { get; set; }
        public int GroupId { get; set; }
        [XmlIgnore] [JsonIgnore]
        public bool Selected { get; set; }
        [XmlIgnore] [JsonIgnore]
        public ImageSource IconSource
        {
            get
            {
                var icon = Selected ? "starred.png" : "starred_not.png";
                return ImageSource.FromFile(icon);
            }
        }
    }
}
