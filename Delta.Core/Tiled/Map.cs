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

    public class Map : EntityCollection, IEntity
    {
        internal static Map Instance { get; set; }

        [ContentSerializer(FlattenContent=true, ElementName="Tileset")]
        internal List<Tileset> _tilesets = new List<Tileset>();
        [ContentSerializer(ElementName = "SpriteSheet")]
        internal string _spriteSheetName = string.Empty;
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
            bool layerIsVisible = true;
            foreach (XmlNode layerNode in node.SelectNodes("layer|objectgroup"))
            {
                layerName = layerNode.Attributes["name"].Value;
                layerIsVisible = (node.Attributes["visible"] != null) ? int.Parse(node.Attributes["visible"].Value, CultureInfo.InvariantCulture) == 1 : true;
                switch (layerNode.Name.ToLower())
                {
                    case "layer":
                        if (layerIsVisible)
                            ImportTileLayer(layerNode, layerOrder);
                        break;
                    case "objectgroup":
                        ImportObjectGroup(layerNode, layerOrder, layerIsVisible);
                        break;
                    default:
                        throw new Exception(String.Format("Unknown layer type '{0}'.", layerNode.Name));
                }
                layerOrder++;
            }
            this.ImportTiledProperties(node.SelectSingleNode("properties"));
        }

        bool IEntity.ImportCustomValues(string name, string value)
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

        void ImportTileLayer(XmlNode node, int layerOrder)
        {
            XmlNode dataNode = node["data"];
            uint[] tileLayerData = new uint[Map.Instance.Width * Map.Instance.Height];
            if (dataNode.Attributes["encoding"] == null)
                throw new NotSupportedException(string.Format("{0} does not support un-encoded Tiled layer data.", Assembly.GetExecutingAssembly().GetName().Name));
            switch (dataNode.Attributes["encoding"].Value.ToLower())
            {
                case "base64":
                    Stream stream = new MemoryStream(Convert.FromBase64String(dataNode.InnerText), false);
                    if (dataNode.Attributes["compression"] != null)
                    {
                        switch (dataNode.Attributes["compression"].Value.ToLower())
                        {
                            case "gzip":
                                stream = new GZipStream(stream, CompressionMode.Decompress, false);
                                break;
                            default:
                                throw new NotSupportedException(string.Format("{0} does not support the compression '{1}' for Tiled layer data.", Assembly.GetExecutingAssembly().GetName().Name, dataNode.Attributes["compression"].Value));
                        }
                        using (stream)
                        {
                            using (BinaryReader reader = new BinaryReader(stream))
                            {
                                for (int i = 0; i < tileLayerData.Length; i++)
                                    tileLayerData[i] = reader.ReadUInt32();
                            }
                        }
                    }
                    break;
                default:
                    throw new NotSupportedException(string.Format("{0} does not support the encoding '{1}' for Tiled layer data.", Assembly.GetExecutingAssembly().GetName().Name, dataNode.Attributes["encoding"].Value));
            }
            for (int x = 0; x < Map.Instance.Width; x++)
            {
                for (int y = 0; y < Map.Instance.Height; y++)
                {
                    Tile tile = new Tile(new Vector2(x * Map.Instance.TileWidth, y * Map.Instance.TileHeight), tileLayerData[y * Map.Instance.Width + x]);
                    tile.MajorLayer = layerOrder;
                    if (tile._tilesetIndex >= 0)
                        InternalAdd(tile);
                }
            }
        }

        void ImportObjectGroup(XmlNode node, int layerOrder, bool layerIsVisible)
        {
            foreach (XmlNode objectNode in node.SelectNodes("object"))
            {
                Entity entity = StyleSheet.Load(objectNode.Attributes["type"].Value);
                if (entity == null)
                    continue;

                entity.IsVisible = layerIsVisible;
                entity.MajorLayer = layerOrder;

                entity.ID = objectNode.Attributes["name"] == null ? String.Empty : objectNode.Attributes["name"].Value;
                entity.Position = new Vector2(
                    objectNode.Attributes["x"] == null ? 0 : float.Parse(objectNode.Attributes["x"].Value, CultureInfo.InvariantCulture),
                    objectNode.Attributes["y"] == null ? 0 : float.Parse(objectNode.Attributes["y"].Value, CultureInfo.InvariantCulture)
                );
                entity.Size = new Vector2(
                    objectNode.Attributes["width"] == null ? 0 : float.Parse(objectNode.Attributes["width"].Value, CultureInfo.InvariantCulture),
                    objectNode.Attributes["height"] == null ? 0 : float.Parse(objectNode.Attributes["height"].Value, CultureInfo.InvariantCulture)
                );

                //List<Vector2> vertices = new List<Vector2>();
                //XmlNode polyNode = objectNode["polygon"];
                //if (polyNode == null)
                //    polyNode = objectNode["polyline"];
                //if (polyNode != null)
                //{
                //    foreach (string point in polyNode.Attributes["points"].Value.ToString().Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries))
                //    {
                //        string[] split = point.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                //        if (split.Length == 2)
                //            vertices.Add(entity.Position + new Vector2(float.Parse(split[0], CultureInfo.InvariantCulture), float.Parse(split[1], CultureInfo.InvariantCulture)));
                //        else
                //            throw new Exception(string.Format("The poly point'{0}' is not in the format 'x,y'.", point));
                //    }
                //}

                //CollideableEntity collideableEntity = entity as CollideableEntity;
                //if (collideableEntity != null)
                //{
                //    if (size != Vector2.Zero)
                //    {
                //        collideableEntity.Polygon = new OBB(size.X, size.Y);
                //        // tiled's position is the top-left position of a tile. position the entity at the tile center.
                //        collideableEntity.Position += new Vector2(size.X / 2, size.Y / 2);
                //    } else {
                //        /*
                //        // remove the closing vertex
                //        if (polyVertices[0] == polyVertices[polyVertices.Count - 1])
                //            polyVertices.RemoveAt(polyVertices.Count - 1);
                //        */
                //        // unless the polygon is convex decompose it into polylines.
                //        Vector2 distance = Vector2.Zero;
                //        Vector2 totalDistance = Vector2.Zero;
                //        for (int i = 0; i < vertices.Count; i++)
                //        {
                //            CollideableEntity line = new CollideableEntity();
                //            line.Polygon = new Polygon(vertices[i], vertices[(i+1)%(vertices.Count-1)]);
                //            distance =  (vertices[(i + 1) % (vertices.Count - 1)] - vertices[i]);
                //            totalDistance += distance;
                //            line.Position = position + totalDistance - distance / 2;
                //            Add(line);
                //        }
                //    }
                //}
                entity.ImportTiledProperties(objectNode["properties"]);

                bool added = false;
                SpriteEntity sprite = entity as SpriteEntity;
                if (sprite != null)
                {
                    if (sprite.IsOverlay)
                    {
                        PostEffects.Add(sprite);
                        added = true;
                    }
                }
                if (!added)
                    Add(entity);
            }
        }

#endif

        public override void LoadContent()
        {
            base.LoadContent();
            if (!string.IsNullOrEmpty(_spriteSheetName))
                _spriteSheet = G.Content.Load<SpriteSheet>(_spriteSheetName);
        }

    }

    internal static class MapHelper
    {
        internal static void ImportTiledProperties(this IEntity entity, XmlNode node)
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
