//using System;
//using System.Drawing;
//using Microsoft.DirectX;
//using Microsoft.DirectX.Direct3D;

//namespace AvengersUTD.Odyssey.UserInterface.Style
//{
//    public static class Shapes
//    {
//        static float darkShadeFactor = 0.5f;
//        static float lightShadeFactor = 1.15f;

//        public delegate int[] ShadingMode(Color color);

//        #region ShadingMode methods

//        /// <summary>
//        /// Shades a rectangle top to bottom using the specified color.
//        /// </summary>
//        /// <param name="color">The color.</param>
//        /// <returns>An array of ints containing the color of each vertex (0: top left 1: top right 2: bottomLeft 3: bottom right</returns>
//        public static int[] ShadeTopToBottom(Color color)
//        {
//            int colorTopLeft = color.ToArgb();
//            int colorTopRight = colorTopLeft;
//            int colorBottomLeft = Color.FromArgb(color.A, ColorOperator.Scale(color, darkShadeFactor)).ToArgb();
//            int colorBottomRight = colorBottomLeft;

//            return new int[] {colorTopLeft, colorTopRight, colorBottomLeft, colorBottomRight};
//        }

//        /// <summary>
//        /// Shades a rectangle bottom to top using the specified color.
//        /// </summary>
//        /// <param name="color">The color.</param>
//        /// <returns>An array of ints containing the color of each vertex (0: top left 1: top right 2: bottomLeft 3: bottom right</returns>
//        public static int[] ShadeBottomToTop(Color color)
//        {
//            int colorTopLeft = Color.FromArgb(color.A, ColorOperator.Scale(color, darkShadeFactor)).ToArgb();
//            int colorTopRight = colorTopLeft;
//            int colorBottomLeft = color.ToArgb();
//            int colorBottomRight = colorBottomLeft;

//            return new int[] {colorTopLeft, colorTopRight, colorBottomLeft, colorBottomRight};
//        }

//        /// <summary>
//        /// Shades a rectangle left to right using the specified color.
//        /// </summary>
//        /// <param name="color">The color.</param>
//        /// <returns>An array of ints containing the color of each vertex (0: top left 1: top right 2: bottomLeft 3: bottom right</returns>
//        public static int[] ShadeLeftToRight(Color color)
//        {
//            int colorTopLeft = color.ToArgb();
//            int colorTopRight = Color.FromArgb(color.A, ColorOperator.Scale(color, darkShadeFactor)).ToArgb();
//            int colorBottomLeft = colorTopLeft;
//            int colorBottomRight = colorTopRight;

//            return new int[] {colorTopLeft, colorTopRight, colorBottomLeft, colorBottomRight};
//        }

//        /// <summary>
//        /// Shades a rectangle right to left using the specified color.
//        /// </summary>
//        /// <param name="color">The color.</param>
//        /// <returns>An array of ints containing the color of each vertex (0: top left 1: top right 2: bottomLeft 3: bottom right</returns>
//        public static int[] ShadeRightToLeft(Color color)
//        {
//            int colorTopLeft = Color.FromArgb(color.A, ColorOperator.Scale(color, darkShadeFactor)).ToArgb();
//            int colorTopRight = color.ToArgb();
//            int colorBottomLeft = colorTopLeft;
//            int colorBottomRight = colorTopRight;

//            return new int[] {colorTopLeft, colorTopRight, colorBottomLeft, colorBottomRight};
//        }


//        /// <summary>
//        /// It assigns the same color for each of the four vertex.
//        /// </summary>
//        /// <param name="color">The color.</param>
//        /// <returns>An array of ints containing the color of each vertex (0: top left 1: top right 2: bottomLeft 3: bottom right</returns>
//        public static int[] ShadeNone(Color color)
//        {
//            int colorToArgb = color.ToArgb();
//            return new int[] {colorToArgb, colorToArgb, colorToArgb, colorToArgb};
//        }

//        #endregion

//        #region Circle

//        /// <summary>
//        /// Draws a circle with an outline.
//        /// </summary>
//        /// <param name="center">The center.</param>
//        /// <param name="radius">The radius.</param>
//        /// <param name="outlineRadius">The outline radius.</param>
//        /// <param name="slices">The slices.</param>
//        /// <param name="borderSize">Size of the border.</param>
//        /// <param name="color">The color.</param>
//        /// <param name="borderColor">Color of the border.</param>
//        /// <returns>A full Circular ShapeDescriptor object.</returns>
//        public static ShapeDescriptor DrawFullCircle(Vector2 center, float radius, float outlineRadius,
//                                                     int slices, int borderSize, Color color, Color borderColor)
//        {
//            return ShapeDescriptor.Join(
//                DrawCircle(center, radius, slices, color),
//                DrawCircularOutline(center, outlineRadius, slices, borderSize, borderColor));
//        }

//        /// <summary>
//        /// Draws a circle without an outline.
//        /// </summary>
//        /// <param name="center">The center.</param>
//        /// <param name="radius">The radius.</param>
//        /// <param name="slices">The slices.</param>
//        /// <param name="color">The color.</param>
//        /// <returns>A Circular ShapeDescriptor object.</returns>
//        public static ShapeDescriptor DrawCircle(Vector2 center, float radius, int slices, Color color)
//        {
//            CustomVertex.TransformedColored[] vertices = new CustomVertex.TransformedColored[slices + 2];
//            int[] indices = new int[slices*3];
//            int col1;
//            float x, y;
//            x = center.X;
//            y = center.Y;
//            col1 = color.ToArgb();

//            float deltaRad = Geometry.DegreeToRadian(360)/slices;
//            float delta = 0;

//            vertices[0] = new CustomVertex.TransformedColored(x, y, 0, 1, col1);

//            for (int i = 1; i < slices + 2; i++)
//            {
//                vertices[i] = new CustomVertex.TransformedColored(
//                    (float) Math.Cos(delta)*radius + x,
//                    (float) Math.Sin(delta)*radius + y,
//                    0, 1, col1);
//                delta += deltaRad;
//            }

//            indices[0] = 0;
//            indices[1] = 1;

//            for (int i = 0; i < slices; i++)
//            {
//                indices[3*i] = 0;
//                indices[(3*i) + 1] = i + 1;
//                indices[(3*i) + 2] = i + 2;
//            }
//            return new ShapeDescriptor(slices, vertices, indices);
//        }

//        /// <summary>
//        /// Draws a circular outline.
//        /// </summary>
//        /// <param name="center">The center.</param>
//        /// <param name="radius">The radius.</param>
//        /// <param name="slices">The slices.</param>
//        /// <param name="width">The width.</param>
//        /// <param name="color">The color.</param>
//        /// <returns>A Circular outline ShapeDescriptor object.</returns>
//        public static ShapeDescriptor DrawCircularOutline(Vector2 center, float radius, int slices, int width,
//                                                          Color color)
//        {
//            Vector2[] points = new Vector2[slices];
//            float deltaRad = Geometry.DegreeToRadian(360)/slices;
//            float delta = 0;

//            for (int i = 0; i < slices; i++)
//            {
//                points[i] = new Vector2(
//                    (float) Math.Cos(delta)*radius + center.X,
//                    (float) Math.Sin(delta)*radius + center.Y);

//                delta += deltaRad;
//            }

//            return DrawPolyLine(width, color, true, points);
//        }

//        #endregion

//        #region Ellipse

//        public static ShapeDescriptor DrawEllipse(
//            Vector2 center, int radius1, int radius2, int slices, Color color)
//        {
//            return
//                DrawEllipse(center, radius1, radius2, Geometry.DegreeToRadian(0), Geometry.DegreeToRadian(360), slices,
//                            color);
//        }

//        public static ShapeDescriptor DrawEllipse(Vector2 center, int radius1, int radius2, float radFrom, float radTo,
//                                                  int slices, Color color)
//        {
//            CustomVertex.TransformedColored[] vertices = new CustomVertex.TransformedColored[slices + 2];
//            int[] indices = new int[slices*3];
//            int col1;
//            float x, y;
//            x = center.X;
//            y = center.Y;
//            col1 = color.ToArgb();

//            float deltaRad = radTo/slices;
//            float delta = radFrom;

//            vertices[0] = new CustomVertex.TransformedColored(x, y, 0, 1, col1);

//            for (int i = 1; i < slices + 2; i++)
//            {
//                vertices[i] = new CustomVertex.TransformedColored(
//                    (float) Math.Cos(delta)*radius1 + x,
//                    (float) Math.Sin(delta)*radius2 + y,
//                    0, 1, col1);
//                delta -= deltaRad;
//            }

//            indices[0] = 0;
//            indices[1] = 1;

//            for (int i = 0; i < slices; i++)
//            {
//                indices[3*i] = 0;
//                indices[(3*i) + 1] = i + 2;
//                indices[(3*i) + 2] = i + 1;
//            }
//            return new ShapeDescriptor(slices, vertices, indices);
//        }

//        #endregion

//        #region Lines

//        public static ShapeDescriptor DrawLine(float width, Color color, Vector2 v1, Vector2 v2)
//        {
//            CustomVertex.TransformedColored[] vertices = new CustomVertex.TransformedColored[4];
//            int col1 = color.ToArgb();

//            Vector2 vDir = (v1 - v2);
//            vDir = new Vector2(-vDir.Y, vDir.X);
//            vDir.Normalize();
//            width /= 2;

//            Vector2 vTopLeft = v1 + (-width*vDir);
//            Vector2 vTopRight = v1 + (width*vDir);
//            Vector2 vBottomLeft = v2 + (-width*vDir);
//            Vector2 vBottomRight = v2 + (width*vDir);
//            vertices[0] = new CustomVertex.TransformedColored(vTopLeft.X, vTopLeft.Y, 0, 1, col1);
//            vertices[1] = new CustomVertex.TransformedColored(vBottomLeft.X, vBottomLeft.Y, 0, 1, col1);
//            vertices[2] = new CustomVertex.TransformedColored(vBottomRight.X, vBottomRight.Y, 0, 1, col1);
//            vertices[3] = new CustomVertex.TransformedColored(vTopRight.X, vTopRight.Y, 0, 1, col1);

//            int[] indices = new int[6];

//            indices[0] = 0;
//            indices[1] = 2;
//            indices[2] = 1;
//            indices[3] = 2;
//            indices[4] = 0;
//            indices[5] = 3;

//            return new ShapeDescriptor(2, vertices, indices);
//        }

//        public static ShapeDescriptor DrawPolyLine(int width, Color color, bool closed, params Vector2[] points)
//        {
//            CustomVertex.TransformedColored[] vertices = new CustomVertex.TransformedColored[4];
//            ShapeDescriptor[] segments;

//            int col1 = color.ToArgb();

//            if (closed)
//            {
//                segments = new ShapeDescriptor[points.Length];
//                for (int i = 0; i < points.Length - 1; i++)
//                    segments[i] = DrawLine(width, color, points[i], points[i + 1]);
//                segments[points.Length - 1] = DrawLine(width, color, points[points.Length - 1], points[0]);
//            }
//            else
//            {
//                segments = new ShapeDescriptor[points.Length - 1];
//                for (int i = 0; i < points.Length - 1; i++)
//                    segments[i] = DrawLine(width, color, points[i], points[i + 1]);
//            }
//            return ShapeDescriptor.Join(segments);
//        }

//        #endregion

//        #region Rectangle

//        public static ShapeDescriptor DrawRectangle(Vector2 topLeft, Size size, Color color)
//        {
//            return DrawRectangle(topLeft, size, color, ShadeNone);
//        }

//        public static ShapeDescriptor DrawFullRectangle(Vector2 position, Size size, Color color, ShadingMode shadeMode,
//                                                        int borderSize, Color borderColor, BorderStyle style)
//        {
//            return DrawFullRectangle(position, size, color, shadeMode, borderSize, borderColor, style,
//                                     (style != BorderStyle.None) ? Border.All : Border.None);
//        }

//        public static ShapeDescriptor DrawFullRectangle(Vector2 position, Size size, Color color, ShadingMode shadeMode,
//                                                        int borderSize, Color borderColor, BorderStyle style,
//                                                        Border borders)
//        {
//            return ShapeDescriptor.Join(
//                DrawRectangle(new Vector2(position.X + borderSize, position.Y + borderSize),
//                              new Size(size.Width - borderSize*2, size.Height - borderSize*2),
//                              color,
//                              shadeMode),
//                DrawRectangularOutline(position, size, borderSize, borderColor, style, borders));
//        }

//        public static ShapeDescriptor DrawRectangle(Vector2 topLeft, Size size, Color color, ShadingMode shadeMode)
//        {
//            CustomVertex.TransformedColored[] vertices = new CustomVertex.TransformedColored[4];

//            int width = size.Width;
//            int height = size.Height;

//            int[] colors = shadeMode(color);

//            int colorTopLeft = colors[0];
//            int colorTopRight = colors[1];
//            int colorBottomleft = colors[2];
//            int colorBottomRight = colors[3];

//            vertices[0] = new CustomVertex.TransformedColored(topLeft.X, topLeft.Y, 0, 1, colorTopLeft);
//            vertices[1] = new CustomVertex.TransformedColored(topLeft.X, topLeft.Y + size.Height, 0, 1, colorBottomleft);
//            vertices[2] =
//                new CustomVertex.TransformedColored(topLeft.X + size.Width, topLeft.Y + size.Height, 0, 1,
//                                                    colorBottomRight);
//            vertices[3] = new CustomVertex.TransformedColored(topLeft.X + size.Width, topLeft.Y, 0, 1, colorTopRight);

//            int[] indices = new int[6];

//            indices[0] = 0;
//            indices[1] = 3;
//            indices[2] = 2;
//            indices[3] = 0;
//            indices[4] = 2;
//            indices[5] = 1;

//            return new ShapeDescriptor(2, vertices, indices);
//        }

//        public static ShapeDescriptor DrawRectangularOutline(Vector2 position,
//                                                             Size size, int borderSize, Color borderColor,
//                                                             BorderStyle style, Border borders)
//        {
//            switch (style)
//            {
//                case BorderStyle.Raised:
//                    return DrawRectangularOutline(position, size,
//                                                  Color.FromArgb(255, ColorOperator.Scale(borderColor, lightShadeFactor)),
//                                                  Color.FromArgb(255, ColorOperator.Scale(borderColor, darkShadeFactor)),
//                                                  borderSize, borders);

//                case BorderStyle.Sunken:
//                    return DrawRectangularOutline(position, size,
//                                                  Color.FromArgb(255, ColorOperator.Scale(borderColor, darkShadeFactor)),
//                                                  Color.FromArgb(255, ColorOperator.Scale(borderColor, lightShadeFactor)),
//                                                  borderSize, borders);

//                case BorderStyle.Flat:
//                default:
//                    return DrawRectangularOutline(position, size,
//                                                  borderColor, borderColor,
//                                                  borderSize, borders);
//            }
//        }

//        public static ShapeDescriptor DrawRectangularOutline(Vector2 position,
//                                                             Size size, Color borderTopAndLeft,
//                                                             Color borderBottomAndRight, int borderSize,
//                                                             Border borders)
//        {
//            ShapeDescriptor vBorderTop = ShapeDescriptor.Empty;
//            ShapeDescriptor vBorderSideL = ShapeDescriptor.Empty;
//            ShapeDescriptor vBorderSideR = ShapeDescriptor.Empty;
//            ShapeDescriptor vBorderBottom = ShapeDescriptor.Empty;

//            Vector2 innerPositionTopLeft = new Vector2(
//                position.X + borderSize, position.Y + borderSize);

//            Vector2 borderPositionTopRight = new Vector2(
//                position.X + size.Width - borderSize, position.Y);

//            Vector2 borderPositionBottomLeft = new Vector2(
//                position.X, position.Y + size.Height - borderSize);

//            Size borderTop = new Size(size.Width, borderSize);
//            Size borderSide = new Size(borderSize, size.Height);

//            if ((borders & Border.Top) != 0)
//                vBorderTop = DrawRectangle(
//                    position, borderTop, borderTopAndLeft);
//            if ((borders & Border.Left) != 0)
//                vBorderSideL = DrawRectangle(
//                    position, borderSide, borderTopAndLeft);
//            if ((borders & Border.Right) != 0)
//                vBorderSideR = DrawRectangle(
//                    borderPositionTopRight, borderSide, borderBottomAndRight);
//            if ((borders & Border.Bottom) != 0)
//                vBorderBottom = DrawRectangle(
//                    borderPositionBottomLeft, borderTop, borderBottomAndRight);

//            return ShapeDescriptor.Join(vBorderTop, vBorderSideL, vBorderSideR, vBorderBottom);
//        }

//        #endregion

//        #region Trapezoids

//        /// <summary>
//        /// Draws a left trapezoidal outline.
//        /// </summary>
//        /// <param name="position">The position.</param>
//        /// <param name="size">The size.</param>
//        /// <param name="triangleWidth">Width of the triangle.</param>
//        /// <param name="isTriangleUpside">Ff set to <c>true</c> the triangle will be upside.</param>
//        /// <param name="color">The color.</param>
//        /// <param name="borderSize">Size of the border.</param>
//        /// <param name="borderColor">Color of the border.</param>
//        /// <param name="style">The style.</param>
//        /// <param name="rectangleBorders">The rectangle borders.</param>
//        /// <param name="triangleBorders">The triangle borders.</param>
//        /// <returns>A left trapezoidal outline ShapeDescriptor object.</returns>
//        public static ShapeDescriptor DrawLeftTrapezoidalOutline(Vector2 position, Size size,
//                                                                 int triangleWidth, bool isTriangleUpside, Color color,
//                                                                 int borderSize, Color borderColor,
//                                                                 BorderStyle style, Border rectangleBorders,
//                                                                 Border triangleBorders)
//        {
//            Vector2 topLeft = new Vector2(position.X + triangleWidth, position.Y);
//            Size innerSize = new Size(size.Width - borderSize, size.Height - borderSize);

//            ShapeDescriptor sTriangleSide = ShapeDescriptor.Empty;
//            ShapeDescriptor sTriangleBase = ShapeDescriptor.Empty;
//            ShapeDescriptor sRectangleOutline =
//                DrawRectangularOutline(topLeft, size, borderSize, borderColor, style,
//                                       rectangleBorders);

//            if (isTriangleUpside)
//            {
//                if ((triangleBorders & Border.Left) == Border.Left)
//                    sTriangleSide = DrawLine(borderSize,
//                                             Color.FromArgb(255, ColorOperator.Scale(borderColor, lightShadeFactor)),
//                                             new Vector2(topLeft.X - triangleWidth, topLeft.Y + size.Height - borderSize),
//                                             new Vector2(topLeft.X, topLeft.Y + borderSize));

//                if ((triangleBorders & Border.Bottom) == Border.Bottom)
//                    sTriangleBase = DrawRectangle(
//                        new Vector2(topLeft.X - triangleWidth, topLeft.Y + size.Height - borderSize),
//                        new Size(triangleWidth, borderSize),
//                        Color.FromArgb(255, ColorOperator.Scale(borderColor, darkShadeFactor)));
//            }
//            else
//            {
//                if ((triangleBorders & Border.Left) == Border.Left)
//                    sTriangleSide = DrawLine(borderSize,
//                                             Color.FromArgb(255, ColorOperator.Scale(borderColor, darkShadeFactor)),
//                                             new Vector2(topLeft.X - triangleWidth + borderSize,
//                                                         topLeft.Y + (borderSize/2)),
//                                             new Vector2(topLeft.X + borderSize,
//                                                         topLeft.Y + innerSize.Height + (borderSize/2)));


//                if ((triangleBorders & Border.Top) == Border.Top)
//                    sTriangleBase = DrawRectangle(
//                        new Vector2(topLeft.X - triangleWidth, topLeft.Y),
//                        new Size(triangleWidth, borderSize),
//                        Color.FromArgb(255, ColorOperator.Scale(borderColor, lightShadeFactor)));
//            }

//            return ShapeDescriptor.Join(sRectangleOutline, sTriangleSide, sTriangleBase);
//        }


//        /// <summary>
//        /// This methods draws a trapezoid whose 90° angle is in the left side.
//        /// </summary>
//        /// <param name="position">The topLeft point of the trapezoid</param>
//        /// <param name="size">The <b>whole size</b> of the trapezoid</param>
//        /// <param name="triangleWidth">The extra width of the greater base side</param>
//        /// <param name="isTriangleUpside">If the triangle part is to be drawn upside</param>
//        /// <param name="isShaded">If shading is to be applied to the trapezoid polygons</param>
//        /// <param name="color">Color of the trapezoid's inner area. The specified ShadeVertices 
//        /// delegate will then proceed to create a shaded version.</param>
//        /// <param name="shadeMode">ShadeVertices delegate used.</param>
//        /// <returns>A Trapezoidal ShapeDescriptor object.</returns>
//        public static ShapeDescriptor DrawLeftTrapezoid(Vector2 position, Size size,
//                                                        int triangleWidth, bool isTriangleUpside,
//                                                        Color color, int borderSize,
//                                                        ShadingMode shadeMode)
//        {
//            CustomVertex.TransformedColored[] vertices = new CustomVertex.TransformedColored[5];
//            int[] indices = new int[9];
//            Vector2 topLeft = new Vector2(position.X + triangleWidth, position.Y);

//            Size innerSize = new Size(size.Width - borderSize, size.Height - borderSize);

//            ShapeDescriptor sTrapezoid;

//            int width = innerSize.Width;
//            int height = innerSize.Height;

//            int[] colors = shadeMode(color);

//            int colorTopLeft = colors[0];
//            int colorTopRight = colors[1];
//            int colorBottomleft = colors[2];
//            int colorBottomRight = colors[3];

//            vertices[0] =
//                new CustomVertex.TransformedColored(topLeft.X + innerSize.Width, topLeft.Y, 0, 1, colorTopRight);
//            vertices[1] =
//                new CustomVertex.TransformedColored(topLeft.X + innerSize.Width, topLeft.Y + innerSize.Height, 0, 1,
//                                                    colorBottomRight);
//            vertices[2] =
//                new CustomVertex.TransformedColored(topLeft.X, topLeft.Y + innerSize.Height, 0, 1, colorBottomleft);
//            vertices[4] = new CustomVertex.TransformedColored(topLeft.X, topLeft.Y, 0, 1, colorTopLeft);


//            if (isTriangleUpside)
//                vertices[3] =
//                    new CustomVertex.TransformedColored(topLeft.X - triangleWidth, topLeft.Y + innerSize.Height, 0, 1,
//                                                        colorBottomleft);
//            else
//                vertices[3] =
//                    new CustomVertex.TransformedColored(topLeft.X - triangleWidth, topLeft.Y, 0, 1, colorTopLeft);

//            indices[0] = 0;
//            indices[1] = 2;
//            indices[2] = 4;
//            indices[3] = 0;
//            indices[4] = 1;
//            indices[5] = 2;
//            indices[6] = 3;
//            indices[7] = 4;
//            indices[8] = 2;


//            sTrapezoid = new ShapeDescriptor(3, vertices, indices);
//            return sTrapezoid;
//        }

//        /// <summary>
//        /// This methods draws a trapezoid whose 90° angle is in the left side.
//        /// </summary>
//        /// <param name="position">The topLeft point of the trapezoid</param>
//        /// <param name="size">The <b>whole size</b> of the trapezoid</param>
//        /// <param name="triangleWidth">The extra width of the greater base side</param>
//        /// <param name="isTriangleUpside">If the triangle part is to be drawn upside</param>
//        /// <param name="isShaded">If shading is to be applied to the trapezoid polygons</param>
//        /// <param name="color">Color of the trapezoid's inner area. The specified ShadeVertices 
//        /// delegate will then proceed to create a shaded version.</param>
//        /// <param name="borderSize">Size in pixels of the border.</param>
//        /// <param name="borderColor">Average color of the border.</param>
//        /// <param name="rectangleBorders">Which borders of the rectangular part of the outline to draw.</param>
//        /// <param name="triangleBorders">Which borders of the triangular part of the outline to draw.</param>
//        /// <param name="shadeMode">ShadeVertices delegate used.</param>
//        /// <returns>A Trapezoidal ShapeDescriptor object.</returns>
//        public static ShapeDescriptor DrawFullLeftTrapezoid(Vector2 position, Size size,
//                                                            int triangleWidth, bool isTriangleUpside,
//                                                            Color color,
//                                                            int borderSize, Color borderColor,
//                                                            BorderStyle style, Border rectangleBorders,
//                                                            Border triangleBorders,
//                                                            ShadingMode shadeMode)
//        {
//            ShapeDescriptor sTrapezoid = DrawLeftTrapezoid(position, size, triangleWidth, isTriangleUpside,
//                                                           color, borderSize, shadeMode);
//            ShapeDescriptor sOutline = DrawLeftTrapezoidalOutline(position, size, triangleWidth, isTriangleUpside,
//                                                                  color, borderSize, borderColor, style,
//                                                                  rectangleBorders, triangleBorders);

//            return ShapeDescriptor.Join(sTrapezoid, sOutline);
//        }

//        /// <summary>
//        /// This methods draws a trapezoid whose 90° angle is in the right side.
//        /// </summary>
//        /// <param name="position">The topLeft point of the trapezoid</param>
//        /// <param name="size">The <b>whole size</b> of the trapezoid</param>
//        /// <param name="triangleWidth">The extra width of the greater base side</param>
//        /// <param name="isTriangleUpside">If the triangle part is to be drawn upside</param>
//        /// <param name="isShaded">If shading is to be applied to the trapezoid polygons</param>
//        /// <param name="color">Color of the trapezoid's inner area. The specified ShadeVertices 
//        /// delegate will then proceed to create a shaded version.</param>
//        /// <param name="borderSize">Size in pixels of the border.</param>
//        /// <param name="borderColor">Average color of the border.</param>
//        /// <param name="rectangleBorders">Which borders of the rectangular part of the outline to draw.</param>
//        /// <param name="triangleBorders">Which borders of the triangular part of the outline to draw.</param>
//        /// <param name="shadeMode">ShadeVertices delegate used.</param>
//        /// <returns>A Trapezoidal ShapeDescriptor object.</returns>
//        public static ShapeDescriptor DrawFullRightTrapezoid(Vector2 position, Size size,
//                                                             int triangleWidth, bool isTriangleUpside,
//                                                             Color color,
//                                                             int borderSize, Color borderColor,
//                                                             BorderStyle style, Border rectangleBorders,
//                                                             Border triangleBorders,
//                                                             ShadingMode shadeMode)
//        {
//            ShapeDescriptor sTrapezoid = DrawRightTrapezoid(position, size, triangleWidth, isTriangleUpside,
//                                                            color, borderSize, shadeMode);
//            ShapeDescriptor sOutline = DrawRightTrapezoidalOutline(position, size, triangleWidth, isTriangleUpside,
//                                                                   color, borderSize, borderColor, style,
//                                                                   rectangleBorders, triangleBorders);

//            return ShapeDescriptor.Join(sTrapezoid, sOutline);
//        }

//        /// <summary>
//        /// Draws a right trapezoidal outline.
//        /// </summary>
//        /// <param name="position">The position.</param>
//        /// <param name="size">The size.</param>
//        /// <param name="triangleWidth">Width of the triangle.</param>
//        /// <param name="isTriangleUpside">Ff set to <c>true</c> the triangle will be upside.</param>
//        /// <param name="color">The color.</param>
//        /// <param name="borderSize">Size of the border.</param>
//        /// <param name="borderColor">Color of the border.</param>
//        /// <param name="style">The style.</param>
//        /// <param name="rectangleBorders">The rectangle borders.</param>
//        /// <param name="triangleBorders">The triangle borders.</param>
//        /// <returns>A right trapezoidal outline ShapeDescriptor object.</returns>
//        public static ShapeDescriptor DrawRightTrapezoidalOutline(Vector2 position, Size size,
//                                                                  int triangleWidth, bool isTriangleUpside, Color color,
//                                                                  int borderSize, Color borderColor,
//                                                                  BorderStyle style, Border rectangleBorders,
//                                                                  Border triangleBorders)
//        {
//            Vector2 topLeft = position;
//            Size innerSize = new Size(size.Width - borderSize, size.Height - borderSize);

//            ShapeDescriptor sTriangleSide = ShapeDescriptor.Empty;
//            ShapeDescriptor sTriangleBase = ShapeDescriptor.Empty;
//            ShapeDescriptor sRectangleOutline =
//                DrawRectangularOutline(topLeft, size, borderSize, borderColor, style,
//                                       rectangleBorders);

//            if (isTriangleUpside)
//            {
//                if ((triangleBorders & Border.Right) == Border.Right)
//                    sTriangleSide = DrawLine(borderSize,
//                                             Color.FromArgb(255, ColorOperator.Scale(borderColor, lightShadeFactor)),
//                                             new Vector2(topLeft.X + size.Width, position.Y + (borderSize/2)),
//                                             new Vector2(topLeft.X + size.Width + triangleWidth,
//                                                         topLeft.Y + size.Height - (borderSize/2)));

//                if ((triangleBorders & Border.Bottom) == Border.Bottom)
//                    sTriangleBase =
//                        DrawRectangle(new Vector2(topLeft.X + size.Width, topLeft.Y + size.Height - borderSize),
//                                      new Size(triangleWidth, borderSize),
//                                      Color.FromArgb(255, ColorOperator.Scale(borderColor, darkShadeFactor)));
//            }
//            else
//            {
//                if ((triangleBorders & Border.Left) == Border.Left)
//                    sTriangleSide = DrawLine(borderSize,
//                                             Color.FromArgb(255, ColorOperator.Scale(borderColor, darkShadeFactor)),
//                                             new Vector2(topLeft.X + innerSize.Width,
//                                                         position.Y + size.Height - (borderSize/2)),
//                                             new Vector2(topLeft.X + innerSize.Width + triangleWidth,
//                                                         position.Y + (borderSize/2)));


//                if ((triangleBorders & Border.Top) == Border.Top)
//                    sTriangleBase = DrawRectangle(new Vector2(topLeft.X + innerSize.Width - borderSize, topLeft.Y),
//                                                  new Size(triangleWidth, borderSize),
//                                                  Color.FromArgb(255, ColorOperator.Scale(borderColor, lightShadeFactor)));
//            }

//            return ShapeDescriptor.Join(sRectangleOutline, sTriangleSide, sTriangleBase);
//        }

//        /// <summary>
//        /// Draws a right trapezoid.
//        /// </summary>
//        /// <param name="position">The position.</param>
//        /// <param name="size">The size.</param>
//        /// <param name="triangleWidth">Width of the triangle.</param>
//        /// <param name="isTriangleUpside">if set to <c>true</c> [is triangle upside].</param>
//        /// <param name="color">The color.</param>
//        /// <param name="borderSize">Size of the border.</param>
//        /// <param name="borderColor">Color of the border.</param>
//        /// <param name="style">The style.</param>
//        /// <param name="rectangleBorders">The rectangle borders.</param>
//        /// <param name="triangleBorders">The triangle borders.</param>
//        /// <param name="shadeMode">The shade mode.</param>
//        /// <returns>A right trapezoidal ShapeDescriptor object.</returns>
//        public static ShapeDescriptor DrawRightTrapezoid(Vector2 position, Size size, int triangleWidth,
//                                                         bool isTriangleUpside,
//                                                         Color color,
//                                                         int borderSize,
//                                                         ShadingMode shadeMode)
//        {
//            ShapeDescriptor sTrapezoid;

//            CustomVertex.TransformedColored[] vertices = new CustomVertex.TransformedColored[5];
//            int[] indices = new int[9];

//            Vector2 topLeft = position;

//            Size innerSize = new Size(size.Width - borderSize, size.Height - borderSize);

//            int[] colors = shadeMode(color);

//            int colorTopLeft = colors[0];
//            int colorTopRight = colors[1];
//            int colorBottomleft = colors[2];
//            int colorBottomRight = colors[3];


//            vertices[0] = new CustomVertex.TransformedColored(topLeft.X, topLeft.Y, 0, 1, colorTopLeft);
//            vertices[1] =
//                new CustomVertex.TransformedColored(topLeft.X, topLeft.Y + innerSize.Height, 0, 1, colorBottomleft);
//            vertices[2] =
//                new CustomVertex.TransformedColored(topLeft.X + innerSize.Width, topLeft.Y + innerSize.Height, 0, 1,
//                                                    colorBottomRight);
//            vertices[3] =
//                new CustomVertex.TransformedColored(topLeft.X + innerSize.Width, topLeft.Y, 0, 1, colorTopRight);

//            if (isTriangleUpside)

//                vertices[4] =
//                    new CustomVertex.TransformedColored(topLeft.X + innerSize.Width + triangleWidth,
//                                                        topLeft.Y + innerSize.Height, 0, 1, colorBottomRight);
//            else
//                vertices[4] =
//                    new CustomVertex.TransformedColored(topLeft.X + innerSize.Width + triangleWidth, topLeft.Y, 0, 1,
//                                                        colorTopRight);

//            indices[0] = 0;
//            indices[1] = 3;
//            indices[2] = 2;
//            indices[3] = 0;
//            indices[4] = 2;
//            indices[5] = 1;
//            indices[6] = 3;
//            indices[7] = 4;
//            indices[8] = 2;

//            sTrapezoid = new ShapeDescriptor(3, vertices, indices);
//            return sTrapezoid;
//        }

//        #endregion

//        #region Triangles

//        public static ShapeDescriptor DrawEquilateralTriangle(Vector2 leftVertex, float sideLength, Color color,
//                                                              bool isShaded, bool isTriangleUpside)
//        {
//            CustomVertex.TransformedColored[] vertices = new CustomVertex.TransformedColored[3];
//            Color shaded;
//            float heightOffset = (float) (sideLength/2*Math.Sqrt(3));

//            int col1 = color.ToArgb();
//            int col2;

//            if (isShaded)
//            {
//                shaded = Color.FromArgb(color.A, ColorOperator.Scale(color, darkShadeFactor));
//                col2 = shaded.ToArgb();
//            }
//            else
//                col2 = col1;

//            vertices[0] = new CustomVertex.TransformedColored(leftVertex.X, leftVertex.Y, 0, 1, col2);
//            vertices[1] = new CustomVertex.TransformedColored(leftVertex.X + sideLength, leftVertex.Y, 0, 1, col2);

//            int[] indices = new int[3];

//            if (isTriangleUpside)
//            {
//                heightOffset *= -1;
//                indices[0] = 0;
//                indices[1] = 1;
//                indices[2] = 2;
//            }
//            else
//            {
//                indices[0] = 2;
//                indices[1] = 0;
//                indices[2] = 1;
//            }

//            vertices[2] =
//                new CustomVertex.TransformedColored(leftVertex.X + sideLength/2, leftVertex.Y + heightOffset, 0, 1, col1);


//            return new ShapeDescriptor(1, vertices, indices);
//        }

//        #endregion
//    }
//}