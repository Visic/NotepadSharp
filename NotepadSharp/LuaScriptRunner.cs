using NLua;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFUtility;

namespace NotepadSharp {
    public static class LuaScriptRunner {
        public static void Execute(string filepath) {
            var state = new Lua();
            //state.LoadCLRPackage(); --Allows access to everything {e.g. import("NotepadSharp", "NotepadSharp") allows you to access the entire application}
            //state["obj"] = new LuaKeyBinding(filepath);
            //state.RegisterFunction("TestFunc", typeof(ArgsAndSettings).GetMethod("SaveSettings"));

            try {
                var result = state.DoFile(filepath)[0];
                WindowGenerator.Popup(result.ToString());
            }
            catch(Exception ex) {
                WindowGenerator.Popup(ex.Message);
            }
            state.Dispose();
        }
    }
}
