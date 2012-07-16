using System.Collections.Generic;
using System.Collections;
 
namespace Delta
{
	/// HashSet for Xbox360.
	///
	/// NOTE:
	/// This HastSet was designed to minimize block allocations.  I could have
	/// implemented it more like a true Hash; allocating an array of linked lists
	/// but this felt cleaner.  I don't know the internals of Dictionary but I'm
	/// guessing it's similar to a heap or std::map in c++, which is generally
	/// implemented as a binary search tree.
	public class HashSet<T> : IEnumerable where T : class
	{
		Dictionary<T, bool> _data = new Dictionary<T, bool>();
 
		public HashSet()
            : base()
		{
		}
 
		public void Add(T item)
		{
			if (!Contains(item))
				_data.Add(item, true);
		}
 
		public void Clear()
		{
			_data.Clear();
		}
 
		public int Count { get { return _data.Count; } }
 
		public bool Contains(T item)
		{
			return _data.ContainsKey(item);
		}
 
		public IEnumerator GetEnumerator()
		{
			return _data.Keys.GetEnumerator();
		}
 
		IEnumerator IEnumerable.GetEnumerator()
		{
			return _data.Keys.GetEnumerator(); 
		}
	}
}