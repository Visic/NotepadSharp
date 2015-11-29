using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using WPFUtility;

namespace NotepadSharp {
    public class DirectoryViewModel : FileSystemEntityViewModel {
        bool _isOpen;

        public DirectoryViewModel(string path) {
            SetPath(path);
        }

        public NotifyingProperty<ObservableCollection<ViewModelBase>> Items { get; } = new NotifyingProperty<ObservableCollection<ViewModelBase>>();

        protected override void SetPath(string path) {
            base.SetPath(path);
            _path = path;

            InteractCommand = new RelayCommand(arg => {
                _isOpen = !_isOpen;
                UpdateState();
            });

            UpdateState();
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
