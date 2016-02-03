using System.Collections.Generic;
using NotepadSharp.Properties;
using System.Collections;
using System.Collections.Specialized;

namespace NotepadSharp {
    public class PersistentCollection<T> : IEnumerable<T>, INotifyCollectionChanged {
        public event NotifyCollectionChangedEventHandler CollectionChanged;
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
            NotifyAdded(ele);
        }

        public void Remove(T ele) {
            var collection = GetSavedCollection();
            if(collection.Contains(ele)) {
                var index = collection.IndexOf(ele);
                collection.RemoveAt(index);
                ArgsAndSettings.SaveSettings();
                NotifyRemoved(ele, index);
            }
        }

        public void AddOrReplace(T ele) {
            Remove(ele);
            Add(ele);
        }

        public bool Contains(T ele) {
            return GetSavedCollection().Contains(ele);
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

        private void NotifyRemoved(T ele, int index) {
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, ele, index));
        }

        private void NotifyAdded(T ele) {
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, ele));
        }
    }
}
