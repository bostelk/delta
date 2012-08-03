using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Delta.Editor
{
    public class FlagEnumUIEditor : UITypeEditor
    {
        FlagCheckedListBox _flaggedEnumCheckedListBox = new FlagCheckedListBox();

        public FlagEnumUIEditor()
        {
            _flaggedEnumCheckedListBox.BorderStyle = BorderStyle.None;
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (context != null && context.Instance != null && provider != null)
            {
                IWindowsFormsEditorService editorService = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
                if (editorService != null)
                {
                    Enum e = (Enum)Convert.ChangeType(value, context.PropertyDescriptor.PropertyType);
                    _flaggedEnumCheckedListBox.EnumValue = e;
                    editorService.DropDownControl(_flaggedEnumCheckedListBox);
                    return _flaggedEnumCheckedListBox.EnumValue;
                }
            }
            return null;
        }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }
    }
}
