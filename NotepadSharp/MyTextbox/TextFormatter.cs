using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace NotepadSharp {
    public class TextFormatter : ITextFormatter {
        //Note:: The defaults will get set by the FormattedTextbox if they are not already set
        public FontFamily DefaultFontFamily { get; set; }
        public FontStyle? DefaultFontStyle { get; set; }
        public FontWeight? DefaultFontWeight { get; set; }
        public FontStretch? DefaultFontStretch { get; set; }
        public Brush DefaultFontColor { get; set; }
        public double? DefaultFontSize { get; set; }

        public virtual IEnumerable<TextWithFormatting> Format(string text) {
            //var parts = text.Split('a');
            //return new[] {
            //    new TextWithFormatting(
            //        parts[0],
            //        DefaultFontFamily,
            //        DefaultFontStyle.Value,
            //        DefaultFontWeight.Value,
            //        DefaultFontStretch.Value,
            //        DefaultFontColor,
            //        DefaultFontSize.Value
            //    ),

            //    new TextWithFormatting(
            //        text.Substring(parts[0].Length),
            //        DefaultFontFamily,
            //        DefaultFontStyle.Value,
            //        DefaultFontWeight.Value,
            //        DefaultFontStretch.Value,
            //        Brushes.Red,
            //        DefaultFontSize.Value
            //    )
            //};

            return new[] {
                new TextWithFormatting(
                    text,
                    DefaultFontFamily,
                    DefaultFontStyle.Value,
                    DefaultFontWeight.Value,
                    DefaultFontStretch.Value,
                    DefaultFontColor
                )
            };
        }
    }
}
