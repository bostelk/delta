//using System.Drawing;

//namespace AvengersUTD.Odyssey.UserInterface.Style
//{
//    public enum ColorIndex : int
//    {
//        Enabled = 0,
//        Highlighted = 1,
//        Clicked = 2,
//        Disabled = 3,
//        Selected = 4,
//        Focused = 5,
//        BorderEnabled = 6,
//        BorderHighlighted = 7
//    }

//    public class ColorArray
//    {
//        const int ColorCount = 8;
//        Color[] colorArray;

//        #region Properties

//        public static ColorArray Transparent
//        {
//            get
//            {
//                Color[] colors = new Color[ColorCount];
//                for (int i = 0; i < colors.Length; i++)
//                    colors[i] = Color.Transparent;

//                return new ColorArray(colors);
//            }
//        }

//        public Color Enabled
//        {
//            get { return this[ColorIndex.Enabled]; }
//            set { this[ColorIndex.Enabled] = value; }
//        }

//        public Color Highlighted
//        {
//            get { return this[ColorIndex.Highlighted]; }
//            set { this[ColorIndex.Highlighted] = value; }
//        }

//        public Color Clicked
//        {
//            get { return this[ColorIndex.Clicked]; }
//            set { this[ColorIndex.Clicked] = value; }
//        }

//        public Color Disabled
//        {
//            get { return this[ColorIndex.Disabled]; }
//            set { this[ColorIndex.Disabled] = value; }
//        }

//        public Color Focused
//        {
//            get { return this[ColorIndex.Focused]; }
//            set { this[ColorIndex.Focused] = value; }
//        }

//        public Color Selected
//        {
//            get { return this[ColorIndex.Selected]; }
//            set { this[ColorIndex.Selected] = value; }
//        }

//        public Color BorderEnabled
//        {
//            get { return this[ColorIndex.BorderEnabled]; }
//            set { this[ColorIndex.BorderEnabled] = value; }
//        }

//        public Color BorderHighlighted
//        {
//            get { return this[ColorIndex.BorderHighlighted]; }
//            set { this[ColorIndex.BorderHighlighted] = value; }
//        }

//        public Color this[ColorIndex colorIndex]
//        {
//            get { return colorArray[(int) colorIndex]; }
//            set { colorArray[(int) colorIndex] = value; }
//        }

//        #endregion

//        public ColorArray(Color[] colors)
//        {
//            colorArray = colors;
//        }

//        public ColorArray(Color enabled, Color highlighted, Color clicked, Color disabled, Color selected,
//                          Color focused, Color borderEnabled, Color borderHighlighted)
//        {
//            colorArray = new Color[ColorCount];
//            this[ColorIndex.Enabled] = enabled;
//            this[ColorIndex.Highlighted] = highlighted;
//            this[ColorIndex.Clicked] = clicked;
//            this[ColorIndex.Disabled] = disabled;
//            this[ColorIndex.Selected] = selected;
//            this[ColorIndex.Focused] = focused;
//            this[ColorIndex.BorderEnabled] = borderEnabled;
//            this[ColorIndex.BorderHighlighted] = borderHighlighted;
//        }
//    }
//}