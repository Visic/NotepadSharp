using System;
using System.Linq;
using System.IO;
using Utility;
using WPFUtility;

namespace NotepadSharp {
    //This view model deals with abstracting the persistency away from the content vm
    public class DocumentViewModel : ViewModelBase {
        SerializableFileInfo _fileInfo;

        public DocumentViewModel(string filePath) {
            _fileInfo = ArgsAndSettings.CachedFiles.FirstOrDefault(x => x.OriginalFilePath == filePath);
            IsDirty = new NotifyingProperty<bool>(x => SaveFileInfo());

            if(_fileInfo == null) {
                var uid = Guid.NewGuid().ToString();
                _fileInfo = new SerializableFileInfo();
                _fileInfo.CachedFilePath = Path.Combine(Constants.FileCachePath, uid);
                _fileInfo.IsDirty = false;
                _fileInfo.OriginalFilePath = filePath;
                _fileInfo.Hash = Methods.HashFile(filePath);
                File.Copy(filePath, _fileInfo.CachedFilePath);
                ArgsAndSettings.CachedFiles.Add(_fileInfo);
            } else {
                UpdateIsDirty();
            }

            DocumentContent = new RichTextViewModel(_fileInfo.CachedFilePath);
            DocumentContent.Content.PropertyChanged += (s,e) => UpdateHash();
            DocumentContent.ApiProvider.Save = SaveChanges;
        }

        public RichTextViewModel DocumentContent { get; }
        public NotifyingProperty<bool> IsDirty { get; }
        
        public void Close() {
            ArgsAndSettings.CachedFiles.Remove(_fileInfo);
            File.Delete(_fileInfo.CachedFilePath);
        }

        private void UpdateIsDirty() {
            bool state = false;
            if(!File.Exists(_fileInfo.OriginalFilePath)) {
                state = true;
            } else if (Methods.HashFile(_fileInfo.OriginalFilePath) != _fileInfo.Hash) {
                state = true;
            }
            IsDirty.Value = state;
        }

        private void UpdateHash() {
            _fileInfo.Hash = Methods.Hash(DocumentContent.Content.Value);
            SaveFileInfo();
            UpdateIsDirty();
        }

        private void SaveFileInfo() {
            ArgsAndSettings.CachedFiles.AddOrReplace(_fileInfo);
        }
        
        private void SaveChanges() {
            if(IsDirty.Value) {
                UpdateCachedFile();
                File.Copy(_fileInfo.CachedFilePath, _fileInfo.OriginalFilePath, true);
                IsDirty.Value = false;
            }
        }

        private void UpdateCachedFile() {
            File.WriteAllText(_fileInfo.CachedFilePath, DocumentContent.Content.Value);
        }

        public override void Dispose() {
            base.Dispose();
            if (IsDirty.Value) UpdateCachedFile();
            DocumentContent.Dispose();
        }
    }
}
