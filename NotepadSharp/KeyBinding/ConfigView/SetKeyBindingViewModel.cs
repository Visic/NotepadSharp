using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WPFUtility;

namespace NotepadSharp {
    public class SetKeyBindingViewModel : ViewModelBase {
        KeyBinding _currentBinding;

        public SetKeyBindingViewModel(KeyBinding binding) {
            _currentBinding = binding;
            Keys = new NotifyingProperty<HashSet<Key>>(_currentBinding.Keys);
            Label = new NotifyingProperty<string>(_currentBinding.Label);
            ScriptFilePath = new NotifyingProperty<string>((binding as LuaKeyBinding)?.ScriptPath?.Value);
            IsEditingBinding = new NotifyingProperty<bool>();
            StartEditingCommand = new RelayCommand(x => StartEditBinding((RoutedEventArgs)x));
            EndEditingCommand = new RelayCommand(x => EndEditBinding((RoutedEventArgs)x));
            KeyDownCommand = new RelayCommand(x => KeyDown((KeyEventArgs)x), x => IsEditingBinding.Value);
            KeyUpCommand = new RelayCommand(x => KeyUp((KeyEventArgs)x), x => IsEditingBinding.Value);
        }

        public NotifyingProperty<HashSet<Key>> Keys { get; private set; }
        public NotifyingProperty<string> Label { get; }
        public NotifyingProperty<string> ScriptFilePath { get; }
        public NotifyingProperty<bool> IsEditingBinding { get; }
        public ICommand KeyDownCommand { get; }
        public ICommand KeyUpCommand { get; }
        public ICommand StartEditingCommand { get; }
        public ICommand EndEditingCommand { get; }

        public KeyBinding GetBinding() {
            return string.IsNullOrEmpty(ScriptFilePath.Value) ? 
                new KeyBinding(_currentBinding.Action, Label.Value, Keys.Value.ToArray()) : 
                new LuaKeyBinding(ScriptFilePath.Value, Label.Value, Keys.Value.ToArray());
        }

        private void StartEditBinding(RoutedEventArgs obj) {
            IsEditingBinding.Value = true;
        }

        private void EndEditBinding(RoutedEventArgs obj) {
            IsEditingBinding.Value = false;
        }

        private void KeyUp(KeyEventArgs e) {
            ArgsAndSettings.KeyBindings.KeyReleased(GetRelevantKey(e));
        }

        private void KeyDown(KeyEventArgs e) {
            e.Handled = ArgsAndSettings.KeyBindings.KeyPressed(GetRelevantKey(e), false);
            Keys.Value = new HashSet<Key>(ArgsAndSettings.KeyBindings.PressedKeys);
        }

        private Key GetRelevantKey(KeyEventArgs e) {
            return e.Key == Key.System ? e.SystemKey : e.Key;
        }
    }
}
