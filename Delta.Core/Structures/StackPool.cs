using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Delta.Structures
{
    public class StackPool<T> : IPool<T> where T:IRecyclable, new()
    {
        private const int DEFAULT_SIZE = 50;
        private const int INCREMENT_AMOUNT = 100;

        private Stack<T> _items;
        private int _initialSize;

        public StackPool() : this(DEFAULT_SIZE) { }

        public StackPool(int size)
        {
            _initialSize = size;
            _items = new Stack<T>(_initialSize);

            Initialize();
        }

        public void Initialize()
        {
            _items.Clear();

            for (int i = 0; i < _initialSize; i++)
            {
                _items.Push(new T());
            }
        }

        public T Fetch()
        {
            T newobj;
            if (_items.Count == 0)
            {
                for (int i = 0; i < INCREMENT_AMOUNT; i++)
                {
                    _items.Push(new T());
                }
            }
            newobj = _items.Pop();
            return newobj;
        }

        public void Release(T obj)
        {
            _items.Push(obj);
        }

        public override string ToString()
        {
            return "hi";
            //return String.Format("Size: {0} , Used: {1}", _items.ccap, _used);
        }
    }
}
