using System;

namespace Delta
{
    public interface IEntity : IRecyclable, IGameComponent
    {
        string Name { get; set; }
    }
}
