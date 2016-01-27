using System.Windows.Input;
using System;
using System.Collections.Generic;
using Utility;

namespace NotepadSharp {
    [Serializable]
    public class LuaKeyBinding : KeyBinding {
        public LuaKeyBinding(KeyBinding otherBinding, string pathOrLiteral, params Key[] keys)
            : base(otherBinding, keys)
        {
            PathOrLiteral = pathOrLiteral;
        }

        public LuaKeyBinding(ulong displayIndex, string pathOrLiteral, params Key[] keys)
            : base(displayIndex, keys)
        {
            PathOrLiteral = pathOrLiteral;
        }

        public string PathOrLiteral { get; }

        public override Option<Exception> Execute(Dictionary<string, object> args) {
            return LuaScriptRunner.Execute(PathOrLiteral, args);
        }
    }
}
