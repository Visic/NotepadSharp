using NLua;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFUtility;

namespace NotepadSharp {
    public static class LuaScriptRunner {
        public static void Execute(string filepath, Dictionary<string, object> args) {
            var state = new Lua();
            foreach(var ele in args) {
                state[ele.Key] = ele.Value;
            }

            try {
                var result = state.DoFile(filepath)[0];
                //WindowGenerator.Popup(result.ToString());
            }
            catch(Exception ex) {
                WindowGenerator.Popup(ex.Message);
            }
            state.Dispose();
        }
    }
}
