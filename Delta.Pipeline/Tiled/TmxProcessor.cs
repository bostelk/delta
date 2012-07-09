using System;
using System.IO;
using Microsoft.Xna.Framework.Content.Pipeline;

namespace Delta.Tiled
{
    [ContentProcessor(DisplayName = "TmxProcessor")]
    public class TmxProcessor : ContentProcessor<FilenameContent, Map>
    {
        public override Map Process(FilenameContent input, ContentProcessorContext context)
        {
            foreach (var file in Directory.GetFiles(Environment.CurrentDirectory))
            {
                if (file.ToLower().EndsWith(".stylesheet"))
                    context.BuildAndLoadAsset<FilenameContent, StyleSheet>(new ExternalReference<FilenameContent>(file), "StyleSheetProcessor");
            }
            return new Map(input.FileName);
        }
    }
}
