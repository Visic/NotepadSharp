using System;
using System.Linq;
using System.IO;
using Utility;
using WPFUtility;

namespace NotepadSharp {
    public class DocumentViewModel : ViewModelBase {
        SerializableFileInfo _fileInfo;

        public DocumentViewModel(string filePath) {
            _fileInfo = ArgsAndSettings.CachedFiles.FirstOrDefault(x => x.OriginalFilePath == filePath);

            if(_fileInfo == null) {
                var uid = Guid.NewGuid().ToString();

                _fileInfo = new SerializableFileInfo();
                _fileInfo.CachedFilePath = Path.Combine(Constants.FileCachePath, uid);
                _fileInfo.IsDirty = false;
                _fileInfo.OriginalFilePath = filePath;
                _fileInfo.Hash = Methods.HashFile(filePath);

                File.Copy(filePath, _fileInfo.CachedFilePath);
                ArgsAndSettings.CachedFiles.Add(_fileInfo);
            }

            Title = Path.GetFileName(filePath);
            DocumentContent = new RichTextViewModel(_fileInfo);
            IsDirty = DocumentContent.IsDirty;
        }

        public string Title { get; }
        public RichTextViewModel DocumentContent { get; }
        public NotifyingProperty<bool> IsDirty { get; }
        
        public void Close() {
            ArgsAndSettings.CachedFiles.Remove(_fileInfo);
            File.Delete(_fileInfo.CachedFilePath);
        }
    }
}
