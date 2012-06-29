using System;
using System.ComponentModel;
using Microsoft.Xna.Framework.Content;

#if WINDOWS
using System.IO;
using System.Xml;
using System.Globalization;
#endif

namespace Delta
{
    public class Tileset
    {
        [ContentSerializer]
        public int FirstGID { get; private set; }
        [ContentSerializer]
        public int Width { get; private set; }
        [ContentSerializer]
        public int Height { get; private set; }
        [ContentSerializer]
        public int Spacing { get; private set; }
        [ContentSerializer]
        public int Margin { get; private set; }
        [ContentSerializer]
        public string ExternalImagePath { get; private set; }

        public Tileset()
            : base()
        {
        }

#if WINDOWS
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Tileset(XmlNode node)
            : this()
        {
            FirstGID = int.Parse(node.Attributes["firstgid"].Value, CultureInfo.InvariantCulture);
            XmlNode imageNode = node["image"];
            XmlNode tilesetNode = null;
            if (imageNode == null)
            {   //import the external tileset saved as a seperate xml file
                XmlDocument tilesetDocument = new XmlDocument(); tilesetDocument.Load(node.Attributes["source"].Value);
                tilesetNode = tilesetDocument["tileset"];
                imageNode = tilesetNode["image"];
            }
            else //the tileset is embedded within the map xml file
                tilesetNode = node;
            ExternalImagePath = imageNode.Attributes["source"].Value;
            while (ExternalImagePath.StartsWith(@".."))
                ExternalImagePath = ExternalImagePath.Remove(0, 3);
            Width = int.Parse(imageNode.Attributes["width"].Value, CultureInfo.InvariantCulture);
            Height = int.Parse(imageNode.Attributes["height"].Value, CultureInfo.InvariantCulture);
            Spacing = tilesetNode.Attributes["spacing"] == null ? 0 : int.Parse(tilesetNode.Attributes["spacing"].Value, CultureInfo.InvariantCulture);
            Margin = tilesetNode.Attributes["margin"] == null ? 0 : int.Parse(tilesetNode.Attributes["margin"].Value, CultureInfo.InvariantCulture);
        }
#endif

    }
}