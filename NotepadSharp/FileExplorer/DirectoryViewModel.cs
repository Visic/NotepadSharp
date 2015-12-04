using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WPFUtility;

namespace NotepadSharp {
    public class DirectoryViewModel : FileSystemEntityViewModel {
        bool _isUpdating;

        public DirectoryViewModel(string path) {
            Items.Value = new ObservableCollection<FileSystemEntityViewModel>(new FileSystemEntityViewModel[] { null });

            InteractCommand = new RelayCommand(async arg => {
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
            if(!IsExpanded.Value) return;
            await UpdateState_Async();
            foreach(var ele in Items.Value.Where(x => x is DirectoryViewModel).Cast<DirectoryViewModel>()) {
                await ele.Refresh_Async();
            }
        }

        private async Task UpdateState_Async() {
            if (_isUpdating) return;
            _isUpdating = true;
            IconImage.Value = IsExpanded.Value ? Constants.Image_FolderOpen : Constants.Image_FolderClosed;

            if(IsExpanded.Value) {
                try {
                    var maybeNewItems = GetListing(EntityPath.Value);
                    
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

        //This method returns a list of files/folders/drives using the path as an initial search and moving up the path structure
        //until a valid path is found, and then using the rest of the initial path as a filter for any files/folders in that result
        private IEnumerable<string> GetListing(string path, string filter = "") {
            IEnumerable<string> result;
            if(string.IsNullOrWhiteSpace(path)) {
                result = Environment.GetLogicalDrives();
            } else if (!Directory.Exists(path)) {
                result = GetListing(Path.GetDirectoryName(path), Path.GetFileName(path));
            } else {
                var directoryVms = Directory.GetDirectories(path);
                var fileVms = Directory.GetFiles(path);
                result = directoryVms.Concat(fileVms);
            }

            return result.Where(x => {
                var fileName = Path.GetFileName(x);
                if (string.IsNullOrEmpty(fileName)) fileName = x;
                return fileName.ToLower().StartsWith(filter.ToLower());
            });
        }

        private FileSystemEntityViewModel MakeNewItem(string path) {
            return File.Exists(path) ? (FileSystemEntityViewModel)(new FileViewModel(path)) : new DirectoryViewModel(path);
        }
    }
}
