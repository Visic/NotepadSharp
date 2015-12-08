using NLua;
using System;
using System.Collections.Generic;
using System.IO;
using Utility;

namespace NotepadSharp {
    public static class LuaScriptRunner {
        public static Option<Exception> Execute(string pathOrLiteral, Dictionary<string, object> args) {
            using(var state = new Lua()) {
                Exception internalEx = null;
                state.HookException += (s, e) => internalEx = e.Exception;

                foreach(var ele in args) {
                    state[ele.Key] = ele.Value;
                }

                try {
                    if(File.Exists(pathOrLiteral) || Path.GetExtension(pathOrLiteral).ToLower() == ".lua" || Path.IsPathRooted(pathOrLiteral)) {
                        state.DoFile(pathOrLiteral);
                    } else {
                        state.DoString(pathOrLiteral);
                    }
                    return internalEx != null ? Option.New(internalEx) : Option.New<Exception>();
                } catch(Exception ex) {
                    return Option.New(ex);
                }
            }
        }
    }
}
