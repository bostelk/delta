//#region Using directives

//using System.Drawing;
//using AvengersUTD.Odyssey.UserInterface.Style;
//using Microsoft.DirectX;

//#endregion

//namespace AvengersUTD.Odyssey.UserInterface.RenderableControls
//{
//    public class Table : Panel
//    {
//        int rowCount;
//        int columnCount;

//        protected Cell[,] cells;

//        protected TableStyle style;

//        #region Properties

//        /// <summary>
//        /// Gets or sets the <see cref="AvengersUTD.Odyssey.UserInterface.RenderableControls.Cell"/> with 
//        /// the specified row and column.
//        /// </summary>
//        /// <value>The cell.</value>
//        public Cell this[int row, int column]
//        {
//            get { return cells[row, column]; }
//            set { cells[row, column] = value; }
//        }

//        public int RowCount
//        {
//            get { return rowCount; }
//            set { rowCount = value; }
//        }

//        public int ColumnCount
//        {
//            get { return columnCount; }
//            set { columnCount = value; }
//        }

//        #endregion

//        protected new static ColorArray defaultColors = new ColorArray(
//            Color.Transparent,
//            Color.Red,
//            Color.Transparent,
//            Color.Gray,
//            Color.BlueViolet,
//            Color.Transparent,
//            Color.Black,
//            Color.Black
//            );

//        #region Constructors

//        /// <summary>
//        /// Initializes a new instance of the <see cref="Table"/> class. Uses the 
//        /// default <see cref="TableStyle"/> style and the default <see cref="CellStyle"/> style.
//        /// Uses the flat <see cref="BorderStyle"/> value. Doesn't apply shading to the background.
//        /// </summary>
//        /// <param name="id">The id.</param>
//        /// <param name="rowCount">The row count.</param>
//        /// <param name="columnCount">The column count.</param>
//        /// <param name="position">The position.</param>
//        public Table(string id, int rowCount, int columnCount, Vector2 position)
//            : this(id, rowCount, columnCount, position, BorderStyle.Flat, TableStyle.Default, CellStyle.Default,
//                   Shapes.ShadeNone,
//                   defaultColors)
//        {
//        }

//        /// <summary>
//        /// Initializes a new instance of the <see cref="Table"/> class. Uses the 
//        /// default <see cref="TableStyle"/> style. Allows the user to define which <see cref="BorderStyle"/>
//        /// value to use. Allows the user to define which <see cref="CellStyle"/> to use. 
//        /// Doesn't apply shading to the background.
//        /// </summary>
//        /// <param name="id">The id.</param>
//        /// <param name="rowCount">The row count.</param>
//        /// <param name="columnCount">The column count.</param>
//        /// <param name="position">The position.</param>
//        /// <param name="borderStyle">The border style.</param>
//        /// <param name="cellBorders">The cell borders.</param>
//        public Table(string id, int rowCount, int columnCount, Vector2 position, BorderStyle borderStyle,
//                     CellStyle cellStyle)
//            : this(id, rowCount, columnCount, position, borderStyle, TableStyle.Default, cellStyle,
//                   Shapes.ShadeNone,
//                   defaultColors)
//        {
//        }

//        /// <summary>
//        /// Initializes a new instance of the <see cref="Table"/> class. Uses the 
//        /// <see cref="TableStyle"/> value provided. Allows the user to define which <see cref="BorderStyle"/>
//        /// value to use, which <see cref="CellStyle"/> to use, which <see cref="Shapes.ShadeVertices"/> 
//        /// method to use and which <see cref="ColorArray"/> too use.
//        /// </summary>
//        /// <param name="id">The id.</param>
//        /// <param name="rowCount">The row count.</param>
//        /// <param name="columnCount">The column count.</param>
//        /// <param name="position">The position.</param>
//        /// <param name="cellStyle">The cell style.</param>
//        /// <param name="tableStyle">The style.</param>
//        /// <param name="cellBorders">The cell borders.</param>
//        /// <param name="shadeMode">The shade mode.</param>
//        /// <param name="colorArray">The color array.</param>
//        public Table(string id, int rowCount, int columnCount, Vector2 position, BorderStyle borderStyle,
//                     TableStyle tableStyle,
//                     CellStyle cellStyle,
//                     Shapes.ShadingMode shadeMode, ColorArray colorArray)
//            : base(id, position,
//                   ComputeStandardSize(DefaultBorderSize, rowCount, columnCount, tableStyle),
//                   Shape.Custom, borderStyle, shadeMode, colorArray)
//        {
//            this.rowCount = rowCount;
//            this.columnCount = columnCount;
//            style = tableStyle;
//            cells = new Cell[rowCount,columnCount];

//            applyStatusChanges = false;

//            for (int r = 0; r < rowCount; r++)
//            {
//                for (int c = 0; c < columnCount; c++)
//                {
//                    Cell cell = new Cell(id + string.Format("_Cell:R{0}C{1}", r.ToString("00"), c.ToString("00")),
//                                         CellPosition(r, c),
//                                         new Size(tableStyle.ColumnWidth, tableStyle.RowHeight),
//                                         BorderStyle.Flat, cellStyle, Shapes.ShadeNone, defaultColors
//                        );

//                    cells[r, c] = cell;
//                    Add(cell);
//                }
//            }
//        }

//        #endregion

//        Vector2 CellPosition(int r, int c)
//        {
//            return new Vector2(
//                borderSize + style.CellSpacingX + (style.CellSpacingX + style.ColumnWidth)*c,
//                borderSize + style.CellSpacingY + (style.CellSpacingY + style.RowHeight)*r
//                );
//        }

//        static Size ComputeStandardSize(int borderSize, int rowCount, int columnCount, TableStyle style)
//        {
//            return new Size(
//                borderSize*2 + style.CellSpacingX + (style.CellSpacingX + style.ColumnWidth)*columnCount,
//                borderSize*2 + style.CellSpacingY + (style.CellSpacingY + style.RowHeight)*rowCount);
//        }

//        /// <summary>
//        /// Formats the table using the specified style.
//        /// </summary>
//        /// <param name="style">The style.</param>
//        public void Format(TableStyle style)
//        {
//            this.style = style;
//            for (int r = 0; r < rowCount; r++)
//            {
//                for (int c = 0; c < columnCount; c++)
//                {
//                    Cell cell = cells[r, c];
//                    cell.Size = new Size(style.ColumnWidth, style.RowHeight);
//                    cell.Position = CellPosition(r, c);
//                }
//            }
//            size = ComputeStandardSize(borderSize, rowCount, columnCount, style);
//        }

//        public void SetRowHeight(int row, int rowHeight)
//        {
//            int delta = rowHeight - this[row, 0].Size.Height;
//            size = new Size(size.Width, size.Height + delta);

//            for (int c = 0; c < columnCount; c++)
//            {
//                Cell cell = this[row, c];
//                cell.Size = new Size(cell.Size.Width, rowHeight);
//            }

//            for (int r = row + 1; r < rowCount; r++)
//            {
//                for (int c = 0; c < columnCount; c++)
//                {
//                    Cell cell = this[r, c];
//                    cell.Position = Vector2.Add(cell.Position, new Vector2(0, delta));
//                }
//            }
//        }

//        public void SetColumnWidth(int column, int columnWidth)
//        {
//            int delta = columnWidth - this[0, column].Size.Width;
//            size = new Size(size.Width + delta, size.Height);

//            for (int r = 0; r < rowCount; r++)
//            {
//                Cell cell = this[r, column];
//                cell.Size = new Size(columnWidth, cell.Size.Height);
//            }

//            for (int r = 0; r < rowCount; r++)
//            {
//                for (int c = column + 1; c < columnCount; c++)
//                {
//                    Cell cell = this[r, c];
//                    cell.Position = Vector2.Add(cell.Position, new Vector2(delta, 0));
//                }
//            }
//        }

//        #region IRenderable overriden methods

//        public override bool IntersectTest(Point cursorLocation)
//        {
//            return Intersection.RectangleTest(absolutePosition, size, cursorLocation);
//        }

//        public override void CreateShape()
//        {
//            base.CreateShape();
//            shapeDescriptor = Shapes.DrawFullRectangle(absolutePosition, size, colorArray.Enabled,
//                                                       shadingMode, borderSize, colorArray.BorderEnabled, borderStyle,
//                                                       style.Borders);
//            shapeDescriptor.Depth = depth;
//            shapeDescriptors[0] = shapeDescriptor;
//        }

//        public override void UpdateShape()
//        {
//            shapeDescriptor.UpdateShape(Shapes.DrawFullRectangle(absolutePosition, size, colorArray.Enabled,
//                                                                 shadingMode, borderSize, colorArray.BorderEnabled,
//                                                                 borderStyle, style.Borders));
//        }

//        #endregion
//    }
//}