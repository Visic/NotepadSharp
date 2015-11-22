using NLua;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Utility;

namespace NotepadSharp {
    public static class LuaScriptRunner {
        public static Option<Exception> Execute(string filepath, Dictionary<string, object> args) {
            using(var state = new Lua()) {
                Exception internalEx = null;
                state.HookException += (s, e) => internalEx = e.Exception;
                foreach(var ele in args) {
                    state[ele.Key] = ele.Value;
                }

                try {
                    Task.Run(() => state.DoFile(filepath));
                    Thread.Sleep(500);
                    while(state.IsExecuting) { Thread.Sleep(1); }
                    return internalEx != null ? Option.New(internalEx) : Option.New<Exception>();
                } catch(Exception ex) {
                    return Option.New(ex);
                }
            }
        }
    }
}
