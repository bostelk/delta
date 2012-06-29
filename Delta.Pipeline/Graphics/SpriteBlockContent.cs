using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Delta.Graphics
{
    internal class SpriteBlockContent
    {
        internal ImageContent Image { get; set; }
        internal Rectangle SourceRegion { get; set; }
        internal Rectangle DestinationRegion { get; set; }
    }
}
