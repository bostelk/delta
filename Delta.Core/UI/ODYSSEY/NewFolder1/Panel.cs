//using System.Drawing;
//using AvengersUTD.Odyssey.UserInterface.Style;
//using Microsoft.DirectX;

//namespace AvengersUTD.Odyssey.UserInterface.RenderableControls
//{
//    public class Panel : ContainerControl, IContainer
//    {
//        protected static ColorArray defaultColors = new ColorArray(
//            Color.FromArgb(170, 0, 150, 255),
//            Color.MediumBlue,
//            Color.FromArgb(170, 0, 150, 255),
//            Color.Gray,
//            Color.BlueViolet,
//            Color.FromArgb(170, 0, 150, 255),
//            Color.Gray,
//            Color.Gray
//            );

//        #region Constructors

//        public Panel(string id, Vector2 location, Size size)
//            : this(id, location, size, Shape.Rectangle, BorderStyle.Raised, Shapes.ShadeTopToBottom, defaultColors)
//        {
//        }

//        public Panel(string id, Vector2 location, Size size, Shape shape, BorderStyle style)
//            : this(id, location, size, shape, style, Shapes.ShadeTopToBottom, defaultColors)
//        {
//        }

//        public Panel(string id, Vector2 location, Size size, Shape shape, BorderStyle style,
//                     Shapes.ShadingMode shadeMode, ColorArray colorArray)
//            : base(id, location, size, shape, style, shadeMode, colorArray)
//        {
//        }

//        #endregion

//        public override bool IntersectTest(Point cursorLocation)
//        {
//            return Intersection.RectangleTest(absolutePosition, size, cursorLocation);
//        }
//    }
//}