using System;
using System.ComponentModel;

namespace Delta
{
    /// <summary>
    /// Defines a recyclable object.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IRecyclable
    {
        /// <summary>
        /// Recycles the object so it may be re-used.
        /// </summary>
        void Recycle();
    }
}
