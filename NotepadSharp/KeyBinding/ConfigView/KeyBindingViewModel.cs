using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WPFUtility;

namespace NotepadSharp {
    public class KeyBindingViewModel : ViewModelBase {
        KeyBinding _currentBinding;
        Action<KeyBinding, KeyBinding> _bindingChangedCallback;

        public KeyBindingViewModel(KeyBinding binding, Action<KeyBinding, KeyBinding> bindingChangedCallback) {
            _currentBinding = binding;
            _bindingChangedCallback = bindingChangedCallback;
            Keys = new NotifyingProperty<HashSet<Key>>(_currentBinding.Keys);
            Label = new NotifyingPropertyWithChangedAction<string>(x => CommitChanges(), _currentBinding.Label);
            ScriptFilePath = new NotifyingPropertyWithChangedAction<string>(x => CommitChanges(), (binding as LuaKeyBinding)?.ScriptPath?.Value);
            IsEditingBinding = new NotifyingProperty<bool>();
            StartEditingCommand = new RelayCommand(x => StartEditBinding((RoutedEventArgs)x));
            EndEditingCommand = new RelayCommand(x => EndEditBinding((RoutedEventArgs)x));
            KeyDownCommand = new RelayCommand(x => KeyDown((KeyEventArgs)x), x => IsEditingBinding.Value);
            KeyUpCommand = new RelayCommand(x => KeyUp((KeyEventArgs)x), x => IsEditingBinding.Value);
        }

        public NotifyingProperty<HashSet<Key>> Keys { get; private set; }
        public NotifyingPropertyWithChangedAction<string> Label { get; }
        public NotifyingPropertyWithChangedAction<string> ScriptFilePath { get; }
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

        private void CommitChanges() {
            var newBinding = GetBinding();
            _bindingChangedCallback(_currentBinding, newBinding);
            _currentBinding = newBinding;
        }

        private void StartEditBinding(RoutedEventArgs obj) {
            IsEditingBinding.Value = true;
        }

        private void EndEditBinding(RoutedEventArgs obj) {
            IsEditingBinding.Value = false;
            CommitChanges();
        }

        private void KeyUp(KeyEventArgs e) {
            ArgsAndSettings.KeyBindings.KeyReleased(GetRelevantKey(e));
        }

        private void KeyDown(KeyEventArgs e) {
            var key = GetRelevantKey(e);
            e.Handled = ArgsAndSettings.KeyBindings.KeyPressed(key, false);
            if(!e.Handled && key == Key.Escape) {
                Keys.Value = _currentBinding.Keys;
            } else {
                Keys.Value = new HashSet<Key>(ArgsAndSettings.KeyBindings.PressedKeys);
            }
        }

        private Key GetRelevantKey(KeyEventArgs e) {
            return e.Key == Key.System ? e.SystemKey : e.Key;
        }
    }
}
