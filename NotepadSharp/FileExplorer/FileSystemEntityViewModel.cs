using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using WPFUtility;

namespace NotepadSharp {
    public abstract class FileSystemEntityViewModel : ViewModelBase {
        protected string _path;

        protected FileSystemEntityViewModel() {
            IsExpanded = new NotifyingPropertyWithChangedAction<bool>(
                x => InteractCommand?.Execute(null)
            );

            ErrorMessage = new NotifyingPropertyWithChangedAction<string>(
                x => Focusable.Value = string.IsNullOrEmpty(x)
            );

            DragItem = new DragAndDropHandler(
                () => string.IsNullOrEmpty(ErrorMessage.Value), 
                () => new DataObject(DataFormats.FileDrop, new string[] { _path })
            );
        }

        public NotifyingProperty<string> Name { get; } = new NotifyingProperty<string>();
        public NotifyingProperty<BitmapImage> IconImage { get; } = new NotifyingProperty<BitmapImage>();
        public NotifyingProperty<bool> Focusable { get; } = new NotifyingProperty<bool>(true);
        public NotifyingProperty<string> ErrorMessage { get; }
        public NotifyingProperty<bool> IsExpanded { get; }
        public DragAndDropHandler DragItem { get; }
        public ICommand InteractCommand { get; protected set; }

        protected virtual void SetPath(string path) {
            _path = path;

            try {
                Name.Value = Path.GetFileName(path);
                if(string.IsNullOrEmpty(Name.Value) && !string.IsNullOrEmpty(path)) Name.Value = Path.GetPathRoot(path); //handle drive letters
            } catch(Exception ex) {
                ErrorMessage.Value = ex.Message;
            }
        }
    }
}
