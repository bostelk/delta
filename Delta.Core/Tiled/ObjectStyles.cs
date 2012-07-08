using System;
using System.Xml;
using System.Reflection;
using System.Collections.Generic;

namespace Delta.Tiled
{
    internal class ObjectStyle
    {
        public IEntity Entity;
        public Type Type;
        public XmlNode Node;
    }

    public class ObjectStyles
    {
        static XmlDocument _document = new XmlDocument();
        static Dictionary<string, ObjectStyle> _objectStyles = new Dictionary<string, ObjectStyle>();

        static string _fileName = "ObjectStyles.xml";
        public static string FileName
        {
            get { return _fileName; }
            set 
            {
                if (_fileName != value)
                    _fileName = value;
            }
        }

        public static void Cache()
        {
            _document.Load(_fileName);
            _objectStyles.Clear();
            foreach (XmlNode node in _document.DocumentElement.ChildNodes)
            {
                string typeName = node.Attributes["Type"] == null ? string.Empty : node.Attributes["Type"].Value;
                if (string.IsNullOrEmpty(typeName))
                    continue;
                ObjectStyle _style = new ObjectStyle();
                ObjectStyle _parentStyle = null;
                _style.Node = node;
                if (_objectStyles.ContainsKey(typeName))
                {
                    _parentStyle = _objectStyles[typeName];
                    _style.Entity = Activator.CreateInstance(_parentStyle.Type) as IEntity;
                }
                else
                {
                    foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
                    {
                        _style.Type = assembly.GetType(typeName, false, true);
                        if (_style.Type != null)
                        {
                            _style.Entity = Activator.CreateInstance(_style.Type) as IEntity;
                            break;
                        }
                    }
                    if (_style.Entity == null)
                        continue;
                }
                if (_parentStyle != null)
                {
                    foreach (XmlNode childNode in _parentStyle.Node.ChildNodes)
                        _style.Entity.ImportCustomValues(childNode.Name.ToLower(), childNode.InnerText);
                }
                foreach (XmlNode childNode in node.ChildNodes)
                {
                    if (!_style.Entity.ImportCustomValues(childNode.Name.ToLower(), childNode.InnerText))
                        throw new Exception(String.Format("Could not import XML property '{0}', no such property exists for '{1}'.", childNode.Name.ToLower(), _style.Type.Name));
                }
                _objectStyles.Add(node.Name, _style);
            }
        }

        public static IEntity Load(string name)
        {
            if (_document.FirstChild == null)
                Cache();
            if (!_objectStyles.ContainsKey(name))
            {
                foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
                {
                    Type type = assembly.GetType(name, false, true);
                    if (type != null)
                        return Activator.CreateInstance(type) as IEntity;
                }
                return null;
            }
            IEntity copyedEntity = _objectStyles[name].Entity.Copy() as IEntity;
            return copyedEntity;
        }

    }
}
