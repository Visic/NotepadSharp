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

            try {
                var result = state.DoFile(filepath)[0];
                WindowGenerator.Popup((string)result);
            }
            catch(Exception ex) {
                WindowGenerator.Popup(ex.Message);
            }
            state.Dispose();
        }
    }
}
