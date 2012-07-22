//using System;
//using System.Drawing;
//using System.Windows.Forms;
//using AvengersUTD.Odyssey.UserInterface.Helpers;
//using AvengersUTD.Odyssey.UserInterface.Style;
//using Microsoft.DirectX;
//using Microsoft.DirectX.Direct3D;

//namespace AvengersUTD.Odyssey.UserInterface.RenderableControls
//{

//    #region DropDownButton Class

//    internal class DropDownButton : RenderableControl
//    {
//        public const int DefaultTriangleSideLength = 10;
//        public const int DefaultDropDownButtonWidth = 20;
//        public const int DefaultDropDownButtonHeight = 30;

//        ShapeDescriptor buttonDescriptor;
//        ShapeDescriptor triangleDescriptor;
//        Vector2 triangleLeftVertexPosition;
//        Vector2 triangleLeftVertexAbsolutePosition;

//        public DropDownButton(string id, Vector2 position, Size size, ColorArray colorArray)
//            : base(id, position, size, Shape.Custom, BorderStyle.Raised, Shapes.ShadeTopToBottom, colorArray)
//        {
//            shapeDescriptors = new ShapeDescriptor[2];
//            canRaiseEvents = false;
//            isFocusable = false;
//            triangleLeftVertexPosition = new Vector2(DefaultDropDownButtonWidth/2 - DefaultTriangleSideLength/2,
//                                                     (size.Height - (float) (DefaultTriangleSideLength/2*Math.Sqrt(3)))/
//                                                     2);
//        }

//        #region IRenderable overriden methods

//        public override void UpdateShape()
//        {
//            buttonDescriptor.UpdateShape(ShapeDescriptor.ComputeShape(this, Shape.Rectangle));
//        }

//        public override void CreateShape()
//        {
//            base.CreateShape();
//            buttonDescriptor = ShapeDescriptor.ComputeShape(this, Shape.Rectangle);
//            triangleDescriptor =
//                Shapes.DrawEquilateralTriangle(triangleLeftVertexAbsolutePosition, DefaultTriangleSideLength,
//                                               ColorOperator.Scale(Color.Black, 0.5f), false, false);
//            buttonDescriptor.Depth = depth;
//            triangleDescriptor.Depth = Depth.AsChildOf(depth);

//            shapeDescriptors[0] = buttonDescriptor;
//            shapeDescriptors[1] = triangleDescriptor;
//        }

//        public override void ComputeAbsolutePosition()
//        {
//            base.ComputeAbsolutePosition();
//            triangleLeftVertexAbsolutePosition = Vector2.Add(absolutePosition, triangleLeftVertexPosition);
//        }

//        public override bool IntersectTest(Point cursorLocation)
//        {
//            return false;
//        }

//        #endregion
//    }

//    #endregion

//    /// <summary>
//    /// This is the DropDownList control, also known as "ComboBox". 
//    /// It allows the user to choose from a list of strings a particular choice or
//    /// option. When the DropDownList is not clicked it displays the currently 
//    /// selected choice. Othewise, when the user clicks on it, it displays the full
//    /// list oof choices.
//    /// </summary>
//    public class DropDownList : Panel, ISpriteControl, IActivableControl
//    {
//        public const int DefaultListLabelOffsetX = 10;
//        public const int DefaultListLabelOffsetY = 3;

//        #region Default Colors

//        protected new static ColorArray defaultColors = new ColorArray(
//            Color.RoyalBlue,
//            Color.RoyalBlue,
//            Color.RoyalBlue,
//            Color.Gray,
//            Color.Red,
//            Color.RoyalBlue,
//            Color.Gray,
//            Color.MediumBlue
//            );

//        protected static ColorArray dropDownButtonDefaultColors = new ColorArray(
//            Color.Silver,
//            Color.DeepSkyBlue,
//            Color.Red,
//            Color.Gray,
//            Color.MediumBlue,
//            Color.Red,
//            Color.Gray,
//            Color.MediumBlue
//            );

//        protected static Color labelDefaultEnabledColor = Label.DefaultColor;
//        protected static Color labelDefaultHighlightedColor = Color.DeepSkyBlue;

//        #endregion

//        // Bool variable used to detect if the control is in its droppeddown status
//        protected bool droppedDown;

//        // Currently Selected Index
//        protected int selectedIndex;

//        // Font Size used in the control
//        protected int fontSize;

//        // Button and label colors
//        protected Color buttonInnerArea, buttonBorderColor;
//        protected Color labelEnabledColor, labelHighlightedColor;
//        protected ColorArray buttonColorArray;

//        // Currently selected label and label List.
//        protected Label selectedLabel;

//        // List shape (not yet implemented)
//        protected Shape listShape;

//        DropDownButton dropDownButton;
//        Panel listPanel;

//        #region Private members

//        int highlightedLabelIndex;

//        // Sizes for the various sub-parts of the control
//        Size boxSize;
//        Size dropDownButtonSize;
//        Size listSize;
//        Size itemSize;
//        Size expandedSize;

//        // Position vectors
//        Vector2 dropDownButtonPosition;
//        Vector2 listPosition;
//        Vector2 labelListPosition;
//        Vector2 selectedLabelPosition;

//        // ShapeDescriptor object
//        ShapeDescriptor boxDescriptor;

//        #endregion

//        #region Properties

//        public int SelectedIndex
//        {
//            get { return selectedIndex; }
//        }

//        public string SelectedItem
//        {
//            get { return (listPanel.Controls[selectedIndex] as Label).Text; }
//        }

//        #endregion

//        #region Constructors

//        /// <summary>
//        /// Default constructor.
//        /// </summary>
//        /// <param name="id"></param>
//        /// <param name="optionLabels"></param>
//        /// <param name="position"></param>
//        /// <param name="size"></param>
//        public DropDownList(string id, string[] optionLabels, Vector2 position, Size size)
//            : this(id, optionLabels, position, size, BorderStyle.Raised, labelDefaultEnabledColor,
//                   labelDefaultHighlightedColor, Shapes.ShadeTopToBottom, defaultColors, dropDownButtonDefaultColors)
//        {
//        }

//        /// <summary>
//        /// Allows user to specify every parameter.
//        /// </summary>
//        /// <param name="id"></param>
//        /// <param name="optionList"></param>
//        /// <param name="position"></param>
//        /// <param name="size"></param>
//        /// <param name="boxColors"></param>
//        /// <param name="buttonColors"></param>
//        /// <param name="labelEnabled"></param>
//        /// <param name="labelHighlighted"></param>
//        /// <param name="style"></param>
//        public DropDownList(string id, string[] optionList, Vector2 position, Size size,
//                            BorderStyle style, Color labelEnabled, Color labelHighlighted,
//                            Shapes.ShadingMode shadeMode,
//                            ColorArray boxColors, ColorArray buttonColors)
//            : base(id, position, size, Shape.Custom, style, shadeMode, boxColors)
//        {
//            applyStatusChanges = true;
//            isFocusable = true;
//            shapeDescriptors = new ShapeDescriptor[1];

//            // In the future a constructor will allow to specify the size of the font used in the control
//            fontSize = StyleManager.NormalFontSize;

//            // Define sub-parts sizes
//            boxSize = new Size(size.Width - DropDownButton.DefaultDropDownButtonWidth + DefaultBorderSize, size.Height);
//            dropDownButtonSize =
//                new Size(DropDownButton.DefaultDropDownButtonWidth, DropDownButton.DefaultDropDownButtonHeight);
//            itemSize = new Size(size.Width,
//                                fontSize + DefaultListLabelOffsetY);

//            // Define sub-parts relative positions
//            dropDownButtonPosition = new Vector2(boxSize.Width - DefaultBorderSize, 0);

//            labelListPosition = new Vector2(DefaultListLabelOffsetX, borderSize + DefaultListLabelOffsetY);
//            selectedLabelPosition =
//                new Vector2(DefaultListLabelOffsetX, (DropDownButton.DefaultDropDownButtonHeight - fontSize)/2);
//            listPosition = new Vector2(0, boxSize.Height);
//            listSize = new Size(itemSize.Width, itemSize.Height*optionList.Length + 2*borderSize);
//            expandedSize = new Size(size.Width, size.Height + listSize.Height);

//            // Define sub-parts colors
//            buttonColorArray = buttonColors;
//            buttonInnerArea = buttonColorArray[ColorIndex.Enabled];
//            buttonBorderColor = buttonColorArray[ColorIndex.BorderEnabled];
//            labelEnabledColor = labelEnabled;
//            labelHighlightedColor = labelHighlighted;

//            // Create child-controls
//            dropDownButton = new DropDownButton(id + "_Button", dropDownButtonPosition, dropDownButtonSize,
//                                                buttonColorArray);
//            dropDownButton.IsSubComponent = true;
//            dropDownButton.CanRaiseEvents = false;

//            listPanel = new Panel(id + "_Panel", listPosition, listSize, Shape.Rectangle, BorderStyle.Raised,
//                                  Shapes.ShadeTopToBottom, defaultColors);
//            listPanel.IsFocusable = false;
//            listPanel.IsVisible = false;
//            listPanel.IsSubComponent = true;

//            listPanel.MouseMove += delegate(BaseControl ctl, MouseEventArgs e)
//                                       {
//                                           highlightedLabelIndex =
//                                               (int)
//                                               ((e.Location.Y - listPanel.AbsolutePosition.Y + borderSize)/
//                                                (itemSize.Height + 1));

//                                           if (highlightedLabelIndex >= listPanel.Controls.Count)
//                                               return;

//                                           DebugManager.LogToScreen(highlightedLabelIndex.ToString());

//                                           for (int i = 0; i < listPanel.Controls.Count; i++)
//                                           {
//                                               Label label = listPanel.Controls[i] as Label;
//                                               if (i != highlightedLabelIndex)
//                                                   label.SetHighlight(false);
//                                               else
//                                                   label.SetHighlight(true);
//                                           }
//                                       };

//            listPanel.MouseDown += delegate(BaseControl ctl, MouseEventArgs e) { Select(highlightedLabelIndex); };

//            listPanel.MouseUp += delegate(BaseControl ctl, MouseEventArgs e)
//                                     {
//                                         dropDownButton.IsHighlighted = false;
//                                         Select(highlightedLabelIndex);
//                                     };

//            // Create label list
//            for (int i = 0; i < optionList.Length; i++)
//            {
//                Vector2 labelPosition = Vector2.Add(labelListPosition, new Vector2(0, i*(itemSize.Height + 1)));
//                Label label = new Label(id + "_l" + i.ToString(), optionList[i], fontSize,
//                                        Alignment.Left, Alignment.Top, labelPosition, labelEnabledColor,
//                                        labelHighlightedColor);
//                // let the panel handle highlights
//                label.Style.ApplyHighlight = false;
//                listPanel.Add(label);
//            }
//            // Assign needed depth info
//            listPanel.Depth = new Depth(0, 1, 0);

//            Add(dropDownButton);
//            Add(listPanel);

//            // Create a copy of the currently selected label
//            selectedLabel = (listPanel.Controls[0] as Label).Clone(id + "_ls");
//            //selectedLabel = new Label(id + "_ls", optionList[0],
//            //    Alignment.Left, Alignment.Top, selectedLabelPosition, Label.DefaultColor);
//        }

//        #endregion

//        #region Exposed events

//        public event ControlEventHandler SelectedIndexChanged;
//        public event ControlEventHandler DropDown;
//        public event ControlEventHandler DropDownClosed;

//        protected virtual void OnSelectedIndexChanged(BaseControl ctl)
//        {
//            if (SelectedIndexChanged != null)
//                SelectedIndexChanged(ctl);
//        }

//        protected virtual void OnDropDown(BaseControl ctl)
//        {
//            Expand();

//            if (DropDown != null)
//                DropDown(ctl);
//        }

//        protected virtual void OnDropDownClosed(BaseControl ctl)
//        {
//            Collapse();

//            if (DropDownClosed != null)
//                DropDownClosed(ctl);
//        }

//        #endregion

//        #region Overriden inherited events

//        protected override void OnMouseEnter(BaseControl ctl, MouseEventArgs args)
//        {
//            base.OnMouseEnter(ctl, args);
//            if (UI.CurrentHud.ClickedControl == null)
//            {
//                dropDownButton.IsHighlighted = true;
//            }
//        }

//        protected override void OnMouseLeave(BaseControl ctl, MouseEventArgs args)
//        {
//            base.OnMouseLeave(ctl, args);
//            if (UI.CurrentHud.ClickedControl == null)
//            {
//                dropDownButton.IsHighlighted = false;
//            }
//        }

//        protected override void OnLostFocus(BaseControl ctl)
//        {
//            if (ctl == listPanel)
//                return;

//            if (droppedDown)
//            {
//                UI.CurrentHud.BeginDesign();
//                OnDropDownClosed(ctl);
//                UI.CurrentHud.EndDesign();
//            }
//            dropDownButton.IsSelected = false;
//            base.OnLostFocus(ctl);
//        }

//        protected override void OnMouseDown(BaseControl ctl, MouseEventArgs args)
//        {
//            base.OnMouseDown(ctl, args);

//            if (!droppedDown)
//            {
//                UI.CurrentHud.BeginDesign();
//                dropDownButton.IsSelected = true;
//                OnDropDown(this);
//                UI.CurrentHud.EndDesign();
//            }
//            else
//            {
//                UI.CurrentHud.BeginDesign();
//                OnDropDownClosed(this);
//                UI.CurrentHud.EndDesign();
//            }
//        }

//        #endregion

//        #region IRenderable overriden methods

//        public override void UpdateShape()
//        {
//            base.UpdateShape();
//            // Updates sub-parts shapes
//            boxDescriptor.UpdateShape(Shapes.DrawFullRectangle(absolutePosition, boxSize,
//                                                               innerAreaColor, shadingMode, borderSize, borderColor,
//                                                               borderStyle, Border.All ^ Border.Right));
//        }

//        public override void CreateShape()
//        {
//            base.CreateShape();
//            boxDescriptor = Shapes.DrawFullRectangle(absolutePosition, boxSize,
//                                                     innerAreaColor, shadingMode, borderSize, borderColor,
//                                                     BorderStyle.Raised, Border.All ^ Border.Right);
//            shapeDescriptors[0] = boxDescriptor;
//            boxDescriptor.Depth = depth;
//            selectedLabel.Parent = this;
//        }

//        public override bool IntersectTest(Point cursorLocation)
//        {
//            return Intersection.RectangleTest(absolutePosition, size, cursorLocation);
//        }

//        #endregion

//        void Expand()
//        {
//            droppedDown = true;
//            listPanel.IsVisible = true;
//        }

//        void Collapse()
//        {
//            droppedDown = false;
//            isHighlighted = false;
//            listPanel.IsVisible = false;
//        }

//        /// <summary>
//        /// Selects the label whose index is passed as parameter.
//        /// </summary>
//        /// <param name="index">The index of the label to select.</param>
//        public void Select(int index)
//        {
//            if (index != selectedIndex && index < listPanel.Controls.Count)
//            {
//                selectedIndex = index;
//                selectedLabel.Text = (listPanel.Controls[selectedIndex] as Label).Text;
//                OnSelectedIndexChanged(this);
//            }
//            if (droppedDown)
//                OnLostFocus(this);
//        }

//        #region ISpriteControl Members

//        public void Render()
//        {
//            selectedLabel.Render();
//        }

//        #endregion

//        #region IActivableControl Members

//        public bool IsActivated
//        {
//            get { return droppedDown; }
//        }

//        #endregion
//    }
//}