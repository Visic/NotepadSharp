using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace NotepadSharp {
    public class KeyBindingCollection : IEnumerable<KeyBinding> {
        public static readonly Key[] C_AllowedFirstKeys = new Key[] { Key.LeftCtrl, Key.RightCtrl, Key.LeftAlt, Key.RightAlt };

        HashSet<KeyBinding> _keyBindings = new HashSet<KeyBinding>();
        HashSet<Key> _pressedKeys = new HashSet<Key>();

        public bool SetBinding(KeyBinding keyBinding) {
            if (!keyBinding.Keys.Overlaps(C_AllowedFirstKeys)) return false;

            //if the binding already exists, remove it (essentially letting us overwriting the associated action)
            if (_keyBindings.Contains(keyBinding)) _keyBindings.Remove(keyBinding);
            _keyBindings.Add(keyBinding);
            return true;
        }

        public void ClearBinding(KeyBinding keyBinding) {
            _keyBindings.Remove(keyBinding);
        }

        public void ClearBinding(params Key[] keys) {
            _keyBindings.Remove(new KeyBinding(() => { }, keys: keys));
        }

        //returns whether or not a binding was executed
        public bool KeyPressed(Key key) {
            if(_pressedKeys.Count == 0 && !C_AllowedFirstKeys.Contains(key)) return false;
            _pressedKeys.Add(key);

            //if any keybinding matches this set of keys, execute it
            var maybeBinding = _keyBindings.FirstOrDefault(x => x.Keys.SetEquals(_pressedKeys));
            maybeBinding?.Execute();
            return maybeBinding != null;
        }

        public void KeyReleased(Key key) {
            _pressedKeys.Remove(key);
        }

        public IEnumerator<KeyBinding> GetEnumerator() {
            return _keyBindings.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return _keyBindings.GetEnumerator();
        }
    }
}
