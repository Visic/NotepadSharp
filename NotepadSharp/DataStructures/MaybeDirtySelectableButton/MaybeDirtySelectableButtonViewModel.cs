using System.ComponentModel;
using System.Windows.Input;
using WPFUtility;

namespace NotepadSharp {
    public class MaybeDirtySelectableButtonViewModel : ISelectableButtonViewModel {
        SelectableButtonViewModel _buttonVm;
        public MaybeDirtySelectableButtonViewModel(string text, ICommand command) {
            _buttonVm = new SelectableButtonViewModel(text, command);
        }

        public NotifyingProperty<bool> IsDirty { get; set; }

        public ICommand Command { get { return _buttonVm.Command; } }
        public bool IsCancel { get { return _buttonVm.IsCancel; } }
        public bool IsDefault { get { return _buttonVm.IsDefault; } }
        public NotifyingProperty<bool> IsSelected { get { return _buttonVm.IsSelected; } }
        public NotifyingProperty<string> Text { get { return _buttonVm.Text; } }
    }
}