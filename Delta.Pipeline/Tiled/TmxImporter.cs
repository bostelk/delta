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
    [ContentImporter(".tmx", DisplayName="Tiled Map - Delta", DefaultProcessor="TmxProcessor")]
    public class TmxImporter : ContentImporter<Map>
    {
        public override Map Import(string fileName, ContentImporterContext context)
        {
            CheckFilesForDependencies(context);
            return new Map(fileName);
        }

        static void FilesInDirectory(DirectoryInfo di, ref List<FileInfo> files)
        {
            foreach (var fi in di.GetFiles())
                _files.Add(fi);
            foreach (var d in di.GetDirectories())
                FilesInDirectory(d, ref files);
        }

        internal static List<FileInfo> _files = null;
        static void CheckFilesForDependencies(ContentImporterContext context)
        {
            if (_files == null)
            {
                _files = new List<FileInfo>();
                FilesInDirectory(new DirectoryInfo(Environment.CurrentDirectory), ref _files);
            }
            List<FileInfo> _filesToRemove = new List<FileInfo>();
            foreach (var file in _files)
            {
                switch (file.Extension.ToLower())
                {
                    case ".spritesheet":
                        if (!SpriteSheetContent._spriteSheetFiles.ContainsKey(file.FullName))
                            SpriteSheetContent._spriteSheetFiles.Add(file.FullName, new SpriteSheetContent(file.FullName));
                        break;
                    case ".stylesheet":
                        context.AddDependency(file.FullName);
                        Map.LoadStyleSheet(file.FullName);
                        break;
                    default:
                        _filesToRemove.Add(file);
                        break;
                }
            }
            foreach (var file in _filesToRemove)
                _files.Remove(file);
        }
    }
}
