//#region Using directives

//using System.Drawing;
//using AvengersUTD.Odyssey.UserInterface.Style;
//using Microsoft.DirectX;
//using Font=Microsoft.DirectX.Direct3D.Font;

//#endregion

//namespace AvengersUTD.Odyssey.UserInterface.RenderableControls
//{
//    public class Cell : RenderableControl, ISpriteControl
//    {
//        const int DefaultPaddingLeft = 5;

//        protected CellStyle style;
//        protected Label textLabel;

//        protected ShapeDescriptor cellDescriptor;

//        protected static ColorArray defaultColors = new ColorArray(
//            Color.Transparent,
//            Color.Red,
//            Color.Blue,
//            Color.Gray,
//            Color.BlueViolet,
//            Color.Transparent,
//            Color.Gray,
//            Color.Gray
//            );

//        #region Properties

//        public string Text
//        {
//            get { return textLabel.Text; }
//            set { textLabel.Text = value; }
//        }

//        public bool IgnoreBounds
//        {
//            get { return textLabel.Style.IgnoreBounds; }
//            set { textLabel.Style.IgnoreBounds = value; }
//        }

//        public Color TextColor
//        {
//            get { return textLabel.Color; }
//            set { textLabel.Color = value; }
//        }

//        public Font Font
//        {
//            get { return textLabel.Font; }
//            set { textLabel.Font = value; }
//        }

//        public CellStyle Style
//        {
//            get { return style; }
//            set { style = value; }
//        }

//        #endregion

//        #region Constructors

//        public Cell(string id, Vector2 position, Size size)
//            : this(id, position, size, BorderStyle.None, CellStyle.Default, Shapes.ShadeNone, defaultColors)
//        {
//        }

//        public Cell(string id, Vector2 position, Size size, BorderStyle borderStyle,
//                    CellStyle cellStyle, Shapes.ShadingMode shadeMode, ColorArray colorArray)
//            : base(id, position, size, Shape.Rectangle, borderStyle, shadeMode, colorArray)
//        {
//            style = cellStyle;
//            textLabel = new Label(id + "_Label", string.Empty,
//                                  new Vector2(style.PaddingLeft, style.PaddingTop),
//                                  style.TextStyle);
//            textLabel.Parent = this;
//            applyStatusChanges = false;
//            shapeDescriptors = new ShapeDescriptor[1];
//        }

//        #endregion

//        public void Format(CellStyle cellStyle)
//        {
//            style = cellStyle;
//        }

//        public override void ComputeAbsolutePosition()
//        {
//            base.ComputeAbsolutePosition();
//            textLabel.ComputeAbsolutePosition();
//        }

//        #region IRenderable overriden controls

//        public override void CreateShape()
//        {
//            base.CreateShape();
//            cellDescriptor = Shapes.DrawFullRectangle(absolutePosition, size, innerAreaColor, shadingMode,
//                                                      borderSize, borderColor, borderStyle, style.Borders);
//            shapeDescriptors[0] = cellDescriptor;
//            cellDescriptor.Depth = depth;

//            base.CreateShape();
//        }

//        public override void UpdateShape()
//        {
//            cellDescriptor.UpdateShape(Shapes.DrawFullRectangle(absolutePosition, size, innerAreaColor, shadingMode,
//                                                                borderSize, borderColor, borderStyle, style.Borders));
//        }

//        public override bool IntersectTest(Point cursorLocation)
//        {
//            return Intersection.RectangleTest(absolutePosition, size, cursorLocation);
//        }

//        #endregion

//        #region ISpriteControl Members

//        public void Render()
//        {
//            textLabel.Render();
//        }

//        #endregion
//    }
//}