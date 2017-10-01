using System;

namespace Jaktloggen.Models
{
    public interface IEntity
    {
        int ID { get; set; }
        DateTime Created { get; set; }
        DateTime Changed { get; set; }
        string UserId { get; set; }
    }
}