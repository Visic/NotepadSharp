using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace NotepadSharp {
    public class BindableRichTextBox : RichTextBox {
        public static readonly DependencyProperty ApiProperty = DependencyProperty.Register(
            "Api",
            typeof(RichTextBox_LuaApiProvider),
            typeof(BindableRichTextBox),
            new FrameworkPropertyMetadata(ApiChanged)
        );

        public RichTextBox_LuaApiProvider Api {
            get { return (RichTextBox_LuaApiProvider)GetValue(ApiProperty); }
            set { SetValue(ApiProperty, value); }
        }

        private static void ApiChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            var source = (BindableRichTextBox)d;
            source.Api.DeleteNextWord =		    () => EditingCommands.DeleteNextWord.Execute(null, source);
            source.Api.AlignCenter =		    () => EditingCommands.AlignCenter.Execute(null, source);
            source.Api.AlignJustify =		    () => EditingCommands.AlignJustify.Execute(null, source);
            source.Api.AlignLeft =		        () => EditingCommands.AlignLeft.Execute(null, source);
            source.Api.AlignRight =		        () => EditingCommands.AlignRight.Execute(null, source);
            source.Api.Backspace =		        () => EditingCommands.Backspace.Execute(null, source);
            source.Api.CorrectSpellingError =	() => EditingCommands.CorrectSpellingError.Execute(null, source);
            source.Api.DecreaseFontSize =		() => EditingCommands.DecreaseFontSize.Execute(null, source);
            source.Api.DecreaseIndentation =	() => EditingCommands.DecreaseIndentation.Execute(null, source);
            source.Api.Delete =		            () => EditingCommands.Delete.Execute(null, source);
            source.Api.DeleteNextWord =		    () => EditingCommands.DeleteNextWord.Execute(null, source);
            source.Api.DeletePreviousWord =		() => EditingCommands.DeletePreviousWord.Execute(null, source);
            source.Api.EnterLineBreak =		    () => EditingCommands.EnterLineBreak.Execute(null, source);
            source.Api.EnterParagraphBreak =	() => EditingCommands.EnterParagraphBreak.Execute(null, source);
            source.Api.IgnoreSpellingError =	() => EditingCommands.IgnoreSpellingError.Execute(null, source);
            source.Api.IncreaseFontSize =		() => EditingCommands.IncreaseFontSize.Execute(null, source);
            source.Api.IncreaseIndentation =	() => EditingCommands.IncreaseIndentation.Execute(null, source);
            source.Api.MoveDownByLine =		    () => EditingCommands.MoveDownByLine.Execute(null, source);
            source.Api.MoveDownByPage =		    () => EditingCommands.MoveDownByPage.Execute(null, source);
            source.Api.MoveDownByParagraph =	() => EditingCommands.MoveDownByParagraph.Execute(null, source);
            source.Api.MoveLeftByCharacter =	() => EditingCommands.MoveLeftByCharacter.Execute(null, source);
            source.Api.MoveLeftByWord =		    () => EditingCommands.MoveLeftByWord.Execute(null, source);
            source.Api.MoveRightByCharacter =	() => EditingCommands.MoveRightByCharacter.Execute(null, source);
            source.Api.MoveRightByWord =		() => EditingCommands.MoveRightByWord.Execute(null, source);
            source.Api.MoveToDocumentEnd =		() => EditingCommands.MoveToDocumentEnd.Execute(null, source);
            source.Api.MoveToDocumentStart =	() => EditingCommands.MoveToDocumentStart.Execute(null, source);
            source.Api.MoveToLineEnd =		    () => EditingCommands.MoveToLineEnd.Execute(null, source);
            source.Api.MoveToLineStart =		() => EditingCommands.MoveToLineStart.Execute(null, source);
            source.Api.MoveUpByLine =		    () => EditingCommands.MoveUpByLine.Execute(null, source);
            source.Api.MoveUpByPage =		    () => EditingCommands.MoveUpByPage.Execute(null, source);
            source.Api.MoveUpByParagraph =		() => EditingCommands.MoveUpByParagraph.Execute(null, source);
            source.Api.SelectDownByLine =		() => EditingCommands.SelectDownByLine.Execute(null, source);
            source.Api.SelectDownByPage =		() => EditingCommands.SelectDownByPage.Execute(null, source);
            source.Api.SelectDownByParagraph =	() => EditingCommands.SelectDownByParagraph.Execute(null, source);
            source.Api.SelectLeftByCharacter =	() => EditingCommands.SelectLeftByCharacter.Execute(null, source);
            source.Api.SelectLeftByWord =		() => EditingCommands.SelectLeftByWord.Execute(null, source);
            source.Api.SelectRightByCharacter =	() => EditingCommands.SelectRightByCharacter.Execute(null, source);
            source.Api.SelectRightByWord =		() => EditingCommands.SelectRightByWord.Execute(null, source);
            source.Api.SelectToDocumentEnd =	() => EditingCommands.SelectToDocumentEnd.Execute(null, source);
            source.Api.SelectToDocumentStart =	() => EditingCommands.SelectToDocumentStart.Execute(null, source);
            source.Api.SelectToLineEnd =		() => EditingCommands.SelectToLineEnd.Execute(null, source);
            source.Api.SelectToLineStart =		() => EditingCommands.SelectToLineStart.Execute(null, source);
            source.Api.SelectUpByLine =		    () => EditingCommands.SelectUpByLine.Execute(null, source);
            source.Api.SelectUpByPage =		    () => EditingCommands.SelectUpByPage.Execute(null, source);
            source.Api.SelectUpByParagraph =	() => EditingCommands.SelectUpByParagraph.Execute(null, source);
            source.Api.TabBackward =		    () => EditingCommands.TabBackward.Execute(null, source);
            source.Api.TabForward =		        () => EditingCommands.TabForward.Execute(null, source);
            source.Api.ToggleBold =		        () => EditingCommands.ToggleBold.Execute(null, source);
            source.Api.ToggleBullets =		    () => EditingCommands.ToggleBullets.Execute(null, source);
            source.Api.ToggleInsert =		    () => EditingCommands.ToggleInsert.Execute(null, source);
            source.Api.ToggleItalic =		    () => EditingCommands.ToggleItalic.Execute(null, source);
            source.Api.ToggleNumbering =		() => EditingCommands.ToggleNumbering.Execute(null, source);
            source.Api.ToggleSubscript =		() => EditingCommands.ToggleSubscript.Execute(null, source);
            source.Api.ToggleSuperscript =		() => EditingCommands.ToggleSuperscript.Execute(null, source);
            source.Api.ToggleUnderline =		() => EditingCommands.ToggleUnderline.Execute(null, source);
        }
    }
}
