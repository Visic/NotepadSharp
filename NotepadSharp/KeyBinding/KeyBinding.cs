using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Utility;

namespace NotepadSharp {
    public abstract class KeyBinding {
        public const int C_Unassigned = -1;
        int _hashCode = -1;

        public KeyBinding(params Key[] keys) {
            Keys = new HashSet<Key>(keys);
        }

        public KeyBinding(KeyBinding otherBinding, params Key[] keys) :
            this(keys)
        {
            UID = otherBinding.UID;
            Label = otherBinding.Label;
        }

        [JsonIgnore]
        public string UID { get; } = Guid.NewGuid().ToString();
        
        public string Label { get; protected set; } = "";

        HashSet<Key> _keys;
        public HashSet<Key> Keys {
            get { return _keys; }
            protected set 
            {
                _keys = value;
                _hashCode = _keys.Count > 0 ? int.Parse(_keys.Select(x => (int)x).ToDelimitedString("")) : C_Unassigned;
            }
        }

        public abstract void Execute();

        public override int GetHashCode() {
            return _hashCode;
        }

        public override bool Equals(object obj) {
            var maybeHashcode = (obj as KeyBinding)?.GetHashCode();
            return maybeHashcode.HasValue && maybeHashcode == _hashCode;
        }

        public static bool operator ==(KeyBinding a, KeyBinding b) {
            return a?.Equals(b) ?? false;
        }

        public static bool operator !=(KeyBinding a, KeyBinding b) {
            return !(a == b);
        }
    }
}
