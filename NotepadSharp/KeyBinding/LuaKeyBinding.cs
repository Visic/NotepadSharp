using System.Windows.Input;
using WPFUtility;

namespace NotepadSharp {
    public class LuaKeyBinding : KeyBinding {
        public LuaKeyBinding(string scriptPath, string label = "", params Key[] keys)
            : base(() => LuaScriptRunner.Execute(scriptPath), label, keys)
        {
            ScriptPath = new NotifyingProperty<string>(scriptPath);
        }

        public NotifyingProperty<string> ScriptPath { get; }
    }
}
