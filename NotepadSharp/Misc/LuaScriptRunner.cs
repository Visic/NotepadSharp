using NLua;
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using Utility;
using System.Reflection;

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
                    if(IsValidLuaScriptPath(pathOrLiteral)) {
                        state.DoFile(pathOrLiteral);
                    } else {
                        state.DoString(pathOrLiteral);
                    }
                    return internalEx != null ? Option.New(internalEx) : Option.New<Exception>();
                } catch (TargetInvocationException ex) {
                    return Option.New(ex.InnerException);
                } catch(Exception ex) {
                    return Option.New(ex);
                }
            }
        }

        private static bool IsValidLuaScriptPath(string str) {
            Func<bool> hasInvalidChars = () => str.Except(Path.GetInvalidPathChars()).Count() == 0;
            Func<bool> exists = () => File.Exists(str);
            Func<bool> isLua = () => Path.GetExtension(str).ToLower() == ".lua";

            return !hasInvalidChars() && exists() && isLua();
        }
    }
}
