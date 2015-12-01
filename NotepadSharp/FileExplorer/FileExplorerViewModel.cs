using System;
using System.IO;
using System.Windows.Threading;
using WPFUtility;

namespace NotepadSharp {
    public class FileExplorerViewModel : DirectoryViewModel {
        DispatcherTimer _refreshTimer = new DispatcherTimer();

        public FileExplorerViewModel(string initialDirectory) : base(initialDirectory) {
            RootPath = new NotifyingPropertyWithChangedAction<string>(
                x => SetPath(x), 
                initialDirectory
            );
            DragDropRootPath = new DragAndDropHandler(AllowDropPath, DropPath);

            _refreshTimer.Interval = TimeSpan.FromMilliseconds(100);
            _refreshTimer.Tick += _refreshTimer_Tick;
            _refreshTimer.Start();
        }

        private async void _refreshTimer_Tick(object sender, EventArgs e) {
            _refreshTimer.Stop();
            await Refresh_Async();
            _refreshTimer.Start();
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
