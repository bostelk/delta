using System;
using System.IO;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

#if WINDOWS
using System.Xml;
using System.Reflection;
using System.Globalization;
#endif

using Delta.Graphics;

namespace Delta.Tiled
{
    public class Map : EntityParent<ILayer>
    {
        internal static Map Instance { get; set; }

        [ContentSerializer(FlattenContent=true, ElementName="Tileset")]
        internal List<Tileset> _tilesets = new List<Tileset>();
        internal SpriteSheet _spriteSheet = null;

        [ContentSerializer]
        public string Version { get; private set; }
        [ContentSerializer]
        public int TileWidth { get; private set; }
        [ContentSerializer]
        public int TileHeight { get; private set; }
        [ContentSerializer]
        public int Width { get; private set; }
        [ContentSerializer]
        public int Height { get; private set; }
        [ContentSerializer]
        public MapOrientation Orientation { get; private set; }
        [ContentSerializer(ElementName = "SpriteSheet")]
        public string SpriteSheetName { get; set; }

        public Map()
            : base()
        {
            Instance = this;
        }

#if WINDOWS
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Map(string fileName)
            : this()
        {
            XmlDocument document = new XmlDocument(); document.Load(fileName);
            XmlNode node = document["map"];

            Version = node.Attributes["version"].Value;
            TileWidth = int.Parse(node.Attributes["tilewidth"].Value, CultureInfo.InvariantCulture);
            TileHeight = int.Parse(node.Attributes["tileheight"].Value, CultureInfo.InvariantCulture);
            Width = int.Parse(node.Attributes["width"].Value, CultureInfo.InvariantCulture);
            Height = int.Parse(node.Attributes["height"].Value, CultureInfo.InvariantCulture);
            Orientation = (MapOrientation)Enum.Parse(typeof(MapOrientation), node.Attributes["orientation"].Value, true);

            if (Orientation != MapOrientation.Orthogonal)
                throw new NotSupportedException(String.Format("{0} does not have built in support for rendering isometric Tiled maps.", Assembly.GetExecutingAssembly().GetName().Name));

            foreach (XmlNode tilesetNode in node.SelectNodes("tileset"))
                _tilesets.Add(new Tileset(tilesetNode));

            //sort the tilesets by largest GID, this allows proper tile setup.
            _tilesets.Sort((a, b) => (-a.FirstGID.CompareTo(b.FirstGID)));

            int layerOrder = 0;
            foreach (XmlNode layerNode in node.SelectNodes("layer|objectgroup"))
            {
                ILayer layer = null;
                if (layerNode.Name.ToLower() == "layer")
                    layer = new TileLayer(fileName, layerNode);
                else if (layerNode.Name.ToLower() == "objectgroup")
                    layer = new EntityLayer(fileName, layerNode);
                else
                    throw new Exception(String.Format("Unknown layer type '{0}'.", layerNode.Name));
                layer.ImportXmlProperties(layerNode["properties"]);
                this.Add(layer);
                layer.Order = layerOrder;
                layerOrder++;
            }

            this.ImportXmlProperties(node.SelectSingleNode("properties"));
        }
#endif

        protected override void LateInitialize()
        {
            base.LateInitialize();
            _spriteSheet = G.Content.Load<SpriteSheet>(SpriteSheetName);
        }

        protected override bool CanDraw()
        {
            if (_spriteSheet == null) return false;
            return base.CanDraw();
        }

        protected override bool CanUpdate()
        {
            if (_spriteSheet == null) return false;
            return base.CanUpdate();
        }

    }
}
