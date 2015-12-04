using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using WPFUtility;

namespace NotepadSharp {
    public abstract class FileSystemEntityViewModel : ViewModelBase, ISelectableViewModel {
        protected FileSystemEntityViewModel() {
            IsExpanded = new NotifyingPropertyWithChangedAction<bool>(
                x => InteractCommand?.Execute(null)
            );

            ErrorMessage = new NotifyingPropertyWithChangedAction<string>(
                x => Focusable.Value = string.IsNullOrEmpty(x)
            );

            DragItem = new DragAndDropHandler(
                () => string.IsNullOrEmpty(ErrorMessage.Value), 
                () => new DataObject(DataFormats.FileDrop, new string[] { EntityPath.Value })
            );
        }

        public NotifyingProperty<string> EntityPath { get; } = new NotifyingProperty<string>();
        public NotifyingProperty<string> Name { get; } = new NotifyingProperty<string>();
        public NotifyingProperty<BitmapImage> IconImage { get; } = new NotifyingProperty<BitmapImage>();
        public NotifyingProperty<bool> Focusable { get; } = new NotifyingProperty<bool>(true);
        public NotifyingProperty<string> ErrorMessage { get; }
        public NotifyingProperty<bool> IsExpanded { get; }
        public NotifyingProperty<bool> IsSelected { get; } = new NotifyingProperty<bool>();
        public DragAndDropHandler DragItem { get; }
        public ICommand InteractCommand { get; protected set; }

        protected virtual void SetPath(string path) {
            EntityPath.Value = path;

            try {
                Name.Value = Path.GetFileName(path);
                if(string.IsNullOrEmpty(Name.Value) && !string.IsNullOrEmpty(path)) Name.Value = Path.GetPathRoot(path); //handle drive letters
            } catch(Exception ex) {
                ErrorMessage.Value = ex.Message;
            }
        }

        public override bool Equals(object obj) {
            var fse = obj as FileSystemEntityViewModel;
            if (fse == null) return false;

            return fse.EntityPath.Value == EntityPath.Value;
        }

        public override int GetHashCode() {
            return EntityPath.Value.GetHashCode();
        }

        public static bool operator ==(FileSystemEntityViewModel a, FileSystemEntityViewModel b) {
            return a?.Equals(b) ?? (object)b == null;
        }

        public static bool operator !=(FileSystemEntityViewModel left, FileSystemEntityViewModel right) {
            return !(left == right);
        }
    }
}
