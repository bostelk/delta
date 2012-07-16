using System;
using System.IO;
using System.Xml;
using System.ComponentModel;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Intermediate;
using System.Globalization;

namespace Delta.Graphics
{
    internal class SpriteBlockContent
    {
        internal ImageContent Image { get; set; }
        internal Rectangle SourceRegion { get; set; }
        internal Rectangle DestinationRegion { get; set; }
    }

    public class SpriteSheetContent
    {
        public static Dictionary<string, SpriteSheetContent> _spriteSheetFiles = new Dictionary<string, SpriteSheetContent>();
        public static Dictionary<string, string> _imageReferences = new Dictionary<string, string>();

        internal Texture2DContent _texture = null;
        internal List<SpriteBlockContent> _blocks = new List<SpriteBlockContent>();
        internal Dictionary<string, Dictionary<int, Rectangle>> _imageFrameSourceRectangles = new Dictionary<string, Dictionary<int, Rectangle>>();
        internal string _fileName = string.Empty;

        public List<ImageContent> Images { get; set; }

        public SpriteSheetContent()
            : base()
        {
            Images = new List<ImageContent>();
        }

        public SpriteSheetContent(string fileName)
            : this()
        {
            _fileName = fileName;
            XmlDocument document = new XmlDocument();
            document.Load(fileName);
            foreach (XmlNode node in document.DocumentElement.ChildNodes)
            {
                if (node.Name.ToLower() != "image")
                    continue;
                ImageContent image = new ImageContent();
                foreach (XmlNode childNode in node.ChildNodes)
                {
                    switch (childNode.Name.ToLower())
                    {
                        case "path":
                            image.Path = childNode.InnerText;
                            if (!_imageReferences.ContainsKey(image.Path))
                            {
                                string xnbFileName = Path.ChangeExtension(fileName, "");
                                xnbFileName = xnbFileName.Remove(xnbFileName.Length - 1);
                                xnbFileName = xnbFileName.Replace(Environment.CurrentDirectory, "");
                                xnbFileName = xnbFileName.Remove(0, 1);
                                _imageReferences.Add(image.Path, xnbFileName);
                            }
                            break;
                        case "framesize":
                            Vector2 size = Vector2Extensions.Parse(childNode.InnerText);
                            image.FrameWidth = (int)size.X;
                            image.FrameHeight = (int)size.Y;
                            break;
                        case "animation":
                            string animationName = string.Empty;
                            string frames = string.Empty;
                            float frameDuration = 0.0f;
                            foreach (XmlNode animationChildNode in childNode.ChildNodes)
                            {
                                switch (animationChildNode.Name.ToLower())
                                {
                                    case "name":
                                        animationName = animationChildNode.InnerText;
                                        break;
                                    case "frames":
                                        frames = animationChildNode.InnerText;
                                        break;
                                    case "frameduration":
                                        frameDuration = float.Parse(animationChildNode.InnerText, CultureInfo.InvariantCulture);
                                        break;
                                }
                            }
                            image.Animations.Add(new Animation(animationName, frames, frameDuration));
                            break;
                    }
                }
                if (!string.IsNullOrEmpty(image.Path) && image.FrameWidth != 0 && image.FrameHeight != 0)
                    Images.Add(image);
            }
        }
    }

}


