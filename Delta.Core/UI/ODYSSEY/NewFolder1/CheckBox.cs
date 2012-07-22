//using System.Drawing;
//using System.Windows.Forms;
//using AvengersUTD.Odyssey.UserInterface.Style;
//using Microsoft.DirectX;

//namespace AvengersUTD.Odyssey.UserInterface.RenderableControls
//{
//    public class CheckBox : RenderableControl, ISpriteControl
//    {
//        public const int DefaultCheckBoxSize = 18;
//        public const int DefaultCheckBoxLabelOffset = 24;
//        public const int DefaultCheckOffset = 4;

//        protected static ColorArray defaultColors = new ColorArray(
//            Color.Transparent,
//            Color.Transparent,
//            Color.WhiteSmoke,
//            Color.Gray,
//            Color.DarkGray,
//            Color.Transparent,
//            Color.Silver,
//            Color.White
//            );


//        Label label;
//        Vector2 checkTopRightPosition, checkTopRightAbsolutePosition;
//        Vector2 checkBottomPosition, checkBottomPositionAbsolutePosition;
//        Vector2 checkTopLeftPosition, checkTopLeftAbsolutePosition;
//        ShapeDescriptor checkDescriptor;
//        ShapeDescriptor boxDescriptor;

//        #region Properties 

//        public Color LabelColor
//        {
//            get { return label.Color; }
//            set { label.Color = value; }
//        }

//        #endregion

//        public CheckBox(string id, string text, Vector2 position, Color labelColor) :
//            this(
//            id, text, position, labelColor, Shape.Custom,
//            Shapes.ShadeTopToBottom)
//        {
//        }

//        public CheckBox(string id, string text, Vector2 position, Color labelColor, Shape shape,
//                        Shapes.ShadingMode shadeMode) :
//                            base(id, position, Size.Empty, shape, BorderStyle.Raised, shadeMode, defaultColors)
//        {
//            Vector2 labelPosition = new Vector2(DefaultCheckBoxLabelOffset, 0);
//            label = new Label(id + "_Label",
//                              text,
//                              LabelSize.Small,
//                              Alignment.Left,
//                              Alignment.Top,
//                              labelPosition,
//                              labelColor);

//            // Compute individual size areas
//            Size checkBoxSize = new Size(DefaultCheckBoxSize, DefaultCheckBoxSize);
//            Size labelSize = label.Area.Size;

//            // Compute total size
//            size = new Size(checkBoxSize.Width + DefaultCheckOffset + labelSize.Width, checkBoxSize.Height);


//            checkTopLeftPosition = new Vector2(DefaultCheckOffset, DefaultCheckBoxSize/2);
//            checkBottomPosition = new Vector2(DefaultCheckBoxSize/2, DefaultCheckBoxSize - DefaultCheckOffset);
//            checkTopRightPosition = new Vector2(DefaultCheckBoxSize - DefaultCheckOffset, DefaultCheckOffset);

//            IsFocusable = false;
//        }

//        public override void CreateShape()
//        {
//            base.CreateShape();
//            boxDescriptor =
//                Shapes.DrawRectangularOutline(absolutePosition, new Size(DefaultCheckBoxSize, DefaultCheckBoxSize),
//                                              borderSize, borderColor, borderStyle, Border.All);
//            checkDescriptor =
//                Shapes.DrawPolyLine(borderSize, (isSelected ? colorArray.Selected : Color.Transparent), false,
//                                    checkTopLeftAbsolutePosition, checkBottomPositionAbsolutePosition,
//                                    checkTopRightAbsolutePosition);

//            boxDescriptor.Depth = depth;
//            checkDescriptor.Depth = Depth.AsChildOf(depth);

//            shapeDescriptors = new ShapeDescriptor[2];
//            shapeDescriptors[0] = boxDescriptor;
//            shapeDescriptors[1] = checkDescriptor;
//        }

//        public override void UpdateShape()
//        {
//            boxDescriptor.UpdateShape(
//                Shapes.DrawRectangularOutline(absolutePosition, new Size(DefaultCheckBoxSize, DefaultCheckBoxSize),
//                                              borderSize, borderColor, borderStyle, Border.All));
//            checkDescriptor.UpdateShape(
//                Shapes.DrawPolyLine(borderSize, (isSelected ? colorArray.Selected : Color.Transparent), false,
//                                    checkTopLeftAbsolutePosition, checkBottomPositionAbsolutePosition,
//                                    checkTopRightAbsolutePosition));
//            //shapeDescriptors[0] = boxDescriptor;
//            //shapeDescriptors[1] = checkDescriptor;

//            //boxDescriptor.Depth = new Depth(windowId, layer, zOrder);
//            //checkDescriptor.Depth = new Depth(windowId, layer, zOrder + 1);
//        }

//        public override void ComputeAbsolutePosition()
//        {
//            base.ComputeAbsolutePosition();
//            checkTopLeftAbsolutePosition = Vector2.Add(checkTopLeftPosition, absolutePosition);
//            checkTopRightAbsolutePosition = Vector2.Add(checkTopRightPosition, absolutePosition);
//            checkBottomPositionAbsolutePosition = Vector2.Add(checkBottomPosition, absolutePosition);


//            label.Parent = this;
//        }

//        public override bool IntersectTest(Point cursorLocation)
//        {
//            return Intersection.RectangleTest(absolutePosition, size, cursorLocation);
//        }

//        #region Exposed events

//        protected override void OnMouseClick(BaseControl ctl, MouseEventArgs args)
//        {
//            IsSelected = !IsSelected;
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