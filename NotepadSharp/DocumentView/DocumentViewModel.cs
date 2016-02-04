using System;
using System.Linq;
using System.IO;
using Utility;
using WPFUtility;
using Microsoft.Win32;
using System.Windows.Threading;

namespace NotepadSharp {
    //This view model deals with abstracting the persistency away from the content vm
    public class DocumentViewModel : ViewModelBase {
        SerializableFileInfo _fileInfo;
        DispatcherTimer _fileCheckTimer = new DispatcherTimer();

        public DocumentViewModel(string filePath, Action<string, string> filePathChangingCallback) {
            _fileInfo = ArgsAndSettings.CachedFiles.FirstOrDefault(x => x.OriginalFilePath == filePath || x.CachedFilePath == filePath);
            IsDirty = new NotifyingProperty<bool>(x => SaveFileInfo());

            if(_fileInfo == null) {
                var uid = Guid.NewGuid().ToString();
                var fileName = Path.GetFileName(filePath);
                var cachedDirectoryPath = Path.Combine(Constants.FileCachePath, uid);

                _fileInfo = new SerializableFileInfo();
                _fileInfo.CachedFilePath = Path.Combine(cachedDirectoryPath, fileName);
                Directory.CreateDirectory(cachedDirectoryPath);

                if(File.Exists(filePath)) {
                    _fileInfo.OriginalFilePath = filePath;
                    _fileInfo.Hash = Methods.HashFile(filePath);
                    File.Copy(filePath, _fileInfo.CachedFilePath);
                } else {
                    File.WriteAllText(_fileInfo.CachedFilePath, "");
                }

                ArgsAndSettings.CachedFiles.Add(_fileInfo);
            }

            FileName = new NotifyingProperty<string>(
                (oldName, newName) => {
                    UpdateCachedFileName(newName);
                    filePathChangingCallback(oldName, newName);
                },
                Path.GetFileName(_fileInfo.CachedFilePath)
            );

            FilePath = new NotifyingProperty<string>(_fileInfo.OriginalFilePath);

            DocumentContent = new AvalonTextViewModel(_fileInfo.CachedFilePath);
            DocumentContent.Content.PropertyChanged += (s,e) => UpdateHash();
            DocumentContent.ApiProvider.Save = SaveChanges;

            _fileCheckTimer.Interval = TimeSpan.FromMilliseconds(150);
            _fileCheckTimer.Tick += (s, e) => UpdateIsDirty();
            _fileCheckTimer.Start();
            UpdateIsDirty();
        }

        public AvalonTextViewModel DocumentContent { get; }
        public NotifyingProperty<bool> IsDirty { get; }
        public NotifyingProperty<string> FilePath { get; }
        public NotifyingProperty<string> FileName { get; }
        
        public void Close() {
            _fileCheckTimer.Stop();
            IsDirty.Value = false;
            ArgsAndSettings.CachedFiles.Remove(_fileInfo);
            Directory.Delete(Path.GetDirectoryName(_fileInfo.CachedFilePath), true);
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

                if (_fileInfo.OriginalFilePath == null) {
                    var dialog = new SaveFileDialog();
                    dialog.FileName = FileName.Value;
                    dialog.DefaultExt = ".txt";
                    dialog.Filter = "All Types|*.*";
                    dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                    if (!dialog.ShowDialog() ?? false) return;
                    _fileInfo.OriginalFilePath = dialog.FileName;

                    FileName.Value = Path.GetFileName(dialog.FileName);
                    FilePath.Value = _fileInfo.OriginalFilePath;
                }
                
                File.Copy(_fileInfo.CachedFilePath, _fileInfo.OriginalFilePath, true);
                UpdateHash();
            }
        }

        private void UpdateCachedFile() {
            File.WriteAllText(_fileInfo.CachedFilePath, DocumentContent.Content.Value);
        }

        private void UpdateCachedFileName(string newName) {
            var newCachedPath = Path.Combine(Path.GetDirectoryName(_fileInfo.CachedFilePath), newName);
            File.Move(_fileInfo.CachedFilePath, newCachedPath);
            _fileInfo.CachedFilePath = newCachedPath;
            SaveFileInfo();
        }

        public override void Dispose() {
            base.Dispose();
            _fileCheckTimer.Stop();
            if (IsDirty.Value) UpdateCachedFile();
            DocumentContent.Dispose();
        }
    }
}
