using System;
using System.IO;
using System.Windows;
using WPFUtility;

namespace NotepadSharp {
    public class FileExplorerViewModel : DirectoryViewModel {
        public FileExplorerViewModel(string initialDirectory) : base(initialDirectory) {
            RootPath = new NotifyingPropertyWithChangedAction<string>(
                x => SetPath(x), 
                initialDirectory
            );
        }

        public NotifyingProperty<string> RootPath { get; }

        protected override bool AllowDrop(string path) {
            return !File.Exists(path);
        }

        protected override void Drop(DragEventArgs e) {
            RootPath.Value = GetDropPath(e);
        }
    }
}
