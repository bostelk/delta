//#region Using directives



//#endregion

//namespace AvengersUTD.Odyssey.UserInterface.RenderableControls
//{

//    #region TableStyle struct

//    public struct TableStyle
//    {
//        public const int DefaultCellSpacingX = 1;
//        public const int DefaultCellSpacingY = 1;
//        public const int DefaultColumnWidth = 100;
//        public const int DefaultRowHeight = 30;

//        int columnWidth;
//        int rowHeight;
//        int cellSpacingX;
//        int cellSpacingY;
//        Border borders;

//        #region Properties

//        /// <summary>
//        /// Gets the default TableStyle value.
//        /// </summary>
//        /// <value>The default.</value>
//        public static TableStyle Default
//        {
//            get
//            {
//                return new TableStyle(DefaultColumnWidth, DefaultRowHeight,
//                                      DefaultCellSpacingX, DefaultCellSpacingY,
//                                      Border.All);
//            }
//        }

//        /// <summary>
//        /// Gets or sets the width of the column.
//        /// </summary>
//        /// <value>The width of the column.</value>
//        public int ColumnWidth
//        {
//            get { return columnWidth; }
//            set { columnWidth = value; }
//        }

//        /// <summary>
//        /// Gets or sets the height of the row.
//        /// </summary>
//        /// <value>The height of the row.</value>
//        public int RowHeight
//        {
//            get { return rowHeight; }
//            set { rowHeight = value; }
//        }

//        /// <summary>
//        /// Gets or sets the cell spacing X value.
//        /// </summary>
//        /// <value>The amount of space in pixels on the X Axis.</value>
//        public int CellSpacingX
//        {
//            get { return cellSpacingX; }
//            set { cellSpacingX = value; }
//        }

//        /// <summary>
//        /// Gets or sets the cell spacing Y value.
//        /// </summary>
//        /// <value>The amount of space in pixels on the Y Axis.</value>
//        public int CellSpacingY
//        {
//            get { return cellSpacingY; }
//            set { cellSpacingY = value; }
//        }

//        /// <summary>
//        /// Gets or sets which borders to draw for the table.
//        /// </summary>
//        /// <value>The borders.</value>
//        public Border Borders
//        {
//            get { return borders; }
//            set { borders = value; }
//        }

//        #endregion

//        /// <summary>
//        /// Initializes a new instance of the <see cref="TableStyle"/> class.
//        /// </summary>
//        /// <param name="columnWidth">Width of the table columns.</param>
//        /// <param name="rowHeight">Height of the table rows.</param>
//        /// <param name="cellSpacingX">The amount of space in pixels between cells on the X axis.</param>
//        /// <param name="cellSpacingY">The amount of space in pixels between cells on the Y axis</param>
//        /// <param name="borders">Which border to draw for the table.</param>
//        public TableStyle(int columnWidth, int rowHeight, int cellSpacingX, int cellSpacingY, Border borders)
//        {
//            this.columnWidth = columnWidth;
//            this.rowHeight = rowHeight;
//            this.cellSpacingX = cellSpacingX;
//            this.cellSpacingY = cellSpacingY;
//            this.borders = borders;
//        }
//    }

//    #endregion

//    #region CellStyle struct

//    public struct CellStyle
//    {
//        public const int DefaultPaddingLeft = 5;
//        public const int DefaultPaddingRight = 5;

//        int paddingTop;
//        int paddingLeft;
//        int paddingRight;
//        int paddingBottom;
//        Border borders;
//        TextStyle textStyle;

//        #region Properties

//        public static CellStyle Default
//        {
//            get
//            {
//                return new CellStyle(
//                    (TableStyle.DefaultRowHeight - StyleManager.NormalFontSize)/2,
//                    DefaultPaddingLeft,
//                    (TableStyle.DefaultRowHeight - StyleManager.NormalFontSize)/2,
//                    DefaultPaddingRight,
//                    Border.All,
//                    TextStyle.Default);
//            }
//        }

//        public int PaddingTop
//        {
//            get { return paddingTop; }
//            set { paddingTop = value; }
//        }

//        public int PaddingLeft
//        {
//            get { return paddingLeft; }
//            set { paddingLeft = value; }
//        }

//        public int PaddingRight
//        {
//            get { return paddingRight; }
//            set { paddingRight = value; }
//        }

//        public int PaddingBottom
//        {
//            get { return paddingBottom; }
//            set { paddingBottom = value; }
//        }

//        public Border Borders
//        {
//            get { return borders; }
//            set { borders = value; }
//        }

//        public TextStyle TextStyle
//        {
//            get { return textStyle; }
//            set { textStyle = value; }
//        }

//        #endregion

//        public CellStyle(int rowHeight, int fontSize, Border borders, TextStyle style) :
//            this(
//            (rowHeight - fontSize)/2,
//            DefaultPaddingLeft, DefaultPaddingRight,
//            (rowHeight - fontSize)/2,
//            borders,
//            style)
//        {
//        }


//        public CellStyle(int paddingTop, int paddingLeft, int paddingRight, int paddingBottom, Border borders,
//                         TextStyle style)
//        {
//            this.paddingTop = paddingTop;
//            this.paddingLeft = paddingLeft;
//            this.paddingRight = paddingRight;
//            this.paddingBottom = paddingBottom;
//            this.borders = borders;
//            textStyle = style;
//        }
//    }

//    #endregion
//}