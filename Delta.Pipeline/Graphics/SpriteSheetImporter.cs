using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content.Pipeline;

namespace Delta.Graphics
{
    [ContentImporter(".spritesheet", DisplayName = "SpriteSheetImporter", DefaultProcessor = "SpriteSheetProcessor")]
    public class SpriteSheetImporter : ContentImporter<SpriteSheetContent>
    {
        public override SpriteSheetContent Import(string fileName, ContentImporterContext context)
        {
            return new SpriteSheetContent(fileName);
        }
    }
}
