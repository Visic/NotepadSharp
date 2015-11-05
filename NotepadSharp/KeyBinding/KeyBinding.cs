using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Utility;

namespace NotepadSharp {
    public class KeyBinding {
        readonly int _hashCode;

        public KeyBinding(Action action, string label = "", params Key[] keys) {
            Action = action;
            Label = label;
            Keys = new HashSet<Key>(keys);
            _hashCode = int.Parse(keys.Select(x => (int)x).ToDelimitedString(""));
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
    }
}
