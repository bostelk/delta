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
            CheckFilesForDependencies(context);
            if (input._tilesets.Count > 0)
            {
                foreach (var image in SpriteSheetContent._imageReferences)
                {
                    if (image.Key == input._tilesets[0].ExternalImagePath)
                        input._spriteSheetName = image.Value._outputPath;
                }
            }
            return input;
        }

        internal static List<string> _files = null;
        internal static HashSet<string> _loadedFileNames = new HashSet<string>();
        static void CheckFilesForDependencies(ContentProcessorContext context)
        {
            if (_files == null)
            {
                _files = new List<string>();
                DirectoryInfo d = new DirectoryInfo(Environment.CurrentDirectory);
                foreach (var fi in d.GetFiles())
                    _files.Add(fi.FullName);
                foreach (var di in d.GetDirectories())
                    foreach (var fi in di.GetFiles())
                        _files.Add(fi.FullName);
            }
            for (int x = 0; x < _files.Count; x++)
            {
                string fileName = _files[x];
                string lowerFileName = fileName.ToLower(); ;
                if (lowerFileName.EndsWith(".spritesheet"))
                {
                    context.AddDependency(fileName);
                    if (!_loadedFileNames.Contains(fileName))
                    {
                        _loadedFileNames.Add(fileName);
                        context.BuildAsset<SpriteSheetContent, SpriteSheetContent>(new ExternalReference<SpriteSheetContent>(fileName), "SpriteSheetProcessor");
                    }
                    continue;
                }
                if (lowerFileName.EndsWith(".stylesheet"))
                {
                    context.AddDependency(fileName);
                    if (!_loadedFileNames.Contains(fileName))
                    {
                        _loadedFileNames.Add(fileName);
                        StyleSheet styleSheet = new StyleSheet(fileName); //don't built it as XNB. We just want this when building, nothing else.
                        foreach (var objectStyle in styleSheet.ObjectStyles)
                        {
                            if (!StyleSheet._globalObjectStyles.ContainsKey(objectStyle.Key))
                                StyleSheet._globalObjectStyles.Add(objectStyle.Key, objectStyle.Value);
                            else
                                StyleSheet._globalObjectStyles[objectStyle.Key] = objectStyle.Value;
                        }
                    }
                    continue;
                }
                _files.RemoveAt(x);
            }
        }
    }
}
