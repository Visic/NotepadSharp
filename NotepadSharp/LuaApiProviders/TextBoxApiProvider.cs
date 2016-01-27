using System;

namespace NotepadSharp {
    public class TextBoxApiProvider {
        public Action Save { get; set; }
        public Func<string> GetSelectedText { get; set; }
        public Action Backspace { get; set; }
        public Action Delete { get; set; }
        public Action StartSelect { get; set; }
        public Action EndSelect { get; set; }
        public Func<int> GetCaretOffset { get; set; }
        public Action<int> SetCaretOffset { get; set; }
    }
}
