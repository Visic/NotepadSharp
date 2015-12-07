using System;

namespace NotepadSharp {
    [Serializable]
    public class SerializableFileInfo {
        public string Hash { get; set; }
        public string OriginalFilePath { get; set; }
        public string CachedFilePath { get; set; }
        public bool IsDirty { get; set; }

        public override int GetHashCode() {
            return OriginalFilePath?.GetHashCode() ?? CachedFilePath.GetHashCode();
        }

        public override bool Equals(object obj) {
            var fileInfo = obj as SerializableFileInfo;
            if (fileInfo == null) return false;

            return fileInfo.GetHashCode() == GetHashCode();
        }

        public static bool operator ==(SerializableFileInfo a, SerializableFileInfo b) {
            return a?.Equals(b) ?? (object)b == null;
        }

        public static bool operator !=(SerializableFileInfo a, SerializableFileInfo b) {
            return !(a == b);
        }
    }
}
