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
        KeyPressHandler _keyPressHandler;

        public KeyBindingViewModel(KeyBinding binding, Action<KeyBinding, KeyBinding> bindingChangedCallback, Action<KeyBindingViewModel> deleteBindingCallback) {
            _currentBinding = binding;
            _bindingChangedCallback = bindingChangedCallback;
            _keyPressHandler = new KeyPressHandler(KeyPressed);
            Keys = new NotifyingProperty<HashSet<Key>>(_currentBinding.Keys);
            ScriptFilePath = new NotifyingPropertyWithChangedAction<string>(x => CommitChanges(), (binding as LuaKeyBinding)?.ScriptPath);
            ExecuteOnKeyUp = new NotifyingPropertyWithChangedAction<bool>(x => { _currentBinding.ExecuteOnKeyUp = x; CommitChanges(); }, _currentBinding.ExecuteOnKeyUp);
            ExecuteOnKeyDown = new NotifyingPropertyWithChangedAction<bool>(
                x => {
                    _currentBinding.ExecuteOnKeyDown = x;
                    if(!x) RepeatOnKeyDown.Value = x;
                    CommitChanges();
                }, 
                _currentBinding.ExecuteOnKeyDown
            );
            RepeatOnKeyDown = new NotifyingPropertyWithChangedAction<bool>(
                x => {
                    _currentBinding.RepeatOnKeyDown = x;
                    if (x) ExecuteOnKeyDown.Value = x;
                    CommitChanges();
                }, 
                _currentBinding.RepeatOnKeyDown
            );
            IsEditingBinding = new NotifyingProperty<bool>();
            StartEditingCommand = new RelayCommand(x => StartEditBinding((RoutedEventArgs)x));
            EndEditingCommand = new RelayCommand(x => EndEditBinding((RoutedEventArgs)x));
            KeyDownCommand = new RelayCommand(x => _keyPressHandler.KeyDown((KeyEventArgs)x), x => IsEditingBinding.Value);
            KeyUpCommand = new RelayCommand(x => _keyPressHandler.KeyUp((KeyEventArgs)x), x => IsEditingBinding.Value);
            if (deleteBindingCallback != null) DeleteBindingCommand = new RelayCommand(x => deleteBindingCallback(this));

            ScriptFilePathGotFocusCommand = new RelayCommand(x => ScriptFilePathIsFocused.Value = true);
            ScriptFilePathLostFocusCommand = new RelayCommand(x => ScriptFilePathIsFocused.Value = false);
            DropCommand = new RelayCommand(x => Drop((DragEventArgs)x));
            DragOverCommand = new RelayCommand(x => DragOver((DragEventArgs)x));
            LostKeyboardFocusCommand = new RelayCommand(x => _keyPressHandler.ClearPressedKeys());
        }

        public NotifyingProperty<bool> ScriptFilePathIsFocused { get; } = new NotifyingProperty<bool>();
        public NotifyingProperty<HashSet<Key>> Keys { get; private set; }
        public NotifyingPropertyWithChangedAction<string> ScriptFilePath { get; }
        public NotifyingPropertyWithChangedAction<bool> ExecuteOnKeyDown { get; }
        public NotifyingPropertyWithChangedAction<bool> ExecuteOnKeyUp { get; }
        public NotifyingPropertyWithChangedAction<bool> RepeatOnKeyDown { get; }
        public NotifyingProperty<bool> IsEditingBinding { get; }
        public ICommand KeyDownCommand { get; }
        public ICommand KeyUpCommand { get; }
        public ICommand StartEditingCommand { get; }
        public ICommand EndEditingCommand { get; }
        public ICommand DeleteBindingCommand { get; }
        public ICommand ScriptFilePathGotFocusCommand { get; }
        public ICommand ScriptFilePathLostFocusCommand { get; }
        public ICommand DropCommand { get; }
        public ICommand DragOverCommand { get; }
        public ICommand LostKeyboardFocusCommand { get; }

        public KeyBinding GetBinding() {
            return new LuaKeyBinding(_currentBinding, ScriptFilePath.Value, Keys.Value.ToArray());
        }

        public void ClearBinding() {
            StartEditBinding(null);
            EndEditBinding(null);
        }

        private void CommitChanges() {
            var newBinding = GetBinding();
            _bindingChangedCallback(_currentBinding, newBinding);
            _currentBinding = newBinding;
        }
        
        private void DragOver(DragEventArgs x) {
            x.Effects = DragDropEffects.Copy;
            x.Handled = true;
        }

        private void Drop(DragEventArgs e) {
            var files = (string[])e.Data.GetData(DataFormats.FileDrop);
            ScriptFilePath.Value = (files?.Length ?? 0) == 0 ? "" : files[0];
        }

        private void StartEditBinding(RoutedEventArgs obj) {
            IsEditingBinding.Value = true;
            Keys.Value = new HashSet<Key>();
        }

        private void EndEditBinding(RoutedEventArgs obj) {
            IsEditingBinding.Value = false;
            if(GetBinding() != _currentBinding) CommitChanges();
        }

        private bool KeyPressed(IReadOnlyList<Key> arg) {
            if(arg.Count == 1) {
                switch(arg[0]) {
                    case Key.Escape:
                        _keyPressHandler.ClearPressedKeys();
                        Keys.Value = _currentBinding.Keys;
                        EndEditBinding(null);
                        return true;
                    case Key.Enter:
                        _keyPressHandler.ClearPressedKeys();
                        EndEditBinding(null);
                        return true;
                }
            }
            
            Keys.Value = new HashSet<Key>(arg);
            return true;
        }
    }
}
