using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace NotepadSharp
{
    public class KeyBindingExecution : KeyPressHandler
    {
        Dictionary<string, object> _scriptArgs = new Dictionary<string, object>();

        public KeyBindingExecution() {
            _keyPressedCallback = KeyPressed;
        }

        public void SetScriptArg(string name, object arg) {
            _scriptArgs[name] = arg;
        }

        public void ClearScriptArg(string name) {
            _scriptArgs.Remove(name);
        }

        //returns whether or not a binding existed with these keys
        private bool KeyPressed(IReadOnlyList<Key> keys) {
            //if any keybinding matches this set of keys, execute it
            var maybeBinding = ArgsAndSettings.KeyBindings.FirstOrDefault(x => x.Keys.SetEquals(keys));
            maybeBinding?.Execute(_scriptArgs);
            return maybeBinding != null;
        }
    }
}
