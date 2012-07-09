#if WINDOWS

using System;
using System.Xml;
using System.Reflection;
using System.Collections.Generic;

namespace Delta.Tiled
{
    public class StyleSheet
    {
        XmlDocument _document = new XmlDocument();
        static Dictionary<string, IEntity> _objectStyles = new Dictionary<string, IEntity>();

        public StyleSheet()
            : base()
        {
        }

        public StyleSheet(string fileName)
            : this()
        {
            _document.Load(fileName);
            foreach (XmlNode node in _document.DocumentElement.ChildNodes)
            {
                string typeName = node.Attributes["Type"] == null ? string.Empty : node.Attributes["Type"].Value;
                if (string.IsNullOrEmpty(typeName))
                    continue;
                IEntity entity = null;
                if (_objectStyles.ContainsKey(typeName))
                    entity = _objectStyles[typeName].Copy() as IEntity;
                else
                    entity = CreateInstance(typeName);
                if (entity == null)
                    continue;
                entity.ID = node.Name;
                foreach (XmlNode childNode in node.ChildNodes)
                {
                    if (!entity.ImportCustomValues(childNode.Name.ToLower(), childNode.InnerText))
                        throw new Exception(String.Format("Could not import XML property '{0}', no such property exists for '{1}'.", childNode.Name.ToLower(), entity.GetType().Name));
                }
                _objectStyles.Add(node.Name, entity);
            }
        }

        static IEntity CreateInstance(string typeName)
        {
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                Type type = assembly.GetType(typeName, false, true);
                if (type != null)
                    return Activator.CreateInstance(type) as IEntity;
            }
            return null;
        }

        public static IEntity Load(string name)
        {
            if (!_objectStyles.ContainsKey(name))
                return CreateInstance(name);
            return _objectStyles[name].Copy() as IEntity;
        }

    }
}

#endif