using System;

namespace Jaktloggen.Models
{
    public interface IPosition
    {
        int ID { get; set; }
        string Latitude { get; set; }
        string Longitude { get; set; }
    }
}