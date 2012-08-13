using System;
using System.Collections.Generic;

namespace Delta
{

    public static class Pool
    {
        public static Dictionary<Type, InternalPool> _pools = new Dictionary<Type, InternalPool>();

        internal static object Fetch(Type type)
        {
            object obj = null;
            if (!_pools.ContainsKey(type))
                _pools.Add(type, new InternalPool(type, 50));
            obj = _pools[type].Fetch();
            return obj;
        }

        public static T Acquire<T>() where T : IPoolable
        {
            return (T)Fetch(typeof(T));
        }

        public static bool Release<T>(this T obj) where T : IPoolable
        {
            Type type = obj.GetType();
            if (!_pools.ContainsKey(type))
                return false;
            _pools[type].Release(obj);
            return true;
        }

    }

    public class InternalPool
    {
        Type _type = null;
        Stack<object> _items = new Stack<object>();
        int _size = 0;
        int _objectsInUse = 0;
       
        internal InternalPool(Type type, int initialSize)
        {
            _type = type;
            _size = initialSize;
            for (int i = 0; i < _size; i++)
                _items.Push(Activator.CreateInstance(_type, true));
        }

        internal object Fetch()
        {
            object newObj = null;
            if (_items.Count == 0)
                for (int i = 0; i < 100; i++)
                    _items.Push(Activator.CreateInstance(_type, true));
            newObj = _items.Pop();
            _objectsInUse++;
            return newObj;
        }

        internal void Release(object obj)
        {
            _items.Push(obj);
            _objectsInUse--;
        }

        public override string ToString()
        {
            return string.Format("Available Objects: {0}, Used Objects: {1}", _items.Count, _objectsInUse);
        }
    }
}
