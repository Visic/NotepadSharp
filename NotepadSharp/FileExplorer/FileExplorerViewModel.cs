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

            DropCommand = new RelayCommand(x => Drop((DragEventArgs)x));
            DragOverCommand = new RelayCommand(x => DragOver((DragEventArgs)x));
        }

        public NotifyingProperty<string> RootPath { get; }
        public ICommand DropCommand { get; }
        public ICommand DragOverCommand { get; }

        private void DragOver(DragEventArgs e) {
            e.Effects = File.Exists(GetDropPath(e)) ? DragDropEffects.None : DragDropEffects.Copy;
            e.Handled = true;
        }

        private void Drop(DragEventArgs e) {
            RootPath.Value = GetDropPath(e);
        }

        private string GetDropPath(DragEventArgs args) {
            var files = (string[])args.Data.GetData(DataFormats.FileDrop);
            return (files?.Length ?? 0) == 0 ? "" : files[0];
        }
    }
}
