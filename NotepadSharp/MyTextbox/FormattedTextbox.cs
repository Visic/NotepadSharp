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
            public int CharacterIndex { get; set; }

            public int CompareTo(object other) {
                var pos = (other as Character)?.EndX ?? (double)other;
                return EndX.CompareTo(pos);
            }
        }

        public class Line : IComparable {
            public double Y { get; set; }
            public double Height { get; set; }
            public double Width { get; set; }
            public FormattedText Text { get; set; }
            public OrderedList<Character> Chars { get; set; } = new OrderedList<Character>();

            public int CompareTo(object other) {
                var pos = (other as Line)?.Y ?? (double)other;
                return Y.CompareTo(pos);
            }
        }

        public class Caret : IComparable {
            public double X { get; set; }
            public double Y { get; set; }
            public int CharacterIndex { get; set; }
            public double Height { get; set; }
            public Brush Brush { get; set; } = Brushes.Green;

            public int CompareTo(object obj) {
                var index = (obj as Caret)?.CharacterIndex ?? (int)obj;
                return CharacterIndex.CompareTo(index);
            }
        }

        DispatcherTimer _blinkTimer = new DispatcherTimer();
        OrderedList<Caret> _carets = new OrderedList<Caret>();
        KeyPressHandler _keyPressHandler;
        ITextFormatter _defaultFormatter;
        OrderedList<Line> _visibleLines = new OrderedList<Line>();
        bool _showCarets = false;

        public FormattedTextbox() {
            FocusVisualStyle = null;
            Background = Brushes.Transparent;
            Cursor = Cursors.IBeam;
            _keyPressHandler = new KeyPressHandler(KeyPressed);
            Loaded += FormattedTextbox_Loaded;

            _blinkTimer.Interval = TimeSpan.FromMilliseconds(400);
            _blinkTimer.Tick += _blinkTimer_Tick;
            _blinkTimer.Start();

            //Text = Constants.LoremIpsum;
            //Text = new string('W', 5000000);
            //Text = string.Join("", Enumerable.Repeat("abcdefghijklmnopqrstuvwxyz", 50).ToArray());
            //Text = string.Join(" --- ", Enumerable.Repeat(Constants.LoremIpsum, 50).ToArray());
            //Text = string.Join(" --- ", Enumerable.Repeat(Constants.LoremIpsum, 5000).ToArray());
            //Text = new string('A', 100) + "abcdefghi";
        }

        private bool KeyPressed(IReadOnlyList<Key> arg) {
            if((arg.Count > 1 && !arg.Contains(Key.LeftAlt)) || arg.Count > 2) return false;

            if(arg.Count == 1) {
                var newText = Text ?? "";
                bool modified = false, caretsChanged = false;
                for(int i = 0; i < _carets.Count; ++i) {
                    if(arg[0] == Key.Back) {
                        if(string.IsNullOrEmpty(newText)) break;
                        if(_carets[i].CharacterIndex < 0) continue;

                        newText = newText.Remove(_carets[i].CharacterIndex, 1);
                        for(int z = i; z < _carets.Count; ++z) {
                            _carets[z].CharacterIndex -= 1;
                        }
                        modified = true;
                    } else if(arg[0] == Key.Delete) {
                        if(string.IsNullOrEmpty(newText)) break;
                        if(_carets[i].CharacterIndex == newText.Length - 1) continue;

                        newText = newText.Remove(_carets[i].CharacterIndex + 1, 1);
                        for(int z = i + 1; z < _carets.Count; ++z) {
                            _carets[z].CharacterIndex -= 1;
                        }
                        modified = true;
                    } else if(arg[0] == Key.Left || arg[0] == Key.Right) {
                        if(string.IsNullOrEmpty(newText)) break;
                        _carets[i].CharacterIndex += arg[0] == Key.Left ? -1 : 1;
                        if (_carets[i].CharacterIndex < -1) _carets[i].CharacterIndex = -1;
                        caretsChanged = true;
                    } else {
                        var maybeCh = KeyHelper.GetCharForKey(arg.First());
                        if(maybeCh.IsNone) break; //a character we don't handle
                        newText = newText.Insert(_carets[i].CharacterIndex + 1, "" + maybeCh.Value);
                        for(int z = i; z < _carets.Count; ++z) {
                            _carets[z].CharacterIndex += 1;
                        }
                        modified = true;
                    }

                    if(i + 1 < _carets.Count) {
                        //If this caret is in the same position as the one after it, remove the next caret
                        if(_carets[i].CharacterIndex == _carets[i + 1].CharacterIndex) {
                            _carets.Remove(_carets[i + 1]);
                        }
                    }
                }
                
                if(modified) Text = newText;
                if(caretsChanged) {
                    _showCarets = true;
                    InvalidateVisual();
                }

                if (modified || caretsChanged) return true;
            } else {
                //TODO:: Handle alt codes
            }

            return false;
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
            if(!Keyboard.IsKeyDown(Key.LeftCtrl)) _carets.Clear();
            var index = -1;

            if(_visibleLines.Count > 0) {
                var pos = e.GetPosition(this);
                var line = _visibleLines[_visibleLines.FindInsertionIndex(pos.Y) - 1];

                var chIndex = line.Chars.FindInsertionIndex(pos.X);
                var ch = line.Chars[chIndex == line.Chars.Count ? chIndex - 1 : chIndex];
                index = ch.CharacterIndex;
            }

            var alreadyThere = _carets.FirstOrDefault(x => x.CharacterIndex == index);
            if(alreadyThere != null) { //clicking on a caret that is already there, removes it
                _carets.Remove(alreadyThere);
            } else {
                _carets.Add(new Caret() {
                    CharacterIndex = index,
                });
            }
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
                int charIndex = 0;
                _visibleLines.Clear();
                foreach(var ele in (TextFormatter ?? _defaultFormatter).Format(Text)) {
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

            //render the carets
            if(_showCarets) {
                var lineIndex = 0;
                foreach(var caret in _carets) {
                    if(caret.CharacterIndex < 0) {
                        caret.X = 0;
                        if(caret.Height == 0) caret.Height = FontSize * 2; //TODO:: Caret size?
                    } else {
                        for(; lineIndex < _visibleLines.Count; ++lineIndex) {
                            var line = _visibleLines[lineIndex];
                            var ch = line.Chars.FirstOrDefault(x => x.CharacterIndex == caret.CharacterIndex);
                            if(ch != null) {
                                caret.X = ch.EndX;
                                caret.Y = line.Y;
                                caret.Height = line.Height;
                                break;
                            }
                        }
                    }

                    drawingContext.DrawLine(new Pen(caret.Brush, 0.75), new Point(caret.X, caret.Y), new Point(caret.X, caret.Y + caret.Height));
                }
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
                        line.Chars.Add(new Character() { CharacterIndex = charIndex, Ch = ch, EndX = txtWidth + offsetX, Width = chDim.Item1 });
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
            if(TextFormatter == null) InvalidateVisual();
        }

        private void _blinkTimer_Tick(object sender, EventArgs e) {
            if(!HasEffectiveKeyboardFocus) return;
            if(_carets.Count() > 0) {
                _showCarets = !_showCarets;
                InvalidateVisual();
            } else {
                _showCarets = false;
            }
        }
    }
}