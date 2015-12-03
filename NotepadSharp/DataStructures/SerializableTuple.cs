using System;

namespace NotepadSharp {
    [Serializable]
    public class SerializableTuple<T1, T2> {
        public T1 Item1 { get; set; }
        public T2 Item2 { get; set; }

        public static implicit operator Tuple<T1, T2>(SerializableTuple<T1, T2> src) {
            return new Tuple<T1, T2>(src.Item1, src.Item2);
        }

        public static implicit operator SerializableTuple<T1, T2>(Tuple<T1, T2> src) {
            return new SerializableTuple<T1, T2>() { Item1 = src.Item1, Item2 = src.Item2 };
        }
    }
}
