using System;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Input;
using WPFUtility;
using System.IO;
using System.Windows;

namespace NotepadSharp {
    public class FileTabButtonViewModel : ViewModelBase, ISelectableButtonViewModel {
        SelectableCloseableButtonWithContextMenuViewModel _buttonVm;

        public FileTabButtonViewModel(ICommand command, ICommand closeCommand, DocumentViewModel doc) {
            var contextMenuOptions = new List<CommandWithText>();
            contextMenuOptions.Add(new CommandWithText(
                new RelayCommand(x => RenameTab()),
                "Rename Tab"
            ));

            _buttonVm = new SelectableCloseableButtonWithContextMenuViewModel(doc.FileName.Value, command, closeCommand, contextMenuOptions.ToArray());
            DocumentVM = doc;
        }

        public ICommand Command { get { return _buttonVm.Command; } }
        public ICommand CloseCommand { get { return _buttonVm.CloseCommand; } }
        public bool IsCancel { get { return _buttonVm.IsCancel; } }
        public bool IsDefault { get { return _buttonVm.IsDefault; } }
        public NotifyingProperty<bool> IsSelected { get { return _buttonVm.IsSelected; } }
        public NotifyingProperty<string> Text { get { return DocumentVM.FileName; } }
        public IReadOnlyList<CommandWithText> ContextMenuOptions { get { return _buttonVm.ContextMenuOptions; } }
        public DocumentViewModel DocumentVM { get; }

        private void RenameTab() {
            Func<string, string> validateFileName = newName => {
                if (string.IsNullOrEmpty(newName)) return "Name cannot be left blank.";
                if (newName.Length > 30) return "Name cannot be longer than 30 characters.";

                var notAllowedChars = Path.GetInvalidFileNameChars();
                var currentInvalidChars = newName.Where(x => notAllowedChars.Contains(x)).Distinct().ToArray();
                if (currentInvalidChars.Length > 0) return $"Name cannot contain {string.Join(" ", currentInvalidChars)}";

                return "";
            };

            RenameViewModel renameVm = null;
            WindowGenerator.CreateAndShowModal(x => renameVm = new RenameViewModel(validateFileName, DocumentVM.FileName.Value, x), false, 100, 350, "#EEEEEE");
            if(string.IsNullOrEmpty(renameVm.ValidationError.Value) && (renameVm.DialogResult ?? false)) {
                DocumentVM.FileName.Value = renameVm.Name.Value;
            }
        }
    }
}