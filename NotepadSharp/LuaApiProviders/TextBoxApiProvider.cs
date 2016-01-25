using System;

namespace NotepadSharp {
    public class TextBoxApiProvider {
        public Action Save { get; set; }
        public Action AlignCenter { get; set; }
        public Action AlignJustify { get; set; }
        public Action AlignLeft { get; set; }
        public Action AlignRight { get; set; }
        public Action Backspace { get; set; }
        public Action CorrectSpellingError { get; set; }
        public Action DecreaseFontSize { get; set; }
        public Action DecreaseIndentation { get; set; }
        public Action Delete { get; set; }
        public Action DeleteNextWord { get; set; }
        public Action DeletePreviousWord { get; set; }
        public Action EnterLineBreak { get; set; }
        public Action EnterParagraphBreak { get; set; }
        public Action IgnoreSpellingError { get; set; }
        public Action IncreaseFontSize { get; set; }
        public Action IncreaseIndentation { get; set; }
        public Action MoveDownByLine { get; set; }
        public Action MoveDownByPage { get; set; }
        public Action MoveDownByParagraph { get; set; }
        public Action MoveLeftByCharacter { get; set; }
        public Action MoveLeftByWord { get; set; }
        public Action MoveRightByCharacter { get; set; }
        public Action MoveRightByWord { get; set; }
        public Action MoveToDocumentEnd { get; set; }
        public Action MoveToDocumentStart { get; set; }
        public Action MoveToLineEnd { get; set; }
        public Action MoveToLineStart { get; set; }
        public Action MoveUpByLine { get; set; }
        public Action MoveUpByPage { get; set; }
        public Action MoveUpByParagraph { get; set; }
        public Action SelectDownByLine { get; set; }
        public Action SelectDownByPage { get; set; }
        public Action SelectDownByParagraph { get; set; }
        public Action SelectLeftByCharacter { get; set; }
        public Action SelectLeftByWord { get; set; }
        public Action SelectRightByCharacter { get; set; }
        public Action SelectRightByWord { get; set; }
        public Action SelectToDocumentEnd { get; set; }
        public Action SelectToDocumentStart { get; set; }
        public Action SelectToLineEnd { get; set; }
        public Action SelectToLineStart { get; set; }
        public Action SelectUpByLine { get; set; }
        public Action SelectUpByPage { get; set; }
        public Action SelectUpByParagraph { get; set; }
        public Action TabBackward { get; set; }
        public Action TabForward { get; set; }
        public Action ToggleBold { get; set; }
        public Action ToggleBullets { get; set; }
        public Action ToggleInsert { get; set; }
        public Action ToggleItalic { get; set; }
        public Action ToggleNumbering { get; set; }
        public Action ToggleSubscript { get; set; }
        public Action ToggleSuperscript { get; set; }
        public Action ToggleUnderline { get; set; }
    }
}
