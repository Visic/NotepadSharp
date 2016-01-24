using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Utility;

namespace NotepadSharp {
    public class TextWithFormatting {
        public TextWithFormatting(string text, FontFamily font, FontStyle style, FontWeight weight, FontStretch strech, Brush fontColor) {
            Text = text;
            FontFamily = font;
            FontStyle = style;
            FontWeight = weight;
            FontStretch = strech;
            FontColor = fontColor;
        }

        public TextWithFormatting(string text, TextWithFormatting other) 
            : this(text, other.FontFamily, other.FontStyle, other.FontWeight, other.FontStretch, other.FontColor) 
        {
        }

        public string Text { get; }
        public FontFamily FontFamily { get; }
        public FontStyle FontStyle { get; }
        public FontWeight FontWeight { get; }
        public FontStretch FontStretch { get; }
        public Brush FontColor { get; }

        public FormattedText GetFormattedText(double fontSize) {
            return new FormattedText(
                Text, 
                CultureInfo.CurrentCulture, 
                FlowDirection.LeftToRight, 
                new Typeface(
                    FontFamily,
                    FontStyle,
                    FontWeight,
                    FontStretch
                ),
                fontSize,
                FontColor
            );
        }

        public bool SizeFactorsAreEqual(TextWithFormatting other) {
            return FontFamily == other.FontFamily &&
                   FontStyle == other.FontStyle &&
                   FontWeight == other.FontWeight &&
                   FontStretch == other.FontStretch;
        }
    }

    public interface ITextFormatter {
        FontFamily DefaultFontFamily { get; set; }
        FontStyle? DefaultFontStyle { get; set; }
        FontWeight? DefaultFontWeight { get; set; }
        FontStretch? DefaultFontStretch { get; set; }
        Brush DefaultFontColor { get; set; }
        IEnumerable<TextWithFormatting> Format(string text);
    }
}
