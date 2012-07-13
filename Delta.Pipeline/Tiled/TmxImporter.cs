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
            return new Map(fileName);
        }

    }
}
