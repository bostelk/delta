//using System;
//using System.Drawing;
//using AvengersUTD.Odyssey.UserInterface.Style;
//using Microsoft.DirectX;
//using System.Windows.Forms;
//using System.Threading;

//namespace AvengersUTD.Odyssey.UserInterface.RenderableControls
//{
//    public enum DialogBoxButtons
//    {
//        Ok,
//        OkCancel,
//        YesNo
//    }

//    public enum DialogResult
//    {
//        None,
//        Ok,
//        Cancel,
//        Yes,
//        No
//    }


//    public delegate void DialogResultEventHandler(BaseControl sender, DialogResult dialogResult);

//    public class DialogBox : Window
//    {
//        static Size DefaultButtonSize = new Size(90, 30);
//        static Size DefaultDialogSize = new Size(500, 250);
//        const int DefaultButtonPaddingX = 5;
//        const int DefaultButtonPaddingY = 5;

//        public event DialogResultEventHandler DialogResultAvailable;

//        DialogResult dialogResult;
//        DialogResult closeBehavior;

//        DialogBox(string title, Vector2 position, string text, DialogBoxButtons buttons): base(
//            string.Format("{0}_Dialog{1}",title, UI.CurrentHud.WindowManager.Count),
//            title, position, DefaultDialogSize)

//        {
//            isModal = true;
//            containerPanel.Size = new Size(containerPanel.Size.Width, containerPanel.Size.Height -
//                DefaultButtonSize.Height - DefaultButtonPaddingY);
//            RichTextArea rta = new RichTextArea(title + "_Rta",
//                                                new Vector2(0, 0),
//                                                containerPanel.Size, text, TextStyle.Default);

//            Add(rta);

//            captionBar.CloseButton.MouseClick +=
//                delegate(BaseControl ctl, MouseEventArgs args)
//                    {
//                        dialogResult = closeBehavior;
//                        OnDialogResultAvailable(this, dialogResult);
//                    };


//            switch (buttons)
//            {
//                default:
//                case DialogBoxButtons.Ok:
//                    closeBehavior = DialogResult.Ok;
//                    AddButtonsTo(1, new string[] { "Ok" },
//                                 new MouseEventHandler[]
//                                     {
//                                         new MouseEventHandler(ReturnOk)
//                                     });
//                    break;

//                case DialogBoxButtons.OkCancel:
//                    closeBehavior = DialogResult.Cancel;
//                    AddButtonsTo(2, new string[] { "Ok", "Cancel" },
//                                 new MouseEventHandler[]
//                                     {
//                                         new MouseEventHandler(ReturnOk),
//                                         new MouseEventHandler(ReturnCancel), 
//                                     });
//                    break;

//                case DialogBoxButtons.YesNo:
//                    closeBehavior = DialogResult.No;
//                    AddButtonsTo(2, new string[] { "Yes", "No" },
//                                 new MouseEventHandler[]
//                                     {
//                                         new MouseEventHandler(ReturnYes),
//                                         new MouseEventHandler(ReturnNo), 
//                                     });
//                    break;
//            }
//        }

//        public static void Show(string title, string text, DialogBoxButtons buttons, DialogResultEventHandler eventHandler)
//        {
//            HUD hud = UI.CurrentHud;
//            hud.BeginDesign();
//            Size hudSize = hud.Size;
//            Vector2 topLeft = new Vector2(hudSize.Width/2 - DefaultDialogSize.Width/2,
//                                          hudSize.Height/2 - DefaultDialogSize.Height/2);
//            DialogBox dialog = new DialogBox(title, topLeft, text, buttons);

//            hud.Add(dialog);
//            hud.WindowManager.BringToFront(dialog);
//            hud.EndDesign();
//            dialog.DialogResultAvailable += eventHandler;

//        }


//        void AddButtonsTo(int number, string[] labels, params MouseEventHandler[] handlers)
//        {
//            if (labels.Length != number && handlers.Length != number)
//                throw new ArgumentException("You need to supply an equal number of buttons, labels and event handlers");

//            for (int i = 0; i < number; i++)
//            {
//                Vector2 buttonPosition = new Vector2(
//                    // The formula used in the code is a 'simplified' version of the one commented below:
//                    //dialog.InternalSize.Width - ((number-i) * DefaultButtonSize.Width + (number-1) * DefaultButtonPaddingX) + (i * DefaultButtonPaddingX),
//                    containerPanel.Size.Width + DefaultButtonSize.Width*(i - number) +
//                    DefaultButtonPaddingX*(i + 1 - number),
//                    containerPanel.Size.Height + DefaultButtonPaddingY);
//                Button button = new Button(string.Format("{0}Button", labels[i]),
//                                           labels[i],
//                                           buttonPosition,
//                                           DefaultButtonSize);

//                button.MouseClick += handlers[i];


//                Add(button);
//            }
//        }

//        void ReturnOk(BaseControl ctl, MouseEventArgs args)
//        {
//            dialogResult = DialogResult.Ok;
//            Close();
//            OnDialogResultAvailable(this, dialogResult);
//        }

//        void ReturnCancel(BaseControl ctl, MouseEventArgs args)
//        {
//            dialogResult = DialogResult.Cancel;
//            Close();
//            OnDialogResultAvailable(this, dialogResult);
//        }

//        void ReturnYes(BaseControl ctl, MouseEventArgs args)
//        {
//            dialogResult = DialogResult.Yes;
//            Close();
//            OnDialogResultAvailable(this, dialogResult);
//        }

//        void ReturnNo(BaseControl ctl, MouseEventArgs args)
//        {
//            dialogResult = DialogResult.No;
//            Close();
//            OnDialogResultAvailable(this, dialogResult);
//        }

//        public virtual void OnDialogResultAvailable(BaseControl ctl, DialogResult dialogResult)
//        {
//            if (DialogResultAvailable!=null)
//                DialogResultAvailable(ctl, dialogResult);
//        }
        
//    }
//}