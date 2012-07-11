using System;
using System.IO;
using Microsoft.Xna.Framework.Content.Pipeline;

namespace Delta.Tiled
{
    [ContentProcessor(DisplayName = "StyleSheetProcessor")]
    public class StyleSheetProcessor : ContentProcessor<FilenameContent, StyleSheet>
    {
        public override StyleSheet Process(FilenameContent input, ContentProcessorContext context)
        {
            return new StyleSheet(input.FileName);
        }
    }
}
