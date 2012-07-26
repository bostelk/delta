using System;
using System.ComponentModel;

namespace Delta
{
    /// <summary>
    /// Defines an importable object.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public interface IImportable
    {
        /// <summary>
        /// Sets a field's value by it's name.
        /// </summary>
        /// <param name="name">Value name.</param>
        /// <param name="value">Value.</param>
        /// <returns>A value indicating whether the field exists and that it's value was sucessfully set.</returns>
        bool SetField(string name, string value);
    }
}
