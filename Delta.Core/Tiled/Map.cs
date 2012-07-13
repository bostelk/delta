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
using System.IO.Compression;

namespace Delta.Tiled
{
    public enum MapOrientation : byte
    {
        Orthogonal,
        Isometric,
    }

    public class Map : EntityCollection, IImportable
    {
        internal static Map Instance { get; set; }

        [EditorBrowsable(EditorBrowsableState.Never)]
        [ContentSerializer(FlattenContent=true, ElementName="Tileset")]
        public List<Tileset> _tilesets = new List<Tileset>();
        [EditorBrowsable(EditorBrowsableState.Never)]
        [ContentSerializer(ElementName = "SpriteSheet")]
        public string _spriteSheetName = string.Empty;
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
        [ContentSerializer]
        public EntityCollection BelowGround { get; private set; }
        [ContentSerializer]
        public EntityCollection Ground { get; private set; }
        [ContentSerializer]
        public EntityCollection AboveGround { get; private set; }
        [ContentSerializer]
        public EntityCollection PostEffects { get; private set; }

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

            string layerName = string.Empty;
            int layerOrder = 0;
            bool layerIsVisible = false;
            foreach (XmlNode layerNode in node.SelectNodes("layer|objectgroup"))
            {
                layerName = layerNode.Attributes["name"].Value;
                layerIsVisible = (node.Attributes["visible"] != null) ? int.Parse(node.Attributes["visible"].Value, CultureInfo.InvariantCulture) == 1 : true;
                switch (layerNode.Name.ToLower())
                {
                    case "layer":
                        if (!layerIsVisible)
                            continue;
                        Add(new TileLayer(fileName, layerNode, layerName) { Layer = layerOrder});
                        break;
                    case "objectgroup":
                        EntityLayer entityLayer = new EntityLayer(fileName, layerNode, layerIsVisible) { Layer = layerOrder }; 
                        switch (layerName.ToLower())
                        {
                            case "delta.belowground":
                            case "delta.bg":
                            case "d.bg":
                                Map.Instance.BelowGround = entityLayer;
                                break;
                            case "delta.ground":
                            case "delta.g":
                            case "d.g":
                                Map.Instance.Ground = entityLayer;
                                break;
                            case "delta.aboveground":
                            case "delta.ag":
                            case "d.ag":
                                Map.Instance.AboveGround = entityLayer;
                                break;
                            default:
                                Add(entityLayer);
                                break;
                        }
                        break;
                    default:
                        throw new Exception(String.Format("Unknown layer type '{0}'.", layerNode.Name));
                }
                layerOrder++;
            }
            //this.ImportTiledProperties(node.SelectSingleNode("properties"));
        }

        bool IImportable.ImportCustomValues(string name, string value)
        {
            switch (name)
            {
                case "spritesheet":
                case "spritesheetname":
                    _spriteSheetName = value;
                    return true;
            }
            return false;
        }
#endif

        public override void LoadContent()
        {
            base.LoadContent();
            if (!string.IsNullOrEmpty(_spriteSheetName))
                _spriteSheet = G.Content.Load<SpriteSheet>(_spriteSheetName);
            foreach (IDrawable drawable in _drawables)
            {
                TileLayer tileLayer = drawable as TileLayer;
                foreach (Tile tile in tileLayer._tiles)
                {
                    Tileset tileset = Map.Instance._tilesets[tile._tilesetIndex];
                    tile._sourceRectangle = Map.Instance._spriteSheet.GetFrameSourceRectangle(Path.GetFileNameWithoutExtension(tileset.ExternalImagePath), tile._imageFrameIndex);
                }
            }
        }

    }

    internal static class MapHelper
    {
        internal static void ImportTiledProperties(this IImportable entity, XmlNode node)
        {
            if (node == null)
                return;
            bool isFound = false;
            foreach (XmlNode propertyNode in node.ChildNodes)
            {
                isFound = false;
                string name = propertyNode.Attributes["name"] == null ? string.Empty : propertyNode.Attributes["name"].Value.ToLower();
                string value = propertyNode.Attributes["value"].Value;
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
