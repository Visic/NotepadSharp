using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Windows.Input;
using System.Windows.Threading;
using WPFUtility;
using System.Windows.Controls;

namespace NotepadSharp {
    public class FileExplorerViewModel : DirectoryViewModel {
        DispatcherTimer _refreshTimer = new DispatcherTimer();
        bool _updatePathBoxCaretIndex;

        public FileExplorerViewModel(string initialDirectory) : base(initialDirectory) {
            RootPath = new NotifyingPropertyWithChangedAction<string>(
                x => SetPath(x),
                initialDirectory
            );
            DragDropRootPath = new DragAndDropHandler(x => true, DropPath);

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
            PathBoxTextChangedCommand = new RelayCommand(
                x => {
                    var arg = (TextChangedEventArgs)x;
                    ((TextBox)arg.Source).CaretIndex = RootPath.Value.Length;
                }, 
                x => _updatePathBoxCaretIndex
            );
        }

        public KeyPressHandler KeyPressHandler { get; }
        public NotifyingProperty<string> RootPath { get; }
        public DragAndDropHandler DragDropRootPath { get; }
        public ICommand PathBoxGotFocusCommand { get; }
        public ICommand PathBoxLostFocusCommand { get; }
        public ICommand PathBoxTextChangedCommand { get; }
        public NotifyingProperty<bool> PathBoxHasFocus { get; } = new NotifyingProperty<bool>();
        public NotifyingProperty<FileSystemEntityViewModel> SelectedItem { get; } = new NotifyingProperty<FileSystemEntityViewModel>();

        private bool KeyPressed(IReadOnlyList<Key> arg) {
            if(arg.SequenceEqual(new[] { Key.Down }) && PathBoxHasFocus.Value) {
                PathBoxHasFocus.Value = false;
                SelectedItem.Value = Items.Value.FirstOrDefault();
                SelectedItem.Value.IsSelected.Value = true;
                return true;
            } else if(arg.SequenceEqual(new[] { Key.Enter }) && SelectedItem.Value != null) {
                SelectedItem.Value.IsExpanded.Value = !SelectedItem.Value.IsExpanded.Value;
                return true;
            } else if(arg.SequenceEqual(new[] { Key.LeftCtrl, Key.Enter })) {
                DropPath(SelectedItem.Value.EntityPath.Value);
                return true;
            }
            return false;
        }

        private async void _refreshTimer_Tick(object sender, EventArgs e) {
            _refreshTimer.Stop();
            await Refresh_Async();
            _refreshTimer.Start();
        }

        private void DropPath(string path) {
            _updatePathBoxCaretIndex = true;
            PathBoxHasFocus.Value = true;
            RootPath.Value = path;
            _updatePathBoxCaretIndex = false;
        }
    }
}
