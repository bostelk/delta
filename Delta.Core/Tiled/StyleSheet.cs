#if WINDOWS

using System;
using System.Xml;
using System.Reflection;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;

namespace Delta.Tiled
{
    public class StyleSheet
    {
        public static Dictionary<string, Entity> _globalObjectStyles = new Dictionary<string, Entity>();

        XmlDocument _document = new XmlDocument();
        [ContentSerializer(FlattenContent = true, CollectionItemName = "ObjectStyle")]
        public Dictionary<string, Entity> ObjectStyles { get; private set; }

        public StyleSheet()
            : base()
        {
            ObjectStyles = new Dictionary<string, Entity>();
        }

        public StyleSheet(string fileName)
            : this()
        {
            _document.Load(fileName);
            foreach (XmlNode node in _document.DocumentElement.ChildNodes)
            {
                if (node.NodeType == XmlNodeType.Comment)
                    continue;
                string typeName = node.Attributes["Type"] == null ? string.Empty : node.Attributes["Type"].Value;
                if (string.IsNullOrEmpty(typeName))
                    continue;
                Entity entity = null;
                if (ObjectStyles.ContainsKey(typeName))
                    entity = ObjectStyles[typeName].Copy() as Entity;
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
                ObjectStyles.Add(node.Name, entity);
            }
        }

        static Entity CreateInstance(string typeName)
        {
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                Type type = assembly.GetType(typeName, false, true);
                if (type != null)
                    return Activator.CreateInstance(type) as Entity;
            }
            return null;
        }

        public static Entity Load(string name)
        {
            if (_globalObjectStyles.ContainsKey(name))
                return _globalObjectStyles[name].Copy() as Entity;
            return CreateInstance(name);
        }

    }
}

#endif