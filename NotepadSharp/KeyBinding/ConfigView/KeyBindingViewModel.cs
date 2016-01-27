using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            PathOrLiteral = new NotifyingProperty<string>(x => CommitChanges(), (binding as LuaKeyBinding)?.PathOrLiteral);
            ExecuteOnKeyUp = new NotifyingProperty<bool>(
                x => {
                    if(IsEditingBinding.Value) EndEditBinding();
                    else CommitChanges();
                }, 
                _currentBinding.ExecuteOnKeyUp
            );
            ExecuteOnKeyDown = new NotifyingProperty<bool>(
                x => {
                    if(!x) RepeatOnKeyDown.Value = x;
                    else if(IsEditingBinding.Value) EndEditBinding();
                    else CommitChanges();
                }, 
                _currentBinding.ExecuteOnKeyDown
            );
            RepeatOnKeyDown = new NotifyingProperty<bool>(
                x => {
                    if (x) ExecuteOnKeyDown.Value = x;
                    else if(IsEditingBinding.Value) EndEditBinding();
                    else CommitChanges();
                }, 
                _currentBinding.RepeatOnKeyDown
            );
            IsEditingBinding = new NotifyingProperty<bool>();
            StartEditingCommand = new RelayCommand(x => StartEditBinding());
            EndEditingCommand = new RelayCommand(x => EndEditBinding());
            if (deleteBindingCallback != null) DeleteBindingCommand = new RelayCommand(x => deleteBindingCallback(this));

            PathOrLiteralGotFocusCommand = new RelayCommand(x => PathOrLiteralIsFocused.Value = true);
            PathOrLiteralLostFocusCommand = new RelayCommand(x => PathOrLiteralIsFocused.Value = false);
            DragDrop = new DragAndDropHandler(AllowDrop, Drop);
            LostKeyboardFocusCommand = new RelayCommand(x => KeyPressHandler.ClearPressedKeys());
        }

        public ulong DisplayIndex { get { return _currentBinding.DisplayIndex; } }
        public KeyPressHandler KeyPressHandler { get; }
        public NotifyingProperty<bool> PathOrLiteralIsFocused { get; } = new NotifyingProperty<bool>();
        public NotifyingProperty<HashSet<Key>> Keys { get; private set; }
        public NotifyingProperty<string> PathOrLiteral { get; }
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
        public ICommand PathOrLiteralGotFocusCommand { get; }
        public ICommand PathOrLiteralLostFocusCommand { get; }
        public ICommand LostKeyboardFocusCommand { get; }

        public KeyBinding GetBinding() {
            var newBinding = new LuaKeyBinding(_currentBinding, PathOrLiteral.Value, Keys.Value.ToArray());
            newBinding.ExecuteOnKeyDown = ExecuteOnKeyDown.Value;
            newBinding.ExecuteOnKeyUp = ExecuteOnKeyUp.Value;
            newBinding.RepeatOnKeyDown = RepeatOnKeyDown.Value;
            return newBinding;
        }

        public void ClearBinding() {
            StartEditBinding();
            EndEditBinding();
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
            PathOrLiteral.Value = path;
        }

        private void StartEditBinding() {
            IsEditingBinding.Value = true;
            Keys.Value = new HashSet<Key>();
        }

        private void EndEditBinding() {
            IsEditingBinding.Value = false;
            if(GetBinding() != _currentBinding) CommitChanges();
        }

        private bool KeyPressed(IReadOnlyList<Key> arg) {
            if(arg.Count == 1) {
                switch(arg[0]) {
                    case Key.Escape:
                        KeyPressHandler.ClearPressedKeys();
                        Keys.Value = _currentBinding.Keys;
                        EndEditBinding();
                        return true;
                    case Key.Enter:
                        KeyPressHandler.ClearPressedKeys();
                        EndEditBinding();
                        return true;
                }
            }
            
            Keys.Value = new HashSet<Key>(arg);
            return true;
        }
    }
}
