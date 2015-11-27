using OSIcon;
using WPFUtility;

namespace NotepadSharp {
    public class FileViewModel : FileSystemEntityViewModel {
        public FileViewModel(string path) : base(path) {
            InteractCommand = new RelayCommand(x => ApplicationState.OpenDocument(path));

            try {
                Icon = IconReader.ExtractIconFromFileEx(path, IconSize.Small).ToBitmapImage();
            }
            catch {
                //Getting the icon throws for strange file names (e.g. a file name which contains a comma)
            }
        }
    }
}
