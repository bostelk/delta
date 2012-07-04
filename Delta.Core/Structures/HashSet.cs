using System.Collections.Generic;
using System.Collections;
 
namespace Delta.Structures
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
		Dictionary<T, bool> data = new Dictionary<T, bool>();
 
		public HashSet()
		{
		}
 
		public void Add(T t)
		{
			if (Contains(t) == false)
			{
				data.Add(t, true);
			}
		}
 
		public void Clear()
		{
			data.Clear();
		}
 
		public int Count { get { return data.Count; } }
 
		public bool Contains(T t)
		{
			return data.ContainsKey(t);
		}
 
		public IEnumerator GetEnumerator()
		{
			return data.Keys.GetEnumerator();
		}
 
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return data.Keys.GetEnumerator(); 
		}
	}
}