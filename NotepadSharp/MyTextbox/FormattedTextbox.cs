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
            Text = string.Join(" --- ", Enumerable.Repeat(Constants.LoremIpsum, 50).ToArray());
            //Text = string.Join(" --- ", Enumerable.Repeat(Constants.LoremIpsum, 5000).ToArray());
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
                foreach(var line in WordWrap_Rec(pos.X, ele)) {
                    if(pos.Y + line.Height > ActualHeight) return; //We've run out of room for lines, done rendering text
                    drawingContext.DrawText(line, pos);

                    if(pos == nextLine) {
                        endOfLine.X = line.WidthIncludingTrailingWhitespace;
                        endOfLine.Y = nextLine.Y;
                        nextLine.Y += line.Height;
                    } else {
                        endOfLine.X = line.WidthIncludingTrailingWhitespace;

                        var maybeNextLineY = endOfLine.Y + line.Height; //handle change in font size within one line of text
                        nextLine.Y = nextLine.Y < maybeNextLineY ? maybeNextLineY : nextLine.Y;
                    }

                    pos = nextLine;
                }
            }
        }

        //Control text is created by taking into consideration the frequency of the first 1k characters
        private string GetControlText(string txt) {
            txt = new string(txt.Take(1000).ToArray());
            var freqs = Methods.CountCharOccurrences(txt).OrderByDescending(x => x.Value).ToArray();
            var avg = freqs.Average(x => (double)x.Value);

            //now return a string consisting of the important characters weighted by their frequency of occurrence relative to the average
            return new string(freqs.SelectMany(x => Enumerable.Repeat(x.Key, (int)Math.Round(x.Value / avg))).ToArray());
        }

        private IEnumerable<FormattedText> WordWrap_Rec(double offsetX, TextWithFormatting txt) {
            var ctrlTxt = GetControlText(txt.Text);
            var approxWidthPerChar = new TextWithFormatting(ctrlTxt, txt).GetFormattedText().WidthIncludingTrailingWhitespace / ctrlTxt.Length;
            //TODO:: rather than using control text, maybe just calculate the width of all characters, and the width of space between characters
            var rest = txt.Text;
            while(!string.IsNullOrEmpty(rest)) {
                var remainingWidth = ActualWidth - offsetX;
                var numChars = (int)(remainingWidth / approxWidthPerChar);

                if(numChars < rest.Length) {
                    var strTxt = rest.Substring(0, numChars);
                    if(strTxt.TrimEnd(' ', '\t') == strTxt) {
                        var lastWhiteSpace = strTxt.LastIndexOfAny(new[] { ' ', '\t' });
                        if(lastWhiteSpace != -1) strTxt = strTxt.Substring(0, lastWhiteSpace + 1);
                    }

                    var str = WordWrap_Rec(
                        offsetX,
                        new TextWithFormatting(
                            strTxt,
                            txt
                        )
                    ).First();

                    yield return str;
                    rest = rest.Substring(str.Text.Length);
                    offsetX = 0;
                } else {
                    yield return new TextWithFormatting(rest, txt).GetFormattedText();
                    rest = "";
                }
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
