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

namespace Delta.Tiled
{
    [ContentImporter(".stylesheet", DisplayName="StyleSheetImporter", DefaultProcessor="StyleSheetProcessor")]
    public class StyleSheetImporter : ContentImporter<FilenameContent>
    {
        public override FilenameContent Import(string fileName, ContentImporterContext context)
        {
            return new FilenameContent(fileName);
        }
    }
}
