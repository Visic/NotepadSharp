using System.Collections.Generic;
using NotepadSharp.Properties;
using System.Collections;

namespace NotepadSharp {
    public class PersistentCollection<T> : IEnumerable<T> {
        string _settingName;

        public PersistentCollection(string settingName) {
            _settingName = settingName;
            if(GetSavedCollection() == null) {
                Settings.Default[_settingName] = new List<T>();
                ArgsAndSettings.SaveSettings();
            }
        }

        public IReadOnlyList<T> Collection { get { return GetSavedCollection(); } }

        public void Add(T ele) {
            GetSavedCollection().Add(ele);
            ArgsAndSettings.SaveSettings();
        }

        public void Remove(T ele) {
            GetSavedCollection().Remove(ele);
            ArgsAndSettings.SaveSettings();
        }

        public void AddOrReplace(T ele) {
            GetSavedCollection().Remove(ele);
            Add(ele);
        }

        public IEnumerator<T> GetEnumerator() {
            return Collection.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return Collection.GetEnumerator();
        }

        private List<T> GetSavedCollection() {
            return (List<T>)Settings.Default[_settingName];
        }
    }
}
