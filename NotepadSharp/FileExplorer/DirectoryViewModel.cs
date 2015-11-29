using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
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
            DropCommand = new RelayCommand(x => Drop((DragEventArgs)x));
            DragOverCommand = new RelayCommand(x => DragOver((DragEventArgs)x));
        }

        public NotifyingProperty<ObservableCollection<ViewModelBase>> Items { get; } = new NotifyingProperty<ObservableCollection<ViewModelBase>>();
        public ICommand DropCommand { get; }
        public ICommand DragOverCommand { get; }

        protected override void SetPath(string path) {
            base.SetPath(path);
            UpdateState();
        }

        protected virtual bool AllowDrop(string path) {
            return _path != path;
        }

        protected virtual void Drop(DragEventArgs e) {
            var path = GetDropPath(e);
            if (_path == path) return;

            var newPath = Path.Combine(_path, Path.GetFileName(path));
            if (File.Exists(path)) File.Move(path, newPath);
            else if (Directory.Exists(path)) Directory.Move(path, newPath);

            e.Handled = true;
        }

        protected string GetDropPath(DragEventArgs args) {
            var files = (string[])args.Data.GetData(DataFormats.FileDrop);
            return (files?.Length ?? 0) == 0 ? "" : files[0];
        }

        private void DragOver(DragEventArgs e) {
            var allow = AllowDrop(GetDropPath(e));
            e.Effects = allow ? DragDropEffects.Copy : DragDropEffects.None;
            e.Handled = allow;
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
