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

            //Text = Constants.LoremIpsum;
            //Text = new string('W', 5000000);
            //Text = string.Join("", Enumerable.Repeat("abcdefghijklmnopqrstuvwxyz", 50).ToArray());
            //Text = string.Join(" --- ", Enumerable.Repeat(Constants.LoremIpsum, 50).ToArray());
            Text = string.Join(" --- ", Enumerable.Repeat(Constants.LoremIpsum, 5000).ToArray());
            //Text = new string('A', 100) + "abcdefghi";
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

        //TODO:: Optimize by reducing the memory allocations (e.g. stop copying strings)
        //TODO:: Optimize by not re-rendering unless the screen size has changed by an appreciable amount since last time we rendered, or unless we trigger it ourselves
        protected override void OnRender(DrawingContext drawingContext) {
            base.OnRender(drawingContext);

            //Background (makes the control clickable outside of just the text)
            drawingContext.DrawRectangle(Background, new Pen(), new Rect(0, 0, ActualWidth, ActualHeight));
            if(string.IsNullOrEmpty(Text) || _defaultFormatter == null) return; //not yet loaded, or no text to render

            //render the text
            var endOfLine = new Point(0, 0);
            var nextLine = new Point(0, 0);
            foreach(var ele in (TextFormatter ?? _defaultFormatter).Format(Text)) {
                var pos = endOfLine;
                foreach(var line in WordWrap(pos.X, ele)) {
                    var lineHeight = line.Item1.Height;
                    if(pos.Y + lineHeight > ActualHeight) return; //We've run out of room for lines, done rendering text
                    drawingContext.DrawText(line.Item1, pos);
                    endOfLine.X = line.Item2;

                    if(pos == nextLine) {
                        endOfLine.Y = nextLine.Y;
                        nextLine.Y += lineHeight;
                    } else {
                        var maybeNextLineY = endOfLine.Y + lineHeight; //handle change in font size within one line of text
                        nextLine.Y = nextLine.Y < maybeNextLineY ? maybeNextLineY : nextLine.Y;
                    }

                    pos = nextLine;
                }
            }
        }

        TextWithFormatting _lastTxt;
        Dictionary<char, double> _knownCharSizes;
        private double GetCharSize(char ch, TextWithFormatting txt) {
            if(_lastTxt == null || !_lastTxt.SizeFactorsAreEqual(txt)) {
                _lastTxt = txt;
                _knownCharSizes = new Dictionary<char, double>();
            }

            return _knownCharSizes.GetOrAdd(ch, () => new TextWithFormatting(ch.ToString(), txt).GetFormattedText().WidthIncludingTrailingWhitespace);
        }

        private IEnumerable<Tuple<FormattedText, double>> WordWrap(double offsetX, TextWithFormatting txt) {
            var startIndex = 0;
            while(startIndex < txt.Text.Length) {
                var txtWidth = 0.0;
                var remainingWidth = ActualWidth - offsetX;

                int lastWhiteSpaceIndex = -1;
                int endIndex = startIndex;
                while(remainingWidth > 0 && endIndex < txt.Text.Length) {
                    var ch = txt.Text[endIndex];
                    var chWidth = GetCharSize(ch, txt);
                    remainingWidth -= chWidth;

                    if(remainingWidth >= 0) {
                        txtWidth += chWidth;
                        if(ch == ' ' || ch == '\t') lastWhiteSpaceIndex = endIndex;
                        ++endIndex;
                    } else if(lastWhiteSpaceIndex > -1) {
                        endIndex = lastWhiteSpaceIndex;
                    }
                }
                yield return Tuple.Create(new TextWithFormatting(txt.Text.Substring(startIndex, endIndex - startIndex), txt).GetFormattedText(), txtWidth);
                startIndex = endIndex;
                offsetX = 0;
            }
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
