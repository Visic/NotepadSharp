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
using WPFUtility;

namespace NotepadSharp {
    public class FormattedTextbox : Control {
        public class Character : IComparable {
            public double EndX { get; set; }
            public double Width { get; set; }
            public char Ch { get; set; }
            public int AbsoluteCharacterIndex { get; set; }

            public int CompareTo(object other) {
                var pos = (other as Character)?.EndX ?? (double)other;
                return EndX.CompareTo(pos);
            }
        }

        public class Line : IComparable, IComparable<Line> {
            public double Y { get; set; }
            public double Height { get; set; }
            public double Width { get; set; }
            public FormattedText Text { get; set; }
            public OrderedList<Character> Chars { get; set; } = new OrderedList<Character>();

            public int CompareTo(object other) {
                if (other is Line) return CompareTo((Line)other);
                return Y.CompareTo((double)other);
            }

            public int CompareTo(Line other) {
                return Chars[0].AbsoluteCharacterIndex.CompareTo(other.Chars[0].AbsoluteCharacterIndex);
            }
        }

        public class Caret {
            OrderedList<Line> _visibleLines;
            Func<bool> _scrollUpOneLine, _scrollDownOneLine;

            public Caret(Tuple<int, int> lineAndCharIndicies, OrderedList<Line> visibleLines, Func<bool> scrollUpOneLine, Func<bool> scrollDownOneLine) 
                : this(lineAndCharIndicies.Item1, lineAndCharIndicies.Item2, visibleLines, scrollUpOneLine, scrollDownOneLine) { }

            public Caret(int visibileLineIndex, int charIndex, OrderedList<Line> visibleLines, Func<bool> scrollUpOneLine, Func<bool> scrollDownOneLine) {
                VisibileLineIndex = visibileLineIndex < 0 ? 0 : visibileLineIndex;
                _scrollUpOneLine = scrollUpOneLine;
                _scrollDownOneLine = scrollDownOneLine;
                _lineRelativeCharacterIndex = charIndex;
                _visibleLines = visibleLines;
            }

            public int VisibileLineIndex { get; set; }
            public Brush Brush { get; set; } = Brushes.Green;

            int _lineRelativeCharacterIndex;
            public int LineRelativeCharacterIndex {
                get {
                    //Because of the way scrolling down works with _visibleLines (lazily updated on Render)
                    //our index might be past the end of the line if we just called [MoveDown]
                    //In addition, this approach simplifies MoveDown and MoveUp
                    if(_visibleLines.Count > 0) {
                        var lineLength = _visibleLines[VisibileLineIndex].Chars.Count;
                        if(_lineRelativeCharacterIndex > lineLength - 1) return lineLength - 1;
                    }
                    return _lineRelativeCharacterIndex;
                }
                set { _lineRelativeCharacterIndex = value; }
            }

            public int GetAbsoluteCharacterIndex() {
                if (_visibleLines.Count == 0) return -1;
                return _visibleLines[VisibileLineIndex].Chars[0].AbsoluteCharacterIndex + LineRelativeCharacterIndex;
            }

            public void MoveLeft() {
                if (_lineRelativeCharacterIndex == -1) {
                    if(VisibileLineIndex == 0) {
                        if(_scrollUpOneLine()) _lineRelativeCharacterIndex = _visibleLines[VisibileLineIndex].Chars.Count - 1;
                    } else {
                        --VisibileLineIndex;
                        _lineRelativeCharacterIndex = _visibleLines[VisibileLineIndex].Chars.Count - 1;
                    }
                } else {
                    --_lineRelativeCharacterIndex;
                }
            }

            public void MoveRight() {
                if (_visibleLines.Count > 0 && _lineRelativeCharacterIndex == _visibleLines[VisibileLineIndex].Chars.Count - 1) {
                    if(VisibileLineIndex == _visibleLines.Count - 1) {
                        if(_scrollDownOneLine()) _lineRelativeCharacterIndex = -1;
                    } else {
                        _lineRelativeCharacterIndex = -1;
                        ++VisibileLineIndex;
                    }
                } else {
                    ++_lineRelativeCharacterIndex;
                }
            }

            public void MoveUp() {
                if(VisibileLineIndex == 0) {
                    _scrollUpOneLine();
                } else {
                    --VisibileLineIndex;
                }
            }

            public void MoveDown() {
                if(VisibileLineIndex == _visibleLines.Count - 1) {
                    _scrollDownOneLine();
                } else {
                    ++VisibileLineIndex;
                }
            }
        }

        DispatcherTimer _blinkTimer = new DispatcherTimer();
        KeyPressHandler _keyPressHandler;
        ITextFormatter _defaultFormatter;
        Stack<Line> _linesBeforeVisible = new Stack<Line>(); //lines which have been scrolled out of view
        OrderedList<Line> _visibleLines = new OrderedList<Line>();
        Caret _caret;
        double _defaultCaretHeight;
        bool _showCaret = false;

        public FormattedTextbox() {
            FocusVisualStyle = null;
            Background = Brushes.Transparent;
            Cursor = Cursors.IBeam;
            _caret = new Caret(0, 0, _visibleLines, ScrollUpOneLine, ScrollDownOneLine);
            _keyPressHandler = new KeyPressHandler(KeyPressed);
            Loaded += FormattedTextbox_Loaded;

            _blinkTimer.Interval = TimeSpan.FromMilliseconds(400);
            _blinkTimer.Tick += _blinkTimer_Tick;
            _blinkTimer.Start();

            //Text = Constants.LoremIpsum;
            //Text = new string('W', 5000000);
            //Text = string.Join("", Enumerable.Repeat("abcdefghijklmnopqrstuvwxyz", 50).ToArray());
            //Text = string.Join(" --- ", Enumerable.Repeat(Constants.LoremIpsum, 50).ToArray());
            //Text = string.Join(" --- ", Enumerable.Repeat(Constants.LoremIpsum, 4).ToArray());
            //Text = string.Join(" --- ", Enumerable.Repeat(Constants.LoremIpsum, 5000).ToArray());
            //Text = new string('A', 100) + "abcdefghi";
        }

        private bool ScrollDownOneLine() {
            //if there is more text after what is visible, scroll down a line
            if(_visibleLines.Count > 0 && _visibleLines.Last().Chars.Last().AbsoluteCharacterIndex < Text.Length - 1) {
                _linesBeforeVisible.Push(_visibleLines[0]);
                _visibleLines.RemoveRange(0, 1);
                return true;
            }
            return false;
        }

        private bool ScrollUpOneLine() {
            //if there is more text after what is visible, scroll down a line
            if(_linesBeforeVisible.Count > 0) {
                _visibleLines.Add(_linesBeforeVisible.Pop());
                return true;
            }
            return false;
        }

        private bool KeyPressed(IReadOnlyList<Key> arg) {
            if((arg.Count > 1 && !arg.Contains(Key.LeftAlt)) || arg.Count > 2) return false;

            if(arg.Count == 1) {
                var newText = Text ?? "";
                bool modified = false, caretsChanged = false;
                var absoluteCharacterIndex = _caret.GetAbsoluteCharacterIndex();
                if(arg[0] == Key.Back) {
                    if(_caret.LineRelativeCharacterIndex >= 0) {
                        newText = newText.Remove(absoluteCharacterIndex, 1);
                        --_caret.LineRelativeCharacterIndex;
                        modified = true;
                    }
                } else if(arg[0] == Key.Delete) {
                    if(_caret.LineRelativeCharacterIndex != newText.Length - 1) {
                        newText = newText.Remove(absoluteCharacterIndex + 1, 1);
                        modified = true;
                    }
                } else if(arg[0] == Key.Left || arg[0] == Key.Right || arg[0] == Key.Up || arg[0] == Key.Down) {
                    switch(arg[0]) {
                        case Key.Left:
                            _caret.MoveLeft();
                            break;
                        case Key.Right:
                            _caret.MoveRight();
                            break;
                        case Key.Up:
                            _caret.MoveUp();
                            break;
                        case Key.Down:
                            _caret.MoveDown();
                            break;
                    }
                    caretsChanged = true;
                } else {
                    KeyHelper.GetCharForKey(arg.First()).Apply(x => {
                        newText = newText.Insert(absoluteCharacterIndex + 1, "" + x);
                        ++_caret.LineRelativeCharacterIndex;
                        modified = true;
                    });
                }
                
                if(modified) Text = newText;
                if(caretsChanged) {
                    _showCaret = true;
                    InvalidateVisual();
                }

                if (modified || caretsChanged) return true;
            } else {
                //TODO:: Handle alt codes
            }

            return false;
        }

        private Tuple<int, int> GetLineAndCharIndex(Point pos) { //<line index, char index>
            int lineIndex = -1, charIndex = -1;
            if(_visibleLines.Count > 0) {
                lineIndex = _visibleLines.FindInsertionIndex(pos.Y) - 1;
                charIndex = _visibleLines[lineIndex].Chars.FindInsertionIndex(pos.X);
            }
            return Tuple.Create(lineIndex, charIndex);
        }

        private void PutCaret(Point pos) {
            _caret = new Caret(GetLineAndCharIndex(pos), _visibleLines, ScrollUpOneLine, ScrollDownOneLine);
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
            ctrl._defaultCaretHeight = ctrl.TextFormatter.Format("W").First().GetFormattedText().Height;
            ctrl.InvalidateVisual();
        }
        #endregion

        protected override void OnMouseDown(MouseButtonEventArgs e) {
            base.OnMouseDown(e);
            Focus();
            PutCaret(e.GetPosition(this));
            InvalidateVisual();
        }

        protected override void OnPreviewKeyUp(KeyEventArgs e) {
            base.OnPreviewKeyUp(e);
            if(_keyPressHandler.KeyUpCommand.CanExecute(e)) _keyPressHandler.KeyUpCommand.Execute(e);
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e) {
            base.OnPreviewKeyDown(e);
            if(_keyPressHandler.KeyDownCommand.CanExecute(e)) _keyPressHandler.KeyDownCommand.Execute(e);
            if(GetRelevantKey(e) != Key.LeftAlt && _keyPressHandler.KeyUpCommand.CanExecute(e)) _keyPressHandler.KeyUpCommand.Execute(e);
        }

        private Key GetRelevantKey(KeyEventArgs e) {
            return e.Key == Key.System ? e.SystemKey : e.Key;
        }

        //TODO:: Optimize by not calling the parser unnecessarily (or let the parser be the one to take care of that by caching results)
        protected override void OnRender(DrawingContext drawingContext) {
            base.OnRender(drawingContext);

            //Background (makes the control clickable outside of just the text)
            drawingContext.DrawRectangle(Background, new Pen(), new Rect(0, 0, ActualWidth, ActualHeight));
            if(_defaultFormatter == null) return; //not yet loaded

            if(!string.IsNullOrEmpty(Text)) {
                //render the text
                var endOfLine = new Point(0, 0);
                var nextLine = new Point(0, 0);
                int charIndex = _visibleLines.Count > 0 ? _visibleLines[0].Chars[0].AbsoluteCharacterIndex : 0;
                _visibleLines.Clear();

                //Maybe for a scroll bar if I took out the text size in the formatter, we could figure out how many characters fit in a screen, and how many there are total, and guess at
                //how many lines/whatever we need for the scrollbar
                foreach(var ele in (TextFormatter ?? _defaultFormatter).Format(Text.Substring(charIndex))) {
                    var pos = endOfLine;
                    var outOfRoom = false;
                    foreach(var line in WordWrap(charIndex, pos.X, ele)) {
                        if(pos.Y + line.Height > ActualHeight) {
                            outOfRoom = true;
                            break; //We've run out of room for lines, done rendering text
                        }
                        line.Y = pos.Y;
                        _visibleLines.Add(line);
                        
                        drawingContext.DrawText(line.Text, pos);
                        endOfLine.X = line.Width;

                        if(pos == nextLine) {
                            endOfLine.Y = nextLine.Y;
                            nextLine.Y += line.Height;
                        } else {
                            var maybeNextLineY = endOfLine.Y + line.Height; //handle change in font size within one line of text
                            nextLine.Y = nextLine.Y < maybeNextLineY ? maybeNextLineY : nextLine.Y;
                        }

                        pos = nextLine;
                    }
                    if(outOfRoom) break;
                    charIndex = ele.Text.Length - 1;
                }
            }

            //render the caret
            if(_showCaret) {
                double x = 0, y = 0, height = _defaultCaretHeight;
                if(_visibleLines.Count > 0) {
                    var line = _visibleLines[_caret.VisibileLineIndex];
                    y = line.Y;
                    height = line.Height;

                    if(_caret.LineRelativeCharacterIndex >= 0) {
                        x = line.Chars[_caret.LineRelativeCharacterIndex].EndX;
                    }
                }
                drawingContext.DrawLine(new Pen(_caret.Brush, 0.75), new Point(x, y), new Point(x, y + height));
            }
        }

        TextWithFormatting _lastTxt;
        Dictionary<char, Tuple<double, double>> _knownCharSizes; //Tuple<Width, Height>
        private Tuple<double, double> GetCharSize(char ch, TextWithFormatting txt) {
            if(_lastTxt == null || !_lastTxt.SizeFactorsAreEqual(txt)) {
                _lastTxt = txt;
                _knownCharSizes = new Dictionary<char, Tuple<double, double>>();
            }

            return _knownCharSizes.GetOrAdd(ch, () => {
                var fmtTxt = new TextWithFormatting(ch.ToString(), txt).GetFormattedText();
                return Tuple.Create(fmtTxt.WidthIncludingTrailingWhitespace, fmtTxt.LineHeight);
            });
        }

        private IEnumerable<Line> WordWrap(int charIndex, double offsetX, TextWithFormatting txt) {
            var startIndex = 0;
            while(startIndex < txt.Text.Length) {
                var line = new Line();
                var txtWidth = 0.0;
                var remainingWidth = ActualWidth - offsetX;

                int lastWhiteSpaceIndex = -1;
                int endIndex = startIndex;
                while(remainingWidth > 0 && endIndex < txt.Text.Length) {
                    var ch = txt.Text[endIndex];
                    var chDim = GetCharSize(ch, txt);
                    remainingWidth -= chDim.Item1;

                    if(remainingWidth >= 0) {
                        txtWidth += chDim.Item1;
                        line.Chars.Add(new Character() { AbsoluteCharacterIndex = charIndex, Ch = ch, EndX = txtWidth + offsetX, Width = chDim.Item1 });
                        if(ch == ' ' || ch == '\t') lastWhiteSpaceIndex = endIndex;
                        ++endIndex;
                        ++charIndex;
                    } else if(lastWhiteSpaceIndex > -1) {
                        endIndex = lastWhiteSpaceIndex + 1;
                        var cnt = line.Chars.Count - (endIndex - startIndex);
                        line.Chars.RemoveRange(endIndex - startIndex, cnt);
                        charIndex -= cnt;
                    }
                }

                line.Text = new TextWithFormatting(txt.Text.Substring(startIndex, endIndex - startIndex), txt).GetFormattedText();
                line.Width = txtWidth;
                line.Height = line.Text.Height;
                yield return line;

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
            _defaultCaretHeight = _defaultFormatter.Format("W").First().GetFormattedText().Height;
            if(TextFormatter == null) InvalidateVisual();
        }

        private void _blinkTimer_Tick(object sender, EventArgs e) {
            if(!HasEffectiveKeyboardFocus) return;
            _showCaret = !_showCaret;
            InvalidateVisual();
        }
    }
}