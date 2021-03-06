﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Utility;

namespace NotepadSharp {
    [Serializable]
    public abstract class KeyBinding {
        public const int C_Unassigned = -1;
        int _hashCode = -1;

        public KeyBinding(ulong displayIndex, params Key[] keys) {
            DisplayIndex = displayIndex;
            Keys = new HashSet<Key>(keys);
        }

        public KeyBinding(KeyBinding otherBinding, params Key[] keys) :
            this(otherBinding.DisplayIndex, keys) {
            UID = otherBinding.UID;
            ExecuteOnKeyDown = otherBinding.ExecuteOnKeyDown;
            ExecuteOnKeyUp = otherBinding.ExecuteOnKeyUp;
            RepeatOnKeyDown = otherBinding.RepeatOnKeyDown;
        }

        public string UID { get; } = Guid.NewGuid().ToString();
        public ulong DisplayIndex { get; }
        public bool ExecuteOnKeyDown { get; set; }
        public bool ExecuteOnKeyUp { get; set; }
        public bool RepeatOnKeyDown { get; set; }

        HashSet<Key> _keys;
        public HashSet<Key> Keys {
            get { return _keys; }
            protected set {
                _keys = value;
                _hashCode = _keys.Count > 0 ? int.Parse(_keys.Select(x => (int)x).ToDelimitedString("")) : C_Unassigned;
            }
        }

        public abstract Option<Exception> Execute(Dictionary<string, object> args);

        public override int GetHashCode() {
            return _hashCode;
        }

        public override bool Equals(object obj) {
            if (!(obj is KeyBinding) || obj == null) return false;

            var otherBinding = (KeyBinding)obj;
            return otherBinding._hashCode == _hashCode && 
                   otherBinding.ExecuteOnKeyDown == ExecuteOnKeyDown && 
                   otherBinding.ExecuteOnKeyUp == ExecuteOnKeyUp && 
                   otherBinding.RepeatOnKeyDown == RepeatOnKeyDown;
        }

        public static bool operator ==(KeyBinding a, KeyBinding b) {
            return a?.Equals(b) ?? (object)b == null;
        }

        public static bool operator !=(KeyBinding a, KeyBinding b) {
            return !(a == b);
        }
    }
}
