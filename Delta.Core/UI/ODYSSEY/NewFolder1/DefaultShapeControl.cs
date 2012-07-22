//#region Using directives

//using System.Drawing;
//using AvengersUTD.Odyssey.UserInterface.Style;
//using Microsoft.DirectX;

//#endregion

//namespace AvengersUTD.Odyssey.UserInterface.RenderableControls
//{
//    public abstract class DefaultShapeControl : RenderableControl
//    {
//        protected ShapeDescriptor shapeDescriptor;

//        #region Constructors

//        public DefaultShapeControl(string id, Vector2 position, Size size, Shape shape,
//                                   BorderStyle style, Shapes.ShadingMode shadeMode, ColorArray colorArray)
//            : base(id, position, size, shape, style, shadeMode, colorArray)
//        {
//            shapeDescriptors = new ShapeDescriptor[1];
//        }

//        #endregion

//        public override void CreateShape()
//        {
//            base.CreateShape();
//            if (shape != Shape.Custom && shape != Shape.None)
//            {
//                shapeDescriptor = ShapeDescriptor.ComputeShape(this, shape);
//                shapeDescriptors[0] = shapeDescriptor;
//                shapeDescriptor.Depth = depth;
//            }
//        }

//        public override void UpdateShape()
//        {
//            if (shape != Shape.Custom && shape != Shape.None)
//            {
//                shapeDescriptor.UpdateShape(ShapeDescriptor.ComputeShape(this, shape));
//                shapeDescriptor.Depth = new Depth(depth.WindowLayer, depth.ComponentLayer, depth.ZOrder);
//            }
//        }

//        public override bool IntersectTest(Point cursorLocation)
//        {
//            switch (shape)
//            {
//                default:
//                case Shape.Rectangle:
//                    return Intersection.RectangleTest(absolutePosition, size, cursorLocation);

//                case Shape.Circle:
//                    return
//                        Intersection.CircleTest(absolutePosition, (this as ICircularControl).OutlineRadius,
//                                                cursorLocation);
//            }
//        }
//    }
//}