using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace NotepadSharp {
    public class KeyBindingCollection : IEnumerable<KeyBinding> {
        public static readonly Key[] C_AllowedFirstKeys = new Key[] { Key.LeftCtrl, Key.RightCtrl, Key.LeftAlt, Key.RightAlt };

        PersistentCollection<LuaKeyBinding> _persistentLuaBindings;
        List<KeyBinding> _unbound = new List<KeyBinding>();
        HashSet<KeyBinding> _keyBindings = new HashSet<KeyBinding>();
        HashSet<Key> _pressedKeys = new HashSet<Key>();
        bool _initializing = true;

        public IReadOnlyCollection<Key> PressedKeys { get { return _pressedKeys; } }

        public KeyBindingCollection(PersistentCollection<LuaKeyBinding> persistentLuaBindings) {
            _persistentLuaBindings = persistentLuaBindings;
            foreach(var ele in _persistentLuaBindings.Collection) {
                SetBinding(ele);
            }

            _initializing = false;
        }

        public void SetBinding(KeyBinding keyBinding) {
            if (keyBinding.GetHashCode() == KeyBinding.C_Unassigned) Add(keyBinding, _unbound);
            if (!keyBinding.Keys.Overlaps(C_AllowedFirstKeys)) return;

            //if the binding already exists, remove it (essentially letting us overwriting the associated action)
            if (_keyBindings.Contains(keyBinding)) Remove(keyBinding, _keyBindings);
            Add(keyBinding, _keyBindings);
        }

        public void ClearBinding(KeyBinding keyBinding) {
            if (keyBinding.GetHashCode() != KeyBinding.C_Unassigned) Remove(keyBinding, _keyBindings);
            else Remove(_unbound.FirstOrDefault(x => x.UID == keyBinding.UID), _unbound);
        }

        //returns whether or not a binding was executed
        public bool KeyPressed(Key key, bool executeBinding) {
            if(_pressedKeys.Count == 0 && !C_AllowedFirstKeys.Contains(key)) return false;
            _pressedKeys.Add(key);
            if (!executeBinding) return true;

            //if any keybinding matches this set of keys, execute it
            var maybeBinding = _keyBindings.FirstOrDefault(x => x.Keys.SetEquals(_pressedKeys));
            maybeBinding?.Execute();
            return maybeBinding != null;
        }

        public void KeyReleased(Key key) {
            _pressedKeys.Remove(key);
        }

        public IEnumerator<KeyBinding> GetEnumerator() {
            return _unbound.Concat(_keyBindings).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return _unbound.Concat(_keyBindings).GetEnumerator();
        }

        private void Add(KeyBinding binding, ICollection<KeyBinding> collection) {
            collection.Add(binding);
            if (!_initializing && binding is LuaKeyBinding) _persistentLuaBindings.Add((LuaKeyBinding)binding);
        }

        private void Remove(KeyBinding binding, ICollection<KeyBinding> collection) {
            collection.Remove(binding);
            if (!_initializing && binding is LuaKeyBinding) _persistentLuaBindings.Remove((LuaKeyBinding)binding);
        }
    }
}
