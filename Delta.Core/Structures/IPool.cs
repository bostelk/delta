using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Delta.Structures
{
    public interface IPool<T>
    {
    
        /// <summary>
        /// Find an unused instance of the type T so it may be put to use.
        /// </summary>
        /// <returns>a fresh new T</returns>
        T Fetch();

        /// <summary>
        /// Return the object to the pool for use at another time.
        /// </summary>
        /// <param name="obj"></param>
        void Release(T obj);
    }
}
