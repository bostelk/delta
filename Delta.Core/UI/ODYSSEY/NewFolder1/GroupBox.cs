//using System.Drawing;
//using AvengersUTD.Odyssey.UserInterface.Style;
//using Microsoft.DirectX;
//using Microsoft.DirectX.Direct3D;

//namespace AvengersUTD.Odyssey.UserInterface.RenderableControls
//{
//    public class GroupBox : ContainerControl, ISpriteControl
//    {
//        protected new static ColorArray defaultColors = new ColorArray(
//            Color.Transparent,
//            Color.Transparent,
//            Color.Transparent,
//            Color.DarkGray,
//            Color.Transparent,
//            Color.Transparent,
//            ColorOperator.Scale(Color.Gray, 1.15f),
//            Color.LightGray
//            );

//        const int DefaultGroupBoxOffset = 5;

//        int fontSize;

//        Label label;

//        #region Properties

//        public string Caption
//        {
//            get { return label.Text; }
//            set { label.Text = value; }
//        }

//        #endregion

//        public GroupBox(string id, string text, Vector2 position, Size size, Color labelColor,
//                        BorderStyle style)
//            : base(id, position, size, Shape.Rectangle, style, Shapes.ShadeNone, defaultColors)
//        {
//            fontSize = StyleManager.NormalFontSize;

//            Vector2 labelPosition = new Vector2(0 + DefaultGroupBoxOffset,
//                                                - fontSize);

//            label = new Label(id + "_Label",
//                              text,
//                              LabelSize.Normal,
//                              Alignment.Left,
//                              Alignment.Top,
//                              labelPosition,
//                              labelColor,
//                              labelColor,
//                              FontManager.DefaultFontName,
//                              true,
//                              true);

//            label.Style.IgnoreBounds = true;
//            label.IsSubComponent = true;
//            Add(label);

//            isFocusable = false;
//            applyStatusChanges = false;
//            canRaiseEvents = false;
//        }

//        #region ISpriteControl Members

//        public void Render()
//        {
//            label.Render();
//        }

//        #endregion
//    }
//}