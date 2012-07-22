//using System.Drawing;
//using Microsoft.DirectX;

//namespace AvengersUTD.Odyssey.UserInterface.RenderableControls
//{
//    public class OptionGroup : ContainerControl
//    {
//        protected int selectedIndex;

//        // Future use
//        int fontSize;

//        #region Exposed Events

//        public event ControlEventHandler SelectedIndexChanged;

//        protected virtual void OnSelectedIndexChanged(BaseControl ctl)
//        {
//            if (SelectedIndexChanged != null)
//                SelectedIndexChanged(ctl);
//        }

//        #endregion

//        #region Properties

//        public int SelectedIndex
//        {
//            get { return selectedIndex; }
//            set { selectedIndex = value; }
//        }

//        #endregion

//        /// <summary>
//        /// Creates a list of OptionButtons.
//        /// </summary>
//        /// <param name="id">The ID of the control.</param>
//        /// <param name="optionLabels">The label to assign the various options, from top to bottom.</param>
//        /// <param name="position">The position of the first OptionButton center</param>
//        public OptionGroup(string id, string[] optionLabels, Vector2 position, Size optionButtonLabelSize)
//            : base(id, position,
//                   new Size(optionButtonLabelSize.Width, optionButtonLabelSize.Height*optionLabels.Length))
//        {
//            fontSize = StyleManager.SmallFontSize;

//            for (int i = 0; i < optionLabels.Length; i++)
//            {
//                string s = optionLabels[i];
//                Vector2 optionPosition = new Vector2(0,
//                                                     i*(fontSize));
//                OptionButton ob = new OptionButton(id + "_OB" + i.ToString("00"), i, s, optionPosition,
//                                                   optionButtonLabelSize);
//                Add(ob);
//            }
//            (controls[0] as OptionButton).IsSelected = true;
//            selectedIndex = 0;
//        }


//        public void Select(int optionNumber)
//        {
//            for (int i = 0; i < controls.Count; i++)
//            {
//                OptionButton ob = controls[i] as OptionButton;
//                if (i != optionNumber)
//                    ob.IsSelected = false;
//                else
//                    ob.IsSelected = true;

//                ob.Update();
//            }

//            selectedIndex = optionNumber;
//            OnSelectedIndexChanged(this);
//        }
//    }
//}