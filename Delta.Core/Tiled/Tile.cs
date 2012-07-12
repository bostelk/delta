using System;
using System.Xml;
using System.ComponentModel;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Delta.Graphics;
using Delta;

namespace Delta.Tiled
{
    public class Tile : EntityBase
    {
        [ContentSerializer]
        internal int _imageFrameIndex = -1;
        internal Rectangle _sourceRectangle = Rectangle.Empty;
        [ContentSerializer]
        internal short _tilesetIndex = -1;

        public Tile()
            : base()
        {
        }

#if WINDOWS
        public Tile(Vector2 position, uint tileID)
        {
            _renderPosition = position;
            int imageFrameIndex = (int)(tileID & ~(0x40000000 | 0x80000000));

            for (int i = 0; i < Map.Instance._tilesets.Count; i++)
            {
                Tileset tileset = Map.Instance._tilesets[i];
                if (imageFrameIndex >= tileset.FirstGID)
                {
                    _imageFrameIndex = (ushort)(imageFrameIndex - tileset.FirstGID);
                    _tilesetIndex = (byte)Map.Instance._tilesets.IndexOf(tileset);
                    break;
                }
            }

            // THERE ARE JUST HERE INCASE WE NEED IT, ROB DOESN'T USE MARGIN OR SPACING IN TILED
            //SpriteEffects = SpriteEffects.None;
            //if ((tileID & FlippedHorizontallyFlag) != 0)
            //    SpriteEffects |= SpriteEffects.FlipHorizontally;
            //if ((tileID & FlippedVerticallyFlag) != 0)
            //    SpriteEffects |= SpriteEffects.FlipVertically;

            //int imageWidthInTiles = (tileset.Width - tileset.Margin * 2) / map.TileWidth;
            //Source = new Rectangle(
            //    (tileset.Margin + tileIndex % imageWidthInTiles) * (map.TileWidth + tileset.Spacing),
            //    (tileset.Margin + tileIndex / imageWidthInTiles) * (map.TileHeight + tileset.Spacing),
            //    map.TileWidth, 
            //    map.TileHeight);

        }
#endif

        public override void InternalDraw(DeltaTime deltaTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Map.Instance._spriteSheet.Texture, RenderPosition, _sourceRectangle, Color.White, 0, Vector2.Zero, Vector2.One, SpriteEffects.None, 0);
        }

    }
}
