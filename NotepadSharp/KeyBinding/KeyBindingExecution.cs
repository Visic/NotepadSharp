using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace NotepadSharp {
    public class KeyBindingExecution : KeyPressHandler {
        Dictionary<string, object> _scriptArgs = new Dictionary<string, object>();
        IReadOnlyList<Key> _lastPressedKeys = new Key[0];
        Action<Exception> _exceptionHandler;

        public KeyBindingExecution(Action<Exception> exceptionHandler) {
            _exceptionHandler = exceptionHandler;
            _keyPressedCallback = KeyPressed;
            _keyReleasedCallback = KeyReleased;
        }

        public void SetScriptArgs(Dictionary<string, object> args) {
            foreach(var arg in args) {
                _scriptArgs[arg.Key] = arg.Value;
            }
        }

        public void SetScriptArg(string name, object arg) {
            _scriptArgs[name] = arg;
        }

        public void ClearScriptArg(string name) {
            _scriptArgs.Remove(name);
        }

        //returns whether or not we executed the binding
        private bool KeyReleased(IReadOnlyList<Key> pressedKeys, IReadOnlyList<Key> releasedKeys) { //TODO:: Re-evaluate now that I have [releasedKeys]
            var executed = false;
            var bindings = KeysChanged(pressedKeys);

            if (bindings.Item1 != null && bindings.Item1.ExecuteOnKeyUp) {
                bindings.Item1.Execute(_scriptArgs).Apply(_exceptionHandler);
                executed = true;
            }

            if (bindings.Item2 != null && bindings.Item2.RepeatOnKeyDown) {
                bindings.Item2.Execute(_scriptArgs).Apply(_exceptionHandler);
                executed = true;
            }

            return executed;
        }

        //returns whether or not a binding existed with these keys
        private bool KeyPressed(IReadOnlyList<Key> keys) {
            var bindings = KeysChanged(keys);
            if (bindings.Item2 != null && bindings.Item2.ExecuteOnKeyDown) { //if your supposed to execute it on key down
                if (bindings.Item1 != null && bindings.Item1 == bindings.Item2 && !bindings.Item1.RepeatOnKeyDown) return true; //if it didn't change and your not suppose to execute it on repeat
                bindings.Item2.Execute(_scriptArgs).Apply(_exceptionHandler);
            }

            return bindings.Item2 != null;
        }

        private Tuple<KeyBinding, KeyBinding> KeysChanged(IReadOnlyList<Key> keys) {
            var previousBinding = ArgsAndSettings.KeyBindings.FirstOrDefault(x => x.Keys.SetEquals(_lastPressedKeys));
            var currentBinding = ArgsAndSettings.KeyBindings.FirstOrDefault(x => x.Keys.SetEquals(keys));
            _lastPressedKeys = keys;
            return Tuple.Create(previousBinding, currentBinding);
        }
    }
}
