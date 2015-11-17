using System.Collections.Generic;
using System.Windows.Input;
using System.Linq;
using System;

namespace NotepadSharp {
    public class KeyPressHandler {
        HashSet<Key> _pressedKeys = new HashSet<Key>();
        protected Func<IReadOnlyList<Key>, bool> _keyPressedCallback;
        protected KeyPressHandler() { }

        public KeyPressHandler(Func<IReadOnlyList<Key>, bool> keyPressedCallback) {
            _keyPressedCallback = keyPressedCallback;
        }

        public void ClearPressedKeys() {
            _pressedKeys.Clear();
        }

        public void KeyUp(KeyEventArgs e) {
            _pressedKeys.Remove(GetRelevantKey(e));
        }
        
        public void KeyDown(KeyEventArgs e) {
            _pressedKeys.Add(GetRelevantKey(e));
            e.Handled = _keyPressedCallback(_pressedKeys.ToList());
        }

        private Key GetRelevantKey(KeyEventArgs e) {
            return e.Key == Key.System ? e.SystemKey : e.Key;
        }
    }
}