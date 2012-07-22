//using System.Drawing;
//using System.Windows.Forms;
//using AvengersUTD.Odyssey.UserInterface.Style;
//using Microsoft.DirectX;

//namespace AvengersUTD.Odyssey.UserInterface.RenderableControls
//{
//    public enum CaptionButtonType
//    {
//        Close,
//        Help,
//        Maximize,
//        Minimize,
//        Restore
//    }

//    #region CaptioButton class

//    public class CaptionButton : RenderableControl
//    {
//        public const int DefaultCrossPaddingX = 10;
//        public const int DefaultCrossPaddingY = 7;
//        public const int DefaultCrossWidth = 14;
//        public const int DefaultCaptionButtonWidth = 60;
//        public const int DefaultCaptionButtonHeight = 28;

//        ShapeDescriptor buttonDescriptor;
//        ShapeDescriptor crossDescriptor;
//        Vector2 crossTopLeftPosition;
//        Vector2 crossTopLeftAbsolutePosition;

//        CaptionButtonType buttonType;

//        public CaptionButton(string id, Vector2 position, ColorArray colorArray, CaptionButtonType type)
//            : base(id, position, new Size(DefaultCaptionButtonWidth, DefaultCaptionButtonHeight),
//                   Shape.Custom, BorderStyle.Raised, Shapes.ShadeTopToBottom, colorArray)
//        {
//            shapeDescriptors = new ShapeDescriptor[2];
//            isFocusable = false;
//            crossTopLeftPosition = new Vector2(DefaultCaptionButtonWidth/2 - DefaultCrossWidth/2,
//                                               DefaultCrossPaddingY);

//            buttonType = type;
//        }

//        protected override void OnMouseUp(BaseControl ctl, MouseEventArgs args)
//        {
//            base.OnMouseUp(ctl, args);
//            Window owner = parent.Parent as Window;
         
//            owner.Close();
//        }

//        #region IRenderable Overriden Methods

//        public override void UpdateShape()
//        {
//            switch (buttonType)
//            {
//                default:
//                case CaptionButtonType.Close:
//                    buttonDescriptor.UpdateShape(ShapeDescriptor.ComputeShape(this, Shape.Rectangle));
//                    crossDescriptor.UpdateShape(ShapeDescriptor.Join(
//                                                    Shapes.DrawLine(4, Color.AntiqueWhite,
//                                                                    crossTopLeftAbsolutePosition,
//                                                                    crossTopLeftAbsolutePosition +
//                                                                    new Vector2(DefaultCrossWidth, DefaultCrossWidth)),
//                                                    Shapes.DrawLine(4, Color.AntiqueWhite,
//                                                                    crossTopLeftAbsolutePosition +
//                                                                    new Vector2(0, DefaultCrossWidth),
//                                                                    crossTopLeftAbsolutePosition +
//                                                                    new Vector2(DefaultCrossWidth, 0)
//                                                        ))
//                        );
//                    break;
//            }
//        }

//        public override void CreateShape()
//        {
//            base.CreateShape();

//            switch (buttonType)
//            {
//                default:
//                case CaptionButtonType.Close:
//                    buttonDescriptor = ShapeDescriptor.ComputeShape(this, Shape.Rectangle);
//                    crossDescriptor = ShapeDescriptor.Join(
//                        Shapes.DrawLine(4, Color.AntiqueWhite,
//                                        crossTopLeftAbsolutePosition,
//                                        crossTopLeftAbsolutePosition + new Vector2(DefaultCrossWidth, DefaultCrossWidth)),
//                        Shapes.DrawLine(4, Color.AntiqueWhite,
//                                        crossTopLeftAbsolutePosition + new Vector2(0, DefaultCrossWidth),
//                                        crossTopLeftAbsolutePosition + new Vector2(DefaultCrossWidth, 0)
//                            )
//                        );
//                    buttonDescriptor.Depth = depth;
//                    crossDescriptor.Depth = Depth.AsChildOf(depth);

//                    shapeDescriptors[0] = buttonDescriptor;
//                    shapeDescriptors[1] = crossDescriptor;
//                    break;
//            }
//        }


//        public override void ComputeAbsolutePosition()
//        {
//            base.ComputeAbsolutePosition();
//            crossTopLeftAbsolutePosition = Vector2.Add(absolutePosition, crossTopLeftPosition);
//        }

//        #endregion

//        public override bool IntersectTest(Point cursorLocation)
//        {
//            return Intersection.RectangleTest(absolutePosition, size, cursorLocation);
//        }
//    }

//    #endregion

//    /// <summary>
//    /// This control represents the bar that appears on the top of
//    /// windows and dialog panels. It should only be used by the
//    /// Window control.
//    /// </summary>
//    public class CaptionBar : ContainerControl, ISpriteControl
//    {
//        public const int DefaultBarHeight = 30;
//        const int DefaultTitlePaddingX = 8;
//        const int DefaultTitlePaddingY = 5;

//        Vector2 dragStartPosition;
//        bool drag;
//        Label titleLabel;
//        CaptionButton closeButton;

//        #region Default Colors

//        protected static ColorArray defaultColors = new ColorArray(
//            Color.DarkBlue,
//            Color.MediumBlue,
//            Color.FromArgb(170, 0, 150, 255),
//            Color.Gray,
//            Color.BlueViolet,
//            Color.FromArgb(170, 0, 150, 255),
//            Color.Gray,
//            Color.Gray
//            );

//        protected static ColorArray closeButtonDefaultColors = new ColorArray(
//            Color.Crimson,
//            Color.Red,
//            Color.Sienna,
//            Color.DarkRed,
//            Color.Yellow,
//            Color.Green,
//            Color.Gray,
//            Color.Gray); 
//        #endregion

//        #region Properties
//        internal Label TitleLabel
//        {
//            get { return titleLabel; }
//        }
//        internal CaptionButton CloseButton
//        {
//            get { return closeButton; }
//        }
//        #endregion


//        public CaptionBar(string id, Vector2 position, string title, int windowWidth) :
//            base(id, position, new Size(windowWidth, DefaultBarHeight), Shape.None,
//                 BorderStyle.None, Shapes.ShadeLeftToRight, defaultColors)
//        {
//            isFocusable = false;
//            //canRaiseEvents = false;
//            titleLabel = new Label(id + "_Title", title,
//                                   new Vector2(DefaultTitlePaddingX, DefaultTitlePaddingY),
//                                   TextStyle.Default);
//            titleLabel.Style.IsShadowed = true;
//            titleLabel.IsSubComponent = true;

//            closeButton = new CaptionButton(id + "_CloseButton",
//                                            new Vector2(
//                                                size.Width - CaptionButton.DefaultCaptionButtonWidth -
//                                                Window.DefaultBorderPadding, 1),
//                                            closeButtonDefaultColors, CaptionButtonType.Close);
//            closeButton.IsSubComponent = true;

//            Add(titleLabel);
//            Add(closeButton);
//        }

//        #region Overriden inherited events

//        protected override void OnMouseDown(BaseControl ctl, MouseEventArgs args)
//        {
//            base.OnMouseDown(ctl, args);
//            drag = true;
//            UI.CurrentHud.CaptureControl = this;
//            dragStartPosition = new Vector2(args.X, args.Y);
//        }

//        protected override void OnMouseMove(BaseControl ctl, MouseEventArgs ctlArgs)
//        {
//            base.OnMouseMove(ctl, ctlArgs);
//            if (drag)
//            {
//                Vector2 newDragPosition = new Vector2(ctlArgs.Location.X, ctlArgs.Location.Y);
//                Parent.Position += newDragPosition - dragStartPosition;
//                dragStartPosition = newDragPosition;
//                Update();
//            }
//            else
//                return;
//        }

//        protected override void OnMouseClick(BaseControl ctl, MouseEventArgs args)
//        {
//            base.OnMouseUp(ctl, args);
//            if (drag)
//            {
//                drag = false;
//                OnMouseCaptureChanged(ctl, args);
//            }
//        }

//        #endregion

//        #region ISpriteControl Members

//        public void Render()
//        {
//            titleLabel.Render();
//        }

//        #endregion
//    }
//}