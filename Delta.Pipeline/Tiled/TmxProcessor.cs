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
                {
                    StyleSheet styleSheet = context.BuildAndLoadAsset<FilenameContent, StyleSheet>(new ExternalReference<FilenameContent>(file), "StyleSheetProcessor");
                    foreach (var objectStyle in styleSheet.ObjectStyles)
                    {
                        if (!StyleSheet.GlobalObjectStyles.ContainsKey(objectStyle.Key))
                            StyleSheet.GlobalObjectStyles.Add(objectStyle.Key, objectStyle.Value);
                        else
                            StyleSheet.GlobalObjectStyles[objectStyle.Key] = objectStyle.Value;
                    }
                }
            }
            return new Map(input.FileName);
        }
    }
}
