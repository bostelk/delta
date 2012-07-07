#if WINDOWS
using System;
using System.Xml;
using System.Reflection;
using System.ComponentModel;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Delta.Tiled
{
    internal static class TmxImporterExtensions
    {
        internal static void ImportLayer(this ILayer layer, XmlNode node)
        {
            layer.ID = node.Attributes["name"].Value;
            layer.IsVisible = (node.Attributes["visible"] != null) ? int.Parse(node.Attributes["visible"].Value, CultureInfo.InvariantCulture) == 1 : true;
        }

        internal static void ImportXmlProperties(this IEntity entity, XmlNode node)
        {
            if (node == null)
                return;
            bool isFound = false;
            foreach (XmlNode propertyNode in node.ChildNodes)
            {
                isFound = false;
                string name = propertyNode.Attributes["name"] == null ? string.Empty : propertyNode.Attributes["name"].Value.ToLower();
                string value = propertyNode.Attributes["value"].Value.ToLower();
                if (!string.IsNullOrEmpty(name))
                    isFound = entity.ImportCustomValues(name, value);
                else
                    continue;
                if (!isFound)
                    throw new Exception(String.Format("Could not import Tiled XML property '{0}', no such property exists for '{1}'.", name, entity.GetType().Name));
            }
        }

    }
}
#endif