//using System;
//using System.IO;
//using System.Collections.Generic;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
//using Microsoft.Xna.Framework.Content;

//#if WINDOWS
//using System.Xml;
//using System.IO.Compression;
//using System.Reflection;
//using System.Globalization;
//#endif

//namespace Delta.Tiled
//{

//    public class TileLayer : Entity
//    {
//        [ContentSerializer(FlattenContent=true, CollectionItemName="Tile")]
//        List<Tile> _tiles = new List<Tile>();

//        public float Parallax { get; set; }
//        public int TileCount { get { return _tiles.Count; } }

//#if DEBUG
//        public int TilesDrawn { get; set; }
//#endif

//        public TileLayer()
//            : base()
//        {
//        }

//#if WINDOWS
//        public TileLayer(string fileName, XmlNode node)
//            : base()
//        {
//            this.ImportLayer(node);

//        }
//#endif

//        protected override void LateInitialize()
//        {
//            base.LateInitialize();
//            for (int i = 0; i < _tiles.Count; i++)
//            {
//                Tileset tileset = Map.Instance._tilesets[_tiles[i]._tilesetIndex];
//                _tiles[i]._sourceRectangle = Map.Instance._spriteSheet.GetFrameSourceRectangle(Path.GetFileNameWithoutExtension(tileset.ExternalImagePath), _tiles[i]._imageFrameIndex);
//            }
//        }

//        protected internal override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
//        {
//#if DEBUG
//            TilesDrawn = 0;
//#endif
//            Tile tile;
//            Rectangle tileArea = Rectangle.Empty;
//            Rectangle viewingArea = G.World.Camera.ViewingArea;
//            viewingArea.Inflate(Map.Instance.TileWidth, Map.Instance.TileHeight); // pad the viewing area with a border of off-screen tiles. for smooth scrolling, otherwise tiles seem to 'pop' in.
//            for (int i = 0; i < _tiles.Count; i++)
//            {
//                tile = _tiles[i];
//                tileArea = new Rectangle((int)tile._renderPosition.X, (int)tile._renderPosition.Y, Map.Instance.TileWidth, Map.Instance.TileHeight);
//                if (viewingArea.Contains(tileArea) || viewingArea.Intersects(tileArea))
//                {
//                    tile.Draw(spriteBatch);
//#if DEBUG
//                    TilesDrawn++;
//#endif
//                }
//            }
//        }

//    }
//}
