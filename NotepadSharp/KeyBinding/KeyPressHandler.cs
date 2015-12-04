using System.Collections.Generic;
using System.Windows.Input;
using System.Linq;
using System;
using WPFUtility;

namespace NotepadSharp {
    public class KeyPressHandler {
        HashSet<Key> _pressedKeys = new HashSet<Key>();
        protected Func<IReadOnlyList<Key>, bool> _keyPressedCallback, _keyReleasedCallback;

        public KeyPressHandler(Func<IReadOnlyList<Key>, bool> keyPressedCallback = null, 
                               Func<IReadOnlyList<Key>, bool> keyReleasedCallback = null, 
                               Func<object, bool> keyDownCanExecute = null, 
                               Func<object, bool> keyUpCanExecute = null) 
        {
            _keyPressedCallback = keyPressedCallback ?? new Func<IReadOnlyList<Key>, bool>(x => false);
            _keyReleasedCallback = keyReleasedCallback ?? new Func<IReadOnlyList<Key>, bool>(x => false);
            keyDownCanExecute = keyDownCanExecute ?? new Func<object, bool>(x => true);
            keyUpCanExecute = keyUpCanExecute ?? new Func<object, bool>(x => true);

            KeyDownCommand = new RelayCommand(x => KeyDown((KeyEventArgs)x), keyDownCanExecute);
            KeyUpCommand = new RelayCommand(x => KeyUp((KeyEventArgs)x), keyUpCanExecute);
        }

        public ICommand KeyDownCommand { get; }
        public ICommand KeyUpCommand { get; }

        public void ClearPressedKeys() {
            _pressedKeys.Clear();
            _keyReleasedCallback(_pressedKeys.ToList());
        }

        private void KeyUp(KeyEventArgs e) {
            _pressedKeys.Remove(GetRelevantKey(e));
            e.Handled = _keyReleasedCallback(_pressedKeys.ToList());
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