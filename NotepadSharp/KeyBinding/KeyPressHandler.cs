using System.Collections.Generic;
using System.Windows.Input;
using System.Linq;
using System;

namespace NotepadSharp {
    public class KeyPressHandler {
        HashSet<Key> _pressedKeys = new HashSet<Key>();
        protected Func<IReadOnlyList<Key>, bool> _keyPressedCallback, _keyReleasedCallback;

        public KeyPressHandler(Func<IReadOnlyList<Key>, bool> keyPressedCallback = null, Func<IReadOnlyList<Key>, bool> keyReleasedCallback = null) {
            _keyPressedCallback = keyPressedCallback ?? new Func<IReadOnlyList<Key>, bool>(x => false);
            _keyReleasedCallback = keyReleasedCallback ?? new Func<IReadOnlyList<Key>, bool>(x => false);
        }

        public void ClearPressedKeys() {
            _pressedKeys.Clear();
            _keyReleasedCallback(_pressedKeys.ToList());
        }

        public void KeyUp(KeyEventArgs e) {
            _pressedKeys.Remove(GetRelevantKey(e));
            e.Handled = _keyReleasedCallback(_pressedKeys.ToList());
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