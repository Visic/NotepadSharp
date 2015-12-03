using System;
using Utility;

namespace NotepadSharp {
    public class PersistentCrossReferenceCollection<T1, T2> : CrossReferenceCollection<T1, T2> {
        PersistentCollection<SerializableTuple<T1, T2>> _persistentCollection;

        public PersistentCrossReferenceCollection(PersistentCollection<SerializableTuple<T1, T2>> persistentCollection) {
            _persistentCollection = persistentCollection;

            foreach(var ele in persistentCollection) {
                base.Add(ele);
            }
        }

        public override void Add(Tuple<T1, T2> tuple) {
            base.Add(tuple);
            _persistentCollection.Add(tuple);
        }

        public override void Add(T1 t1, T2 t2) {
            Add(Tuple.Create(t1, t2));
        }

        public override bool RemoveByT1(T1 t1) {
            T2 t2;
            if(TryGetT2(t1, out t2)) {
                _persistentCollection.Remove(Tuple.Create(t1, t2));
            }
            return base.RemoveByT1(t1);
        }

        public override bool RemoveByT2(T2 t2) {
            T1 t1;
            if(TryGetT1(t2, out t1)) {
                _persistentCollection.Remove(Tuple.Create(t1, t2));
            }
            return base.RemoveByT2(t2);
        }
    }
}
