#if WINDOWS
using System;

namespace Delta
{
    public interface IImportable
    {
        bool ImportCustomValues(string name, string value);
    }
}
#endif
