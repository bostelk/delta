using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Delta.Tiled;
using Delta.Graphics;

namespace Delta
{
    [ContentProcessor(DisplayName = "Tiled Map - Delta")]
    public class TmxProcessor : ContentProcessor<Map, Map>
    {
        public override Map Process(Map input, ContentProcessorContext context)
        {
            if (input._tilesets.Count > 0)
            {
                foreach (var spriteSheetImage in SpriteSheetContent._imageReferences)
                {
                    foreach (var tileset in input._tilesets)
                    {
                        if (Path.GetFileName(spriteSheetImage.Key) == Path.GetFileName(tileset.ExternalImagePath))
                            input._spriteSheetName = spriteSheetImage.Value;
                        break;
                    }
                    if (!string.IsNullOrEmpty(input._spriteSheetName))
                        break;
                }
            }
            return input;
        }
    }
}
