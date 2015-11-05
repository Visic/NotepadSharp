using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WPFUtility;

namespace NotepadSharp {
    public class KeyBindingsViewModel : ViewModelBase {
        public KeyBindingsViewModel() {
            KeyBindings = new ObservableCollection<KeyBindingViewModel>(
                ArgsAndSettings.KeyBindings.Select(x => new KeyBindingViewModel(x, EditBinding))
            );
        }

        public string Title { get; } = "Key Bindings";
        public ObservableCollection<KeyBindingViewModel> KeyBindings { get; }

        private void EditBinding(KeyBinding oldBinding, KeyBinding newBinding) {
            ArgsAndSettings.KeyBindings.ClearBinding(oldBinding);
            ArgsAndSettings.KeyBindings.SetBinding(newBinding);
        }
    }
}
