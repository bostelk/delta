using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Delta.Input
{
    public sealed class PlayerInputCollection : IList<PlayerInput>
    {
        PlayerInput[] _input;

        public int Count { get { return _input.Length; } }
        public PlayerInput this[int index] { get { return _input[index]; } }
        public PlayerInput this[PlayerIndex index] { get { return _input[(int)index]; } }

        internal PlayerInputCollection(PlayerInput[] input)
        {
            _input = input;
        }

        int IList<PlayerInput>.IndexOf(PlayerInput item)
        {
            return item._index;
        }

        void IList<PlayerInput>.Insert(int index, PlayerInput item)
        {
            throw new NotSupportedException();
        }

        void IList<PlayerInput>.RemoveAt(int index)
        {
            throw new NotSupportedException();
        }

        PlayerInput IList<PlayerInput>.this[int index]
        {
            get { return _input[index]; }
            set { throw new NotSupportedException(); }
        }

        void ICollection<PlayerInput>.Add(PlayerInput item)
        {
            throw new NotSupportedException();
        }

        void ICollection<PlayerInput>.Clear()
        {
            throw new NotSupportedException();
        }

        bool ICollection<PlayerInput>.Contains(PlayerInput item)
        {
            throw new NotSupportedException();
        }

        void ICollection<PlayerInput>.CopyTo(PlayerInput[] array, int arrayIndex)
        {
            throw new NotSupportedException();
        }

        public bool IsReadOnly
        {
            get { return true; }
        }

        bool ICollection<PlayerInput>.Remove(PlayerInput item)
        {
            throw new NotSupportedException();
        }

        public IEnumerator<PlayerInput> GetEnumerator()
        {
            return ((IEnumerable<PlayerInput>)_input).GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _input.GetEnumerator();
        }
    }
}
