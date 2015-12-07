using System;
using System.Collections.Generic;
using System.IO;
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

        public KeyBindingViewModel(KeyBinding binding, Action<KeyBinding, KeyBinding> bindingChangedCallback, Action<KeyBindingViewModel> deleteBindingCallback) {
            _currentBinding = binding;
            _bindingChangedCallback = bindingChangedCallback;
            KeyPressHandler = new KeyPressHandler(
                KeyPressed, 
                keyDownCanExecute: x => IsEditingBinding.Value, 
                keyUpCanExecute: x => IsEditingBinding.Value
            );
            Keys = new NotifyingProperty<HashSet<Key>>(_currentBinding.Keys);
            ScriptFilePath = new NotifyingProperty<string>(x => CommitChanges(), (binding as LuaKeyBinding)?.ScriptPath);
            ExecuteOnKeyUp = new NotifyingProperty<bool>(x => { _currentBinding.ExecuteOnKeyUp = x; CommitChanges(); }, _currentBinding.ExecuteOnKeyUp);
            ExecuteOnKeyDown = new NotifyingProperty<bool>(
                x => {
                    _currentBinding.ExecuteOnKeyDown = x;
                    if(!x) RepeatOnKeyDown.Value = x;
                    CommitChanges();
                }, 
                _currentBinding.ExecuteOnKeyDown
            );
            RepeatOnKeyDown = new NotifyingProperty<bool>(
                x => {
                    _currentBinding.RepeatOnKeyDown = x;
                    if (x) ExecuteOnKeyDown.Value = x;
                }, 
                _currentBinding.RepeatOnKeyDown
            );
            IsEditingBinding = new NotifyingProperty<bool>();
            StartEditingCommand = new RelayCommand(x => StartEditBinding((RoutedEventArgs)x));
            EndEditingCommand = new RelayCommand(x => EndEditBinding((RoutedEventArgs)x));
            if (deleteBindingCallback != null) DeleteBindingCommand = new RelayCommand(x => deleteBindingCallback(this));

            ScriptFilePathGotFocusCommand = new RelayCommand(x => ScriptFilePathIsFocused.Value = true);
            ScriptFilePathLostFocusCommand = new RelayCommand(x => ScriptFilePathIsFocused.Value = false);
            DragDrop = new DragAndDropHandler(AllowDrop, Drop);
            LostKeyboardFocusCommand = new RelayCommand(x => KeyPressHandler.ClearPressedKeys());
        }

        public KeyPressHandler KeyPressHandler { get; }
        public NotifyingProperty<bool> ScriptFilePathIsFocused { get; } = new NotifyingProperty<bool>();
        public NotifyingProperty<HashSet<Key>> Keys { get; private set; }
        public NotifyingProperty<string> ScriptFilePath { get; }
        public NotifyingProperty<bool> ExecuteOnKeyDown { get; }
        public NotifyingProperty<bool> ExecuteOnKeyUp { get; }
        public NotifyingProperty<bool> RepeatOnKeyDown { get; }
        public NotifyingProperty<bool> IsEditingBinding { get; }
        public DragAndDropHandler DragDrop { get; }
        public ICommand KeyDownCommand { get; }
        public ICommand KeyUpCommand { get; }
        public ICommand StartEditingCommand { get; }
        public ICommand EndEditingCommand { get; }
        public ICommand DeleteBindingCommand { get; }
        public ICommand ScriptFilePathGotFocusCommand { get; }
        public ICommand ScriptFilePathLostFocusCommand { get; }
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

        private bool AllowDrop(string path) {
            return File.Exists(path);
        }

        private void Drop(string path) {
            ScriptFilePath.Value = path;
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
                        KeyPressHandler.ClearPressedKeys();
                        Keys.Value = _currentBinding.Keys;
                        EndEditBinding(null);
                        return true;
                    case Key.Enter:
                        KeyPressHandler.ClearPressedKeys();
                        EndEditBinding(null);
                        return true;
                }
            }
            
            Keys.Value = new HashSet<Key>(arg);
            return true;
        }
    }
}
