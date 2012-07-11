using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Delta.Structures
{
    public class Pool<T> : IPool<T> where T:IRecyclable, new()
    {
        static List<Pool<T>> _pools;

        const int DEFAULT_SIZE = 50;
        const int INCREMENT_AMOUNT = 100;

        Stack<T> _items;
        int _initialSize;
        int _objectsInUse;

        static Pool() {
            _pools = new List<Pool<T>>();
        }
        
        public Pool() : this(DEFAULT_SIZE) { }

        public Pool(int size)
        {
            _pools.Add(this);
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
            _objectsInUse++;
            return newobj;
        }

        public void Release(T obj)
        {
            _items.Push(obj);
            _objectsInUse--;
        }

        public void Clear()
        {
        }

        public override string ToString()
        {
            return String.Format("Objects Available: {0} / Objects In Use: {1}", _items.Count, _objectsInUse);
        }

        public static string PerformanceInfo
        {
            get
            {
                if (_pools.Count == 0)
                    return "No pools available.";
                string result = string.Empty;
                foreach (Pool<T> pool in _pools)
                    result += pool.ToString() + " / ";
                return result;
            }
        }
    }
}
