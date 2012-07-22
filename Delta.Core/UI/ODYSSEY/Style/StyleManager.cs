//using D3DX = Microsoft.DirectX.Direct3D;

//namespace AvengersUTD.Odyssey.UserInterface
//{
//    public static class StyleManager
//    {
//        static int smallFontSize = 16;
//        static int normalFontSize = 20;
//        static int largeFontSize = 24;
//        static int veryLargeFontSize = 32;
//        static int hugeFontSize = 48;

//        #region Properties

//        public static int HugeFontSize
//        {
//            get { return hugeFontSize; }
//            set { hugeFontSize = value; }
//        }

//        public static int NormalFontSize
//        {
//            get { return normalFontSize; }
//            set { normalFontSize = value; }
//        }

//        public static int LargeFontSize
//        {
//            get { return largeFontSize; }
//            set { largeFontSize = value; }
//        }

//        public static int VeryLargeFontSize
//        {
//            get { return veryLargeFontSize; }
//            set { veryLargeFontSize = value; }
//        }

//        public static int SmallFontSize
//        {
//            get { return smallFontSize; }
//            set { smallFontSize = value; }
//        }

//        #endregion

//        public static int GetLabelSize(LabelSize labelSize)
//        {
//            int size;

//            switch (labelSize)
//            {
//                case LabelSize.Small:
//                    size = smallFontSize;
//                    break;

//                default:
//                case LabelSize.Normal:
//                    size = normalFontSize;
//                    break;

//                case LabelSize.Large:
//                    size = largeFontSize;
//                    break;

//                case LabelSize.VeryLarge:
//                    size = veryLargeFontSize;
//                    break;

//                case LabelSize.Huge:
//                    size = hugeFontSize;
//                    break;
//            }
//            return size;
//        }
//    }
//}