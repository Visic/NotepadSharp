using System.Windows.Input;
using System;
using System.Collections.Generic;

namespace NotepadSharp {
    [Serializable]
    public class LuaKeyBinding : KeyBinding {
        public LuaKeyBinding(KeyBinding otherBinding, string scriptPath, string label, params Key[] keys)
            : base(otherBinding, keys)
        {
            ScriptPath = scriptPath;
            Label = label;
        }

        public LuaKeyBinding(string scriptPath, string label = "", params Key[] keys)
            : base(keys)
        {
            ScriptPath = scriptPath;
            Label = label;
        }

        public string ScriptPath { get; }

        public override void Execute(Dictionary<string, object> args) {
            LuaScriptRunner.Execute(ScriptPath, args);
        }
    }
}
