//using System.Drawing;
//using System.Windows.Forms;
//using Microsoft.DirectX;
//using Font=Microsoft.DirectX.Direct3D.Font;

//namespace AvengersUTD.Odyssey.UserInterface.RenderableControls
//{
//    public class Label : BaseControl, ISpriteControl
//    {
//        const int lineHeight = 10;
//        public static Color DefaultColor = Color.White;

//        protected Color color;
//        protected Color normal;
//        protected Color highlighted;

//        protected Rectangle area;

//        string text;

//        TextStyle style;
//        TextManager textManager;

//        #region Properties

//        public string Text
//        {
//            get { return text; }
//            set
//            {
//                text = value;
//                area = MeasureText(text, style);
//            }
//        }

//        public Rectangle Area
//        {
//            get { return area; }
//        }

///* Old Properties
// * 
//        public Alignment HorizontalAlignment
//        {
//            get { return hAlign; }
//            set
//            {
//                switch (hAlign)
//                {
//                    case Alignment.Left:
//                        flags ^= DrawTextFormat.Left;
//                        break;

//                    case Alignment.Right:
//                        flags ^= DrawTextFormat.Right;
//                        break;

//                    case Alignment.Center:
//                        flags ^= DrawTextFormat.Center;
//                        break;
//                }

//                hAlign = value;

//                switch (hAlign)
//                {
//                    case Alignment.Left:
//                        flags |= DrawTextFormat.Left;
//                        break;

//                    case Alignment.Right:
//                        flags |= DrawTextFormat.Right;
//                        break;

//                    case Alignment.Center:
//                        flags |= DrawTextFormat.Center;
//                        break;
//                }
//            }
//        }

//        public Alignment VerticalAlignment
//        {
//            get { return vAlign; }
//            set
//            {
//                switch (vAlign)
//                {
//                    case Alignment.Top:
//                        flags ^= DrawTextFormat.Top;
//                        break;

//                    case Alignment.Center:
//                        flags ^= DrawTextFormat.VerticalCenter;
//                        break;

//                    case Alignment.Bottom:
//                        flags ^= DrawTextFormat.Bottom;
//                        break;

//                }

//                vAlign = value;

//                switch (vAlign)
//                {
//                    case Alignment.Top:
//                        flags |= DrawTextFormat.Top;
//                        break;

//                    case Alignment.Center:
//                        flags |= DrawTextFormat.VerticalCenter;
//                        break;

//                    case Alignment.Bottom:
//                        flags |= DrawTextFormat.Bottom;
//                        break;
//                }
//            }
//        }

// * */

//        public Font Font
//        {
//            get { return textManager.TextFont; }
//            set { textManager.TextFont = value; }
//        }

//        public Color Color
//        {
//            get { return color; }
//            set { color = value; }
//        }

//        public TextStyle Style
//        {
//            get { return style; }
//        }

//        #endregion

//        #region Constructors

//        /// <summary>
//        /// Default Constructor. Creates a label object using the default font defined in
//        /// the <see>AvengersUTD.Odyssey.UserInterFace.FontManager</see> class.
//        /// </summary>
//        /// <param name="id">The id of the control.</param>
//        /// <param name="text">Text to be displayed.</param>
//        /// <param name="position">Absolute position of the control.</param>
//        /// <param name="color">Color of the text</param>
//        public Label(string id,
//                     string text,
//                     Vector2 position,
//                     Color color)
//            : this(id, text, position,
//                   new TextStyle(false, false, false,
//                                 color, color, StyleManager.NormalFontSize,
//                                 FontManager.DefaultFontName, Alignment.Left, Alignment.Top))
//        {
//        }

//        /// <summary>
//        /// Returns a label control, using the default Font defined in the
//        /// <see>AvengersUTD.Odyssey.UserInterFace.FontManager</see> class, but allows
//        /// the user the specify the font Size.
//        /// </summary>
//        /// <param name="id">The id of the control.</param>
//        /// <param name="text">Text to be displayed.</param>
//        /// <param name="horizontalAlign">Horizontal alignment.</param>
//        /// <param name="verticalAlign">Vertical alignment.</param>
//        /// <param name="position">Absolute position of the control.</param>
//        /// <param name="color">Color of the text</param>
//        public Label(string id,
//                     string text,
//                     Alignment horizontalAlign,
//                     Alignment verticalAlign,
//                     Vector2 position,
//                     Color color)
//            : this(id, text, position,
//                   new TextStyle(false, false, false,
//                                 color, color, StyleManager.NormalFontSize,
//                                 FontManager.DefaultFontName, horizontalAlign, verticalAlign))
//        {
//        }

//        /// <summary>
//        /// Returns a label control, using the default Font defined in the
//        /// <see>AvengersUTD.Odyssey.UserInterFace.FontManager</see> class, but allows
//        /// the user the specify the font Size.
//        /// </summary>
//        /// <param name="id">The id of the control.</param>
//        /// <param name="text">Text to be displayed.</param>
//        /// <param name="horizontalAlign">Horizontal alignment.</param>
//        /// <param name="verticalAlign">Vertical alignment.</param>
//        /// <param name="position">Absolute position of the control.</param>
//        /// <param name="color">Color of the text</param>
//        public Label(string id,
//                     string text,
//                     Alignment horizontalAlign,
//                     Alignment verticalAlign,
//                     Vector2 position,
//                     Color standard,
//                     Color highlighted)
//            : this(id, text, position,
//                   new TextStyle(false, false, false,
//                                 standard, highlighted, StyleManager.NormalFontSize,
//                                 FontManager.DefaultFontName, horizontalAlign, verticalAlign))
//        {
//        }

//        /// <summary>
//        /// Returns a label control, using the default Font defined in the
//        /// <see>AvengersUTD.Odyssey.UserInterFace.FontManager</see> class, but allows
//        /// the user the specify the font Size.
//        /// </summary>
//        /// <param name="id">The id of the control.</param>
//        /// <param name="text">Text to be displayed.</param>
//        /// <param name="fontSize">Font size.</param>
//        /// <param name="horizontalAlign">Horizontal alignment.</param>
//        /// <param name="verticalAlign">Vertical alignment.</param>
//        /// <param name="position">Absolute position of the control.</param>
//        /// <param name="color">Color of the text</param>
//        public Label(string id,
//                     string text,
//                     int fontSize,
//                     Alignment horizontalAlign,
//                     Alignment verticalAlign,
//                     Vector2 position,
//                     Color color)
//            : this(id, text, position,
//                   new TextStyle(false, false, false,
//                                 color, color, fontSize,
//                                 FontManager.DefaultFontName, horizontalAlign, verticalAlign))
//        {
//        }

//        /// <summary>
//        /// Returns a label control, using the default Font defined in the
//        /// <see>AvengersUTD.Odyssey.UserInterFace.FontManager</see> class, but allows
//        /// the user the specify the font Size.
//        /// </summary>
//        /// <param name="id">The id of the control.</param>
//        /// <param name="text">Text to be displayed.</param>
//        /// <param name="fontSize">Font size.</param>
//        /// <param name="horizontalAlign">Horizontal alignment.</param>
//        /// <param name="verticalAlign">Vertical alignment.</param>
//        /// <param name="position">Absolute position of the control.</param>
//        /// <param name="standard">Color of the text</param>
//        /// <param name="highlighted">Highlighted color of the text.</param>
//        public Label(string id,
//                     string text,
//                     int fontSize,
//                     Alignment horizontalAlign,
//                     Alignment verticalAlign,
//                     Vector2 position,
//                     Color standard,
//                     Color highlighted)
//            : this(id, text, position,
//                   new TextStyle(false, false, false,
//                                 standard, highlighted, fontSize,
//                                 FontManager.DefaultFontName, horizontalAlign, verticalAlign))
//        {
//        }

//        /// <summary>
//        /// Returns a label control, using the default Font with 
//        /// the size value defined in the <see>AvengersUTD.Odyssey.UserInterFace.FontManager</see> 
//        /// class.
//        /// </summary>
//        /// <param name="id">The id of the control.</param>
//        /// <param name="text">Text to be displayed.</param>
//        /// <param name="labelSize">Font size.</param>
//        /// <param name="horizontalAlign">Horizontal alignment.</param>
//        /// <param name="verticalAlign">Vertical alignment.</param>
//        /// <param name="position">Absolute position of the control.</param>
//        /// <param name="color">Color of the text</param>
//        public Label(string id,
//                     string text,
//                     LabelSize labelSize,
//                     Alignment horizontalAlign,
//                     Alignment verticalAlign,
//                     Vector2 position,
//                     Color color)
//            : this(id, text, position,
//                   new TextStyle(false, false, false,
//                                 color, color, StyleManager.GetLabelSize(labelSize),
//                                 FontManager.DefaultFontName, horizontalAlign, verticalAlign))
//        {
//        }

//        /// <summary>
//        /// Creates a label control using the user specified font information.
//        /// </summary>
//        /// <param name="id">The id of the control.</param>
//        /// <param name="text">Text to be displayed.</param>
//        /// <param name="labelSize">Font size.</param>
//        /// <param name="horizontalAlign">Horizontal alignment.</param>
//        /// <param name="verticalAlign">Vertical alignment.</param>
//        /// <param name="position">Absolute position of the control.</param>
//        /// <param name="normal">Color of the text.</param>
//        /// <param name="highlighted">Highlighted color of the text.</param>
//        /// <param name="italic">True if the font should be rendered in italic.</param>
//        /// <param name="fontName">Name of the font family</param>
//        public Label(string id,
//                     string text,
//                     LabelSize labelSize,
//                     Alignment horizontalAlign,
//                     Alignment verticalAlign,
//                     Vector2 position,
//                     Color normal,
//                     Color highlighted,
//                     string fontName,
//                     bool bold,
//                     bool italic
//            )
//            : this(id, text, position,
//                   new TextStyle(bold, italic, false, normal, highlighted, StyleManager.GetLabelSize(labelSize),
//                                 fontName, horizontalAlign, verticalAlign))
//        {
//        }


//        public Label(string id,
//                     string text,
//                     Vector2 position,
//                     TextStyle textStyle) : base(id, position, Size.Empty)
//        {
//            style = textStyle;
//            textManager = new TextManager(Style.Font, lineHeight);
//            Font = Style.Font;
//            area = Font.MeasureString(UI.CurrentHud.SpriteManager, text, style.Flags, normal);
//            size = area.Size;
//            this.text = text;

//            color = Style.StandardColor;
//        }

//        #endregion

//        public void Render()
//        {
//            if (!isVisible)
//                return;


//            textManager.SetSize(parent.Size);

//            if (style.IsShadowed)
//            {
//                textManager.SetForegroundColor(Color.Black);
//                textManager.SetInsertionPoint(Vector2.Add(absolutePosition, new Vector2(1, 1)));
//                textManager.DrawTextLine(text, style.Flags);
//            }

//            textManager.SetInsertionPoint(absolutePosition);
//            textManager.SetForegroundColor(color);

//            textManager.DrawTextLine(text, style.Flags);
//        }

//        #region Overriden inherited events

//        protected override void OnMouseEnter(BaseControl ctl, MouseEventArgs args)
//        {
//            base.OnMouseEnter(ctl, args);
//            if (style.ApplyHighlight)
//                color = Style.HighlightedColor;
//        }

//        protected override void OnMouseLeave(BaseControl ctl, MouseEventArgs args)
//        {
//            base.OnMouseLeave(ctl, args);
//            if (style.ApplyHighlight)
//                color = Style.StandardColor;
//        }

//        #endregion

//        #region IRenderable overriden methods

//        public override bool IntersectTest(Point cursorLocation)
//        {
//            if (style.ApplyHighlight)
//                return Intersection.RectangleTest(absolutePosition, size, cursorLocation);
//            else
//                return false;
//        }

//        /// <summary>
//        /// Overrides inherited method. It also computes the parent size to be able to place
//        /// the label when the user choose the alignment (Left/Center/Right). The parent control
//        /// then acts like a bounding box (for example: a button)
//        /// </summary>
//        public override void ComputeAbsolutePosition()
//        {
//            base.ComputeAbsolutePosition();
//            ////topLeftAbsolutePosition = Vector2.Add(absolutePosition,new Vector2(area.X, area.Y));
//            //if (UI.CurrentHud.DesignState)
//            //    depth = new Depth(depth.WindowLayer, parent.Depth.ComponentLayer, depth.ComponentLayer);
//        }

//        #endregion

//        public Rectangle MeasureStringIfIncreasedBy(char c)
//        {
//            return Font.MeasureString(UI.CurrentHud.SpriteManager, text + c, style.Flags, normal);
//        }

//        public Rectangle MeasureStringIfIncreasedBy(string addition)
//        {
//            return Font.MeasureString(UI.CurrentHud.SpriteManager, text + addition, style.Flags, normal);
//        }

//        public void SetHighlight(bool enabled)
//        {
//            if (enabled)
//                color = Style.HighlightedColor;
//            else
//                color = Style.StandardColor;
//        }

//        public static Rectangle MeasureText(string text, TextStyle style)
//        {
//            return style.Font.MeasureString(UI.CurrentHud.SpriteManager,
//                                            text, style.Flags, style.StandardColor);
//        }

//        public Label Clone(string newId)
//        {
//            return new Label(newId, text, position, Style);
//        }
//    }
//}