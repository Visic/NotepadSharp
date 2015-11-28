using System.IO;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using WPFUtility;

namespace NotepadSharp {
    public abstract class FileSystemEntityViewModel : ViewModelBase {
        protected FileSystemEntityViewModel(string path) {
            IsExpanded = new NotifyingPropertyWithChangedAction<bool>(
                x => InteractCommand?.Execute(null)
            );

            SetPath(path);
        }

        public NotifyingProperty<string> Name { get; } = new NotifyingProperty<string>();
        public NotifyingProperty<BitmapImage> IconImage { get; } = new NotifyingProperty<BitmapImage>();
        public NotifyingProperty<bool> IsExpanded { get; }
        public ICommand InteractCommand { get; protected set; }
        public bool Focusable { get; protected set; } = true;

        protected virtual void SetPath(string path) {
            Name.Value = Path.GetFileName(path);
            if (string.IsNullOrEmpty(Name.Value) && !string.IsNullOrEmpty(path)) Name.Value = Path.GetPathRoot(path); //handle drive letters
        }
    }
}
