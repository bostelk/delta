using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Design;
using System.ComponentModel;
using System.Windows.Forms.Design;
using Microsoft.Xna.Framework;

namespace Delta.Editor
{
    public class ColorUIEditor : UITypeEditor
    {
        ColorPicker _colorPicker = new ColorPicker();

        public ColorUIEditor()
        {
        }

        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.DropDown;
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            if (context != null && context.Instance != null && provider != null)
            {
                IWindowsFormsEditorService editorService = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
                if (editorService != null)
                {
                    editorService.DropDownControl(_colorPicker);
                    return new Color(_colorPicker.SelectedColor);
                }
            }
            return null;
        }
    }
}
