using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using WPFUtility;

namespace NotepadSharp {
    public class FileExplorerViewModel : DirectoryViewModel {
        public FileExplorerViewModel(string initialDirectory) : base(initialDirectory) {
            RootPath = new NotifyingPropertyWithChangedAction<string>(
                x => SetPath(x), 
                initialDirectory
            );
            DragDropRootPath = new DragAndDropHandler(AllowDropPath, DropPath);
        }

        public NotifyingProperty<string> RootPath { get; }
        public DragAndDropHandler DragDropRootPath { get; }

        private bool AllowDropPath(string path) {
            return Directory.Exists(path);
        }

        private void DropPath(string path) {
            RootPath.Value = path;
        }
    }
}
