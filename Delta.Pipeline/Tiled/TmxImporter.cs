using System;
using System.Xml;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Globalization;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Content.Pipeline;
using Delta.Graphics;

namespace Delta.Tiled
{
    [ContentImporter(".tmx", DisplayName="TmxImporter", DefaultProcessor="TmxProcessor")]
    public class TmxImporter : ContentImporter<Map>
    {
        public override Map Import(string fileName, ContentImporterContext context)
        {
            CheckFilesForDependencies(context);
            return new Map(fileName);
        }

        static void FilesInDirectory(DirectoryInfo di, List<FileInfo> files)
        {
            foreach (var fi in di.GetFiles())
                _files.Add(fi);
            foreach (var d in di.GetDirectories())
                FilesInDirectory(d, files);
        }

        internal static List<FileInfo> _files = null;
        static void CheckFilesForDependencies(ContentImporterContext context)
        {
            if (_files == null)
            {
                _files = new List<FileInfo>();
                FilesInDirectory(new DirectoryInfo(Environment.CurrentDirectory), _files);
            }
            for (int x = 0; x < _files.Count; x++)
            {
                FileInfo file = _files[x];
                switch (file.Extension.ToLower())
                {
                    case ".spritesheet":
                        context.AddDependency(file.FullName);
                        break;
                    case ".stylesheet":
                        context.AddDependency(file.FullName);
                        StyleSheet styleSheet = new StyleSheet(file.FullName); //don't built it as XNB. We just want this when building, nothing else.
                        foreach (var objectStyle in styleSheet.ObjectStyles)
                        {
                            if (!StyleSheet._globalObjectStyles.ContainsKey(objectStyle.Key))
                                StyleSheet._globalObjectStyles.Add(objectStyle.Key, objectStyle.Value);
                            else
                                StyleSheet._globalObjectStyles[objectStyle.Key] = objectStyle.Value;
                        }
                        break;
                    default:
                        _files.RemoveAt(x);
                        break;
                }
            }
        }
    }
}
