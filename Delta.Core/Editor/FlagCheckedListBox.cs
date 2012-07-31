using System;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms.Design;

namespace Delta.Editor
{

    [EditorBrowsable(EditorBrowsableState.Never)]
    public class FlagCheckedListBoxItem
    {
        internal int _value = 0;
        internal string _caption = string.Empty;

        public FlagCheckedListBoxItem(int value, string caption)
        {
            _value = value;
            _caption = caption;
        }

        public override string ToString()
        {
            return _caption;
        }

        public bool IsFlag { get { return ((_value & (_value - 1)) == 0); } }

        public bool IsMemberFlag(FlagCheckedListBoxItem item)
        {
            return (IsFlag && ((_value & item._value) == _value));
        }
    }

    public class FlagCheckedListBox : CheckedListBox
    {

        Container _components = null;
        bool _isUpdatingCheckStates = false;
        Type _enumType = null;
        Enum _enumValue = null;

        public FlagCheckedListBox()
        {
            InitializeComponent();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_components != null)
                    _components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            CheckOnClick = true;
        }

        public FlagCheckedListBoxItem Add(int value, string caption)
        {
            FlagCheckedListBoxItem item = new FlagCheckedListBoxItem(value, caption);
            Items.Add(item);
            return item;
        }

        public FlagCheckedListBoxItem Add(FlagCheckedListBoxItem item)
        {
            Items.Add(item);
            return item;
        }

        protected override void OnItemCheck(ItemCheckEventArgs e)
        {
            base.OnItemCheck(e);
            if (_isUpdatingCheckStates)
                return;
            FlagCheckedListBoxItem item = Items[e.Index] as FlagCheckedListBoxItem;
            UpdateCheckedItems(item, e.NewValue);
        }

        protected void UpdateCheckedItems(int value)
        {
            _isUpdatingCheckStates = true;

            for (int i = 0; i < Items.Count; i++)
            {
                FlagCheckedListBoxItem item = Items[i] as FlagCheckedListBoxItem;
                if (item._value == 0)
                    SetItemChecked(i, value == 0);
                else
                {
                    if ((item._value & value) == item._value && item._value != 0)
                        SetItemChecked(i, true);
                    else
                        SetItemChecked(i, false);
                }
            }
            _isUpdatingCheckStates = false;
        }

        protected void UpdateCheckedItems(FlagCheckedListBoxItem item, CheckState checkState)
        {
            if (item._value == 0)
                UpdateCheckedItems(0);

            int sumValue = 0;
            for (int x = 0; x < Items.Count; x++)
            {
                FlagCheckedListBoxItem i = Items[x] as FlagCheckedListBoxItem;
                if (GetItemChecked(x))
                    sumValue |= i._value;
            }

            if (checkState == CheckState.Unchecked)
                sumValue = sumValue & (~item._value);
            else
                sumValue |= item._value;

            UpdateCheckedItems(sumValue);
        }

        public int GetCurrentValue()
        {
            int sumValue = 0;
            for (int x = 0; x < Items.Count; x++)
            {
                FlagCheckedListBoxItem item = Items[x] as FlagCheckedListBoxItem;
                if (GetItemChecked(x))
                    sumValue |= item._value;
            }
            return sumValue;
        }

        private void FillEnumMembers()
        {
            foreach (string name in Enum.GetNames(_enumType))
                Add((int)Convert.ChangeType(Enum.Parse(_enumType, name), typeof(int)), name);
        }

        private void ApplyEnumValue()
        {
            int intValue = (int)Convert.ChangeType(_enumValue, typeof(int));
            UpdateCheckedItems(intValue);
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Enum EnumValue
        {
            get
            {
                object e = Enum.ToObject(_enumType, GetCurrentValue());
                return (Enum)e;
            }
            set
            {
                Items.Clear();
                _enumValue = value; // Store the current enum value
                _enumType = value.GetType(); // Store enum type
                FillEnumMembers(); // Add items for enum members
                ApplyEnumValue(); // Check/uncheck items depending on enum value

            }
        }


    }
}
