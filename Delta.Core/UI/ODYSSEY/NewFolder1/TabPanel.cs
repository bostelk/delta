//#region Using directives

//using System.Collections.Generic;
//using System.Drawing;
//using System.Windows.Forms;
//using AvengersUTD.Odyssey.UserInterface.Style;
//using Microsoft.DirectX;
//using Microsoft.DirectX.Direct3D;
//using Font=Microsoft.DirectX.Direct3D.Font;

//#endregion

//namespace AvengersUTD.Odyssey.UserInterface.RenderableControls
//{
//    public class TabPanel : Panel, ISpriteControl
//    {
//        public const int DefaultTabTriangleWidth = 20;
//        const int DefaultTabBaseWidth = 80;
//        const int DefaultTabButtonWidth = DefaultTabBaseWidth + DefaultTabTriangleWidth;
//        const int DefaultTabHeight = 30;

//        protected ColorArray tabButtonColorArray;
//        protected Font tabButtonFont = FontManager.DefaultFont;
//        protected int selectedIndex = -1;

//        #region ColorArrays

//        protected new static ColorArray defaultColors = new ColorArray(
//            Color.FromArgb(200, 0, 150, 255),
//            Color.MediumBlue,
//            Color.DeepSkyBlue,
//            Color.Gray,
//            Color.Red,
//            Color.FromArgb(200, Color.SteelBlue),
//            Color.Gray,
//            Color.Gray
//            );

//        protected static ColorArray tabButtonDefaultColors = new ColorArray(
//            Color.RoyalBlue,
//            Color.MediumBlue,
//            Color.DeepSkyBlue,
//            Color.Gray,
//            Color.Cyan,
//            Color.RoyalBlue,
//            Color.Gray,
//            Color.Gray
//            );

//        #endregion

//        List<Button> tabButtons;
//        List<Panel> tabPanels;

//        Panel currentTab;

//        int totalWidth;

//        #region Properties

//        //public override SortedList<string, BaseControl> Controls
//        //{
//        //    get
//        //    {
//        //        SortedList<string, BaseControl> list = new SortedList<string, BaseControl>(
//        //            currentTab.Controls);
//        //        foreach (Button button in tabButtons)
//        //            list.Add(button.ID, button);
//        //        list.Add(currentTab.ID, currentTab);
//        //        return list;
//        //    }
//        //}

//        #endregion

//        #region Constructors

//        public TabPanel(string id, Vector2 position, Size size, ColorArray colorArray,
//                        ColorArray tabButtonColorArray)
//            : base(id, position, size, Shape.Rectangle, BorderStyle.Raised, Shapes.ShadeTopToBottom, colorArray)
//        {
//            this.tabButtonColorArray = tabButtonColorArray;
//            tabButtons = new List<Button>();
//            tabPanels = new List<Panel>();
//        }

//        public TabPanel(string id, Vector2 position, Size size)
//            : this(id, position, size, defaultColors, tabButtonDefaultColors)
//        {
//        }

//        #endregion

//        #region Exposed Events

//        public event ControlEventHandler SelectedIndexChanged;

//        protected virtual void OnSelectedIndexChanged(BaseControl ctl)
//        {
//            if (SelectedIndexChanged != null)
//                SelectedIndexChanged(ctl);
//        }

//        #endregion

//        public void AddTab(string tabLabel)
//        {
//            Panel tabPage = new Panel(string.Format("{0}_TabPage{1}", id, tabButtons.Count), Vector2.Empty, size);

//            Rectangle tabRectangle = tabButtonFont.MeasureString(UI.CurrentHud.SpriteManager, tabLabel,
//                                                                 DrawTextFormat.Left, Label.DefaultColor);

//            int tabBaseWidth = DefaultTabBaseWidth;
//            if (tabRectangle.Width > DefaultTabBaseWidth)
//                tabBaseWidth = tabRectangle.Width;

//            Button tabButton = new Button(string.Format("{0}_TabButton{1}", id, tabButtons.Count),
//                                          tabLabel,
//                                          new Vector2(totalWidth,
//                                                      -DefaultTabHeight),
//                                          new Size(tabBaseWidth, DefaultTabHeight),
//                                          Shape.RightTrapezoidUpside,
//                                          Shapes.ShadeTopToBottom,
//                                          tabButtonColorArray
//                );

//            totalWidth += tabBaseWidth + DefaultTabTriangleWidth;

//            tabButton.MouseClick += new MouseEventHandler(tabButton_MouseClick);

//            currentTab = tabPage;

//            foreach (Button button in tabButtons)
//                button.IsSelected = false;
//            foreach (Panel panel in tabPanels)
//                panel.IsVisible = false;

//            tabButtons.Add(tabButton);
//            tabPanels.Add(tabPage);

//            tabButton.IsSelected = true;

//            Add(currentTab);
//            Add(tabButton);
//        }

//        void tabButton_MouseClick(BaseControl ctl, MouseEventArgs args)
//        {
//            int newIndex = tabButtons.IndexOf(ctl as Button);
//            if (newIndex != selectedIndex)
//                Select(newIndex);
//        }

//        public void Select(int tabPage)
//        {
//            if (tabPage != selectedIndex)
//            {
//                currentTab = tabPanels[tabPage];
//                foreach (Button tabButton in tabButtons)
//                    tabButton.IsSelected = false;

//                UI.CurrentHud.BeginDesign();
//                foreach (Panel tabPanel in tabPanels)
//                    tabPanel.IsVisible = false;

//                tabButtons[tabPage].IsSelected = true;
//                currentTab.IsVisible = true;

//                selectedIndex = tabPage;
//                OnSelectedIndexChanged(this);
//                UI.CurrentHud.EndDesign();
//            }
//        }

//        public void AddControlInTab(BaseControl control, int tabPage)
//        {
//            tabPanels[tabPage].Add(control);
//        }

//        #region ISpriteControl Members

//        public void Render()
//        {
//            foreach (Button bt in tabButtons)
//                bt.Render();
//        }

//        #endregion

//        #region IRenderable overriden methods

//        public override void ComputeAbsolutePosition()
//        {
//            base.ComputeAbsolutePosition();

//            //foreach (Button button in tabButtons)
//            //    button.Parent = this;

//            //foreach (Panel tabPage in tabPanels)
//            //    tabPage.Parent = this;
//        }

//        #endregion
//    }
//}