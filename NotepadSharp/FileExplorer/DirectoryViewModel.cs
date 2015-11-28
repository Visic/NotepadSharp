using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using WPFUtility;

namespace NotepadSharp {
    public class DirectoryViewModel : FileSystemEntityViewModel {
        public DirectoryViewModel(string path) : base(path) {
            IconImage.Value = Constants.Image_FolderClosed;
            ClearCollection();

            bool isOpen = false;
            InteractCommand = new RelayCommand(arg => {
                isOpen = !isOpen;
                IconImage.Value = isOpen ? Constants.Image_FolderOpen : Constants.Image_FolderClosed;

                if(isOpen) {
                    var directoryVms = Directory.GetDirectories(path).Select(x => new DirectoryViewModel(x));
                    var fileVms = Directory.GetFiles(path).Select(x => new FileViewModel(x));
                    Items.Value = new ObservableCollection<ViewModelBase>(directoryVms.Cast<ViewModelBase>().Concat(fileVms));
                } else {
                    ClearCollection();
                }
            });
        }

        public NotifyingProperty<ObservableCollection<ViewModelBase>> Items { get; } = new NotifyingProperty<ObservableCollection<ViewModelBase>>();

        private void ClearCollection() {
            Items.Value = new ObservableCollection<ViewModelBase>(new ViewModelBase[] { null });
        }
    }
}
