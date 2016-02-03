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
        public List<SerializableFileInfo> CachedFiles {
            get {
                return ((List<SerializableFileInfo>)(this[Constants.CachedFileCollectionSettingName]));
            }
            set {
                this[Constants.CachedFileCollectionSettingName] = value;
            }
        }

        [System.Configuration.UserScopedSetting()]
        [System.Diagnostics.DebuggerNonUserCode()]
        [System.Configuration.SettingsSerializeAs(System.Configuration.SettingsSerializeAs.Binary)]
        public List<string> FavoritedLocations {
            get {
                return ((List<string>)(this[Constants.FavoritedLocationsCollectionSettingName]));
            }
            set {
                this[Constants.FavoritedLocationsCollectionSettingName] = value;
            }
        }
    }
}
