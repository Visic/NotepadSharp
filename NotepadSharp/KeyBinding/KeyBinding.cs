using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Utility;

namespace NotepadSharp {
    public class KeyBinding {
        readonly Action _op;
        readonly int _hashCode;

        public KeyBinding(Action op, string function = "", params Key[] keys) {
            _op = op;
            Function = function;
            Keys = new HashSet<Key>(keys);
            _hashCode = int.Parse(keys.Select(x => (int)x).ToDelimitedString(""));
        }

        public HashSet<Key> Keys { get; }
        public string Function { get; }

        public void Execute() {
            _op();
        }

        public override int GetHashCode() {
            return _hashCode;
        }

        public override bool Equals(object obj) {
            var maybeHashcode = (obj as KeyBinding)?.GetHashCode();
            return maybeHashcode.HasValue && maybeHashcode == _hashCode;
        }
    }
}
