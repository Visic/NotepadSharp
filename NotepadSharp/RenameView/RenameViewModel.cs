using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WPFUtility;

namespace NotepadSharp {
    public class RenameViewModel : ViewModelBase {
        public RenameViewModel(Func<string, string> validate, string currentName, Action requestClose) {
            Name = new NotifyingProperty<string>(x => ValidationError.Value = validate(x), currentName);
            OkCommand = new RelayCommand(x => { DialogResult = true; requestClose(); });
            CancelCommand = new RelayCommand(x => { DialogResult = false; requestClose(); });
        }

        public bool? DialogResult { get; private set; }
        public NotifyingProperty<string> Name { get; }
        public NotifyingProperty<string> ValidationError { get; } = new NotifyingProperty<string>();
        public ICommand OkCommand { get; }
        public ICommand CancelCommand { get; }
    }
}