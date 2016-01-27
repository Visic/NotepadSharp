using ICSharpCode.AvalonEdit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using Utility;

namespace NotepadSharp {
    public class BindableAvalonEdit : TextEditor {
        bool _updatingText;
        ICSharpCode.AvalonEdit.Document.IDocument _currentDocument;
        Option<int> _startSelectLoc;

        public static readonly DependencyProperty Text_Property = DependencyProperty.Register(
            "Text_",
            typeof(string),
            typeof(BindableAvalonEdit),
            new FrameworkPropertyMetadata(_TextChanged)
        );

        public string Text_ {
            get { return (string)GetValue(Text_Property); }
            set { SetValue(Text_Property, value); }
        }

        private static void _TextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            var src = (BindableAvalonEdit)d;
            if (src._updatingText) return;
            src.Document.Text = src.Text_;
        }

        public static readonly DependencyProperty ApiProviderProperty = DependencyProperty.Register(
            "ApiProvider",
            typeof(TextBoxApiProvider),
            typeof(BindableAvalonEdit),
            new FrameworkPropertyMetadata(ApiProviderChanged)
        );

        public TextBoxApiProvider ApiProvider {
            get { return (TextBoxApiProvider)GetValue(ApiProviderProperty); }
            set { SetValue(ApiProviderProperty, value); }
        }

        private static void ApiProviderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            var source = (BindableAvalonEdit)d;
            source.ApiProvider.GetSelectedText = () => source.SelectedText;

            source.ApiProvider.Backspace = () => {
                if (source.CaretOffset == 0 && source.SelectionLength == 0) return;

                if (source.SelectionLength > 0) {
                    source.RemoveSelectedText();
                } else {
                    var offset = source.CaretOffset - 1;
                    source.Text = source.Text.Remove(offset, 1);
                    source.CaretOffset = offset;
                }
            };

            source.ApiProvider.Delete = () => {
                if (source.CaretOffset == 0 && source.SelectionLength == 0) return;

                if (source.SelectionLength > 0) {
                    source.RemoveSelectedText();
                } else {
                    var offset = source.CaretOffset;
                    source.Text = source.Text.Remove(offset, 1);
                    source.CaretOffset = offset;
                }
            };

            source.ApiProvider.StartSelect = () => source._startSelectLoc = source.CaretOffset;

            source.ApiProvider.EndSelect = () => {
                source._startSelectLoc.Apply(startLoc => {
                    var offsetToRestore = source.CaretOffset;

                    var endLoc = source.CaretOffset;
                    Methods.Order(ref startLoc, ref endLoc);
                    source.Select(startLoc, endLoc - startLoc); //this changes the CaretOffset
                    source._startSelectLoc = new Option<int>();

                    source.CaretOffset = offsetToRestore;
                });
            };

            source.ApiProvider.GetCaretOffset = () => source.CaretOffset;

            source.ApiProvider.SetCaretOffset = offset => {
                if (offset > source.Text.Length - 1) offset = source.Text.Length - 1;
                if (offset < 0) offset = 0;
                source.CaretOffset = offset;
            };
        }

        private void RemoveSelectedText() {
            if (SelectionLength == 0) return;

            var offset = CaretOffset;
            if (SelectionStart < offset) offset = SelectionStart;
            Text = Text.Remove(offset, SelectionLength);
            SelectionLength = 0;
            CaretOffset = offset;
        }

        protected override void OnDocumentChanged(EventArgs e) {
            base.OnDocumentChanged(e);
            if (_currentDocument != null) _currentDocument.TextChanged -= Document_TextChanged;
            _currentDocument = Document;
            Document.TextChanged += Document_TextChanged;
        }

        private void Document_TextChanged(object sender, EventArgs e) {
            _updatingText = true;
            Text_ = Document.Text;
            _updatingText = false;
        }
    }
}
