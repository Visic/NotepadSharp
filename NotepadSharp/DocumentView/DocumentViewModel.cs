using System;
using System.IO;
using WPFUtility;

namespace NotepadSharp {
    public class DocumentViewModel : ViewModelBase {
        public DocumentViewModel(string filePath) {
            string uid, cachedFilePath;
            if(!ArgsAndSettings.CachedFiles.TryGetT1(filePath, out uid)) {
                uid = Guid.NewGuid().ToString();
                cachedFilePath = Path.Combine(Constants.FileCachePath, uid);
                ArgsAndSettings.CachedFiles.Add(uid, filePath);
            } else {
                cachedFilePath = Path.Combine(Constants.FileCachePath, uid);
            }

            if (!File.Exists(cachedFilePath)) File.Copy(filePath, cachedFilePath);

            Title = Path.GetFileName(filePath);
            DocumentContent = new RichTextViewModel(File.ReadAllText(cachedFilePath));
            IsDirty = DocumentContent.IsDirty;
        }

        public string Title { get; }
        public RichTextViewModel DocumentContent { get; }
        public NotifyingProperty<bool> IsDirty { get; }
    }
}
