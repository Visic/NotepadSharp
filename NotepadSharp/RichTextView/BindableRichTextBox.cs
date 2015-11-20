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
        public static readonly DependencyProperty ApiProviderProperty = DependencyProperty.Register(
            "ApiProvider",
            typeof(RichTextBoxApiProvider),
            typeof(BindableRichTextBox),
            new FrameworkPropertyMetadata(ApiProviderChanged)
        );

        public RichTextBoxApiProvider ApiProvider {
            get { return (RichTextBoxApiProvider)GetValue(ApiProviderProperty); }
            set { SetValue(ApiProviderProperty, value); }
        }

        private static void ApiProviderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            var source = (BindableRichTextBox)d;
            source.ApiProvider.DeleteNextWord =		    () => EditingCommands.DeleteNextWord.Execute(null, source);
            source.ApiProvider.AlignCenter =		    () => EditingCommands.AlignCenter.Execute(null, source);
            source.ApiProvider.AlignJustify =		    () => EditingCommands.AlignJustify.Execute(null, source);
            source.ApiProvider.AlignLeft =		        () => EditingCommands.AlignLeft.Execute(null, source);
            source.ApiProvider.AlignRight =		        () => EditingCommands.AlignRight.Execute(null, source);
            source.ApiProvider.Backspace =		        () => EditingCommands.Backspace.Execute(null, source);
            source.ApiProvider.CorrectSpellingError =	() => EditingCommands.CorrectSpellingError.Execute(null, source);
            source.ApiProvider.DecreaseFontSize =		() => EditingCommands.DecreaseFontSize.Execute(null, source);
            source.ApiProvider.DecreaseIndentation =	() => EditingCommands.DecreaseIndentation.Execute(null, source);
            source.ApiProvider.Delete =		            () => EditingCommands.Delete.Execute(null, source);
            source.ApiProvider.DeleteNextWord =		    () => EditingCommands.DeleteNextWord.Execute(null, source);
            source.ApiProvider.DeletePreviousWord =		() => EditingCommands.DeletePreviousWord.Execute(null, source);
            source.ApiProvider.EnterLineBreak =		    () => EditingCommands.EnterLineBreak.Execute(null, source);
            source.ApiProvider.EnterParagraphBreak =	() => EditingCommands.EnterParagraphBreak.Execute(null, source);
            source.ApiProvider.IgnoreSpellingError =	() => EditingCommands.IgnoreSpellingError.Execute(null, source);
            source.ApiProvider.IncreaseFontSize =		() => EditingCommands.IncreaseFontSize.Execute(null, source);
            source.ApiProvider.IncreaseIndentation =	() => EditingCommands.IncreaseIndentation.Execute(null, source);
            source.ApiProvider.MoveDownByLine =		    () => EditingCommands.MoveDownByLine.Execute(null, source);
            source.ApiProvider.MoveDownByPage =		    () => EditingCommands.MoveDownByPage.Execute(null, source);
            source.ApiProvider.MoveDownByParagraph =	() => EditingCommands.MoveDownByParagraph.Execute(null, source);
            source.ApiProvider.MoveLeftByCharacter =	() => EditingCommands.MoveLeftByCharacter.Execute(null, source);
            source.ApiProvider.MoveLeftByWord =		    () => EditingCommands.MoveLeftByWord.Execute(null, source);
            source.ApiProvider.MoveRightByCharacter =	() => EditingCommands.MoveRightByCharacter.Execute(null, source);
            source.ApiProvider.MoveRightByWord =		() => EditingCommands.MoveRightByWord.Execute(null, source);
            source.ApiProvider.MoveToDocumentEnd =		() => EditingCommands.MoveToDocumentEnd.Execute(null, source);
            source.ApiProvider.MoveToDocumentStart =	() => EditingCommands.MoveToDocumentStart.Execute(null, source);
            source.ApiProvider.MoveToLineEnd =		    () => EditingCommands.MoveToLineEnd.Execute(null, source);
            source.ApiProvider.MoveToLineStart =		() => EditingCommands.MoveToLineStart.Execute(null, source);
            source.ApiProvider.MoveUpByLine =		    () => EditingCommands.MoveUpByLine.Execute(null, source);
            source.ApiProvider.MoveUpByPage =		    () => EditingCommands.MoveUpByPage.Execute(null, source);
            source.ApiProvider.MoveUpByParagraph =		() => EditingCommands.MoveUpByParagraph.Execute(null, source);
            source.ApiProvider.SelectDownByLine =		() => EditingCommands.SelectDownByLine.Execute(null, source);
            source.ApiProvider.SelectDownByPage =		() => EditingCommands.SelectDownByPage.Execute(null, source);
            source.ApiProvider.SelectDownByParagraph =	() => EditingCommands.SelectDownByParagraph.Execute(null, source);
            source.ApiProvider.SelectLeftByCharacter =	() => EditingCommands.SelectLeftByCharacter.Execute(null, source);
            source.ApiProvider.SelectLeftByWord =		() => EditingCommands.SelectLeftByWord.Execute(null, source);
            source.ApiProvider.SelectRightByCharacter =	() => EditingCommands.SelectRightByCharacter.Execute(null, source);
            source.ApiProvider.SelectRightByWord =		() => EditingCommands.SelectRightByWord.Execute(null, source);
            source.ApiProvider.SelectToDocumentEnd =	() => EditingCommands.SelectToDocumentEnd.Execute(null, source);
            source.ApiProvider.SelectToDocumentStart =	() => EditingCommands.SelectToDocumentStart.Execute(null, source);
            source.ApiProvider.SelectToLineEnd =		() => EditingCommands.SelectToLineEnd.Execute(null, source);
            source.ApiProvider.SelectToLineStart =		() => EditingCommands.SelectToLineStart.Execute(null, source);
            source.ApiProvider.SelectUpByLine =		    () => EditingCommands.SelectUpByLine.Execute(null, source);
            source.ApiProvider.SelectUpByPage =		    () => EditingCommands.SelectUpByPage.Execute(null, source);
            source.ApiProvider.SelectUpByParagraph =	() => EditingCommands.SelectUpByParagraph.Execute(null, source);
            source.ApiProvider.TabBackward =		    () => EditingCommands.TabBackward.Execute(null, source);
            source.ApiProvider.TabForward =		        () => EditingCommands.TabForward.Execute(null, source);
            source.ApiProvider.ToggleBold =		        () => EditingCommands.ToggleBold.Execute(null, source);
            source.ApiProvider.ToggleBullets =		    () => EditingCommands.ToggleBullets.Execute(null, source);
            source.ApiProvider.ToggleInsert =		    () => EditingCommands.ToggleInsert.Execute(null, source);
            source.ApiProvider.ToggleItalic =		    () => EditingCommands.ToggleItalic.Execute(null, source);
            source.ApiProvider.ToggleNumbering =		() => EditingCommands.ToggleNumbering.Execute(null, source);
            source.ApiProvider.ToggleSubscript =		() => EditingCommands.ToggleSubscript.Execute(null, source);
            source.ApiProvider.ToggleSuperscript =		() => EditingCommands.ToggleSuperscript.Execute(null, source);
            source.ApiProvider.ToggleUnderline =		() => EditingCommands.ToggleUnderline.Execute(null, source);
        }
    }
}
