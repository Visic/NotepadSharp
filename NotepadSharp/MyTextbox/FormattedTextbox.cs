using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Windows.Media;
using Utility;

namespace NotepadSharp {
    public class FormattedTextbox : Control {
        DispatcherTimer _blinkTimer = new DispatcherTimer();
        ITextFormatter _defaultFormatter;

        public FormattedTextbox() {
            Background = Brushes.Transparent;
            Cursor = Cursors.IBeam;

            Loaded += FormattedTextbox_Loaded;

            _blinkTimer.Interval = TimeSpan.FromMilliseconds(500);
            _blinkTimer.Tick += _blinkTimer_Tick;
            _blinkTimer.Start();

            Text = Constants.LoremIpsum;
        }

        #region Dependency Properties
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
            "Text",
            typeof(string),
            typeof(FormattedTextbox),
            new FrameworkPropertyMetadata(TextChanged)
        );

        public string Text {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        private static void TextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            var ctrl = (FormattedTextbox)d;
            ctrl.InvalidateVisual();
        }

        public static readonly DependencyProperty TextFormatterProperty = DependencyProperty.Register(
            "TextFormatter",
            typeof(ITextFormatter),
            typeof(FormattedTextbox),
            new FrameworkPropertyMetadata(TextFormatterChanged)
        );

        public ITextFormatter TextFormatter {
            get { return (ITextFormatter)GetValue(TextFormatterProperty); }
            set { SetValue(TextFormatterProperty, value); }
        }

        private static void TextFormatterChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            var ctrl = (FormattedTextbox)d;
            ctrl.TextFormatter.DefaultFontFamily = ctrl.TextFormatter?.DefaultFontFamily ?? ctrl.FontFamily;
            ctrl.TextFormatter.DefaultFontSize = ctrl.TextFormatter?.DefaultFontSize ?? ctrl.FontSize;
            ctrl.TextFormatter.DefaultFontStretch = ctrl.TextFormatter?.DefaultFontStretch ?? ctrl.FontStretch;
            ctrl.TextFormatter.DefaultFontStyle = ctrl.TextFormatter?.DefaultFontStyle ?? ctrl.FontStyle;
            ctrl.TextFormatter.DefaultFontWeight = ctrl.TextFormatter?.DefaultFontWeight ?? ctrl.FontWeight;
            ctrl.TextFormatter.DefaultFontColor = ctrl.TextFormatter?.DefaultFontColor ?? ctrl.Foreground;
            ctrl.InvalidateVisual();
        }
        #endregion

        protected override void OnMouseDown(MouseButtonEventArgs e) {
            base.OnMouseDown(e);
            Focus();
        }

        protected override void OnRender(DrawingContext drawingContext) {
            base.OnRender(drawingContext);

            //Background (makes the control clickable outside of just the text)
            drawingContext.DrawRectangle(Background, new Pen(), new Rect(0, 0, ActualWidth, ActualHeight));
            if(string.IsNullOrEmpty(Text) || _defaultFormatter == null) return; //not yet loaded, or no text to render

            //render the text
            var nextLoc = new Point(0, 0);
            foreach(var ele in (TextFormatter ?? _defaultFormatter).Format(Text)) {
                foreach(var line in WordWrap_Rec(nextLoc.X, ele)) {
                    drawingContext.DrawText(line, nextLoc);

                    nextLoc.X = (int)((nextLoc.X + line.Width) / ActualWidth);
                    nextLoc.Y = nextLoc.Y + line.Height;
                }
            }
        }

        //Recursively wrap the given text to return a collection of lines
        private IEnumerable<FormattedText> WordWrap_Rec(double originX, TextWithFormatting txt) {
            var result = new List<FormattedText>();
            var requiredWidth = originX + txt.GetFormattedText().Width;

            if(requiredWidth > ActualWidth) {
                var approxChars = (int)(txt.Text.Length / (requiredWidth / ActualWidth));

                var str1 = txt.Text.Substring(0, approxChars);
                var whiteSpaceIndex = str1.LastIndexOfAny(new[] { ' ', '\t' });
                if (whiteSpaceIndex > -1) str1 = str1.Substring(0, whiteSpaceIndex + 1);

                result.Add(WordWrap_Rec(originX, new TextWithFormatting(str1, txt)).First());

                var str2 = txt.Text.Substring(result[0].Text.Length, txt.Text.Length - result[0].Text.Length);
                result.AddRange(WordWrap_Rec(0, new TextWithFormatting(str2, txt)));
            } else {
                result.Add(txt.GetFormattedText());
            }
            return result;
        }

        private void FormattedTextbox_Loaded(object sender, RoutedEventArgs e) {
            _defaultFormatter = new TextFormatter() {
                DefaultFontFamily = FontFamily,
                DefaultFontSize = FontSize,
                DefaultFontStretch = FontStretch,
                DefaultFontStyle = FontStyle,
                DefaultFontWeight = FontWeight,
                DefaultFontColor = Foreground
            };
            if(TextFormatter == null) InvalidateVisual();
        }

        private void _blinkTimer_Tick(object sender, EventArgs e) {
            if(!HasEffectiveKeyboardFocus) return;

            //TODO:: blink the cursor
        }
    }
}
