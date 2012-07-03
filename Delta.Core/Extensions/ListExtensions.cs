using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Delta
{
    public static class ListExtensions
    {
        /// <summary>
        /// Quickly removes the element by swapping it with the last element and removing the last element.
        /// This avoids memory copying...
        /// </summary>
        public static void FastRemove<T>(this List<T> list, T item)
        {
            int index = list.IndexOf(item);
            if (index >= 0)
            {
                int lastIndex = list.Count - 1;
                list[index] = list[lastIndex];
                list.RemoveAt(lastIndex);
            }
        }
    }
}
