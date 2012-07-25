using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Delta
{
    public static class Tweaker
    {
        public interface ITweak
        {
            string InstanceName { get; }
            string VariableName { get; }
            void SetValue(object value);
            object GetValue();
        }

        public struct TweakProperty : ITweak
        {
            object _instance;
            PropertyInfo _property;

            public string InstanceName { get { return _instance.ToString(); } }
            public string VariableName { get { return _property.Name; } }

            public TweakProperty(object tweaking, PropertyInfo property)
            {
                if (tweaking == null)
                {
                    throw new ArgumentNullException("tweaking is null");
                }
                if (property == null)
                {
                    throw new ArgumentNullException("field is null");
                }
                _instance = tweaking;
                _property = property;
            }

            public void SetValue(object value)
            {
                // there's a chance the object no longer exists...
                if (_instance == null)
                    return;
                if (value == null)
                {
                    throw new ArgumentNullException();
                }
                if (_property == null)
                {
                    throw new InvalidOperationException();
                }
                _property.SetValue(_instance, value, null);
            }

            public object GetValue()
            {
                return _property.GetValue(_instance, null);
            }
        }

        public struct TweakField : ITweak
        {
            object _instance;
            FieldInfo _field;

            public string InstanceName { get { return _instance.ToString(); } }
            public string VariableName { get { return _field.Name; } }

            public TweakField(object tweaking, FieldInfo field)
            {
                if (tweaking == null)
                {
                    throw new ArgumentNullException("tweaking is null");
                }
                if (field == null)
                {
                    throw new ArgumentNullException("field is null");
                }
                _instance = tweaking;
                _field = field;
            }

            public void SetValue(object value)
            {
                // there's a chance the object no longer exists...
                if (_instance == null)
                    return;
                if (value == null)
                {
                    throw new ArgumentNullException();
                }
                if (_field == null)
                {
                    throw new InvalidOperationException();
                }
                _field.SetValue(_instance, value);
            }

            public object GetValue()
            {
                return _field.GetValue(_instance);
            }
        }

        static Dictionary<object, Dictionary<string, ITweak>> _tweakCache;

        static Tweaker()
        {
            _tweakCache = new Dictionary<object, Dictionary<string, ITweak>>();
        }

        /// <summary>
        /// Find all the variables that can be tweaked at run-time.
        /// </summary>
        /// <param name="obj">Object that will be tweaked.</param>
        /// <returns>Tweaking varaibles.</returns>
        public static Dictionary<string, ITweak> FindTweakables(object obj) {
            if (_tweakCache.ContainsKey(obj))
                return _tweakCache[obj];

            Dictionary<string, ITweak> tweakables = new Dictionary<string, ITweak>();
            foreach (PropertyInfo property in obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                foreach (Tweakable tweak in property.GetCustomAttributes(typeof(Tweakable), true))
                {
                    if (tweak != null)
                    {
                        TweakProperty tp = new TweakProperty(obj, property);
                        tweakables.Add(tp.VariableName, tp as ITweak);
                    }
                }
            }
            foreach (FieldInfo field in obj.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                // assume there is only one attribute and the first one is tweakable.
                foreach (Tweakable tweak in field.GetCustomAttributes(typeof(Tweakable), true))
                {
                    if (tweak != null)
                    {
                        TweakField tp = new TweakField(obj, field);
                        tweakables.Add(tp.VariableName, tp as ITweak);
                    }
                }
            }
            _tweakCache[obj] = tweakables;
            return tweakables;
        }

    }
}
