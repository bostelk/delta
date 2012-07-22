//using System.Drawing;
//using System.Windows.Forms;
//using AvengersUTD.Odyssey.UserInterface.Style;
//using Microsoft.DirectX;

//namespace AvengersUTD.Odyssey.UserInterface.RenderableControls
//{
//    /// <summary>
//    /// This is the OptionButton control - also known as Radio Button. Instead of creating
//    /// several stand-alone OptionButton, instantiate a single OptionGroup control (which is
//    /// a set of OptionButton). To check whether an OptionButton is selected use the IsSelected 
//    /// property.
//    /// </summary>
//    public class OptionButton : DefaultShapeControl, ICircularControl, ISpriteControl
//    {
//        public const int DefaultOptionButtonLabelOffset = 26;
//        public const int DefaultOptionButtonSelectedRadius = 6;
//        public const int DefaultOptionButtonOutlineRadius = 10;
//        public const int DefaultOptionButtonSlices = 10;
//        public const int DefaultOptionButtonSize = 2;


//        protected static ColorArray defaultColors = new ColorArray(
//            Color.Transparent,
//            Color.WhiteSmoke,
//            Color.DeepSkyBlue,
//            Color.Gray,
//            Color.Silver,
//            Color.Silver,
//            Color.Silver,
//            Color.White
//            );

//        protected float radius, outlineRadius;
//        protected int slices;
//        protected int optionIndex;
//        protected Vector2 center, centerAbsolutePosition;
//        protected Alignment labelAlignment;
//        protected Label label;

//        #region Properties

//        public Label Label
//        {
//            get { return label; }
//        }

//        #endregion

//        public OptionButton(string id, int index, string text, Vector2 position, Size size)
//            : this(id, index, text, position, size, Shape.Circle, Shapes.ShadeNone)
//        {
//        }

//        public OptionButton(string id, int index, string text, Vector2 position, Size size, Shape shape,
//                            Shapes.ShadingMode shadeMode) :
//                                base(id, position, size,
//                                     shape, BorderStyle.Flat, shadeMode, defaultColors)
//        {
//            center = new Vector2(position.X + DefaultOptionButtonOutlineRadius,
//                                 position.Y + DefaultOptionButtonOutlineRadius);

//            Vector2 labelPosition = new Vector2(DefaultOptionButtonLabelOffset, position.Y
//                );

//            radius = DefaultOptionButtonSelectedRadius;
//            outlineRadius = DefaultOptionButtonOutlineRadius;
//            slices = DefaultOptionButtonSlices;
//            borderSize = DefaultOptionButtonSize;

//            optionIndex = index;

//            label = new Label(id + "_Label",
//                              text,
//                              LabelSize.Small,
//                              Alignment.Left,
//                              Alignment.Top,
//                              labelPosition,
//                              Label.DefaultColor);
//        }

//        public override bool IntersectTest(Point cursorLocation)
//        {
//            return Intersection.CircleTest(centerAbsolutePosition, outlineRadius, cursorLocation) ||
//                   Intersection.RectangleTest(label.AbsolutePosition, label.Area.Size, cursorLocation);
//        }

//        #region Exposed events

//        protected override void OnMouseClick(BaseControl ctl, MouseEventArgs args)
//        {
//            base.OnMouseClick(ctl, args);
//            OptionGroup optionGroup = parent as OptionGroup;
//            optionGroup.Select(optionIndex);
//        }

//        #endregion

//        public override void ComputeAbsolutePosition()
//        {
//            base.ComputeAbsolutePosition();
//            centerAbsolutePosition = Vector2.Add(absolutePosition, center);
//            label.Parent = this;
//        }

//        #region ICircularControl Members

//        public Vector2 Center
//        {
//            get { return center; }
//            set { center = value; }
//        }

//        public Vector2 CenterAbsolutePosition
//        {
//            get { return centerAbsolutePosition; }
//        }

//        public float Radius
//        {
//            get { return radius; }
//            set { radius = value; }
//        }

//        public float OutlineRadius
//        {
//            get { return outlineRadius; }
//            set { outlineRadius = value; }
//        }

//        public int Slices
//        {
//            get { return slices; }
//            set { slices = value; }
//        }

//        #endregion

//        #region ISpriteControl Members

//        public void Render()
//        {
//            label.Render();
//        }

//        #endregion
//    }
//}