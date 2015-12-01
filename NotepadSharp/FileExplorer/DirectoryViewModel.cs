using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WPFUtility;

namespace NotepadSharp {
    public class DirectoryViewModel : FileSystemEntityViewModel {
        bool _isOpen;
        bool _isUpdating;

        public DirectoryViewModel(string path) {
            Items.Value = new ObservableCollection<FileSystemEntityViewModel>(new FileSystemEntityViewModel[] { null });

            InteractCommand = new RelayCommand(async arg => {
                _isOpen = !_isOpen;
                await UpdateState_Async();
            });

            SetPath(path);
        }

        public NotifyingProperty<ObservableCollection<FileSystemEntityViewModel>> Items { get; } = new NotifyingProperty<ObservableCollection<FileSystemEntityViewModel>>();

        protected async override void SetPath(string path) {
            base.SetPath(path);
            await UpdateState_Async();
        }

        protected async Task Refresh_Async() {
            if(!_isOpen) return;
            await UpdateState_Async();
            foreach(var ele in Items.Value.Where(x => x is DirectoryViewModel).Cast<DirectoryViewModel>()) {
                await ele.Refresh_Async();
            }
        }

        private async Task UpdateState_Async() {
            if (_isUpdating) return;
            _isUpdating = true;
            IconImage.Value = _isOpen ? Constants.Image_FolderOpen : Constants.Image_FolderClosed;

            if(_isOpen) {
                try {
                    IEnumerable<string> maybeNewItems = new string[0];
                    if(string.IsNullOrWhiteSpace(EntityPath.Value)) {
                        maybeNewItems = Environment.GetLogicalDrives();
                    } else {
                        var directoryVms = Directory.GetDirectories(EntityPath.Value);
                        var fileVms = Directory.GetFiles(EntityPath.Value);
                        maybeNewItems = directoryVms.Concat(fileVms);
                    }
                    
                    var removed = Items.Value.Where(x => x == null || !maybeNewItems.Contains(x.EntityPath.Value));
                    var temp = removed.Count();
                    foreach(var ele in removed.ToArray()) {
                        Items.Value.Remove(ele);
                    }

                    var currentNames = Items.Value.Select(x => x == null ? null : x.EntityPath.Value);
                    var added = maybeNewItems.Except(currentNames).Select(x => MakeNewItem(x)).ToArray();
                    foreach(var ele in added) {
                        Items.Value.Add(ele);
                    }

                    //list updated, now slowly get all of the new files icons
                    foreach(var ele in added) {
                        await ((ele as FileViewModel)?.UpdateIcon_Async() ?? Task.CompletedTask);
                    }
                } catch(Exception ex) {
                    Items.Value.Clear();
                    Items.Value.Add(new ErrorItemViewModel(EntityPath.Value, ex.Message.Trim()));
                }
            }
            _isUpdating = false;
        }

        private FileSystemEntityViewModel MakeNewItem(string path) {
            return File.Exists(path) ? (FileSystemEntityViewModel)(new FileViewModel(path)) : new DirectoryViewModel(path);
        }
    }
}
