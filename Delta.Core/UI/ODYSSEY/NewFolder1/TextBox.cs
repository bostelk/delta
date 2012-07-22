//using System;
//using System.Drawing;
//using System.Windows.Forms;
//using AvengersUTD.Odyssey.UserInterface.Style;
//using Microsoft.DirectX;

//namespace AvengersUTD.Odyssey.UserInterface.RenderableControls
//{
//    [Flags]
//    public enum KeyType
//    {
//        None = 0,
//        Letter = 1,
//        Digit = 2,
//        LetterOrDigit = Letter | Digit,
//        Punctuation = 4,
//        Separator = 8,
//        Control = 16
//    }

//    public class TextBox : DefaultShapeControl, ISpriteControl
//    {
//        const int DefaultTextBoxLabelOffset = 4;

//        Label textLabel;
//        KeyType keyMask = KeyType.LetterOrDigit | KeyType.Punctuation | KeyType.Separator;
//        bool nonAcceptedKeyEntered = false;
//        bool acceptsReturn = false;
//        int textBoxInnerWidth;

//        protected static ColorArray defaultColors = new ColorArray(
//            Color.Transparent,
//            Color.Transparent,
//            Color.FromArgb(100, Color.DeepSkyBlue),
//            Color.Gray,
//            Color.White,
//            Color.FromArgb(100, Color.DeepSkyBlue),
//            Color.Gray,
//            Color.LightGray
//            );

//        #region Properties

//        public string Text
//        {
//            get { return textLabel.Text; }
//            set { textLabel.Text = value; }
//        }

//        public Color TextColor
//        {
//            get { return textLabel.Color; }
//            set { textLabel.Color = value; }
//        }

//        public bool AcceptsReturn
//        {
//            get { return acceptsReturn; }
//            set { acceptsReturn = value; }
//        }

//        #endregion

//        #region Constructors

//        public TextBox(string id, Vector2 position, Size size, int fontSize, Color labelColor)
//            : this(id, position, size, fontSize, labelColor, Shape.Rectangle, Shapes.ShadeNone, defaultColors)
//        {
//        }

//        public TextBox(string id, Vector2 position, Size size, int fontSize, Color labelColor, Shape shape,
//                       Shapes.ShadingMode shadeMode, ColorArray colorArray)
//            : base(id, position, size, shape, BorderStyle.Sunken, shadeMode, colorArray)
//        {
//            borderStyle = BorderStyle.Sunken;
//            textLabel = new Label(id + "_textLabel", string.Empty, fontSize, Alignment.Left, Alignment.Top,
//                                  new Vector2(DefaultTextBoxLabelOffset, DefaultTextBoxLabelOffset), labelColor);
//            textBoxInnerWidth = size.Width - 2*DefaultTextBoxLabelOffset;
//        }

//        #endregion

//        public override bool IntersectTest(Point cursorLocation)
//        {
//            return Intersection.RectangleTest(absolutePosition, size, cursorLocation);
//        }

//        KeyType GetKeyType(Char c)
//        {
//            KeyType keyType = KeyType.None;
//            if (Char.IsLetter(c))
//                keyType = KeyType.Letter;
//            else if (Char.IsDigit(c))
//                keyType = KeyType.Digit;
//            else if (Char.IsPunctuation(c))
//                keyType = KeyType.Punctuation;
//            else if (Char.IsSeparator(c))
//                keyType = KeyType.Separator;
//            else
//                keyType = KeyType.Control;

//            return keyType;
//        }

//        protected override void OnKeyDown(BaseControl ctl, KeyEventArgs args)
//        {
//            base.OnKeyDown(ctl, args);

//            switch (args.KeyCode)
//            {
//                case Keys.Back:
//                    if (textLabel.Text.Length == 0)
//                        return;

//                    textLabel.Text = textLabel.Text.Substring(0, textLabel.Text.Length - 1);
//                    nonAcceptedKeyEntered = true;
//                    break;

//                case Keys.Escape:
//                    nonAcceptedKeyEntered = true;
//                    OnLostFocus(this);
//                    break;

//                case Keys.Return:
//                    if (!acceptsReturn)
//                    {
//                        nonAcceptedKeyEntered = true;
//                        OnLostFocus(this);
//                    }
//                    break;

//                default:
//                    nonAcceptedKeyEntered = false;
//                    break;
//            }
//        }

//        protected override void OnKeyPress(BaseControl ctl, KeyPressEventArgs args)
//        {
//            base.OnKeyPress(ctl, args);

//            if (nonAcceptedKeyEntered)
//                return;
//            else
//            {
//                Char c = args.KeyChar;
//                KeyType type = GetKeyType(c);
//                if ((type & keyMask) == type && textLabel.MeasureStringIfIncreasedBy(c).Size.Width <= textBoxInnerWidth)
//                    textLabel.Text += c;
//            }
//        }

//        public void SetKeyMask(KeyType newKeyMask)
//        {
//            keyMask = newKeyMask;
//        }

//        public override void ComputeAbsolutePosition()
//        {
//            base.ComputeAbsolutePosition();
//            textLabel.Parent = this;
//        }

//        #region ISpriteControl Members

//        public void Render()
//        {
//            textLabel.Render();
//        }

//        #endregion
//    }
//}