using System;
using System.ComponentModel;
using System.Windows.Input;
using WPFUtility;

namespace NotepadSharp {
    public class MaybeDirtySelectableClosableButtonViewModel : ISelectableButtonViewModel {
        SelectableCloseableButtonViewModel _buttonVm;
        public MaybeDirtySelectableClosableButtonViewModel(string text, ICommand command, ICommand closeCommand) {
            _buttonVm = new SelectableCloseableButtonViewModel(text, command, closeCommand);
        }

        public NotifyingProperty<bool> IsDirty { get; set; }

        public ICommand Command { get { return _buttonVm.Command; } }
        public ICommand CloseCommand { get { return _buttonVm.CloseCommand; } }
        public bool IsCancel { get { return _buttonVm.IsCancel; } }
        public bool IsDefault { get { return _buttonVm.IsDefault; } }
        public NotifyingProperty<bool> IsSelected { get { return _buttonVm.IsSelected; } }
        public NotifyingProperty<string> Text { get { return _buttonVm.Text; } }

        public void Dispose() {
        }
    }
}