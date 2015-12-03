using System;
using System.Collections.Generic;

namespace NotepadSharp.Properties {
    internal sealed partial class Settings {
        [System.Configuration.UserScopedSetting()]
        [System.Diagnostics.DebuggerNonUserCode()]
        [System.Configuration.SettingsSerializeAs(System.Configuration.SettingsSerializeAs.Binary)]
        public List<LuaKeyBinding> KeyBindings {
            get {
                return ((List<LuaKeyBinding>)(this[Constants.KeyBindingCollectionSettingName]));
            }
            set {
                this[Constants.KeyBindingCollectionSettingName] = value;
            }
        }

        [System.Configuration.UserScopedSetting()]
        [System.Diagnostics.DebuggerNonUserCode()]
        [System.Configuration.SettingsSerializeAs(System.Configuration.SettingsSerializeAs.Binary)]
        public List<SerializableTuple<string, string>> CachedFiles {
            get {
                return ((List<SerializableTuple<string, string>>)(this[Constants.CachedFileCollectionSettingName]));
            }
            set {
                this[Constants.CachedFileCollectionSettingName] = value;
            }
        }
    }
}
