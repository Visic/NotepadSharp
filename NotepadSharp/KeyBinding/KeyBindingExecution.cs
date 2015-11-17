﻿using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace NotepadSharp {
    public class KeyBindingExecution : KeyPressHandler
    {
        Dictionary<string, object> _scriptArgs = new Dictionary<string, object>();
        KeyBinding _currentBinding;
        bool _repeat;

        public KeyBindingExecution() {
            _keyPressedCallback = KeyPressed;
            _keyReleasedCallback = KeyReleased;
        }

        public void SetScriptArg(string name, object arg) {
            _scriptArgs[name] = arg;
        }

        public void ClearScriptArg(string name) {
            _scriptArgs.Remove(name);
        }

        //returns whether or not we executed the binding
        private bool KeyReleased(IReadOnlyList<Key> keys) {
            bool executed = false;
            if (_currentBinding != null && _currentBinding.ExecuteOnKeyUp) {
                _currentBinding.Execute(_scriptArgs);
                executed = true;
            }

            _currentBinding = null;
            return executed;
        }

        //returns whether or not a binding existed with these keys
        private bool KeyPressed(IReadOnlyList<Key> keys) {
            if (_currentBinding != null && !_repeat) return true;

            //if any keybinding matches this set of keys, make it the new current binding
            if(_currentBinding == null) {
                _currentBinding = ArgsAndSettings.KeyBindings.FirstOrDefault(x => x.Keys.SetEquals(keys));
                _repeat = _currentBinding?.RepeatOnKeyDown ?? false;
            }

            if(_currentBinding != null &&_currentBinding.ExecuteOnKeyDown) _currentBinding.Execute(_scriptArgs);
            return _currentBinding != null;
        }
    }
}