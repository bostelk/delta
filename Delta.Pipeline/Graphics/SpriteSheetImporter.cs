using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content.Pipeline;

namespace Delta.Graphics
{
    [ContentImporter(".spritesheet", DisplayName = "Sprite Sheet - Delta", DefaultProcessor = "SpriteSheetProcessor")]
    public class SpriteSheetImporter : ContentImporter<SpriteSheetContent>
    {
        public override SpriteSheetContent Import(string fileName, ContentImporterContext context)
        {
            if (SpriteSheetContent._spriteSheetFiles.ContainsKey(fileName))
                return SpriteSheetContent._spriteSheetFiles[fileName];
            else
            {
                SpriteSheetContent spriteSheetContent = new SpriteSheetContent(fileName);
                SpriteSheetContent._spriteSheetFiles.Add(fileName, spriteSheetContent);
                return spriteSheetContent;
            }
        }
    }
}
