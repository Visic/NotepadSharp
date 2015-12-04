using System;
using System.Linq;
using System.IO;
using Utility;
using WPFUtility;

namespace NotepadSharp {
    public class DocumentViewModel : ViewModelBase {
        public DocumentViewModel(string filePath) {
            var fileInfo = ArgsAndSettings.CachedFiles.FirstOrDefault(x => x.OriginalFilePath == filePath);

            if(fileInfo == null) {
                var uid = Guid.NewGuid().ToString();

                fileInfo = new SerializableFileInfo();
                fileInfo.CachedFilePath = Path.Combine(Constants.FileCachePath, uid);
                fileInfo.IsDirty = false;
                fileInfo.OriginalFilePath = filePath;
                fileInfo.Hash = Methods.HashFile(filePath);

                File.Copy(filePath, fileInfo.CachedFilePath);
                ArgsAndSettings.CachedFiles.Add(fileInfo);
            }

            Title = Path.GetFileName(filePath);
            DocumentContent = new RichTextViewModel(fileInfo);
            IsDirty = DocumentContent.IsDirty;
        }

        public string Title { get; }
        public RichTextViewModel DocumentContent { get; }
        public NotifyingProperty<bool> IsDirty { get; }
    }
}
