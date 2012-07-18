using System;

namespace Delta
{
    public interface IEntityManager
    {
        DeltaTime Time { get; }
        Camera Camera { get; }
    }
}
