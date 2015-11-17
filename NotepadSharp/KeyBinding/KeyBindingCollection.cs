using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace NotepadSharp {
    public class KeyBindingCollection : IEnumerable<KeyBinding> {
        PersistentCollection<LuaKeyBinding> _persistentLuaBindings;
        List<KeyBinding> _unbound = new List<KeyBinding>();
        HashSet<KeyBinding> _keyBindings = new HashSet<KeyBinding>();
        bool _initializing = true;

        public KeyBindingCollection(PersistentCollection<LuaKeyBinding> persistentLuaBindings) {
            _persistentLuaBindings = persistentLuaBindings;
            foreach(var ele in _persistentLuaBindings.Collection) {
                SetBinding(ele);
            }

            _initializing = false;
        }

        public void SetBinding(KeyBinding keyBinding) {
            if (keyBinding.GetHashCode() == KeyBinding.C_Unassigned) Add(keyBinding, _unbound);

            //if the binding already exists, remove it (essentially letting us overwriting the associated action)
            if (_keyBindings.Contains(keyBinding)) Remove(keyBinding, _keyBindings);
            Add(keyBinding, _keyBindings);
        }

        public void ClearBinding(KeyBinding keyBinding) {
            if (keyBinding.GetHashCode() != KeyBinding.C_Unassigned) Remove(keyBinding, _keyBindings);
            else Remove(_unbound.FirstOrDefault(x => x.UID == keyBinding.UID), _unbound);
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
