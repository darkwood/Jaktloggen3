using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;

using Xamarin.Forms;

namespace Jaktloggen.Models
{
    
    public class StatItem
    {
        private string _details;
        public string Title { get; set; }

        public string Details
        {
            get
            {
                if (Count > 0)
                {
                    return Count.ToString();
                }
                return _details;
            }
            set { _details = value; }
        }

        public double Count { get; set; }
        public string ImagePath { get; set; }
        public ImageSource Image
        {
            get
            {
                return ImageSource.FromFile(string.IsNullOrWhiteSpace(ImagePath) ? "linechart.png" : ImagePath);
            }

        }
        public List<StatItem> Items { get; set; }
    }
}
