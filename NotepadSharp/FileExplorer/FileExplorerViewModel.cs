using System;
using System.IO;
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
    }
}
