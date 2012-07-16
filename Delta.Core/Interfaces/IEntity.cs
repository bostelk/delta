using System;

namespace Delta
{
    public interface IEntity : IRecyclable, IGameComponent
    {
        string ID { get; set; }
    }
}
