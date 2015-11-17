using System.Windows.Input;
using System;
using System.Collections.Generic;

namespace NotepadSharp {
    [Serializable]
    public class LuaKeyBinding : KeyBinding {
        public LuaKeyBinding(KeyBinding otherBinding, string scriptPath, params Key[] keys)
            : base(otherBinding, keys)
        {
            ScriptPath = scriptPath;
        }

        public LuaKeyBinding(string scriptPath, params Key[] keys)
            : base(keys)
        {
            ScriptPath = scriptPath;
        }

        public string ScriptPath { get; }

        public override void Execute(Dictionary<string, object> args) {
            LuaScriptRunner.Execute(ScriptPath, args);
        }
    }
}
