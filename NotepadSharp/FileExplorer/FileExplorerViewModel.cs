using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Windows.Input;
using System.Windows.Threading;
using WPFUtility;
using System.Windows;

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

            InteractCommand = new ChainCommand(
                InteractCommand,
                x => {
                    if(IsExpanded.Value) _refreshTimer.Start();
                    else _refreshTimer.Stop();
                }
            );

            KeyPressHandler = new KeyPressHandler(KeyPressed);
            PathBoxGotFocusCommand = new RelayCommand(x => { PathBoxHasFocus.Value = true; });
            PathBoxLostFocusCommand = new RelayCommand(x => { PathBoxHasFocus.Value = false; });
        }

        public KeyPressHandler KeyPressHandler { get; }
        public NotifyingProperty<string> RootPath { get; }
        public DragAndDropHandler DragDropRootPath { get; }
        public ICommand PathBoxGotFocusCommand { get; }
        public ICommand PathBoxLostFocusCommand { get; }
        public NotifyingProperty<bool> PathBoxHasFocus { get; } = new NotifyingProperty<bool>();
        public NotifyingProperty<FileSystemEntityViewModel> SelectedItem { get; } = new NotifyingProperty<FileSystemEntityViewModel>();

        private bool KeyPressed(IReadOnlyList<Key> arg) {
            if(arg.Any(x => x == Key.Down) && PathBoxHasFocus.Value) {
                PathBoxHasFocus.Value = false;
                SelectedItem.Value = Items.Value.FirstOrDefault();
                SelectedItem.Value.IsSelected.Value = true;
                return true;
            } else if(arg.Any(x => x == Key.Enter) && SelectedItem.Value != null) {
                SelectedItem.Value.IsExpanded.Value = !SelectedItem.Value.IsExpanded.Value;
                return true;
            }
            return false;
        }

        private async void _refreshTimer_Tick(object sender, EventArgs e) {
            _refreshTimer.Stop();
            await Refresh_Async();
            _refreshTimer.Start();
        }

        private bool AllowDropPath(string path) {
            return Directory.Exists(path);
        }

        private void DropPath(string path) {
            PathBoxHasFocus.Value = true;
            RootPath.Value = path;
        }
    }
}
