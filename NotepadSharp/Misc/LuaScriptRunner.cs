using NLua;
using System;
using System.Collections.Generic;
using Utility;

namespace NotepadSharp {
    public static class LuaScriptRunner {
        public static Option<Exception> Execute(string filepath, Dictionary<string, object> args) {
            using(var state = new Lua()) {
                foreach(var ele in args) {
                    state[ele.Key] = ele.Value;
                }

                try {
                    state.DoFile(filepath);
                    return Option.New<Exception>();
                } catch(Exception ex) {
                    return Option.New(ex);
                }
            }
        }
    }
}
