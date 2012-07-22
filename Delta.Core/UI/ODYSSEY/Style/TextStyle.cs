//using System;
//using System.Drawing;
//using System.Globalization;
//using System.Text.RegularExpressions;
//using Microsoft.DirectX.Direct3D;
//using Font=Microsoft.DirectX.Direct3D.Font;

//namespace AvengersUTD.Odyssey.UserInterface
//{
//    public class TextStyle
//    {
//        bool bold;
//        bool italic;
//        bool isShadowed;
//        bool applyHighlight;
//        bool ignoreBounds;
//        Color standardColor;
//        Color highlightedColor;
//        int size;
//        string fontName;
//        Alignment horizontalAlignment;
//        Alignment verticalAlignment;
//        DrawTextFormat flags;

//        #region Properties

//        public bool Bold
//        {
//            get { return bold; }
//            set { bold = value; }
//        }

//        public bool Italic
//        {
//            get { return italic; }
//            set { italic = value; }
//        }

//        public bool IsShadowed
//        {
//            get { return isShadowed; }
//            set { isShadowed = value; }
//        }

//        public Color StandardColor
//        {
//            get { return standardColor; }
//            set { standardColor = value; }
//        }

//        public Color HighlightedColor
//        {
//            get { return highlightedColor; }
//            set { highlightedColor = value; }
//        }

//        public int Size
//        {
//            get { return size; }
//            set { size = value; }
//        }

//        public string FontName
//        {
//            get { return fontName; }
//            set { fontName = value; }
//        }

//        public Alignment HorizontalAlignment
//        {
//            get { return horizontalAlignment; }
//            set
//            {
//                horizontalAlignment = value;
//                flags = BuildFlags(this);
//            }
//        }

//        public Alignment VerticalAlignment
//        {
//            get { return verticalAlignment; }
//            set
//            {
//                verticalAlignment = value;
//                flags = BuildFlags(this);
//            }
//        }

//        public bool IgnoreBounds
//        {
//            get { return ignoreBounds; }
//            set
//            {
//                ignoreBounds = value;
//                flags = BuildFlags(this);
//            }
//        }

//        public bool ApplyHighlight
//        {
//            get { return applyHighlight; }
//            set { applyHighlight = value; }
//        }

//        public DrawTextFormat Flags
//        {
//            get { return flags; }
//        }

//        public static TextStyle Default
//        {
//            get
//            {
//                return
//                    new TextStyle(false, false, Color.White, Color.White, StyleManager.LargeFontSize,
//                                  FontManager.DefaultFontName);
//            }
//        }

//        #endregion

//        #region Constructors

//        public TextStyle(string styleInfo)
//        {
//            bold = false;
//            italic = false;
//            isShadowed = false;
//            applyHighlight = false;
//            ignoreBounds = false;

//            standardColor = Color.White;
//            highlightedColor = Color.LightBlue;
//            size = StyleManager.GetLabelSize(LabelSize.Large);
//            fontName = FontManager.DefaultFontName;
//            horizontalAlignment = Alignment.Left;
//            verticalAlignment = Alignment.Top;

//            ParseMarkup(styleInfo);
//            flags = BuildFlags(this);
//        }

//        public TextStyle(bool bold, bool italic, LabelSize size, Color standardColor) :
//            this(bold, italic, false,
//                 standardColor, standardColor,
//                 StyleManager.GetLabelSize(size), FontManager.DefaultFontName, Alignment.Left, Alignment.Top)
//        {
//        }

//        public TextStyle(bool bold, bool italic, bool isShadowed, Color standardColor,
//                         Color highlightedColor, int size, string fontName, Alignment horizontalAlignment,
//                         Alignment verticalAlignment)
//        {
//            this.bold = bold;
//            this.italic = italic;
//            this.isShadowed = isShadowed;
//            ignoreBounds = false;
//            applyHighlight = (standardColor != highlightedColor) ? true : false;
//            this.standardColor = standardColor;
//            this.highlightedColor = highlightedColor;
//            this.size = size;
//            this.fontName = fontName;
//            this.horizontalAlignment = horizontalAlignment;
//            this.verticalAlignment = verticalAlignment;

//            flags = BuildFlags(this);
//        }

//        public TextStyle(bool bold, bool italic, Color standardColor, Color highlightedColor, int size, string fontName)
//        {
//            this.bold = bold;
//            this.italic = italic;
//            this.standardColor = standardColor;
//            this.highlightedColor = highlightedColor;
//            this.size = size;
//            this.fontName = fontName;

//            applyHighlight = (standardColor == highlightedColor) ? false : true;
//            isShadowed = false;
//            ignoreBounds = false;

//            horizontalAlignment = Alignment.Left;
//            verticalAlignment = Alignment.Top;
//            BuildFlags(this);
//        }

//        #endregion

//        public static DrawTextFormat BuildFlags(TextStyle style)
//        {
//            DrawTextFormat flags;

//            if (style.IgnoreBounds)
//            {
//                flags = DrawTextFormat.NoClip;
//            }
//            else
//            {
//                flags = DrawTextFormat.ExpandTabs | DrawTextFormat.WordBreak;
//            }

//            switch (style.HorizontalAlignment)
//            {
//                case Alignment.Left:
//                    flags |= DrawTextFormat.Left;
//                    break;

//                case Alignment.Right:
//                    flags |= DrawTextFormat.Right;
//                    break;

//                case Alignment.Center:
//                    flags |= DrawTextFormat.Center;
//                    break;
//            }

//            switch (style.VerticalAlignment)
//            {
//                case Alignment.Top:
//                    flags |= DrawTextFormat.Top;
//                    break;

//                case Alignment.Center:
//                    flags |= DrawTextFormat.VerticalCenter;
//                    break;

//                case Alignment.Bottom:
//                    flags |= DrawTextFormat.Bottom;
//                    break;
//            }
//            return flags;
//        }

//        void ParseMarkup(string markup)
//        {
//            string[] commandList = markup.Split(',');

//            foreach (string s in commandList)
//            {
//                if (s.Length == 1)
//                    ParseSimpleCommand(s.ToLower());
//                else
//                    ParseComplexCommand(s);
//            }
//        }

//        void ParseSimpleCommand(string command)
//        {
//            switch (command)
//            {
//                case "b":
//                    bold = true;
//                    break;

//                case "i":
//                    italic = true;
//                    break;

//                case "s":
//                    isShadowed = true;
//                    break;

//                default:
//                    break;
//            }
//        }

//        void ParseComplexCommand(string command)
//        {
//            Regex regex = new Regex("(?<param>.+)=\"(?<value>.+)\"");
//            Match m = regex.Match(command);
//            string param = m.Groups["param"].Value.ToLower();
//            string value = m.Groups["value"].Value.ToLower();

//            switch (param)
//            {
//                case "color":
//                case "c":
//                    try
//                    {
//                        // Try parsing it as hex value
//                        standardColor =
//                            Color.FromArgb(Int32.Parse(value, NumberStyles.AllowHexSpecifier));
//                    }
//                    catch (FormatException)
//                    {
//                        // Try parsing it as a string value
//                        standardColor = Color.FromName(value);
//                    }
//                    break;

//                case "hover":
//                case "h":
//                    try
//                    {
//                        // Try parsing it as hex value
//                        highlightedColor =
//                            Color.FromArgb(Int32.Parse(value, NumberStyles.AllowHexSpecifier));
//                    }
//                    catch (FormatException ex)
//                    {
//                        // Try parsing it as a string value
//                        highlightedColor = Color.FromName(value);
//                    }
//                    finally
//                    {
//                        applyHighlight = true;
//                    }
//                    break;
//            }
//        }

//        public Font Font
//        {
//            get
//            {
//                return FontManager.CreateFont(fontName,
//                                              size,
//                                              bold ? FontWeight.Bold : FontWeight.Normal,
//                                              italic);
//            }
//        }
//    }
//}