//using System;
//using AvengersUTD.Odyssey.UserInterface.RenderableControls;
//using AvengersUTD.Odyssey.UserInterface.Style;
//using Microsoft.DirectX.Direct3D;

//namespace AvengersUTD.Odyssey.UserInterface
//{
//    public struct Depth : IComparable<Depth>
//    {
//        int windowLayer;
//        int componentLayer;
//        int zOrder;

//        #region Properties

//        public int WindowLayer
//        {
//            get { return windowLayer; }
//            set { componentLayer = value; }
//        }

//        public int ComponentLayer
//        {
//            get { return componentLayer; }
//            set { componentLayer = value; }
//        }

//        public int ZOrder
//        {
//            get { return zOrder; }
//            set { zOrder = value; }
//        }

//        #endregion

//        /// <summary>
//        /// Creates depth information for the control or ShapeDescriptor.
//        /// </summary>
//        /// <param name="window">The window layer. A value of 0 means the background. The highest value represents the currently focused window.</param>
//        /// <param name="component">Inside a window there can be controls that have to overlap other ones (such as the expandable panel of the DropDownBox). Increase this value to render them correctly.</param>
//        /// <param name="zOrder">A value of 0 means that the control is in the background of the window layer. An higher value means that the control has to be drawn on top of the other ones.</param>
//        public Depth(int window, int component, int zOrder)
//        {
//            windowLayer = window;
//            componentLayer = component;
//            this.zOrder = zOrder;
//        }

//        /// <summary>
//        /// Creates Depth information assuming that the current object will be the child of the control whose depth is passed as a parameter.
//        /// </summary>
//        /// <param name="parentDepth">The parent depth information.</param>
//        public static Depth AsChildOf(Depth parentDepth)
//        {
//            return new Depth(parentDepth.windowLayer,
//                             parentDepth.componentLayer,
//                             parentDepth.zOrder + 1);
//        }

//        public void IncreaseComponentLayer()
//        {
//            componentLayer++;
//        }

//        public override string ToString()
//        {
//            return string.Format("{0}.{1}.{2}", windowLayer, componentLayer, zOrder);
//        }

//        #region IComparable<Depth> Members

//        public int CompareTo(Depth other)
//        {
//            if (windowLayer > other.windowLayer)
//                return +100;
//            else if (windowLayer < other.windowLayer)
//                return -100;
//            else if (componentLayer > other.componentLayer)
//                return +50;
//            else if (componentLayer < other.componentLayer)
//                return -50;
//            else
//                return (zOrder - other.zOrder);
//        }

//        #endregion
//    }


//    public class ShapeDescriptor : IComparable<ShapeDescriptor>
//    {
//        int numPrimitives;
//        int arrayOffset;
//        Depth depth;

//        CustomVertex.TransformedColored[] vertices;
//        int[] indices;

//        #region Properties

//        public int NumPrimitives
//        {
//            get { return numPrimitives; }
//            set { numPrimitives = value; }
//        }

//        public CustomVertex.TransformedColored[] Vertices
//        {
//            get { return vertices; }
//        }

//        public Depth Depth
//        {
//            get { return depth; }
//            set { depth = value; }
//        }


//        /// <summary>
//        /// Returns or set the array offset in the descripted VertexBuffer object
//        /// (ie which index the related control has in the vertexbuffer: useful
//        /// when updating it
//        /// </summary>
//        public int ArrayOffset
//        {
//            get { return arrayOffset; }
//            set { arrayOffset = value; }
//        }

//        public static ShapeDescriptor Empty
//        {
//            get { return new ShapeDescriptor(0, new CustomVertex.TransformedColored[0], new int[0]); }
//        }

//        public int[] Indices
//        {
//            get { return indices; }
//        }

//        #endregion

//        public ShapeDescriptor(int numPrimitives, CustomVertex.TransformedColored[] vertices, int[] indices)
//        {
//            arrayOffset = 0;
//            this.numPrimitives = numPrimitives;
//            this.vertices = vertices;
//            this.indices = indices;
//        }

//        public static ShapeDescriptor Join(params ShapeDescriptor[] descriptors)
//        {
//            int vbTotal = 0;
//            int ibTotal = 0;
//            int numPrimitives = 0;

//            int arrayOffset = descriptors[0].arrayOffset;

//            CustomVertex.TransformedColored[] vertices;
//            int[] indices;

//            foreach (ShapeDescriptor descriptor in descriptors)
//            {
//                vbTotal += descriptor.Vertices.Length;
//                ibTotal += descriptor.Indices.Length;
//                numPrimitives += descriptor.numPrimitives;
//            }

//            vertices = new CustomVertex.TransformedColored[vbTotal];
//            indices = new int[ibTotal];

//            int vbOffset = 0;
//            int ibOffset = 0;

//            foreach (ShapeDescriptor descriptor in descriptors)
//            {
//                Array.Copy(descriptor.Vertices, 0, vertices, vbOffset, descriptor.Vertices.Length);
//                //for (int i = 0; i < descriptor.Vertices.Length; i++)
//                //    vertices[i + vbOffset] = descriptor.Vertices[i];

//                for (int j = 0; j < descriptor.Indices.Length; j++)
//                    indices[j + ibOffset] = descriptor.Indices[j] + vbOffset;

//                vbOffset += descriptor.Vertices.Length;
//                ibOffset += descriptor.Indices.Length;
//            }

//            ShapeDescriptor newSDesc = new ShapeDescriptor(numPrimitives, vertices, indices);
//            newSDesc.arrayOffset = arrayOffset;
//            return newSDesc;
//        }

//        /// <summary>
//        /// Updates shape with the information provided by the new descriptor.
//        /// <remarks>It must have the same number of vertices as the old one. Use this to
//        /// change the position/color of the control. If the appearance must be changed
//        /// have it already precomputed and set its color to invisible and bring it in the
//        /// foreground when needed by changing the color</remarks>
//        /// </summary>
//        /// <param name="newShape"></param>
//        public void UpdateShape(ShapeDescriptor newShape)
//        {
//            vertices = newShape.vertices;
//            indices = newShape.Indices;
//            numPrimitives = newShape.numPrimitives;
//        }

//        public static ShapeDescriptor ComputeShape(RenderableControl ctl, Shape shape)
//        {
//            switch (shape)
//            {
//                case Shape.Rectangle:
//                    return Shapes.DrawFullRectangle(ctl.AbsolutePosition, ctl.Size, ctl.InnerAreaColor,
//                                                    ctl.ShadeMode, ctl.BorderSize, ctl.BorderColor,
//                                                    ctl.BorderStyle);

//                case Shape.Circle:
//                    ICircularControl circleCtl = ctl as ICircularControl;
//                    return
//                        Shapes.DrawFullCircle(circleCtl.CenterAbsolutePosition, circleCtl.Radius,
//                                              circleCtl.OutlineRadius,
//                                              circleCtl.Slices, ctl.BorderSize, ctl.InnerAreaColor,
//                                              ctl.BorderColor);

//                case Shape.LeftTrapezoidUpside:
//                    return Shapes.DrawFullLeftTrapezoid(
//                        ctl.AbsolutePosition, ctl.Size, TabPanel.DefaultTabTriangleWidth, true, ctl.InnerAreaColor,
//                        ctl.BorderSize, ctl.BorderColor, ctl.BorderStyle, Border.All & ~Border.Left, Border.All,
//                        ctl.ShadeMode);

//                case Shape.LeftTrapezoidDownside:
//                    return Shapes.DrawFullLeftTrapezoid(
//                        ctl.AbsolutePosition, ctl.Size, TabPanel.DefaultTabTriangleWidth, false, ctl.InnerAreaColor,
//                        ctl.BorderSize, ctl.BorderColor, ctl.BorderStyle, Border.All & ~Border.Left, Border.All,
//                        ctl.ShadeMode);

//                case Shape.RightTrapezoidUpside:
//                    return Shapes.DrawFullRightTrapezoid(
//                        ctl.AbsolutePosition, ctl.Size, TabPanel.DefaultTabTriangleWidth, true, ctl.InnerAreaColor,
//                        ctl.BorderSize, ctl.BorderColor, ctl.BorderStyle, Border.All & ~Border.Right, Border.All,
//                        ctl.ShadeMode);

//                case Shape.RightTrapezoidDownside:
//                    return Shapes.DrawFullRightTrapezoid(
//                        ctl.AbsolutePosition, ctl.Size, TabPanel.DefaultTabTriangleWidth, false, ctl.InnerAreaColor,
//                        ctl.BorderSize, ctl.BorderColor, ctl.BorderStyle, Border.All & ~Border.Right, Border.All,
//                        ctl.ShadeMode);

//                default:
//                    return Empty;
//            }
//        }

//        #region IComparable<ShapeDescriptor> Members

//        public int CompareTo(ShapeDescriptor other)
//        {
//            return depth.CompareTo(other.depth);
//        }

//        #endregion
//    }
//}