using System.Windows.Input;
using System.Windows.Media.Imaging;
using WPFUtility;

namespace NotepadSharp {
    public abstract class FileSystemEntityViewModel : ViewModelBase {
        protected FileSystemEntityViewModel(string path) {
            Path = path;
            Name = System.IO.Path.GetFileName(path);
            IsExpanded = new NotifyingPropertyWithChangedAction<bool>(
                x => InteractCommand?.Execute(null)
            );
        }

        public string Path { get; }
        public string Name { get; }
        public NotifyingProperty<BitmapImage> Icon { get; } = new NotifyingProperty<BitmapImage>();
        public ICommand InteractCommand { get; protected set; }
        public NotifyingProperty<bool> IsExpanded { get; }
    }
}
