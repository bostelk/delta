using System;
using System.Collections.Generic;
using System.Reflection;

namespace Delta
{
    class VisitedGraph : Dictionary<object, object>
    {
        public new bool ContainsKey(object key)
        {
            if (key == null)
                return true;
            return base.ContainsKey(key);
        }

        public new object this[object key]
        {
            get { if (key == null) return null; return base[key]; }
        }
    }

    public static class ObjectExtensions
    {
        /// <summary>
        /// Creates a copy of the object.
        /// </summary>
        /// <param name="instance">The object to be copied.</param>
        /// <returns>A deep copy of the object.</returns>
        public static object Copy(this object instance)
        {
            if (instance == null)
                return null;
            return Copy(instance, DeduceInstance(instance));
        }

        internal static object Copy(this object instance, object copy)
        {
            if (instance == null)
                return null;
            if (copy == null)
                throw new ArgumentNullException("The copy instance cannot be null");
            return Clone(instance, new VisitedGraph(), copy);
        }

        private static object Clone(this object instance, VisitedGraph visited)
        {
            if (instance == null)
                return null;

            Type instanceType = instance.GetType();

            if (instanceType.IsValueType || instanceType == typeof(string))
                return instance; // Value types and strings are immutable
            else if (instanceType.IsArray)
            {
                int length = ((Array)instance).Length;
                Array copied = (Array)Activator.CreateInstance(instanceType, length);
                visited.Add(instance, copied);
                for (int i = 0; i < length; ++i)
                    copied.SetValue(((Array)instance).GetValue(i).Clone(visited), i);
                return copied;
            }
            else
                return Clone(instance, visited, DeduceInstance(instance));
        }

        private static object Clone(this object instance, VisitedGraph visited, object copy)
        {
            visited.Add(instance, copy);
            List<FieldInfo> fields = new List<FieldInfo>();
            FindFields(fields, instance.GetType());
            foreach (FieldInfo field in fields)
            {
                object value = field.GetValue(instance);
                if (visited.ContainsKey(value))
                    field.SetValue(copy, visited[value]);
                else
                    field.SetValue(copy, value.Clone(visited));
            }
            return copy;
        }

        static BindingFlags _fieldFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly;
        static void FindFields(List<FieldInfo> fields, Type t)
        {
            fields.AddRange(t.GetFields(_fieldFlags));
            var baseType = t.BaseType;
            if (baseType != null)
                FindFields(fields, baseType);
        }

        private static object DeduceInstance(object instance)
        {
            Type instanceType = instance.GetType();
            try
            {
                return Activator.CreateInstance(instanceType);
            }
            catch
            {
                throw new ArgumentException(string.Format("Object of type {0} cannot be cloned because it does not have a parameterless constructor.", instanceType.FullName));
            }
        }
    }
}
