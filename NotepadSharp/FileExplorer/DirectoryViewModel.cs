using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Utility;
using WPFUtility;

namespace NotepadSharp {
    public class DirectoryViewModel : FileSystemEntityViewModel {
        bool _isOpen;

        public DirectoryViewModel(string path) {
            SetPath(path);
            InteractCommand = new RelayCommand(arg => {
                _isOpen = !_isOpen;
                UpdateState();
            });
            DragDrop = new DragAndDropHandler(AllowDrop, Drop);
        }

        public NotifyingProperty<ObservableCollection<ViewModelBase>> Items { get; } = new NotifyingProperty<ObservableCollection<ViewModelBase>>();
        public DragAndDropHandler DragDrop { get; }

        protected override void SetPath(string path) {
            base.SetPath(path);
            UpdateState();
        }

        private bool AllowDrop(string path) {
            var newPath = Path.Combine(_path, Path.GetFileName(path));
            return path != _path && path != newPath;
        }

        private void Drop(string path) {
            var newPath = Path.Combine(_path, Path.GetFileName(path));

            if(File.Exists(path)) {
                if(File.Exists(newPath)) { //handle name conflictions
                    var currentFiles = Directory.EnumerateFiles(_path);
                    var currentName = Path.GetFileNameWithoutExtension(newPath);
                    var newName = UniqueNameGenerator.NextNumbered(currentName, currentFiles);
                    newPath = Path.Combine(_path, newName) + Path.GetExtension(path);
                }

                File.Move(path, newPath);
            } else if(Directory.Exists(path)) {
                if(Directory.Exists(newPath)) { //handle name conflictions
                    var currentDirectories = Directory.EnumerateDirectories(_path);
                    var currentName = Path.GetFileName(newPath);
                    var newName = UniqueNameGenerator.NextNumbered(currentName, currentDirectories);
                    newPath = Path.Combine(_path, newName);
                }

                Directory.Move(path, newPath);
            }
        }

        private void UpdateState() {
            IconImage.Value = _isOpen ? Constants.Image_FolderOpen : Constants.Image_FolderClosed;

            if(_isOpen) {
                try {
                    if (string.IsNullOrWhiteSpace(_path)) {
                        SetItems(Environment.GetLogicalDrives().Select(x => new DirectoryViewModel(x)).ToArray());
                    } else {
                        var directoryVms = Directory.GetDirectories(_path).Select(x => new DirectoryViewModel(x));
                        var fileVms = Directory.GetFiles(_path).Select(x => new FileViewModel(x));
                        SetItems(directoryVms.Cast<ViewModelBase>().Concat(fileVms).ToArray());
                    }
                } catch(Exception ex) {
                    SetItems(new ErrorItemViewModel(_path, ex.Message));
                }
            } else {
                SetItems(null);
            }
        }

        private void SetItems(params ViewModelBase[] vms) {
            Items.Value = new ObservableCollection<ViewModelBase>(vms ?? new ViewModelBase[] { null });
        }
    }
}
