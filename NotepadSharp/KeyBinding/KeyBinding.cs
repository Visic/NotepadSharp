using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Utility;

namespace NotepadSharp {
    public class KeyBinding {
        public const int C_Unassigned = -1;
        readonly int _hashCode = -1;

        public KeyBinding(Action action, string label = "", params Key[] keys) {
            Action = action;
            Label = label;
            Keys = new HashSet<Key>(keys);
            if (keys.Length > 0) _hashCode = int.Parse(keys.Select(x => (int)x).ToDelimitedString(""));
        }

        public Action Action { get; }
        public HashSet<Key> Keys { get; }
        public string Label { get; }

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
