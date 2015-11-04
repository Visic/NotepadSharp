using System.Windows.Input;

namespace NotepadSharp {
    public class LuaKeyBinding : KeyBinding {
        public LuaKeyBinding(string scriptPath, params Key[] keys)
            : base(() => LuaScriptRunner.Execute(scriptPath), scriptPath, keys)
        {
        }
    }
}
