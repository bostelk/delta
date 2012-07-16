using System;

namespace Delta
{
    public interface IGameComponentCollection
    {
        bool NeedsToSort { get; set; }

        void Add(IGameComponent item);
        void Remove(IGameComponent item);
    }
}
