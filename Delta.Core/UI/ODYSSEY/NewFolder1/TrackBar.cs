//using System;
//using System.Drawing;
//using System.Windows.Forms;
//using AvengersUTD.Odyssey.UserInterface.Style;
//using Microsoft.DirectX;

//namespace AvengersUTD.Odyssey.UserInterface.RenderableControls
//{
//    public class TrackBar : RenderableControl
//    {
//        const int DefaultSliderWidth = 20;
//        const int DefaultBarHeight = 5;
//        const int DefaultTickWidth = 3;
//        const int DefaultTickHeight = 10;
//        const int DefaultTickOffset = 10;

//        protected static ColorArray defaultColors = new ColorArray(
//            Color.Silver,
//            Color.SlateGray,
//            Color.WhiteSmoke,
//            Color.Gray,
//            Color.BlueViolet,
//            Color.WhiteSmoke,
//            Color.Gray,
//            Color.Gray
//            );

//        Color barColor;
//        bool drag = false;
//        Size sliderSize;
//        Size barSize;
//        Size sensitiveArea;
//        Vector2 barPosition, barAbsolutePosition;
//        Vector2 sliderPosition, sliderAbsolutePosition;

//        ShapeDescriptor barDescriptor;
//        ShapeDescriptor sliderDescriptor;

//        //TopLeft Corner of the sensitive area when dragging.
//        Vector2 topLeft;

//        protected float value = 0f;
//        protected float minValue;
//        protected float maxValue;
//        protected float tickFrequency;
//        protected float pixelsPerTick;

//        #region Properties

//        public float Value
//        {
//            get { return value; }
//        }

//        #endregion

//        #region Exposed events

//        public event ControlEventHandler ValueChanged;

//        protected virtual void OnValueChanged(BaseControl ctl)
//        {
//            if (ValueChanged != null)
//                ValueChanged(ctl);
//        }

//        #endregion

//        public TrackBar(string id, Vector2 position, Size size)
//            : base(id, position, size, Shape.Custom, BorderStyle.Raised, Shapes.ShadeTopToBottom, defaultColors)
//        {
//            shape = Shape.Custom;

//            sliderSize = new Size(DefaultSliderWidth, size.Height);
//            barSize = new Size(size.Width + sliderSize.Width/2, DefaultBarHeight);
//            sliderPosition = new Vector2(- (sliderSize.Width/2), (barSize.Height - sliderSize.Height)/2);
//            barColor = colorArray[ColorIndex.BorderEnabled];
//            barPosition = new Vector2(- (sliderSize.Width/2), 0);

//            sensitiveArea = new Size(barSize.Width, size.Height);
//        }

//        public void SetValues(int minValue, int tickFrequency, int maxValue)
//        {
//            this.minValue = minValue;
//            this.tickFrequency = tickFrequency;
//            this.maxValue = maxValue;
//            pixelsPerTick = (sensitiveArea.Width - sliderSize.Width)/((maxValue - minValue)/tickFrequency);
//            //pixelsPerTick = barSize.Width/((maxValue - minValue)/tickFrequency);
//        }

//        public override bool IntersectTest(Point cursorLocation)
//        {
//            return Intersection.RectangleTest(topLeft, sensitiveArea, cursorLocation);
//        }

//        public override void CreateShape()
//        {
//            base.CreateShape();

//            barDescriptor = Shapes.DrawFullRectangle(
//                barAbsolutePosition, barSize, barColor,
//                Shapes.ShadeNone,
//                borderSize, colorArray.BorderEnabled, borderStyle);
//            sliderDescriptor = Shapes.DrawFullRectangle(
//                sliderAbsolutePosition, sliderSize, innerAreaColor,
//                Shapes.ShadeTopToBottom,
//                borderSize, borderColor, borderStyle);

//            //barDescriptor.Depth = new Depth(windowOrder, layer, zOrder);
//            barDescriptor.Depth = depth;
//            sliderDescriptor.Depth = Depth.AsChildOf(depth);

//            // Draw "Ticks"
//            //int tickCount =  (int)Math.Round(sensitiveArea.Width / pixelsPerTick);
//            int tickCount = 1 + (int) ((maxValue - minValue)/tickFrequency);
//            shapeDescriptors = new ShapeDescriptor[2 + tickCount];

//            shapeDescriptors[0] = sliderDescriptor;
//            // Position 1 is reserved for the slider descriptor
//            shapeDescriptors[1] = barDescriptor;

//            for (int i = 2; i < 2 + tickCount; i++)
//            {
//                shapeDescriptors[i] = Shapes.DrawLine(DefaultTickWidth, colorArray[ColorIndex.Disabled],
//                                                      new Vector2(
//                                                          topLeft.X + sliderSize.Width/2 + (i - 2)*pixelsPerTick,
//                                                          topLeft.Y + sensitiveArea.Height + DefaultTickOffset),
//                                                      new Vector2(
//                                                          topLeft.X + sliderSize.Width/2 + (i - 2)*pixelsPerTick,
//                                                          topLeft.Y + sensitiveArea.Height + DefaultTickOffset +
//                                                          DefaultTickHeight));
//                shapeDescriptors[i].Depth = depth;
//            }

//            //for (int i = 2; i < shapeDescriptors.Length; i++)
//        }

//        public override void UpdateShape()
//        {
//            sliderDescriptor.UpdateShape(Shapes.DrawFullRectangle(
//                                             sliderAbsolutePosition, sliderSize, innerAreaColor, Shapes.ShadeTopToBottom,
//                                             borderSize, borderColor, borderStyle));
//        }

//        #region Overriden inherited events

//        protected override void OnMouseDown(BaseControl ctl, MouseEventArgs args)
//        {
//            base.OnMouseDown(ctl, args);
//            if (Intersection.RectangleTest(sliderAbsolutePosition, sliderSize, args.Location))
//                drag = true;
//        }

//        protected override void OnMouseUp(BaseControl ctl, MouseEventArgs args)
//        {
//            base.OnMouseUp(ctl, args);
//            drag = false;
//        }

//        protected override void OnMouseClick(BaseControl ctl, MouseEventArgs args)
//        {
//            base.OnMouseClick(ctl, args);
//            float oldSliderValue = value;
//            int tickPosition;
//            if (!drag)
//            {
//                tickPosition = (int) Math.Floor((args.Location.X - topLeft.X)/pixelsPerTick);
//                sliderAbsolutePosition.X = topLeft.X + tickPosition*pixelsPerTick;
//                value = tickPosition*tickFrequency;
//            }

//            if (value == oldSliderValue)
//                return;
//            else
//                OnValueChanged(this);
//        }

//        protected override void OnMouseMove(BaseControl ctl, MouseEventArgs args)
//        {
//            base.OnMouseMove(ctl, args);

//            if (!drag)
//                return;
//            else
//            {
//                float oldSliderValue = value;
//                int tickPosition;
//                tickPosition = (int) Math.Floor((args.Location.X - topLeft.X)/pixelsPerTick);
//                sliderAbsolutePosition.X = topLeft.X + tickPosition*pixelsPerTick;
//                value = tickPosition*tickFrequency;

//                if (sliderAbsolutePosition.X < topLeft.X)
//                {
//                    sliderAbsolutePosition.X = topLeft.X;
//                    value = minValue;
//                }
//                if (sliderAbsolutePosition.X > absolutePosition.X + size.Width - sliderSize.Width)
//                {
//                    sliderAbsolutePosition.X = absolutePosition.X + size.Width - sliderSize.Width;
//                    value = maxValue;
//                }

//                if (value == oldSliderValue)
//                    return;
//                else
//                    OnValueChanged(this);

//                sliderPosition = Vector2.Subtract(sliderAbsolutePosition, absolutePosition);

//                Update();
//            }
//        }

//        #endregion

//        public override void ComputeAbsolutePosition()
//        {
//            base.ComputeAbsolutePosition();
//            topLeft = new Vector2(absolutePosition.X - (sliderSize.Width/2), absolutePosition.Y - size.Height/2);
//            sliderAbsolutePosition = Vector2.Add(sliderPosition, absolutePosition);
//            barAbsolutePosition = Vector2.Add(barPosition, absolutePosition);
//        }
//    }
//}