using System.Collections.Generic;
using System.Windows.Input;
using System.Linq;
using System;
using WPFUtility;

namespace NotepadSharp {
    public class KeyPressHandler {
        static HashSet<Key> _pressedKeys = new HashSet<Key>();
        protected Func<IReadOnlyList<Key>, bool> _keyPressedCallback;
        protected Func<IReadOnlyList<Key>, IReadOnlyList<Key>, bool> _keyReleasedCallback;

        public KeyPressHandler(Func<IReadOnlyList<Key>, bool> keyPressedCallback = null, 
                               Func<IReadOnlyList<Key>, IReadOnlyList<Key>, bool> keyReleasedCallback = null, 
                               Func<object, bool> keyDownCanExecute = null, 
                               Func<object, bool> keyUpCanExecute = null) 
        {
            _keyPressedCallback = keyPressedCallback ?? new Func<IReadOnlyList<Key>, bool>(x => false);
            _keyReleasedCallback = keyReleasedCallback ?? new Func<IReadOnlyList<Key>, IReadOnlyList<Key>, bool>((x, y) => false);
            keyDownCanExecute = keyDownCanExecute ?? new Func<object, bool>(x => true);
            keyUpCanExecute = keyUpCanExecute ?? new Func<object, bool>(x => true);

            KeyDownCommand = new RelayCommand(x => KeyDown((KeyEventArgs)x), keyDownCanExecute);
            KeyUpCommand = new RelayCommand(x => KeyUp((KeyEventArgs)x), keyUpCanExecute);
        }

        public ICommand KeyDownCommand { get; }
        public ICommand KeyUpCommand { get; }

        public void ClearPressedKeys() {
            _keyReleasedCallback(new List<Key>(), _pressedKeys.ToList());
            _pressedKeys.Clear();
        }

        private void KeyUp(KeyEventArgs e) {
            var releasedKey = GetRelevantKey(e);
            _pressedKeys.Remove(releasedKey);
            e.Handled = _keyReleasedCallback(_pressedKeys.ToList(), new List<Key>() { releasedKey });
        }
        
        private void KeyDown(KeyEventArgs e) {
            _pressedKeys.Add(GetRelevantKey(e));
            e.Handled = _keyPressedCallback(_pressedKeys.ToList());
        }

        private Key GetRelevantKey(KeyEventArgs e) {
            return e.Key == Key.System ? e.SystemKey : e.Key;
        }
    }
}