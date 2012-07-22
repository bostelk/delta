//#region Using directives

//using System;
//using System.Collections.Generic;
//using System.Drawing.Text;
//using Microsoft.DirectX.Direct3D;

//#endregion

//namespace AvengersUTD.Odyssey.UserInterface
//{
//    public static class FontManager
//    {
//        public const string DefaultFontName = "Arial";

//        static Dictionary<string, Font> fontCache = new Dictionary<string, Font>();
//        static Font defaultFont = CreateFont(DefaultFontName, LabelSize.Normal, FontWeight.Normal, false);

//        // future use;
//        static PrivateFontCollection fontCollection;

//        #region Properties

//        public static Font DefaultFont
//        {
//            get { return defaultFont; }
//        }

//        #endregion

//        /// <summary>
//        /// Creates a user defined font object.
//        /// </summary>
//        /// <param name="fontName">The font name</param>
//        /// <param name="size">Font size.</param>
//        /// <param name="weight">Bold/Demi bold, etc.</param>
//        /// <param name="isItalic">True if the text is too be rendered in italic.</param>
//        /// <returns>The font object.</returns>
//        public static Font CreateFont(string fontName, int size, FontWeight weight, bool isItalic)
//        {
//            String fontKey = fontName + size + weight.ToString() + isItalic.ToString();

//            if (fontCache.ContainsKey(fontKey))
//                return fontCache[fontKey];
//            else
//            {
//                Font font = new Font(
//                    UI.Device,
//                    size,
//                    0,
//                    weight,
//                    1,
//                    isItalic,
//                    CharacterSet.Default,
//                    Precision.Tt,
//                    FontQuality.AntiAliased | FontQuality.ClearTypeNatural,
//                    PitchAndFamily.FamilyRoman, fontName
//                    );

//                fontCache.Add(fontKey, font);
//                return font;
//            }
//        }

//        /// <summary>
//        /// Returns a font object from the default type.
//        /// </summary>
//        /// <param name="size">Font size.</param>
//        /// <param name="weight">Bold/Demi bold, etc.</param>
//        /// <param name="italic">True if the text is too be rendered in italic.</param>
//        /// <returns>The font object.</returns>
//        public static Font CreateFont(int size, FontWeight weight, bool italic)
//        {
//            return CreateFont(DefaultFontName, size, weight, italic);
//        }

//        /// <summary>
//        /// Creates a font object from the default type specifying the size value to
//        /// use. Use this overload if you don't want to set a font size value for each
//        /// label created. Define those amounts by setting their properties in the 
//        /// FontManager class before creating the labels.
//        /// <remarks>For example, if you want to use a bigger screen resolution, you
//        /// should increase the font size value stored in the [size]FontSize properties
//        /// as needed and then using the related LabelSize enum value to create 
//        /// the labels you need.</remarks>
//        /// </summary>
//        /// <param name="fontName">Name of the font family.</param>
//        /// <param name="labelSize">Font size.</param>
//        /// <param name="weight">Bold/Demi bold, etc.</param>
//        /// <param name="italic">True if the text is too be rendered in italic.</param>
//        /// <returns>The font object.</returns>
//        public static Font CreateFont(LabelSize size, FontWeight weight, bool italic)
//        {
//            return CreateFont(DefaultFontName, size, weight, italic);
//        }

//        /// <summary>
//        /// Creates a font object from the specified type specifying the size value to
//        /// use. Use this overload if you don't want to set a font size value for each
//        /// label created. Define those amounts by setting their properties in the 
//        /// FontManager class before creating the labels.
//        /// <remarks>For example, if you want to use a bigger screen resolution, you
//        /// should increase the font size value stored in the [size]FontSize properties
//        /// as needed and then using the related LabelSize enum value to create 
//        /// the labels you need.</remarks>
//        /// </summary>
//        /// <param name="fontName">Name of the font family.</param>
//        /// <param name="labelSize">Font size.</param>
//        /// <param name="weight">Bold/Demi bold, etc.</param>
//        /// <param name="italic">True if the text is too be rendered in italic.</param>
//        /// <returns>The font object.</returns>
//        public static Font CreateFont(string fontName, LabelSize labelSize, FontWeight weight, bool italic)
//        {
//            return CreateFont(StyleManager.GetLabelSize(labelSize), weight, italic);
//        }
//    }
//}