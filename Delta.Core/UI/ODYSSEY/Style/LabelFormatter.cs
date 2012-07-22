//using System;
//using System.Collections.Generic;
//using System.Collections.Specialized;
//using System.Drawing;
//using AvengersUTD.Odyssey.UserInterface.RenderableControls;
//using Microsoft.DirectX;

//namespace AvengersUTD.Odyssey.UserInterface.Style
//{

//    #region Structs

//    internal struct CommandInfo
//    {
//        string command;
//        string value;


//        public string Command
//        {
//            get { return command; }
//        }

//        public string Value
//        {
//            get { return value; }
//        }

//        public CommandInfo(string command, string value)
//        {
//            this.command = command;
//            this.value = value;
//        }
//    }

//    internal struct LabelInfo
//    {
//        Vector2 insertionPoint;
//        TextStyle style;
//        string text;


//        public LabelInfo(string text, Vector2 insertionPoint, TextStyle style)
//        {
//            this.insertionPoint = insertionPoint;
//            this.style = style;
//            this.text = text;
//        }


//        public Vector2 InsertionPoint
//        {
//            get { return insertionPoint; }
//        }

//        public TextStyle Style
//        {
//            get { return style; }
//        }

//        public string Text
//        {
//            get { return text; }
//        }
//    }

//    #endregion

//    public class LabelFormatter
//    {
//        string baseTag;
//        TextStyle defaultStyle;
//        TextStyle currentStyle;
//        Vector2 startCursor;
//        Vector2 cursor;
//        Size size;
//        string code;

//        int labelIndex = 0;
//        List<LabelInfo> labelInfoCollection;
//        List<Label> labelCollection;
//        LabelPage page;

//        public LabelFormatter(string baseTag, TextStyle defaultStyle, Vector2 startCursor, Size size, string code)
//        {
//            this.baseTag = baseTag;
//            this.defaultStyle = defaultStyle;
//            this.startCursor = cursor = startCursor;
//            this.size = size;
//            this.code = code;

//            labelInfoCollection = new List<LabelInfo>();
//            labelCollection = new List<Label>();
//            page = new LabelPage();
//        }


//        public LabelPage FormattedLabelCollection
//        {
//            get
//            {
//                CreateLabels(ParseLabelMarkup(code));
//                return page;
//            }
//        }


//        List<CommandInfo> ParseLabelMarkup(string value)
//        {
//            char[] charArray = value.ToCharArray();
//            string markupText, labelText;
//            markupText = labelText = string.Empty;
//            List<CommandInfo> commandInfoList = new List<CommandInfo>();

//            bool markup = false;


//            for (int i = 0; i < charArray.Length; i++)
//            {
//                char c = charArray[i];
//                if (markup)
//                {
//                    if (c == ']')
//                        markup = false;
//                    else
//                        markupText += c;
//                }
//                else
//                {
//                    if (c == '[')
//                    {
//                        if (labelText != string.Empty)
//                            commandInfoList.Add(new CommandInfo(markupText, labelText));
//                        markupText = labelText = string.Empty;

//                        if (charArray[i + 1] == '/')
//                        {
//                            i += 2;
//                            continue;
//                        }

//                        markup = true;
//                    }
//                    else
//                        labelText += c;
//                }
//            }
//            commandInfoList.Add(new CommandInfo(markupText, labelText));

//            return commandInfoList;
//        }

//        void CheckForCarriageReturn(string text)
//        {
//            int index = text.IndexOf('\n');
//            if (index != -1)
//            {
//                List<string> lines = new List<string>();

//                while ((index = text.IndexOf('\n')) != -1)
//                {
//                    if (index == 0)
//                    {
//                        lines.Add(string.Empty);
//                        text = text.Remove(0, 1);
//                    }
//                    else
//                    {
//                        lines.Add(text.Substring(0, index));
//                        text = text.Substring(index+1);
//                    }
//                }

//                lines.Add(text);

//                for (int i = 0; i < lines.Count - 1; i++)
//                {
//                    string s = lines[i];
//                    if (s == string.Empty)
//                        GotoNextLine();
//                    else
//                    {
//                        CheckForWordWrap(s);
//                        GotoNextLine();
//                    }
//                }
//                CheckForWordWrap(lines[lines.Count - 1]);
//            }
//            else
//                CheckForWordWrap(text);
//        }

//        void GotoNextLine()
//        {
//            cursor = new Vector2(startCursor.X, cursor.Y + currentStyle.Size);
//            page.InsertLine();
//        }

//        void CheckForWordWrap(string text)
//        {
//            if (ComputeLineLength(cursor.X, Label.MeasureText(text, currentStyle).Width) > size.Width)
//            {
//                string nextLine = string.Empty;
//                string[] wordList = text.TrimEnd(null).Split(null);
//                for (int i = wordList.Length - 1; i >= 0; i--)
//                {
//                    nextLine = nextLine.Insert(0, wordList[i] + ' ');
//                    text = text.Remove(text.Length - wordList[i].Length, wordList[i].Length).TrimEnd();

//                    if (ComputeLineLength(cursor.X, Label.MeasureText(text, currentStyle).Width) <= size.Width)
//                    {
//                        labelInfoCollection.Add(new LabelInfo(text, cursor, currentStyle));
//                        GotoNextLine();
//                        CheckForWordWrap(nextLine.TrimEnd());
//                        break;
//                    }
//                }

//                // in case the last line has only one word.
//                if (text == string.Empty && nextLine != string.Empty)
//                {
//                    GotoNextLine();
//                    labelInfoCollection.Add(new LabelInfo(nextLine, cursor, currentStyle));
//                }
//            }
//            else
//            {
//                labelInfoCollection.Add(new LabelInfo(text, cursor, currentStyle));
//                cursor.X += Label.MeasureText(text, currentStyle).Width;
//            }
//        }

//        void DebugCmd(NameValueCollection commandList)
//        {
//            for (int i = 0; i < commandList.Count; i++)
//            {
//                string markup = commandList.GetKey(i);
//                string labelText = commandList[i];
//                System.Diagnostics.Debug.WriteLine(markup + ' ' + labelText);
//            }
//        }

//        void Process(List<CommandInfo> commandInfoList)
//        {
//            for (int i = 0; i < commandInfoList.Count; i++)
//            {
//                string markup = commandInfoList[i].Command;
//                string labelText = commandInfoList[i].Value;
//                int prevLength = labelText.Length;
//                labelText = labelText.TrimEnd(null);
//                int whiteSpaces = prevLength - labelText.Length;

//                if (markup == string.Empty)
//                    currentStyle = defaultStyle;
//                else
//                    currentStyle = new TextStyle(markup);

//                CheckForCarriageReturn(labelText);

//                // See if any whitespaces are to be added
//                if (whiteSpaces > 0)
//                    cursor.X += whiteSpaces*(currentStyle.Size/4);
//            }
//        }

//        void CreateLabels(List<CommandInfo> commandInfoList)
//        {
//            Process(commandInfoList);
//            foreach (LabelInfo labelInfo in labelInfoCollection)
//            {
//                Add(new Label(string.Format("{0}_{1}", baseTag, labelIndex),
//                              labelInfo.Text,
//                              labelInfo.InsertionPoint,
//                              labelInfo.Style));
//            }
//        }


//        void Debug()
//        {
//            foreach (Label l in labelCollection)
//            {
//                Console.WriteLine(l.Text + " - " + l.ID);
//            }
//        }

//        void Add(Label label)
//        {
//            //labelCollection.Add(label);
//            page.Append(label);
//            labelIndex++;
//        }

//        float ComputeLineLength(float insertionPoint, float additionLength)
//        {
//            return insertionPoint + additionLength - startCursor.X;
//        }
//    }
//}