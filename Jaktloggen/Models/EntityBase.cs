using System;

namespace Jaktloggen.Models
{
    public class EntityBase : IEntity
    {
        public int ID { get; set; }
        public DateTime Created { get; set; } = DateTime.MinValue;
        public DateTime Changed { get; set; } = DateTime.Now;
        public string UserId { get; set; }
    }
}