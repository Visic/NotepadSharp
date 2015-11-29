using System.Drawing;
using WPFUtility;

namespace NotepadSharp {
    public class FileViewModel : FileSystemEntityViewModel {
        public FileViewModel(string path) {
            InteractCommand = new RelayCommand(x => ApplicationState.OpenDocument(path));

            try {
                IconImage.Value = Icon.ExtractAssociatedIcon(path).ToBitmapImage();
            } catch {
                //Getting the icon throws for strange file names (e.g. a file name which contains a comma)
            }

            SetPath(path);
        }
    }
}
