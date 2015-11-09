using Newtonsoft.Json;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Collections;
using NotepadSharp.Properties;
using System.Collections.Specialized;

namespace NotepadSharp {
    public class PersistentCollection<T> {
        string _settingName;

        public PersistentCollection(string settingName) {
            _settingName = settingName;
            if(GetSavedCollection() == null) {
                Settings.Default[_settingName] = new StringCollection();
                ArgsAndSettings.SaveSettings();
            }
        }

        public IEnumerable<T> Collection { get { return GetSavedCollection().Cast<string>().Select(x => JsonConvert.DeserializeObject<T>(x)).ToList(); } }

        public void Add(T ele) {
            GetSavedCollection().Add(JsonConvert.SerializeObject(ele));
            ArgsAndSettings.SaveSettings();
        }

        public void Remove(T ele) {
            GetSavedCollection().Remove(JsonConvert.SerializeObject(ele));
            ArgsAndSettings.SaveSettings();
        }

        private StringCollection GetSavedCollection() {
            return (StringCollection)Settings.Default[_settingName];
        }
    }
}
