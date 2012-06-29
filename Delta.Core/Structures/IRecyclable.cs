using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Delta.Structures
{
    public interface IRecyclable
    {

        /// <summary>
        /// Scrub the instance clean so it may be reused. ie. Clear any state it holds.
        /// </summary>
        void Recycle();
    }
}
