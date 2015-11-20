using System.Collections.Generic;

namespace NotepadSharp.Properties {
    internal sealed partial class Settings {
        [System.Configuration.UserScopedSetting()]
        [System.Diagnostics.DebuggerNonUserCode()]
        [System.Configuration.SettingsSerializeAs(System.Configuration.SettingsSerializeAs.Binary)]
        public List<LuaKeyBinding> KeyBindings {
            get {
                return ((List<LuaKeyBinding>)(this["KeyBindings"]));
            }
            set {
                this["KeyBindings"] = value;
            }
        }
    }
}
