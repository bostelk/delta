using System;

namespace Delta
{
    public interface IEntity
    {
#if WINDOWS
        bool ImportCustomValues(string name, string value);
#endif
    }
}
